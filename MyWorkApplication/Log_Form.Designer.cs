namespace MyWorkApplication
{
    partial class Log_Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Log_Form));
            this.DeleteImage_button = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.Refresh_button = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.ExportToExcel_button = new System.Windows.Forms.Button();
            this.Count_label = new System.Windows.Forms.Label();
            this.Selected_label = new System.Windows.Forms.Label();
            this.Log_dataGridView = new Zuby.ADGV.AdvancedDataGridView();
            this.SearchToolBar = new Zuby.ADGV.AdvancedDataGridViewSearchToolBar();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Log_dataGridView)).BeginInit();
            this.SearchToolBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // DeleteImage_button
            // 
            this.DeleteImage_button.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.DeleteImage_button.BackColor = System.Drawing.Color.Transparent;
            this.DeleteImage_button.BackgroundImage = global::MyWorkApplication.Properties.Resources.Trash_D;
            this.DeleteImage_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.DeleteImage_button.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.DeleteImage_button.FlatAppearance.BorderSize = 0;
            this.DeleteImage_button.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.DeleteImage_button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.DeleteImage_button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.DeleteImage_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DeleteImage_button.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(210)))), ((int)(((byte)(210)))));
            this.DeleteImage_button.Location = new System.Drawing.Point(35, 4);
            this.DeleteImage_button.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.DeleteImage_button.MaximumSize = new System.Drawing.Size(29, 32);
            this.DeleteImage_button.MinimumSize = new System.Drawing.Size(29, 32);
            this.DeleteImage_button.Name = "DeleteImage_button";
            this.DeleteImage_button.Size = new System.Drawing.Size(29, 32);
            this.DeleteImage_button.TabIndex = 78;
            this.DeleteImage_button.UseVisualStyleBackColor = false;
            this.DeleteImage_button.Click += new System.EventHandler(this.DeleteLog_button_Click);
            this.DeleteImage_button.MouseEnter += new System.EventHandler(this.DeleteImage_button_MouseEnter);
            this.DeleteImage_button.MouseLeave += new System.EventHandler(this.DeleteImage_button_MouseLeave);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(1170, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(100, 681);
            this.panel1.TabIndex = 421;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(94, 681);
            this.panel2.TabIndex = 422;
            // 
            // Refresh_button
            // 
            this.Refresh_button.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.Refresh_button.BackgroundImage = global::MyWorkApplication.Properties.Resources.Refresh2_D;
            this.Refresh_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Refresh_button.FlatAppearance.BorderSize = 0;
            this.Refresh_button.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.Refresh_button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.Refresh_button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.Refresh_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Refresh_button.Location = new System.Drawing.Point(70, 4);
            this.Refresh_button.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Refresh_button.MaximumSize = new System.Drawing.Size(29, 32);
            this.Refresh_button.MinimumSize = new System.Drawing.Size(29, 32);
            this.Refresh_button.Name = "Refresh_button";
            this.Refresh_button.Size = new System.Drawing.Size(29, 32);
            this.Refresh_button.TabIndex = 412;
            this.Refresh_button.UseVisualStyleBackColor = true;
            this.Refresh_button.Click += new System.EventHandler(this.Refresh_button_Click);
            this.Refresh_button.MouseEnter += new System.EventHandler(this.Refresh_button_MouseEnter);
            this.Refresh_button.MouseLeave += new System.EventHandler(this.Refresh_button_MouseLeave);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.tableLayoutPanel1.ColumnCount = 6;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 44F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15.45802F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.60305F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 67.93893F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 65F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 66F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel1.Controls.Add(this.Refresh_button, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.DeleteImage_button, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.ExportToExcel_button, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.Count_label, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.Selected_label, 2, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(94, 641);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1076, 40);
            this.tableLayoutPanel1.TabIndex = 457;
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
            this.ExportToExcel_button.Location = new System.Drawing.Point(1036, 4);
            this.ExportToExcel_button.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.ExportToExcel_button.MaximumSize = new System.Drawing.Size(29, 32);
            this.ExportToExcel_button.MinimumSize = new System.Drawing.Size(29, 32);
            this.ExportToExcel_button.Name = "ExportToExcel_button";
            this.ExportToExcel_button.Size = new System.Drawing.Size(29, 32);
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
            this.Count_label.Location = new System.Drawing.Point(893, 6);
            this.Count_label.Margin = new System.Windows.Forms.Padding(0);
            this.Count_label.Name = "Count_label";
            this.Count_label.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Count_label.Size = new System.Drawing.Size(139, 27);
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
            this.Selected_label.Location = new System.Drawing.Point(744, 6);
            this.Selected_label.Margin = new System.Windows.Forms.Padding(0);
            this.Selected_label.Name = "Selected_label";
            this.Selected_label.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Selected_label.Size = new System.Drawing.Size(149, 27);
            this.Selected_label.TabIndex = 476;
            this.Selected_label.Text = "Selected";
            this.Selected_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Log_dataGridView
            // 
            this.Log_dataGridView.AllowUserToAddRows = false;
            this.Log_dataGridView.AllowUserToDeleteRows = false;
            this.Log_dataGridView.AllowUserToOrderColumns = true;
            this.Log_dataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.Log_dataGridView.BackgroundColor = System.Drawing.Color.White;
            this.Log_dataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Log_dataGridView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Rubik", 10.8F);
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Log_dataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.Log_dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Log_dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Log_dataGridView.EnableHeadersVisualStyles = false;
            this.Log_dataGridView.FilterAndSortEnabled = true;
            this.Log_dataGridView.GridColor = System.Drawing.Color.Black;
            this.Log_dataGridView.Location = new System.Drawing.Point(94, 31);
            this.Log_dataGridView.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Log_dataGridView.Name = "Log_dataGridView";
            this.Log_dataGridView.ReadOnly = true;
            this.Log_dataGridView.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Log_dataGridView.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Rubik", 10.8F);
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Log_dataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.Log_dataGridView.RowHeadersWidth = 51;
            this.Log_dataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.Log_dataGridView.RowTemplate.Height = 27;
            this.Log_dataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.Log_dataGridView.Size = new System.Drawing.Size(1076, 610);
            this.Log_dataGridView.TabIndex = 568;
            this.Log_dataGridView.SortStringChanged += new System.EventHandler<Zuby.ADGV.AdvancedDataGridView.SortEventArgs>(this.dataGridView_SortStringChanged);
            this.Log_dataGridView.FilterStringChanged += new System.EventHandler<Zuby.ADGV.AdvancedDataGridView.FilterEventArgs>(this.dataGridView_FilterStringChanged);
            this.Log_dataGridView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.Log_dataGridView_DataError);
            this.Log_dataGridView.SelectionChanged += new System.EventHandler(this.Log_dataGridView_SelectionChanged);
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
            this.SearchToolBar.Location = new System.Drawing.Point(94, 0);
            this.SearchToolBar.MaximumSize = new System.Drawing.Size(0, 21);
            this.SearchToolBar.MinimumSize = new System.Drawing.Size(0, 31);
            this.SearchToolBar.Name = "SearchToolBar";
            this.SearchToolBar.Padding = new System.Windows.Forms.Padding(0);
            this.SearchToolBar.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.SearchToolBar.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.SearchToolBar.Size = new System.Drawing.Size(1076, 31);
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
            // Log_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 27F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(1270, 681);
            this.Controls.Add(this.Log_dataGridView);
            this.Controls.Add(this.SearchToolBar);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Font = new System.Drawing.Font("Rubik", 10.8F);
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Log_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Load += new System.EventHandler(this.Log_Form_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Log_dataGridView)).EndInit();
            this.SearchToolBar.ResumeLayout(false);
            this.SearchToolBar.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button DeleteImage_button;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button Refresh_button;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button ExportToExcel_button;
        private System.Windows.Forms.Label Count_label;
        private System.Windows.Forms.Label Selected_label;
        private Zuby.ADGV.AdvancedDataGridView Log_dataGridView;
        private Zuby.ADGV.AdvancedDataGridViewSearchToolBar SearchToolBar;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
    }
}