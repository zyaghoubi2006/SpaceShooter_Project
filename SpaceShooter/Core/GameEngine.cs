using SpaceShooter.Entities;
using SpaceShooter.Entities;
using SpaceShooter.Entities.Enemies;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Media;

namespace SpaceShooter.Core
{
    public class GameEngine
    {
        public Player Player { get; private set; }
        public List<Enemy> Enemies { get; private set; }
        public List<Bullet> Bullets { get; private set; }
        public List<Coin> Coins { get; private set; }

        private Random random;
        private float spawnTimer;
        private const float SpawnInterval = 1.5f;

        public int CurrentWave { get; private set; }
        public int TotalWaves { get; private set; }
        public bool IsWaveComplete { get; private set; }
        public bool IsGameComplete { get; private set; }
        public float WaveTransitionTimer { get; private set; }
        private const float WaveTransitionDelay = 2f;
        private int enemiesToSpawnThisWave;
        private int enemiesSpawnedThisWave;

        private bool _playerDelayedExplosion = false;
        private DateTime _playerExplosionTime;

        private AudioManager audioManager;

        private int _blinkCounter = 0;


        public GameEngine(int screenWidth, int screenHeight, AudioManager audioManager)
        {
            this.audioManager = audioManager;

            Player = new Player(screenWidth + 150 , screenHeight + 300);
            Enemies = new List<Enemy>();
            Bullets = new List<Bullet>();
            Coins = new List<Coin>();
            random = new Random();
            spawnTimer = 0;

            CurrentWave = 1;
            TotalWaves = 10;
            IsWaveComplete = false;
            IsGameComplete = false;
            WaveTransitionTimer = 0;
            StartNewWave();
        }

        private void StartNewWave()
        {
            enemiesToSpawnThisWave = 5 + (CurrentWave * 2);
            enemiesSpawnedThisWave = 0;
            IsWaveComplete = false;
        }

        public void Update(float deltaTime)
        {
            if (_playerDelayedExplosion && DateTime.Now >= _playerExplosionTime)
            {
                _playerDelayedExplosion = false;
                Player.TakeDamage(50);
            }

            if (IsWaveComplete)
            {
                WaveTransitionTimer += deltaTime;
                if (WaveTransitionTimer >= WaveTransitionDelay)
                {
                    WaveTransitionTimer = 0;
                    CurrentWave++;

                    if (CurrentWave > TotalWaves)
                    {
                        IsGameComplete = true;
                        return;
                    }

                    StartNewWave();
                }
                return;
            }

            Player.Update(deltaTime);

            foreach (var enemy in Enemies)
            {
                if (enemy is TerroristEnemy terrorist)
                {
                    terrorist.SetTarget(Player.Position.X, Player.Position.Y);
                }
                enemy.Update(deltaTime);
            }

            foreach (var bullet in Bullets)
            {
                bullet.Update(deltaTime);
            }

            foreach (var coin in Coins)
            {
                coin.Update(deltaTime);
            }

            CheckCollisions();
            RemoveInactiveObjects();

            if (enemiesSpawnedThisWave < enemiesToSpawnThisWave)
            {
                spawnTimer += deltaTime;
                if (spawnTimer >= SpawnInterval)
                {
                    SpawnEnemy();
                    spawnTimer = 0;
                    enemiesSpawnedThisWave++;
                }
            }
            else if (Enemies.Count == 0)
            {
                IsWaveComplete = true;
            }
        }

        private void SpawnEnemy()
        {
            int screenWidth = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
            float x = random.Next(50, screenWidth - 50);
            float y = -50;

            Enemy newEnemy = CreateRandomEnemy(x, y);
            newEnemy.OnShoot += (x1, y1, vx, vy) => Bullets.Add(new Bullet(x1, y1, vx, vy, false));
            Enemies.Add(newEnemy);
        }

        private Enemy CreateRandomEnemy(float x, float y)
        {
            int enemyType = GetWeightedEnemyType();
            Enemy enemy = null;

            switch (enemyType)
            {
                case 0:
                    enemy = new StandardEnemy(x, y);
                    break;
                case 1:
                    enemy = new ScoutEnemy(x, y);
                    break;
                case 2:
                    enemy = new ShooterEnemy(x, y, audioManager);
                    break;
                case 3:
                    enemy = new TerroristEnemy(x, y);
                    break;
                case 4:
                    enemy = new HeavyTankEnemy(x, y, audioManager);
                    break;
            }

            ApplyWaveDifficulty(enemy);
            return enemy;
        }

        private int GetWeightedEnemyType()
        {
            if (CurrentWave < 3)
            {
                int roll = random.Next(100);
                if (roll < 50) return 0;
                return 1;
            }

            else if (CurrentWave < 6)
            {
                int roll = random.Next(100);
                if (roll < 20) return 0;
                if (roll < 50) return 1;
                if (roll < 85) return 2;
                return 3;
            }

            else if (CurrentWave < 9)
            {
                int roll = random.Next(100);
                if (roll < 15) return 0;
                if (roll < 35) return 1;
                if (roll < 65) return 2;
                return 3;
            }

            else
            {
                int roll = random.Next(100);
                if (roll < 10) return 0;
                if (roll < 25) return 1;
                if (roll < 50) return 2;
                if (roll < 80) return 3;
                return 4;
            }
        }

        private void ApplyWaveDifficulty(Enemy enemy)
        {
            float speedMultiplier = 1f + (0.1f * CurrentWave);
            int hpBonus = 2 * CurrentWave;

            enemy.Velocity = new PointF(
                enemy.Velocity.X * speedMultiplier,
                enemy.Velocity.Y * speedMultiplier
            );

            enemy.Health += hpBonus;
            enemy.MaxHealth += hpBonus;
        }

        private void CheckCollisions()
        {
            for (int i = Enemies.Count - 1; i >= 0; i--)
            {
                Enemy enemy = Enemies[i];

                if (enemy.CollidesWith(Player))
                {
                    if (enemy is TerroristEnemy terrorist)
                    {
                        enemy.IsActive = false;
                        _playerDelayedExplosion = true;
                        _playerExplosionTime = DateTime.Now.AddSeconds(1);
                        audioManager.PlaySoundEffect(@"F:\SpaceShooter\SpaceShooter\Resources\bombexplosion4.mp3");
                        continue;
                    }
                    else
                    {
                        Player.TakeDamage(20);
                        audioManager.PlaySoundEffect(@"F:\SpaceShooter\SpaceShooter\Resources\PlayerDamage.wav");
                        enemy.IsActive = false;
                        continue;
                    }
                }

                for (int j = Bullets.Count - 1; j >= 0; j--)
                {
                    Bullet bullet = Bullets[j];
                    if (bullet.IsPlayerBullet && enemy.CollidesWith(bullet))
                    {
                        enemy.TakeDamage(bullet.Damage);
                        bullet.IsActive = false;

                        if (!enemy.IsActive)
                        {
                            Player.Score += enemy.ScoreValue;
                            TryDropCoin(enemy.Position.X, enemy.Position.Y, enemy);
                        }
                        break;
                    }
                }
            }

            for (int i = Bullets.Count - 1; i >= 0; i--)
            {
                Bullet bullet = Bullets[i];
                if (!bullet.IsPlayerBullet && Player.CollidesWith(bullet))
                {
                    Player.TakeDamage(bullet.Damage);
                    audioManager.PlaySoundEffect(@"F:\SpaceShooter\SpaceShooter\Resources\PlayerDamage.wav");
                    bullet.IsActive = false;
                }
            }

            for (int i = Coins.Count - 1; i >= 0; i--)
            {
                if (Coins[i].CheckCollision(Player))
                {
                    Player.Coins += Coins[i].Value;
                    audioManager.PlaySoundEffect(@"F:\SpaceShooter\SpaceShooter\Resources\CoinDrop.wav");
                }
            }
        }

        private void TryDropCoin(float x, float y, Enemy enemy)
        {
            float dropChance = GetCoinDropChance(enemy);

            if (random.NextDouble() < dropChance)
            {
                CoinType coinType = DetermineCoinType(enemy);
                Coins.Add(new Coin(x, y, coinType));
            }
        }

        private float GetCoinDropChance(Enemy enemy)
        {
            float baseChance = 0.2f;

            if (enemy is HeavyTankEnemy)
                baseChance = 0.85f;
            else if (enemy is TerroristEnemy)
                baseChance = 0.6f;
            else if (enemy is ShooterEnemy)
                baseChance = 0.5f;
            else if (enemy is ScoutEnemy)
                baseChance = 0.35f;
            else if (enemy is StandardEnemy)
                baseChance = 0.25f;

            baseChance += CurrentWave * 0.02f;
            return Math.Min(baseChance, 0.95f);
        }

        private CoinType DetermineCoinType(Enemy enemy)
        {
            int goldChance = 10;

            if (enemy is HeavyTankEnemy)
                goldChance = 70;
            else if (enemy is TerroristEnemy)
                goldChance = 50;
            else if (enemy is ShooterEnemy)
                goldChance = 35;
            else if (enemy is ScoutEnemy)
                goldChance = 20;
            else if (enemy is StandardEnemy)
                goldChance = 15;

            goldChance += CurrentWave * 2;
            goldChance = Math.Min(goldChance, 85);

            return random.Next(100) < goldChance ? CoinType.Gold : CoinType.Silver;
        }

        private void RemoveInactiveObjects()
        {
            Enemies.RemoveAll(e => !e.IsActive);
            Bullets.RemoveAll(b => !b.IsActive);
            Coins.RemoveAll(c => !c.IsActive);
        }

        public void Draw(Graphics g)
        {
            Player.Draw(g);

            foreach (var enemy in Enemies)
            {
                enemy.Draw(g);
            }

            foreach (var bullet in Bullets)
            {
                bullet.Draw(g);
            }

            foreach (var coin in Coins)
            {
                coin.Draw(g);
            }
        }

        public void PlayerShoot()
        {
            float bulletX = Player.Position.X + Player.Size.Width / 2 - 2.5f;
            float bulletY = Player.Position.Y;
            Bullets.Add(new Bullet(bulletX, bulletY, 0, -500, true));
        }
    }
}
