using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpaceShooter.Core;
using System.Drawing;

namespace SpaceShooter.Entities.Enemy
{
     class ShooterEnemy :EnemyBase
    {
        private float shootTimer;
        private float fireRate = 1.5f;
        private List<Bullet> enemyBullets;


        public ShooterEnemy(int x, int y, Rectangle gameBounds, List<Bullet> enemyBullets) : base(x, y, 45, 45, 80f, 30, 200, gameBounds)
        {
            this.enemyBullets = enemyBullets;
        }

        public override void Update(float deltaTime)
        {
            shootTimer += deltaTime;
            if (shootTimer >= fireRate)
            {

                shootTimer = 0;

                enemyBullets.Add(new Bullet(X + Width / 2 - 3,Y + Height,6,15,0,250,false));

            }

            Y += Speed * deltaTime;

            if (Y > Bounds.Height) IsAlive = false;
        }

        public override void Draw(Graphics g)
        {
            
            g.FillRectangle(Brushes.BlueViolet, Bounds);
        }
    }
}
