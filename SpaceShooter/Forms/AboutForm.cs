using System;
using System.Drawing;
using System.Media;
using System.Windows.Forms;

namespace SpaceShooter.Forms
{
    public partial class AboutForm : Form
    {
        private static readonly Color PanelBackColor = Color.FromArgb(180, 10, 20);
        private static readonly Color CardBackColor = Color.FromArgb(200, 20, 35);
        private static readonly Color ButonColor = Color.FromArgb(200, 30, 30, 50);
        private static readonly Color ButtonHover = Color.FromArgb(220, 50, 50, 80);

        public AboutForm()
        {
            InitializeComponent();
            ApplyStyle();
        }

        private void ApplyStyle()
        {
            this.Text = "About";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.Black;

            this.BackgroundImage = Properties.Resources.shopbackground;
            this.BackgroundImageLayout = ImageLayout.Stretch;

            Label lblTitle = new Label
            {
                Text = "ABOUT",
                Font = new Font("Courier New", 28, FontStyle.Bold),
                ForeColor = Color.Cyan,
                BackColor = Color.Transparent,
                AutoSize = false,
                Size = new Size(760, 60),
                Location = new Point(20, 30),
                TextAlign = ContentAlignment.MiddleCenter
            };

            Panel card = new Panel
            {
                Size = new Size(600, 340),
                Location = new Point(90, 110),
                BackColor = ButonColor
            };

            Label lblStudents = new Label
            {
                Text =
                    "Developed by:\n\n" +
                    "Name:  Setayesh Jahedi\n" +
                "Student Code:  403411474 \n\n" +
                    "Name:  Zeinab Yaghoubi\n" +
                    "Student Code:  403462073",
                Font = new Font("Courier New", 13, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                AutoSize = false,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };

            card.Controls.Add(lblStudents);

            Button backBtn = new Button
            {
                Text = "Back to Menu",
                Size = new Size(200, 45),
                Location = new Point(290, 480),
                Font = new Font("Arial", 12, FontStyle.Bold),
                BackColor = ButonColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            backBtn.FlatAppearance.BorderSize = 0;
            backBtn.FlatAppearance.MouseOverBackColor = ButtonHover;
            backBtn.Click += btnBack_Click;

            this.Controls.Add(lblTitle);
            this.Controls.Add(card);
            this.Controls.Add(backBtn);
        }

        private void AboutForm_Load(object sender, EventArgs e)
        {
        }

        private void PlayClickSound()
        {
            SoundPlayer player = new SoundPlayer(Properties.Resources.resources_audio_menu_click);
            player.Play();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            PlayClickSound();
            this.Close();
        }
    }
}
