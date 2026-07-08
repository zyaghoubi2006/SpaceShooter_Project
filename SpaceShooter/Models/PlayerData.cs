using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter.Models
{
    public class PlayerData
    {
        public int Id { get; set; }
        public int TotalCoins { get; set; }
        public int HighScore { get; set; }
        public int ExtraLivesOwned { get; set; }
        public int MaxHealth { get; set; }
    }
}
