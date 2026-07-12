using System;
using System.Drawing;
using SpaceShooter.Core;

namespace SpaceShooter.Entities.Enemies
{
    public class ScoutEnemy : Enemy
    {
        private float _time = 0f;
        private const float Speed = 200f;
        private const float Amplitude = 80f;
        private const float Frequency = 3f;
        private readonly float _centerX;

        protected override Image Sprite => GameAssets.ScoutEnemyImg;


        public ScoutEnemy(float x, float y) : base(x, y, 48, 48)
        {
            Health = 20;
            MaxHealth = 20;
            ScoreValue = 20;
            CoinValue = 8;
            Color = Color.Yellow;
            _centerX = x;
        }

        public override void Update(float deltaTime)
        {
            _time += deltaTime;

            Position = new PointF(
                Position.X,
                Position.Y + Speed * deltaTime
            );

            Position = new PointF(
                _centerX + (float)Math.Sin(_time * Frequency) * Amplitude,
                Position.Y
            );

            if (Position.Y > 900)
            {
                IsActive = false;
            }
        }
    }
}
