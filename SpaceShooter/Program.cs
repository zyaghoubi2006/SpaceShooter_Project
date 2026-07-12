using SpaceShooter.Forms;
using SpaceShooter.Managers;
using System;
using System.Windows.Forms;

namespace SpaceShooter
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            DatabaseManager.Initialize();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            AudioManager audioManager = new AudioManager();
            Application.Run(new MainMenuForm(audioManager));
            audioManager.Dispose();
        }
    }
}
