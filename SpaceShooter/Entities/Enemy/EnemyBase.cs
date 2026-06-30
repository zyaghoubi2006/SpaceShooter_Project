using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpaceShooter.Core;
using System.Drawing;

namespace SpaceShooter.Entities.Enemy
{
    public abstract class EnemyBase:GameEngin
    {
        public int HP { get; protected set; }
        public int MaxHP { get; protected set; }
        public int ScoreValue { get; protected set; }
        public int CoinDropChance { get; protected set; }
        public float Speed { get; protected set; }

        protected Rectangle gameBounds;

        

        protected EnemyBase(int x, int y, int width, int height, float speed, int maxhp, int scorevalue, Rectangle bounds)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Speed = speed;
            MaxHP = maxhp;
            ScoreValue = scorevalue;
            gameBounds = bounds;
            IsAlive = true;

        }
    }
}
