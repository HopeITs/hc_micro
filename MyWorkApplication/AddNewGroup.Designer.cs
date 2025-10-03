namespace MyWorkApplication
{
    partial class AddNewGroup
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddNewGroup));
            this.Save_button = new System.Windows.Forms.Button();
            this.label48 = new System.Windows.Forms.Label();
            this.Name_textBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.Close_button = new System.Windows.Forms.Button();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.group_dataGridView = new System.Windows.Forms.DataGridView();
            this.Donor_comboBox = new System.Windows.Forms.ComboBox();
            this.Donor_label = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.ExportToExcel_button = new System.Windows.Forms.Button();
            this.Count_label = new System.Windows.Forms.Label();
            this.Selected_label = new System.Windows.Forms.Label();
            this.tableLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.group_dataGridView)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // Save_button
            // 
            this.Save_button.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.Save_button.BackColor = System.Drawing.Color.Transparent;
            this.Save_button.BackgroundImage = global::MyWorkApplication.Properties.Resources.Save_CD;
            this.Save_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Save_button.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.Save_button.FlatAppearance.BorderSize = 0;
            this.Save_button.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.Save_button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.Save_button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.Save_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Save_button.ForeColor = System.Drawing.Color.White;
            this.Save_button.Location = new System.Drawing.Point(200, 170);
            this.Save_button.Margin = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.Save_button.Name = "Save_button";
            this.Save_button.Size = new System.Drawing.Size(99, 36);
            this.Save_button.TabIndex = 3;
            this.Save_button.UseVisualStyleBackColor = false;
            this.Save_button.Click += new System.EventHandler(this.Save_button_Click);
            this.Save_button.MouseEnter += new System.EventHandler(this.Add_button_MouseEnter);
            this.Save_button.MouseLeave += new System.EventHandler(this.Add_button_MouseLeave);
            // 
            // label48
            // 
            this.label48.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label48.AutoSize = true;
            this.label48.BackColor = System.Drawing.Color.Transparent;
            this.label48.ForeColor = System.Drawing.Color.Black;
            this.label48.Location = new System.Drawing.Point(338, 84);
            this.label48.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label48.Name = "label48";
            this.label48.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label48.Size = new System.Drawing.Size(127, 27);
            this.label48.TabIndex = 461;
            this.label48.Text = "اسم المجموعة:";
            this.label48.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Name_textBox
            // 
            this.Name_textBox.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.Name_textBox.BackColor = System.Drawing.Color.White;
            this.Name_textBox.ForeColor = System.Drawing.Color.Black;
            this.Name_textBox.Location = new System.Drawing.Point(49, 81);
            this.Name_textBox.Margin = new System.Windows.Forms.Padding(4, 7, 4, 7);
            this.Name_textBox.Name = "Name_textBox";
            this.Name_textBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Name_textBox.Size = new System.Drawing.Size(275, 29);
            this.Name_textBox.TabIndex = 1;
            this.Name_textBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Name_textBox.TextChanged += new System.EventHandler(this.Name_textBox_TextChanged);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Rubik", 14F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(144, 17);
            this.label2.Margin = new System.Windows.Forms.Padding(0);
            this.label2.Name = "label2";
            this.label2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label2.Size = new System.Drawing.Size(103, 37);
            this.label2.TabIndex = 450;
            this.label2.Text = "مجموعة";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Rubik", 14F);
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(247, 17);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label1.Size = new System.Drawing.Size(152, 37);
            this.label1.TabIndex = 449;
            this.label1.Text = "إضافة / حذف";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Close_button
            // 
            this.Close_button.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.Close_button.BackColor = System.Drawing.Color.Transparent;
            this.Close_button.BackgroundImage = global::MyWorkApplication.Properties.Resources.Exit_D;
            this.Close_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Close_button.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.Close_button.FlatAppearance.BorderSize = 0;
            this.Close_button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.Close_button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.Close_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Close_button.Font = new System.Drawing.Font("Avenir LT Std 65 Medium", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Close_button.Location = new System.Drawing.Point(473, 9);
            this.Close_button.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Close_button.Name = "Close_button";
            this.Close_button.Size = new System.Drawing.Size(20, 36);
            this.Close_button.TabIndex = 448;
            this.Close_button.UseVisualStyleBackColor = false;
            this.Close_button.Click += new System.EventHandler(this.Close_button_Click);
            this.Close_button.MouseEnter += new System.EventHandler(this.Close_button_MouseEnter);
            this.Close_button.MouseLeave += new System.EventHandler(this.Close_button_MouseLeave);
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel4.ColumnCount = 4;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 53.68732F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 46.31268F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 53F));
            this.tableLayoutPanel4.Controls.Add(this.label2, 2, 0);
            this.tableLayoutPanel4.Controls.Add(this.label1, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.Close_button, 0, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(506, 54);
            this.tableLayoutPanel4.TabIndex = 464;
            // 
            // group_dataGridView
            // 
            this.group_dataGridView.AllowUserToAddRows = false;
            this.group_dataGridView.AllowUserToDeleteRows = false;
            this.group_dataGridView.AllowUserToOrderColumns = true;
            this.group_dataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.group_dataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.group_dataGridView.BackgroundColor = System.Drawing.Color.White;
            this.group_dataGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.group_dataGridView.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.group_dataGridView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Rubik", 10.8F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(216)))), ((int)(((byte)(214)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.group_dataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.group_dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Rubik", 10.8F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(217)))), ((int)(((byte)(214)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.group_dataGridView.DefaultCellStyle = dataGridViewCellStyle2;
            this.group_dataGridView.EnableHeadersVisualStyles = false;
            this.group_dataGridView.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.group_dataGridView.Location = new System.Drawing.Point(13, 214);
            this.group_dataGridView.Margin = new System.Windows.Forms.Padding(4);
            this.group_dataGridView.Name = "group_dataGridView";
            this.group_dataGridView.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.group_dataGridView.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Rubik", 10.8F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(216)))), ((int)(((byte)(214)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.group_dataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.group_dataGridView.RowHeadersWidth = 51;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(216)))), ((int)(((byte)(214)))));
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.group_dataGridView.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.group_dataGridView.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            this.group_dataGridView.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            this.group_dataGridView.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(216)))), ((int)(((byte)(214)))));
            this.group_dataGridView.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.group_dataGridView.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.group_dataGridView.RowTemplate.Height = 30;
            this.group_dataGridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.group_dataGridView.Size = new System.Drawing.Size(478, 419);
            this.group_dataGridView.TabIndex = 466;
            this.group_dataGridView.VirtualMode = true;
            this.group_dataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.group_dataGridView_CellContentClick);
            this.group_dataGridView.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.group_dataGridView_CellEndEdit);
            this.group_dataGridView.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.group_dataGridView_CellPainting);
            this.group_dataGridView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.group_dataGridView_DataError);
            this.group_dataGridView.SelectionChanged += new System.EventHandler(this.group_dataGridView_SelectionChanged);
            // 
            // Donor_comboBox
            // 
            this.Donor_comboBox.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.Donor_comboBox.BackColor = System.Drawing.Color.White;
            this.Donor_comboBox.ForeColor = System.Drawing.Color.Black;
            this.Donor_comboBox.FormattingEnabled = true;
            this.Donor_comboBox.Location = new System.Drawing.Point(49, 121);
            this.Donor_comboBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Donor_comboBox.Name = "Donor_comboBox";
            this.Donor_comboBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Donor_comboBox.Size = new System.Drawing.Size(275, 35);
            this.Donor_comboBox.TabIndex = 469;
            // 
            // Donor_label
            // 
            this.Donor_label.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.Donor_label.AutoSize = true;
            this.Donor_label.BackColor = System.Drawing.Color.Transparent;
            this.Donor_label.ForeColor = System.Drawing.Color.Black;
            this.Donor_label.Location = new System.Drawing.Point(345, 124);
            this.Donor_label.Margin = new System.Windows.Forms.Padding(0);
            this.Donor_label.Name = "Donor_label";
            this.Donor_label.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Donor_label.Size = new System.Drawing.Size(119, 27);
            this.Donor_label.TabIndex = 470;
            this.Donor_label.Text = "الجهة الممولة:";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.ExportToExcel_button, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.Count_label, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.Selected_label, 2, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 650);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(506, 74);
            this.tableLayoutPanel2.TabIndex = 483;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel2.SetColumnSpan(this.label3, 4);
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.label3.Location = new System.Drawing.Point(0, 5);
            this.label3.Margin = new System.Windows.Forms.Padding(0);
            this.label3.Name = "label3";
            this.label3.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label3.Size = new System.Drawing.Size(506, 27);
            this.label3.TabIndex = 479;
            this.label3.Text = "يمكن التعديل على المهنة مباشرة من الجدول...";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ExportToExcel_button
            // 
            this.ExportToExcel_button.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.ExportToExcel_button.AutoSize = true;
            this.ExportToExcel_button.BackColor = System.Drawing.Color.Transparent;
            this.ExportToExcel_button.BackgroundImage = global::MyWorkApplication.Properties.Resources.Excel_D;
            this.ExportToExcel_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ExportToExcel_button.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.ExportToExcel_button.FlatAppearance.BorderSize = 0;
            this.ExportToExcel_button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.ExportToExcel_button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.ExportToExcel_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ExportToExcel_button.ForeColor = System.Drawing.Color.White;
            this.ExportToExcel_button.Location = new System.Drawing.Point(460, 41);
            this.ExportToExcel_button.Margin = new System.Windows.Forms.Padding(4);
            this.ExportToExcel_button.MaximumSize = new System.Drawing.Size(28, 31);
            this.ExportToExcel_button.MinimumSize = new System.Drawing.Size(28, 31);
            this.ExportToExcel_button.Name = "ExportToExcel_button";
            this.ExportToExcel_button.Size = new System.Drawing.Size(28, 31);
            this.ExportToExcel_button.TabIndex = 411;
            this.ExportToExcel_button.UseVisualStyleBackColor = false;
            this.ExportToExcel_button.Click += new System.EventHandler(this.ExportToExcel_button_Click);
            // 
            // Count_label
            // 
            this.Count_label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.Count_label.AutoSize = true;
            this.Count_label.BackColor = System.Drawing.Color.Transparent;
            this.Count_label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.Count_label.Location = new System.Drawing.Point(304, 42);
            this.Count_label.Margin = new System.Windows.Forms.Padding(0);
            this.Count_label.Name = "Count_label";
            this.Count_label.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Count_label.Size = new System.Drawing.Size(152, 27);
            this.Count_label.TabIndex = 477;
            this.Count_label.Text = "Count";
            this.Count_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Selected_label
            // 
            this.Selected_label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.Selected_label.AutoSize = true;
            this.Selected_label.BackColor = System.Drawing.Color.Transparent;
            this.Selected_label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.Selected_label.Location = new System.Drawing.Point(152, 42);
            this.Selected_label.Margin = new System.Windows.Forms.Padding(0);
            this.Selected_label.Name = "Selected_label";
            this.Selected_label.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Selected_label.Size = new System.Drawing.Size(152, 27);
            this.Selected_label.TabIndex = 478;
            this.Selected_label.Text = "Selected";
            this.Selected_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // AddNewGroup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 27F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.BackgroundImage = global::MyWorkApplication.Properties.Resources.Background_Border;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(506, 724);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.Donor_comboBox);
            this.Controls.Add(this.Donor_label);
            this.Controls.Add(this.group_dataGridView);
            this.Controls.Add(this.Save_button);
            this.Controls.Add(this.label48);
            this.Controls.Add(this.Name_textBox);
            this.Controls.Add(this.tableLayoutPanel4);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Rubik", 10.8F);
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "AddNewGroup";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.AddNewGroup_Load);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.group_dataGridView)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button Save_button;
        private System.Windows.Forms.Label label48;
        private System.Windows.Forms.TextBox Name_textBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Close_button;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.DataGridView group_dataGridView;
        private System.Windows.Forms.ComboBox Donor_comboBox;
        private System.Windows.Forms.Label Donor_label;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button ExportToExcel_button;
        private System.Windows.Forms.Label Count_label;
        private System.Windows.Forms.Label Selected_label;
    }
}