namespace MyWorkApplication
{
    partial class Purchasing_Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Purchasing_Form));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.Save_button = new System.Windows.Forms.Button();
            this.MP_Name_label = new System.Windows.Forms.Label();
            this.MP_Name_textBox = new System.Windows.Forms.TextBox();
            this.LoanDate_label = new System.Windows.Forms.Label();
            this.Person_Name_textBox = new System.Windows.Forms.TextBox();
            this.Person_Name_label = new System.Windows.Forms.Label();
            this.MicroProject_ID_label = new System.Windows.Forms.Label();
            this.MicroProject_ID_textBox = new System.Windows.Forms.TextBox();
            this.LoanAmount_label = new System.Windows.Forms.Label();
            this.LoanAmount_textBox1 = new System.Windows.Forms.TextBox();
            this.LoanDate_dateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.SubHeader1_label = new System.Windows.Forms.Label();
            this.tableLayoutPanel14 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.Career_textBox = new System.Windows.Forms.TextBox();
            this.Items_dataGridView = new System.Windows.Forms.DataGridView();
            this.addNotesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.application_toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clear_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MP_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Brand = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemCategory_ID = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Quantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Currency = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.InvoiceNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TraderName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.P_DeleteRow = new System.Windows.Forms.DataGridViewButtonColumn();
            this.P_InDataBase = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel14.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Items_dataGridView)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Save_button
            // 
            this.Save_button.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Save_button.BackColor = System.Drawing.Color.Transparent;
            this.Save_button.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Save_button.BackgroundImage")));
            this.Save_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Save_button.FlatAppearance.BorderSize = 0;
            this.Save_button.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.Save_button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.Save_button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.Save_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Save_button.ForeColor = System.Drawing.Color.Black;
            this.Save_button.Location = new System.Drawing.Point(386, 543);
            this.Save_button.MaximumSize = new System.Drawing.Size(126, 51);
            this.Save_button.MinimumSize = new System.Drawing.Size(110, 51);
            this.Save_button.Name = "Save_button";
            this.Save_button.Size = new System.Drawing.Size(126, 51);
            this.Save_button.TabIndex = 8;
            this.Save_button.UseVisualStyleBackColor = false;
            this.Save_button.Click += new System.EventHandler(this.Save_button_Click);
            this.Save_button.MouseEnter += new System.EventHandler(this.Save_button_MouseEnter);
            this.Save_button.MouseLeave += new System.EventHandler(this.Save_button_MouseLeave);
            // 
            // MP_Name_label
            // 
            this.MP_Name_label.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.MP_Name_label.AutoSize = true;
            this.MP_Name_label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.MP_Name_label.Location = new System.Drawing.Point(341, 149);
            this.MP_Name_label.Name = "MP_Name_label";
            this.MP_Name_label.Size = new System.Drawing.Size(116, 27);
            this.MP_Name_label.TabIndex = 584;
            this.MP_Name_label.Text = "اسم المشروع:";
            this.MP_Name_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MP_Name_textBox
            // 
            this.MP_Name_textBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.MP_Name_textBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.MP_Name_textBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.MP_Name_textBox.BackColor = System.Drawing.Color.White;
            this.MP_Name_textBox.ForeColor = System.Drawing.Color.Black;
            this.MP_Name_textBox.Location = new System.Drawing.Point(102, 148);
            this.MP_Name_textBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MP_Name_textBox.Name = "MP_Name_textBox";
            this.MP_Name_textBox.ReadOnly = true;
            this.MP_Name_textBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.MP_Name_textBox.Size = new System.Drawing.Size(233, 29);
            this.MP_Name_textBox.TabIndex = 4;
            this.MP_Name_textBox.TabStop = false;
            // 
            // LoanDate_label
            // 
            this.LoanDate_label.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.LoanDate_label.AutoSize = true;
            this.LoanDate_label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.LoanDate_label.Location = new System.Drawing.Point(730, 186);
            this.LoanDate_label.Name = "LoanDate_label";
            this.LoanDate_label.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.LoanDate_label.Size = new System.Drawing.Size(147, 27);
            this.LoanDate_label.TabIndex = 579;
            this.LoanDate_label.Text = "تاريخ بدء المشروع:";
            this.LoanDate_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Person_Name_textBox
            // 
            this.Person_Name_textBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.Person_Name_textBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.Person_Name_textBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.Person_Name_textBox.BackColor = System.Drawing.Color.White;
            this.Person_Name_textBox.ForeColor = System.Drawing.Color.Black;
            this.Person_Name_textBox.Location = new System.Drawing.Point(102, 111);
            this.Person_Name_textBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Person_Name_textBox.Name = "Person_Name_textBox";
            this.Person_Name_textBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Person_Name_textBox.Size = new System.Drawing.Size(233, 29);
            this.Person_Name_textBox.TabIndex = 2;
            this.Person_Name_textBox.TabStop = false;
            this.Person_Name_textBox.Leave += new System.EventHandler(this.Person_Name_textBox2_Leave);
            // 
            // Person_Name_label
            // 
            this.Person_Name_label.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.Person_Name_label.AutoSize = true;
            this.Person_Name_label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.Person_Name_label.Location = new System.Drawing.Point(341, 112);
            this.Person_Name_label.Name = "Person_Name_label";
            this.Person_Name_label.Size = new System.Drawing.Size(123, 27);
            this.Person_Name_label.TabIndex = 577;
            this.Person_Name_label.Text = "اسم المستفيد:";
            this.Person_Name_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MicroProject_ID_label
            // 
            this.MicroProject_ID_label.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.MicroProject_ID_label.AutoSize = true;
            this.MicroProject_ID_label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.MicroProject_ID_label.Location = new System.Drawing.Point(730, 112);
            this.MicroProject_ID_label.Name = "MicroProject_ID_label";
            this.MicroProject_ID_label.Size = new System.Drawing.Size(110, 27);
            this.MicroProject_ID_label.TabIndex = 573;
            this.MicroProject_ID_label.Text = "رقم المشروع:";
            this.MicroProject_ID_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MicroProject_ID_textBox
            // 
            this.MicroProject_ID_textBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.MicroProject_ID_textBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.MicroProject_ID_textBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.MicroProject_ID_textBox.BackColor = System.Drawing.Color.White;
            this.MicroProject_ID_textBox.ForeColor = System.Drawing.Color.Black;
            this.MicroProject_ID_textBox.Location = new System.Drawing.Point(518, 111);
            this.MicroProject_ID_textBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MicroProject_ID_textBox.Name = "MicroProject_ID_textBox";
            this.MicroProject_ID_textBox.Size = new System.Drawing.Size(206, 29);
            this.MicroProject_ID_textBox.TabIndex = 1;
            this.MicroProject_ID_textBox.Leave += new System.EventHandler(this.MicroProject_ID_textBox_Leave);
            // 
            // LoanAmount_label
            // 
            this.LoanAmount_label.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.LoanAmount_label.AutoSize = true;
            this.LoanAmount_label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.LoanAmount_label.Location = new System.Drawing.Point(341, 186);
            this.LoanAmount_label.Name = "LoanAmount_label";
            this.LoanAmount_label.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.LoanAmount_label.Size = new System.Drawing.Size(119, 27);
            this.LoanAmount_label.TabIndex = 576;
            this.LoanAmount_label.Text = "المبلغ الممول:";
            this.LoanAmount_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LoanAmount_textBox1
            // 
            this.LoanAmount_textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.LoanAmount_textBox1.BackColor = System.Drawing.Color.White;
            this.LoanAmount_textBox1.ForeColor = System.Drawing.Color.Black;
            this.LoanAmount_textBox1.Location = new System.Drawing.Point(102, 185);
            this.LoanAmount_textBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.LoanAmount_textBox1.Name = "LoanAmount_textBox1";
            this.LoanAmount_textBox1.Size = new System.Drawing.Size(233, 29);
            this.LoanAmount_textBox1.TabIndex = 6;
            this.LoanAmount_textBox1.TabStop = false;
            // 
            // LoanDate_dateTimePicker
            // 
            this.LoanDate_dateTimePicker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.LoanDate_dateTimePicker.CustomFormat = "yyyy/MM/dd";
            this.LoanDate_dateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.LoanDate_dateTimePicker.Location = new System.Drawing.Point(518, 185);
            this.LoanDate_dateTimePicker.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.LoanDate_dateTimePicker.Name = "LoanDate_dateTimePicker";
            this.LoanDate_dateTimePicker.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.LoanDate_dateTimePicker.RightToLeftLayout = true;
            this.LoanDate_dateTimePicker.Size = new System.Drawing.Size(206, 29);
            this.LoanDate_dateTimePicker.TabIndex = 5;
            this.LoanDate_dateTimePicker.TabStop = false;
            // 
            // SubHeader1_label
            // 
            this.SubHeader1_label.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SubHeader1_label.AutoSize = true;
            this.tableLayoutPanel14.SetColumnSpan(this.SubHeader1_label, 5);
            this.SubHeader1_label.Font = new System.Drawing.Font("Rubik", 18F, System.Drawing.FontStyle.Bold);
            this.SubHeader1_label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(54)))), ((int)(((byte)(48)))));
            this.SubHeader1_label.Location = new System.Drawing.Point(3, 26);
            this.SubHeader1_label.Name = "SubHeader1_label";
            this.SubHeader1_label.Padding = new System.Windows.Forms.Padding(0, 0, 0, 15);
            this.SubHeader1_label.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.SubHeader1_label.Size = new System.Drawing.Size(915, 61);
            this.SubHeader1_label.TabIndex = 587;
            this.SubHeader1_label.Text = "مشتريات المشروع";
            this.SubHeader1_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel14
            // 
            this.tableLayoutPanel14.AutoScroll = true;
            this.tableLayoutPanel14.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel14.ColumnCount = 5;
            this.tableLayoutPanel14.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 21.0987F));
            this.tableLayoutPanel14.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 23.05631F));
            this.tableLayoutPanel14.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 19.24984F));
            this.tableLayoutPanel14.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.99273F));
            this.tableLayoutPanel14.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10.60242F));
            this.tableLayoutPanel14.Controls.Add(this.MicroProject_ID_label, 0, 3);
            this.tableLayoutPanel14.Controls.Add(this.MicroProject_ID_textBox, 1, 3);
            this.tableLayoutPanel14.Controls.Add(this.SubHeader1_label, 0, 1);
            this.tableLayoutPanel14.Controls.Add(this.LoanAmount_textBox1, 3, 5);
            this.tableLayoutPanel14.Controls.Add(this.LoanAmount_label, 2, 5);
            this.tableLayoutPanel14.Controls.Add(this.Person_Name_textBox, 3, 3);
            this.tableLayoutPanel14.Controls.Add(this.Person_Name_label, 2, 3);
            this.tableLayoutPanel14.Controls.Add(this.LoanDate_label, 0, 5);
            this.tableLayoutPanel14.Controls.Add(this.LoanDate_dateTimePicker, 1, 5);
            this.tableLayoutPanel14.Controls.Add(this.MP_Name_textBox, 3, 4);
            this.tableLayoutPanel14.Controls.Add(this.MP_Name_label, 2, 4);
            this.tableLayoutPanel14.Controls.Add(this.label1, 0, 4);
            this.tableLayoutPanel14.Controls.Add(this.Career_textBox, 1, 4);
            this.tableLayoutPanel14.Controls.Add(this.Items_dataGridView, 0, 7);
            this.tableLayoutPanel14.Controls.Add(this.Save_button, 2, 11);
            this.tableLayoutPanel14.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel14.ForeColor = System.Drawing.Color.Black;
            this.tableLayoutPanel14.Location = new System.Drawing.Point(80, 0);
            this.tableLayoutPanel14.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tableLayoutPanel14.Name = "tableLayoutPanel14";
            this.tableLayoutPanel14.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tableLayoutPanel14.RowCount = 13;
            this.tableLayoutPanel14.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel14.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.27649F));
            this.tableLayoutPanel14.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel14.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.264718F));
            this.tableLayoutPanel14.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.264718F));
            this.tableLayoutPanel14.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.264718F));
            this.tableLayoutPanel14.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.264718F));
            this.tableLayoutPanel14.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.264718F));
            this.tableLayoutPanel14.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 37.3894F));
            this.tableLayoutPanel14.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 1.216375F));
            this.tableLayoutPanel14.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 2.838063F));
            this.tableLayoutPanel14.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.51586F));
            this.tableLayoutPanel14.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.264718F));
            this.tableLayoutPanel14.Size = new System.Drawing.Size(921, 639);
            this.tableLayoutPanel14.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.label1.Location = new System.Drawing.Point(730, 149);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 27);
            this.label1.TabIndex = 588;
            this.label1.Text = "المهنة:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Career_textBox
            // 
            this.Career_textBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.Career_textBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.Career_textBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.Career_textBox.BackColor = System.Drawing.Color.White;
            this.Career_textBox.ForeColor = System.Drawing.Color.Black;
            this.Career_textBox.Location = new System.Drawing.Point(518, 148);
            this.Career_textBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Career_textBox.Name = "Career_textBox";
            this.Career_textBox.ReadOnly = true;
            this.Career_textBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Career_textBox.Size = new System.Drawing.Size(206, 29);
            this.Career_textBox.TabIndex = 3;
            this.Career_textBox.TabStop = false;
            // 
            // Items_dataGridView
            // 
            this.Items_dataGridView.BackgroundColor = System.Drawing.Color.White;
            this.Items_dataGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Items_dataGridView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Rubik", 10.8F);
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Items_dataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.Items_dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Items_dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID,
            this.MP_ID,
            this.ItemName,
            this.Brand,
            this.ItemCategory_ID,
            this.Quantity,
            this.Price,
            this.Currency,
            this.InvoiceNumber,
            this.TraderName,
            this.P_DeleteRow,
            this.P_InDataBase});
            this.tableLayoutPanel14.SetColumnSpan(this.Items_dataGridView, 5);
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Rubik", 10.8F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Items_dataGridView.DefaultCellStyle = dataGridViewCellStyle2;
            this.Items_dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Items_dataGridView.EnableHeadersVisualStyles = false;
            this.Items_dataGridView.GridColor = System.Drawing.Color.DarkGray;
            this.Items_dataGridView.Location = new System.Drawing.Point(3, 259);
            this.Items_dataGridView.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Items_dataGridView.Name = "Items_dataGridView";
            this.Items_dataGridView.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Items_dataGridView.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Rubik", 10.8F);
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Items_dataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.Items_dataGridView.RowHeadersVisible = false;
            this.Items_dataGridView.RowHeadersWidth = 51;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Items_dataGridView.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.tableLayoutPanel14.SetRowSpan(this.Items_dataGridView, 2);
            this.Items_dataGridView.RowTemplate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Items_dataGridView.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Items_dataGridView.RowTemplate.Height = 26;
            this.Items_dataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.Items_dataGridView.Size = new System.Drawing.Size(915, 253);
            this.Items_dataGridView.TabIndex = 7;
            this.Items_dataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Items_dataGridView_CellContentClick);
            this.Items_dataGridView.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.Items_dataGridView_CellPainting);
            this.Items_dataGridView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.Items_dataGridView_DataError);
            this.Items_dataGridView.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.Items_dataGridView_EditingControlShowing);
            // 
            // addNotesToolStripMenuItem
            // 
            this.addNotesToolStripMenuItem.Name = "addNotesToolStripMenuItem";
            this.addNotesToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.addNotesToolStripMenuItem.Size = new System.Drawing.Size(202, 24);
            this.addNotesToolStripMenuItem.Text = "Add Notes";
            // 
            // application_toolStripMenuItem
            // 
            this.application_toolStripMenuItem.Name = "application_toolStripMenuItem";
            this.application_toolStripMenuItem.Size = new System.Drawing.Size(202, 24);
            this.application_toolStripMenuItem.Text = "Show Application";
            // 
            // clear_ToolStripMenuItem
            // 
            this.clear_ToolStripMenuItem.Name = "clear_ToolStripMenuItem";
            this.clear_ToolStripMenuItem.Size = new System.Drawing.Size(202, 24);
            this.clear_ToolStripMenuItem.Text = "Clear Form";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clear_ToolStripMenuItem,
            this.application_toolStripMenuItem,
            this.addNotesToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(203, 76);
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.ContextMenuStrip = this.contextMenuStrip1;
            this.panel1.Controls.Add(this.tableLayoutPanel14);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(80, 0, 80, 0);
            this.panel1.Size = new System.Drawing.Size(1081, 639);
            this.panel1.TabIndex = 417;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.FillWeight = 30F;
            this.dataGridViewTextBoxColumn1.HeaderText = "ID";
            this.dataGridViewTextBoxColumn1.MinimumWidth = 10;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn1.Visible = false;
            this.dataGridViewTextBoxColumn1.Width = 125;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "MP_ID";
            this.dataGridViewTextBoxColumn2.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Visible = false;
            this.dataGridViewTextBoxColumn2.Width = 125;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn3.FillWeight = 10F;
            this.dataGridViewTextBoxColumn3.HeaderText = "اسم المادة";
            this.dataGridViewTextBoxColumn3.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn4.FillWeight = 10F;
            this.dataGridViewTextBoxColumn4.HeaderText = "الماركة";
            this.dataGridViewTextBoxColumn4.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn5.FillWeight = 10F;
            this.dataGridViewTextBoxColumn5.HeaderText = "العدد";
            this.dataGridViewTextBoxColumn5.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn6.FillWeight = 10F;
            this.dataGridViewTextBoxColumn6.HeaderText = "السعر";
            this.dataGridViewTextBoxColumn6.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn7.FillWeight = 10F;
            this.dataGridViewTextBoxColumn7.HeaderText = "رقم الفاتورة";
            this.dataGridViewTextBoxColumn7.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn8.FillWeight = 10F;
            this.dataGridViewTextBoxColumn8.HeaderText = "اسم التاجر";
            this.dataGridViewTextBoxColumn8.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn9.FillWeight = 1F;
            this.dataGridViewTextBoxColumn9.HeaderText = "P_InDataBase";
            this.dataGridViewTextBoxColumn9.MinimumWidth = 50;
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            this.dataGridViewTextBoxColumn9.Visible = false;
            // 
            // ID
            // 
            this.ID.FillWeight = 30F;
            this.ID.HeaderText = "ID";
            this.ID.MinimumWidth = 10;
            this.ID.Name = "ID";
            this.ID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ID.Visible = false;
            this.ID.Width = 125;
            // 
            // MP_ID
            // 
            this.MP_ID.HeaderText = "MP_ID";
            this.MP_ID.MinimumWidth = 6;
            this.MP_ID.Name = "MP_ID";
            this.MP_ID.Visible = false;
            this.MP_ID.Width = 125;
            // 
            // ItemName
            // 
            this.ItemName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ItemName.FillWeight = 10.92805F;
            this.ItemName.HeaderText = "اسم المادة";
            this.ItemName.MinimumWidth = 6;
            this.ItemName.Name = "ItemName";
            // 
            // Brand
            // 
            this.Brand.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Brand.FillWeight = 10.92805F;
            this.Brand.HeaderText = "الماركة";
            this.Brand.MinimumWidth = 6;
            this.Brand.Name = "Brand";
            // 
            // ItemCategory_ID
            // 
            this.ItemCategory_ID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ItemCategory_ID.FillWeight = 10F;
            this.ItemCategory_ID.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ItemCategory_ID.HeaderText = "فئة المادة";
            this.ItemCategory_ID.MinimumWidth = 6;
            this.ItemCategory_ID.Name = "ItemCategory_ID";
            this.ItemCategory_ID.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ItemCategory_ID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // Quantity
            // 
            this.Quantity.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Quantity.FillWeight = 10.92805F;
            this.Quantity.HeaderText = "العدد";
            this.Quantity.MinimumWidth = 6;
            this.Quantity.Name = "Quantity";
            // 
            // Price
            // 
            this.Price.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Price.FillWeight = 10.92805F;
            this.Price.HeaderText = "السعر";
            this.Price.MinimumWidth = 6;
            this.Price.Name = "Price";
            // 
            // Currency
            // 
            this.Currency.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Currency.FillWeight = 10.92805F;
            this.Currency.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Currency.HeaderText = "نوع العملة";
            this.Currency.Items.AddRange(new object[] {
            "USD",
            "SYP"});
            this.Currency.MinimumWidth = 6;
            this.Currency.Name = "Currency";
            // 
            // InvoiceNumber
            // 
            this.InvoiceNumber.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.InvoiceNumber.FillWeight = 10.92805F;
            this.InvoiceNumber.HeaderText = "رقم الفاتورة";
            this.InvoiceNumber.MinimumWidth = 6;
            this.InvoiceNumber.Name = "InvoiceNumber";
            // 
            // TraderName
            // 
            this.TraderName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.TraderName.FillWeight = 10.92805F;
            this.TraderName.HeaderText = "اسم التاجر";
            this.TraderName.MinimumWidth = 6;
            this.TraderName.Name = "TraderName";
            // 
            // P_DeleteRow
            // 
            this.P_DeleteRow.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.P_DeleteRow.FillWeight = 26.9583F;
            this.P_DeleteRow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.P_DeleteRow.HeaderText = " ";
            this.P_DeleteRow.MinimumWidth = 50;
            this.P_DeleteRow.Name = "P_DeleteRow";
            this.P_DeleteRow.Width = 50;
            // 
            // P_InDataBase
            // 
            this.P_InDataBase.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.P_InDataBase.FillWeight = 1F;
            this.P_InDataBase.HeaderText = "P_InDataBase";
            this.P_InDataBase.MinimumWidth = 50;
            this.P_InDataBase.Name = "P_InDataBase";
            this.P_InDataBase.Visible = false;
            // 
            // Purchasing_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 27F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1081, 639);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Rubik", 10.8F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Purchasing_Form";
            this.Text = "Purchasing_Form";
            this.Load += new System.EventHandler(this.Purchasing_Form_Load);
            this.tableLayoutPanel14.ResumeLayout(false);
            this.tableLayoutPanel14.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Items_dataGridView)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel14;
        private System.Windows.Forms.Label MP_Name_label;
        private System.Windows.Forms.TextBox MP_Name_textBox;
        private System.Windows.Forms.Label LoanDate_label;
        private System.Windows.Forms.TextBox Person_Name_textBox;
        private System.Windows.Forms.Label Person_Name_label;
        private System.Windows.Forms.Label MicroProject_ID_label;
        private System.Windows.Forms.TextBox MicroProject_ID_textBox;
        private System.Windows.Forms.Label LoanAmount_label;
        private System.Windows.Forms.TextBox LoanAmount_textBox1;
        private System.Windows.Forms.DateTimePicker LoanDate_dateTimePicker;
        private System.Windows.Forms.Label SubHeader1_label;
        private System.Windows.Forms.Button Save_button;
        private System.Windows.Forms.ToolStripMenuItem addNotesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem application_toolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clear_ToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox Career_textBox;
        private System.Windows.Forms.DataGridView Items_dataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn MP_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Brand;
        private System.Windows.Forms.DataGridViewComboBoxColumn ItemCategory_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Quantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn Price;
        private System.Windows.Forms.DataGridViewComboBoxColumn Currency;
        private System.Windows.Forms.DataGridViewTextBoxColumn InvoiceNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn TraderName;
        private System.Windows.Forms.DataGridViewButtonColumn P_DeleteRow;
        private System.Windows.Forms.DataGridViewTextBoxColumn P_InDataBase;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
    }
}