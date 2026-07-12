using System.Drawing;
using SpaceShooter.Core;

namespace SpaceShooter.Entities.Enemies
{
    public class StandardEnemy : Enemy
    {
        private const float Speed = 150f;
        protected override Image Sprite => GameAssets.StandardEnemyImg;


        public StandardEnemy(float x, float y) : base(x, y, 60, 60)
        {
            Health = 30;
            MaxHealth = 30;
            ScoreValue = 10;
            CoinValue = 5;
            Color = Color.Red;
            Velocity = new System.Drawing.PointF(0, Speed);
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            if (Position.Y > 900)
            {
                IsActive = false;
            }
        }
    }
}
