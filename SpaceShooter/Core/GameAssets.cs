using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using SpaceShooter.Models;
using SpaceShooter.Managers.Data;

namespace SpaceShooter.Core
{
    public static class GameAssets
    {
        public static Image PlayerImg;
        public static Image BulletImg;
        public static Image EnemyBulletImg;
        public static Image StandardEnemyImg;
        public static Image ScoutEnemyImg;
        public static Image ShooterEnemyImg;
        public static Image HeavyTankEnemyImg;
        public static Image TerroristEnemyImg;
        public static Image BackgroundImg;

        public static void Load()
        {
            StandardEnemyImg = MakeTransparent(Properties.Resources.standardenemy);
            ScoutEnemyImg = MakeTransparent(Properties.Resources.scoutenemy);
            ShooterEnemyImg = MakeTransparent(Properties.Resources.shooterenemy);
            HeavyTankEnemyImg = MakeTransparent(Properties.Resources.heavytankenemy);
            TerroristEnemyImg = MakeTransparent(Properties.Resources.terroristenemy);
            EnemyBulletImg = MakeTransparent(Properties.Resources.bullet1);

            LoadEquippedItems();
        }

        public static void LoadEquippedItems()
        {
            var equippedShip = ShopRepository.GetEquippedItem(ShopItemType.ShipSkin);
            PlayerImg = MakeTransparent(GetShipImage(equippedShip?.ItemName));

            var equippedBullet = ShopRepository.GetEquippedItem(ShopItemType.BulletStyle);
            BulletImg = MakeTransparent(GetBulletImage(equippedBullet?.ItemName));

            var equippedBg = ShopRepository.GetEquippedItem(ShopItemType.Background);
            BackgroundImg = GetBackgroundImage(equippedBg?.ItemName);
        }

        private static Bitmap GetShipImage(string itemName)
        {
            switch (itemName)
            {
                case "player_white": return Properties.Resources.spaceship1;
                case "player_blue": return Properties.Resources.spaceship2;
                case "player_gray": return Properties.Resources.spaceship3;
                case "player_green": return Properties.Resources.spaceship4;
                case "player_yellow": return Properties.Resources.spaceship5;
                default: return Properties.Resources.spaceship1;
            }
        }

        private static Bitmap GetBulletImage(string itemName)
        {
            switch (itemName)
            {
                case "yellow_bullet": return Properties.Resources.bullet1;
                case "blue_bullet": return Properties.Resources.bullet2;
                case "green_bullet": return Properties.Resources.bullet3;
                case "red_bullet": return Properties.Resources.bullet4;
                case "purple_bullet": return Properties.Resources.bullet5;
                default: return Properties.Resources.bullet1;
            }
        }

        private static Bitmap GetBackgroundImage(string itemName)
        {
            switch (itemName)
            {
                case "background1": return Properties.Resources.background1;
                case "background2": return Properties.Resources.background2;
                case "background3": return Properties.Resources.background3;
                case "background4": return Properties.Resources.background4;
                case "background5": return Properties.Resources.background5;
                default: return Properties.Resources.background1;
            }
        }

        private static Bitmap MakeTransparent(Bitmap source)
        {
            Bitmap result = new Bitmap(source);
            Color bgColor = Color.White;

            FloodFillEdge(result, 0, 0, bgColor);
            FloodFillEdge(result, result.Width - 1, 0, bgColor);
            FloodFillEdge(result, 0, result.Height - 1, bgColor);
            FloodFillEdge(result, result.Width - 1, result.Height - 1, bgColor);

            for (int x = 0; x < result.Width; x++)
            {
                FloodFillEdge(result, x, 0, bgColor);
                FloodFillEdge(result, x, result.Height - 1, bgColor);
            }

            for (int y = 0; y < result.Height; y++)
            {
                FloodFillEdge(result, 0, y, bgColor);
                FloodFillEdge(result, result.Width - 1, y, bgColor);
            }

            return result;
        }

        private static void FloodFillEdge(Bitmap bmp, int x, int y, Color targetColor)
        {
            if (x < 0 || x >= bmp.Width || y < 0 || y >= bmp.Height)
                return;

            Color pixelColor = bmp.GetPixel(x, y);

            if (pixelColor.A == 0 || !ColorsMatch(pixelColor, targetColor))
                return;

            Stack<Point> pixels = new Stack<Point>();
            pixels.Push(new Point(x, y));

            while (pixels.Count > 0)
            {
                Point pt = pixels.Pop();

                if (pt.X < 0 || pt.X >= bmp.Width || pt.Y < 0 || pt.Y >= bmp.Height)
                    continue;

                Color current = bmp.GetPixel(pt.X, pt.Y);

                if (current.A == 0 || !ColorsMatch(current, targetColor))
                    continue;

                bmp.SetPixel(pt.X, pt.Y, Color.Transparent);

                pixels.Push(new Point(pt.X + 1, pt.Y));
                pixels.Push(new Point(pt.X - 1, pt.Y));
                pixels.Push(new Point(pt.X, pt.Y + 1));
                pixels.Push(new Point(pt.X, pt.Y - 1));
            }
        }

        private static bool ColorsMatch(Color c1, Color c2, int tolerance = 30)
        {
            return Math.Abs(c1.R - c2.R) <= tolerance &&
                   Math.Abs(c1.G - c2.G) <= tolerance &&
                   Math.Abs(c1.B - c2.B) <= tolerance;
        }

    }

}
