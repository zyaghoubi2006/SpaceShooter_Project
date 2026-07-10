using SpaceShooter.Managers.Data;
using System;
using System.Drawing;
using System.Media;
using System.Windows.Forms;

namespace SpaceShooter.Forms
{
    public partial class MainMenuForm : Form
    {
        private SoundPlayer ClickPlayer;
        private AudioManager audioManager;

        private Label lblCoins;
        private Label lblHighScore;

        public MainMenuForm(AudioManager audioManager)
        {
            this.BackgroundImage = Properties.Resources.Backgroundmainmenu;
            this.BackgroundImageLayout = ImageLayout.Stretch;
            InitializeComponent();
            InitializeCustomControls();
            ApplyStyle();
            this.audioManager = audioManager;
            ClickPlayer = new SoundPlayer(Properties.Resources.resources_audio_menu_click);
            LoadPlayerStats();
        }

        private void InitializeCustomControls()
        {

            lblCoins = new Label
            {
                Location = new Point(20, 100),
                AutoSize = true,
                Font = new Font("Courier New", 12, FontStyle.Bold),
                ForeColor = Color.Gold,
                BackColor = Color.Transparent,
                AccessibleName = "Coins"
            };

            lblHighScore = new Label
            {
                Location = new Point(20, 120),
                AutoSize = true,
                Font = new Font("Courier New", 12, FontStyle.Bold),
                ForeColor = Color.Cyan,
                BackColor = Color.Transparent,
                AccessibleName = "High Score"
            };

            this.Controls.Add(lblCoins);
            this.Controls.Add(lblHighScore);
        }

        private void ApplyStyle()
        {
            this.Size = new Size(700, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = SystemColors.Control;

            Button[] buttons = { btn_Play, btn_Shop, btn_Option, btn_About, btn_Quit };
            foreach (var btn in buttons)
            {
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.BackColor = Color.FromArgb(120, 0, 0, 0);
                btn.ForeColor = Color.White;
                btn.Font = new Font("Arial", 12, FontStyle.Bold);
            }
        }

        private void LoadPlayerStats()
        {
            var playerData = PlayerRepository.GetPlayerData();
            if (playerData != null)
            {
                lblCoins.Text = $"Total Coins : {playerData.TotalCoins}";
                lblHighScore.Text = $"Highest Score : {playerData.HighScore}";
            }
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (this.Visible)
                LoadPlayerStats();
        }

        private void PlayClickSound() => ClickPlayer.Play();
        private void PlayClickSoundSync() => ClickPlayer.PlaySync();

        private void btn_Play_Click(object sender, EventArgs e)
        {
            PlayClickSound();
            this.Hide();
            new GameForm().ShowDialog();
            this.Show();
        }

        private void btn_Shop_Click(object sender, EventArgs e)
        {
            PlayClickSound();
            this.Hide();
            new ShopForm().ShowDialog();
            this.Show();
        }

        private void btn_Option_Click(object sender, EventArgs e)
        {
            PlayClickSound();
            this.Hide();
            new OptionForm(audioManager).ShowDialog();
            this.Show();
        }

        private void btn_About_Click(object sender, EventArgs e)
        {
            PlayClickSound();
            this.Hide();
            new AboutForm().ShowDialog();
            this.Show();
        }

        private void btn_Quit_Click(object sender, EventArgs e)
        {
            PlayClickSoundSync();
            Application.Exit();
        }
    }
}
