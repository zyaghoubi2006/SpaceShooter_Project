using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SpaceShooter.Entities;

namespace SpaceShooter.Forms
{
    public partial class GameForm : Form
    {
        private Timer GameTimer;

        private Player player;

        private DateTime lastTime;
        private bool IsMovingLeft;
        private bool IsMovingRight;
        private bool IsMovingUp;
        private bool IsMovingDown;

        private DateTime lastShootTime = DateTime.MinValue;

        public GameForm()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            InitializeGame();
        }

        private void InitializeGame()
        {
            lastTime = DateTime.Now;
            player = new Player(180, 400, 50, 50, 8);
            GameTimer = new Timer();
            GameTimer.Interval = 20;
            GameTimer.Tick += Gameloop;
            GameTimer.Start();
           
        }

        private void Gameloop(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            float deltaTime = (float)(now - lastTime).TotalMilliseconds;
            lastTime = now;
            UpdatePlayerMovement(deltaTime);
            
            Invalidate();
        }

        private void UpdatePlayerMovement(float deltaTime)
        {
            
            if (IsMovingLeft && player.X > 0)
                player.MoveLeft();

            if (IsMovingRight && player.X + player.Width < this.ClientSize.Width)
                player.MoveRight();

            if (IsMovingUp && player.Y > 0)
                player.MoveUp();

            if (IsMovingDown && player.Y + player.Height < this.ClientSize.Height)
                player.MoveDown();
        }

        private void GameForm_KeDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close(); 
            }
            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.A)
                IsMovingLeft = true;

            if (e.KeyCode == Keys.Right || e.KeyCode == Keys.D)
                IsMovingRight = true;

            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.W)
                IsMovingUp = true;

            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.S)
                IsMovingDown = true;

        }

        private void GameForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.A)
                IsMovingLeft = false;

            if (e.KeyCode == Keys.Right || e.KeyCode == Keys.D)
                IsMovingRight = false;

            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.W)
                IsMovingUp = false;

            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.S)
                IsMovingDown = false;

        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            player.Draw(e.Graphics);

            
        }
    }
}
