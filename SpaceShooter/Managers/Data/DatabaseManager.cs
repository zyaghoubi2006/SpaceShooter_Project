using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Data.SQLite;
using System.IO;

namespace SpaceShooter.Managers
{
    public class DatabaseManager
    {
        private static readonly string DbPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "SpaceShooter",
            "gamedata.db"
        );

        private static string ConnectionString => $"Data Source={DbPath};Version=3;";

        public static void Initialize()
        {
            string directory = Path.GetDirectoryName(DbPath);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            if (!File.Exists(DbPath))
            {
                SQLiteConnection.CreateFile(DbPath);
                CreateTables();
            }
        }

        public static SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(ConnectionString);
        }

        private static void CreateTables()
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                string createPlayerDataTable = @"
                CREATE TABLE IF NOT EXISTS PlayerData (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    TotalCoins INTEGER DEFAULT 0,
                    HighScore INTEGER DEFAULT 0,
                    ExtraLivesOwned INTEGER DEFAULT 0,
                    MaxHealth INTEGER DEFAULT 100
                )";

                string createShopItemsTable = @"
                CREATE TABLE IF NOT EXISTS ShopItems (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    ItemType TEXT NOT NULL,
                    ItemName TEXT NOT NULL,
                    Price INTEGER NOT NULL,
                    IsPurchased INTEGER DEFAULT 0,
                    IsEquipped INTEGER DEFAULT 0
                )";

                using (var cmd = new SQLiteCommand(createPlayerDataTable, connection))
                    cmd.ExecuteNonQuery();

                using (var cmd = new SQLiteCommand(createShopItemsTable, connection))
                    cmd.ExecuteNonQuery();

                InitializePlayerData(connection);
                InitializeShopItems(connection);
            }
        }

        private static void InitializePlayerData(SQLiteConnection connection)
        {
            string insert = "INSERT INTO PlayerData (TotalCoins, HighScore, ExtraLivesOwned, MaxHealth) VALUES (0, 0, 0, 100)";
            using (var cmd = new SQLiteCommand(insert, connection))
                cmd.ExecuteNonQuery();
        }

        private static void InitializeShopItems(SQLiteConnection connection)
        {
            var defaultItems = new[]
            {
                ("ShipSkin", "player_white", 0, 1, 1),
                ("ShipSkin", "player_blue", 5, 0, 0),
                ("ShipSkin", "player_gray", 10, 0, 0),
                ("ShipSkin", "player_green", 20, 0, 0),
                ("ShipSkin", "player_yellow", 50, 0, 0),

                ("BulletStyle", "yellow_bullet", 0, 1, 1),
                ("BulletStyle", "blue_bullet", 5, 0, 0),
                ("BulletStyle", "green_bullet", 10, 0, 0),
                ("BulletStyle", "red_bullet", 15, 0, 0),
                ("BulletStyle", "purple_bullet", 20, 0, 0),

                ("Background", "background1", 0, 1, 1),
                ("Background", "background2", 5, 0, 0),
                ("Background", "background3", 10, 0, 0),
                ("Background", "background4", 30, 0, 0),
                ("Background", "background5", 50, 0, 0),

                ("Consumable", "extralife", 10, 0, 0)
            };

            foreach (var item in defaultItems)
            {
                string insert = @"INSERT INTO ShopItems (ItemType, ItemName, Price, IsPurchased, IsEquipped) 
                                 VALUES (@type, @name, @price, @purchased, @equipped)";
                using (var cmd = new SQLiteCommand(insert, connection))
                {
                    cmd.Parameters.AddWithValue("@type", item.Item1);
                    cmd.Parameters.AddWithValue("@name", item.Item2);
                    cmd.Parameters.AddWithValue("@price", item.Item3);
                    cmd.Parameters.AddWithValue("@purchased", item.Item4);
                    cmd.Parameters.AddWithValue("@equipped", item.Item5);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }

}
