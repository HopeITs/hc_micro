namespace MyWorkApplication.Visit_Forms
{
    partial class Visit_Results_Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Visit_Results_Form));
            this.Answers_dataGridView = new Zuby.ADGV.AdvancedDataGridView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.refreshPageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearAllFiltersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.ExportToExcel_button = new System.Windows.Forms.Button();
            this.AllCount_label = new System.Windows.Forms.Label();
            this.SelectedCount_label = new System.Windows.Forms.Label();
            this.SelectedSum_label = new System.Windows.Forms.Label();
            this.SearchToolBar = new Zuby.ADGV.AdvancedDataGridViewSearchToolBar();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.Search_panel = new System.Windows.Forms.Panel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.DateTo_dateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.FundedDateFrom_label = new System.Windows.Forms.Label();
            this.FundedDateTo_label = new System.Windows.Forms.Label();
            this.VisitNumber_comboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.DateFrom_dateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.LastV_button = new System.Windows.Forms.Button();
            this.Type_comboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.VisitKind_comboBox = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.MP_Category_label = new System.Windows.Forms.Label();
            this.Category_comboBox = new System.Windows.Forms.ComboBox();
            this.SubCategory_label = new System.Windows.Forms.Label();
            this.SubCategory_comboBox = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.Career_comboBox = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.ShowHide_button = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.SearchBy_comboBox = new System.Windows.Forms.ComboBox();
            this.KoboFilters_panel = new System.Windows.Forms.TableLayoutPanel();
            this.KoboForm_label = new System.Windows.Forms.Label();
            this.KoboForm_comboBox = new System.Windows.Forms.ComboBox();
            this.KoboVersion_label = new System.Windows.Forms.Label();
            this.KoboVersion_comboBox = new System.Windows.Forms.ComboBox();
            this.KoboSummary_label = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.Answers_dataGridView)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SearchToolBar.SuspendLayout();
            this.Search_panel.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.KoboFilters_panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // Answers_dataGridView
            // 
            this.Answers_dataGridView.AllowDrop = true;
            this.Answers_dataGridView.AllowUserToAddRows = false;
            this.Answers_dataGridView.AllowUserToDeleteRows = false;
            this.Answers_dataGridView.AllowUserToOrderColumns = true;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Answers_dataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.Answers_dataGridView.BackgroundColor = System.Drawing.Color.White;
            this.Answers_dataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Answers_dataGridView.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            this.Answers_dataGridView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Rubik", 11F);
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Answers_dataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.Answers_dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Answers_dataGridView.ContextMenuStrip = this.contextMenuStrip1;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Rubik", 11F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Answers_dataGridView.DefaultCellStyle = dataGridViewCellStyle3;
            this.Answers_dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Answers_dataGridView.EnableHeadersVisualStyles = false;
            this.Answers_dataGridView.FilterAndSortEnabled = true;
            this.Answers_dataGridView.GridColor = System.Drawing.Color.Black;
            this.Answers_dataGridView.Location = new System.Drawing.Point(0, 219);
            this.Answers_dataGridView.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.Answers_dataGridView.Name = "Answers_dataGridView";
            this.Answers_dataGridView.ReadOnly = true;
            this.Answers_dataGridView.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Answers_dataGridView.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Rubik", 11F);
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Answers_dataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.Answers_dataGridView.RowHeadersWidth = 51;
            this.Answers_dataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.Answers_dataGridView.RowTemplate.Height = 27;
            this.Answers_dataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.Answers_dataGridView.Size = new System.Drawing.Size(1262, 412);
            this.Answers_dataGridView.TabIndex = 568;
            this.Answers_dataGridView.SortStringChanged += new System.EventHandler<Zuby.ADGV.AdvancedDataGridView.SortEventArgs>(this.dataGridView_SortStringChanged);
            this.Answers_dataGridView.FilterStringChanged += new System.EventHandler<Zuby.ADGV.AdvancedDataGridView.FilterEventArgs>(this.dataGridView_FilterStringChanged);
            this.Answers_dataGridView.SelectionChanged += new System.EventHandler(this.dataGridView_SelectionChanged);
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
            this.refreshPageToolStripMenuItem.Text = "Refresh";
            this.refreshPageToolStripMenuItem.Click += new System.EventHandler(this.RefreshPageToolStripMenuItem_Click);
            // 
            // clearAllFiltersToolStripMenuItem
            // 
            this.clearAllFiltersToolStripMenuItem.Name = "clearAllFiltersToolStripMenuItem";
            this.clearAllFiltersToolStripMenuItem.Size = new System.Drawing.Size(173, 24);
            this.clearAllFiltersToolStripMenuItem.Text = "Clear all filters";
            this.clearAllFiltersToolStripMenuItem.Click += new System.EventHandler(this.ClearAllFiltersToolStripMenuItem_Click);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.tableLayoutPanel2.ColumnCount = 7;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.1911F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.00329F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 13.50906F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30.56013F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.79055F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.79055F));
            this.tableLayoutPanel2.Controls.Add(this.ExportToExcel_button, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.AllCount_label, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.SelectedCount_label, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.SelectedSum_label, 3, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 631);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1262, 42);
            this.tableLayoutPanel2.TabIndex = 569;
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
            this.ExportToExcel_button.Location = new System.Drawing.Point(1211, 4);
            this.ExportToExcel_button.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ExportToExcel_button.MaximumSize = new System.Drawing.Size(33, 37);
            this.ExportToExcel_button.MinimumSize = new System.Drawing.Size(33, 37);
            this.ExportToExcel_button.Name = "ExportToExcel_button";
            this.ExportToExcel_button.Size = new System.Drawing.Size(33, 37);
            this.ExportToExcel_button.TabIndex = 411;
            this.ExportToExcel_button.UseVisualStyleBackColor = false;
            this.ExportToExcel_button.Click += new System.EventHandler(this.ExportToExcel_button_Click);
            // 
            // AllCount_label
            // 
            this.AllCount_label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.AllCount_label.AutoSize = true;
            this.AllCount_label.BackColor = System.Drawing.Color.Transparent;
            this.AllCount_label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.AllCount_label.Location = new System.Drawing.Point(1060, 6);
            this.AllCount_label.Margin = new System.Windows.Forms.Padding(0);
            this.AllCount_label.Name = "AllCount_label";
            this.AllCount_label.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.AllCount_label.Size = new System.Drawing.Size(147, 29);
            this.AllCount_label.TabIndex = 475;
            this.AllCount_label.Text = "00000";
            this.AllCount_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SelectedCount_label
            // 
            this.SelectedCount_label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.SelectedCount_label.AutoSize = true;
            this.SelectedCount_label.BackColor = System.Drawing.Color.Transparent;
            this.SelectedCount_label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.SelectedCount_label.Location = new System.Drawing.Point(891, 6);
            this.SelectedCount_label.Margin = new System.Windows.Forms.Padding(0);
            this.SelectedCount_label.Name = "SelectedCount_label";
            this.SelectedCount_label.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.SelectedCount_label.Size = new System.Drawing.Size(169, 29);
            this.SelectedCount_label.TabIndex = 476;
            this.SelectedCount_label.Text = "00000";
            this.SelectedCount_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SelectedSum_label
            // 
            this.SelectedSum_label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.SelectedSum_label.AutoSize = true;
            this.SelectedSum_label.BackColor = System.Drawing.Color.Transparent;
            this.SelectedSum_label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.SelectedSum_label.Location = new System.Drawing.Point(728, 6);
            this.SelectedSum_label.Margin = new System.Windows.Forms.Padding(0);
            this.SelectedSum_label.Name = "SelectedSum_label";
            this.SelectedSum_label.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.SelectedSum_label.Size = new System.Drawing.Size(163, 29);
            this.SelectedSum_label.TabIndex = 477;
            this.SelectedSum_label.Text = "00000";
            this.SelectedSum_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SearchToolBar
            // 
            this.SearchToolBar.AllowMerge = false;
            this.SearchToolBar.AutoSize = false;
            this.SearchToolBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.SearchToolBar.Font = new System.Drawing.Font("Rubik", 11F);
            this.SearchToolBar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.SearchToolBar.ImageScalingSize = new System.Drawing.Size(18, 18);
            this.SearchToolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator4});
            this.SearchToolBar.Location = new System.Drawing.Point(0, 182);
            this.SearchToolBar.MaximumSize = new System.Drawing.Size(0, 33);
            this.SearchToolBar.MinimumSize = new System.Drawing.Size(0, 37);
            this.SearchToolBar.Name = "SearchToolBar";
            this.SearchToolBar.Padding = new System.Windows.Forms.Padding(0);
            this.SearchToolBar.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.SearchToolBar.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.SearchToolBar.Size = new System.Drawing.Size(1262, 37);
            this.SearchToolBar.TabIndex = 567;
            this.SearchToolBar.Search += new Zuby.ADGV.AdvancedDataGridViewSearchToolBarSearchEventHandler(this.SearchToolBar_Search);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.AutoSize = false;
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.toolStripSeparator4.Size = new System.Drawing.Size(10, 35);
            // 
            // Search_panel
            // 
            this.Search_panel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.Search_panel.ContextMenuStrip = this.contextMenuStrip1;
            this.Search_panel.Controls.Add(this.tableLayoutPanel3);
            this.Search_panel.Dock = System.Windows.Forms.DockStyle.Top;
            this.Search_panel.Location = new System.Drawing.Point(0, 52);
            this.Search_panel.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.Search_panel.Name = "Search_panel";
            this.Search_panel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Search_panel.Size = new System.Drawing.Size(1262, 130);
            this.Search_panel.TabIndex = 418;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 8;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15.82278F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.177216F));
            this.tableLayoutPanel3.Controls.Add(this.DateTo_dateTimePicker, 3, 0);
            this.tableLayoutPanel3.Controls.Add(this.FundedDateFrom_label, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.FundedDateTo_label, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.VisitNumber_comboBox, 5, 0);
            this.tableLayoutPanel3.Controls.Add(this.label1, 4, 0);
            this.tableLayoutPanel3.Controls.Add(this.DateFrom_dateTimePicker, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.LastV_button, 6, 0);
            this.tableLayoutPanel3.Controls.Add(this.Type_comboBox, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.VisitKind_comboBox, 3, 1);
            this.tableLayoutPanel3.Controls.Add(this.label5, 2, 1);
            this.tableLayoutPanel3.Controls.Add(this.MP_Category_label, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.Category_comboBox, 1, 2);
            this.tableLayoutPanel3.Controls.Add(this.SubCategory_label, 2, 2);
            this.tableLayoutPanel3.Controls.Add(this.SubCategory_comboBox, 3, 2);
            this.tableLayoutPanel3.Controls.Add(this.label4, 4, 2);
            this.tableLayoutPanel3.Controls.Add(this.Career_comboBox, 5, 2);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tableLayoutPanel3.RowCount = 3;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(1262, 130);
            this.tableLayoutPanel3.TabIndex = 562;
            // 
            // DateTo_dateTimePicker
            // 
            this.DateTo_dateTimePicker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.DateTo_dateTimePicker.CustomFormat = "yyyy-MM-dd";
            this.DateTo_dateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DateTo_dateTimePicker.Location = new System.Drawing.Point(637, 7);
            this.DateTo_dateTimePicker.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.DateTo_dateTimePicker.MinDate = new System.DateTime(2018, 1, 1, 0, 0, 0, 0);
            this.DateTo_dateTimePicker.Name = "DateTo_dateTimePicker";
            this.DateTo_dateTimePicker.Size = new System.Drawing.Size(151, 29);
            this.DateTo_dateTimePicker.TabIndex = 572;
            this.DateTo_dateTimePicker.ValueChanged += new System.EventHandler(this.DateFrom_bcDateTimePicker_ValueChanged);
            // 
            // FundedDateFrom_label
            // 
            this.FundedDateFrom_label.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.FundedDateFrom_label.AutoSize = true;
            this.FundedDateFrom_label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.FundedDateFrom_label.Location = new System.Drawing.Point(1106, 7);
            this.FundedDateFrom_label.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.FundedDateFrom_label.Name = "FundedDateFrom_label";
            this.FundedDateFrom_label.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.FundedDateFrom_label.Size = new System.Drawing.Size(146, 29);
            this.FundedDateFrom_label.TabIndex = 559;
            this.FundedDateFrom_label.Text = "تاريخ الزيارات من:";
            // 
            // FundedDateTo_label
            // 
            this.FundedDateTo_label.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.FundedDateTo_label.AutoSize = true;
            this.FundedDateTo_label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.FundedDateTo_label.Location = new System.Drawing.Point(792, 7);
            this.FundedDateTo_label.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.FundedDateTo_label.Name = "FundedDateTo_label";
            this.FundedDateTo_label.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.FundedDateTo_label.Size = new System.Drawing.Size(41, 29);
            this.FundedDateTo_label.TabIndex = 558;
            this.FundedDateTo_label.Text = "إلى:";
            // 
            // VisitNumber_comboBox
            // 
            this.VisitNumber_comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.VisitNumber_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.VisitNumber_comboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VisitNumber_comboBox.FormattingEnabled = true;
            this.VisitNumber_comboBox.Items.AddRange(new object[] {
            "Monitorings",
            "M1",
            "M2",
            "M3",
            "M4",
            "M5",
            "Openings",
            "Closings"});
            this.VisitNumber_comboBox.Location = new System.Drawing.Point(323, 4);
            this.VisitNumber_comboBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.VisitNumber_comboBox.Name = "VisitNumber_comboBox";
            this.VisitNumber_comboBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.VisitNumber_comboBox.Size = new System.Drawing.Size(151, 35);
            this.VisitNumber_comboBox.TabIndex = 502;
            this.VisitNumber_comboBox.SelectedValueChanged += new System.EventHandler(this.VisitKind_comboBox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.label1.Location = new System.Drawing.Point(477, 7);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label1.Size = new System.Drawing.Size(95, 29);
            this.label1.TabIndex = 501;
            this.label1.Text = "رقم الزيارة:";
            // 
            // DateFrom_dateTimePicker
            // 
            this.DateFrom_dateTimePicker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.DateFrom_dateTimePicker.CustomFormat = "yyyy-MM-dd";
            this.DateFrom_dateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DateFrom_dateTimePicker.Location = new System.Drawing.Point(951, 7);
            this.DateFrom_dateTimePicker.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.DateFrom_dateTimePicker.MinDate = new System.DateTime(2018, 1, 1, 0, 0, 0, 0);
            this.DateFrom_dateTimePicker.Name = "DateFrom_dateTimePicker";
            this.DateFrom_dateTimePicker.Size = new System.Drawing.Size(151, 29);
            this.DateFrom_dateTimePicker.TabIndex = 571;
            this.DateFrom_dateTimePicker.Value = new System.DateTime(2018, 1, 1, 0, 0, 0, 0);
            this.DateFrom_dateTimePicker.ValueChanged += new System.EventHandler(this.DateFrom_bcDateTimePicker_ValueChanged);
            // 
            // LastV_button
            // 
            this.LastV_button.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LastV_button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Silver;
            this.LastV_button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.LastV_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LastV_button.Location = new System.Drawing.Point(124, 4);
            this.LastV_button.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.LastV_button.Name = "LastV_button";
            this.LastV_button.Size = new System.Drawing.Size(193, 35);
            this.LastV_button.TabIndex = 504;
            this.LastV_button.Text = "آخر زيارة لكل مستفيد";
            this.LastV_button.UseVisualStyleBackColor = true;
            this.LastV_button.Visible = false;
            this.LastV_button.Click += new System.EventHandler(this.LastV_button_Click);
            // 
            // Type_comboBox
            // 
            this.Type_comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.Type_comboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.Type_comboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.Type_comboBox.BackColor = System.Drawing.Color.White;
            this.Type_comboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Type_comboBox.ForeColor = System.Drawing.Color.Black;
            this.Type_comboBox.FormattingEnabled = true;
            this.Type_comboBox.Location = new System.Drawing.Point(951, 47);
            this.Type_comboBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Type_comboBox.Name = "Type_comboBox";
            this.Type_comboBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Type_comboBox.Size = new System.Drawing.Size(151, 35);
            this.Type_comboBox.TabIndex = 575;
            this.Type_comboBox.SelectedIndexChanged += new System.EventHandler(this.Type_comboBox_SelectedIndexChanged);
            this.Type_comboBox.SelectedValueChanged += new System.EventHandler(this.VisitKind_comboBox_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.label2.Location = new System.Drawing.Point(1105, 50);
            this.label2.Margin = new System.Windows.Forms.Padding(0);
            this.label2.Name = "label2";
            this.label2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label2.Size = new System.Drawing.Size(113, 29);
            this.label2.TabIndex = 573;
            this.label2.Text = "نوع المشروع:";
            // 
            // VisitKind_comboBox
            // 
            this.VisitKind_comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.VisitKind_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.VisitKind_comboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VisitKind_comboBox.FormattingEnabled = true;
            this.VisitKind_comboBox.Items.AddRange(new object[] {
            "مشاريع أخرى",
            "مركبات"});
            this.VisitKind_comboBox.Location = new System.Drawing.Point(637, 47);
            this.VisitKind_comboBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.VisitKind_comboBox.Name = "VisitKind_comboBox";
            this.VisitKind_comboBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.VisitKind_comboBox.Size = new System.Drawing.Size(151, 35);
            this.VisitKind_comboBox.TabIndex = 580;
            this.VisitKind_comboBox.SelectedValueChanged += new System.EventHandler(this.VisitKind_comboBox_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.label5.Location = new System.Drawing.Point(791, 50);
            this.label5.Margin = new System.Windows.Forms.Padding(0);
            this.label5.Name = "label5";
            this.label5.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label5.Size = new System.Drawing.Size(93, 29);
            this.label5.TabIndex = 579;
            this.label5.Text = "نوع الزيارة:";
            // 
            // MP_Category_label
            // 
            this.MP_Category_label.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.MP_Category_label.AutoSize = true;
            this.MP_Category_label.BackColor = System.Drawing.Color.Transparent;
            this.MP_Category_label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.MP_Category_label.Location = new System.Drawing.Point(1105, 93);
            this.MP_Category_label.Margin = new System.Windows.Forms.Padding(0);
            this.MP_Category_label.Name = "MP_Category_label";
            this.MP_Category_label.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.MP_Category_label.Size = new System.Drawing.Size(98, 29);
            this.MP_Category_label.TabIndex = 568;
            this.MP_Category_label.Text = "Category:";
            // 
            // Category_comboBox
            // 
            this.Category_comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.Category_comboBox.BackColor = System.Drawing.Color.White;
            this.Category_comboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Category_comboBox.ForeColor = System.Drawing.Color.Black;
            this.Category_comboBox.FormattingEnabled = true;
            this.Category_comboBox.Location = new System.Drawing.Point(951, 91);
            this.Category_comboBox.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.Category_comboBox.Name = "Category_comboBox";
            this.Category_comboBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Category_comboBox.Size = new System.Drawing.Size(151, 35);
            this.Category_comboBox.TabIndex = 567;
            this.Category_comboBox.SelectedIndexChanged += new System.EventHandler(this.Category_comboBox_SelectedIndexChanged);
            this.Category_comboBox.SelectedValueChanged += new System.EventHandler(this.VisitKind_comboBox_SelectedIndexChanged);
            this.Category_comboBox.TextChanged += new System.EventHandler(this.Category_comboBox_TextChanged);
            // 
            // SubCategory_label
            // 
            this.SubCategory_label.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.SubCategory_label.AutoSize = true;
            this.SubCategory_label.BackColor = System.Drawing.Color.Transparent;
            this.SubCategory_label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.SubCategory_label.Location = new System.Drawing.Point(791, 93);
            this.SubCategory_label.Margin = new System.Windows.Forms.Padding(0);
            this.SubCategory_label.Name = "SubCategory_label";
            this.SubCategory_label.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.SubCategory_label.Size = new System.Drawing.Size(132, 29);
            this.SubCategory_label.TabIndex = 569;
            this.SubCategory_label.Text = "SubCategory:";
            // 
            // SubCategory_comboBox
            // 
            this.SubCategory_comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.SubCategory_comboBox.BackColor = System.Drawing.Color.White;
            this.SubCategory_comboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SubCategory_comboBox.ForeColor = System.Drawing.Color.Black;
            this.SubCategory_comboBox.FormattingEnabled = true;
            this.SubCategory_comboBox.Location = new System.Drawing.Point(637, 91);
            this.SubCategory_comboBox.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.SubCategory_comboBox.Name = "SubCategory_comboBox";
            this.SubCategory_comboBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.SubCategory_comboBox.Size = new System.Drawing.Size(151, 35);
            this.SubCategory_comboBox.TabIndex = 570;
            this.SubCategory_comboBox.SelectedValueChanged += new System.EventHandler(this.VisitKind_comboBox_SelectedIndexChanged);
            this.SubCategory_comboBox.TextChanged += new System.EventHandler(this.SubCategory_comboBox_TextChanged);
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.label4.Location = new System.Drawing.Point(477, 93);
            this.label4.Margin = new System.Windows.Forms.Padding(0);
            this.label4.Name = "label4";
            this.label4.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label4.Size = new System.Drawing.Size(77, 29);
            this.label4.TabIndex = 577;
            this.label4.Text = "Career:";
            // 
            // Career_comboBox
            // 
            this.Career_comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.Career_comboBox.BackColor = System.Drawing.Color.White;
            this.Career_comboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Career_comboBox.ForeColor = System.Drawing.Color.Black;
            this.Career_comboBox.FormattingEnabled = true;
            this.Career_comboBox.Location = new System.Drawing.Point(323, 91);
            this.Career_comboBox.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.Career_comboBox.Name = "Career_comboBox";
            this.Career_comboBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Career_comboBox.Size = new System.Drawing.Size(151, 35);
            this.Career_comboBox.TabIndex = 578;
            this.Career_comboBox.SelectedValueChanged += new System.EventHandler(this.VisitKind_comboBox_SelectedIndexChanged);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.ColumnCount = 8;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15.66456F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.335443F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel1.ContextMenuStrip = this.contextMenuStrip1;
            this.tableLayoutPanel1.Controls.Add(this.ShowHide_button, 6, 0);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.SearchBy_comboBox, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1262, 52);
            this.tableLayoutPanel1.TabIndex = 497;
            // 
            // ShowHide_button
            // 
            this.ShowHide_button.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.ShowHide_button.BackColor = System.Drawing.Color.Transparent;
            this.ShowHide_button.BackgroundImage = global::MyWorkApplication.Properties.Resources.Show2_D;
            this.ShowHide_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ShowHide_button.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.ShowHide_button.FlatAppearance.BorderSize = 0;
            this.ShowHide_button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.ShowHide_button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.ShowHide_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ShowHide_button.Location = new System.Drawing.Point(126, 4);
            this.ShowHide_button.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ShowHide_button.Name = "ShowHide_button";
            this.ShowHide_button.Size = new System.Drawing.Size(191, 43);
            this.ShowHide_button.TabIndex = 577;
            this.ShowHide_button.UseVisualStyleBackColor = false;
            this.ShowHide_button.Click += new System.EventHandler(this.ShowHide_button_Click);
            this.ShowHide_button.MouseEnter += new System.EventHandler(this.ShowHide_button_MouseEnter);
            this.ShowHide_button.MouseLeave += new System.EventHandler(this.ShowHide_button_MouseLeave);
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.label3.Location = new System.Drawing.Point(1106, 11);
            this.label3.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.label3.Name = "label3";
            this.label3.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label3.Size = new System.Drawing.Size(118, 29);
            this.label3.TabIndex = 576;
            this.label3.Text = "نتيجة العرض:";
            // 
            // SearchBy_comboBox
            // 
            this.SearchBy_comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.SearchBy_comboBox.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel1.SetColumnSpan(this.SearchBy_comboBox, 3);
            this.SearchBy_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SearchBy_comboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SearchBy_comboBox.ForeColor = System.Drawing.Color.Black;
            this.SearchBy_comboBox.FormattingEnabled = true;
            this.SearchBy_comboBox.Location = new System.Drawing.Point(637, 8);
            this.SearchBy_comboBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.SearchBy_comboBox.Name = "SearchBy_comboBox";
            this.SearchBy_comboBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.SearchBy_comboBox.Size = new System.Drawing.Size(465, 35);
            this.SearchBy_comboBox.TabIndex = 563;
            this.SearchBy_comboBox.SelectedIndexChanged += new System.EventHandler(this.SearchBy_comboBox_SelectedIndexChanged);
            // 
            // KoboFilters_panel
            //
            this.KoboFilters_panel.ColumnCount = 4;
            this.KoboFilters_panel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.KoboFilters_panel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 65F));
            this.KoboFilters_panel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.KoboFilters_panel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.KoboFilters_panel.Controls.Add(this.KoboForm_label, 0, 0);
            this.KoboFilters_panel.Controls.Add(this.KoboForm_comboBox, 1, 0);
            this.KoboFilters_panel.Controls.Add(this.KoboVersion_label, 2, 0);
            this.KoboFilters_panel.Controls.Add(this.KoboVersion_comboBox, 3, 0);
            this.KoboFilters_panel.Controls.Add(this.KoboSummary_label, 0, 1);
            this.KoboFilters_panel.SetColumnSpan(this.KoboSummary_label, 4);
            this.KoboFilters_panel.Dock = System.Windows.Forms.DockStyle.Top;
            this.KoboFilters_panel.Name = "KoboFilters_panel";
            this.KoboFilters_panel.Padding = new System.Windows.Forms.Padding(8, 2, 8, 2);
            this.KoboFilters_panel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.KoboFilters_panel.RowCount = 2;
            this.KoboFilters_panel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.KoboFilters_panel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.KoboFilters_panel.Size = new System.Drawing.Size(1262, 80);
            this.KoboFilters_panel.Visible = false;
            //
            // KoboForm_label
            //
            this.KoboForm_label.AutoSize = true;
            this.KoboForm_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.KoboForm_label.Name = "KoboForm_label";
            this.KoboForm_label.Margin = new System.Windows.Forms.Padding(8, 3, 8, 3);
            this.KoboForm_label.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.KoboForm_label.Text = "النموذج (اختياري):";
            this.KoboForm_label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // KoboForm_comboBox
            //
            this.KoboForm_comboBox.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.KoboForm_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.KoboForm_comboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.KoboForm_comboBox.Name = "KoboForm_comboBox";
            this.KoboForm_comboBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.KoboForm_comboBox.SelectionChangeCommitted += new System.EventHandler(this.KoboForm_comboBox_SelectionChangeCommitted);
            //
            // KoboVersion_label
            //
            this.KoboVersion_label.AutoSize = true;
            this.KoboVersion_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.KoboVersion_label.Name = "KoboVersion_label";
            this.KoboVersion_label.Margin = new System.Windows.Forms.Padding(8, 3, 8, 3);
            this.KoboVersion_label.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.KoboVersion_label.Text = "الإصدار:";
            this.KoboVersion_label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // KoboVersion_comboBox
            //
            this.KoboVersion_comboBox.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.KoboVersion_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.KoboVersion_comboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.KoboVersion_comboBox.Name = "KoboVersion_comboBox";
            this.KoboVersion_comboBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.KoboVersion_comboBox.SelectionChangeCommitted += new System.EventHandler(this.KoboVersion_comboBox_SelectionChangeCommitted);
            //
            // KoboSummary_label
            //
            this.KoboSummary_label.AutoEllipsis = true;
            this.KoboSummary_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.KoboSummary_label.Name = "KoboSummary_label";
            this.KoboSummary_label.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.KoboSummary_label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // Visit_Results_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 27F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.ClientSize = new System.Drawing.Size(1262, 673);
            this.ContextMenuStrip = this.contextMenuStrip1;
            this.Controls.Add(this.Answers_dataGridView);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.SearchToolBar);
            this.Controls.Add(this.Search_panel);
            this.Controls.Add(this.KoboFilters_panel);
            this.Controls.Add(this.tableLayoutPanel1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Rubik", 11F);
            this.ForeColor = System.Drawing.Color.Black;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Visit_Results_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Visit_Results_Form";
            this.Load += new System.EventHandler(this.Monitoring_Visit_Results_Form_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Answers_dataGridView)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.SearchToolBar.ResumeLayout(false);
            this.SearchToolBar.PerformLayout();
            this.Search_panel.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.KoboFilters_panel.ResumeLayout(false);
            this.KoboFilters_panel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ComboBox VisitNumber_comboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button LastV_button;
        private System.Windows.Forms.Label FundedDateFrom_label;
        private System.Windows.Forms.Label FundedDateTo_label;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.ComboBox SubCategory_comboBox;
        private System.Windows.Forms.Label SubCategory_label;
        private System.Windows.Forms.ComboBox Category_comboBox;
        private System.Windows.Forms.Label MP_Category_label;
        private System.Windows.Forms.DateTimePicker DateTo_dateTimePicker;
        private System.Windows.Forms.DateTimePicker DateFrom_dateTimePicker;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox Type_comboBox;
        private System.Windows.Forms.ComboBox SearchBy_comboBox;
        private System.Windows.Forms.Label label3;
        private Zuby.ADGV.AdvancedDataGridViewSearchToolBar SearchToolBar;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private Zuby.ADGV.AdvancedDataGridView Answers_dataGridView;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label AllCount_label;
        private System.Windows.Forms.Label SelectedCount_label;
        private System.Windows.Forms.Button ExportToExcel_button;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem refreshPageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearAllFiltersToolStripMenuItem;
        private System.Windows.Forms.ComboBox Career_comboBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label SelectedSum_label;
        private System.Windows.Forms.ComboBox VisitKind_comboBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button ShowHide_button;
        private System.Windows.Forms.Panel Search_panel;
        private System.Windows.Forms.TableLayoutPanel KoboFilters_panel;
        private System.Windows.Forms.Label KoboForm_label;
        private System.Windows.Forms.ComboBox KoboForm_comboBox;
        private System.Windows.Forms.Label KoboVersion_label;
        private System.Windows.Forms.ComboBox KoboVersion_comboBox;
        private System.Windows.Forms.Label KoboSummary_label;
    }
}
