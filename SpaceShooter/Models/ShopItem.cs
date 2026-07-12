using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter.Models
{
    public enum ShopItemType
    {
        ShipSkin,
        BulletStyle,
        Background,
        Consumable
    }

    public class ShopItem
    {
        public int Id { get; set; }
        public ShopItemType ItemType { get; set; }
        public string ItemName { get; set; }
        public int Price { get; set; }
        public bool IsPurchased { get; set; }
        public bool IsEquipped { get; set; }
    }
}
