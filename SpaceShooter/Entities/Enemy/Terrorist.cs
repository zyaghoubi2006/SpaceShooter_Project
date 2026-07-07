using System;
using System.Drawing;
using SpaceShooter.Core;

namespace SpaceShooter.Entities.Enemies
{
    public class TerroristEnemy : Enemy
    {
        private PointF _targetPosition;
        private const float Speed = 150f;

        protected override Image Sprite => GameAssets.TerroristEnemyImg;


        public TerroristEnemy(float x, float y) : base(x, y, 70, 70)
        {
            Health = 80;
            MaxHealth = 80;
            ScoreValue = 50;
            CoinValue = 30;
            Color = Color.OrangeRed;
        }

        public void SetTarget(float playerX, float playerY)
        {
            _targetPosition = new PointF(playerX, playerY);
        }

        public override void Update(float deltaTime)
        {
            float dx = _targetPosition.X - Position.X;
            float dy = _targetPosition.Y - Position.Y;
            float distance = (float)Math.Sqrt(dx * dx + dy * dy);

            if (distance > 5f)
            {
                Velocity = new PointF(
                    (dx / distance) * Speed,
                    (dy / distance) * Speed
                );
            }
            else
            {
                Velocity = new PointF(0, 0);
            }

            base.Update(deltaTime);

            if (Position.Y > 650 || Position.Y < -50)
            {
                IsActive = false;
            }
        }

        public bool HasReachedTarget()
        {
            float dx = _targetPosition.X - Position.X;
            float dy = _targetPosition.Y - Position.Y;
            float distance = (float)Math.Sqrt(dx * dx + dy * dy);

            return distance < 40f;
        }
    }
}
