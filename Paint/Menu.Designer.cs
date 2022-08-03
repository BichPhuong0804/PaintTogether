
namespace Paint
{
    partial class Menu
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Menu));
            this.panel_welcome = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.Uname = new System.Windows.Forms.Label();
            this.hostIP_textBox = new System.Windows.Forms.TextBox();
            this.join_btn = new System.Windows.Forms.Button();
            this.pictureBox6 = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.btnMinimize = new FontAwesome.Sharp.IconButton();
            this.btnMaximize = new FontAwesome.Sharp.IconButton();
            this.btnClose = new FontAwesome.Sharp.IconButton();
            this.codeRoom_textBox = new System.Windows.Forms.TextBox();
            this.room_btn = new System.Windows.Forms.Button();
            this.panel_welcome.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            this.SuspendLayout();
            // 
            // panel_welcome
            // 
            this.panel_welcome.Controls.Add(this.label1);
            this.panel_welcome.Controls.Add(this.Uname);
            this.panel_welcome.Controls.Add(this.hostIP_textBox);
            this.panel_welcome.Controls.Add(this.join_btn);
            this.panel_welcome.Controls.Add(this.pictureBox6);
            this.panel_welcome.Controls.Add(this.pictureBox3);
            this.panel_welcome.Controls.Add(this.pictureBox4);
            this.panel_welcome.Controls.Add(this.btnMinimize);
            this.panel_welcome.Controls.Add(this.btnMaximize);
            this.panel_welcome.Controls.Add(this.btnClose);
            this.panel_welcome.Controls.Add(this.codeRoom_textBox);
            this.panel_welcome.Controls.Add(this.room_btn);
            this.panel_welcome.Location = new System.Drawing.Point(0, 0);
            this.panel_welcome.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel_welcome.Name = "panel_welcome";
            this.panel_welcome.Size = new System.Drawing.Size(869, 458);
            this.panel_welcome.TabIndex = 19;
            this.panel_welcome.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel_welcome_MouseDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Forte", 25.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label1.Location = new System.Drawing.Point(114, 87);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(438, 47);
            this.label1.TabIndex = 28;
            this.label1.Text = "Painting with HADPU";
            // 
            // Uname
            // 
            this.Uname.AutoSize = true;
            this.Uname.Font = new System.Drawing.Font("Montserrat SemiBold", 10F, System.Drawing.FontStyle.Bold);
            this.Uname.Location = new System.Drawing.Point(12, 14);
            this.Uname.Name = "Uname";
            this.Uname.Size = new System.Drawing.Size(0, 24);
            this.Uname.TabIndex = 27;
            // 
            // hostIP_textBox
            // 
            this.hostIP_textBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.hostIP_textBox.Font = new System.Drawing.Font("Montserrat", 16F);
            this.hostIP_textBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.hostIP_textBox.Location = new System.Drawing.Point(176, 210);
            this.hostIP_textBox.Margin = new System.Windows.Forms.Padding(4);
            this.hostIP_textBox.Name = "hostIP_textBox";
            this.hostIP_textBox.Size = new System.Drawing.Size(311, 33);
            this.hostIP_textBox.TabIndex = 25;
            this.hostIP_textBox.Text = "\r\nIP address...\r\n";
            this.hostIP_textBox.WordWrap = false;
            this.hostIP_textBox.Click += new System.EventHandler(this.hostIP_textBox_Click);
            this.hostIP_textBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.hostIP_textBox_KeyDown);
            this.hostIP_textBox.Leave += new System.EventHandler(this.hostIP_textBox_Leave);
            // 
            // join_btn
            // 
            this.join_btn.BackColor = System.Drawing.Color.DarkCyan;
            this.join_btn.Font = new System.Drawing.Font("Montserrat SemiBold", 12F, System.Drawing.FontStyle.Bold);
            this.join_btn.ForeColor = System.Drawing.Color.LightSalmon;
            this.join_btn.Location = new System.Drawing.Point(421, 281);
            this.join_btn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.join_btn.Name = "join_btn";
            this.join_btn.Size = new System.Drawing.Size(121, 57);
            this.join_btn.TabIndex = 24;
            this.join_btn.Text = "Join";
            this.join_btn.UseVisualStyleBackColor = false;
            this.join_btn.Click += new System.EventHandler(this.join_btn_Click_1);
            // 
            // pictureBox6
            // 
            this.pictureBox6.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox6.BackgroundImage")));
            this.pictureBox6.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox6.Location = new System.Drawing.Point(745, 77);
            this.pictureBox6.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox6.Name = "pictureBox6";
            this.pictureBox6.Size = new System.Drawing.Size(80, 73);
            this.pictureBox6.TabIndex = 23;
            this.pictureBox6.TabStop = false;
            // 
            // pictureBox3
            // 
            this.pictureBox3.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox3.BackgroundImage")));
            this.pictureBox3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox3.Location = new System.Drawing.Point(615, 310);
            this.pictureBox3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(254, 148);
            this.pictureBox3.TabIndex = 21;
            this.pictureBox3.TabStop = false;
            // 
            // pictureBox4
            // 
            this.pictureBox4.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox4.BackgroundImage")));
            this.pictureBox4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox4.Location = new System.Drawing.Point(0, 304);
            this.pictureBox4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(169, 154);
            this.pictureBox4.TabIndex = 20;
            this.pictureBox4.TabStop = false;
            // 
            // btnMinimize
            // 
            this.btnMinimize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMinimize.BackColor = System.Drawing.Color.LightSalmon;
            this.btnMinimize.FlatAppearance.BorderSize = 0;
            this.btnMinimize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMinimize.IconChar = FontAwesome.Sharp.IconChar.WindowMinimize;
            this.btnMinimize.IconColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnMinimize.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnMinimize.IconSize = 30;
            this.btnMinimize.Location = new System.Drawing.Point(745, 14);
            this.btnMinimize.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnMinimize.Name = "btnMinimize";
            this.btnMinimize.Size = new System.Drawing.Size(36, 28);
            this.btnMinimize.TabIndex = 17;
            this.btnMinimize.UseVisualStyleBackColor = false;
            this.btnMinimize.Click += new System.EventHandler(this.btnMinimize_Click_1);
            // 
            // btnMaximize
            // 
            this.btnMaximize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMaximize.BackColor = System.Drawing.Color.LightSalmon;
            this.btnMaximize.FlatAppearance.BorderSize = 0;
            this.btnMaximize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMaximize.IconChar = FontAwesome.Sharp.IconChar.WindowMaximize;
            this.btnMaximize.IconColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnMaximize.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnMaximize.IconSize = 30;
            this.btnMaximize.Location = new System.Drawing.Point(784, 14);
            this.btnMaximize.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnMaximize.Name = "btnMaximize";
            this.btnMaximize.Size = new System.Drawing.Size(36, 28);
            this.btnMaximize.TabIndex = 16;
            this.btnMaximize.UseVisualStyleBackColor = false;
            this.btnMaximize.Click += new System.EventHandler(this.btnMaximize_Click_1);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackColor = System.Drawing.Color.LightSalmon;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.IconChar = FontAwesome.Sharp.IconChar.WindowClose;
            this.btnClose.IconColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnClose.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnClose.IconSize = 30;
            this.btnClose.Location = new System.Drawing.Point(821, 14);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(36, 28);
            this.btnClose.TabIndex = 15;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click_1);
            // 
            // codeRoom_textBox
            // 
            this.codeRoom_textBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.codeRoom_textBox.Font = new System.Drawing.Font("Montserrat", 16F);
            this.codeRoom_textBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.codeRoom_textBox.Location = new System.Drawing.Point(176, 291);
            this.codeRoom_textBox.Margin = new System.Windows.Forms.Padding(4);
            this.codeRoom_textBox.Name = "codeRoom_textBox";
            this.codeRoom_textBox.Size = new System.Drawing.Size(222, 33);
            this.codeRoom_textBox.TabIndex = 3;
            this.codeRoom_textBox.Text = "Code...";
            this.codeRoom_textBox.WordWrap = false;
            this.codeRoom_textBox.Click += new System.EventHandler(this.codeRoom_textBox_Click);
            this.codeRoom_textBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.codeRom_textBox_KeyDown);
            this.codeRoom_textBox.Leave += new System.EventHandler(this.codeRoom_textBox_Leave);
            // 
            // room_btn
            // 
            this.room_btn.BackColor = System.Drawing.Color.DarkCyan;
            this.room_btn.Font = new System.Drawing.Font("Montserrat SemiBold", 12F, System.Drawing.FontStyle.Bold);
            this.room_btn.ForeColor = System.Drawing.Color.LightSalmon;
            this.room_btn.Location = new System.Drawing.Point(511, 201);
            this.room_btn.Margin = new System.Windows.Forms.Padding(4);
            this.room_btn.Name = "room_btn";
            this.room_btn.Size = new System.Drawing.Size(224, 61);
            this.room_btn.TabIndex = 0;
            this.room_btn.Text = "Create Room";
            this.room_btn.UseVisualStyleBackColor = false;
            this.room_btn.Click += new System.EventHandler(this.room_btn_Click);
            // 
            // Menu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSalmon;
            this.ClientSize = new System.Drawing.Size(869, 458);
            this.Controls.Add(this.panel_welcome);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Menu";
            this.Text = "Draw together";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Menu_FormClosed);
            this.panel_welcome.ResumeLayout(false);
            this.panel_welcome.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel_welcome;
        private FontAwesome.Sharp.IconButton btnMinimize;
        private FontAwesome.Sharp.IconButton btnMaximize;
        private FontAwesome.Sharp.IconButton btnClose;
        private System.Windows.Forms.TextBox codeRoom_textBox;
        private System.Windows.Forms.Button room_btn;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.PictureBox pictureBox6;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.Button join_btn;
        private System.Windows.Forms.TextBox hostIP_textBox;
        private System.Windows.Forms.Label Uname;
        private System.Windows.Forms.Label label1;
    }
}