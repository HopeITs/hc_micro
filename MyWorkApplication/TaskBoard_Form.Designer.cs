using System;
using System.Windows.Forms;

namespace MyWorkApplication
{
    partial class TaskBoard_Form
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TaskBoard_Form));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.clearAllFiltersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showAllColumnsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshPageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.goToToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.searchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statisticsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.taskBoardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Top_tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.TaskType_comboBox = new System.Windows.Forms.ComboBox();
            this.Counter_textBox = new System.Windows.Forms.TextBox();
            this.Refresh_button = new System.Windows.Forms.Button();
            this.ExactSearch_checkBox = new System.Windows.Forms.CheckBox();
            this.ShowHide_button = new System.Windows.Forms.Button();
            this.ExportToExcel_button = new System.Windows.Forms.Button();
            this.Search_TxtBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.Filters_tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.f30_label = new System.Windows.Forms.Label();
            this.f29_label = new System.Windows.Forms.Label();
            this.MP_dataGridView = new System.Windows.Forms.DataGridView();
            this.f06_Type_comboBox = new System.Windows.Forms.ComboBox();
            this.f04_State_comboBox = new System.Windows.Forms.ComboBox();
            this.f05_Donor_comboBox = new System.Windows.Forms.ComboBox();
            this.f07_comboBox = new System.Windows.Forms.ComboBox();
            this.f07_label = new System.Windows.Forms.Label();
            this.f8_label = new System.Windows.Forms.Label();
            this.Finished_button = new System.Windows.Forms.Button();
            this.Cancelled_button = new System.Windows.Forms.Button();
            this.Closed_button = new System.Windows.Forms.Button();
            this.f14_comboBox = new System.Windows.Forms.ComboBox();
            this.f14_label = new System.Windows.Forms.Label();
            this.f13_comboBox = new System.Windows.Forms.ComboBox();
            this.f13_label = new System.Windows.Forms.Label();
            this.f12_comboBox = new System.Windows.Forms.ComboBox();
            this.f12_label = new System.Windows.Forms.Label();
            this.f11_comboBox = new System.Windows.Forms.ComboBox();
            this.f11_label = new System.Windows.Forms.Label();
            this.f10_comboBox = new System.Windows.Forms.ComboBox();
            this.f10_label = new System.Windows.Forms.Label();
            this.f09_comboBox = new System.Windows.Forms.ComboBox();
            this.f09_label = new System.Windows.Forms.Label();
            this.f8_comboBox = new System.Windows.Forms.ComboBox();
            this.f28_comboBox = new System.Windows.Forms.ComboBox();
            this.f28_label = new System.Windows.Forms.Label();
            this.f27_comboBox = new System.Windows.Forms.ComboBox();
            this.f27_label = new System.Windows.Forms.Label();
            this.f26_comboBox = new System.Windows.Forms.ComboBox();
            this.f26_label = new System.Windows.Forms.Label();
            this.f25_comboBox = new System.Windows.Forms.ComboBox();
            this.f25_label = new System.Windows.Forms.Label();
            this.f24_comboBox = new System.Windows.Forms.ComboBox();
            this.f24_label = new System.Windows.Forms.Label();
            this.f23_comboBox = new System.Windows.Forms.ComboBox();
            this.f23_label = new System.Windows.Forms.Label();
            this.f22_comboBox = new System.Windows.Forms.ComboBox();
            this.f22_label = new System.Windows.Forms.Label();
            this.f21_comboBox = new System.Windows.Forms.ComboBox();
            this.f21_label = new System.Windows.Forms.Label();
            this.f20_comboBox = new System.Windows.Forms.ComboBox();
            this.f20_label = new System.Windows.Forms.Label();
            this.f19_comboBox = new System.Windows.Forms.ComboBox();
            this.f19_label = new System.Windows.Forms.Label();
            this.f18_comboBox = new System.Windows.Forms.ComboBox();
            this.f18_label = new System.Windows.Forms.Label();
            this.f17_comboBox = new System.Windows.Forms.ComboBox();
            this.f17_label = new System.Windows.Forms.Label();
            this.f29_comboBox = new System.Windows.Forms.ComboBox();
            this.f30_comboBox = new System.Windows.Forms.ComboBox();
            this.f16_comboBox = new System.Windows.Forms.ComboBox();
            this.f16_label = new System.Windows.Forms.Label();
            this.Grid_panel = new System.Windows.Forms.Panel();
            this.contextMenuStrip1.SuspendLayout();
            this.Top_tableLayoutPanel.SuspendLayout();
            this.Filters_tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MP_dataGridView)).BeginInit();
            this.Grid_panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearAllFiltersToolStripMenuItem,
            this.showAllColumnsToolStripMenuItem,
            this.refreshPageToolStripMenuItem,
            this.goToToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(198, 100);
            // 
            // clearAllFiltersToolStripMenuItem
            // 
            this.clearAllFiltersToolStripMenuItem.Name = "clearAllFiltersToolStripMenuItem";
            this.clearAllFiltersToolStripMenuItem.Size = new System.Drawing.Size(197, 24);
            this.clearAllFiltersToolStripMenuItem.Text = "Clear all filters";
            this.clearAllFiltersToolStripMenuItem.Click += new System.EventHandler(this.clearAllFiltersToolStripMenuItem_Click);
            // 
            // showAllColumnsToolStripMenuItem
            // 
            this.showAllColumnsToolStripMenuItem.Name = "showAllColumnsToolStripMenuItem";
            this.showAllColumnsToolStripMenuItem.Size = new System.Drawing.Size(197, 24);
            this.showAllColumnsToolStripMenuItem.Text = "Show All Columns";
            this.showAllColumnsToolStripMenuItem.Click += new System.EventHandler(this.showAllColumnsToolStripMenuItem_Click);
            // 
            // refreshPageToolStripMenuItem
            // 
            this.refreshPageToolStripMenuItem.Name = "refreshPageToolStripMenuItem";
            this.refreshPageToolStripMenuItem.Size = new System.Drawing.Size(197, 24);
            this.refreshPageToolStripMenuItem.Text = "Refresh Page";
            this.refreshPageToolStripMenuItem.Click += new System.EventHandler(this.refreshPageToolStripMenuItem_Click);
            // 
            // goToToolStripMenuItem
            // 
            this.goToToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.searchToolStripMenuItem,
            this.statisticsToolStripMenuItem,
            this.taskBoardToolStripMenuItem});
            this.goToToolStripMenuItem.Name = "goToToolStripMenuItem";
            this.goToToolStripMenuItem.Size = new System.Drawing.Size(197, 24);
            this.goToToolStripMenuItem.Text = "Go To";
            // 
            // searchToolStripMenuItem
            // 
            this.searchToolStripMenuItem.Name = "searchToolStripMenuItem";
            this.searchToolStripMenuItem.Size = new System.Drawing.Size(163, 26);
            this.searchToolStripMenuItem.Text = "Search";
            this.searchToolStripMenuItem.Click += new System.EventHandler(this.searchToolStripMenuItem_Click);
            // 
            // statisticsToolStripMenuItem
            // 
            this.statisticsToolStripMenuItem.Name = "statisticsToolStripMenuItem";
            this.statisticsToolStripMenuItem.Size = new System.Drawing.Size(163, 26);
            this.statisticsToolStripMenuItem.Text = "Statistics";
            this.statisticsToolStripMenuItem.Click += new System.EventHandler(this.statisticsToolStripMenuItem_Click);
            // 
            // taskBoardToolStripMenuItem
            // 
            this.taskBoardToolStripMenuItem.Name = "taskBoardToolStripMenuItem";
            this.taskBoardToolStripMenuItem.Size = new System.Drawing.Size(163, 26);
            this.taskBoardToolStripMenuItem.Text = "Task Board";
            this.taskBoardToolStripMenuItem.Click += new System.EventHandler(this.taskBoardToolStripMenuItem_Click);
            // 
            // Top_tableLayoutPanel
            // 
            this.Top_tableLayoutPanel.BackColor = System.Drawing.Color.Transparent;
            this.Top_tableLayoutPanel.ColumnCount = 9;
            this.Top_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10.83591F));
            this.Top_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 24.5614F));
            this.Top_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.77399F));
            this.Top_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 13.82869F));
            this.Top_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.Top_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 93F));
            this.Top_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.Top_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 58F));
            this.Top_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 101F));
            this.Top_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.Top_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.Top_tableLayoutPanel.ContextMenuStrip = this.contextMenuStrip1;
            this.Top_tableLayoutPanel.Controls.Add(this.TaskType_comboBox, 1, 0);
            this.Top_tableLayoutPanel.Controls.Add(this.Counter_textBox, 5, 0);
            this.Top_tableLayoutPanel.Controls.Add(this.Refresh_button, 6, 0);
            this.Top_tableLayoutPanel.Controls.Add(this.ExactSearch_checkBox, 3, 0);
            this.Top_tableLayoutPanel.Controls.Add(this.ShowHide_button, 8, 0);
            this.Top_tableLayoutPanel.Controls.Add(this.ExportToExcel_button, 7, 0);
            this.Top_tableLayoutPanel.Controls.Add(this.Search_TxtBox, 2, 0);
            this.Top_tableLayoutPanel.Controls.Add(this.label5, 0, 0);
            this.Top_tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.Top_tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.Top_tableLayoutPanel.Margin = new System.Windows.Forms.Padding(4);
            this.Top_tableLayoutPanel.Name = "Top_tableLayoutPanel";
            this.Top_tableLayoutPanel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Top_tableLayoutPanel.RowCount = 1;
            this.Top_tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.Top_tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 43F));
            this.Top_tableLayoutPanel.Size = new System.Drawing.Size(1252, 43);
            this.Top_tableLayoutPanel.TabIndex = 1;
            // 
            // TaskType_comboBox
            // 
            this.TaskType_comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.TaskType_comboBox.BackColor = System.Drawing.Color.White;
            this.TaskType_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TaskType_comboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.TaskType_comboBox.ForeColor = System.Drawing.Color.Black;
            this.TaskType_comboBox.FormattingEnabled = true;
            this.TaskType_comboBox.Items.AddRange(new object[] {
            "مرحلة ماقبل أخذ القرار",
            "مرحلة التمويل"});
            this.TaskType_comboBox.Location = new System.Drawing.Point(921, 4);
            this.TaskType_comboBox.Margin = new System.Windows.Forms.Padding(2);
            this.TaskType_comboBox.Name = "TaskType_comboBox";
            this.TaskType_comboBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.TaskType_comboBox.Size = new System.Drawing.Size(227, 35);
            this.TaskType_comboBox.TabIndex = 485;
            this.TaskType_comboBox.SelectedIndexChanged += new System.EventHandler(this.TaskType_comboBox_SelectedIndexChanged);
            // 
            // Counter_textBox
            // 
            this.Counter_textBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.Counter_textBox.BackColor = System.Drawing.Color.White;
            this.Counter_textBox.ForeColor = System.Drawing.Color.Black;
            this.Counter_textBox.HideSelection = false;
            this.Counter_textBox.Location = new System.Drawing.Point(214, 7);
            this.Counter_textBox.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.Counter_textBox.Name = "Counter_textBox";
            this.Counter_textBox.ReadOnly = true;
            this.Counter_textBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Counter_textBox.Size = new System.Drawing.Size(85, 29);
            this.Counter_textBox.TabIndex = 409;
            this.Counter_textBox.TabStop = false;
            // 
            // Refresh_button
            // 
            this.Refresh_button.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Refresh_button.BackgroundImage = global::MyWorkApplication.Properties.Resources.Refresh2_D;
            this.Refresh_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Refresh_button.FlatAppearance.BorderSize = 0;
            this.Refresh_button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.Refresh_button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.Refresh_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Refresh_button.Location = new System.Drawing.Point(164, 7);
            this.Refresh_button.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.Refresh_button.MaximumSize = new System.Drawing.Size(44, 29);
            this.Refresh_button.MinimumSize = new System.Drawing.Size(44, 29);
            this.Refresh_button.Name = "Refresh_button";
            this.Refresh_button.Size = new System.Drawing.Size(44, 29);
            this.Refresh_button.TabIndex = 413;
            this.Refresh_button.UseVisualStyleBackColor = true;
            this.Refresh_button.Click += new System.EventHandler(this.Refresh_button_Click);
            this.Refresh_button.MouseEnter += new System.EventHandler(this.Refresh_button_MouseEnter);
            this.Refresh_button.MouseLeave += new System.EventHandler(this.Refresh_button_MouseLeave);
            // 
            // ExactSearch_checkBox
            // 
            this.ExactSearch_checkBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ExactSearch_checkBox.AutoSize = true;
            this.ExactSearch_checkBox.Location = new System.Drawing.Point(330, 6);
            this.ExactSearch_checkBox.Margin = new System.Windows.Forms.Padding(4);
            this.ExactSearch_checkBox.Name = "ExactSearch_checkBox";
            this.ExactSearch_checkBox.Size = new System.Drawing.Size(107, 31);
            this.ExactSearch_checkBox.TabIndex = 2;
            this.ExactSearch_checkBox.Text = "تطابق كلي";
            this.ExactSearch_checkBox.UseVisualStyleBackColor = true;
            this.ExactSearch_checkBox.CheckedChanged += new System.EventHandler(this.ExactSearch_checkBox_CheckedChanged);
            // 
            // ShowHide_button
            // 
            this.ShowHide_button.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ShowHide_button.BackColor = System.Drawing.Color.Transparent;
            this.ShowHide_button.BackgroundImage = global::MyWorkApplication.Properties.Resources.Down_D;
            this.ShowHide_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ShowHide_button.FlatAppearance.BorderSize = 0;
            this.ShowHide_button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.ShowHide_button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.ShowHide_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ShowHide_button.Location = new System.Drawing.Point(56, 8);
            this.ShowHide_button.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.ShowHide_button.MaximumSize = new System.Drawing.Size(42, 27);
            this.ShowHide_button.MinimumSize = new System.Drawing.Size(42, 27);
            this.ShowHide_button.Name = "ShowHide_button";
            this.ShowHide_button.Size = new System.Drawing.Size(42, 27);
            this.ShowHide_button.TabIndex = 445;
            this.ShowHide_button.UseVisualStyleBackColor = false;
            this.ShowHide_button.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ShowHide_button_MouseClick);
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
            this.ExportToExcel_button.Location = new System.Drawing.Point(108, 7);
            this.ExportToExcel_button.Margin = new System.Windows.Forms.Padding(6, 2, 6, 2);
            this.ExportToExcel_button.MaximumSize = new System.Drawing.Size(44, 29);
            this.ExportToExcel_button.MinimumSize = new System.Drawing.Size(44, 29);
            this.ExportToExcel_button.Name = "ExportToExcel_button";
            this.ExportToExcel_button.Size = new System.Drawing.Size(44, 29);
            this.ExportToExcel_button.TabIndex = 446;
            this.ExportToExcel_button.UseVisualStyleBackColor = false;
            this.ExportToExcel_button.Click += new System.EventHandler(this.ExportToExcel_button_Click);
            this.ExportToExcel_button.MouseEnter += new System.EventHandler(this.ExportToExcel_button_MouseEnter);
            this.ExportToExcel_button.MouseLeave += new System.EventHandler(this.ExportToExcel_button_MouseLeave);
            // 
            // Search_TxtBox
            // 
            this.Search_TxtBox.BackColor = System.Drawing.Color.White;
            this.Search_TxtBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Search_TxtBox.ForeColor = System.Drawing.Color.Black;
            this.Search_TxtBox.Location = new System.Drawing.Point(448, 5);
            this.Search_TxtBox.Margin = new System.Windows.Forms.Padding(7, 5, 7, 5);
            this.Search_TxtBox.Multiline = true;
            this.Search_TxtBox.Name = "Search_TxtBox";
            this.Search_TxtBox.Size = new System.Drawing.Size(464, 33);
            this.Search_TxtBox.TabIndex = 1;
            this.Search_TxtBox.TextChanged += new System.EventHandler(this.MP_idTxtBox_TextChanged);
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(1157, 8);
            this.label5.Margin = new System.Windows.Forms.Padding(7, 5, 7, 5);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 27);
            this.label5.TabIndex = 0;
            this.label5.Text = "بحث:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Filters_tableLayoutPanel
            // 
            this.Filters_tableLayoutPanel.AutoScroll = true;
            this.Filters_tableLayoutPanel.AutoSize = true;
            this.Filters_tableLayoutPanel.ColumnCount = 33;
            this.Filters_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 41F));
            this.Filters_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 58F));
            this.Filters_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.Filters_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.Filters_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.Filters_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 76F));
            this.Filters_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 76F));
            this.Filters_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.Filters_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 76F));
            this.Filters_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 76F));
            this.Filters_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 76F));
            this.Filters_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 76F));
            this.Filters_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 76F));
            this.Filters_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 76F));
            this.Filters_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 76F));
            this.Filters_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 76F));
            this.Filters_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 76F));
            this.Filters_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 76F));
            this.Filters_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 76F));
            this.Filters_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 76F));
            this.Filters_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 76F));
            this.Filters_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 76F));
            this.Filters_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 76F));
            this.Filters_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 76F));
            this.Filters_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 76F));
            this.Filters_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 76F));
            this.Filters_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 76F));
            this.Filters_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 76F));
            this.Filters_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 76F));
            this.Filters_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 76F));
            this.Filters_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 76F));
            this.Filters_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 76F));
            this.Filters_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.Filters_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.Filters_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.Filters_tableLayoutPanel.Controls.Add(this.f30_label, 31, 0);
            this.Filters_tableLayoutPanel.Controls.Add(this.f29_label, 30, 0);
            this.Filters_tableLayoutPanel.Controls.Add(this.MP_dataGridView, 0, 3);
            this.Filters_tableLayoutPanel.Controls.Add(this.f06_Type_comboBox, 7, 2);
            this.Filters_tableLayoutPanel.Controls.Add(this.f04_State_comboBox, 5, 2);
            this.Filters_tableLayoutPanel.Controls.Add(this.f05_Donor_comboBox, 6, 2);
            this.Filters_tableLayoutPanel.Controls.Add(this.f07_comboBox, 8, 2);
            this.Filters_tableLayoutPanel.Controls.Add(this.f07_label, 8, 0);
            this.Filters_tableLayoutPanel.Controls.Add(this.f8_label, 9, 0);
            this.Filters_tableLayoutPanel.Controls.Add(this.Finished_button, 0, 2);
            this.Filters_tableLayoutPanel.Controls.Add(this.Cancelled_button, 1, 2);
            this.Filters_tableLayoutPanel.Controls.Add(this.Closed_button, 2, 2);
            this.Filters_tableLayoutPanel.Controls.Add(this.f14_comboBox, 15, 2);
            this.Filters_tableLayoutPanel.Controls.Add(this.f14_label, 15, 0);
            this.Filters_tableLayoutPanel.Controls.Add(this.f13_comboBox, 14, 2);
            this.Filters_tableLayoutPanel.Controls.Add(this.f13_label, 14, 0);
            this.Filters_tableLayoutPanel.Controls.Add(this.f12_comboBox, 13, 2);
            this.Filters_tableLayoutPanel.Controls.Add(this.f12_label, 13, 0);
            this.Filters_tableLayoutPanel.Controls.Add(this.f11_comboBox, 12, 2);
            this.Filters_tableLayoutPanel.Controls.Add(this.f11_label, 12, 0);
            this.Filters_tableLayoutPanel.Controls.Add(this.f10_comboBox, 11, 2);
            this.Filters_tableLayoutPanel.Controls.Add(this.f10_label, 11, 0);
            this.Filters_tableLayoutPanel.Controls.Add(this.f09_comboBox, 10, 2);
            this.Filters_tableLayoutPanel.Controls.Add(this.f09_label, 10, 0);
            this.Filters_tableLayoutPanel.Controls.Add(this.f8_comboBox, 9, 2);
            this.Filters_tableLayoutPanel.Controls.Add(this.f28_comboBox, 29, 2);
            this.Filters_tableLayoutPanel.Controls.Add(this.f28_label, 29, 0);
            this.Filters_tableLayoutPanel.Controls.Add(this.f27_comboBox, 28, 2);
            this.Filters_tableLayoutPanel.Controls.Add(this.f27_label, 28, 0);
            this.Filters_tableLayoutPanel.Controls.Add(this.f26_comboBox, 27, 2);
            this.Filters_tableLayoutPanel.Controls.Add(this.f26_label, 27, 0);
            this.Filters_tableLayoutPanel.Controls.Add(this.f25_comboBox, 26, 2);
            this.Filters_tableLayoutPanel.Controls.Add(this.f25_label, 26, 0);
            this.Filters_tableLayoutPanel.Controls.Add(this.f24_comboBox, 25, 2);
            this.Filters_tableLayoutPanel.Controls.Add(this.f24_label, 25, 0);
            this.Filters_tableLayoutPanel.Controls.Add(this.f23_comboBox, 24, 2);
            this.Filters_tableLayoutPanel.Controls.Add(this.f23_label, 24, 0);
            this.Filters_tableLayoutPanel.Controls.Add(this.f22_comboBox, 23, 2);
            this.Filters_tableLayoutPanel.Controls.Add(this.f22_label, 23, 0);
            this.Filters_tableLayoutPanel.Controls.Add(this.f21_comboBox, 22, 2);
            this.Filters_tableLayoutPanel.Controls.Add(this.f21_label, 22, 0);
            this.Filters_tableLayoutPanel.Controls.Add(this.f20_comboBox, 21, 2);
            this.Filters_tableLayoutPanel.Controls.Add(this.f20_label, 21, 0);
            this.Filters_tableLayoutPanel.Controls.Add(this.f19_comboBox, 20, 2);
            this.Filters_tableLayoutPanel.Controls.Add(this.f19_label, 20, 0);
            this.Filters_tableLayoutPanel.Controls.Add(this.f18_comboBox, 19, 2);
            this.Filters_tableLayoutPanel.Controls.Add(this.f18_label, 19, 0);
            this.Filters_tableLayoutPanel.Controls.Add(this.f17_comboBox, 18, 2);
            this.Filters_tableLayoutPanel.Controls.Add(this.f17_label, 18, 0);
            this.Filters_tableLayoutPanel.Controls.Add(this.f29_comboBox, 30, 2);
            this.Filters_tableLayoutPanel.Controls.Add(this.f30_comboBox, 31, 2);
            this.Filters_tableLayoutPanel.Controls.Add(this.f16_comboBox, 17, 2);
            this.Filters_tableLayoutPanel.Controls.Add(this.f16_label, 17, 0);
            this.Filters_tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Filters_tableLayoutPanel.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.AddColumns;
            this.Filters_tableLayoutPanel.Location = new System.Drawing.Point(0, 43);
            this.Filters_tableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.Filters_tableLayoutPanel.Name = "Filters_tableLayoutPanel";
            this.Filters_tableLayoutPanel.RowCount = 4;
            this.Filters_tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.Filters_tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.Filters_tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.Filters_tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.Filters_tableLayoutPanel.Size = new System.Drawing.Size(1252, 703);
            this.Filters_tableLayoutPanel.TabIndex = 3;
            // 
            // f30_label
            // 
            this.f30_label.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.f30_label.AutoEllipsis = true;
            this.f30_label.AutoSize = true;
            this.f30_label.BackColor = System.Drawing.Color.Transparent;
            this.f30_label.Font = new System.Drawing.Font("Rubik", 10F);
            this.f30_label.Location = new System.Drawing.Point(14, 0);
            this.f30_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.f30_label.Name = "f30_label";
            this.Filters_tableLayoutPanel.SetRowSpan(this.f30_label, 2);
            this.f30_label.Size = new System.Drawing.Size(68, 78);
            this.f30_label.TabIndex = 488;
            this.f30_label.Text = "label2";
            this.f30_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // f29_label
            // 
            this.f29_label.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.f29_label.AutoEllipsis = true;
            this.f29_label.AutoSize = true;
            this.f29_label.BackColor = System.Drawing.Color.Transparent;
            this.f29_label.Font = new System.Drawing.Font("Rubik", 10F);
            this.f29_label.Location = new System.Drawing.Point(90, 0);
            this.f29_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.f29_label.Name = "f29_label";
            this.Filters_tableLayoutPanel.SetRowSpan(this.f29_label, 2);
            this.f29_label.Size = new System.Drawing.Size(68, 78);
            this.f29_label.TabIndex = 487;
            this.f29_label.Text = "label1";
            this.f29_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MP_dataGridView
            // 
            this.MP_dataGridView.AllowUserToAddRows = false;
            this.MP_dataGridView.AllowUserToDeleteRows = false;
            this.MP_dataGridView.AllowUserToResizeColumns = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.MP_dataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.MP_dataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MP_dataGridView.BackgroundColor = System.Drawing.Color.White;
            this.MP_dataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.MP_dataGridView.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.MP_dataGridView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Rubik", 10.8F);
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(3);
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.MP_dataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.MP_dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Filters_tableLayoutPanel.SetColumnSpan(this.MP_dataGridView, 33);
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Rubik", 10.8F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.MP_dataGridView.DefaultCellStyle = dataGridViewCellStyle3;
            this.MP_dataGridView.EnableHeadersVisualStyles = false;
            this.MP_dataGridView.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.MP_dataGridView.Location = new System.Drawing.Point(4, 121);
            this.MP_dataGridView.Margin = new System.Windows.Forms.Padding(4);
            this.MP_dataGridView.Name = "MP_dataGridView";
            this.MP_dataGridView.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.MP_dataGridView.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Rubik", 10.8F);
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.MP_dataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.MP_dataGridView.RowHeadersWidth = 40;
            this.MP_dataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.MP_dataGridView.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.MP_dataGridView.RowTemplate.Height = 33;
            this.MP_dataGridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.MP_dataGridView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.MP_dataGridView.Size = new System.Drawing.Size(2677, 559);
            this.MP_dataGridView.TabIndex = 4;
            this.MP_dataGridView.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.MP_dataGridView_CellBeginEdit);
            this.MP_dataGridView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.MP_dataGridView_CellDoubleClick);
            this.MP_dataGridView.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.MP_dataGridView_CellEndEdit);
            this.MP_dataGridView.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.MP_dataGridView_ColumnHeaderMouseClick);
            this.MP_dataGridView.ColumnStateChanged += new System.Windows.Forms.DataGridViewColumnStateChangedEventHandler(this.MP_dataGridView_ColumnStateChanged);
            this.MP_dataGridView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.MP_dataGridView_DataError);
            this.MP_dataGridView.RowHeaderMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.MP_dataGridView_RowHeaderMouseDoubleClick);
            this.MP_dataGridView.Sorted += new System.EventHandler(this.MP_dataGridView_Sorted);
            // 
            // f06_Type_comboBox
            // 
            this.f06_Type_comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.f06_Type_comboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.f06_Type_comboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.f06_Type_comboBox.BackColor = System.Drawing.Color.White;
            this.f06_Type_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.f06_Type_comboBox.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.f06_Type_comboBox.ForeColor = System.Drawing.Color.Black;
            this.f06_Type_comboBox.FormattingEnabled = true;
            this.f06_Type_comboBox.Location = new System.Drawing.Point(1836, 80);
            this.f06_Type_comboBox.Margin = new System.Windows.Forms.Padding(2);
            this.f06_Type_comboBox.Name = "f06_Type_comboBox";
            this.f06_Type_comboBox.Size = new System.Drawing.Size(96, 35);
            this.f06_Type_comboBox.TabIndex = 12;
            this.f06_Type_comboBox.SelectedIndexChanged += new System.EventHandler(this.State_comboBox_SelectedIndexChanged);
            // 
            // f04_State_comboBox
            // 
            this.f04_State_comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.f04_State_comboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.f04_State_comboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.f04_State_comboBox.BackColor = System.Drawing.Color.White;
            this.f04_State_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.f04_State_comboBox.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.f04_State_comboBox.ForeColor = System.Drawing.Color.Black;
            this.f04_State_comboBox.FormattingEnabled = true;
            this.f04_State_comboBox.Items.AddRange(new object[] {
            "",
            "بالانتظار",
            "قيد الدراسة",
            "منسحب",
            "مرفوض",
            "مؤجل",
            "مقبول",
            "ممول",
            "منتهي",
            "ملغى",
            "مغلق"});
            this.f04_State_comboBox.Location = new System.Drawing.Point(2012, 80);
            this.f04_State_comboBox.Margin = new System.Windows.Forms.Padding(2);
            this.f04_State_comboBox.Name = "f04_State_comboBox";
            this.f04_State_comboBox.Size = new System.Drawing.Size(72, 35);
            this.f04_State_comboBox.TabIndex = 10;
            this.f04_State_comboBox.SelectedIndexChanged += new System.EventHandler(this.State_comboBox_SelectedIndexChanged);
            // 
            // f05_Donor_comboBox
            // 
            this.f05_Donor_comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.f05_Donor_comboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.f05_Donor_comboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.f05_Donor_comboBox.BackColor = System.Drawing.Color.White;
            this.f05_Donor_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.f05_Donor_comboBox.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.f05_Donor_comboBox.ForeColor = System.Drawing.Color.Black;
            this.f05_Donor_comboBox.FormattingEnabled = true;
            this.f05_Donor_comboBox.Location = new System.Drawing.Point(1936, 80);
            this.f05_Donor_comboBox.Margin = new System.Windows.Forms.Padding(2);
            this.f05_Donor_comboBox.Name = "f05_Donor_comboBox";
            this.f05_Donor_comboBox.Size = new System.Drawing.Size(72, 35);
            this.f05_Donor_comboBox.TabIndex = 11;
            this.f05_Donor_comboBox.SelectedIndexChanged += new System.EventHandler(this.State_comboBox_SelectedIndexChanged);
            // 
            // f07_comboBox
            // 
            this.f07_comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.f07_comboBox.BackColor = System.Drawing.Color.White;
            this.f07_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.f07_comboBox.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.f07_comboBox.Font = new System.Drawing.Font("Segoe UI Symbol", 11F);
            this.f07_comboBox.ForeColor = System.Drawing.Color.Black;
            this.f07_comboBox.FormattingEnabled = true;
            this.f07_comboBox.Items.AddRange(new object[] {
            "",
            "",
            ""});
            this.f07_comboBox.Location = new System.Drawing.Point(1762, 81);
            this.f07_comboBox.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.f07_comboBox.Name = "f07_comboBox";
            this.f07_comboBox.Size = new System.Drawing.Size(68, 33);
            this.f07_comboBox.TabIndex = 13;
            this.f07_comboBox.SelectedIndexChanged += new System.EventHandler(this.State_comboBox_SelectedIndexChanged);
            // 
            // f07_label
            // 
            this.f07_label.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.f07_label.AutoEllipsis = true;
            this.f07_label.AutoSize = true;
            this.f07_label.BackColor = System.Drawing.Color.Transparent;
            this.f07_label.Font = new System.Drawing.Font("Rubik", 10F);
            this.f07_label.Location = new System.Drawing.Point(1762, 0);
            this.f07_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.f07_label.Name = "f07_label";
            this.Filters_tableLayoutPanel.SetRowSpan(this.f07_label, 2);
            this.f07_label.Size = new System.Drawing.Size(68, 78);
            this.f07_label.TabIndex = 456;
            this.f07_label.Text = "label1";
            this.f07_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // f8_label
            // 
            this.f8_label.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.f8_label.AutoEllipsis = true;
            this.f8_label.AutoSize = true;
            this.f8_label.BackColor = System.Drawing.Color.Transparent;
            this.f8_label.Font = new System.Drawing.Font("Rubik", 10F);
            this.f8_label.Location = new System.Drawing.Point(1686, 0);
            this.f8_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.f8_label.Name = "f8_label";
            this.Filters_tableLayoutPanel.SetRowSpan(this.f8_label, 2);
            this.f8_label.Size = new System.Drawing.Size(68, 78);
            this.f8_label.TabIndex = 479;
            this.f8_label.Text = "label2";
            this.f8_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Finished_button
            // 
            this.Finished_button.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Finished_button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(73)))), ((int)(((byte)(106)))));
            this.Finished_button.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Finished_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Finished_button.Location = new System.Drawing.Point(2648, 86);
            this.Finished_button.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.Finished_button.MaximumSize = new System.Drawing.Size(45, 29);
            this.Finished_button.MinimumSize = new System.Drawing.Size(35, 23);
            this.Finished_button.Name = "Finished_button";
            this.Finished_button.Size = new System.Drawing.Size(35, 23);
            this.Finished_button.TabIndex = 482;
            this.Finished_button.UseVisualStyleBackColor = false;
            this.Finished_button.Visible = false;
            // 
            // Cancelled_button
            // 
            this.Cancelled_button.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Cancelled_button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.Cancelled_button.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Cancelled_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Cancelled_button.Location = new System.Drawing.Point(2595, 86);
            this.Cancelled_button.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.Cancelled_button.MaximumSize = new System.Drawing.Size(45, 29);
            this.Cancelled_button.MinimumSize = new System.Drawing.Size(35, 23);
            this.Cancelled_button.Name = "Cancelled_button";
            this.Cancelled_button.Size = new System.Drawing.Size(45, 23);
            this.Cancelled_button.TabIndex = 483;
            this.Cancelled_button.UseVisualStyleBackColor = false;
            this.Cancelled_button.Visible = false;
            // 
            // Closed_button
            // 
            this.Closed_button.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Closed_button.BackColor = System.Drawing.Color.PeachPuff;
            this.Closed_button.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Closed_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Closed_button.Location = new System.Drawing.Point(2537, 86);
            this.Closed_button.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.Closed_button.MaximumSize = new System.Drawing.Size(45, 29);
            this.Closed_button.MinimumSize = new System.Drawing.Size(35, 23);
            this.Closed_button.Name = "Closed_button";
            this.Closed_button.Size = new System.Drawing.Size(45, 23);
            this.Closed_button.TabIndex = 484;
            this.Closed_button.UseVisualStyleBackColor = false;
            this.Closed_button.Visible = false;
            // 
            // f14_comboBox
            // 
            this.f14_comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.f14_comboBox.BackColor = System.Drawing.Color.White;
            this.f14_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.f14_comboBox.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.f14_comboBox.Font = new System.Drawing.Font("Segoe UI Symbol", 11F);
            this.f14_comboBox.ForeColor = System.Drawing.Color.Black;
            this.f14_comboBox.FormattingEnabled = true;
            this.f14_comboBox.Items.AddRange(new object[] {
            "",
            "",
            ""});
            this.f14_comboBox.Location = new System.Drawing.Point(1230, 81);
            this.f14_comboBox.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.f14_comboBox.Name = "f14_comboBox";
            this.f14_comboBox.Size = new System.Drawing.Size(68, 33);
            this.f14_comboBox.TabIndex = 20;
            this.f14_comboBox.SelectedIndexChanged += new System.EventHandler(this.State_comboBox_SelectedIndexChanged);
            // 
            // f14_label
            // 
            this.f14_label.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.f14_label.AutoEllipsis = true;
            this.f14_label.AutoSize = true;
            this.f14_label.BackColor = System.Drawing.Color.Transparent;
            this.f14_label.Font = new System.Drawing.Font("Rubik", 10F);
            this.f14_label.Location = new System.Drawing.Point(1230, 0);
            this.f14_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.f14_label.Name = "f14_label";
            this.Filters_tableLayoutPanel.SetRowSpan(this.f14_label, 2);
            this.f14_label.Size = new System.Drawing.Size(68, 78);
            this.f14_label.TabIndex = 463;
            this.f14_label.Text = "label1";
            this.f14_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // f13_comboBox
            // 
            this.f13_comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.f13_comboBox.BackColor = System.Drawing.Color.White;
            this.f13_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.f13_comboBox.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.f13_comboBox.Font = new System.Drawing.Font("Segoe UI Symbol", 11F);
            this.f13_comboBox.ForeColor = System.Drawing.Color.Black;
            this.f13_comboBox.FormattingEnabled = true;
            this.f13_comboBox.Items.AddRange(new object[] {
            "",
            "",
            ""});
            this.f13_comboBox.Location = new System.Drawing.Point(1306, 81);
            this.f13_comboBox.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.f13_comboBox.Name = "f13_comboBox";
            this.f13_comboBox.Size = new System.Drawing.Size(68, 33);
            this.f13_comboBox.TabIndex = 19;
            this.f13_comboBox.SelectedIndexChanged += new System.EventHandler(this.State_comboBox_SelectedIndexChanged);
            // 
            // f13_label
            // 
            this.f13_label.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.f13_label.AutoEllipsis = true;
            this.f13_label.AutoSize = true;
            this.f13_label.BackColor = System.Drawing.Color.Transparent;
            this.f13_label.Font = new System.Drawing.Font("Rubik", 10F);
            this.f13_label.Location = new System.Drawing.Point(1306, 0);
            this.f13_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.f13_label.Name = "f13_label";
            this.Filters_tableLayoutPanel.SetRowSpan(this.f13_label, 2);
            this.f13_label.Size = new System.Drawing.Size(68, 78);
            this.f13_label.TabIndex = 462;
            this.f13_label.Text = "label1";
            this.f13_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // f12_comboBox
            // 
            this.f12_comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.f12_comboBox.BackColor = System.Drawing.Color.White;
            this.f12_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.f12_comboBox.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.f12_comboBox.Font = new System.Drawing.Font("Segoe UI Symbol", 11F);
            this.f12_comboBox.ForeColor = System.Drawing.Color.Black;
            this.f12_comboBox.FormattingEnabled = true;
            this.f12_comboBox.Items.AddRange(new object[] {
            "",
            "",
            ""});
            this.f12_comboBox.Location = new System.Drawing.Point(1382, 81);
            this.f12_comboBox.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.f12_comboBox.Name = "f12_comboBox";
            this.f12_comboBox.Size = new System.Drawing.Size(68, 33);
            this.f12_comboBox.TabIndex = 18;
            this.f12_comboBox.SelectedIndexChanged += new System.EventHandler(this.State_comboBox_SelectedIndexChanged);
            // 
            // f12_label
            // 
            this.f12_label.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.f12_label.AutoEllipsis = true;
            this.f12_label.AutoSize = true;
            this.f12_label.BackColor = System.Drawing.Color.Transparent;
            this.f12_label.Font = new System.Drawing.Font("Rubik", 10F);
            this.f12_label.Location = new System.Drawing.Point(1382, 0);
            this.f12_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.f12_label.Name = "f12_label";
            this.Filters_tableLayoutPanel.SetRowSpan(this.f12_label, 2);
            this.f12_label.Size = new System.Drawing.Size(68, 78);
            this.f12_label.TabIndex = 461;
            this.f12_label.Text = "label1";
            this.f12_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // f11_comboBox
            // 
            this.f11_comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.f11_comboBox.BackColor = System.Drawing.Color.White;
            this.f11_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.f11_comboBox.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.f11_comboBox.Font = new System.Drawing.Font("Segoe UI Symbol", 11F);
            this.f11_comboBox.ForeColor = System.Drawing.Color.Black;
            this.f11_comboBox.FormattingEnabled = true;
            this.f11_comboBox.Items.AddRange(new object[] {
            "",
            "",
            ""});
            this.f11_comboBox.Location = new System.Drawing.Point(1458, 81);
            this.f11_comboBox.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.f11_comboBox.Name = "f11_comboBox";
            this.f11_comboBox.Size = new System.Drawing.Size(68, 33);
            this.f11_comboBox.TabIndex = 17;
            this.f11_comboBox.SelectedIndexChanged += new System.EventHandler(this.State_comboBox_SelectedIndexChanged);
            // 
            // f11_label
            // 
            this.f11_label.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.f11_label.AutoEllipsis = true;
            this.f11_label.AutoSize = true;
            this.f11_label.BackColor = System.Drawing.Color.Transparent;
            this.f11_label.Font = new System.Drawing.Font("Rubik", 10F);
            this.f11_label.Location = new System.Drawing.Point(1458, 0);
            this.f11_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.f11_label.Name = "f11_label";
            this.Filters_tableLayoutPanel.SetRowSpan(this.f11_label, 2);
            this.f11_label.Size = new System.Drawing.Size(68, 78);
            this.f11_label.TabIndex = 460;
            this.f11_label.Text = "label1";
            this.f11_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // f10_comboBox
            // 
            this.f10_comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.f10_comboBox.BackColor = System.Drawing.Color.White;
            this.f10_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.f10_comboBox.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.f10_comboBox.Font = new System.Drawing.Font("Segoe UI Symbol", 11F);
            this.f10_comboBox.ForeColor = System.Drawing.Color.Black;
            this.f10_comboBox.FormattingEnabled = true;
            this.f10_comboBox.Items.AddRange(new object[] {
            "",
            "",
            ""});
            this.f10_comboBox.Location = new System.Drawing.Point(1534, 81);
            this.f10_comboBox.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.f10_comboBox.Name = "f10_comboBox";
            this.f10_comboBox.Size = new System.Drawing.Size(68, 33);
            this.f10_comboBox.TabIndex = 16;
            this.f10_comboBox.SelectedIndexChanged += new System.EventHandler(this.State_comboBox_SelectedIndexChanged);
            // 
            // f10_label
            // 
            this.f10_label.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.f10_label.AutoEllipsis = true;
            this.f10_label.AutoSize = true;
            this.f10_label.BackColor = System.Drawing.Color.Transparent;
            this.f10_label.Font = new System.Drawing.Font("Rubik", 10F);
            this.f10_label.Location = new System.Drawing.Point(1534, 0);
            this.f10_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.f10_label.Name = "f10_label";
            this.Filters_tableLayoutPanel.SetRowSpan(this.f10_label, 2);
            this.f10_label.Size = new System.Drawing.Size(68, 78);
            this.f10_label.TabIndex = 459;
            this.f10_label.Text = "label1";
            this.f10_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // f09_comboBox
            // 
            this.f09_comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.f09_comboBox.BackColor = System.Drawing.Color.White;
            this.f09_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.f09_comboBox.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.f09_comboBox.Font = new System.Drawing.Font("Segoe UI Symbol", 11F);
            this.f09_comboBox.ForeColor = System.Drawing.Color.Black;
            this.f09_comboBox.FormattingEnabled = true;
            this.f09_comboBox.Items.AddRange(new object[] {
            "",
            "",
            ""});
            this.f09_comboBox.Location = new System.Drawing.Point(1610, 81);
            this.f09_comboBox.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.f09_comboBox.Name = "f09_comboBox";
            this.f09_comboBox.Size = new System.Drawing.Size(68, 33);
            this.f09_comboBox.TabIndex = 15;
            this.f09_comboBox.SelectedIndexChanged += new System.EventHandler(this.State_comboBox_SelectedIndexChanged);
            // 
            // f09_label
            // 
            this.f09_label.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.f09_label.AutoEllipsis = true;
            this.f09_label.AutoSize = true;
            this.f09_label.BackColor = System.Drawing.Color.Transparent;
            this.f09_label.Font = new System.Drawing.Font("Rubik", 10F);
            this.f09_label.Location = new System.Drawing.Point(1610, 0);
            this.f09_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.f09_label.Name = "f09_label";
            this.Filters_tableLayoutPanel.SetRowSpan(this.f09_label, 2);
            this.f09_label.Size = new System.Drawing.Size(68, 78);
            this.f09_label.TabIndex = 458;
            this.f09_label.Text = "label1";
            this.f09_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // f8_comboBox
            // 
            this.f8_comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.f8_comboBox.BackColor = System.Drawing.Color.White;
            this.f8_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.f8_comboBox.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.f8_comboBox.Font = new System.Drawing.Font("Segoe UI Symbol", 11F);
            this.f8_comboBox.ForeColor = System.Drawing.Color.Black;
            this.f8_comboBox.FormattingEnabled = true;
            this.f8_comboBox.Items.AddRange(new object[] {
            "",
            "",
            ""});
            this.f8_comboBox.Location = new System.Drawing.Point(1686, 81);
            this.f8_comboBox.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.f8_comboBox.Name = "f8_comboBox";
            this.f8_comboBox.Size = new System.Drawing.Size(68, 33);
            this.f8_comboBox.TabIndex = 481;
            this.f8_comboBox.SelectedIndexChanged += new System.EventHandler(this.State_comboBox_SelectedIndexChanged);
            // 
            // f28_comboBox
            // 
            this.f28_comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.f28_comboBox.BackColor = System.Drawing.Color.White;
            this.f28_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.f28_comboBox.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.f28_comboBox.Font = new System.Drawing.Font("Segoe UI Symbol", 11F);
            this.f28_comboBox.ForeColor = System.Drawing.Color.Black;
            this.f28_comboBox.FormattingEnabled = true;
            this.f28_comboBox.Items.AddRange(new object[] {
            "",
            "",
            ""});
            this.f28_comboBox.Location = new System.Drawing.Point(166, 81);
            this.f28_comboBox.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.f28_comboBox.Name = "f28_comboBox";
            this.f28_comboBox.Size = new System.Drawing.Size(68, 33);
            this.f28_comboBox.TabIndex = 480;
            this.f28_comboBox.SelectedIndexChanged += new System.EventHandler(this.State_comboBox_SelectedIndexChanged);
            // 
            // f28_label
            // 
            this.f28_label.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.f28_label.AutoEllipsis = true;
            this.f28_label.AutoSize = true;
            this.f28_label.BackColor = System.Drawing.Color.Transparent;
            this.f28_label.Font = new System.Drawing.Font("Rubik", 10F);
            this.f28_label.Location = new System.Drawing.Point(166, 0);
            this.f28_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.f28_label.Name = "f28_label";
            this.Filters_tableLayoutPanel.SetRowSpan(this.f28_label, 2);
            this.f28_label.Size = new System.Drawing.Size(68, 78);
            this.f28_label.TabIndex = 478;
            this.f28_label.Text = "label1";
            this.f28_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // f27_comboBox
            // 
            this.f27_comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.f27_comboBox.BackColor = System.Drawing.Color.White;
            this.f27_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.f27_comboBox.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.f27_comboBox.Font = new System.Drawing.Font("Segoe UI Symbol", 11F);
            this.f27_comboBox.ForeColor = System.Drawing.Color.Black;
            this.f27_comboBox.FormattingEnabled = true;
            this.f27_comboBox.Items.AddRange(new object[] {
            "",
            "",
            ""});
            this.f27_comboBox.Location = new System.Drawing.Point(242, 81);
            this.f27_comboBox.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.f27_comboBox.Name = "f27_comboBox";
            this.f27_comboBox.Size = new System.Drawing.Size(68, 33);
            this.f27_comboBox.TabIndex = 475;
            this.f27_comboBox.SelectedIndexChanged += new System.EventHandler(this.State_comboBox_SelectedIndexChanged);
            // 
            // f27_label
            // 
            this.f27_label.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.f27_label.AutoEllipsis = true;
            this.f27_label.AutoSize = true;
            this.f27_label.BackColor = System.Drawing.Color.Transparent;
            this.f27_label.Font = new System.Drawing.Font("Rubik", 10F);
            this.f27_label.Location = new System.Drawing.Point(242, 0);
            this.f27_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.f27_label.Name = "f27_label";
            this.Filters_tableLayoutPanel.SetRowSpan(this.f27_label, 2);
            this.f27_label.Size = new System.Drawing.Size(68, 78);
            this.f27_label.TabIndex = 477;
            this.f27_label.Text = "label1";
            this.f27_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // f26_comboBox
            // 
            this.f26_comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.f26_comboBox.BackColor = System.Drawing.Color.White;
            this.f26_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.f26_comboBox.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.f26_comboBox.Font = new System.Drawing.Font("Segoe UI Symbol", 11F);
            this.f26_comboBox.ForeColor = System.Drawing.Color.Black;
            this.f26_comboBox.FormattingEnabled = true;
            this.f26_comboBox.Items.AddRange(new object[] {
            "",
            "",
            ""});
            this.f26_comboBox.Location = new System.Drawing.Point(318, 81);
            this.f26_comboBox.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.f26_comboBox.Name = "f26_comboBox";
            this.f26_comboBox.Size = new System.Drawing.Size(68, 33);
            this.f26_comboBox.TabIndex = 474;
            this.f26_comboBox.SelectedIndexChanged += new System.EventHandler(this.State_comboBox_SelectedIndexChanged);
            // 
            // f26_label
            // 
            this.f26_label.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.f26_label.AutoEllipsis = true;
            this.f26_label.AutoSize = true;
            this.f26_label.BackColor = System.Drawing.Color.Transparent;
            this.f26_label.Font = new System.Drawing.Font("Rubik", 10F);
            this.f26_label.Location = new System.Drawing.Point(318, 0);
            this.f26_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.f26_label.Name = "f26_label";
            this.Filters_tableLayoutPanel.SetRowSpan(this.f26_label, 2);
            this.f26_label.Size = new System.Drawing.Size(68, 78);
            this.f26_label.TabIndex = 476;
            this.f26_label.Text = "label1";
            this.f26_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // f25_comboBox
            // 
            this.f25_comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.f25_comboBox.BackColor = System.Drawing.Color.White;
            this.f25_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.f25_comboBox.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.f25_comboBox.Font = new System.Drawing.Font("Segoe UI Symbol", 11F);
            this.f25_comboBox.ForeColor = System.Drawing.Color.Black;
            this.f25_comboBox.FormattingEnabled = true;
            this.f25_comboBox.Items.AddRange(new object[] {
            "",
            "",
            ""});
            this.f25_comboBox.Location = new System.Drawing.Point(394, 81);
            this.f25_comboBox.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.f25_comboBox.Name = "f25_comboBox";
            this.f25_comboBox.Size = new System.Drawing.Size(68, 33);
            this.f25_comboBox.TabIndex = 30;
            this.f25_comboBox.SelectedIndexChanged += new System.EventHandler(this.State_comboBox_SelectedIndexChanged);
            // 
            // f25_label
            // 
            this.f25_label.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.f25_label.AutoEllipsis = true;
            this.f25_label.AutoSize = true;
            this.f25_label.BackColor = System.Drawing.Color.Transparent;
            this.f25_label.Font = new System.Drawing.Font("Rubik", 10F);
            this.f25_label.Location = new System.Drawing.Point(394, 0);
            this.f25_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.f25_label.Name = "f25_label";
            this.Filters_tableLayoutPanel.SetRowSpan(this.f25_label, 2);
            this.f25_label.Size = new System.Drawing.Size(68, 78);
            this.f25_label.TabIndex = 473;
            this.f25_label.Text = "label1";
            this.f25_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // f24_comboBox
            // 
            this.f24_comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.f24_comboBox.BackColor = System.Drawing.Color.White;
            this.f24_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.f24_comboBox.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.f24_comboBox.Font = new System.Drawing.Font("Segoe UI Symbol", 11F);
            this.f24_comboBox.ForeColor = System.Drawing.Color.Black;
            this.f24_comboBox.FormattingEnabled = true;
            this.f24_comboBox.Items.AddRange(new object[] {
            "",
            "",
            ""});
            this.f24_comboBox.Location = new System.Drawing.Point(470, 81);
            this.f24_comboBox.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.f24_comboBox.Name = "f24_comboBox";
            this.f24_comboBox.Size = new System.Drawing.Size(68, 33);
            this.f24_comboBox.TabIndex = 29;
            this.f24_comboBox.SelectedIndexChanged += new System.EventHandler(this.State_comboBox_SelectedIndexChanged);
            // 
            // f24_label
            // 
            this.f24_label.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.f24_label.AutoEllipsis = true;
            this.f24_label.AutoSize = true;
            this.f24_label.BackColor = System.Drawing.Color.Transparent;
            this.f24_label.Font = new System.Drawing.Font("Rubik", 10F);
            this.f24_label.Location = new System.Drawing.Point(470, 0);
            this.f24_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.f24_label.Name = "f24_label";
            this.Filters_tableLayoutPanel.SetRowSpan(this.f24_label, 2);
            this.f24_label.Size = new System.Drawing.Size(68, 78);
            this.f24_label.TabIndex = 472;
            this.f24_label.Text = "label1";
            this.f24_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // f23_comboBox
            // 
            this.f23_comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.f23_comboBox.BackColor = System.Drawing.Color.White;
            this.f23_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.f23_comboBox.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.f23_comboBox.Font = new System.Drawing.Font("Segoe UI Symbol", 11F);
            this.f23_comboBox.ForeColor = System.Drawing.Color.Black;
            this.f23_comboBox.FormattingEnabled = true;
            this.f23_comboBox.Items.AddRange(new object[] {
            "",
            "",
            ""});
            this.f23_comboBox.Location = new System.Drawing.Point(546, 81);
            this.f23_comboBox.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.f23_comboBox.Name = "f23_comboBox";
            this.f23_comboBox.Size = new System.Drawing.Size(68, 33);
            this.f23_comboBox.TabIndex = 28;
            this.f23_comboBox.SelectedIndexChanged += new System.EventHandler(this.State_comboBox_SelectedIndexChanged);
            // 
            // f23_label
            // 
            this.f23_label.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.f23_label.AutoEllipsis = true;
            this.f23_label.AutoSize = true;
            this.f23_label.BackColor = System.Drawing.Color.Transparent;
            this.f23_label.Font = new System.Drawing.Font("Rubik", 10F);
            this.f23_label.Location = new System.Drawing.Point(546, 0);
            this.f23_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.f23_label.Name = "f23_label";
            this.Filters_tableLayoutPanel.SetRowSpan(this.f23_label, 2);
            this.f23_label.Size = new System.Drawing.Size(68, 78);
            this.f23_label.TabIndex = 471;
            this.f23_label.Text = "label1";
            this.f23_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // f22_comboBox
            // 
            this.f22_comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.f22_comboBox.BackColor = System.Drawing.Color.White;
            this.f22_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.f22_comboBox.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.f22_comboBox.Font = new System.Drawing.Font("Segoe UI Symbol", 11F);
            this.f22_comboBox.ForeColor = System.Drawing.Color.Black;
            this.f22_comboBox.FormattingEnabled = true;
            this.f22_comboBox.Items.AddRange(new object[] {
            "",
            "",
            ""});
            this.f22_comboBox.Location = new System.Drawing.Point(622, 81);
            this.f22_comboBox.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.f22_comboBox.Name = "f22_comboBox";
            this.f22_comboBox.Size = new System.Drawing.Size(68, 33);
            this.f22_comboBox.TabIndex = 27;
            this.f22_comboBox.SelectedIndexChanged += new System.EventHandler(this.State_comboBox_SelectedIndexChanged);
            // 
            // f22_label
            // 
            this.f22_label.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.f22_label.AutoEllipsis = true;
            this.f22_label.AutoSize = true;
            this.f22_label.BackColor = System.Drawing.Color.Transparent;
            this.f22_label.Font = new System.Drawing.Font("Rubik", 10F);
            this.f22_label.Location = new System.Drawing.Point(622, 0);
            this.f22_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.f22_label.Name = "f22_label";
            this.Filters_tableLayoutPanel.SetRowSpan(this.f22_label, 2);
            this.f22_label.Size = new System.Drawing.Size(68, 78);
            this.f22_label.TabIndex = 470;
            this.f22_label.Text = "label1";
            this.f22_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // f21_comboBox
            // 
            this.f21_comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.f21_comboBox.BackColor = System.Drawing.Color.White;
            this.f21_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.f21_comboBox.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.f21_comboBox.Font = new System.Drawing.Font("Segoe UI Symbol", 11F);
            this.f21_comboBox.ForeColor = System.Drawing.Color.Black;
            this.f21_comboBox.FormattingEnabled = true;
            this.f21_comboBox.Items.AddRange(new object[] {
            "",
            "",
            ""});
            this.f21_comboBox.Location = new System.Drawing.Point(698, 81);
            this.f21_comboBox.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.f21_comboBox.Name = "f21_comboBox";
            this.f21_comboBox.Size = new System.Drawing.Size(68, 33);
            this.f21_comboBox.TabIndex = 26;
            this.f21_comboBox.SelectedIndexChanged += new System.EventHandler(this.State_comboBox_SelectedIndexChanged);
            // 
            // f21_label
            // 
            this.f21_label.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.f21_label.AutoEllipsis = true;
            this.f21_label.AutoSize = true;
            this.f21_label.BackColor = System.Drawing.Color.Transparent;
            this.f21_label.Font = new System.Drawing.Font("Rubik", 10F);
            this.f21_label.Location = new System.Drawing.Point(698, 0);
            this.f21_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.f21_label.Name = "f21_label";
            this.Filters_tableLayoutPanel.SetRowSpan(this.f21_label, 2);
            this.f21_label.Size = new System.Drawing.Size(68, 78);
            this.f21_label.TabIndex = 469;
            this.f21_label.Text = "label1";
            this.f21_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // f20_comboBox
            // 
            this.f20_comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.f20_comboBox.BackColor = System.Drawing.Color.White;
            this.f20_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.f20_comboBox.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.f20_comboBox.Font = new System.Drawing.Font("Segoe UI Symbol", 11F);
            this.f20_comboBox.ForeColor = System.Drawing.Color.Black;
            this.f20_comboBox.FormattingEnabled = true;
            this.f20_comboBox.Items.AddRange(new object[] {
            "",
            "",
            ""});
            this.f20_comboBox.Location = new System.Drawing.Point(774, 81);
            this.f20_comboBox.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.f20_comboBox.Name = "f20_comboBox";
            this.f20_comboBox.Size = new System.Drawing.Size(68, 33);
            this.f20_comboBox.TabIndex = 25;
            this.f20_comboBox.SelectedIndexChanged += new System.EventHandler(this.State_comboBox_SelectedIndexChanged);
            // 
            // f20_label
            // 
            this.f20_label.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.f20_label.AutoEllipsis = true;
            this.f20_label.AutoSize = true;
            this.f20_label.BackColor = System.Drawing.Color.Transparent;
            this.f20_label.Font = new System.Drawing.Font("Rubik", 10F);
            this.f20_label.Location = new System.Drawing.Point(774, 0);
            this.f20_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.f20_label.Name = "f20_label";
            this.Filters_tableLayoutPanel.SetRowSpan(this.f20_label, 2);
            this.f20_label.Size = new System.Drawing.Size(68, 78);
            this.f20_label.TabIndex = 468;
            this.f20_label.Text = "label1";
            this.f20_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // f19_comboBox
            // 
            this.f19_comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.f19_comboBox.BackColor = System.Drawing.Color.White;
            this.f19_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.f19_comboBox.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.f19_comboBox.Font = new System.Drawing.Font("Segoe UI Symbol", 11F);
            this.f19_comboBox.ForeColor = System.Drawing.Color.Black;
            this.f19_comboBox.FormattingEnabled = true;
            this.f19_comboBox.Items.AddRange(new object[] {
            "",
            "",
            ""});
            this.f19_comboBox.Location = new System.Drawing.Point(850, 81);
            this.f19_comboBox.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.f19_comboBox.Name = "f19_comboBox";
            this.f19_comboBox.Size = new System.Drawing.Size(68, 33);
            this.f19_comboBox.TabIndex = 24;
            this.f19_comboBox.SelectedIndexChanged += new System.EventHandler(this.State_comboBox_SelectedIndexChanged);
            // 
            // f19_label
            // 
            this.f19_label.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.f19_label.AutoEllipsis = true;
            this.f19_label.AutoSize = true;
            this.f19_label.BackColor = System.Drawing.Color.Transparent;
            this.f19_label.Font = new System.Drawing.Font("Rubik", 10F);
            this.f19_label.Location = new System.Drawing.Point(850, 0);
            this.f19_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.f19_label.Name = "f19_label";
            this.Filters_tableLayoutPanel.SetRowSpan(this.f19_label, 2);
            this.f19_label.Size = new System.Drawing.Size(68, 78);
            this.f19_label.TabIndex = 467;
            this.f19_label.Text = "label1";
            this.f19_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // f18_comboBox
            // 
            this.f18_comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.f18_comboBox.BackColor = System.Drawing.Color.White;
            this.f18_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.f18_comboBox.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.f18_comboBox.Font = new System.Drawing.Font("Segoe UI Symbol", 11F);
            this.f18_comboBox.ForeColor = System.Drawing.Color.Black;
            this.f18_comboBox.FormattingEnabled = true;
            this.f18_comboBox.Items.AddRange(new object[] {
            "",
            "",
            ""});
            this.f18_comboBox.Location = new System.Drawing.Point(926, 81);
            this.f18_comboBox.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.f18_comboBox.Name = "f18_comboBox";
            this.f18_comboBox.Size = new System.Drawing.Size(68, 33);
            this.f18_comboBox.TabIndex = 23;
            this.f18_comboBox.SelectedIndexChanged += new System.EventHandler(this.State_comboBox_SelectedIndexChanged);
            // 
            // f18_label
            // 
            this.f18_label.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.f18_label.AutoEllipsis = true;
            this.f18_label.AutoSize = true;
            this.f18_label.BackColor = System.Drawing.Color.Transparent;
            this.f18_label.Font = new System.Drawing.Font("Rubik", 10F);
            this.f18_label.Location = new System.Drawing.Point(926, 0);
            this.f18_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.f18_label.Name = "f18_label";
            this.Filters_tableLayoutPanel.SetRowSpan(this.f18_label, 2);
            this.f18_label.Size = new System.Drawing.Size(68, 78);
            this.f18_label.TabIndex = 466;
            this.f18_label.Text = "label1";
            this.f18_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // f17_comboBox
            // 
            this.f17_comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.f17_comboBox.BackColor = System.Drawing.Color.White;
            this.f17_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.f17_comboBox.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.f17_comboBox.Font = new System.Drawing.Font("Segoe UI Symbol", 11F);
            this.f17_comboBox.ForeColor = System.Drawing.Color.Black;
            this.f17_comboBox.FormattingEnabled = true;
            this.f17_comboBox.Items.AddRange(new object[] {
            "",
            "",
            ""});
            this.f17_comboBox.Location = new System.Drawing.Point(1002, 81);
            this.f17_comboBox.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.f17_comboBox.Name = "f17_comboBox";
            this.f17_comboBox.Size = new System.Drawing.Size(68, 33);
            this.f17_comboBox.TabIndex = 22;
            this.f17_comboBox.SelectedIndexChanged += new System.EventHandler(this.State_comboBox_SelectedIndexChanged);
            // 
            // f17_label
            // 
            this.f17_label.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.f17_label.AutoEllipsis = true;
            this.f17_label.AutoSize = true;
            this.f17_label.BackColor = System.Drawing.Color.Transparent;
            this.f17_label.Font = new System.Drawing.Font("Rubik", 10F);
            this.f17_label.Location = new System.Drawing.Point(1002, 0);
            this.f17_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.f17_label.Name = "f17_label";
            this.Filters_tableLayoutPanel.SetRowSpan(this.f17_label, 2);
            this.f17_label.Size = new System.Drawing.Size(68, 78);
            this.f17_label.TabIndex = 465;
            this.f17_label.Text = "label1 cxvsd sdsd";
            this.f17_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // f29_comboBox
            // 
            this.f29_comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.f29_comboBox.BackColor = System.Drawing.Color.White;
            this.f29_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.f29_comboBox.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.f29_comboBox.Font = new System.Drawing.Font("Segoe UI Symbol", 11F);
            this.f29_comboBox.ForeColor = System.Drawing.Color.Black;
            this.f29_comboBox.FormattingEnabled = true;
            this.f29_comboBox.Items.AddRange(new object[] {
            "",
            "",
            ""});
            this.f29_comboBox.Location = new System.Drawing.Point(90, 81);
            this.f29_comboBox.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.f29_comboBox.Name = "f29_comboBox";
            this.f29_comboBox.Size = new System.Drawing.Size(68, 33);
            this.f29_comboBox.TabIndex = 485;
            // 
            // f30_comboBox
            // 
            this.f30_comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.f30_comboBox.BackColor = System.Drawing.Color.White;
            this.f30_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.f30_comboBox.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.f30_comboBox.Font = new System.Drawing.Font("Segoe UI Symbol", 11F);
            this.f30_comboBox.ForeColor = System.Drawing.Color.Black;
            this.f30_comboBox.FormattingEnabled = true;
            this.f30_comboBox.Items.AddRange(new object[] {
            "",
            "",
            ""});
            this.f30_comboBox.Location = new System.Drawing.Point(14, 81);
            this.f30_comboBox.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.f30_comboBox.Name = "f30_comboBox";
            this.f30_comboBox.Size = new System.Drawing.Size(68, 33);
            this.f30_comboBox.TabIndex = 486;
            // 
            // f16_comboBox
            // 
            this.f16_comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.f16_comboBox.BackColor = System.Drawing.Color.White;
            this.f16_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.f16_comboBox.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.f16_comboBox.Font = new System.Drawing.Font("Segoe UI Symbol", 11F);
            this.f16_comboBox.ForeColor = System.Drawing.Color.Black;
            this.f16_comboBox.FormattingEnabled = true;
            this.f16_comboBox.Items.AddRange(new object[] {
            "",
            "",
            ""});
            this.f16_comboBox.Location = new System.Drawing.Point(1078, 81);
            this.f16_comboBox.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.f16_comboBox.Name = "f16_comboBox";
            this.f16_comboBox.Size = new System.Drawing.Size(68, 33);
            this.f16_comboBox.TabIndex = 21;
            this.f16_comboBox.SelectedIndexChanged += new System.EventHandler(this.State_comboBox_SelectedIndexChanged);
            // 
            // f16_label
            // 
            this.f16_label.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.f16_label.AutoEllipsis = true;
            this.f16_label.AutoSize = true;
            this.f16_label.BackColor = System.Drawing.Color.Transparent;
            this.f16_label.Font = new System.Drawing.Font("Rubik", 10F);
            this.f16_label.Location = new System.Drawing.Point(1078, 0);
            this.f16_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.f16_label.Name = "f16_label";
            this.Filters_tableLayoutPanel.SetRowSpan(this.f16_label, 2);
            this.f16_label.Size = new System.Drawing.Size(68, 78);
            this.f16_label.TabIndex = 464;
            this.f16_label.Text = "hon";
            this.f16_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Grid_panel
            // 
            this.Grid_panel.AutoScroll = true;
            this.Grid_panel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Grid_panel.BackColor = System.Drawing.Color.SeaGreen;
            this.Grid_panel.Controls.Add(this.Filters_tableLayoutPanel);
            this.Grid_panel.Controls.Add(this.Top_tableLayoutPanel);
            this.Grid_panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Grid_panel.Location = new System.Drawing.Point(0, 0);
            this.Grid_panel.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.Grid_panel.Name = "Grid_panel";
            this.Grid_panel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Grid_panel.Size = new System.Drawing.Size(1252, 746);
            this.Grid_panel.TabIndex = 454;
            // 
            // TaskBoard_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 27F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.ClientSize = new System.Drawing.Size(1252, 746);
            this.ContextMenuStrip = this.contextMenuStrip1;
            this.Controls.Add(this.Grid_panel);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Rubik", 10.8F);
            this.ForeColor = System.Drawing.Color.Black;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(7, 5, 7, 5);
            this.Name = "TaskBoard_Form";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Hope Center - Micro Projects";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ProjectsTasks_FormClosing);
            this.Load += new System.EventHandler(this.ProjectsTasks_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.Top_tableLayoutPanel.ResumeLayout(false);
            this.Top_tableLayoutPanel.PerformLayout();
            this.Filters_tableLayoutPanel.ResumeLayout(false);
            this.Filters_tableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MP_dataGridView)).EndInit();
            this.Grid_panel.ResumeLayout(false);
            this.Grid_panel.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion
        private System.Windows.Forms.TableLayoutPanel Top_tableLayoutPanel;
        private System.Windows.Forms.TextBox Search_TxtBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox Counter_textBox;
        private System.Windows.Forms.Button Refresh_button;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.CheckBox ExactSearch_checkBox;
        private System.Windows.Forms.Button ShowHide_button;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem refreshPageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearAllFiltersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem goToToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem searchToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem statisticsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem taskBoardToolStripMenuItem;
        private System.Windows.Forms.TableLayoutPanel Filters_tableLayoutPanel;
        private Button ExportToExcel_button;
        private ToolStripMenuItem showAllColumnsToolStripMenuItem;
        private Panel Grid_panel;
        private Label f07_label;
        private Label f25_label;
        private Label f24_label;
        private Label f23_label;
        private Label f22_label;
        private Label f21_label;
        private Label f20_label;
        private Label f19_label;
        private Label f18_label;
        private Label f17_label;
        private Label f16_label;
        private Label f09_label;
        private Label f10_label;
        private Label f11_label;
        private Label f12_label;
        private Label f13_label;
        private Label f14_label;
        private Label f27_label;
        private Label f26_label;
        private Label f28_label;
        private Label f8_label;
        private ComboBox f18_comboBox;
        private ComboBox f16_comboBox;
        private ComboBox f17_comboBox;
        private ComboBox f14_comboBox;
        private ComboBox f11_comboBox;
        private ComboBox f12_comboBox;
        private ComboBox f13_comboBox;
        private ComboBox f06_Type_comboBox;
        private ComboBox f04_State_comboBox;
        private ComboBox f05_Donor_comboBox;
        private ComboBox f09_comboBox;
        private ComboBox f10_comboBox;
        private ComboBox f19_comboBox;
        private ComboBox f07_comboBox;
        private ComboBox f20_comboBox;
        private ComboBox f21_comboBox;
        private ComboBox f22_comboBox;
        private ComboBox f23_comboBox;
        private ComboBox f24_comboBox;
        private ComboBox f25_comboBox;
        private ComboBox f26_comboBox;
        private ComboBox f27_comboBox;
        private ComboBox f28_comboBox;
        private ComboBox f8_comboBox;
        private Button Cancelled_button;
        private ComboBox TaskType_comboBox;
        private DataGridView MP_dataGridView;
        private Button Closed_button;
        private Button Finished_button;
        private ComboBox f29_comboBox;
        private ComboBox f30_comboBox;
        private Label f30_label;
        private Label f29_label;
    }
}