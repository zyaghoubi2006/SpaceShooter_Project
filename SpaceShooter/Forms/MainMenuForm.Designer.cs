
namespace SpaceShooter.Forms
{
    partial class MainMenuForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_Play = new System.Windows.Forms.Button();
            this.btn_Shop = new System.Windows.Forms.Button();
            this.btn_Option = new System.Windows.Forms.Button();
            this.btn_About = new System.Windows.Forms.Button();
            this.btn_Quit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn_Play
            // 
            this.btn_Play.Location = new System.Drawing.Point(382, 379);
            this.btn_Play.Name = "btn_Play";
            this.btn_Play.Size = new System.Drawing.Size(171, 59);
            this.btn_Play.TabIndex = 0;
            this.btn_Play.Text = "Play";
            this.btn_Play.UseVisualStyleBackColor = true;
            this.btn_Play.Click += new System.EventHandler(this.btn_Play_Click);
            // 
            // btn_Shop
            // 
            this.btn_Shop.Location = new System.Drawing.Point(382, 444);
            this.btn_Shop.Name = "btn_Shop";
            this.btn_Shop.Size = new System.Drawing.Size(171, 59);
            this.btn_Shop.TabIndex = 1;
            this.btn_Shop.Text = "Shop";
            this.btn_Shop.UseVisualStyleBackColor = true;
            this.btn_Shop.Click += new System.EventHandler(this.btn_Shop_Click);
            // 
            // btn_Option
            // 
            this.btn_Option.Location = new System.Drawing.Point(382, 509);
            this.btn_Option.Name = "btn_Option";
            this.btn_Option.Size = new System.Drawing.Size(171, 59);
            this.btn_Option.TabIndex = 2;
            this.btn_Option.Text = "Option";
            this.btn_Option.UseVisualStyleBackColor = true;
            this.btn_Option.Click += new System.EventHandler(this.btn_Option_Click);
            // 
            // btn_About
            // 
            this.btn_About.Location = new System.Drawing.Point(382, 574);
            this.btn_About.Name = "btn_About";
            this.btn_About.Size = new System.Drawing.Size(171, 59);
            this.btn_About.TabIndex = 3;
            this.btn_About.Text = "About";
            this.btn_About.UseVisualStyleBackColor = true;
            this.btn_About.Click += new System.EventHandler(this.btn_About_Click);
            // 
            // btn_Quit
            // 
            this.btn_Quit.Location = new System.Drawing.Point(382, 639);
            this.btn_Quit.Name = "btn_Quit";
            this.btn_Quit.Size = new System.Drawing.Size(171, 59);
            this.btn_Quit.TabIndex = 4;
            this.btn_Quit.Text = "Quit";
            this.btn_Quit.UseVisualStyleBackColor = true;
            this.btn_Quit.Click += new System.EventHandler(this.btn_Quit_Click);
            // 
            // MainMenuForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 707);
            this.Controls.Add(this.btn_Quit);
            this.Controls.Add(this.btn_About);
            this.Controls.Add(this.btn_Option);
            this.Controls.Add(this.btn_Shop);
            this.Controls.Add(this.btn_Play);
            this.Name = "MainMenuForm";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_Play;
        private System.Windows.Forms.Button btn_Shop;
        private System.Windows.Forms.Button btn_Option;
        private System.Windows.Forms.Button btn_About;
        private System.Windows.Forms.Button btn_Quit;
    }
}

