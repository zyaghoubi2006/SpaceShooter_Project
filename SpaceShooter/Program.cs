using System;
using System.Windows.Forms;
using SpaceShooter.Forms;

namespace SpaceShooter
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            AudioManager audioManager = new AudioManager();
            Application.Run(new MainMenuForm(audioManager));
            audioManager.Dispose();
        }
    }
}
