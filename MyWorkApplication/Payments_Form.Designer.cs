namespace MyWorkApplication
{
    partial class Payments_Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Payments_Form));
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.Sum_label = new System.Windows.Forms.Label();
            this.ExportToExcel_button = new System.Windows.Forms.Button();
            this.Count_label = new System.Windows.Forms.Label();
            this.Selected_label = new System.Windows.Forms.Label();
            this.Top_tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.refreshPageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearAllFiltersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label2 = new System.Windows.Forms.Label();
            this.Group_comboBox = new System.Windows.Forms.ComboBox();
            this.Pending_button = new System.Windows.Forms.Button();
            this.Paid_button = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.Year_checkBox = new System.Windows.Forms.CheckBox();
            this.Year_comboBox = new System.Windows.Forms.ComboBox();
            this.button12 = new System.Windows.Forms.Button();
            this.button11 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.dataGridView = new Zuby.ADGV.AdvancedDataGridView();
            this.SearchToolBar = new Zuby.ADGV.AdvancedDataGridViewSearchToolBar();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.tableLayoutPanel2.SuspendLayout();
            this.Top_tableLayoutPanel.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.SearchToolBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.tableLayoutPanel2.ColumnCount = 6;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 72F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.32653F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.32653F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 51.02041F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.32653F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 175F));
            this.tableLayoutPanel2.Controls.Add(this.Sum_label, 4, 0);
            this.tableLayoutPanel2.Controls.Add(this.ExportToExcel_button, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.Count_label, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.Selected_label, 2, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 634);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1064, 39);
            this.tableLayoutPanel2.TabIndex = 505;
            // 
            // Sum_label
            // 
            this.Sum_label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.Sum_label.AutoSize = true;
            this.Sum_label.BackColor = System.Drawing.Color.Transparent;
            this.Sum_label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.Sum_label.Location = new System.Drawing.Point(177, 6);
            this.Sum_label.Margin = new System.Windows.Forms.Padding(0);
            this.Sum_label.Name = "Sum_label";
            this.Sum_label.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Sum_label.Size = new System.Drawing.Size(133, 27);
            this.Sum_label.TabIndex = 481;
            this.Sum_label.Text = "Sum";
            this.Sum_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ExportToExcel_button
            // 
            this.ExportToExcel_button.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.ExportToExcel_button.AutoSize = true;
            this.ExportToExcel_button.BackColor = System.Drawing.Color.Transparent;
            this.ExportToExcel_button.BackgroundImage = global::MyWorkApplication.Properties.Resources.Excel_L;
            this.ExportToExcel_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ExportToExcel_button.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.ExportToExcel_button.FlatAppearance.BorderSize = 0;
            this.ExportToExcel_button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.ExportToExcel_button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.ExportToExcel_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ExportToExcel_button.ForeColor = System.Drawing.Color.White;
            this.ExportToExcel_button.Location = new System.Drawing.Point(996, 3);
            this.ExportToExcel_button.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.ExportToExcel_button.MaximumSize = new System.Drawing.Size(37, 32);
            this.ExportToExcel_button.MinimumSize = new System.Drawing.Size(37, 32);
            this.ExportToExcel_button.Name = "ExportToExcel_button";
            this.ExportToExcel_button.Size = new System.Drawing.Size(37, 32);
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
            this.Count_label.Location = new System.Drawing.Point(859, 6);
            this.Count_label.Margin = new System.Windows.Forms.Padding(0);
            this.Count_label.Name = "Count_label";
            this.Count_label.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Count_label.Size = new System.Drawing.Size(133, 27);
            this.Count_label.TabIndex = 475;
            this.Count_label.Text = "Count";
            this.Count_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Selected_label
            // 
            this.Selected_label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.Selected_label.AutoSize = true;
            this.Selected_label.BackColor = System.Drawing.Color.Transparent;
            this.Selected_label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.Selected_label.Location = new System.Drawing.Point(726, 6);
            this.Selected_label.Margin = new System.Windows.Forms.Padding(0);
            this.Selected_label.Name = "Selected_label";
            this.Selected_label.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Selected_label.Size = new System.Drawing.Size(133, 27);
            this.Selected_label.TabIndex = 476;
            this.Selected_label.Text = "Selected";
            this.Selected_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Top_tableLayoutPanel
            // 
            this.Top_tableLayoutPanel.ColumnCount = 9;
            this.Top_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4.675121F));
            this.Top_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.94732F));
            this.Top_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 21.13574F));
            this.Top_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 136F));
            this.Top_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 130F));
            this.Top_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 18.67207F));
            this.Top_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.94732F));
            this.Top_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.94732F));
            this.Top_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4.675121F));
            this.Top_tableLayoutPanel.ContextMenuStrip = this.contextMenuStrip1;
            this.Top_tableLayoutPanel.Controls.Add(this.label2, 1, 0);
            this.Top_tableLayoutPanel.Controls.Add(this.Group_comboBox, 2, 0);
            this.Top_tableLayoutPanel.Controls.Add(this.Pending_button, 3, 0);
            this.Top_tableLayoutPanel.Controls.Add(this.Paid_button, 4, 0);
            this.Top_tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.Top_tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.Top_tableLayoutPanel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Top_tableLayoutPanel.Name = "Top_tableLayoutPanel";
            this.Top_tableLayoutPanel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Top_tableLayoutPanel.RowCount = 1;
            this.Top_tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.52632F));
            this.Top_tableLayoutPanel.Size = new System.Drawing.Size(1064, 55);
            this.Top_tableLayoutPanel.TabIndex = 506;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.refreshPageToolStripMenuItem,
            this.clearAllFiltersToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(174, 52);
            // 
            // refreshPageToolStripMenuItem
            // 
            this.refreshPageToolStripMenuItem.Name = "refreshPageToolStripMenuItem";
            this.refreshPageToolStripMenuItem.Size = new System.Drawing.Size(173, 24);
            this.refreshPageToolStripMenuItem.Text = "Refresh Page";
            this.refreshPageToolStripMenuItem.Click += new System.EventHandler(this.refreshPageToolStripMenuItem_Click);
            // 
            // clearAllFiltersToolStripMenuItem
            // 
            this.clearAllFiltersToolStripMenuItem.Name = "clearAllFiltersToolStripMenuItem";
            this.clearAllFiltersToolStripMenuItem.Size = new System.Drawing.Size(173, 24);
            this.clearAllFiltersToolStripMenuItem.Text = "Clear all filters";
            this.clearAllFiltersToolStripMenuItem.Click += new System.EventHandler(this.clearAllFiltersToolStripMenuItem_Click);
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.label2.Location = new System.Drawing.Point(901, 14);
            this.label2.Margin = new System.Windows.Forms.Padding(9, 5, 9, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 27);
            this.label2.TabIndex = 472;
            this.label2.Text = "المجموعة:";
            // 
            // Group_comboBox
            // 
            this.Group_comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.Group_comboBox.BackColor = System.Drawing.Color.White;
            this.Group_comboBox.FormattingEnabled = true;
            this.Group_comboBox.Location = new System.Drawing.Point(727, 15);
            this.Group_comboBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Group_comboBox.Name = "Group_comboBox";
            this.Group_comboBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Group_comboBox.Size = new System.Drawing.Size(162, 35);
            this.Group_comboBox.TabIndex = 471;
            this.Group_comboBox.SelectedIndexChanged += new System.EventHandler(this.Group_comboBox_SelectedIndexChanged);
            // 
            // Pending_button
            // 
            this.Pending_button.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.Pending_button.AutoSize = true;
            this.Pending_button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DarkGray;
            this.Pending_button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkGray;
            this.Pending_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Pending_button.Location = new System.Drawing.Point(591, 6);
            this.Pending_button.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Pending_button.MaximumSize = new System.Drawing.Size(1364, 45);
            this.Pending_button.Name = "Pending_button";
            this.Pending_button.Size = new System.Drawing.Size(129, 43);
            this.Pending_button.TabIndex = 474;
            this.Pending_button.Text = "Pending";
            this.Pending_button.UseVisualStyleBackColor = true;
            this.Pending_button.Click += new System.EventHandler(this.Pending_button_Click);
            // 
            // Paid_button
            // 
            this.Paid_button.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Paid_button.AutoSize = true;
            this.Paid_button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DarkGray;
            this.Paid_button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkGray;
            this.Paid_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Paid_button.Location = new System.Drawing.Point(463, 6);
            this.Paid_button.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Paid_button.MaximumSize = new System.Drawing.Size(1364, 45);
            this.Paid_button.Name = "Paid_button";
            this.Paid_button.Size = new System.Drawing.Size(122, 43);
            this.Paid_button.TabIndex = 473;
            this.Paid_button.Text = "Paid";
            this.Paid_button.UseVisualStyleBackColor = true;
            this.Paid_button.Click += new System.EventHandler(this.Paid_button_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panel1.ContextMenuStrip = this.contextMenuStrip1;
            this.panel1.Controls.Add(this.tableLayoutPanel1);
            this.panel1.Controls.Add(this.button12);
            this.panel1.Controls.Add(this.button11);
            this.panel1.Controls.Add(this.button10);
            this.panel1.Controls.Add(this.button9);
            this.panel1.Controls.Add(this.button8);
            this.panel1.Controls.Add(this.button7);
            this.panel1.Controls.Add(this.button6);
            this.panel1.Controls.Add(this.button5);
            this.panel1.Controls.Add(this.button4);
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(1064, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(198, 673);
            this.panel1.TabIndex = 507;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.Controls.Add(this.Year_checkBox, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.Year_comboBox, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 512);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(198, 39);
            this.tableLayoutPanel1.TabIndex = 505;
            // 
            // Year_checkBox
            // 
            this.Year_checkBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Year_checkBox.AutoSize = true;
            this.Year_checkBox.Checked = true;
            this.Year_checkBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Year_checkBox.Location = new System.Drawing.Point(166, 10);
            this.Year_checkBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Year_checkBox.Name = "Year_checkBox";
            this.Year_checkBox.Padding = new System.Windows.Forms.Padding(1);
            this.Year_checkBox.Size = new System.Drawing.Size(20, 19);
            this.Year_checkBox.TabIndex = 443;
            this.Year_checkBox.UseVisualStyleBackColor = true;
            this.Year_checkBox.CheckedChanged += new System.EventHandler(this.Year_checkBox_CheckedChanged);
            // 
            // Year_comboBox
            // 
            this.Year_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Year_comboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Year_comboBox.ForeColor = System.Drawing.Color.Black;
            this.Year_comboBox.FormatString = "####";
            this.Year_comboBox.FormattingEnabled = true;
            this.Year_comboBox.Location = new System.Drawing.Point(3, 4);
            this.Year_comboBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Year_comboBox.Name = "Year_comboBox";
            this.Year_comboBox.Size = new System.Drawing.Size(155, 35);
            this.Year_comboBox.TabIndex = 15;
            this.Year_comboBox.SelectedIndexChanged += new System.EventHandler(this.Year_comboBox_SelectedIndexChanged);
            // 
            // button12
            // 
            this.button12.Dock = System.Windows.Forms.DockStyle.Top;
            this.button12.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DarkGray;
            this.button12.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkGray;
            this.button12.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button12.Location = new System.Drawing.Point(0, 473);
            this.button12.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(198, 39);
            this.button12.TabIndex = 11;
            this.button12.Text = "12-December";
            this.button12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button12.UseVisualStyleBackColor = true;
            this.button12.Click += new System.EventHandler(this.button12_Click);
            // 
            // button11
            // 
            this.button11.Dock = System.Windows.Forms.DockStyle.Top;
            this.button11.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DarkGray;
            this.button11.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkGray;
            this.button11.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button11.Location = new System.Drawing.Point(0, 434);
            this.button11.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(198, 39);
            this.button11.TabIndex = 10;
            this.button11.Text = "11-November";
            this.button11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Click += new System.EventHandler(this.button11_Click);
            // 
            // button10
            // 
            this.button10.Dock = System.Windows.Forms.DockStyle.Top;
            this.button10.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DarkGray;
            this.button10.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkGray;
            this.button10.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button10.Location = new System.Drawing.Point(0, 395);
            this.button10.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(198, 39);
            this.button10.TabIndex = 9;
            this.button10.Text = "10-October";
            this.button10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // button9
            // 
            this.button9.Dock = System.Windows.Forms.DockStyle.Top;
            this.button9.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DarkGray;
            this.button9.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkGray;
            this.button9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button9.Location = new System.Drawing.Point(0, 356);
            this.button9.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(198, 39);
            this.button9.TabIndex = 8;
            this.button9.Text = "9-September";
            this.button9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // button8
            // 
            this.button8.Dock = System.Windows.Forms.DockStyle.Top;
            this.button8.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DarkGray;
            this.button8.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkGray;
            this.button8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button8.Location = new System.Drawing.Point(0, 317);
            this.button8.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(198, 39);
            this.button8.TabIndex = 7;
            this.button8.Text = "8-August";
            this.button8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // button7
            // 
            this.button7.Dock = System.Windows.Forms.DockStyle.Top;
            this.button7.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DarkGray;
            this.button7.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkGray;
            this.button7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button7.Location = new System.Drawing.Point(0, 278);
            this.button7.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(198, 39);
            this.button7.TabIndex = 6;
            this.button7.Text = "7-July";
            this.button7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button6
            // 
            this.button6.Dock = System.Windows.Forms.DockStyle.Top;
            this.button6.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DarkGray;
            this.button6.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkGray;
            this.button6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button6.Location = new System.Drawing.Point(0, 239);
            this.button6.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(198, 39);
            this.button6.TabIndex = 5;
            this.button6.Text = "6-June";
            this.button6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button5
            // 
            this.button5.Dock = System.Windows.Forms.DockStyle.Top;
            this.button5.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DarkGray;
            this.button5.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkGray;
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button5.Location = new System.Drawing.Point(0, 200);
            this.button5.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(198, 39);
            this.button5.TabIndex = 4;
            this.button5.Text = "5-May";
            this.button5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button4
            // 
            this.button4.Dock = System.Windows.Forms.DockStyle.Top;
            this.button4.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DarkGray;
            this.button4.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkGray;
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.Location = new System.Drawing.Point(0, 161);
            this.button4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(198, 39);
            this.button4.TabIndex = 3;
            this.button4.Text = "4-April";
            this.button4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            this.button3.Dock = System.Windows.Forms.DockStyle.Top;
            this.button3.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DarkGray;
            this.button3.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkGray;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Location = new System.Drawing.Point(0, 122);
            this.button3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(198, 39);
            this.button3.TabIndex = 2;
            this.button3.Text = "3-March";
            this.button3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Dock = System.Windows.Forms.DockStyle.Top;
            this.button2.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DarkGray;
            this.button2.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkGray;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Location = new System.Drawing.Point(0, 83);
            this.button2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(198, 39);
            this.button2.TabIndex = 1;
            this.button2.Text = "2-February";
            this.button2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Top;
            this.button1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DarkGray;
            this.button1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkGray;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(0, 44);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(198, 39);
            this.button1.TabIndex = 0;
            this.button1.Text = "1-January";
            this.button1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(9, 5, 9, 5);
            this.label1.Name = "label1";
            this.label1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label1.Size = new System.Drawing.Size(198, 44);
            this.label1.TabIndex = 471;
            this.label1.Text = "بحث حسب الأشهر:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dataGridView
            // 
            this.dataGridView.AllowDrop = true;
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.AllowUserToOrderColumns = true;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            this.dataGridView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Rubik", 10.8F);
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView.ColumnHeadersHeight = 30;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Rubik", 10.8F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView.DefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView.EnableHeadersVisualStyles = false;
            this.dataGridView.FilterAndSortEnabled = true;
            this.dataGridView.GridColor = System.Drawing.Color.Black;
            this.dataGridView.Location = new System.Drawing.Point(0, 87);
            this.dataGridView.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.ReadOnly = true;
            this.dataGridView.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.dataGridView.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Rubik", 10.8F);
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridView.RowHeadersWidth = 51;
            this.dataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridView.RowTemplate.Height = 27;
            this.dataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView.Size = new System.Drawing.Size(1064, 547);
            this.dataGridView.TabIndex = 572;
            this.dataGridView.SortStringChanged += new System.EventHandler<Zuby.ADGV.AdvancedDataGridView.SortEventArgs>(this.dataGridView_SortStringChanged);
            this.dataGridView.FilterStringChanged += new System.EventHandler<Zuby.ADGV.AdvancedDataGridView.FilterEventArgs>(this.dataGridView_FilterStringChanged);
            this.dataGridView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridView_DataError);
            this.dataGridView.RowHeaderMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView_RowHeaderMouseDoubleClick);
            this.dataGridView.SelectionChanged += new System.EventHandler(this.dataGridView_SelectionChanged);
            // 
            // SearchToolBar
            // 
            this.SearchToolBar.AllowMerge = false;
            this.SearchToolBar.AutoSize = false;
            this.SearchToolBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.SearchToolBar.Font = new System.Drawing.Font("Rubik", 10.8F);
            this.SearchToolBar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.SearchToolBar.ImageScalingSize = new System.Drawing.Size(18, 18);
            this.SearchToolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator4});
            this.SearchToolBar.Location = new System.Drawing.Point(0, 55);
            this.SearchToolBar.MaximumSize = new System.Drawing.Size(0, 32);
            this.SearchToolBar.MinimumSize = new System.Drawing.Size(0, 32);
            this.SearchToolBar.Name = "SearchToolBar";
            this.SearchToolBar.Padding = new System.Windows.Forms.Padding(0);
            this.SearchToolBar.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.SearchToolBar.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.SearchToolBar.Size = new System.Drawing.Size(1064, 32);
            this.SearchToolBar.TabIndex = 571;
            this.SearchToolBar.Search += new Zuby.ADGV.AdvancedDataGridViewSearchToolBarSearchEventHandler(this.SearchToolBar_Search);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.AutoSize = false;
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.toolStripSeparator4.Size = new System.Drawing.Size(10, 35);
            // 
            // Payments_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 27F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1262, 673);
            this.ContextMenuStrip = this.contextMenuStrip1;
            this.Controls.Add(this.dataGridView);
            this.Controls.Add(this.SearchToolBar);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.Top_tableLayoutPanel);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Rubik", 10.8F);
            this.ForeColor = System.Drawing.Color.Black;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Payments_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Payments_Form";
            this.Load += new System.EventHandler(this.Payments_Form_Load);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.Top_tableLayoutPanel.ResumeLayout(false);
            this.Top_tableLayoutPanel.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.SearchToolBar.ResumeLayout(false);
            this.SearchToolBar.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button ExportToExcel_button;
        private System.Windows.Forms.Label Count_label;
        private System.Windows.Forms.Label Selected_label;
        private System.Windows.Forms.TableLayoutPanel Top_tableLayoutPanel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox Year_comboBox;
        private System.Windows.Forms.Button button12;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox Group_comboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button Paid_button;
        private System.Windows.Forms.Button Pending_button;
        private System.Windows.Forms.Label Sum_label;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.CheckBox Year_checkBox;
        private Zuby.ADGV.AdvancedDataGridView dataGridView;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem refreshPageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearAllFiltersToolStripMenuItem;
        private Zuby.ADGV.AdvancedDataGridViewSearchToolBar SearchToolBar;
    }
}