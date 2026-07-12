using SpaceShooter.Core;
using SpaceShooter.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SpaceShooter.Entities
{
    public class Player : GameObject
    {
        private const float Speed = 300f;
        private const float Friction = 0.7f;
        private const float ShootCooldown = 0.2f;

        private float shootTimer = 0f;

        public int Health { get; set; }
        public int MaxHealth { get; set; } = 100;
        public int ExtraLives { get; set; } = 0;
        public int Score { get; set; }
        public int Coins { get; set; }
        public Bullet LastBullet { get; private set; }

        public Player(float x, float y) : base(x, y, 65, 65)
        {
            Health = 100;
            Score = 0;
            Coins = 0;
        }

        public void AddExtraLife()
        {
            ExtraLives++;
            MaxHealth += 50;
            Health = MaxHealth;
        }

        public void HandleInput(HashSet<Keys> pressedKeys, float deltaTime)
        {
            PointF inputDirection = new PointF(0, 0);

            if (pressedKeys.Contains(Keys.W) || pressedKeys.Contains(Keys.Up))
                inputDirection.Y -= 1;
            if (pressedKeys.Contains(Keys.S) || pressedKeys.Contains(Keys.Down))
                inputDirection.Y += 1;
            if (pressedKeys.Contains(Keys.A) || pressedKeys.Contains(Keys.Left))
                inputDirection.X -= 1;
            if (pressedKeys.Contains(Keys.D) || pressedKeys.Contains(Keys.Right))
                inputDirection.X += 1;

            float length = (float)Math.Sqrt(inputDirection.X * inputDirection.X + inputDirection.Y * inputDirection.Y);
            if (length > 0)
            {
                inputDirection.X /= length;
                inputDirection.Y /= length;

                Velocity = new PointF(inputDirection.X * Speed, inputDirection.Y * Speed);
            }
            else
            {
                Velocity = new PointF(Velocity.X * Friction, Velocity.Y * Friction);
            }
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            if (Position.X < 0) Position = new PointF(0, Position.Y);
            if (Position.X + Size.Width > Screen.PrimaryScreen.Bounds.Width)
                Position = new PointF(Screen.PrimaryScreen.Bounds.Width - Size.Width, Position.Y);
            if (Position.Y < 0) Position = new PointF(Position.X, 0);
            if (Position.Y + Size.Height > Screen.PrimaryScreen.Bounds.Height)
                Position = new PointF(Position.X, Screen.PrimaryScreen.Bounds.Height - Size.Height);

            if (shootTimer > 0)
                shootTimer -= deltaTime;
        }

        public void Shoot()
        {
            if (shootTimer <= 0)
            {
                float bulletX = Position.X + Size.Width / 2 ;
                float bulletY = Position.Y;
                LastBullet = new Bullet(bulletX, bulletY, 0, -1, true);
                shootTimer = ShootCooldown;
            }
            else
            {
                LastBullet = null;
            }
        }

        public override void Draw(Graphics g)
        {
            g.DrawImage(GameAssets.PlayerImg, GetBounds());
        }

        public void TakeDamage(int damage)
        {
            Health -= damage;
            if (Health < 0) Health = 0;
        }
    }
}
