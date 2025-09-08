namespace MyWorkApplication
{
    partial class AddNewChecklistMember
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddNewChecklistMember));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.Save_button = new System.Windows.Forms.Button();
            this.Name_textBox = new System.Windows.Forms.TextBox();
            this.Type_lable = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.Close_button = new System.Windows.Forms.Button();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.ExportToExcel_button = new System.Windows.Forms.Button();
            this.Count_label = new System.Windows.Forms.Label();
            this.Selected_label = new System.Windows.Forms.Label();
            this.Name_lable = new System.Windows.Forms.Label();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.label4 = new System.Windows.Forms.Label();
            this.Position_comboBox = new System.Windows.Forms.ComboBox();
            this.AddPosition_button = new System.Windows.Forms.Button();
            this.Member_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Member_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Position_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Position_Name = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Position_Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isActive = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.DeleteRow = new System.Windows.Forms.DataGridViewButtonColumn();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // Save_button
            // 
            this.Save_button.Anchor = System.Windows.Forms.AnchorStyles.Top;
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
            this.Save_button.Location = new System.Drawing.Point(180, 148);
            this.Save_button.Margin = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.Save_button.Name = "Save_button";
            this.Save_button.Size = new System.Drawing.Size(100, 36);
            this.Save_button.TabIndex = 488;
            this.Save_button.UseVisualStyleBackColor = false;
            this.Save_button.Click += new System.EventHandler(this.Save_button_Click);
            // 
            // Name_textBox
            // 
            this.Name_textBox.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.Name_textBox.BackColor = System.Drawing.Color.White;
            this.Name_textBox.ForeColor = System.Drawing.Color.Black;
            this.Name_textBox.Location = new System.Drawing.Point(53, 64);
            this.Name_textBox.Margin = new System.Windows.Forms.Padding(0);
            this.Name_textBox.Name = "Name_textBox";
            this.Name_textBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Name_textBox.Size = new System.Drawing.Size(296, 29);
            this.Name_textBox.TabIndex = 487;
            // 
            // Type_lable
            // 
            this.Type_lable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Type_lable.AutoSize = true;
            this.Type_lable.BackColor = System.Drawing.Color.Transparent;
            this.Type_lable.Font = new System.Drawing.Font("Rubik", 14F, System.Drawing.FontStyle.Bold);
            this.Type_lable.ForeColor = System.Drawing.Color.Black;
            this.Type_lable.Location = new System.Drawing.Point(103, 12);
            this.Type_lable.Margin = new System.Windows.Forms.Padding(0);
            this.Type_lable.Name = "Type_lable";
            this.Type_lable.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Type_lable.Size = new System.Drawing.Size(147, 37);
            this.Type_lable.TabIndex = 450;
            this.Type_lable.Text = "Key Person";
            this.Type_lable.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Rubik", 14F);
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(250, 12);
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
            this.Close_button.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Close_button.BackgroundImage")));
            this.Close_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Close_button.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.Close_button.FlatAppearance.BorderSize = 0;
            this.Close_button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.Close_button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.Close_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Close_button.Location = new System.Drawing.Point(469, 11);
            this.Close_button.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.Close_button.MinimumSize = new System.Drawing.Size(24, 27);
            this.Close_button.Name = "Close_button";
            this.Close_button.Size = new System.Drawing.Size(24, 27);
            this.Close_button.TabIndex = 448;
            this.Close_button.UseVisualStyleBackColor = false;
            this.Close_button.Click += new System.EventHandler(this.Close_button_Click);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.ExportToExcel_button, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.Count_label, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.Selected_label, 2, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 585);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(498, 74);
            this.tableLayoutPanel2.TabIndex = 492;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel2.SetColumnSpan(this.label3, 4);
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.label3.Location = new System.Drawing.Point(2, 5);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.label3.Name = "label3";
            this.label3.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label3.Size = new System.Drawing.Size(494, 27);
            this.label3.TabIndex = 479;
            this.label3.Text = "يمكن التعديل مباشرة من الجدول...";
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
            this.ExportToExcel_button.Location = new System.Drawing.Point(462, 38);
            this.ExportToExcel_button.Margin = new System.Windows.Forms.Padding(4, 1, 4, 1);
            this.ExportToExcel_button.MaximumSize = new System.Drawing.Size(31, 34);
            this.ExportToExcel_button.MinimumSize = new System.Drawing.Size(31, 34);
            this.ExportToExcel_button.Name = "ExportToExcel_button";
            this.ExportToExcel_button.Size = new System.Drawing.Size(31, 34);
            this.ExportToExcel_button.TabIndex = 411;
            this.ExportToExcel_button.UseVisualStyleBackColor = false;
            // 
            // Count_label
            // 
            this.Count_label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.Count_label.AutoSize = true;
            this.Count_label.BackColor = System.Drawing.Color.Transparent;
            this.Count_label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.Count_label.Location = new System.Drawing.Point(306, 42);
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
            this.Selected_label.Location = new System.Drawing.Point(154, 42);
            this.Selected_label.Margin = new System.Windows.Forms.Padding(0);
            this.Selected_label.Name = "Selected_label";
            this.Selected_label.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Selected_label.Size = new System.Drawing.Size(152, 27);
            this.Selected_label.TabIndex = 478;
            this.Selected_label.Text = "Selected";
            this.Selected_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Name_lable
            // 
            this.Name_lable.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.Name_lable.AutoSize = true;
            this.Name_lable.BackColor = System.Drawing.Color.Transparent;
            this.Name_lable.ForeColor = System.Drawing.Color.Black;
            this.Name_lable.Location = new System.Drawing.Point(353, 68);
            this.Name_lable.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Name_lable.Name = "Name_lable";
            this.Name_lable.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Name_lable.Size = new System.Drawing.Size(60, 27);
            this.Name_lable.TabIndex = 491;
            this.Name_lable.Text = "الاسم:";
            this.Name_lable.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel4.ColumnCount = 4;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            this.tableLayoutPanel4.Controls.Add(this.Type_lable, 2, 0);
            this.tableLayoutPanel4.Controls.Add(this.label1, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.Close_button, 0, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(498, 49);
            this.tableLayoutPanel4.TabIndex = 490;
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToOrderColumns = true;
            this.dataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataGridView.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.dataGridView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Rubik", 10.8F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(216)))), ((int)(((byte)(214)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Member_ID,
            this.Member_Name,
            this.Position_ID,
            this.Position_Name,
            this.Position_Type,
            this.isActive,
            this.DeleteRow});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Rubik", 10.8F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(217)))), ((int)(((byte)(214)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView.EnableHeadersVisualStyles = false;
            this.dataGridView.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dataGridView.Location = new System.Drawing.Point(14, 194);
            this.dataGridView.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.dataGridView.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Rubik", 10.8F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(216)))), ((int)(((byte)(214)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView.RowHeadersWidth = 51;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Rubik", 11F);
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(216)))), ((int)(((byte)(214)))));
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridView.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            this.dataGridView.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            this.dataGridView.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(216)))), ((int)(((byte)(214)))));
            this.dataGridView.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dataGridView.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView.RowTemplate.Height = 30;
            this.dataGridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView.Size = new System.Drawing.Size(470, 387);
            this.dataGridView.TabIndex = 493;
            this.dataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_CellContentClick);
            this.dataGridView.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridView_CellEndEdit);
            this.dataGridView.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_CellEnter);
            this.dataGridView.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dataGridView_CellPainting);
            this.dataGridView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridView_DataError);
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(352, 109);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label4.Size = new System.Drawing.Size(77, 27);
            this.label4.TabIndex = 495;
            this.label4.Text = "المنصب:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Position_comboBox
            // 
            this.Position_comboBox.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.Position_comboBox.BackColor = System.Drawing.Color.White;
            this.Position_comboBox.DisplayMember = "E_Name";
            this.Position_comboBox.ForeColor = System.Drawing.Color.Black;
            this.Position_comboBox.FormattingEnabled = true;
            this.Position_comboBox.Location = new System.Drawing.Point(53, 106);
            this.Position_comboBox.Margin = new System.Windows.Forms.Padding(0);
            this.Position_comboBox.Name = "Position_comboBox";
            this.Position_comboBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Position_comboBox.Size = new System.Drawing.Size(295, 35);
            this.Position_comboBox.Sorted = true;
            this.Position_comboBox.TabIndex = 496;
            // 
            // AddPosition_button
            // 
            this.AddPosition_button.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.AddPosition_button.BackColor = System.Drawing.Color.Transparent;
            this.AddPosition_button.BackgroundImage = global::MyWorkApplication.Properties.Resources.Plus_Sq_D_24;
            this.AddPosition_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.AddPosition_button.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.AddPosition_button.FlatAppearance.BorderSize = 0;
            this.AddPosition_button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.AddPosition_button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.AddPosition_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AddPosition_button.ForeColor = System.Drawing.Color.White;
            this.AddPosition_button.Location = new System.Drawing.Point(21, 107);
            this.AddPosition_button.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.AddPosition_button.MaximumSize = new System.Drawing.Size(30, 31);
            this.AddPosition_button.MinimumSize = new System.Drawing.Size(30, 31);
            this.AddPosition_button.Name = "AddPosition_button";
            this.AddPosition_button.Size = new System.Drawing.Size(30, 31);
            this.AddPosition_button.TabIndex = 528;
            this.AddPosition_button.UseVisualStyleBackColor = false;
            this.AddPosition_button.Click += new System.EventHandler(this.AddPosition_button_Click);
            // 
            // Member_ID
            // 
            this.Member_ID.HeaderText = "Member_ID";
            this.Member_ID.MinimumWidth = 6;
            this.Member_ID.Name = "Member_ID";
            this.Member_ID.ReadOnly = true;
            this.Member_ID.Visible = false;
            // 
            // Member_Name
            // 
            this.Member_Name.FillWeight = 107.2247F;
            this.Member_Name.HeaderText = "الاسم";
            this.Member_Name.MinimumWidth = 6;
            this.Member_Name.Name = "Member_Name";
            // 
            // Position_ID
            // 
            this.Position_ID.HeaderText = "Position_ID";
            this.Position_ID.MinimumWidth = 6;
            this.Position_ID.Name = "Position_ID";
            this.Position_ID.ReadOnly = true;
            this.Position_ID.Visible = false;
            // 
            // Position_Name
            // 
            this.Position_Name.FillWeight = 107.2247F;
            this.Position_Name.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Position_Name.HeaderText = "المنصب الوظيفي";
            this.Position_Name.MinimumWidth = 6;
            this.Position_Name.Name = "Position_Name";
            this.Position_Name.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Position_Name.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // Position_Type
            // 
            this.Position_Type.HeaderText = "Position_Type";
            this.Position_Type.MinimumWidth = 6;
            this.Position_Type.Name = "Position_Type";
            this.Position_Type.ReadOnly = true;
            this.Position_Type.Visible = false;
            // 
            // isActive
            // 
            this.isActive.HeaderText = "is Active";
            this.isActive.MinimumWidth = 6;
            this.isActive.Name = "isActive";
            // 
            // DeleteRow
            // 
            this.DeleteRow.FillWeight = 54.63681F;
            this.DeleteRow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DeleteRow.HeaderText = "";
            this.DeleteRow.MinimumWidth = 40;
            this.DeleteRow.Name = "DeleteRow";
            // 
            // AddNewChecklistMember
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 27F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.BackgroundImage = global::MyWorkApplication.Properties.Resources.Back_700;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(498, 659);
            this.Controls.Add(this.AddPosition_button);
            this.Controls.Add(this.Position_comboBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.Save_button);
            this.Controls.Add(this.Name_textBox);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.Name_lable);
            this.Controls.Add(this.tableLayoutPanel4);
            this.Controls.Add(this.dataGridView);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Rubik", 10.8F);
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "AddNewChecklistMember";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.AddNewChecklistMember_Load);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Save_button;
        private System.Windows.Forms.TextBox Name_textBox;
        private System.Windows.Forms.Label Type_lable;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Close_button;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button ExportToExcel_button;
        private System.Windows.Forms.Label Count_label;
        private System.Windows.Forms.Label Selected_label;
        private System.Windows.Forms.Label Name_lable;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox Position_comboBox;
        private System.Windows.Forms.Button AddPosition_button;
        private System.Windows.Forms.DataGridViewTextBoxColumn Member_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Member_Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn Position_ID;
        private System.Windows.Forms.DataGridViewComboBoxColumn Position_Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn Position_Type;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isActive;
        private System.Windows.Forms.DataGridViewButtonColumn DeleteRow;
    }
}