using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpaceShooter.Forms
{
    public partial class OptionForm : Form
    {

        private readonly AudioManager _audio;
        private bool _loading = false;

        public OptionForm(AudioManager audio)
        {
            this.BackgroundImage = Properties.Resources.shopbackground;
            this.BackgroundImageLayout = ImageLayout.Stretch;
            InitializeComponent();
            _audio = audio;
            BuildLayout();
            this.Load += OptionForm_Load;
        }

        private void BuildLayout()
        {
            // ---- تنظیمات فرم مثل ShopForm ----
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(15, 15, 20);
            this.ForeColor = Color.White;

            int boxWidth = 300;
            int centerX = (this.ClientSize.Width - boxWidth) / 2;

            // === پنل Audio Controls ===
            var grpAudio = new GroupBox
            {
                Text = "Audio Controls",
                ForeColor = Color.Cyan,
                BackColor = Color.FromArgb(25, 25, 32),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Location = new Point(centerX, 20),
                Size = new Size(boxWidth, 200)
            };
            this.Controls.Add(grpAudio);

            ReparentInto(grpAudio, label1, new Point(20, 40));
            ReparentInto(grpAudio, comboBox1, new Point(110, 35));
            ReparentInto(grpAudio, checkBox1, new Point(20, 80));
            ReparentInto(grpAudio, checkBox2, new Point(20, 115));

            label1.ForeColor = Color.White;
            label1.AutoSize = true;

            StyleDark(comboBox1);
            StyleDark(checkBox1);
            StyleDark(checkBox2);
            checkBox1.Text = "Mute Music";
            checkBox2.Text = "Mute SFX";

            comboBox1.BackColor = Color.FromArgb(35, 35, 45);
            comboBox1.ForeColor = Color.White;
            comboBox1.FlatStyle = FlatStyle.Flat;

            // === پنل Controls Guide ===
            var grpGuide = new GroupBox
            {
                Text = "Controls Guide",
                ForeColor = Color.Gold,
                BackColor = Color.FromArgb(25, 25, 32),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Location = new Point(centerX, 240),
                Size = new Size(boxWidth, 200)
            };
            this.Controls.Add(grpGuide);

            var lblGuide = new Label
            {
                AutoSize = false,
                Dock = DockStyle.Fill,
                Padding = new Padding(10),
                ForeColor = Color.White,
                Font = new Font("Consolas", 10F, FontStyle.Regular),
                TextAlign = ContentAlignment.TopLeft,
                Text =
                    "↑ ← ↓ →   Move ship\n\n" +
                    "Space     Shoot\n\n" +
                    "P         Pause\n\n" +
                    "Esc       Back to menu"
            };
            grpGuide.Controls.Add(lblGuide);

            // === دکمه Back پایین صفحه مثل ShopForm ===
            if (btnBack != null)
            {
                btnBack.Text = "Back to Menu";
                btnBack.Dock = DockStyle.Bottom;
                btnBack.Height = 50;
                btnBack.Font = new Font("Arial", 12, FontStyle.Bold);
                btnBack.BackColor = Color.FromArgb(35, 35, 45);
                btnBack.ForeColor = Color.White;
                btnBack.FlatStyle = FlatStyle.Flat;
                btnBack.FlatAppearance.BorderColor = Color.FromArgb(60, 60, 75);
            }
        }



        private void ReparentInto(Control parent, Control child, Point location)
        {
            if (child == null) return;
            child.Parent = parent;
            child.Location = location;
        }

        private void StyleDark(Control c)
        {
            c.BackColor = Color.FromArgb(25,25, 32);
            c.ForeColor = Color.White;
        }



        private void OptionForm_Load(object sender, EventArgs e)
        {
            _loading = true;

            comboBox1.Items.Clear();
            foreach (var track in AudioSettings.Tracks)
                comboBox1.Items.Add(track.Name);

            if (AudioSettings.SelectedTrackIndex >= 0 &
                AudioSettings.SelectedTrackIndex < comboBox1.Items.Count)
                    comboBox1.SelectedIndex = AudioSettings.SelectedTrackIndex;

            checkBox1.Checked = AudioSettings.MusicMuted;
            checkBox2.Checked = AudioSettings.SfxMuted;

            _loading = false;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            _audio.Dispose();
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
            _audio.Dispose();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (_loading) return;

            AudioSettings.MusicMuted = checkBox1.Checked;
            _audio?.SetMusicMuted(AudioSettings.MusicMuted);
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (_loading) return;

            AudioSettings.SfxMuted = checkBox2.Checked;
            _audio?.SetSfxMuted(AudioSettings.SfxMuted);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_loading) return;

            int idx = comboBox1.SelectedIndex;
            if (idx < 0 || idx >= AudioSettings.Tracks.Count) return;

            AudioSettings.SelectedTrackIndex = idx;
            string fullPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                AudioSettings.Tracks[idx].Path);
            _audio?.SwitchTrack(fullPath);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
