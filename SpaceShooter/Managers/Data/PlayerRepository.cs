using SpaceShooter.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter.Managers.Data
{
    public static class PlayerRepository
    {
        public static PlayerData GetPlayerData()
        {
            using (var conn = DatabaseManager.GetConnection())
            {
                conn.Open();
                string query = "SELECT Id, TotalCoins, HighScore, ExtraLivesOwned, MaxHealth FROM PlayerData WHERE Id = 1";
                using (var cmd = new SQLiteCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new PlayerData
                        {
                            Id = reader.GetInt32(0),
                            TotalCoins = reader.GetInt32(1),
                            HighScore = reader.GetInt32(2),
                            ExtraLivesOwned = reader.GetInt32(3),
                            MaxHealth = reader.GetInt32(4)
                        };
                    }
                }
                return null;
            }
        }

        public static void UpdateCoins(int totalCoins)
        {
            using (var conn = DatabaseManager.GetConnection())
            {
                conn.Open();
                string query = "UPDATE PlayerData SET TotalCoins = @coins WHERE Id = 1";
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@coins", totalCoins);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void UpdateHighScore(int score)
        {
            using (var conn = DatabaseManager.GetConnection())
            {
                conn.Open();
                string query = @"UPDATE PlayerData 
                                 SET HighScore = @score 
                                 WHERE Id = 1 AND @score > HighScore";
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@score", score);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void UpdateExtraLives(int lives, int maxHealth)
        {
            using (var conn = DatabaseManager.GetConnection())
            {
                conn.Open();
                string query = @"UPDATE PlayerData 
                SET ExtraLivesOwned = @lives, MaxHealth = @max 
                         WHERE Id = 1";
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@lives", lives);
                    cmd.Parameters.AddWithValue("@max", maxHealth);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void AddCoins(int amount)
        {
            var data = GetPlayerData();
            if (data != null)
            {
                UpdateCoins(data.TotalCoins + amount);
            }
        }

        public static bool SpendCoins(int amount)
        {
            var data = GetPlayerData();
            if (data != null && data.TotalCoins >= amount)
            {
                UpdateCoins(data.TotalCoins - amount);
                return true;
            }
            return false;
        }

        public static int GetHighScore()
        {
            var data = GetPlayerData();
            return data?.HighScore ?? 0;
        }

        public static int GetCoins()
        {
            var data = GetPlayerData();
            return data?.TotalCoins ?? 0;
        }

        public static int GetExtraLives()
        {
            var data = GetPlayerData();
            return data?.ExtraLivesOwned ?? 0;
        }

        public static void SaveHighScore(int score)
        {
            UpdateHighScore(score);
        }
    }
}
