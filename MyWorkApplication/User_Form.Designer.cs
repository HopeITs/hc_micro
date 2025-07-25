namespace MyWorkApplication
{
    partial class User_Form
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(User_Form));
            this.DeleteImage_button = new System.Windows.Forms.Button();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.ProfilePicture_pictureBox = new OvalPictureBox();
            this.AddImage_button = new System.Windows.Forms.Button();
            this.Role_comboBox1 = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.Password_textBox = new System.Windows.Forms.TextBox();
            this.label48 = new System.Windows.Forms.Label();
            this.Username_textBox = new System.Windows.Forms.TextBox();
            this.Save_button = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label8 = new System.Windows.Forms.Label();
            this.tableLayoutPanel9 = new System.Windows.Forms.TableLayoutPanel();
            this.Header1_label = new System.Windows.Forms.Label();
            this.AllTasks_listBox = new System.Windows.Forms.ListBox();
            this.UserTasks_listBox = new System.Windows.Forms.ListBox();
            this.AddTask_button = new System.Windows.Forms.Button();
            this.RemoveTask_button = new System.Windows.Forms.Button();
            this.Header2_label = new System.Windows.Forms.Label();
            this.Refresh_button = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.Email_textBox = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.ShowPassword_button = new System.Windows.Forms.Button();
            this.Header3_label = new System.Windows.Forms.Label();
            this.users_dataGridView = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.Yes_ME_button = new System.Windows.Forms.Button();
            this.No_ME_button = new System.Windows.Forms.Button();
            this.No_V_button = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.Yes_V_button = new System.Windows.Forms.Button();
            this.Yes_Com_button = new System.Windows.Forms.Button();
            this.No_Com_button = new System.Windows.Forms.Button();
            this.No_Pay_button = new System.Windows.Forms.Button();
            this.Yes_Pay_button = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.refreshPageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ProfilePicture_pictureBox)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel9.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.users_dataGridView)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // DeleteImage_button
            // 
            this.DeleteImage_button.BackColor = System.Drawing.Color.Transparent;
            this.DeleteImage_button.BackgroundImage = global::MyWorkApplication.Properties.Resources.Trash_Ds;
            this.DeleteImage_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.DeleteImage_button.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.DeleteImage_button.FlatAppearance.BorderSize = 0;
            this.DeleteImage_button.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.DeleteImage_button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.DeleteImage_button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.DeleteImage_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DeleteImage_button.ForeColor = System.Drawing.Color.White;
            this.DeleteImage_button.Location = new System.Drawing.Point(83, 180);
            this.DeleteImage_button.Margin = new System.Windows.Forms.Padding(7, 0, 0, 0);
            this.DeleteImage_button.MaximumSize = new System.Drawing.Size(28, 34);
            this.DeleteImage_button.MinimumSize = new System.Drawing.Size(28, 34);
            this.DeleteImage_button.Name = "DeleteImage_button";
            this.DeleteImage_button.Size = new System.Drawing.Size(28, 34);
            this.DeleteImage_button.TabIndex = 6;
            this.DeleteImage_button.UseVisualStyleBackColor = false;
            this.DeleteImage_button.Click += new System.EventHandler(this.DeleteImage_button_Click);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 47.4359F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 52.5641F));
            this.tableLayoutPanel2.Controls.Add(this.ProfilePicture_pictureBox, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.AddImage_button, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.DeleteImage_button, 1, 1);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(26, 37);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel1.SetRowSpan(this.tableLayoutPanel2, 7);
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(162, 220);
            this.tableLayoutPanel2.TabIndex = 478;
            // 
            // ProfilePicture_pictureBox
            // 
            this.ProfilePicture_pictureBox.BackColor = System.Drawing.Color.Transparent;
            this.ProfilePicture_pictureBox.BackgroundImage = global::MyWorkApplication.Properties.Resources.Unknown_User;
            this.ProfilePicture_pictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.tableLayoutPanel2.SetColumnSpan(this.ProfilePicture_pictureBox, 2);
            this.ProfilePicture_pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ProfilePicture_pictureBox.Location = new System.Drawing.Point(3, 4);
            this.ProfilePicture_pictureBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ProfilePicture_pictureBox.Name = "ProfilePicture_pictureBox";
            this.ProfilePicture_pictureBox.Size = new System.Drawing.Size(156, 172);
            this.ProfilePicture_pictureBox.TabIndex = 426;
            this.ProfilePicture_pictureBox.TabStop = false;
            // 
            // AddImage_button
            // 
            this.AddImage_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.AddImage_button.BackColor = System.Drawing.Color.Transparent;
            this.AddImage_button.BackgroundImage = global::MyWorkApplication.Properties.Resources.Plus_Sq_D;
            this.AddImage_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.AddImage_button.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.AddImage_button.FlatAppearance.BorderSize = 0;
            this.AddImage_button.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.AddImage_button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.AddImage_button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.AddImage_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AddImage_button.ForeColor = System.Drawing.Color.White;
            this.AddImage_button.Location = new System.Drawing.Point(41, 180);
            this.AddImage_button.Margin = new System.Windows.Forms.Padding(0, 0, 7, 0);
            this.AddImage_button.MaximumSize = new System.Drawing.Size(28, 34);
            this.AddImage_button.MinimumSize = new System.Drawing.Size(28, 34);
            this.AddImage_button.Name = "AddImage_button";
            this.AddImage_button.Size = new System.Drawing.Size(28, 34);
            this.AddImage_button.TabIndex = 5;
            this.AddImage_button.UseVisualStyleBackColor = false;
            this.AddImage_button.Click += new System.EventHandler(this.AddImage_button_Click);
            // 
            // Role_comboBox1
            // 
            this.Role_comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.Role_comboBox1.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel1.SetColumnSpan(this.Role_comboBox1, 2);
            this.Role_comboBox1.FormattingEnabled = true;
            this.Role_comboBox1.Location = new System.Drawing.Point(439, 133);
            this.Role_comboBox1.Margin = new System.Windows.Forms.Padding(2);
            this.Role_comboBox1.Name = "Role_comboBox1";
            this.Role_comboBox1.Size = new System.Drawing.Size(251, 35);
            this.Role_comboBox1.TabIndex = 3;
            this.Role_comboBox1.TextChanged += new System.EventHandler(this.Role_comboBox1_TextChanged);
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(380, 131);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 27);
            this.label3.TabIndex = 485;
            this.label3.Text = "Role:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(334, 67);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 27);
            this.label2.TabIndex = 484;
            this.label2.Text = "Password:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Password_textBox
            // 
            this.Password_textBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.Password_textBox.BackColor = System.Drawing.Color.White;
            this.Password_textBox.ForeColor = System.Drawing.Color.Black;
            this.Password_textBox.Location = new System.Drawing.Point(2, 2);
            this.Password_textBox.Margin = new System.Windows.Forms.Padding(2);
            this.Password_textBox.Name = "Password_textBox";
            this.Password_textBox.PasswordChar = '*';
            this.Password_textBox.Size = new System.Drawing.Size(222, 29);
            this.Password_textBox.TabIndex = 2;
            // 
            // label48
            // 
            this.label48.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label48.AutoSize = true;
            this.label48.BackColor = System.Drawing.Color.Transparent;
            this.label48.ForeColor = System.Drawing.Color.Black;
            this.label48.Location = new System.Drawing.Point(330, 33);
            this.label48.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label48.Name = "label48";
            this.label48.Size = new System.Drawing.Size(103, 27);
            this.label48.TabIndex = 482;
            this.label48.Text = "Username:";
            this.label48.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Username_textBox
            // 
            this.Username_textBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.Username_textBox.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel1.SetColumnSpan(this.Username_textBox, 2);
            this.Username_textBox.ForeColor = System.Drawing.Color.Black;
            this.Username_textBox.Location = new System.Drawing.Point(439, 32);
            this.Username_textBox.Margin = new System.Windows.Forms.Padding(2);
            this.Username_textBox.Name = "Username_textBox";
            this.Username_textBox.Size = new System.Drawing.Size(251, 29);
            this.Username_textBox.TabIndex = 1;
            // 
            // Save_button
            // 
            this.Save_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.Save_button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
            this.Save_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Save_button.FlatAppearance.BorderColor = System.Drawing.Color.LightGray;
            this.Save_button.FlatAppearance.CheckedBackColor = System.Drawing.Color.Gray;
            this.Save_button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
            this.Save_button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gray;
            this.Save_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Save_button.ForeColor = System.Drawing.Color.White;
            this.Save_button.Location = new System.Drawing.Point(568, 334);
            this.Save_button.Margin = new System.Windows.Forms.Padding(2);
            this.Save_button.MaximumSize = new System.Drawing.Size(146, 39);
            this.Save_button.Name = "Save_button";
            this.Save_button.Size = new System.Drawing.Size(122, 33);
            this.Save_button.TabIndex = 15;
            this.Save_button.Text = "Save";
            this.Save_button.UseVisualStyleBackColor = false;
            this.Save_button.Click += new System.EventHandler(this.Save_button_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 9;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.27963F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 19.93522F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10.58289F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10.33678F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 3.60798F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 26.97785F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.27963F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tableLayoutPanel1.Controls.Add(this.label8, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel9, 1, 12);
            this.tableLayoutPanel1.Controls.Add(this.Refresh_button, 3, 10);
            this.tableLayoutPanel1.Controls.Add(this.label48, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.label2, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.label3, 2, 4);
            this.tableLayoutPanel1.Controls.Add(this.label7, 2, 5);
            this.tableLayoutPanel1.Controls.Add(this.Username_textBox, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.Role_comboBox1, 3, 4);
            this.tableLayoutPanel1.Controls.Add(this.Email_textBox, 3, 5);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.Save_button, 4, 10);
            this.tableLayoutPanel1.Controls.Add(this.Header3_label, 6, 1);
            this.tableLayoutPanel1.Controls.Add(this.users_dataGridView, 6, 2);
            this.tableLayoutPanel1.Controls.Add(this.label1, 2, 7);
            this.tableLayoutPanel1.Controls.Add(this.Yes_ME_button, 3, 7);
            this.tableLayoutPanel1.Controls.Add(this.No_ME_button, 4, 7);
            this.tableLayoutPanel1.Controls.Add(this.No_V_button, 4, 6);
            this.tableLayoutPanel1.Controls.Add(this.label5, 2, 6);
            this.tableLayoutPanel1.Controls.Add(this.Yes_V_button, 3, 6);
            this.tableLayoutPanel1.Controls.Add(this.Yes_Com_button, 3, 8);
            this.tableLayoutPanel1.Controls.Add(this.No_Com_button, 4, 8);
            this.tableLayoutPanel1.Controls.Add(this.No_Pay_button, 4, 9);
            this.tableLayoutPanel1.Controls.Add(this.Yes_Pay_button, 3, 9);
            this.tableLayoutPanel1.Controls.Add(this.label4, 1, 8);
            this.tableLayoutPanel1.Controls.Add(this.label6, 1, 9);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 14;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.048857F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.048857F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.048857F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.048857F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.048857F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.048857F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.048857F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.048857F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.748888F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 2.646623F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 42.21363F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1262, 653);
            this.tableLayoutPanel1.TabIndex = 488;
            // 
            // label8
            // 
            this.label8.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.SetColumnSpan(this.label8, 3);
            this.label8.Font = new System.Drawing.Font("Rubik", 8F);
            this.label8.ForeColor = System.Drawing.Color.Black;
            this.label8.Location = new System.Drawing.Point(342, 102);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(346, 22);
            this.label8.TabIndex = 486;
            this.label8.Text = "leave the passwoed empty if there isn\'t any changes";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel9
            // 
            this.tableLayoutPanel9.ColumnCount = 3;
            this.tableLayoutPanel1.SetColumnSpan(this.tableLayoutPanel9, 4);
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 77F));
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel9.Controls.Add(this.Header1_label, 0, 0);
            this.tableLayoutPanel9.Controls.Add(this.AllTasks_listBox, 0, 1);
            this.tableLayoutPanel9.Controls.Add(this.UserTasks_listBox, 2, 1);
            this.tableLayoutPanel9.Controls.Add(this.AddTask_button, 1, 1);
            this.tableLayoutPanel9.Controls.Add(this.RemoveTask_button, 1, 2);
            this.tableLayoutPanel9.Controls.Add(this.Header2_label, 2, 0);
            this.tableLayoutPanel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel9.Location = new System.Drawing.Point(21, 384);
            this.tableLayoutPanel9.Margin = new System.Windows.Forms.Padding(1);
            this.tableLayoutPanel9.Name = "tableLayoutPanel9";
            this.tableLayoutPanel9.RowCount = 3;
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel9.Size = new System.Drawing.Size(670, 235);
            this.tableLayoutPanel9.TabIndex = 491;
            // 
            // Header1_label
            // 
            this.Header1_label.AutoSize = true;
            this.Header1_label.BackColor = System.Drawing.Color.Transparent;
            this.Header1_label.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Header1_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Header1_label.ForeColor = System.Drawing.Color.Black;
            this.Header1_label.Location = new System.Drawing.Point(4, 0);
            this.Header1_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Header1_label.Name = "Header1_label";
            this.Header1_label.Padding = new System.Windows.Forms.Padding(1);
            this.Header1_label.Size = new System.Drawing.Size(288, 48);
            this.Header1_label.TabIndex = 496;
            this.Header1_label.Text = "All Tasks";
            this.Header1_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // AllTasks_listBox
            // 
            this.AllTasks_listBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AllTasks_listBox.ForeColor = System.Drawing.Color.Black;
            this.AllTasks_listBox.FormattingEnabled = true;
            this.AllTasks_listBox.ItemHeight = 27;
            this.AllTasks_listBox.Location = new System.Drawing.Point(3, 52);
            this.AllTasks_listBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.AllTasks_listBox.Name = "AllTasks_listBox";
            this.tableLayoutPanel9.SetRowSpan(this.AllTasks_listBox, 2);
            this.AllTasks_listBox.Size = new System.Drawing.Size(290, 179);
            this.AllTasks_listBox.TabIndex = 0;
            // 
            // UserTasks_listBox
            // 
            this.UserTasks_listBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.UserTasks_listBox.ForeColor = System.Drawing.Color.Black;
            this.UserTasks_listBox.FormattingEnabled = true;
            this.UserTasks_listBox.ItemHeight = 27;
            this.UserTasks_listBox.Location = new System.Drawing.Point(376, 52);
            this.UserTasks_listBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.UserTasks_listBox.Name = "UserTasks_listBox";
            this.tableLayoutPanel9.SetRowSpan(this.UserTasks_listBox, 2);
            this.UserTasks_listBox.Size = new System.Drawing.Size(291, 179);
            this.UserTasks_listBox.TabIndex = 1;
            // 
            // AddTask_button
            // 
            this.AddTask_button.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AddTask_button.BackColor = System.Drawing.Color.Transparent;
            this.AddTask_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.AddTask_button.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.AddTask_button.FlatAppearance.CheckedBackColor = System.Drawing.Color.Silver;
            this.AddTask_button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Silver;
            this.AddTask_button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.AddTask_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AddTask_button.ForeColor = System.Drawing.Color.Black;
            this.AddTask_button.Location = new System.Drawing.Point(298, 97);
            this.AddTask_button.Margin = new System.Windows.Forms.Padding(2);
            this.AddTask_button.MinimumSize = new System.Drawing.Size(70, 42);
            this.AddTask_button.Name = "AddTask_button";
            this.AddTask_button.Size = new System.Drawing.Size(73, 42);
            this.AddTask_button.TabIndex = 488;
            this.AddTask_button.Text = ">";
            this.AddTask_button.UseVisualStyleBackColor = false;
            this.AddTask_button.Click += new System.EventHandler(this.AddTask_button_Click);
            // 
            // RemoveTask_button
            // 
            this.RemoveTask_button.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RemoveTask_button.BackColor = System.Drawing.Color.Transparent;
            this.RemoveTask_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.RemoveTask_button.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.RemoveTask_button.FlatAppearance.CheckedBackColor = System.Drawing.Color.Silver;
            this.RemoveTask_button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Silver;
            this.RemoveTask_button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.RemoveTask_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RemoveTask_button.ForeColor = System.Drawing.Color.Black;
            this.RemoveTask_button.Location = new System.Drawing.Point(298, 143);
            this.RemoveTask_button.Margin = new System.Windows.Forms.Padding(2);
            this.RemoveTask_button.MinimumSize = new System.Drawing.Size(70, 42);
            this.RemoveTask_button.Name = "RemoveTask_button";
            this.RemoveTask_button.Size = new System.Drawing.Size(73, 42);
            this.RemoveTask_button.TabIndex = 489;
            this.RemoveTask_button.Text = "<";
            this.RemoveTask_button.UseVisualStyleBackColor = false;
            this.RemoveTask_button.Click += new System.EventHandler(this.RemoveTask_button_Click);
            // 
            // Header2_label
            // 
            this.Header2_label.AutoSize = true;
            this.Header2_label.BackColor = System.Drawing.Color.Transparent;
            this.Header2_label.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Header2_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Header2_label.ForeColor = System.Drawing.Color.Black;
            this.Header2_label.Location = new System.Drawing.Point(377, 0);
            this.Header2_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Header2_label.Name = "Header2_label";
            this.Header2_label.Padding = new System.Windows.Forms.Padding(1);
            this.Header2_label.Size = new System.Drawing.Size(289, 48);
            this.Header2_label.TabIndex = 497;
            this.Header2_label.Text = "User Tasks";
            this.Header2_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Refresh_button
            // 
            this.Refresh_button.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.Refresh_button.BackgroundImage = global::MyWorkApplication.Properties.Resources.Refresh2_D;
            this.Refresh_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Refresh_button.FlatAppearance.BorderSize = 0;
            this.Refresh_button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.Refresh_button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.Refresh_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Refresh_button.Location = new System.Drawing.Point(530, 336);
            this.Refresh_button.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Refresh_button.MaximumSize = new System.Drawing.Size(33, 35);
            this.Refresh_button.MinimumSize = new System.Drawing.Size(33, 35);
            this.Refresh_button.Name = "Refresh_button";
            this.Refresh_button.Size = new System.Drawing.Size(33, 35);
            this.Refresh_button.TabIndex = 16;
            this.Refresh_button.UseVisualStyleBackColor = true;
            this.Refresh_button.Click += new System.EventHandler(this.Refresh_button_Click);
            this.Refresh_button.MouseEnter += new System.EventHandler(this.Refresh_button_MouseEnter);
            this.Refresh_button.MouseLeave += new System.EventHandler(this.Refresh_button_MouseLeave);
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.Location = new System.Drawing.Point(371, 165);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(62, 27);
            this.label7.TabIndex = 501;
            this.label7.Text = "Email:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Email_textBox
            // 
            this.Email_textBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.Email_textBox.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel1.SetColumnSpan(this.Email_textBox, 2);
            this.Email_textBox.ForeColor = System.Drawing.Color.Black;
            this.Email_textBox.Location = new System.Drawing.Point(439, 164);
            this.Email_textBox.Margin = new System.Windows.Forms.Padding(2);
            this.Email_textBox.Name = "Email_textBox";
            this.Email_textBox.Size = new System.Drawing.Size(251, 29);
            this.Email_textBox.TabIndex = 4;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel1.SetColumnSpan(this.tableLayoutPanel3, 2);
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.tableLayoutPanel3.Controls.Add(this.ShowPassword_button, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.Password_textBox, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(437, 64);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(255, 34);
            this.tableLayoutPanel3.TabIndex = 2;
            this.tableLayoutPanel3.TabStop = true;
            // 
            // ShowPassword_button
            // 
            this.ShowPassword_button.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.ShowPassword_button.BackgroundImage = global::MyWorkApplication.Properties.Resources.show_password_false;
            this.ShowPassword_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ShowPassword_button.FlatAppearance.BorderSize = 0;
            this.ShowPassword_button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.ShowPassword_button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.ShowPassword_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ShowPassword_button.Location = new System.Drawing.Point(230, 4);
            this.ShowPassword_button.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ShowPassword_button.MaximumSize = new System.Drawing.Size(22, 27);
            this.ShowPassword_button.MinimumSize = new System.Drawing.Size(22, 27);
            this.ShowPassword_button.Name = "ShowPassword_button";
            this.ShowPassword_button.Size = new System.Drawing.Size(22, 27);
            this.ShowPassword_button.TabIndex = 17;
            this.ShowPassword_button.TabStop = false;
            this.ShowPassword_button.UseVisualStyleBackColor = true;
            this.ShowPassword_button.Click += new System.EventHandler(this.ShowPassword_button_Click);
            // 
            // Header3_label
            // 
            this.Header3_label.AutoSize = true;
            this.Header3_label.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
            this.Header3_label.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.Header3_label, 2);
            this.Header3_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Header3_label.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.Header3_label.Location = new System.Drawing.Point(737, 30);
            this.Header3_label.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Header3_label.Name = "Header3_label";
            this.Header3_label.Size = new System.Drawing.Size(498, 34);
            this.Header3_label.TabIndex = 496;
            this.Header3_label.Text = "ALL USERS";
            this.Header3_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // users_dataGridView
            // 
            this.users_dataGridView.AllowUserToAddRows = false;
            this.users_dataGridView.AllowUserToDeleteRows = false;
            this.users_dataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.users_dataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.users_dataGridView.BackgroundColor = System.Drawing.Color.White;
            this.users_dataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.users_dataGridView.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.users_dataGridView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Rubik", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(3);
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.users_dataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.users_dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tableLayoutPanel1.SetColumnSpan(this.users_dataGridView, 2);
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Rubik", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.users_dataGridView.DefaultCellStyle = dataGridViewCellStyle2;
            this.users_dataGridView.EnableHeadersVisualStyles = false;
            this.users_dataGridView.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.users_dataGridView.Location = new System.Drawing.Point(739, 70);
            this.users_dataGridView.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.users_dataGridView.Name = "users_dataGridView";
            this.users_dataGridView.ReadOnly = true;
            this.users_dataGridView.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.users_dataGridView.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Rubik", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.users_dataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.users_dataGridView.RowHeadersWidth = 40;
            this.users_dataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Rubik", 10F);
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.users_dataGridView.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.tableLayoutPanel1.SetRowSpan(this.users_dataGridView, 11);
            this.users_dataGridView.RowTemplate.Height = 33;
            this.users_dataGridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.users_dataGridView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.users_dataGridView.Size = new System.Drawing.Size(494, 544);
            this.users_dataGridView.TabIndex = 497;
            this.users_dataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.users_dataGridView_CellContentClick);
            this.users_dataGridView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.users_dataGridView_CellContentClick);
            this.users_dataGridView.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.users_dataGridView_CellPainting);
            this.users_dataGridView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.users_dataGridView_DataError);
            this.users_dataGridView.RowHeaderMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.users_dataGridView_CellMouseDoubleClick);
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(232, 233);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(201, 27);
            this.label1.TabIndex = 491;
            this.label1.Text = "Get M&&E Notifications?";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Yes_ME_button
            // 
            this.Yes_ME_button.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Yes_ME_button.BackColor = System.Drawing.Color.Transparent;
            this.Yes_ME_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Yes_ME_button.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.Yes_ME_button.FlatAppearance.CheckedBackColor = System.Drawing.Color.Silver;
            this.Yes_ME_button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Silver;
            this.Yes_ME_button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.Yes_ME_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Yes_ME_button.ForeColor = System.Drawing.Color.Black;
            this.Yes_ME_button.Location = new System.Drawing.Point(439, 232);
            this.Yes_ME_button.Margin = new System.Windows.Forms.Padding(2);
            this.Yes_ME_button.Name = "Yes_ME_button";
            this.Yes_ME_button.Size = new System.Drawing.Size(125, 30);
            this.Yes_ME_button.TabIndex = 5;
            this.Yes_ME_button.Text = " Yes";
            this.Yes_ME_button.UseVisualStyleBackColor = false;
            this.Yes_ME_button.Click += new System.EventHandler(this.Yes_ME_button_Click);
            // 
            // No_ME_button
            // 
            this.No_ME_button.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.No_ME_button.BackColor = System.Drawing.Color.Transparent;
            this.No_ME_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.No_ME_button.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.No_ME_button.FlatAppearance.CheckedBackColor = System.Drawing.Color.Silver;
            this.No_ME_button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Silver;
            this.No_ME_button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.No_ME_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.No_ME_button.ForeColor = System.Drawing.Color.Black;
            this.No_ME_button.Location = new System.Drawing.Point(568, 232);
            this.No_ME_button.Margin = new System.Windows.Forms.Padding(2);
            this.No_ME_button.Name = "No_ME_button";
            this.No_ME_button.Size = new System.Drawing.Size(122, 30);
            this.No_ME_button.TabIndex = 6;
            this.No_ME_button.Text = "No";
            this.No_ME_button.UseVisualStyleBackColor = false;
            this.No_ME_button.Click += new System.EventHandler(this.No_ME_button_Click);
            // 
            // No_V_button
            // 
            this.No_V_button.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.No_V_button.BackColor = System.Drawing.Color.Transparent;
            this.No_V_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.No_V_button.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.No_V_button.FlatAppearance.CheckedBackColor = System.Drawing.Color.Silver;
            this.No_V_button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Silver;
            this.No_V_button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.No_V_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.No_V_button.ForeColor = System.Drawing.Color.Black;
            this.No_V_button.Location = new System.Drawing.Point(568, 198);
            this.No_V_button.Margin = new System.Windows.Forms.Padding(2);
            this.No_V_button.Name = "No_V_button";
            this.No_V_button.Size = new System.Drawing.Size(122, 30);
            this.No_V_button.TabIndex = 12;
            this.No_V_button.Text = "No";
            this.No_V_button.UseVisualStyleBackColor = false;
            this.No_V_button.Click += new System.EventHandler(this.No_V_button_Click);
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(340, 199);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(93, 27);
            this.label5.TabIndex = 495;
            this.label5.Text = "Is Visitor?";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Yes_V_button
            // 
            this.Yes_V_button.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Yes_V_button.BackColor = System.Drawing.Color.Transparent;
            this.Yes_V_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Yes_V_button.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.Yes_V_button.FlatAppearance.CheckedBackColor = System.Drawing.Color.Silver;
            this.Yes_V_button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Silver;
            this.Yes_V_button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.Yes_V_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Yes_V_button.ForeColor = System.Drawing.Color.Black;
            this.Yes_V_button.Location = new System.Drawing.Point(439, 198);
            this.Yes_V_button.Margin = new System.Windows.Forms.Padding(2);
            this.Yes_V_button.Name = "Yes_V_button";
            this.Yes_V_button.Size = new System.Drawing.Size(125, 30);
            this.Yes_V_button.TabIndex = 11;
            this.Yes_V_button.Text = " Yes";
            this.Yes_V_button.UseVisualStyleBackColor = false;
            this.Yes_V_button.Click += new System.EventHandler(this.Yes_V_button_Click);
            // 
            // Yes_Com_button
            // 
            this.Yes_Com_button.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Yes_Com_button.BackColor = System.Drawing.Color.Transparent;
            this.Yes_Com_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Yes_Com_button.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.Yes_Com_button.FlatAppearance.CheckedBackColor = System.Drawing.Color.Silver;
            this.Yes_Com_button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Silver;
            this.Yes_Com_button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.Yes_Com_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Yes_Com_button.ForeColor = System.Drawing.Color.Black;
            this.Yes_Com_button.Location = new System.Drawing.Point(439, 266);
            this.Yes_Com_button.Margin = new System.Windows.Forms.Padding(2);
            this.Yes_Com_button.Name = "Yes_Com_button";
            this.Yes_Com_button.Size = new System.Drawing.Size(125, 30);
            this.Yes_Com_button.TabIndex = 7;
            this.Yes_Com_button.Text = " Yes";
            this.Yes_Com_button.UseVisualStyleBackColor = false;
            this.Yes_Com_button.Click += new System.EventHandler(this.Yes_Com_button_Click);
            // 
            // No_Com_button
            // 
            this.No_Com_button.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.No_Com_button.BackColor = System.Drawing.Color.Transparent;
            this.No_Com_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.No_Com_button.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.No_Com_button.FlatAppearance.CheckedBackColor = System.Drawing.Color.Silver;
            this.No_Com_button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Silver;
            this.No_Com_button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.No_Com_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.No_Com_button.ForeColor = System.Drawing.Color.Black;
            this.No_Com_button.Location = new System.Drawing.Point(568, 266);
            this.No_Com_button.Margin = new System.Windows.Forms.Padding(2);
            this.No_Com_button.Name = "No_Com_button";
            this.No_Com_button.Size = new System.Drawing.Size(122, 30);
            this.No_Com_button.TabIndex = 8;
            this.No_Com_button.Text = "No";
            this.No_Com_button.UseVisualStyleBackColor = false;
            this.No_Com_button.Click += new System.EventHandler(this.No_Com_button_Click);
            // 
            // No_Pay_button
            // 
            this.No_Pay_button.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.No_Pay_button.BackColor = System.Drawing.Color.Transparent;
            this.No_Pay_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.No_Pay_button.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.No_Pay_button.FlatAppearance.CheckedBackColor = System.Drawing.Color.Silver;
            this.No_Pay_button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Silver;
            this.No_Pay_button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.No_Pay_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.No_Pay_button.ForeColor = System.Drawing.Color.Black;
            this.No_Pay_button.Location = new System.Drawing.Point(568, 300);
            this.No_Pay_button.Margin = new System.Windows.Forms.Padding(2);
            this.No_Pay_button.Name = "No_Pay_button";
            this.No_Pay_button.Size = new System.Drawing.Size(122, 30);
            this.No_Pay_button.TabIndex = 10;
            this.No_Pay_button.Text = "No";
            this.No_Pay_button.UseVisualStyleBackColor = false;
            this.No_Pay_button.Click += new System.EventHandler(this.No_Pay_button_Click);
            // 
            // Yes_Pay_button
            // 
            this.Yes_Pay_button.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Yes_Pay_button.BackColor = System.Drawing.Color.Transparent;
            this.Yes_Pay_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Yes_Pay_button.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.Yes_Pay_button.FlatAppearance.CheckedBackColor = System.Drawing.Color.Silver;
            this.Yes_Pay_button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Silver;
            this.Yes_Pay_button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.Yes_Pay_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Yes_Pay_button.ForeColor = System.Drawing.Color.Black;
            this.Yes_Pay_button.Location = new System.Drawing.Point(439, 300);
            this.Yes_Pay_button.Margin = new System.Windows.Forms.Padding(2);
            this.Yes_Pay_button.Name = "Yes_Pay_button";
            this.Yes_Pay_button.Size = new System.Drawing.Size(125, 30);
            this.Yes_Pay_button.TabIndex = 9;
            this.Yes_Pay_button.Text = " Yes";
            this.Yes_Pay_button.UseVisualStyleBackColor = false;
            this.Yes_Pay_button.Click += new System.EventHandler(this.Yes_Pay_button_Click);
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.SetColumnSpan(this.label4, 2);
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(137, 267);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(296, 27);
            this.label4.TabIndex = 492;
            this.label4.Text = "Get Communication Notifications?";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.SetColumnSpan(this.label6, 2);
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(110, 301);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(323, 27);
            this.label6.TabIndex = 497;
            this.label6.Text = "Get Pending Payments Notifications?";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.refreshPageToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(164, 28);
            // 
            // refreshPageToolStripMenuItem
            // 
            this.refreshPageToolStripMenuItem.Name = "refreshPageToolStripMenuItem";
            this.refreshPageToolStripMenuItem.Size = new System.Drawing.Size(163, 24);
            this.refreshPageToolStripMenuItem.Text = "Refresh Page";
            this.refreshPageToolStripMenuItem.Click += new System.EventHandler(this.Refresh_button_Click);
            // 
            // User_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 27F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1262, 653);
            this.Controls.Add(this.tableLayoutPanel1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Rubik", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.Black;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.Name = "User_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.User_Form_Load);
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ProfilePicture_pictureBox)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel9.ResumeLayout(false);
            this.tableLayoutPanel9.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.users_dataGridView)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button DeleteImage_button;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button AddImage_button;
        private System.Windows.Forms.ComboBox Role_comboBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox Password_textBox;
        private System.Windows.Forms.Label label48;
        private System.Windows.Forms.TextBox Username_textBox;
        private System.Windows.Forms.Button Save_button;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button Yes_ME_button;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button No_ME_button;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button No_Com_button;
        private System.Windows.Forms.Button Yes_Com_button;
        private System.Windows.Forms.Button No_V_button;
        private System.Windows.Forms.Button Yes_V_button;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label Header3_label;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel9;
        private System.Windows.Forms.ListBox AllTasks_listBox;
        private System.Windows.Forms.ListBox UserTasks_listBox;
        private System.Windows.Forms.Button AddTask_button;
        private System.Windows.Forms.Button RemoveTask_button;
        private System.Windows.Forms.Label Header1_label;
        private System.Windows.Forms.Label Header2_label;
        private System.Windows.Forms.DataGridView users_dataGridView;
        private OvalPictureBox ProfilePicture_pictureBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button Yes_Pay_button;
        private System.Windows.Forms.Button No_Pay_button;
        private System.Windows.Forms.Button Refresh_button;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem refreshPageToolStripMenuItem;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox Email_textBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button ShowPassword_button;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label8;
    }
}