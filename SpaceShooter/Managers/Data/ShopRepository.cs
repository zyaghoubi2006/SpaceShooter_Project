using SpaceShooter.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter.Managers.Data
{
    public static class ShopRepository
    {
        public static List<ShopItem> GetAllItems()
        {
            var items = new List<ShopItem>();
            using (var conn = DatabaseManager.GetConnection())
            {
                conn.Open();
                string query = "SELECT Id, ItemType, ItemName, Price, IsPurchased, IsEquipped FROM ShopItems";
                using (var cmd = new SQLiteCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        items.Add(ReadItem(reader));
                    }
                }
            }
            return items;
        }

        public static List<ShopItem> GetItemsByType(ShopItemType type)
        {
            var items = new List<ShopItem>();
            using (var conn = DatabaseManager.GetConnection())
            {
                conn.Open();
                string query = @"SELECT Id, ItemType, ItemName, Price, IsPurchased, IsEquipped 
                                 FROM ShopItems WHERE ItemType = @type";
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@type", type.ToString());
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            items.Add(ReadItem(reader));
                        }
                    }
                }
            }
            return items;
        }

        public static ShopItem GetEquippedItem(ShopItemType type)
        {
            using (var conn = DatabaseManager.GetConnection())
            {
                conn.Open();
                string query = @"SELECT Id, ItemType, ItemName, Price, IsPurchased, IsEquipped 
                                 FROM ShopItems 
                                 WHERE ItemType = @type AND IsEquipped = 1 
                                 LIMIT 1";
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@type", type.ToString());
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return ReadItem(reader);
                        }
                    }
                }
            }
            return null;
        }

        public static void PurchaseItem(int itemId)
        {
            using (var conn = DatabaseManager.GetConnection())
            {
                conn.Open();
                string query = "UPDATE ShopItems SET IsPurchased = 1 WHERE Id = @id";
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", itemId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void EquipItem(int itemId, ShopItemType type)
        {
            using (var conn = DatabaseManager.GetConnection())
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    string unequipQuery = "UPDATE ShopItems SET IsEquipped = 0 WHERE ItemType = @type";
                    using (var cmd = new SQLiteCommand(unequipQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@type", type.ToString());
                        cmd.ExecuteNonQuery();
                    }

                    string equipQuery = "UPDATE ShopItems SET IsEquipped = 1 WHERE Id = @id";
                    using (var cmd = new SQLiteCommand(equipQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", itemId);
                        cmd.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
            }
        }

        public static void ResetConsumableStatus()
        {
            using (var conn = DatabaseManager.GetConnection())
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText =
                        "UPDATE ShopItems SET IsPurchased = 0, IsEquipped = 0 WHERE ItemType = 'Consumable'";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "UPDATE PlayerData SET MaxHealth = 100, ExtraLivesOwned = 0";
                    cmd.ExecuteNonQuery();
                }
            }
        }



        private static ShopItem ReadItem(SQLiteDataReader reader)
        {
            return new ShopItem
            {
                Id = reader.GetInt32(0),
                ItemType = (ShopItemType)System.Enum.Parse(typeof(ShopItemType), reader.GetString(1)),
                ItemName = reader.GetString(2),
                Price = reader.GetInt32(3),
                IsPurchased = reader.GetInt32(4) == 1,
                IsEquipped = reader.GetInt32(5) == 1
            };
        }
    }
}
