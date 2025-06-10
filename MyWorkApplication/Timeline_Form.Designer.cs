namespace MyWorkApplication
{
    partial class Timeline_Form
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series5 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Timeline_Form));
            this.MicroProject_ID_textBox = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.Person_Name_textBox = new System.Windows.Forms.TextBox();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.MP_dataGridView = new System.Windows.Forms.DataGridView();
            this.MP_Category_comboBox = new System.Windows.Forms.ComboBox();
            this.Show_button = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MP_dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // MicroProject_ID_textBox
            // 
            this.MicroProject_ID_textBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.MicroProject_ID_textBox.BackColor = System.Drawing.Color.White;
            this.MicroProject_ID_textBox.ForeColor = System.Drawing.Color.Black;
            this.MicroProject_ID_textBox.Location = new System.Drawing.Point(847, 19);
            this.MicroProject_ID_textBox.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.MicroProject_ID_textBox.Name = "MicroProject_ID_textBox";
            this.MicroProject_ID_textBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.MicroProject_ID_textBox.Size = new System.Drawing.Size(204, 29);
            this.MicroProject_ID_textBox.TabIndex = 411;
            this.MicroProject_ID_textBox.Leave += new System.EventHandler(this.MicroProject_ID_textBox2_Leave);
            // 
            // label21
            // 
            this.label21.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label21.AutoSize = true;
            this.label21.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.label21.Location = new System.Drawing.Point(1057, 20);
            this.label21.Name = "label21";
            this.label21.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label21.Size = new System.Drawing.Size(110, 27);
            this.label21.TabIndex = 409;
            this.label21.Text = "رقم المشروع:";
            this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label20
            // 
            this.label20.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label20.AutoSize = true;
            this.label20.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.label20.Location = new System.Drawing.Point(1057, 54);
            this.label20.Name = "label20";
            this.label20.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label20.Size = new System.Drawing.Size(123, 27);
            this.label20.TabIndex = 410;
            this.label20.Text = "اسم المستفيد:";
            this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Person_Name_textBox
            // 
            this.Person_Name_textBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Person_Name_textBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.Person_Name_textBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.Person_Name_textBox.BackColor = System.Drawing.Color.White;
            this.Person_Name_textBox.ForeColor = System.Drawing.Color.Black;
            this.Person_Name_textBox.Location = new System.Drawing.Point(847, 60);
            this.Person_Name_textBox.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.Person_Name_textBox.Name = "Person_Name_textBox";
            this.Person_Name_textBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Person_Name_textBox.Size = new System.Drawing.Size(204, 29);
            this.Person_Name_textBox.TabIndex = 412;
            this.Person_Name_textBox.Leave += new System.EventHandler(this.Person_Name_textBox2_Leave);
            // 
            // chart1
            // 
            this.chart1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            chartArea1.AlignmentOrientation = System.Windows.Forms.DataVisualization.Charting.AreaAlignmentOrientations.Horizontal;
            chartArea1.AlignmentStyle = System.Windows.Forms.DataVisualization.Charting.AreaAlignmentStyles.AxesView;
            chartArea1.Area3DStyle.PointDepth = 50;
            chartArea1.Area3DStyle.PointGapDepth = 80;
            chartArea1.Area3DStyle.WallWidth = 5;
            chartArea1.AxisX.InterlacedColor = System.Drawing.Color.Silver;
            chartArea1.AxisX.LabelAutoFitMaxFontSize = 11;
            chartArea1.AxisX.LabelAutoFitMinFontSize = 11;
            chartArea1.AxisX.LabelStyle.Font = new System.Drawing.Font("Avenir LT Std 35 Light", 11.25F);
            chartArea1.AxisX.LabelStyle.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Months;
            chartArea1.AxisX.LineWidth = 2;
            chartArea1.AxisX.MajorGrid.LineColor = System.Drawing.Color.Gainsboro;
            chartArea1.AxisX.MajorTickMark.LineColor = System.Drawing.Color.Silver;
            chartArea1.AxisX.MaximumAutoSize = 100F;
            chartArea1.AxisX.MinorGrid.LineColor = System.Drawing.Color.Silver;
            chartArea1.AxisX.ScaleBreakStyle.BreakLineStyle = System.Windows.Forms.DataVisualization.Charting.BreakLineStyle.Straight;
            chartArea1.AxisX.ScaleBreakStyle.Enabled = true;
            chartArea1.AxisX.ScaleBreakStyle.Spacing = 2D;
            chartArea1.AxisX.ScaleView.Zoomable = false;
            chartArea1.AxisX.TitleAlignment = System.Drawing.StringAlignment.Far;
            chartArea1.AxisX.TitleFont = new System.Drawing.Font("Avenir LT Std 35 Light", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea1.AxisY.IsLabelAutoFit = false;
            chartArea1.AxisY.LabelStyle.Font = new System.Drawing.Font("Avenir LT Std 35 Light", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea1.AxisY.LineWidth = 2;
            chartArea1.AxisY.MajorGrid.LineColor = System.Drawing.Color.Gainsboro;
            chartArea1.AxisY.MaximumAutoSize = 50F;
            chartArea1.AxisY.ScaleBreakStyle.CollapsibleSpaceThreshold = 10;
            chartArea1.AxisY.ScrollBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            chartArea1.AxisY.TitleFont = new System.Drawing.Font("Avenir LT Std 35 Light", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea1.BackColor = System.Drawing.Color.White;
            chartArea1.IsSameFontSizeForAllAxes = true;
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            this.chart1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chart1.IsSoftShadows = false;
            legend1.Alignment = System.Drawing.StringAlignment.Center;
            legend1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            legend1.DockedToChartArea = "ChartArea1";
            legend1.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Top;
            legend1.Font = new System.Drawing.Font("Avenir LT Std 35 Light", 10F);
            legend1.IsDockedInsideChartArea = false;
            legend1.IsEquallySpacedItems = true;
            legend1.IsTextAutoFit = false;
            legend1.LegendItemOrder = System.Windows.Forms.DataVisualization.Charting.LegendItemOrder.ReversedSeriesOrder;
            legend1.LegendStyle = System.Windows.Forms.DataVisualization.Charting.LegendStyle.Row;
            legend1.MaximumAutoSize = 40F;
            legend1.Name = "Legend1";
            legend1.TitleFont = new System.Drawing.Font("Avenir LT Std 35 Light", 11.25F);
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(0, 197);
            this.chart1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            series1.CustomProperties = "IsXAxisQuantitative=False";
            series1.Font = new System.Drawing.Font("Avenir LT Std 35 Light", 11.25F);
            series1.IsXValueIndexed = true;
            series1.Legend = "Legend1";
            series1.Name = "Apply Date";
            series1.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Date;
            series1.YValuesPerPoint = 4;
            series1.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            series2.Font = new System.Drawing.Font("Avenir LT Std 35 Light", 11.25F);
            series2.IsXValueIndexed = true;
            series2.Legend = "Legend1";
            series2.Name = "Recieve Date";
            series2.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Date;
            series3.ChartArea = "ChartArea1";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            series3.Font = new System.Drawing.Font("Avenir LT Std 35 Light", 11.25F);
            series3.IsXValueIndexed = true;
            series3.Legend = "Legend1";
            series3.Name = "Paid Paymnts";
            series3.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Date;
            series4.ChartArea = "ChartArea1";
            series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            series4.Font = new System.Drawing.Font("Avenir LT Std 35 Light", 11.25F);
            series4.IsXValueIndexed = true;
            series4.Legend = "Legend1";
            series4.Name = "Unpaid Payments";
            series4.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Date;
            series5.ChartArea = "ChartArea1";
            series5.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            series5.Font = new System.Drawing.Font("Avenir LT Std 35 Light", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            series5.IsXValueIndexed = true;
            series5.Legend = "Legend1";
            series5.Name = "End Of Project";
            series5.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Date;
            this.chart1.Series.Add(series1);
            this.chart1.Series.Add(series2);
            this.chart1.Series.Add(series3);
            this.chart1.Series.Add(series4);
            this.chart1.Series.Add(series5);
            this.chart1.Size = new System.Drawing.Size(1264, 602);
            this.chart1.SuppressExceptions = true;
            this.chart1.TabIndex = 413;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.ColumnCount = 6;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.Controls.Add(this.label21, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.MicroProject_ID_textBox, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label20, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.Person_Name_textBox, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.MP_dataGridView, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.MP_Category_comboBox, 5, 1);
            this.tableLayoutPanel1.Controls.Add(this.Show_button, 5, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 21F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 72F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1264, 197);
            this.tableLayoutPanel1.TabIndex = 421;
            // 
            // MP_dataGridView
            // 
            this.MP_dataGridView.AllowUserToAddRows = false;
            this.MP_dataGridView.AllowUserToDeleteRows = false;
            this.MP_dataGridView.AllowUserToOrderColumns = true;
            this.MP_dataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MP_dataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.MP_dataGridView.BackgroundColor = System.Drawing.Color.White;
            this.MP_dataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.MP_dataGridView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Rubik", 10.8F);
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.MP_dataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.MP_dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tableLayoutPanel1.SetColumnSpan(this.MP_dataGridView, 3);
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Rubik", 10.8F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.MP_dataGridView.DefaultCellStyle = dataGridViewCellStyle2;
            this.MP_dataGridView.EnableHeadersVisualStyles = false;
            this.MP_dataGridView.GridColor = System.Drawing.Color.Gray;
            this.MP_dataGridView.Location = new System.Drawing.Point(218, 18);
            this.MP_dataGridView.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MP_dataGridView.Name = "MP_dataGridView";
            this.MP_dataGridView.ReadOnly = true;
            this.MP_dataGridView.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.MP_dataGridView.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Rubik", 10.8F);
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.MP_dataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.MP_dataGridView.RowHeadersWidth = 51;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Rubik", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.MP_dataGridView.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.tableLayoutPanel1.SetRowSpan(this.MP_dataGridView, 2);
            this.MP_dataGridView.RowTemplate.Height = 26;
            this.MP_dataGridView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.MP_dataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.MP_dataGridView.Size = new System.Drawing.Size(622, 174);
            this.MP_dataGridView.TabIndex = 414;
            this.MP_dataGridView.SelectionChanged += new System.EventHandler(this.MP_dataGridView_SelectionChanged);
            // 
            // MP_Category_comboBox
            // 
            this.MP_Category_comboBox.BackColor = System.Drawing.Color.White;
            this.MP_Category_comboBox.ForeColor = System.Drawing.Color.Black;
            this.MP_Category_comboBox.FormattingEnabled = true;
            this.MP_Category_comboBox.Location = new System.Drawing.Point(60, 18);
            this.MP_Category_comboBox.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.MP_Category_comboBox.Name = "MP_Category_comboBox";
            this.MP_Category_comboBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.MP_Category_comboBox.Size = new System.Drawing.Size(151, 35);
            this.MP_Category_comboBox.TabIndex = 462;
            this.MP_Category_comboBox.Leave += new System.EventHandler(this.MP_Category_comboBox_Leave);
            // 
            // Show_button
            // 
            this.Show_button.BackColor = System.Drawing.Color.Transparent;
            this.Show_button.BackgroundImage = global::MyWorkApplication.Properties.Resources.Show2_L;
            this.Show_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Show_button.FlatAppearance.BorderSize = 0;
            this.Show_button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.Show_button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.Show_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Show_button.Location = new System.Drawing.Point(60, 58);
            this.Show_button.Margin = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.Show_button.Name = "Show_button";
            this.Show_button.Size = new System.Drawing.Size(154, 40);
            this.Show_button.TabIndex = 463;
            this.Show_button.UseVisualStyleBackColor = false;
            this.Show_button.Click += new System.EventHandler(this.Show_button_Click);
            // 
            // Timeline_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 27F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.ClientSize = new System.Drawing.Size(1264, 799);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Rubik", 10.8F);
            this.ForeColor = System.Drawing.Color.Black;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Timeline_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.Timeline_Form_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MP_dataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox MicroProject_ID_textBox;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox Person_Name_textBox;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataGridView MP_dataGridView;
        private System.Windows.Forms.ComboBox MP_Category_comboBox;
        private System.Windows.Forms.Button Show_button;
    }
}