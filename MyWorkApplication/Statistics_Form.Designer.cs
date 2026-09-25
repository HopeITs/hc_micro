using System;

namespace MyWorkApplication
{
    partial class Statistics_Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.Windows.Forms.TableLayoutPanel reportingPanel;
        private MyWorkApplication.Controls.ReportingCityComboBox reportingScope;
        private System.Windows.Forms.Label reportingCitiesLabel;
        private System.Windows.Forms.Label reportingCoverage;
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && reportCancellation != null) reportCancellation.Cancel();
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
            PresentationControls.CheckBoxProperties checkBoxProperties1 = new PresentationControls.CheckBoxProperties();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            PresentationControls.CheckBoxProperties checkBoxProperties2 = new PresentationControls.CheckBoxProperties();
            PresentationControls.CheckBoxProperties checkBoxProperties3 = new PresentationControls.CheckBoxProperties();
            PresentationControls.CheckBoxProperties checkBoxProperties4 = new PresentationControls.CheckBoxProperties();
            PresentationControls.CheckBoxProperties checkBoxProperties5 = new PresentationControls.CheckBoxProperties();
            PresentationControls.CheckBoxProperties checkBoxProperties6 = new PresentationControls.CheckBoxProperties();
            PresentationControls.CheckBoxProperties checkBoxProperties7 = new PresentationControls.CheckBoxProperties();
            PresentationControls.CheckBoxProperties checkBoxProperties8 = new PresentationControls.CheckBoxProperties();
            PresentationControls.CheckBoxProperties checkBoxProperties9 = new PresentationControls.CheckBoxProperties();
            PresentationControls.CheckBoxProperties checkBoxProperties10 = new PresentationControls.CheckBoxProperties();
            PresentationControls.CheckBoxProperties checkBoxProperties11 = new PresentationControls.CheckBoxProperties();
            PresentationControls.CheckBoxProperties checkBoxProperties12 = new PresentationControls.CheckBoxProperties();
            PresentationControls.CheckBoxProperties checkBoxProperties13 = new PresentationControls.CheckBoxProperties();
            PresentationControls.CheckBoxProperties checkBoxProperties14 = new PresentationControls.CheckBoxProperties();
            PresentationControls.CheckBoxProperties checkBoxProperties15 = new PresentationControls.CheckBoxProperties();
            PresentationControls.CheckBoxProperties checkBoxProperties16 = new PresentationControls.CheckBoxProperties();
            PresentationControls.CheckBoxProperties checkBoxProperties17 = new PresentationControls.CheckBoxProperties();
            PresentationControls.CheckBoxProperties checkBoxProperties18 = new PresentationControls.CheckBoxProperties();
            PresentationControls.CheckBoxProperties checkBoxProperties19 = new PresentationControls.CheckBoxProperties();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Statistics_Form));
            this.reportingPanel = new System.Windows.Forms.TableLayoutPanel();
            this.reportingCoverage = new System.Windows.Forms.Label();
            this.reportingScope = new MyWorkApplication.Controls.ReportingCityComboBox();
            this.reportingCitiesLabel = new System.Windows.Forms.Label();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.refreshPageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showHideFiltersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearAllFiltersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showDetailsInSearchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.AdvancedSearch_button = new System.Windows.Forms.Button();
            this.label16 = new System.Windows.Forms.Label();
            this.X_Axis_comboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.results_dataGridView = new System.Windows.Forms.DataGridView();
            this.withFunded_checkBox = new System.Windows.Forms.CheckBox();
            this.ApplyFilter_button = new System.Windows.Forms.Button();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.ExportToExcel_button = new System.Windows.Forms.Button();
            this.Count_all_label = new System.Windows.Forms.Label();
            this.Count_funded_label = new System.Windows.Forms.Label();
            this.Search_panel = new System.Windows.Forms.Panel();
            this.filters_tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.Type_label = new System.Windows.Forms.Label();
            this.DonorGroup_label = new System.Windows.Forms.Label();
            this.MaritalStatus_label = new System.Windows.Forms.Label();
            this.MP_Status_label = new System.Windows.Forms.Label();
            this.Donor_label = new System.Windows.Forms.Label();
            this.P_Parish_label = new System.Windows.Forms.Label();
            this.Category_label = new System.Windows.Forms.Label();
            this.Age_label = new System.Windows.Forms.Label();
            this.SubCategory_label = new System.Windows.Forms.Label();
            this.Gender_label = new System.Windows.Forms.Label();
            this.Career_label = new System.Windows.Forms.Label();
            this.Partnership_label = new System.Windows.Forms.Label();
            this.FundType_label = new System.Windows.Forms.Label();
            this.SubType_label = new System.Windows.Forms.Label();
            this.FundType_comboBox = new PresentationControls.CheckBoxComboBox();
            this.Type_comboBox = new PresentationControls.CheckBoxComboBox();
            this.SubType_comboBox = new PresentationControls.CheckBoxComboBox();
            this.Partnership_comboBox = new PresentationControls.CheckBoxComboBox();
            this.MP_Status_comboBox = new PresentationControls.CheckBoxComboBox();
            this.Donor_comboBox = new PresentationControls.CheckBoxComboBox();
            this.DonorGroup_comboBox = new PresentationControls.CheckBoxComboBox();
            this.P_Parish_comboBox = new PresentationControls.CheckBoxComboBox();
            this.Category_comboBox = new PresentationControls.CheckBoxComboBox();
            this.SubCategory_comboBox = new PresentationControls.CheckBoxComboBox();
            this.Career_comboBox = new PresentationControls.CheckBoxComboBox();
            this.MaritalStatus_comboBox = new PresentationControls.CheckBoxComboBox();
            this.Age_comboBox = new PresentationControls.CheckBoxComboBox();
            this.Gender_comboBox = new PresentationControls.CheckBoxComboBox();
            this.HomeStreet_label = new System.Windows.Forms.Label();
            this.HomeStreet_comboBox = new PresentationControls.CheckBoxComboBox();
            this.ProjectStreet_label = new System.Windows.Forms.Label();
            this.ProjectStreet_comboBox = new PresentationControls.CheckBoxComboBox();
            this.ApplyDateFrom_label = new System.Windows.Forms.Label();
            this.FundedDateFrom_label = new System.Windows.Forms.Label();
            this.FundDate_comboBox = new PresentationControls.CheckBoxComboBox();
            this.ApplyDate_comboBox = new PresentationControls.CheckBoxComboBox();
            this.ApplyDate_checkBox = new System.Windows.Forms.CheckBox();
            this.FundedDate_checkBox = new System.Windows.Forms.CheckBox();
            this.ApplyDateFrom_DateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.ApplyDateTo_DateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.FundedDateFrom_DateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.FundedDateTo_DateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.RequestedFrom_label = new System.Windows.Forms.Label();
            this.FundedFrom_label = new System.Windows.Forms.Label();
            this.RequestedFrom_textBox = new System.Windows.Forms.TextBox();
            this.RequestedTo_textBox = new System.Windows.Forms.TextBox();
            this.FundedFrom_textBox = new System.Windows.Forms.TextBox();
            this.FundedTo_textBox = new System.Windows.Forms.TextBox();
            this.from_label1 = new System.Windows.Forms.Label();
            this.to_label1 = new System.Windows.Forms.Label();
            this.year_label1 = new System.Windows.Forms.Label();
            this.from_label2 = new System.Windows.Forms.Label();
            this.to_label2 = new System.Windows.Forms.Label();
            this.year_label2 = new System.Windows.Forms.Label();
            this.reportingPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.results_dataGridView)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.Search_panel.SuspendLayout();
            this.filters_tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // reportingPanel
            // 
            this.reportingPanel.ColumnCount = 1;
            this.reportingPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.reportingPanel.Controls.Add(this.reportingCoverage, 0, 0);
            this.reportingPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.reportingPanel.Location = new System.Drawing.Point(0, 45);
            this.reportingPanel.Name = "reportingPanel";
            this.reportingPanel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.reportingPanel.RowCount = 1;
            this.reportingPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.reportingPanel.Size = new System.Drawing.Size(1262, 25);
            this.reportingPanel.TabIndex = 417;
            // 
            // reportingCoverage
            // 
            this.reportingCoverage.AutoEllipsis = true;
            this.reportingCoverage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.reportingCoverage.Font = new System.Drawing.Font("Rubik", 8F);
            this.reportingCoverage.Location = new System.Drawing.Point(12, 0);
            this.reportingCoverage.Margin = new System.Windows.Forms.Padding(12, 0, 12, 0);
            this.reportingCoverage.Name = "reportingCoverage";
            this.reportingCoverage.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.reportingCoverage.Size = new System.Drawing.Size(1238, 25);
            this.reportingCoverage.TabIndex = 0;
            this.reportingCoverage.Text = "Current city";
            this.reportingCoverage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // reportingScope
            // 
            this.reportingScope.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            checkBoxProperties1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.reportingScope.CheckBoxProperties = checkBoxProperties1;
            this.reportingScope.DisplayMemberSingleItem = "";
            this.reportingScope.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.reportingScope.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.reportingScope.FormattingEnabled = true;
            this.reportingScope.Location = new System.Drawing.Point(375, 10);
            this.reportingScope.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.reportingScope.Name = "reportingScope";
            this.reportingScope.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.reportingScope.Size = new System.Drawing.Size(155, 34);
            this.reportingScope.TabIndex = 654;
            this.reportingScope.CitiesChanged += new System.EventHandler(this.ReportingScopeChanged);
            this.reportingScope.CitiesCommitted += new System.EventHandler(this.ReportingScopeCommitted);
            // 
            // reportingCitiesLabel
            // 
            this.reportingCitiesLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.reportingCitiesLabel.AutoSize = true;
            this.reportingCitiesLabel.Location = new System.Drawing.Point(303, 9);
            this.reportingCitiesLabel.Margin = new System.Windows.Forms.Padding(3, 0, 8, 0);
            this.reportingCitiesLabel.Name = "reportingCitiesLabel";
            this.reportingCitiesLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.reportingCitiesLabel.Size = new System.Drawing.Size(61, 26);
            this.reportingCitiesLabel.TabIndex = 653;
            this.reportingCitiesLabel.Text = "Cities:";
            this.reportingCitiesLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chart1
            // 
            this.chart1.AntiAliasing = System.Windows.Forms.DataVisualization.Charting.AntiAliasingStyles.Graphics;
            this.chart1.BorderlineColor = System.Drawing.Color.Black;
            chartArea1.AxisX.MajorGrid.LineColor = System.Drawing.Color.Yellow;
            chartArea1.AxisY.MajorGrid.LineColor = System.Drawing.Color.SaddleBrown;
            chartArea1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            this.chart1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chart1.IsSoftShadows = false;
            legend1.Alignment = System.Drawing.StringAlignment.Center;
            legend1.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Top;
            legend1.Font = new System.Drawing.Font("Rubik", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            legend1.IsDockedInsideChartArea = false;
            legend1.IsEquallySpacedItems = true;
            legend1.IsTextAutoFit = false;
            legend1.Name = "Legend0";
            legend1.TableStyle = System.Windows.Forms.DataVisualization.Charting.LegendTableStyle.Wide;
            legend1.TitleFont = new System.Drawing.Font("Rubik", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            legend2.Alignment = System.Drawing.StringAlignment.Center;
            legend2.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
            legend2.Font = new System.Drawing.Font("Rubik", 10F);
            legend2.IsEquallySpacedItems = true;
            legend2.IsTextAutoFit = false;
            legend2.Name = "Legend2";
            legend2.TableStyle = System.Windows.Forms.DataVisualization.Charting.LegendTableStyle.Wide;
            legend2.TitleFont = new System.Drawing.Font("Rubik", 9F);
            this.chart1.Legends.Add(legend1);
            this.chart1.Legends.Add(legend2);
            this.chart1.Location = new System.Drawing.Point(0, 311);
            this.chart1.Margin = new System.Windows.Forms.Padding(4, 7, 4, 7);
            this.chart1.Name = "chart1";
            this.chart1.PaletteCustomColors = new System.Drawing.Color[] {
        System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0))))),
        System.Drawing.Color.Gray};
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Doughnut;
            series1.CustomProperties = "DrawSideBySide=True, LabelStyle=Top";
            series1.Font = new System.Drawing.Font("Rubik", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            series1.IsValueShownAsLabel = true;
            series1.Legend = "Legend0";
            series1.Name = "Series4";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend0";
            series2.Name = "Series2";
            this.chart1.Series.Add(series1);
            this.chart1.Series.Add(series2);
            this.chart1.Size = new System.Drawing.Size(1262, 328);
            this.chart1.SuppressExceptions = true;
            this.chart1.TabIndex = 412;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.refreshPageToolStripMenuItem,
            this.showHideFiltersToolStripMenuItem,
            this.clearAllFiltersToolStripMenuItem,
            this.showDetailsInSearchToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(214, 100);
            // 
            // refreshPageToolStripMenuItem
            // 
            this.refreshPageToolStripMenuItem.Name = "refreshPageToolStripMenuItem";
            this.refreshPageToolStripMenuItem.Size = new System.Drawing.Size(213, 24);
            this.refreshPageToolStripMenuItem.Text = "Refresh Page";
            this.refreshPageToolStripMenuItem.Click += new System.EventHandler(this.refreshPageToolStripMenuItem_Click);
            // 
            // showHideFiltersToolStripMenuItem
            // 
            this.showHideFiltersToolStripMenuItem.Name = "showHideFiltersToolStripMenuItem";
            this.showHideFiltersToolStripMenuItem.Size = new System.Drawing.Size(213, 24);
            this.showHideFiltersToolStripMenuItem.Text = "Show/Hide Filters";
            this.showHideFiltersToolStripMenuItem.Click += new System.EventHandler(this.showHideFiltersToolStripMenuItem_Click);
            // 
            // clearAllFiltersToolStripMenuItem
            // 
            this.clearAllFiltersToolStripMenuItem.Name = "clearAllFiltersToolStripMenuItem";
            this.clearAllFiltersToolStripMenuItem.Size = new System.Drawing.Size(213, 24);
            this.clearAllFiltersToolStripMenuItem.Text = "Clear all filters";
            this.clearAllFiltersToolStripMenuItem.Click += new System.EventHandler(this.clearAllFiltersToolStripMenuItem_Click);
            // 
            // showDetailsInSearchToolStripMenuItem
            // 
            this.showDetailsInSearchToolStripMenuItem.Name = "showDetailsInSearchToolStripMenuItem";
            this.showDetailsInSearchToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.D)));
            this.showDetailsInSearchToolStripMenuItem.Size = new System.Drawing.Size(213, 24);
            this.showDetailsInSearchToolStripMenuItem.Text = "Show Details";
            this.showDetailsInSearchToolStripMenuItem.Click += new System.EventHandler(this.showDetailsToolStripMenuItem_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.ColumnCount = 12;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 197F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 161F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 87F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 167F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 122F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 136F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.tableLayoutPanel1.ContextMenuStrip = this.contextMenuStrip1;
            this.tableLayoutPanel1.Controls.Add(this.comboBox1, 6, 0);
            this.tableLayoutPanel1.Controls.Add(this.AdvancedSearch_button, 9, 0);
            this.tableLayoutPanel1.Controls.Add(this.label16, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.X_Axis_comboBox, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.results_dataGridView, 11, 0);
            this.tableLayoutPanel1.Controls.Add(this.ApplyFilter_button, 8, 0);
            this.tableLayoutPanel1.Controls.Add(this.reportingCitiesLabel, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.reportingScope, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.withFunded_checkBox, 7, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1262, 45);
            this.tableLayoutPanel1.TabIndex = 413;
            // 
            // comboBox1
            // 
            this.comboBox1.BackColor = System.Drawing.Color.White;
            this.comboBox1.ContextMenuStrip = this.contextMenuStrip1;
            this.comboBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.comboBox1.ForeColor = System.Drawing.Color.Black;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(628, 5);
            this.comboBox1.Margin = new System.Windows.Forms.Padding(8, 5, 3, 5);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(156, 34);
            this.comboBox1.TabIndex = 462;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // AdvancedSearch_button
            // 
            this.AdvancedSearch_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.AdvancedSearch_button.BackColor = System.Drawing.Color.Transparent;
            this.AdvancedSearch_button.BackgroundImage = global::MyWorkApplication.Properties.Resources.Show2_L;
            this.AdvancedSearch_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.AdvancedSearch_button.ContextMenuStrip = this.contextMenuStrip1;
            this.AdvancedSearch_button.FlatAppearance.BorderSize = 0;
            this.AdvancedSearch_button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.AdvancedSearch_button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.AdvancedSearch_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AdvancedSearch_button.Location = new System.Drawing.Point(1073, 2);
            this.AdvancedSearch_button.Margin = new System.Windows.Forms.Padding(0);
            this.AdvancedSearch_button.MaximumSize = new System.Drawing.Size(150, 40);
            this.AdvancedSearch_button.MinimumSize = new System.Drawing.Size(150, 40);
            this.AdvancedSearch_button.Name = "AdvancedSearch_button";
            this.AdvancedSearch_button.Size = new System.Drawing.Size(150, 40);
            this.AdvancedSearch_button.TabIndex = 441;
            this.AdvancedSearch_button.UseVisualStyleBackColor = false;
            this.AdvancedSearch_button.Click += new System.EventHandler(this.AdvancedSearch_button_Click);
            this.AdvancedSearch_button.MouseEnter += new System.EventHandler(this.AdvancedSearch_button_MouseEnter);
            this.AdvancedSearch_button.MouseLeave += new System.EventHandler(this.AdvancedSearch_button_MouseLeave);
            // 
            // label16
            // 
            this.label16.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label16.AutoSize = true;
            this.label16.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.label16.Location = new System.Drawing.Point(36, 9);
            this.label16.Margin = new System.Windows.Forms.Padding(7, 9, 7, 9);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(52, 27);
            this.label16.TabIndex = 296;
            this.label16.Text = "X-AXIS:";
            // 
            // X_Axis_comboBox
            // 
            this.X_Axis_comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.X_Axis_comboBox.BackColor = System.Drawing.Color.White;
            this.X_Axis_comboBox.ContextMenuStrip = this.contextMenuStrip1;
            this.X_Axis_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.X_Axis_comboBox.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.X_Axis_comboBox.ForeColor = System.Drawing.Color.Black;
            this.X_Axis_comboBox.FormattingEnabled = true;
            this.X_Axis_comboBox.Location = new System.Drawing.Point(98, 10);
            this.X_Axis_comboBox.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.X_Axis_comboBox.Name = "X_Axis_comboBox";
            this.X_Axis_comboBox.Size = new System.Drawing.Size(191, 34);
            this.X_Axis_comboBox.TabIndex = 439;
            this.X_Axis_comboBox.SelectedIndexChanged += new System.EventHandler(this.X_Axis_comboBox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.label1.Location = new System.Drawing.Point(554, 9);
            this.label1.Margin = new System.Windows.Forms.Padding(7, 9, 7, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 26);
            this.label1.TabIndex = 463;
            this.label1.Text = "Chart:";
            // 
            // results_dataGridView
            // 
            this.results_dataGridView.AllowUserToAddRows = false;
            this.results_dataGridView.AllowUserToDeleteRows = false;
            this.results_dataGridView.AllowUserToOrderColumns = true;
            this.results_dataGridView.AllowUserToResizeRows = false;
            this.results_dataGridView.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.results_dataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.results_dataGridView.BackgroundColor = System.Drawing.Color.Gray;
            this.results_dataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Rubik", 10F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(10)))), ((int)(((byte)(10)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.results_dataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.results_dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Rubik", 10F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(10)))), ((int)(((byte)(10)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.results_dataGridView.DefaultCellStyle = dataGridViewCellStyle2;
            this.results_dataGridView.Location = new System.Drawing.Point(1237, 6);
            this.results_dataGridView.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.results_dataGridView.Name = "results_dataGridView";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Rubik", 10F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(10)))), ((int)(((byte)(10)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.results_dataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.results_dataGridView.RowHeadersVisible = false;
            this.results_dataGridView.RowHeadersWidth = 51;
            this.results_dataGridView.RowTemplate.Height = 26;
            this.results_dataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.results_dataGridView.Size = new System.Drawing.Size(21, 32);
            this.results_dataGridView.TabIndex = 442;
            this.results_dataGridView.Visible = false;
            // 
            // withFunded_checkBox
            // 
            this.withFunded_checkBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.withFunded_checkBox.AutoSize = true;
            this.withFunded_checkBox.Checked = true;
            this.withFunded_checkBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.withFunded_checkBox.Font = new System.Drawing.Font("Rubik", 9F);
            this.withFunded_checkBox.Location = new System.Drawing.Point(790, 9);
            this.withFunded_checkBox.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.withFunded_checkBox.Name = "withFunded_checkBox";
            this.withFunded_checkBox.Size = new System.Drawing.Size(158, 27);
            this.withFunded_checkBox.TabIndex = 546;
            this.withFunded_checkBox.Text = "The Columns: All + Funded";
            this.withFunded_checkBox.ThreeState = true;
            this.withFunded_checkBox.UseVisualStyleBackColor = true;
            this.withFunded_checkBox.CheckStateChanged += new System.EventHandler(this.WithFunded_checkBox_CheckStateChanged);
            // 
            // ApplyFilter_button
            // 
            this.ApplyFilter_button.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.ApplyFilter_button.BackColor = System.Drawing.Color.Transparent;
            this.ApplyFilter_button.BackgroundImage = global::MyWorkApplication.Properties.Resources.Apply1;
            this.ApplyFilter_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ApplyFilter_button.ContextMenuStrip = this.contextMenuStrip1;
            this.ApplyFilter_button.FlatAppearance.BorderSize = 0;
            this.ApplyFilter_button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.ApplyFilter_button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.ApplyFilter_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ApplyFilter_button.ForeColor = System.Drawing.Color.LightGray;
            this.ApplyFilter_button.Location = new System.Drawing.Point(963, 5);
            this.ApplyFilter_button.Margin = new System.Windows.Forms.Padding(0);
            this.ApplyFilter_button.MaximumSize = new System.Drawing.Size(110, 34);
            this.ApplyFilter_button.MinimumSize = new System.Drawing.Size(110, 34);
            this.ApplyFilter_button.Name = "ApplyFilter_button";
            this.ApplyFilter_button.Size = new System.Drawing.Size(110, 34);
            this.ApplyFilter_button.TabIndex = 652;
            this.ApplyFilter_button.UseVisualStyleBackColor = false;
            this.ApplyFilter_button.Visible = false;
            this.ApplyFilter_button.Click += new System.EventHandler(this.ApplyFilter_button_Click);
            this.ApplyFilter_button.MouseEnter += new System.EventHandler(this.ApplyFilter_button_MouseEnter);
            this.ApplyFilter_button.MouseLeave += new System.EventHandler(this.ApplyFilter_button_MouseLeave);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.tableLayoutPanel2.ColumnCount = 6;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15.41067F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17.2735F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 67.31583F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Controls.Add(this.ExportToExcel_button, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.Count_all_label, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.Count_funded_label, 3, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 639);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1262, 34);
            this.tableLayoutPanel2.TabIndex = 415;
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
            this.ExportToExcel_button.Location = new System.Drawing.Point(30, 1);
            this.ExportToExcel_button.Margin = new System.Windows.Forms.Padding(0);
            this.ExportToExcel_button.MaximumSize = new System.Drawing.Size(30, 31);
            this.ExportToExcel_button.MinimumSize = new System.Drawing.Size(30, 31);
            this.ExportToExcel_button.Name = "ExportToExcel_button";
            this.ExportToExcel_button.Size = new System.Drawing.Size(30, 31);
            this.ExportToExcel_button.TabIndex = 411;
            this.ExportToExcel_button.UseVisualStyleBackColor = false;
            this.ExportToExcel_button.Click += new System.EventHandler(this.ExportToExcel_button_Click);
            this.ExportToExcel_button.MouseEnter += new System.EventHandler(this.ExportToExcel_button_MouseEnter);
            this.ExportToExcel_button.MouseLeave += new System.EventHandler(this.ExportToExcel_button_MouseLeave);
            // 
            // Count_all_label
            // 
            this.Count_all_label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.Count_all_label.AutoSize = true;
            this.Count_all_label.BackColor = System.Drawing.Color.Transparent;
            this.Count_all_label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.Count_all_label.Location = new System.Drawing.Point(60, 4);
            this.Count_all_label.Margin = new System.Windows.Forms.Padding(0);
            this.Count_all_label.Name = "Count_all_label";
            this.Count_all_label.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Count_all_label.Size = new System.Drawing.Size(181, 26);
            this.Count_all_label.TabIndex = 476;
            this.Count_all_label.Text = "Count";
            this.Count_all_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Count_funded_label
            // 
            this.Count_funded_label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.Count_funded_label.AutoSize = true;
            this.Count_funded_label.BackColor = System.Drawing.Color.Transparent;
            this.Count_funded_label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.Count_funded_label.Location = new System.Drawing.Point(241, 4);
            this.Count_funded_label.Margin = new System.Windows.Forms.Padding(0);
            this.Count_funded_label.Name = "Count_funded_label";
            this.Count_funded_label.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Count_funded_label.Size = new System.Drawing.Size(203, 26);
            this.Count_funded_label.TabIndex = 477;
            this.Count_funded_label.Text = "Count";
            this.Count_funded_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Search_panel
            // 
            this.Search_panel.BackColor = System.Drawing.Color.Transparent;
            this.Search_panel.Controls.Add(this.filters_tableLayoutPanel);
            this.Search_panel.Dock = System.Windows.Forms.DockStyle.Top;
            this.Search_panel.Location = new System.Drawing.Point(0, 70);
            this.Search_panel.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.Search_panel.Name = "Search_panel";
            this.Search_panel.Size = new System.Drawing.Size(1262, 241);
            this.Search_panel.TabIndex = 416;
            // 
            // filters_tableLayoutPanel
            // 
            this.filters_tableLayoutPanel.ColumnCount = 13;
            this.filters_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.filters_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.filters_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.filters_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.filters_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.filters_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.filters_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.filters_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.filters_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.filters_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.filters_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.filters_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.filters_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.filters_tableLayoutPanel.ContextMenuStrip = this.contextMenuStrip1;
            this.filters_tableLayoutPanel.Controls.Add(this.Type_label, 3, 0);
            this.filters_tableLayoutPanel.Controls.Add(this.DonorGroup_label, 7, 1);
            this.filters_tableLayoutPanel.Controls.Add(this.MaritalStatus_label, 9, 2);
            this.filters_tableLayoutPanel.Controls.Add(this.MP_Status_label, 1, 1);
            this.filters_tableLayoutPanel.Controls.Add(this.Donor_label, 3, 1);
            this.filters_tableLayoutPanel.Controls.Add(this.P_Parish_label, 9, 1);
            this.filters_tableLayoutPanel.Controls.Add(this.Category_label, 1, 2);
            this.filters_tableLayoutPanel.Controls.Add(this.Age_label, 7, 3);
            this.filters_tableLayoutPanel.Controls.Add(this.SubCategory_label, 3, 2);
            this.filters_tableLayoutPanel.Controls.Add(this.Gender_label, 9, 3);
            this.filters_tableLayoutPanel.Controls.Add(this.Career_label, 7, 2);
            this.filters_tableLayoutPanel.Controls.Add(this.Partnership_label, 9, 0);
            this.filters_tableLayoutPanel.Controls.Add(this.FundType_label, 1, 0);
            this.filters_tableLayoutPanel.Controls.Add(this.SubType_label, 7, 0);
            this.filters_tableLayoutPanel.Controls.Add(this.FundType_comboBox, 2, 0);
            this.filters_tableLayoutPanel.Controls.Add(this.Type_comboBox, 5, 0);
            this.filters_tableLayoutPanel.Controls.Add(this.SubType_comboBox, 8, 0);
            this.filters_tableLayoutPanel.Controls.Add(this.Partnership_comboBox, 11, 0);
            this.filters_tableLayoutPanel.Controls.Add(this.MP_Status_comboBox, 2, 1);
            this.filters_tableLayoutPanel.Controls.Add(this.Donor_comboBox, 5, 1);
            this.filters_tableLayoutPanel.Controls.Add(this.DonorGroup_comboBox, 8, 1);
            this.filters_tableLayoutPanel.Controls.Add(this.P_Parish_comboBox, 11, 1);
            this.filters_tableLayoutPanel.Controls.Add(this.Category_comboBox, 2, 2);
            this.filters_tableLayoutPanel.Controls.Add(this.SubCategory_comboBox, 5, 2);
            this.filters_tableLayoutPanel.Controls.Add(this.Career_comboBox, 8, 2);
            this.filters_tableLayoutPanel.Controls.Add(this.MaritalStatus_comboBox, 11, 2);
            this.filters_tableLayoutPanel.Controls.Add(this.Age_comboBox, 8, 3);
            this.filters_tableLayoutPanel.Controls.Add(this.Gender_comboBox, 11, 3);
            this.filters_tableLayoutPanel.Controls.Add(this.HomeStreet_label, 1, 3);
            this.filters_tableLayoutPanel.Controls.Add(this.HomeStreet_comboBox, 2, 3);
            this.filters_tableLayoutPanel.Controls.Add(this.ProjectStreet_label, 3, 3);
            this.filters_tableLayoutPanel.Controls.Add(this.ProjectStreet_comboBox, 5, 3);
            this.filters_tableLayoutPanel.Controls.Add(this.ApplyDateFrom_label, 1, 6);
            this.filters_tableLayoutPanel.Controls.Add(this.FundedDateFrom_label, 7, 6);
            this.filters_tableLayoutPanel.Controls.Add(this.FundDate_comboBox, 11, 6);
            this.filters_tableLayoutPanel.Controls.Add(this.ApplyDate_comboBox, 5, 6);
            this.filters_tableLayoutPanel.Controls.Add(this.ApplyDate_checkBox, 4, 6);
            this.filters_tableLayoutPanel.Controls.Add(this.FundedDate_checkBox, 10, 6);
            this.filters_tableLayoutPanel.Controls.Add(this.ApplyDateFrom_DateTimePicker, 2, 6);
            this.filters_tableLayoutPanel.Controls.Add(this.ApplyDateTo_DateTimePicker, 3, 6);
            this.filters_tableLayoutPanel.Controls.Add(this.FundedDateFrom_DateTimePicker, 8, 6);
            this.filters_tableLayoutPanel.Controls.Add(this.FundedDateTo_DateTimePicker, 9, 6);
            this.filters_tableLayoutPanel.Controls.Add(this.RequestedFrom_label, 1, 7);
            this.filters_tableLayoutPanel.Controls.Add(this.FundedFrom_label, 7, 7);
            this.filters_tableLayoutPanel.Controls.Add(this.RequestedFrom_textBox, 2, 7);
            this.filters_tableLayoutPanel.Controls.Add(this.RequestedTo_textBox, 3, 7);
            this.filters_tableLayoutPanel.Controls.Add(this.FundedFrom_textBox, 8, 7);
            this.filters_tableLayoutPanel.Controls.Add(this.FundedTo_textBox, 9, 7);
            this.filters_tableLayoutPanel.Controls.Add(this.from_label1, 2, 5);
            this.filters_tableLayoutPanel.Controls.Add(this.to_label1, 3, 5);
            this.filters_tableLayoutPanel.Controls.Add(this.year_label1, 5, 5);
            this.filters_tableLayoutPanel.Controls.Add(this.from_label2, 8, 5);
            this.filters_tableLayoutPanel.Controls.Add(this.to_label2, 9, 5);
            this.filters_tableLayoutPanel.Controls.Add(this.year_label2, 11, 5);
            this.filters_tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.filters_tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.filters_tableLayoutPanel.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.filters_tableLayoutPanel.Name = "filters_tableLayoutPanel";
            this.filters_tableLayoutPanel.RowCount = 8;
            this.filters_tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.filters_tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.filters_tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.filters_tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.filters_tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.filters_tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.filters_tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.filters_tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.filters_tableLayoutPanel.Size = new System.Drawing.Size(1262, 241);
            this.filters_tableLayoutPanel.TabIndex = 500;
            // 
            // Type_label
            // 
            this.Type_label.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.Type_label.AutoSize = true;
            this.Type_label.BackColor = System.Drawing.Color.Transparent;
            this.filters_tableLayoutPanel.SetColumnSpan(this.Type_label, 2);
            this.Type_label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.Type_label.Location = new System.Drawing.Point(361, 3);
            this.Type_label.Margin = new System.Windows.Forms.Padding(0);
            this.Type_label.Name = "Type_label";
            this.Type_label.Size = new System.Drawing.Size(115, 26);
            this.Type_label.TabIndex = 592;
            this.Type_label.Text = "Project Type:";
            // 
            // DonorGroup_label
            // 
            this.DonorGroup_label.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.DonorGroup_label.AutoSize = true;
            this.DonorGroup_label.BackColor = System.Drawing.Color.Transparent;
            this.DonorGroup_label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.DonorGroup_label.Location = new System.Drawing.Point(669, 36);
            this.DonorGroup_label.Margin = new System.Windows.Forms.Padding(0);
            this.DonorGroup_label.Name = "DonorGroup_label";
            this.DonorGroup_label.Size = new System.Drawing.Size(116, 26);
            this.DonorGroup_label.TabIndex = 573;
            this.DonorGroup_label.Text = "Donor Group:";
            // 
            // MaritalStatus_label
            // 
            this.MaritalStatus_label.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.MaritalStatus_label.AutoSize = true;
            this.filters_tableLayoutPanel.SetColumnSpan(this.MaritalStatus_label, 2);
            this.MaritalStatus_label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.MaritalStatus_label.Location = new System.Drawing.Point(969, 69);
            this.MaritalStatus_label.Margin = new System.Windows.Forms.Padding(0);
            this.MaritalStatus_label.Name = "MaritalStatus_label";
            this.MaritalStatus_label.Size = new System.Drawing.Size(125, 26);
            this.MaritalStatus_label.TabIndex = 470;
            this.MaritalStatus_label.Text = "Marital Status:";
            // 
            // MP_Status_label
            // 
            this.MP_Status_label.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.MP_Status_label.AutoSize = true;
            this.MP_Status_label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.MP_Status_label.Location = new System.Drawing.Point(98, 36);
            this.MP_Status_label.Margin = new System.Windows.Forms.Padding(0);
            this.MP_Status_label.Name = "MP_Status_label";
            this.MP_Status_label.Size = new System.Drawing.Size(69, 26);
            this.MP_Status_label.TabIndex = 441;
            this.MP_Status_label.Text = "Status:";
            // 
            // Donor_label
            // 
            this.Donor_label.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.Donor_label.AutoSize = true;
            this.filters_tableLayoutPanel.SetColumnSpan(this.Donor_label, 2);
            this.Donor_label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.Donor_label.Location = new System.Drawing.Point(412, 36);
            this.Donor_label.Margin = new System.Windows.Forms.Padding(0);
            this.Donor_label.Name = "Donor_label";
            this.Donor_label.Size = new System.Drawing.Size(64, 26);
            this.Donor_label.TabIndex = 468;
            this.Donor_label.Text = "Donor:";
            // 
            // P_Parish_label
            // 
            this.P_Parish_label.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.P_Parish_label.AutoSize = true;
            this.filters_tableLayoutPanel.SetColumnSpan(this.P_Parish_label, 2);
            this.P_Parish_label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.P_Parish_label.Location = new System.Drawing.Point(1029, 36);
            this.P_Parish_label.Margin = new System.Windows.Forms.Padding(0);
            this.P_Parish_label.Name = "P_Parish_label";
            this.P_Parish_label.Size = new System.Drawing.Size(65, 26);
            this.P_Parish_label.TabIndex = 466;
            this.P_Parish_label.Text = "Parish:";
            // 
            // Category_label
            // 
            this.Category_label.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.Category_label.AutoSize = true;
            this.Category_label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.Category_label.Location = new System.Drawing.Point(79, 69);
            this.Category_label.Margin = new System.Windows.Forms.Padding(0);
            this.Category_label.Name = "Category_label";
            this.Category_label.Size = new System.Drawing.Size(88, 26);
            this.Category_label.TabIndex = 465;
            this.Category_label.Text = "Category:";
            // 
            // Age_label
            // 
            this.Age_label.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.Age_label.AutoSize = true;
            this.Age_label.BackColor = System.Drawing.Color.Transparent;
            this.Age_label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.Age_label.Location = new System.Drawing.Point(675, 102);
            this.Age_label.Margin = new System.Windows.Forms.Padding(0);
            this.Age_label.Name = "Age_label";
            this.Age_label.Size = new System.Drawing.Size(110, 26);
            this.Age_label.TabIndex = 496;
            this.Age_label.Text = "Age Ranges:";
            // 
            // SubCategory_label
            // 
            this.SubCategory_label.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.SubCategory_label.AutoSize = true;
            this.SubCategory_label.BackColor = System.Drawing.Color.Transparent;
            this.filters_tableLayoutPanel.SetColumnSpan(this.SubCategory_label, 2);
            this.SubCategory_label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.SubCategory_label.Location = new System.Drawing.Point(353, 69);
            this.SubCategory_label.Margin = new System.Windows.Forms.Padding(0);
            this.SubCategory_label.Name = "SubCategory_label";
            this.SubCategory_label.Size = new System.Drawing.Size(123, 26);
            this.SubCategory_label.TabIndex = 569;
            this.SubCategory_label.Text = "Sub Category:";
            // 
            // Gender_label
            // 
            this.Gender_label.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.Gender_label.AutoSize = true;
            this.filters_tableLayoutPanel.SetColumnSpan(this.Gender_label, 2);
            this.Gender_label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.Gender_label.Location = new System.Drawing.Point(1020, 102);
            this.Gender_label.Margin = new System.Windows.Forms.Padding(0);
            this.Gender_label.Name = "Gender_label";
            this.Gender_label.Size = new System.Drawing.Size(74, 26);
            this.Gender_label.TabIndex = 467;
            this.Gender_label.Text = "Gender:";
            // 
            // Career_label
            // 
            this.Career_label.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.Career_label.AutoSize = true;
            this.Career_label.BackColor = System.Drawing.Color.Transparent;
            this.Career_label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.Career_label.Location = new System.Drawing.Point(717, 69);
            this.Career_label.Margin = new System.Windows.Forms.Padding(0);
            this.Career_label.Name = "Career_label";
            this.Career_label.Size = new System.Drawing.Size(68, 26);
            this.Career_label.TabIndex = 589;
            this.Career_label.Text = "Career:";
            // 
            // Partnership_label
            // 
            this.Partnership_label.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.Partnership_label.AutoSize = true;
            this.Partnership_label.BackColor = System.Drawing.Color.Transparent;
            this.filters_tableLayoutPanel.SetColumnSpan(this.Partnership_label, 2);
            this.Partnership_label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.Partnership_label.Location = new System.Drawing.Point(986, 3);
            this.Partnership_label.Margin = new System.Windows.Forms.Padding(0);
            this.Partnership_label.Name = "Partnership_label";
            this.Partnership_label.Size = new System.Drawing.Size(108, 26);
            this.Partnership_label.TabIndex = 591;
            this.Partnership_label.Text = "Partnership:";
            // 
            // FundType_label
            // 
            this.FundType_label.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.FundType_label.AutoSize = true;
            this.FundType_label.BackColor = System.Drawing.Color.Transparent;
            this.FundType_label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.FundType_label.Location = new System.Drawing.Point(69, 3);
            this.FundType_label.Margin = new System.Windows.Forms.Padding(0);
            this.FundType_label.Name = "FundType_label";
            this.FundType_label.Size = new System.Drawing.Size(98, 26);
            this.FundType_label.TabIndex = 590;
            this.FundType_label.Text = "Fund Type:";
            // 
            // SubType_label
            // 
            this.SubType_label.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.SubType_label.AutoSize = true;
            this.SubType_label.BackColor = System.Drawing.Color.Transparent;
            this.SubType_label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.SubType_label.Location = new System.Drawing.Point(677, 0);
            this.SubType_label.Margin = new System.Windows.Forms.Padding(0);
            this.SubType_label.Name = "SubType_label";
            this.SubType_label.Size = new System.Drawing.Size(108, 33);
            this.SubType_label.TabIndex = 593;
            this.SubType_label.Text = "Project Sub Type:";
            // 
            // FundType_comboBox
            // 
            this.FundType_comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.FundType_comboBox.BackColor = System.Drawing.Color.Gray;
            checkBoxProperties2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            checkBoxProperties2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FundType_comboBox.CheckBoxProperties = checkBoxProperties2;
            this.FundType_comboBox.DisplayMemberSingleItem = "";
            this.FundType_comboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.FundType_comboBox.ForeColor = System.Drawing.Color.White;
            this.FundType_comboBox.FormattingEnabled = true;
            this.FundType_comboBox.Location = new System.Drawing.Point(170, 3);
            this.FundType_comboBox.Name = "FundType_comboBox";
            this.FundType_comboBox.Size = new System.Drawing.Size(136, 34);
            this.FundType_comboBox.TabIndex = 598;
            // 
            // Type_comboBox
            // 
            this.Type_comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.Type_comboBox.BackColor = System.Drawing.Color.Gray;
            checkBoxProperties3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            checkBoxProperties3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Type_comboBox.CheckBoxProperties = checkBoxProperties3;
            this.Type_comboBox.DisplayMemberSingleItem = "";
            this.Type_comboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Type_comboBox.ForeColor = System.Drawing.Color.White;
            this.Type_comboBox.FormattingEnabled = true;
            this.Type_comboBox.Location = new System.Drawing.Point(479, 3);
            this.Type_comboBox.Name = "Type_comboBox";
            this.Type_comboBox.Size = new System.Drawing.Size(136, 34);
            this.Type_comboBox.TabIndex = 599;
            this.Type_comboBox.SelectedValueChanged += new System.EventHandler(this.Type_comboBox_SelectedValueChanged);
            // 
            // SubType_comboBox
            // 
            this.SubType_comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.SubType_comboBox.BackColor = System.Drawing.Color.Gray;
            checkBoxProperties4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            checkBoxProperties4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.SubType_comboBox.CheckBoxProperties = checkBoxProperties4;
            this.SubType_comboBox.DisplayMemberSingleItem = "";
            this.SubType_comboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SubType_comboBox.ForeColor = System.Drawing.Color.White;
            this.SubType_comboBox.FormattingEnabled = true;
            this.SubType_comboBox.Location = new System.Drawing.Point(788, 3);
            this.SubType_comboBox.Name = "SubType_comboBox";
            this.SubType_comboBox.Size = new System.Drawing.Size(136, 34);
            this.SubType_comboBox.TabIndex = 600;
            // 
            // Partnership_comboBox
            // 
            this.Partnership_comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.Partnership_comboBox.BackColor = System.Drawing.Color.Gray;
            checkBoxProperties5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            checkBoxProperties5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Partnership_comboBox.CheckBoxProperties = checkBoxProperties5;
            this.Partnership_comboBox.DisplayMemberSingleItem = "";
            this.Partnership_comboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Partnership_comboBox.ForeColor = System.Drawing.Color.White;
            this.Partnership_comboBox.FormattingEnabled = true;
            this.Partnership_comboBox.Items.AddRange(new object[] {
            "فردي",
            "شراكة"});
            this.Partnership_comboBox.Location = new System.Drawing.Point(1097, 3);
            this.Partnership_comboBox.Name = "Partnership_comboBox";
            this.Partnership_comboBox.Size = new System.Drawing.Size(136, 34);
            this.Partnership_comboBox.TabIndex = 601;
            // 
            // MP_Status_comboBox
            // 
            this.MP_Status_comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.MP_Status_comboBox.BackColor = System.Drawing.Color.Gray;
            checkBoxProperties6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            checkBoxProperties6.ForeColor = System.Drawing.SystemColors.ControlText;
            this.MP_Status_comboBox.CheckBoxProperties = checkBoxProperties6;
            this.MP_Status_comboBox.DisplayMemberSingleItem = "";
            this.MP_Status_comboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.MP_Status_comboBox.ForeColor = System.Drawing.Color.White;
            this.MP_Status_comboBox.FormattingEnabled = true;
            this.MP_Status_comboBox.Location = new System.Drawing.Point(170, 36);
            this.MP_Status_comboBox.Name = "MP_Status_comboBox";
            this.MP_Status_comboBox.Size = new System.Drawing.Size(136, 34);
            this.MP_Status_comboBox.TabIndex = 602;
            // 
            // Donor_comboBox
            // 
            this.Donor_comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.Donor_comboBox.BackColor = System.Drawing.Color.Gray;
            checkBoxProperties7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            checkBoxProperties7.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Donor_comboBox.CheckBoxProperties = checkBoxProperties7;
            this.Donor_comboBox.DisplayMemberSingleItem = "";
            this.Donor_comboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Donor_comboBox.ForeColor = System.Drawing.Color.White;
            this.Donor_comboBox.FormattingEnabled = true;
            this.Donor_comboBox.Location = new System.Drawing.Point(479, 36);
            this.Donor_comboBox.Name = "Donor_comboBox";
            this.Donor_comboBox.Size = new System.Drawing.Size(136, 34);
            this.Donor_comboBox.TabIndex = 603;
            this.Donor_comboBox.SelectedValueChanged += new System.EventHandler(this.Donor_comboBox_SelectedValueChanged);
            // 
            // DonorGroup_comboBox
            // 
            this.DonorGroup_comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.DonorGroup_comboBox.BackColor = System.Drawing.Color.Gray;
            checkBoxProperties8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            checkBoxProperties8.ForeColor = System.Drawing.SystemColors.ControlText;
            this.DonorGroup_comboBox.CheckBoxProperties = checkBoxProperties8;
            this.DonorGroup_comboBox.DisplayMemberSingleItem = "";
            this.DonorGroup_comboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DonorGroup_comboBox.ForeColor = System.Drawing.Color.White;
            this.DonorGroup_comboBox.FormattingEnabled = true;
            this.DonorGroup_comboBox.Location = new System.Drawing.Point(788, 36);
            this.DonorGroup_comboBox.Name = "DonorGroup_comboBox";
            this.DonorGroup_comboBox.Size = new System.Drawing.Size(136, 34);
            this.DonorGroup_comboBox.TabIndex = 604;
            this.DonorGroup_comboBox.Enter += new System.EventHandler(this.DonorGroup_comboBox_Enter);
            // 
            // P_Parish_comboBox
            // 
            this.P_Parish_comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.P_Parish_comboBox.BackColor = System.Drawing.Color.Gray;
            checkBoxProperties9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            checkBoxProperties9.ForeColor = System.Drawing.SystemColors.ControlText;
            this.P_Parish_comboBox.CheckBoxProperties = checkBoxProperties9;
            this.P_Parish_comboBox.DisplayMemberSingleItem = "";
            this.P_Parish_comboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.P_Parish_comboBox.ForeColor = System.Drawing.Color.White;
            this.P_Parish_comboBox.FormattingEnabled = true;
            this.P_Parish_comboBox.Location = new System.Drawing.Point(1097, 36);
            this.P_Parish_comboBox.Name = "P_Parish_comboBox";
            this.P_Parish_comboBox.Size = new System.Drawing.Size(136, 34);
            this.P_Parish_comboBox.TabIndex = 605;
            // 
            // Category_comboBox
            // 
            this.Category_comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.Category_comboBox.BackColor = System.Drawing.Color.Gray;
            checkBoxProperties10.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            checkBoxProperties10.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Category_comboBox.CheckBoxProperties = checkBoxProperties10;
            this.Category_comboBox.DisplayMemberSingleItem = "";
            this.Category_comboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Category_comboBox.ForeColor = System.Drawing.Color.White;
            this.Category_comboBox.FormattingEnabled = true;
            this.Category_comboBox.Location = new System.Drawing.Point(170, 69);
            this.Category_comboBox.Name = "Category_comboBox";
            this.Category_comboBox.Size = new System.Drawing.Size(136, 34);
            this.Category_comboBox.TabIndex = 606;
            // 
            // SubCategory_comboBox
            // 
            this.SubCategory_comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.SubCategory_comboBox.BackColor = System.Drawing.Color.Gray;
            checkBoxProperties11.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            checkBoxProperties11.ForeColor = System.Drawing.SystemColors.ControlText;
            this.SubCategory_comboBox.CheckBoxProperties = checkBoxProperties11;
            this.SubCategory_comboBox.DisplayMemberSingleItem = "";
            this.SubCategory_comboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SubCategory_comboBox.ForeColor = System.Drawing.Color.White;
            this.SubCategory_comboBox.FormattingEnabled = true;
            this.SubCategory_comboBox.Location = new System.Drawing.Point(479, 69);
            this.SubCategory_comboBox.Name = "SubCategory_comboBox";
            this.SubCategory_comboBox.Size = new System.Drawing.Size(136, 34);
            this.SubCategory_comboBox.TabIndex = 607;
            this.SubCategory_comboBox.Enter += new System.EventHandler(this.SubCategory_comboBox_Enter);
            // 
            // Career_comboBox
            // 
            this.Career_comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.Career_comboBox.BackColor = System.Drawing.Color.Gray;
            checkBoxProperties12.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            checkBoxProperties12.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Career_comboBox.CheckBoxProperties = checkBoxProperties12;
            this.Career_comboBox.DisplayMemberSingleItem = "";
            this.Career_comboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Career_comboBox.ForeColor = System.Drawing.Color.White;
            this.Career_comboBox.FormattingEnabled = true;
            this.Career_comboBox.Location = new System.Drawing.Point(788, 69);
            this.Career_comboBox.Name = "Career_comboBox";
            this.Career_comboBox.Size = new System.Drawing.Size(136, 34);
            this.Career_comboBox.TabIndex = 608;
            this.Career_comboBox.Enter += new System.EventHandler(this.Career_comboBox_Enter);
            // 
            // MaritalStatus_comboBox
            // 
            this.MaritalStatus_comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.MaritalStatus_comboBox.BackColor = System.Drawing.Color.Gray;
            checkBoxProperties13.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            checkBoxProperties13.ForeColor = System.Drawing.SystemColors.ControlText;
            this.MaritalStatus_comboBox.CheckBoxProperties = checkBoxProperties13;
            this.MaritalStatus_comboBox.DisplayMemberSingleItem = "";
            this.MaritalStatus_comboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.MaritalStatus_comboBox.ForeColor = System.Drawing.Color.White;
            this.MaritalStatus_comboBox.FormattingEnabled = true;
            this.MaritalStatus_comboBox.Location = new System.Drawing.Point(1097, 69);
            this.MaritalStatus_comboBox.Name = "MaritalStatus_comboBox";
            this.MaritalStatus_comboBox.Size = new System.Drawing.Size(136, 34);
            this.MaritalStatus_comboBox.TabIndex = 609;
            // 
            // Age_comboBox
            // 
            this.Age_comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.Age_comboBox.BackColor = System.Drawing.Color.Gray;
            checkBoxProperties14.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            checkBoxProperties14.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Age_comboBox.CheckBoxProperties = checkBoxProperties14;
            this.Age_comboBox.DisplayMemberSingleItem = "";
            this.Age_comboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Age_comboBox.ForeColor = System.Drawing.Color.White;
            this.Age_comboBox.FormattingEnabled = true;
            this.Age_comboBox.Location = new System.Drawing.Point(788, 102);
            this.Age_comboBox.Name = "Age_comboBox";
            this.Age_comboBox.Size = new System.Drawing.Size(136, 34);
            this.Age_comboBox.TabIndex = 612;
            // 
            // Gender_comboBox
            // 
            this.Gender_comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.Gender_comboBox.BackColor = System.Drawing.Color.Gray;
            checkBoxProperties15.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            checkBoxProperties15.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Gender_comboBox.CheckBoxProperties = checkBoxProperties15;
            this.Gender_comboBox.DisplayMemberSingleItem = "";
            this.Gender_comboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Gender_comboBox.ForeColor = System.Drawing.Color.White;
            this.Gender_comboBox.FormattingEnabled = true;
            this.Gender_comboBox.Items.AddRange(new object[] {
            "ذكر",
            "أنثى"});
            this.Gender_comboBox.Location = new System.Drawing.Point(1097, 102);
            this.Gender_comboBox.Name = "Gender_comboBox";
            this.Gender_comboBox.Size = new System.Drawing.Size(136, 34);
            this.Gender_comboBox.TabIndex = 613;
            // 
            // HomeStreet_label
            // 
            this.HomeStreet_label.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.HomeStreet_label.AutoSize = true;
            this.HomeStreet_label.BackColor = System.Drawing.Color.Transparent;
            this.HomeStreet_label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.HomeStreet_label.Location = new System.Drawing.Point(49, 102);
            this.HomeStreet_label.Margin = new System.Windows.Forms.Padding(0);
            this.HomeStreet_label.Name = "HomeStreet_label";
            this.HomeStreet_label.Size = new System.Drawing.Size(118, 26);
            this.HomeStreet_label.TabIndex = 594;
            this.HomeStreet_label.Text = "Home Street:";
            // 
            // HomeStreet_comboBox
            // 
            this.HomeStreet_comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.HomeStreet_comboBox.BackColor = System.Drawing.Color.Gray;
            checkBoxProperties16.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            checkBoxProperties16.ForeColor = System.Drawing.SystemColors.ControlText;
            this.HomeStreet_comboBox.CheckBoxProperties = checkBoxProperties16;
            this.HomeStreet_comboBox.DisplayMemberSingleItem = "";
            this.HomeStreet_comboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.HomeStreet_comboBox.ForeColor = System.Drawing.Color.White;
            this.HomeStreet_comboBox.FormattingEnabled = true;
            this.HomeStreet_comboBox.Location = new System.Drawing.Point(170, 102);
            this.HomeStreet_comboBox.Name = "HomeStreet_comboBox";
            this.HomeStreet_comboBox.Size = new System.Drawing.Size(136, 34);
            this.HomeStreet_comboBox.TabIndex = 614;
            // 
            // ProjectStreet_label
            // 
            this.ProjectStreet_label.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.ProjectStreet_label.AutoSize = true;
            this.ProjectStreet_label.BackColor = System.Drawing.Color.Transparent;
            this.filters_tableLayoutPanel.SetColumnSpan(this.ProjectStreet_label, 2);
            this.ProjectStreet_label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.ProjectStreet_label.Location = new System.Drawing.Point(348, 102);
            this.ProjectStreet_label.Margin = new System.Windows.Forms.Padding(0);
            this.ProjectStreet_label.Name = "ProjectStreet_label";
            this.ProjectStreet_label.Size = new System.Drawing.Size(128, 26);
            this.ProjectStreet_label.TabIndex = 596;
            this.ProjectStreet_label.Text = "Project Street:";
            // 
            // ProjectStreet_comboBox
            // 
            this.ProjectStreet_comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ProjectStreet_comboBox.BackColor = System.Drawing.Color.Gray;
            checkBoxProperties17.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            checkBoxProperties17.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ProjectStreet_comboBox.CheckBoxProperties = checkBoxProperties17;
            this.ProjectStreet_comboBox.DisplayMemberSingleItem = "";
            this.ProjectStreet_comboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ProjectStreet_comboBox.ForeColor = System.Drawing.Color.White;
            this.ProjectStreet_comboBox.FormattingEnabled = true;
            this.ProjectStreet_comboBox.Location = new System.Drawing.Point(479, 102);
            this.ProjectStreet_comboBox.Name = "ProjectStreet_comboBox";
            this.ProjectStreet_comboBox.Size = new System.Drawing.Size(136, 34);
            this.ProjectStreet_comboBox.TabIndex = 615;
            // 
            // ApplyDateFrom_label
            // 
            this.ApplyDateFrom_label.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.ApplyDateFrom_label.AutoSize = true;
            this.ApplyDateFrom_label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.ApplyDateFrom_label.Location = new System.Drawing.Point(62, 175);
            this.ApplyDateFrom_label.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.ApplyDateFrom_label.Name = "ApplyDateFrom_label";
            this.ApplyDateFrom_label.Size = new System.Drawing.Size(102, 26);
            this.ApplyDateFrom_label.TabIndex = 557;
            this.ApplyDateFrom_label.Text = "Apply Date:";
            // 
            // FundedDateFrom_label
            // 
            this.FundedDateFrom_label.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.FundedDateFrom_label.AutoSize = true;
            this.FundedDateFrom_label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.FundedDateFrom_label.Location = new System.Drawing.Point(684, 175);
            this.FundedDateFrom_label.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.FundedDateFrom_label.Name = "FundedDateFrom_label";
            this.FundedDateFrom_label.Size = new System.Drawing.Size(98, 26);
            this.FundedDateFrom_label.TabIndex = 558;
            this.FundedDateFrom_label.Text = "Fund Date:";
            // 
            // FundDate_comboBox
            // 
            this.FundDate_comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.FundDate_comboBox.BackColor = System.Drawing.Color.Gray;
            checkBoxProperties18.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            checkBoxProperties18.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FundDate_comboBox.CheckBoxProperties = checkBoxProperties18;
            this.FundDate_comboBox.DisplayMemberSingleItem = "";
            this.FundDate_comboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.FundDate_comboBox.ForeColor = System.Drawing.Color.White;
            this.FundDate_comboBox.FormattingEnabled = true;
            this.FundDate_comboBox.Location = new System.Drawing.Point(1097, 175);
            this.FundDate_comboBox.Name = "FundDate_comboBox";
            this.FundDate_comboBox.Size = new System.Drawing.Size(136, 34);
            this.FundDate_comboBox.TabIndex = 611;
            // 
            // ApplyDate_comboBox
            // 
            this.ApplyDate_comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ApplyDate_comboBox.BackColor = System.Drawing.Color.Gray;
            checkBoxProperties19.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            checkBoxProperties19.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ApplyDate_comboBox.CheckBoxProperties = checkBoxProperties19;
            this.ApplyDate_comboBox.DisplayMemberSingleItem = "";
            this.ApplyDate_comboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ApplyDate_comboBox.ForeColor = System.Drawing.Color.White;
            this.ApplyDate_comboBox.FormattingEnabled = true;
            this.ApplyDate_comboBox.Location = new System.Drawing.Point(479, 175);
            this.ApplyDate_comboBox.Name = "ApplyDate_comboBox";
            this.ApplyDate_comboBox.Size = new System.Drawing.Size(136, 34);
            this.ApplyDate_comboBox.TabIndex = 610;
            // 
            // ApplyDate_checkBox
            // 
            this.ApplyDate_checkBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ApplyDate_checkBox.AutoSize = true;
            this.ApplyDate_checkBox.Location = new System.Drawing.Point(454, 180);
            this.ApplyDate_checkBox.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.ApplyDate_checkBox.Name = "ApplyDate_checkBox";
            this.ApplyDate_checkBox.Size = new System.Drawing.Size(18, 17);
            this.ApplyDate_checkBox.TabIndex = 545;
            this.ApplyDate_checkBox.UseVisualStyleBackColor = true;
            this.ApplyDate_checkBox.CheckedChanged += new System.EventHandler(this.ApplyDate_checkBox_CheckedChanged);
            // 
            // FundedDate_checkBox
            // 
            this.FundedDate_checkBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.FundedDate_checkBox.AutoSize = true;
            this.FundedDate_checkBox.Location = new System.Drawing.Point(1072, 180);
            this.FundedDate_checkBox.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.FundedDate_checkBox.Name = "FundedDate_checkBox";
            this.FundedDate_checkBox.Size = new System.Drawing.Size(18, 17);
            this.FundedDate_checkBox.TabIndex = 546;
            this.FundedDate_checkBox.UseVisualStyleBackColor = true;
            this.FundedDate_checkBox.CheckedChanged += new System.EventHandler(this.FundedDate_checkBox_CheckedChanged);
            // 
            // ApplyDateFrom_DateTimePicker
            // 
            this.ApplyDateFrom_DateTimePicker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ApplyDateFrom_DateTimePicker.CustomFormat = "yyyy-MM-dd";
            this.ApplyDateFrom_DateTimePicker.Enabled = false;
            this.ApplyDateFrom_DateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.ApplyDateFrom_DateTimePicker.Location = new System.Drawing.Point(170, 177);
            this.ApplyDateFrom_DateTimePicker.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.ApplyDateFrom_DateTimePicker.MinDate = new System.DateTime(2018, 1, 1, 0, 0, 0, 0);
            this.ApplyDateFrom_DateTimePicker.Name = "ApplyDateFrom_DateTimePicker";
            this.ApplyDateFrom_DateTimePicker.RightToLeftLayout = true;
            this.ApplyDateFrom_DateTimePicker.Size = new System.Drawing.Size(136, 27);
            this.ApplyDateFrom_DateTimePicker.TabIndex = 584;
            this.ApplyDateFrom_DateTimePicker.Value = new System.DateTime(2018, 1, 1, 0, 0, 0, 0);
            this.ApplyDateFrom_DateTimePicker.ValueChanged += new System.EventHandler(this.ApplyDateFrom_dateTimePicker_ValueChanged);
            // 
            // ApplyDateTo_DateTimePicker
            // 
            this.ApplyDateTo_DateTimePicker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ApplyDateTo_DateTimePicker.CustomFormat = "yyyy-MM-dd";
            this.ApplyDateTo_DateTimePicker.Enabled = false;
            this.ApplyDateTo_DateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.ApplyDateTo_DateTimePicker.Location = new System.Drawing.Point(312, 177);
            this.ApplyDateTo_DateTimePicker.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.ApplyDateTo_DateTimePicker.MinDate = new System.DateTime(2018, 1, 1, 0, 0, 0, 0);
            this.ApplyDateTo_DateTimePicker.Name = "ApplyDateTo_DateTimePicker";
            this.ApplyDateTo_DateTimePicker.RightToLeftLayout = true;
            this.ApplyDateTo_DateTimePicker.Size = new System.Drawing.Size(136, 27);
            this.ApplyDateTo_DateTimePicker.TabIndex = 585;
            this.ApplyDateTo_DateTimePicker.Value = new System.DateTime(2023, 7, 13, 0, 0, 0, 0);
            this.ApplyDateTo_DateTimePicker.ValueChanged += new System.EventHandler(this.ApplyDateFrom_dateTimePicker_ValueChanged);
            // 
            // FundedDateFrom_DateTimePicker
            // 
            this.FundedDateFrom_DateTimePicker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.FundedDateFrom_DateTimePicker.CustomFormat = "yyyy-MM-dd";
            this.FundedDateFrom_DateTimePicker.Enabled = false;
            this.FundedDateFrom_DateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.FundedDateFrom_DateTimePicker.Location = new System.Drawing.Point(788, 177);
            this.FundedDateFrom_DateTimePicker.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.FundedDateFrom_DateTimePicker.MinDate = new System.DateTime(2018, 1, 1, 0, 0, 0, 0);
            this.FundedDateFrom_DateTimePicker.Name = "FundedDateFrom_DateTimePicker";
            this.FundedDateFrom_DateTimePicker.RightToLeftLayout = true;
            this.FundedDateFrom_DateTimePicker.Size = new System.Drawing.Size(136, 27);
            this.FundedDateFrom_DateTimePicker.TabIndex = 586;
            this.FundedDateFrom_DateTimePicker.Value = new System.DateTime(2018, 1, 1, 0, 0, 0, 0);
            this.FundedDateFrom_DateTimePicker.ValueChanged += new System.EventHandler(this.FundedDateTo_bcDateTimePicker_ValueChanged);
            // 
            // FundedDateTo_DateTimePicker
            // 
            this.FundedDateTo_DateTimePicker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.FundedDateTo_DateTimePicker.CustomFormat = "yyyy-MM-dd";
            this.FundedDateTo_DateTimePicker.Enabled = false;
            this.FundedDateTo_DateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.FundedDateTo_DateTimePicker.Location = new System.Drawing.Point(930, 177);
            this.FundedDateTo_DateTimePicker.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.FundedDateTo_DateTimePicker.MinDate = new System.DateTime(2018, 1, 1, 0, 0, 0, 0);
            this.FundedDateTo_DateTimePicker.Name = "FundedDateTo_DateTimePicker";
            this.FundedDateTo_DateTimePicker.RightToLeftLayout = true;
            this.FundedDateTo_DateTimePicker.Size = new System.Drawing.Size(136, 27);
            this.FundedDateTo_DateTimePicker.TabIndex = 587;
            this.FundedDateTo_DateTimePicker.Value = new System.DateTime(2023, 7, 13, 0, 0, 0, 0);
            this.FundedDateTo_DateTimePicker.ValueChanged += new System.EventHandler(this.FundedDateTo_bcDateTimePicker_ValueChanged);
            // 
            // RequestedFrom_label
            // 
            this.RequestedFrom_label.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.RequestedFrom_label.AutoSize = true;
            this.RequestedFrom_label.BackColor = System.Drawing.Color.Transparent;
            this.RequestedFrom_label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.RequestedFrom_label.Location = new System.Drawing.Point(61, 206);
            this.RequestedFrom_label.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.RequestedFrom_label.Name = "RequestedFrom_label";
            this.RequestedFrom_label.Size = new System.Drawing.Size(103, 34);
            this.RequestedFrom_label.TabIndex = 551;
            this.RequestedFrom_label.Text = "Requested Amount:";
            // 
            // FundedFrom_label
            // 
            this.FundedFrom_label.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.FundedFrom_label.AutoSize = true;
            this.FundedFrom_label.BackColor = System.Drawing.Color.Transparent;
            this.FundedFrom_label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.FundedFrom_label.Location = new System.Drawing.Point(659, 206);
            this.FundedFrom_label.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.FundedFrom_label.Name = "FundedFrom_label";
            this.FundedFrom_label.Size = new System.Drawing.Size(123, 34);
            this.FundedFrom_label.TabIndex = 553;
            this.FundedFrom_label.Text = "Fund Amount ($):";
            // 
            // RequestedFrom_textBox
            // 
            this.RequestedFrom_textBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.RequestedFrom_textBox.BackColor = System.Drawing.Color.White;
            this.RequestedFrom_textBox.Location = new System.Drawing.Point(170, 210);
            this.RequestedFrom_textBox.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.RequestedFrom_textBox.Name = "RequestedFrom_textBox";
            this.RequestedFrom_textBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RequestedFrom_textBox.Size = new System.Drawing.Size(136, 27);
            this.RequestedFrom_textBox.TabIndex = 549;
            this.RequestedFrom_textBox.TextChanged += new System.EventHandler(this.RequestedFrom_textBox_TextChanged);
            this.RequestedFrom_textBox.Leave += new System.EventHandler(this.RequestedFrom_textBox_Leave);
            // 
            // RequestedTo_textBox
            // 
            this.RequestedTo_textBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.RequestedTo_textBox.BackColor = System.Drawing.Color.White;
            this.RequestedTo_textBox.Location = new System.Drawing.Point(312, 210);
            this.RequestedTo_textBox.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.RequestedTo_textBox.Name = "RequestedTo_textBox";
            this.RequestedTo_textBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RequestedTo_textBox.Size = new System.Drawing.Size(136, 27);
            this.RequestedTo_textBox.TabIndex = 550;
            this.RequestedTo_textBox.TextChanged += new System.EventHandler(this.RequestedFrom_textBox_TextChanged);
            this.RequestedTo_textBox.Leave += new System.EventHandler(this.RequestedFrom_textBox_Leave);
            // 
            // FundedFrom_textBox
            // 
            this.FundedFrom_textBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.FundedFrom_textBox.BackColor = System.Drawing.Color.White;
            this.FundedFrom_textBox.Location = new System.Drawing.Point(788, 210);
            this.FundedFrom_textBox.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.FundedFrom_textBox.Name = "FundedFrom_textBox";
            this.FundedFrom_textBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.FundedFrom_textBox.Size = new System.Drawing.Size(136, 27);
            this.FundedFrom_textBox.TabIndex = 554;
            this.FundedFrom_textBox.TextChanged += new System.EventHandler(this.RequestedFrom_textBox_TextChanged);
            this.FundedFrom_textBox.Leave += new System.EventHandler(this.FundedFrom_textBox_Leave);
            // 
            // FundedTo_textBox
            // 
            this.FundedTo_textBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.FundedTo_textBox.BackColor = System.Drawing.Color.White;
            this.FundedTo_textBox.Location = new System.Drawing.Point(930, 210);
            this.FundedTo_textBox.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.FundedTo_textBox.Name = "FundedTo_textBox";
            this.FundedTo_textBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.FundedTo_textBox.Size = new System.Drawing.Size(136, 27);
            this.FundedTo_textBox.TabIndex = 555;
            this.FundedTo_textBox.TextChanged += new System.EventHandler(this.RequestedFrom_textBox_TextChanged);
            this.FundedTo_textBox.Leave += new System.EventHandler(this.FundedFrom_textBox_Leave);
            // 
            // from_label1
            // 
            this.from_label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.from_label1.AutoSize = true;
            this.from_label1.Enabled = false;
            this.from_label1.Font = new System.Drawing.Font("Rubik", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.from_label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.from_label1.Location = new System.Drawing.Point(170, 153);
            this.from_label1.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.from_label1.Name = "from_label1";
            this.from_label1.Size = new System.Drawing.Size(136, 18);
            this.from_label1.TabIndex = 617;
            this.from_label1.Text = "from";
            this.from_label1.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // to_label1
            // 
            this.to_label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.to_label1.AutoSize = true;
            this.to_label1.Enabled = false;
            this.to_label1.Font = new System.Drawing.Font("Rubik", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.to_label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.to_label1.Location = new System.Drawing.Point(312, 153);
            this.to_label1.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.to_label1.Name = "to_label1";
            this.to_label1.Size = new System.Drawing.Size(136, 18);
            this.to_label1.TabIndex = 643;
            this.to_label1.Text = "to";
            this.to_label1.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // year_label1
            // 
            this.year_label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.year_label1.AutoSize = true;
            this.year_label1.Enabled = false;
            this.year_label1.Font = new System.Drawing.Font("Rubik", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.year_label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.year_label1.Location = new System.Drawing.Point(479, 153);
            this.year_label1.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.year_label1.Name = "year_label1";
            this.year_label1.Size = new System.Drawing.Size(136, 18);
            this.year_label1.TabIndex = 646;
            this.year_label1.Text = "year";
            this.year_label1.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // from_label2
            // 
            this.from_label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.from_label2.AutoSize = true;
            this.from_label2.Enabled = false;
            this.from_label2.Font = new System.Drawing.Font("Rubik", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.from_label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.from_label2.Location = new System.Drawing.Point(788, 153);
            this.from_label2.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.from_label2.Name = "from_label2";
            this.from_label2.Size = new System.Drawing.Size(136, 18);
            this.from_label2.TabIndex = 647;
            this.from_label2.Text = "from";
            this.from_label2.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // to_label2
            // 
            this.to_label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.to_label2.AutoSize = true;
            this.to_label2.Enabled = false;
            this.to_label2.Font = new System.Drawing.Font("Rubik", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.to_label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.to_label2.Location = new System.Drawing.Point(930, 153);
            this.to_label2.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.to_label2.Name = "to_label2";
            this.to_label2.Size = new System.Drawing.Size(136, 18);
            this.to_label2.TabIndex = 648;
            this.to_label2.Text = "to";
            this.to_label2.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // year_label2
            // 
            this.year_label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.year_label2.AutoSize = true;
            this.year_label2.Enabled = false;
            this.year_label2.Font = new System.Drawing.Font("Rubik", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.year_label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.year_label2.Location = new System.Drawing.Point(1097, 153);
            this.year_label2.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.year_label2.Name = "year_label2";
            this.year_label2.Size = new System.Drawing.Size(136, 18);
            this.year_label2.TabIndex = 649;
            this.year_label2.Text = "year";
            this.year_label2.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // Statistics_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 26F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(1262, 673);
            this.ContextMenuStrip = this.contextMenuStrip1;
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.Search_panel);
            this.Controls.Add(this.reportingPanel);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Rubik", 10F);
            this.ForeColor = System.Drawing.Color.Black;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Statistics_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.Statistics_Form_Load);
            this.reportingPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.results_dataGridView)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.Search_panel.ResumeLayout(false);
            this.filters_tableLayoutPanel.ResumeLayout(false);
            this.filters_tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ComboBox X_Axis_comboBox;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Button AdvancedSearch_button;
        private System.Windows.Forms.DataGridView results_dataGridView;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button ExportToExcel_button;
        private System.Windows.Forms.Panel Search_panel;
        private System.Windows.Forms.Label Category_label;
        private System.Windows.Forms.Label P_Parish_label;
        private System.Windows.Forms.Label MP_Status_label;
        private System.Windows.Forms.Label MaritalStatus_label;
        private System.Windows.Forms.Label Donor_label;
        private System.Windows.Forms.Label Gender_label;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel filters_tableLayoutPanel;
        private System.Windows.Forms.Label Age_label;
        private System.Windows.Forms.Label Count_all_label;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem refreshPageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showHideFiltersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearAllFiltersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showDetailsInSearchToolStripMenuItem;
        private System.Windows.Forms.CheckBox ApplyDate_checkBox;
        private System.Windows.Forms.CheckBox FundedDate_checkBox;
        private System.Windows.Forms.TextBox FundedTo_textBox;
        private System.Windows.Forms.TextBox FundedFrom_textBox;
        private System.Windows.Forms.Label FundedFrom_label;
        private System.Windows.Forms.Label RequestedFrom_label;
        private System.Windows.Forms.TextBox RequestedFrom_textBox;
        private System.Windows.Forms.TextBox RequestedTo_textBox;
        private System.Windows.Forms.Label SubCategory_label;
        private System.Windows.Forms.Label DonorGroup_label;
        private System.Windows.Forms.DateTimePicker ApplyDateFrom_DateTimePicker;
        private System.Windows.Forms.DateTimePicker ApplyDateTo_DateTimePicker;
        private System.Windows.Forms.DateTimePicker FundedDateFrom_DateTimePicker;
        private System.Windows.Forms.DateTimePicker FundedDateTo_DateTimePicker;
        private System.Windows.Forms.Label Count_funded_label;
        private System.Windows.Forms.CheckBox withFunded_checkBox;
        private System.Windows.Forms.Label Career_label;
        private System.Windows.Forms.Label Type_label;
        private System.Windows.Forms.Label Partnership_label;
        private System.Windows.Forms.Label FundType_label;
        private System.Windows.Forms.Label SubType_label;
        private System.Windows.Forms.Label HomeStreet_label;
        private System.Windows.Forms.Label ProjectStreet_label;
        private PresentationControls.CheckBoxComboBox FundType_comboBox;
        private PresentationControls.CheckBoxComboBox Type_comboBox;
        private PresentationControls.CheckBoxComboBox SubType_comboBox;
        private PresentationControls.CheckBoxComboBox Partnership_comboBox;
        private PresentationControls.CheckBoxComboBox MP_Status_comboBox;
        private PresentationControls.CheckBoxComboBox Donor_comboBox;
        private PresentationControls.CheckBoxComboBox DonorGroup_comboBox;
        private PresentationControls.CheckBoxComboBox P_Parish_comboBox;
        private PresentationControls.CheckBoxComboBox Category_comboBox;
        private PresentationControls.CheckBoxComboBox SubCategory_comboBox;
        private PresentationControls.CheckBoxComboBox Career_comboBox;
        private PresentationControls.CheckBoxComboBox MaritalStatus_comboBox;
        private PresentationControls.CheckBoxComboBox Age_comboBox;
        private PresentationControls.CheckBoxComboBox Gender_comboBox;
        private PresentationControls.CheckBoxComboBox HomeStreet_comboBox;
        private PresentationControls.CheckBoxComboBox ProjectStreet_comboBox;
        private System.Windows.Forms.Label ApplyDateFrom_label;
        private PresentationControls.CheckBoxComboBox ApplyDate_comboBox;
        private System.Windows.Forms.Label FundedDateFrom_label;
        private PresentationControls.CheckBoxComboBox FundDate_comboBox;
        private System.Windows.Forms.Label from_label1;
        private System.Windows.Forms.Label to_label1;
        private System.Windows.Forms.Label year_label1;
        private System.Windows.Forms.Label from_label2;
        private System.Windows.Forms.Label to_label2;
        private System.Windows.Forms.Label year_label2;
        private System.Windows.Forms.Button ApplyFilter_button;
    }
}
