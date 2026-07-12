using System.Drawing;

namespace SpaceShooter.Core
{
    public abstract class GameObject
    {
        public PointF Position { get; set; }
        public PointF Velocity { get; set; }
        public SizeF Size { get; set; }
        public bool IsActive { get; set; }

        protected GameObject(float x, float y, float width, float height)
        {
            Position = new PointF(x, y);
            Velocity = new PointF(0, 0);
            Size = new SizeF(width, height);
            IsActive = true;
        }

        public virtual void Update(float deltaTime)
        {
            Position = new PointF(
                Position.X + Velocity.X * deltaTime,
                Position.Y + Velocity.Y * deltaTime
            );
        }

        public virtual void Draw(Graphics g)
        {
        }

        public RectangleF GetBounds()
        {
            return new RectangleF(Position.X, Position.Y, Size.Width, Size.Height);
        }

        public bool CollidesWith(GameObject other)
        {
            return GetBounds().IntersectsWith(other.GetBounds());
        }
    }
}
