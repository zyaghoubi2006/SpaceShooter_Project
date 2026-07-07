using SpaceShooter.Core;
using System;
using System.Drawing;
using System.Security.Policy;

namespace SpaceShooter.Entities
{
    public abstract class Enemy : GameObject
    {
        public int Health { get; set; }
        public int MaxHealth { get; set; }
        public int ScoreValue { get; set; }
        public int CoinValue { get; set; }
        public Color Color { get; set; }

        public event Action<float, float, float, float> OnShoot;

        protected abstract Image Sprite { get; }

        protected Enemy(float x, float y, float width, float height) : base(x, y, width, height)
        {
        }

        public void TakeDamage(int damage)
        {
            Health -= damage;
            if (Health <= 0)
            {
                Health = 0;
                IsActive = false;
            }
        }

        protected virtual void Shoot() { }

        protected void FireBullet(float vx, float vy)
        {
            OnShoot?.Invoke(Position.X + Size.Width / 2, Position.Y + Size.Height, vx, vy);
        }

        public override void Draw(Graphics g)
        {
            if (!IsActive) return;

            g.DrawImage(Sprite, GetBounds());

            float healthBarWidth = Size.Width;
            float healthBarHeight = 4;
            float healthPercent = (float)Health / MaxHealth;

            g.FillRectangle(Brushes.Red, Position.X, Position.Y - 8, healthBarWidth, healthBarHeight);
            g.FillRectangle(Brushes.Green, Position.X, Position.Y - 8, healthBarWidth * healthPercent, healthBarHeight);
        }
    }
}
