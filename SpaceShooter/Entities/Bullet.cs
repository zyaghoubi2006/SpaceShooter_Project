using System.Drawing;
using System.Windows.Forms;
using SpaceShooter.Core;

namespace SpaceShooter.Entities
{
    public class Bullet : GameObject
    {
        public int Damage { get; private set; }
        public bool IsPlayerBullet { get; private set; }

        private const float Speed = 500f;

        public Bullet(float x, float y, float directionX, float directionY, bool isPlayerBullet)
            : base(x, y, 15, 15)
        {
            Position = new PointF(x - Size.Width / 2f, y);

            float length = (float)System.Math.Sqrt(directionX * directionX + directionY * directionY);
            if (length > 0)
            {
                directionX /= length;
                directionY /= length;
            }

            Velocity = new PointF(directionX * Speed, directionY * Speed);
            Damage = 10;
            IsPlayerBullet = isPlayerBullet;
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            if (Position.X < 0 || Position.X > Screen.PrimaryScreen.Bounds.Width ||
                Position.Y < 0 || Position.Y > Screen.PrimaryScreen.Bounds.Height)
            {
                IsActive = false;
            }
        }

        public override void Draw(Graphics g)
        {
            Image Img = IsPlayerBullet ? GameAssets.BulletImg : GameAssets.EnemyBulletImg;
            g.DrawImage(Img, Position.X, Position.Y, Size.Width, Size.Height);
        }
    }
}
