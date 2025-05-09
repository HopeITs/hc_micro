namespace MyWorkApplication
{
    partial class Login_Form
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login_Form));
            this.Connect_button = new System.Windows.Forms.Button();
            this.Remember_checkBox = new System.Windows.Forms.CheckBox();
            this.Close_button = new System.Windows.Forms.Button();
            this.Online_checkBox = new System.Windows.Forms.CheckBox();
            this.UserName_textBox = new System.Windows.Forms.TextBox();
            this.Password_textBox = new System.Windows.Forms.TextBox();
            this.Version_label = new System.Windows.Forms.Label();
            this.ShowPassword_button = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Copyright_label = new System.Windows.Forms.Label();
            this.ProfilePicture_pictureBox = new OvalPictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProfilePicture_pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // Connect_button
            // 
            this.Connect_button.BackColor = System.Drawing.Color.White;
            this.Connect_button.BackgroundImage = global::MyWorkApplication.Properties.Resources.login00;
            this.Connect_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Connect_button.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.Connect_button.FlatAppearance.BorderSize = 0;
            this.Connect_button.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.Connect_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Connect_button.ForeColor = System.Drawing.Color.Black;
            this.Connect_button.Location = new System.Drawing.Point(83, 477);
            this.Connect_button.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Connect_button.Name = "Connect_button";
            this.Connect_button.Size = new System.Drawing.Size(254, 38);
            this.Connect_button.TabIndex = 3;
            this.Connect_button.UseVisualStyleBackColor = false;
            this.Connect_button.Click += new System.EventHandler(this.Connect_button_Click);
            this.Connect_button.MouseEnter += new System.EventHandler(this.Connect_button_MouseEnter);
            this.Connect_button.MouseLeave += new System.EventHandler(this.Connect_button_MouseLeave);
            // 
            // Remember_checkBox
            // 
            this.Remember_checkBox.AutoSize = true;
            this.Remember_checkBox.BackColor = System.Drawing.Color.White;
            this.Remember_checkBox.ForeColor = System.Drawing.Color.Black;
            this.Remember_checkBox.Location = new System.Drawing.Point(83, 530);
            this.Remember_checkBox.Name = "Remember_checkBox";
            this.Remember_checkBox.Size = new System.Drawing.Size(154, 31);
            this.Remember_checkBox.TabIndex = 4;
            this.Remember_checkBox.Text = "Remember Me";
            this.Remember_checkBox.UseVisualStyleBackColor = false;
            // 
            // Close_button
            // 
            this.Close_button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(0)))), ((int)(((byte)(30)))));
            this.Close_button.BackgroundImage = global::MyWorkApplication.Properties.Resources.login_close1;
            this.Close_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Close_button.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(0)))), ((int)(((byte)(30)))));
            this.Close_button.FlatAppearance.BorderSize = 0;
            this.Close_button.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.Close_button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(0)))), ((int)(((byte)(30)))));
            this.Close_button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(0)))), ((int)(((byte)(30)))));
            this.Close_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Close_button.Location = new System.Drawing.Point(975, 37);
            this.Close_button.Margin = new System.Windows.Forms.Padding(0);
            this.Close_button.Name = "Close_button";
            this.Close_button.Size = new System.Drawing.Size(21, 20);
            this.Close_button.TabIndex = 10;
            this.Close_button.UseVisualStyleBackColor = false;
            this.Close_button.Click += new System.EventHandler(this.Close_button_Click);
            this.Close_button.MouseEnter += new System.EventHandler(this.Close_button_MouseEnter);
            this.Close_button.MouseLeave += new System.EventHandler(this.Close_button_MouseLeave);
            // 
            // Online_checkBox
            // 
            this.Online_checkBox.AutoSize = true;
            this.Online_checkBox.BackColor = System.Drawing.Color.White;
            this.Online_checkBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.Online_checkBox.ForeColor = System.Drawing.Color.Black;
            this.Online_checkBox.Location = new System.Drawing.Point(82, 556);
            this.Online_checkBox.Name = "Online_checkBox";
            this.Online_checkBox.Size = new System.Drawing.Size(86, 31);
            this.Online_checkBox.TabIndex = 5;
            this.Online_checkBox.Text = "Online";
            this.Online_checkBox.UseVisualStyleBackColor = false;
            // 
            // UserName_textBox
            // 
            this.UserName_textBox.BackColor = System.Drawing.Color.White;
            this.UserName_textBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.UserName_textBox.Font = new System.Drawing.Font("Rubik", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UserName_textBox.ForeColor = System.Drawing.Color.DimGray;
            this.UserName_textBox.Location = new System.Drawing.Point(96, 383);
            this.UserName_textBox.Name = "UserName_textBox";
            this.UserName_textBox.Size = new System.Drawing.Size(237, 24);
            this.UserName_textBox.TabIndex = 1;
            this.UserName_textBox.Text = "Username";
            // 
            // Password_textBox
            // 
            this.Password_textBox.BackColor = System.Drawing.Color.White;
            this.Password_textBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Password_textBox.Font = new System.Drawing.Font("Rubik", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Password_textBox.ForeColor = System.Drawing.Color.DimGray;
            this.Password_textBox.Location = new System.Drawing.Point(97, 432);
            this.Password_textBox.Name = "Password_textBox";
            this.Password_textBox.PasswordChar = '*';
            this.Password_textBox.Size = new System.Drawing.Size(237, 24);
            this.Password_textBox.TabIndex = 2;
            this.Password_textBox.Text = "password";
            this.Password_textBox.WordWrap = false;
            // 
            // Version_label
            // 
            this.Version_label.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(0)))), ((int)(((byte)(30)))));
            this.Version_label.Font = new System.Drawing.Font("Rubik", 9F);
            this.Version_label.ForeColor = System.Drawing.Color.White;
            this.Version_label.Location = new System.Drawing.Point(459, 569);
            this.Version_label.Name = "Version_label";
            this.Version_label.Size = new System.Drawing.Size(473, 19);
            this.Version_label.TabIndex = 437;
            this.Version_label.Text = "Micro Projects 2018 - 2024  |  V";
            this.Version_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ShowPassword_button
            // 
            this.ShowPassword_button.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.ShowPassword_button.BackColor = System.Drawing.Color.White;
            this.ShowPassword_button.BackgroundImage = global::MyWorkApplication.Properties.Resources.login_eye00;
            this.ShowPassword_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ShowPassword_button.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.ShowPassword_button.FlatAppearance.BorderSize = 0;
            this.ShowPassword_button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
            this.ShowPassword_button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.ShowPassword_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ShowPassword_button.Location = new System.Drawing.Point(301, 434);
            this.ShowPassword_button.MaximumSize = new System.Drawing.Size(20, 20);
            this.ShowPassword_button.MinimumSize = new System.Drawing.Size(20, 20);
            this.ShowPassword_button.Name = "ShowPassword_button";
            this.ShowPassword_button.Size = new System.Drawing.Size(20, 20);
            this.ShowPassword_button.TabIndex = 438;
            this.ShowPassword_button.UseVisualStyleBackColor = false;
            this.ShowPassword_button.Click += new System.EventHandler(this.ShowPassword_button_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BackColor = System.Drawing.Color.Lime;
            this.pictureBox1.BackgroundImage = global::MyWorkApplication.Properties.Resources.login_backgroud1;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox1.Location = new System.Drawing.Point(15, 15);
            this.pictureBox1.MaximumSize = new System.Drawing.Size(1010, 650);
            this.pictureBox1.MinimumSize = new System.Drawing.Size(1010, 650);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1010, 650);
            this.pictureBox1.TabIndex = 439;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(0)))), ((int)(((byte)(30)))));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(459, 279);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(518, 236);
            this.label1.TabIndex = 440;
            this.label1.Text = resources.GetString("label1.Text");
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Copyright_label
            // 
            this.Copyright_label.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(0)))), ((int)(((byte)(30)))));
            this.Copyright_label.ForeColor = System.Drawing.Color.White;
            this.Copyright_label.Location = new System.Drawing.Point(459, 235);
            this.Copyright_label.Name = "Copyright_label";
            this.Copyright_label.Size = new System.Drawing.Size(518, 24);
            this.Copyright_label.TabIndex = 441;
            this.Copyright_label.Text = "© 2018 - 2024 CHRISTIAN HOPE CENTER. All Rights Reserved";
            this.Copyright_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ProfilePicture_pictureBox
            // 
            this.ProfilePicture_pictureBox.BackColor = System.Drawing.Color.White;
            this.ProfilePicture_pictureBox.BackgroundImage = global::MyWorkApplication.Properties.Resources.login_profile;
            this.ProfilePicture_pictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ProfilePicture_pictureBox.ErrorImage = null;
            this.ProfilePicture_pictureBox.InitialImage = null;
            this.ProfilePicture_pictureBox.Location = new System.Drawing.Point(83, 95);
            this.ProfilePicture_pictureBox.MaximumSize = new System.Drawing.Size(250, 251);
            this.ProfilePicture_pictureBox.MinimumSize = new System.Drawing.Size(250, 251);
            this.ProfilePicture_pictureBox.Name = "ProfilePicture_pictureBox";
            this.ProfilePicture_pictureBox.Size = new System.Drawing.Size(250, 251);
            this.ProfilePicture_pictureBox.TabIndex = 425;
            this.ProfilePicture_pictureBox.TabStop = false;
            // 
            // Login_Form
            // 
            this.AcceptButton = this.Connect_button;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Lime;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(1042, 680);
            this.ControlBox = false;
            this.Controls.Add(this.Copyright_label);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Close_button);
            this.Controls.Add(this.ShowPassword_button);
            this.Controls.Add(this.Version_label);
            this.Controls.Add(this.ProfilePicture_pictureBox);
            this.Controls.Add(this.Online_checkBox);
            this.Controls.Add(this.Remember_checkBox);
            this.Controls.Add(this.Connect_button);
            this.Controls.Add(this.Password_textBox);
            this.Controls.Add(this.UserName_textBox);
            this.Controls.Add(this.pictureBox1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Rubik", 10.8F);
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximumSize = new System.Drawing.Size(1060, 698);
            this.MinimumSize = new System.Drawing.Size(850, 550);
            this.Name = "Login_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TransparencyKey = System.Drawing.Color.Lime;
            this.Load += new System.EventHandler(this.Login_Form_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProfilePicture_pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button Connect_button;
        private System.Windows.Forms.CheckBox Remember_checkBox;
        private System.Windows.Forms.Button Close_button;
        private System.Windows.Forms.CheckBox Online_checkBox;
        private System.Windows.Forms.TextBox UserName_textBox;
        private OvalPictureBox ProfilePicture_pictureBox;
        private System.Windows.Forms.TextBox Password_textBox;
        private System.Windows.Forms.Label Version_label;
        private System.Windows.Forms.Button ShowPassword_button;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label Copyright_label;
    }
}