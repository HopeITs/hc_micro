namespace MyWorkApplication
{
    partial class Notification_Box
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Notification_Box));
            this.Project_label = new System.Windows.Forms.Label();
            this.Close_button = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.Counter_textBox = new System.Windows.Forms.TextBox();
            this.SeenCounter_textBox = new System.Windows.Forms.TextBox();
            this.check_all_seen_checkBox = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.ExportToExcel_button = new System.Windows.Forms.Button();
            this.Notification_dataGridView = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.goToToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.applicationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checklistToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loansToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.visitsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Notification_dataGridView)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Project_label
            // 
            this.Project_label.BackColor = System.Drawing.Color.Transparent;
            this.Project_label.Dock = System.Windows.Forms.DockStyle.Top;
            this.Project_label.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Project_label.Font = new System.Drawing.Font("Rubik", 18F, System.Drawing.FontStyle.Bold);
            this.Project_label.ForeColor = System.Drawing.Color.Black;
            this.Project_label.Location = new System.Drawing.Point(0, 0);
            this.Project_label.Margin = new System.Windows.Forms.Padding(0);
            this.Project_label.Name = "Project_label";
            this.Project_label.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Project_label.Size = new System.Drawing.Size(610, 52);
            this.Project_label.TabIndex = 453;
            this.Project_label.Text = "Notification";
            this.Project_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Project_label.Click += new System.EventHandler(this.Project_label_Click);
            this.Project_label.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Notification_Box_MouseDown);
            this.Project_label.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Notification_Box_MouseMove);
            this.Project_label.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Notification_Box_MouseUp);
            // 
            // Close_button
            // 
            this.Close_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Close_button.BackColor = System.Drawing.Color.Transparent;
            this.Close_button.BackgroundImage = global::MyWorkApplication.Properties.Resources.Exit_D;
            this.Close_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Close_button.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.Close_button.FlatAppearance.BorderSize = 0;
            this.Close_button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.Close_button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.Close_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Close_button.Location = new System.Drawing.Point(581, 9);
            this.Close_button.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Close_button.MinimumSize = new System.Drawing.Size(28, 31);
            this.Close_button.Name = "Close_button";
            this.Close_button.Size = new System.Drawing.Size(28, 31);
            this.Close_button.TabIndex = 456;
            this.Close_button.UseVisualStyleBackColor = false;
            this.Close_button.Click += new System.EventHandler(this.Close_button_Click);
            this.Close_button.MouseEnter += new System.EventHandler(this.Close_button_MouseEnter);
            this.Close_button.MouseLeave += new System.EventHandler(this.Close_button_MouseLeave);
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.label2.Location = new System.Drawing.Point(396, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 27);
            this.label2.TabIndex = 459;
            this.label2.Text = "All:";
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.label1.Location = new System.Drawing.Point(214, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 27);
            this.label1.TabIndex = 460;
            this.label1.Text = "Seen:";
            // 
            // Counter_textBox
            // 
            this.Counter_textBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Counter_textBox.BackColor = System.Drawing.Color.White;
            this.Counter_textBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Counter_textBox.Enabled = false;
            this.Counter_textBox.ForeColor = System.Drawing.Color.Black;
            this.Counter_textBox.Location = new System.Drawing.Point(439, 10);
            this.Counter_textBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Counter_textBox.MinimumSize = new System.Drawing.Size(110, 18);
            this.Counter_textBox.Name = "Counter_textBox";
            this.Counter_textBox.Size = new System.Drawing.Size(110, 22);
            this.Counter_textBox.TabIndex = 461;
            this.Counter_textBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // SeenCounter_textBox
            // 
            this.SeenCounter_textBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.SeenCounter_textBox.BackColor = System.Drawing.Color.White;
            this.SeenCounter_textBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.SeenCounter_textBox.Enabled = false;
            this.SeenCounter_textBox.ForeColor = System.Drawing.Color.Black;
            this.SeenCounter_textBox.Location = new System.Drawing.Point(279, 10);
            this.SeenCounter_textBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.SeenCounter_textBox.MinimumSize = new System.Drawing.Size(110, 18);
            this.SeenCounter_textBox.Name = "SeenCounter_textBox";
            this.SeenCounter_textBox.Size = new System.Drawing.Size(110, 22);
            this.SeenCounter_textBox.TabIndex = 462;
            this.SeenCounter_textBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // check_all_seen_checkBox
            // 
            this.check_all_seen_checkBox.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.check_all_seen_checkBox.AutoSize = true;
            this.check_all_seen_checkBox.Location = new System.Drawing.Point(55, 6);
            this.check_all_seen_checkBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.check_all_seen_checkBox.Name = "check_all_seen_checkBox";
            this.check_all_seen_checkBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.check_all_seen_checkBox.Size = new System.Drawing.Size(112, 31);
            this.check_all_seen_checkBox.TabIndex = 463;
            this.check_all_seen_checkBox.Text = "Check all seen";
            this.check_all_seen_checkBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.check_all_seen_checkBox.UseVisualStyleBackColor = true;
            this.check_all_seen_checkBox.CheckedChanged += new System.EventHandler(this.Check_all_seen_checkBox_CheckedChanged);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 8;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 52F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 23.4252F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4.133858F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17.09402F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 19.49405F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.05357F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 23.80952F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.Controls.Add(this.Counter_textBox, 6, 0);
            this.tableLayoutPanel1.Controls.Add(this.SeenCounter_textBox, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.check_all_seen_checkBox, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.ExportToExcel_button, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 52);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.375F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(610, 43);
            this.tableLayoutPanel1.TabIndex = 464;
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
            this.ExportToExcel_button.Location = new System.Drawing.Point(19, 5);
            this.ExportToExcel_button.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.ExportToExcel_button.MaximumSize = new System.Drawing.Size(29, 32);
            this.ExportToExcel_button.MinimumSize = new System.Drawing.Size(29, 32);
            this.ExportToExcel_button.Name = "ExportToExcel_button";
            this.ExportToExcel_button.Size = new System.Drawing.Size(29, 32);
            this.ExportToExcel_button.TabIndex = 464;
            this.ExportToExcel_button.UseVisualStyleBackColor = false;
            this.ExportToExcel_button.Click += new System.EventHandler(this.ExportToExcel_button_Click);
            this.ExportToExcel_button.MouseEnter += new System.EventHandler(this.ExportToExcel_button_MouseEnter);
            this.ExportToExcel_button.MouseLeave += new System.EventHandler(this.ExportToExcel_button_MouseLeave);
            // 
            // Notification_dataGridView
            // 
            this.Notification_dataGridView.AllowUserToAddRows = false;
            this.Notification_dataGridView.AllowUserToDeleteRows = false;
            this.Notification_dataGridView.AllowUserToOrderColumns = true;
            this.Notification_dataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.Notification_dataGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            this.Notification_dataGridView.BackgroundColor = System.Drawing.Color.White;
            this.Notification_dataGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Notification_dataGridView.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.Notification_dataGridView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Rubik", 10.8F);
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Notification_dataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.Notification_dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Notification_dataGridView.ColumnHeadersVisible = false;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Rubik", 10.8F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Notification_dataGridView.DefaultCellStyle = dataGridViewCellStyle2;
            this.Notification_dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Notification_dataGridView.EnableHeadersVisualStyles = false;
            this.Notification_dataGridView.GridColor = System.Drawing.Color.Black;
            this.Notification_dataGridView.Location = new System.Drawing.Point(0, 95);
            this.Notification_dataGridView.Margin = new System.Windows.Forms.Padding(4);
            this.Notification_dataGridView.Name = "Notification_dataGridView";
            this.Notification_dataGridView.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Rubik", 10.8F);
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Notification_dataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.Notification_dataGridView.RowHeadersVisible = false;
            this.Notification_dataGridView.RowHeadersWidth = 51;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Notification_dataGridView.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.Notification_dataGridView.RowTemplate.Height = 30;
            this.Notification_dataGridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Notification_dataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.Notification_dataGridView.Size = new System.Drawing.Size(610, 323);
            this.Notification_dataGridView.TabIndex = 455;
            this.Notification_dataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Notification_dataGridView_CellContentClick);
            this.Notification_dataGridView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.Notification_dataGridView_DataError);
            this.Notification_dataGridView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Notification_dataGridView_MouseDown);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.goToToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(115, 28);
            // 
            // goToToolStripMenuItem
            // 
            this.goToToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.applicationToolStripMenuItem,
            this.checklistToolStripMenuItem,
            this.loansToolStripMenuItem,
            this.visitsToolStripMenuItem});
            this.goToToolStripMenuItem.Name = "goToToolStripMenuItem";
            this.goToToolStripMenuItem.Size = new System.Drawing.Size(114, 24);
            this.goToToolStripMenuItem.Text = "Open";
            // 
            // applicationToolStripMenuItem
            // 
            this.applicationToolStripMenuItem.Name = "applicationToolStripMenuItem";
            this.applicationToolStripMenuItem.Size = new System.Drawing.Size(212, 26);
            this.applicationToolStripMenuItem.Text = "Application";
            this.applicationToolStripMenuItem.Click += new System.EventHandler(this.applicationToolStripMenuItem_Click);
            // 
            // checklistToolStripMenuItem
            // 
            this.checklistToolStripMenuItem.Name = "checklistToolStripMenuItem";
            this.checklistToolStripMenuItem.Size = new System.Drawing.Size(212, 26);
            this.checklistToolStripMenuItem.Text = "CheckList";
            this.checklistToolStripMenuItem.Click += new System.EventHandler(this.checklistToolStripMenuItem_Click);
            // 
            // loansToolStripMenuItem
            // 
            this.loansToolStripMenuItem.Name = "loansToolStripMenuItem";
            this.loansToolStripMenuItem.Size = new System.Drawing.Size(212, 26);
            this.loansToolStripMenuItem.Text = "Loans && Payments";
            this.loansToolStripMenuItem.Click += new System.EventHandler(this.loansToolStripMenuItem_Click);
            // 
            // visitsToolStripMenuItem
            // 
            this.visitsToolStripMenuItem.Name = "visitsToolStripMenuItem";
            this.visitsToolStripMenuItem.Size = new System.Drawing.Size(212, 26);
            this.visitsToolStripMenuItem.Text = "Visits";
            this.visitsToolStripMenuItem.Click += new System.EventHandler(this.visitsToolStripMenuItem_Click);
            // 
            // Notification_Box
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 27F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.BackgroundImage = global::MyWorkApplication.Properties.Resources.Back_600;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(610, 418);
            this.ControlBox = false;
            this.Controls.Add(this.Notification_dataGridView);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.Close_button);
            this.Controls.Add(this.Project_label);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Rubik", 10.8F);
            this.ForeColor = System.Drawing.Color.Black;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(7, 5, 7, 5);
            this.Name = "Notification_Box";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Notification_Box_FormClosing);
            this.Load += new System.EventHandler(this.Notification_Box_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Notification_Box_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Notification_Box_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Notification_Box_MouseUp);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Notification_dataGridView)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label Project_label;
        private System.Windows.Forms.Button Close_button;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox Counter_textBox;
        private System.Windows.Forms.TextBox SeenCounter_textBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.CheckBox check_all_seen_checkBox;
        private System.Windows.Forms.DataGridView Notification_dataGridView;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem goToToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem applicationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loansToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checklistToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem visitsToolStripMenuItem;
        private System.Windows.Forms.Button ExportToExcel_button;
    }
}