using SpaceShooter.Entities;
using System;
using System.Drawing;

namespace SpaceShooter.Entities
{
    public enum CoinType
    {
        Silver,
        Gold
    }

    public class Coin
    {
        public PointF Position { get; set; }
        public PointF Velocity { get; set; }
        public CoinType Type { get; private set; }
        public int Value { get; private set; }
        public bool IsActive { get; set; }
        public SizeF Size { get; private set; }

        private float pulseTimer;
        private float rotationAngle;
        private const float RotationSpeed = 180f;

        public Coin(float x, float y, CoinType type)
        {
            Position = new PointF(x, y);
            Type = type;
            Value = type == CoinType.Gold ? 5 : 1;
            IsActive = true;
            Size = new SizeF(20, 20);
            pulseTimer = 0;
            rotationAngle = 0;

            Random random = new Random();
            float vx = (float)(random.NextDouble() * 100 - 50);
            float vy = (float)(random.NextDouble() * 50 + 100);
            Velocity = new PointF(vx, vy);
        }

        public void Update(float deltaTime)
        {
            Velocity = new PointF(
                Velocity.X * 0.98f,
                Velocity.Y + 200 * deltaTime
            );

            Position = new PointF(
                Position.X + Velocity.X * deltaTime,
                Position.Y + Velocity.Y * deltaTime
            );

            pulseTimer += deltaTime * 3;
            rotationAngle += RotationSpeed * deltaTime;
            if (rotationAngle >= 360f)
                rotationAngle -= 360f;

            if (Position.Y > System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height + 50)
            {
                IsActive = false;
            }
        }

        public bool CheckCollision(Player player)
        {
            float dx = Position.X - (player.Position.X + player.Size.Width / 2);
            float dy = Position.Y - (player.Position.Y + player.Size.Height / 2);
            float distance = (float)Math.Sqrt(dx * dx + dy * dy);

            if (distance < (Size.Width / 2 + player.Size.Width / 2))
            {
                IsActive = false;
                return true;
            }
            return false;
        }

        public void Draw(Graphics g)
        {
            float pulse = 1f + (float)Math.Sin(pulseTimer) * 0.15f;
            float drawHeight = Size.Height * pulse;

            float radians = rotationAngle * (float)Math.PI / 180f;
            float drawWidth = Size.Width * pulse * Math.Abs((float)Math.Cos(radians));

            Color coinColor = Type == CoinType.Gold ? Color.Gold : Color.Silver;
            using (SolidBrush brush = new SolidBrush(coinColor))
            {
                g.FillEllipse(brush,
                    Position.X - drawWidth / 2,
                    Position.Y - drawHeight / 2,
                    drawWidth,
                    drawHeight);
            }

            using (Pen pen = new Pen(Color.FromArgb(200, 255, 255, 255), 2))
            {
                g.DrawEllipse(pen,
                    Position.X - drawWidth / 2,
                    Position.Y - drawHeight / 2,
                    drawWidth,
                    drawHeight);
            }

            if (Math.Abs((float)Math.Cos(radians)) > 0.3f)
            {
                using (Font font = new Font("Arial", 8, FontStyle.Bold))
                using (SolidBrush textBrush = new SolidBrush(Color.White))
                {
                    string valueText = Value.ToString();
                    SizeF textSize = g.MeasureString(valueText, font);
                    g.DrawString(valueText, font, textBrush,
                        Position.X - textSize.Width / 2,
                        Position.Y - textSize.Height / 2);
                }
            }
        }
    }
}
