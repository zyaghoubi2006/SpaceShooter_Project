using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Media;

namespace SpaceShooter.Forms
{
    public partial class MainMenuForm : Form


    {
        private SoundPlayer ClickPlayer;
        private AudioManager audioManager;

        public MainMenuForm(AudioManager audioManager)
        {
            InitializeComponent();
            this.audioManager = audioManager;
            ClickPlayer = new SoundPlayer(Properties.Resources.resources_audio_menu_click);
        }

        private void PlayClickSound()
        {
            ClickPlayer.Play();
        }

        private void PlayClickSoundSync()
        {
            ClickPlayer.PlaySync();
        }

        private void btn_Play_Click(object sender, EventArgs e)
        {
            PlayClickSound();

            this.Hide();
            GameForm gameform = new GameForm();
            gameform.ShowDialog();

            this.Show();
        }

        private void btn_Shop_Click(object sender, EventArgs e)
        {
            PlayClickSound();
            this.Hide();
            ShopForm shopform = new ShopForm();
            shopform.ShowDialog();
            this.Show();
        }

        private void btn_Option_Click(object sender, EventArgs e)
        {
            PlayClickSound();
            this.Hide();
            OptionForm optionform = new OptionForm(audioManager);
            optionform.ShowDialog();
            this.Show();
        }

        private void btn_About_Click(object sender, EventArgs e)
        {
            PlayClickSound();
            this.Hide();
            AboutForm aboutform = new AboutForm();
            aboutform.ShowDialog();
            this.Show();
        }

        private void btn_Quit_Click(object sender, EventArgs e)
        {
            PlayClickSoundSync();
            Application.Exit();
        }
    }
}
