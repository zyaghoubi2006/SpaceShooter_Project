using System;
using System.Drawing;
using SpaceShooter.Core;

namespace SpaceShooter.Entities.Enemies
{
    public class HeavyTankEnemy : Enemy
    {
        private float _shootTimer = 0f;
        private const float ShootInterval = 3f;
        private const float Speed = 30f;

        private AudioManager audioManager;

        public HeavyTankEnemy(float x, float y, AudioManager audioManager) : base(x, y, 50, 50)
        {
            Health = 200;
            MaxHealth = 200;
            ScoreValue = 100;
            CoinValue = 50;
            Color = Color.DarkGray;
            Velocity = new PointF(0, Speed);

            this.audioManager = audioManager;
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            _shootTimer += deltaTime;
            if (_shootTimer >= ShootInterval)
            {
                Shoot();
                _shootTimer = 0f;
            }

            if (Position.Y > 900)
            {
                IsActive = false;
            }
        }

        protected override void Shoot()
        {
            float bulletSpeed = 250f;
            audioManager.PlaySoundEffect(@"Resources\enemyshoot.wav");
            for (int i = 0; i < 8; i++)
            {
                float angle = i * 45f * (float)Math.PI / 180f;
                float vx = (float)Math.Cos(angle) * bulletSpeed;
                float vy = (float)Math.Sin(angle) * bulletSpeed;

                FireBullet(vx, vy);
            }
        }
    }
}
