using SpaceShooter.Entities;
using SpaceShooter.Managers.Data;
using SpaceShooter.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpaceShooter.Forms
{
    public partial class ShopForm : Form
    {
        private FlowLayoutPanel flpShips;
        private FlowLayoutPanel flpBullets;
        private FlowLayoutPanel flpPowerUps;
        private FlowLayoutPanel flpBackgrounds;
        private Label lblCoins;
        private Label lblHighScore;
        private Label lblExtraLives;
        private TabControl tabShop;
        private Button btnBack;

        public ShopForm()
        {
            this.BackgroundImage = Properties.Resources.shopbackground;
            this.BackgroundImageLayout = ImageLayout.Stretch;
            InitializeComponent();
            InitializeCustomControls();
            RefreshShopUI();
            LoadShopItems();
        }

        private void InitializeCustomControls()
        {
            this.Text = "Shop";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(15, 15, 20);

            Panel topPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.FromArgb(25, 25, 32)
            };

            lblCoins = new Label
            {
                Location = new Point(20, 15),
                Size = new Size(200, 30),
                Font = new Font("Arial", 12, FontStyle.Bold),
                ForeColor = Color.Gold,
                Text = "Coins: 0"
            };

            lblHighScore = new Label
            {
                Location = new Point(250, 15),
                Size = new Size(200, 30),
                Font = new Font("Arial", 12, FontStyle.Bold),
                ForeColor = Color.Cyan,
                Text = "High Score: 0"
            };

            lblExtraLives = new Label
            {
                Location = new Point(480, 15),
                Size = new Size(200, 30),
                Font = new Font("Arial", 12, FontStyle.Bold),
                ForeColor = Color.LimeGreen,
                Text = "Extra Lives: 0"
            };

            topPanel.Controls.AddRange(new Control[] { lblCoins, lblHighScore, lblExtraLives });

            tabShop = new TabControl
            {
                Dock = DockStyle.Fill,
                Font = new Font("Arial", 10)
            };

            TabPage tabShips = new TabPage("Ships") { BackColor = Color.FromArgb(20, 20, 26) };
            flpShips = CreateFlowLayoutPanel();
            tabShips.Controls.Add(flpShips);

            TabPage tabBullets = new TabPage("Bullets") { BackColor = Color.FromArgb(20, 20, 26) };
            flpBullets = CreateFlowLayoutPanel();
            tabBullets.Controls.Add(flpBullets);

            TabPage tabPowerUps = new TabPage("Extralife") { BackColor = Color.FromArgb(20, 20, 26) };
            flpPowerUps = CreateFlowLayoutPanel();
            tabPowerUps.Controls.Add(flpPowerUps);

            TabPage tabBackgrounds = new TabPage("Backgrounds") { BackColor = Color.FromArgb(20, 20, 26) };
            flpBackgrounds = CreateFlowLayoutPanel();
            tabBackgrounds.Controls.Add(flpBackgrounds);

            tabShop.TabPages.AddRange(new TabPage[] { tabShips, tabBullets, tabPowerUps, tabBackgrounds });

            btnBack = new Button
            {
                Text = "Back to Menu",
                Dock = DockStyle.Bottom,
                Height = 50,
                Font = new Font("Arial", 12, FontStyle.Bold),
                BackColor = Color.FromArgb(35, 35, 45),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnBack.FlatAppearance.BorderColor = Color.FromArgb(60, 60, 75);
            btnBack.Click += btnBack_Click;

            this.Controls.Add(tabShop);
            this.Controls.Add(topPanel);
            this.Controls.Add(btnBack);
        }

        private FlowLayoutPanel CreateFlowLayoutPanel()
        {
            return new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                BackColor = Color.FromArgb(20, 20, 26),
                Padding = new Padding(10)
            };
        }


        private void RefreshShopUI()
        {
            int coins = PlayerRepository.GetCoins();
            int highScore = PlayerRepository.GetHighScore();
            int extraLives = PlayerRepository.GetExtraLives();

            lblCoins.Text = $"Coins: {coins}";
            lblHighScore.Text = $"High Score: {highScore}";
            lblExtraLives.Text = $"Extra Lives: {extraLives}";
        }

        private void LoadShopItems()
        {
            flpShips.Controls.Clear();
            flpBullets.Controls.Clear();
            flpPowerUps.Controls.Clear();
            flpBackgrounds.Controls.Clear();

            var ships = ShopRepository.GetItemsByType(ShopItemType.ShipSkin);
            foreach (var ship in ships)
                flpShips.Controls.Add(CreateShopItemPanel(ship));

            var bullets = ShopRepository.GetItemsByType(ShopItemType.BulletStyle);
            foreach (var bullet in bullets)
                flpBullets.Controls.Add(CreateShopItemPanel(bullet));

            var powerUps = ShopRepository.GetItemsByType(ShopItemType.Consumable);
            foreach (var powerUp in powerUps)
                flpPowerUps.Controls.Add(CreateShopItemPanel(powerUp));

            var backgrounds = ShopRepository.GetItemsByType(ShopItemType.Background);
            foreach (var bg in backgrounds)
                flpBackgrounds.Controls.Add(CreateShopItemPanel(bg));


        }

        private Panel CreateShopItemPanel(ShopItem item)
        {
            var panel = new Panel
            {
                Width = 160,
                Height = 200,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(10),
                BackColor = Color.FromArgb(30, 30, 40)
            };

            var picBox = new PictureBox
            {
                Width = 120,
                Height = 100,
                Location = new Point(20, 10),
                SizeMode = PictureBoxSizeMode.Zoom,
                Image = GetItemImage(item.ItemName),
                BackColor = Color.Transparent
            };

            var lblName = new Label
            {
                Text = item.ItemName,
                Location = new Point(10, 115),
                Width = 140,
                Height = 20,
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.White,
                Font = new Font("Arial", 9, FontStyle.Bold)
            };

            var lblPrice = new Label
            {
                Text = $"{item.Price} Coins",
                Location = new Point(10, 140),
                Width = 140,
                Height = 20,
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.Gold,
                Font = new Font("Arial", 9)
            };

            var btn = new Button
            {
                Width = 140,
                Height = 35,
                Location = new Point(10, 160),
                Font = new Font("Arial", 9, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            btn.FlatAppearance.BorderColor = Color.FromArgb(70, 70, 90);

            if (item.IsEquipped)
            {
                btn.Text = "✓ Equipped";
                btn.Enabled = false;
                btn.BackColor = Color.FromArgb(45, 45, 55);
                btn.ForeColor = Color.LimeGreen;
            }
            else if (item.IsPurchased)
            {
                btn.Text = "Equip";
                btn.BackColor = Color.FromArgb(45, 45, 60);
                btn.ForeColor = Color.White;
                btn.Click += (s, e) => EquipItem(item);
            }
            else
            {
                btn.Text = "Buy";
                btn.BackColor = Color.FromArgb(0, 120, 60);
                btn.ForeColor = Color.White;
                btn.Click += (s, e) => BuyItem(item);
            }

            panel.Controls.AddRange(new Control[] { picBox, lblName, lblPrice, btn });
            return panel;
        }


        private Image GetItemImage(string itemName)
        {
            Image img = null;
            switch (itemName.ToLower())
            {
                case "player_white": img = Properties.Resources.spaceship1; break;
                case "player_blue": img = Properties.Resources.spaceship2; break;
                case "player_gray": img = Properties.Resources.spaceship3; break;
                case "player_green": img = Properties.Resources.spaceship4; break;
                case "player_yellow": img = Properties.Resources.spaceship5; break;
                case "yellow_bullet": img = Properties.Resources.bullet1; break;
                case "blue_bullet": img = Properties.Resources.bullet2; break;
                case "green_bullet": img = Properties.Resources.bullet3; break;
                case "red_bullet": img = Properties.Resources.bullet4; break;
                case "purple_bullet": img = Properties.Resources.bullet5; break;
                case "background1": img = Properties.Resources.background1; break;
                case "background2": img = Properties.Resources.background2; break;
                case "background3": img = Properties.Resources.background3; break;
                case "background4": img = Properties.Resources.background4; break;
                case "background5": img = Properties.Resources.background5; break;
                case "extralife": img = Properties.Resources.extralife; break;
            }

            if (img == null)
                System.Diagnostics.Debug.WriteLine($"[Shop] No image match for ItemName: '{itemName}'");

            return img;
        }


        private void BuyItem(ShopItem item)
        {
            int currentCoins = PlayerRepository.GetCoins();

            if (currentCoins < item.Price)
            {
                MessageBox.Show("Not enough coins!", "Shop", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            PlayerRepository.SpendCoins(item.Price);
            ShopRepository.PurchaseItem(item.Id);

            MessageBox.Show($"{item.ItemName} purchased!", "Shop", MessageBoxButtons.OK, MessageBoxIcon.Information);

            RefreshShopUI();
            LoadShopItems();
        }

        private void EquipItem(ShopItem item)
        {
            ShopRepository.EquipItem(item.Id, item.ItemType);

            MessageBox.Show($"{item.ItemName} equipped!", "Shop", MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (item.ItemName == "extralife")
            {
                var data = PlayerRepository.GetPlayerData();
                int newLives = data.ExtraLivesOwned + 1;
                int newMaxHealth = data.MaxHealth + 50;
                PlayerRepository.UpdateExtraLives(newLives, newMaxHealth);
            }

            LoadShopItems();
        }

        private void PlayClickSound()
        {
            SoundPlayer player = new SoundPlayer(Properties.Resources.resources_audio_menu_click);
            player.Play();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            PlayClickSound();
            this.Close();
        }


    }
}
