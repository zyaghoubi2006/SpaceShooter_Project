using NAudio.Wave;
using SpaceShooter.Core;
using SpaceShooter.Entities;
using SpaceShooter.Managers.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Media;
using System.Windows.Forms;

namespace SpaceShooter.Forms
{
    public partial class GameForm : Form
    {
        private GameEngine gameEngine;
        private Timer gameTimer;
        private DateTime lastUpdateTime;
        private HashSet<Keys> pressedKeys;
        private DateTime lastShootTime;
        private const float ShootCooldown = 0.2f;

        private AudioManager audioManager;

        public GameForm()
        {
            InitializeComponent();
            InitializeGame();
        }

        private void InitializeGame()
        {
            audioManager = new AudioManager();
            var track = AudioSettings.Tracks[AudioSettings.SelectedTrackIndex];
            string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, track.Path);

            audioManager.SetMusicMuted(AudioSettings.MusicMuted);
            audioManager.SetSfxMuted(AudioSettings.SfxMuted);
            audioManager.SetMusicVolume(AudioSettings.MusicVolume);
            audioManager.SetSfxVolume(AudioSettings.SfxVolume);
            audioManager.PlayBackgroundMusic(fullPath);


            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.DoubleBuffered = true;
            this.BackColor = Color.Black;

            gameEngine = new GameEngine(this.ClientSize.Width, this.ClientSize.Height, audioManager);


            pressedKeys = new HashSet<Keys>();
            lastUpdateTime = DateTime.Now;
            lastShootTime = DateTime.Now;

            gameTimer = new Timer();
            gameTimer.Interval = 16; 
            gameTimer.Tick += GameTimer_Tick;
            gameTimer.Start();

            this.KeyDown += GameForm_KeyDown;
            this.KeyUp += GameForm_KeyUp;
            this.Paint += GameForm_Paint;
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            DateTime currentTime = DateTime.Now;
            float deltaTime = (float)(currentTime - lastUpdateTime).TotalSeconds;
            lastUpdateTime = currentTime;

            if (pressedKeys.Contains(Keys.Space))
            {
                TimeSpan timeSinceLastShoot = currentTime - lastShootTime;
                if (timeSinceLastShoot.TotalSeconds >= ShootCooldown)
                {
                    audioManager.PlaySoundEffect(@"Resources\shooting.wav");

                    gameEngine.Player.Shoot();
                    if (gameEngine.Player.LastBullet != null)
                    {
                        gameEngine.Bullets.Add(gameEngine.Player.LastBullet);
                    }
                    lastShootTime = currentTime;
                }
            }

            gameEngine.Update(deltaTime);

            if (gameEngine.IsGameComplete)
            {
                GameComplete();
                return;
            }

            HandleInput(deltaTime);

            if (gameEngine.Player.Health <= 0)
            {
                GameOver();
                audioManager.StopBackgroundMusic();
                
            }

            this.Invalidate();
        }

        private void GameComplete()
        {
            gameTimer.Stop();
            audioManager.StopBackgroundMusic();
            audioManager.PlaySoundEffect(@"Resources\Winning.wav");

            int currentScore = gameEngine.Player.Score;
            int highScore = PlayerRepository.GetHighScore();

            bool isNewRecord = currentScore > highScore;

            if (isNewRecord)
            {
                PlayerRepository.SaveHighScore(currentScore);
            }

            MessageBox.Show(
                $"CONGRATS! YOU WON!\n\nFinal score: {gameEngine.Player.Score}\nCoins gained: {gameEngine.Player.Coins}",
                "End Game",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );

            ShopRepository.ResetConsumableStatus();

            this.Close();
        }



        private void HandleInput(float deltaTime)
        {
            float moveSpeed = 300f * deltaTime;

            if (pressedKeys.Contains(Keys.Left) || pressedKeys.Contains(Keys.A))
            {
                gameEngine.Player.Position = new System.Drawing.PointF(
                    Math.Max(0, gameEngine.Player.Position.X - moveSpeed),
                    gameEngine.Player.Position.Y
                );
            }

            if (pressedKeys.Contains(Keys.Right) || pressedKeys.Contains(Keys.D))
            {
                gameEngine.Player.Position = new System.Drawing.PointF(
                    Math.Min(this.ClientSize.Width - gameEngine.Player.Size.Width,
                             gameEngine.Player.Position.X + moveSpeed),
                    gameEngine.Player.Position.Y
                );
            }

            if (pressedKeys.Contains(Keys.Up) || pressedKeys.Contains(Keys.W))
            {
                gameEngine.Player.Position = new System.Drawing.PointF(
                    gameEngine.Player.Position.X,
                    Math.Max(0, gameEngine.Player.Position.Y - moveSpeed)
                );
            }

            if (pressedKeys.Contains(Keys.Down) || pressedKeys.Contains(Keys.S))
            {
                gameEngine.Player.Position = new System.Drawing.PointF(
                    gameEngine.Player.Position.X,
                    Math.Min(this.ClientSize.Height - gameEngine.Player.Size.Height,
                             gameEngine.Player.Position.Y + moveSpeed)
                );
            }
        }

        private void GameForm_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            if (GameAssets.BackgroundImg != null)
            {
                g.DrawImage(GameAssets.BackgroundImg, 0, 0, ClientSize.Width, ClientSize.Height);
            }

            gameEngine.Draw(g);
            DrawHUD(g);
        }

        private void DrawHUD(Graphics g)
        {
            using (Font font = new Font("Arial", 16, FontStyle.Bold))
            {
                g.DrawString($"Health: {gameEngine.Player.Health}", font, Brushes.White, 10, 10);
                g.FillRectangle(Brushes.Red, 10, 40, gameEngine.Player.Health * 2, 20);
                g.DrawRectangle(Pens.White, 10, 40, gameEngine.Player.Health * 2, 20);

                g.DrawString($"Score: {gameEngine.Player.Score}", font, Brushes.White, 10, 70);
                g.DrawString($"Coins: {gameEngine.Player.Coins}", font, Brushes.Gold, 10, 100);

                g.DrawString($"Wave: {gameEngine.CurrentWave}", font, Brushes.Cyan, 10, 130);

            }
        }

        private void GameForm_KeyDown(object sender, KeyEventArgs e)
        {
            pressedKeys.Add(e.KeyCode);

            if (e.KeyCode == Keys.Escape)
            {
                PauseGame();
            }
        }

        private void GameForm_KeyUp(object sender, KeyEventArgs e)
        {
            pressedKeys.Remove(e.KeyCode);
        }

        private void PauseGame()
        {
            gameTimer.Stop();
            DialogResult result = MessageBox.Show(
                "Game Paused\nReturn to main menu?",
                "Pause",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            int currentScore = gameEngine.Player.Score;
            int highScore = PlayerRepository.GetHighScore();

            bool isNewRecord = currentScore > highScore;

            if (isNewRecord)
            {
                PlayerRepository.SaveHighScore(currentScore);
            }

            if (result == DialogResult.Yes)
            {
                this.Close();
            }
            else
            {
                lastUpdateTime = DateTime.Now;
                gameTimer.Start();
            }
            ShopRepository.ResetConsumableStatus();

        }

        private void GameOver()
        {
            gameTimer.Stop();
            audioManager.StopBackgroundMusic();
            audioManager.PlaySoundEffect(@"Resources\gameover.wav");

            int currentScore = gameEngine.Player.Score;
            int currentCoins = gameEngine.Player.Coins;
            int highScore = PlayerRepository.GetHighScore();

            bool isNewRecord = currentScore > highScore;

            string message = $"Game Over!\nScore: {currentScore}\nCoins: {currentCoins}\n";

            if (isNewRecord)
            {
                message += $"\n🎉 NEW HIGH SCORE! 🎉\nPrevious: {highScore}";
                PlayerRepository.SaveHighScore(currentScore);
            }
            else
            {
                message += $"\nHigh Score: {highScore}";
            }

            MessageBox.Show(
                message,
                isNewRecord ? "New Record!" : "Game Over",
                MessageBoxButtons.OK,
                isNewRecord ? MessageBoxIcon.Exclamation : MessageBoxIcon.Information
            );

            ShopRepository.ResetConsumableStatus();

            this.Close();
        }


        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            gameTimer?.Stop();
            gameTimer?.Dispose();
            base.OnFormClosing(e);
            audioManager.StopBackgroundMusic();
        }
    }

}