namespace MyWorkApplication
{
    partial class AddNewEducation
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddNewEducation));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.EducationType_comboBox = new System.Windows.Forms.ComboBox();
            this.EducationName_textBox = new System.Windows.Forms.TextBox();
            this.Save_button = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.Close_button = new System.Windows.Forms.Button();
            this.Category_lable = new System.Windows.Forms.Label();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.Name_lable = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.ExportToExcel_button = new System.Windows.Forms.Button();
            this.Count_label = new System.Windows.Forms.Label();
            this.Selected_label = new System.Windows.Forms.Label();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // EducationType_comboBox
            // 
            this.EducationType_comboBox.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.EducationType_comboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.EducationType_comboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.EducationType_comboBox.BackColor = System.Drawing.Color.White;
            this.EducationType_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.EducationType_comboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.EducationType_comboBox.ForeColor = System.Drawing.Color.Black;
            this.EducationType_comboBox.Location = new System.Drawing.Point(8, 72);
            this.EducationType_comboBox.Margin = new System.Windows.Forms.Padding(0);
            this.EducationType_comboBox.Name = "EducationType_comboBox";
            this.EducationType_comboBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.EducationType_comboBox.Size = new System.Drawing.Size(352, 35);
            this.EducationType_comboBox.TabIndex = 1;
            this.EducationType_comboBox.SelectedIndexChanged += new System.EventHandler(this.EducationType_comboBox_SelectedIndexChanged);
            // 
            // EducationName_textBox
            // 
            this.EducationName_textBox.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.EducationName_textBox.BackColor = System.Drawing.Color.White;
            this.EducationName_textBox.ForeColor = System.Drawing.Color.Black;
            this.EducationName_textBox.Location = new System.Drawing.Point(8, 114);
            this.EducationName_textBox.Margin = new System.Windows.Forms.Padding(0);
            this.EducationName_textBox.Name = "EducationName_textBox";
            this.EducationName_textBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.EducationName_textBox.Size = new System.Drawing.Size(352, 29);
            this.EducationName_textBox.TabIndex = 2;
            this.EducationName_textBox.TextChanged += new System.EventHandler(this.EducationName_textBox_TextChanged);
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
            this.Save_button.Location = new System.Drawing.Point(163, 156);
            this.Save_button.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.Save_button.Name = "Save_button";
            this.Save_button.Size = new System.Drawing.Size(99, 38);
            this.Save_button.TabIndex = 3;
            this.Save_button.UseVisualStyleBackColor = false;
            this.Save_button.Click += new System.EventHandler(this.Save_button_Click);
            this.Save_button.MouseEnter += new System.EventHandler(this.Add_button_MouseEnter);
            this.Save_button.MouseLeave += new System.EventHandler(this.Add_button_MouseLeave);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Rubik", 14F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(42, 22);
            this.label2.Margin = new System.Windows.Forms.Padding(0);
            this.label2.Name = "label2";
            this.label2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label2.Size = new System.Drawing.Size(180, 37);
            this.label2.TabIndex = 450;
            this.label2.Text = "التحصيل العلمي";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Rubik", 14F);
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(222, 22);
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
            this.Close_button.Location = new System.Drawing.Point(413, 11);
            this.Close_button.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Close_button.Name = "Close_button";
            this.Close_button.Size = new System.Drawing.Size(25, 36);
            this.Close_button.TabIndex = 448;
            this.Close_button.UseVisualStyleBackColor = false;
            this.Close_button.Click += new System.EventHandler(this.Close_Click);
            this.Close_button.MouseEnter += new System.EventHandler(this.Close_button_MouseEnter);
            this.Close_button.MouseLeave += new System.EventHandler(this.Close_button_MouseLeave);
            // 
            // Category_lable
            // 
            this.Category_lable.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.Category_lable.AutoSize = true;
            this.Category_lable.BackColor = System.Drawing.Color.Transparent;
            this.Category_lable.ForeColor = System.Drawing.Color.Black;
            this.Category_lable.Location = new System.Drawing.Point(364, 72);
            this.Category_lable.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Category_lable.Name = "Category_lable";
            this.Category_lable.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Category_lable.Size = new System.Drawing.Size(67, 27);
            this.Category_lable.TabIndex = 461;
            this.Category_lable.Text = "الدرجة:";
            this.Category_lable.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel4.ColumnCount = 4;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tableLayoutPanel4.Controls.Add(this.label2, 2, 0);
            this.tableLayoutPanel4.Controls.Add(this.label1, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.Close_button, 0, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(443, 59);
            this.tableLayoutPanel4.TabIndex = 463;
            // 
            // Name_lable
            // 
            this.Name_lable.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.Name_lable.AutoSize = true;
            this.Name_lable.BackColor = System.Drawing.Color.Transparent;
            this.Name_lable.ForeColor = System.Drawing.Color.Black;
            this.Name_lable.Location = new System.Drawing.Point(364, 117);
            this.Name_lable.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Name_lable.Name = "Name_lable";
            this.Name_lable.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Name_lable.Size = new System.Drawing.Size(69, 27);
            this.Name_lable.TabIndex = 464;
            this.Name_lable.Text = "المجال:";
            this.Name_lable.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 44F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.ExportToExcel_button, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.Count_label, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.Selected_label, 2, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 523);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(443, 75);
            this.tableLayoutPanel2.TabIndex = 484;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel2.SetColumnSpan(this.label3, 4);
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.label3.Location = new System.Drawing.Point(3, 5);
            this.label3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.label3.Name = "label3";
            this.label3.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label3.Size = new System.Drawing.Size(437, 27);
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
            this.ExportToExcel_button.Location = new System.Drawing.Point(403, 40);
            this.ExportToExcel_button.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
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
            this.Count_label.Location = new System.Drawing.Point(266, 42);
            this.Count_label.Margin = new System.Windows.Forms.Padding(0);
            this.Count_label.Name = "Count_label";
            this.Count_label.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Count_label.Size = new System.Drawing.Size(133, 27);
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
            this.Selected_label.Location = new System.Drawing.Point(133, 42);
            this.Selected_label.Margin = new System.Windows.Forms.Padding(0);
            this.Selected_label.Name = "Selected_label";
            this.Selected_label.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Selected_label.Size = new System.Drawing.Size(133, 27);
            this.Selected_label.TabIndex = 478;
            this.Selected_label.Text = "Selected";
            this.Selected_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.AllowUserToOrderColumns = true;
            this.dataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataGridView.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.dataGridView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Rubik", 10.8F);
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(216)))), ((int)(((byte)(214)))));
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Rubik", 10.8F);
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(217)))), ((int)(((byte)(214)))));
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView.DefaultCellStyle = dataGridViewCellStyle6;
            this.dataGridView.EnableHeadersVisualStyles = false;
            this.dataGridView.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dataGridView.Location = new System.Drawing.Point(16, 200);
            this.dataGridView.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.dataGridView.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Rubik", 10.8F);
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(216)))), ((int)(((byte)(214)))));
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.dataGridView.RowHeadersWidth = 51;
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Rubik", 11F);
            dataGridViewCellStyle8.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(216)))), ((int)(((byte)(214)))));
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView.RowsDefaultCellStyle = dataGridViewCellStyle8;
            this.dataGridView.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            this.dataGridView.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            this.dataGridView.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(216)))), ((int)(((byte)(214)))));
            this.dataGridView.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dataGridView.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView.RowTemplate.Height = 30;
            this.dataGridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView.Size = new System.Drawing.Size(412, 317);
            this.dataGridView.TabIndex = 4;
            this.dataGridView.VirtualMode = true;
            this.dataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Education_dataGridView_CellContentClick);
            this.dataGridView.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.Education_dataGridView_CellEndEdit);
            this.dataGridView.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.Education_dataGridView_CellPainting);
            this.dataGridView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridView_DataError);
            this.dataGridView.SelectionChanged += new System.EventHandler(this.Education_dataGridView_SelectionChanged);
            // 
            // AddNewEducation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 27F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.BackgroundImage = global::MyWorkApplication.Properties.Resources.Background_Border;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(443, 598);
            this.Controls.Add(this.dataGridView);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.Name_lable);
            this.Controls.Add(this.Save_button);
            this.Controls.Add(this.Category_lable);
            this.Controls.Add(this.tableLayoutPanel4);
            this.Controls.Add(this.EducationType_comboBox);
            this.Controls.Add(this.EducationName_textBox);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Rubik", 10.8F);
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "AddNewEducation";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.AddNewEducation_Load);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox EducationType_comboBox;
        private System.Windows.Forms.TextBox EducationName_textBox;
        private System.Windows.Forms.Button Save_button;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Close_button;
        private System.Windows.Forms.Label Category_lable;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Label Name_lable;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button ExportToExcel_button;
        private System.Windows.Forms.Label Count_label;
        private System.Windows.Forms.Label Selected_label;
        private System.Windows.Forms.DataGridView dataGridView;
    }
}