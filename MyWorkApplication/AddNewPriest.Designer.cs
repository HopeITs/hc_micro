namespace MyWorkApplication
{
    partial class AddNewPriest
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddNewPriest));
            this.Priest_dataGridView = new System.Windows.Forms.DataGridView();
            this.Delete_button = new System.Windows.Forms.Button();
            this.Save_button = new System.Windows.Forms.Button();
            this.PriestName_textBox = new System.Windows.Forms.TextBox();
            this.label48 = new System.Windows.Forms.Label();
            this.Close_button = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.Priest_dataGridView)).BeginInit();
            this.tableLayoutPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // Priest_dataGridView
            // 
            this.Priest_dataGridView.AllowUserToAddRows = false;
            this.Priest_dataGridView.AllowUserToDeleteRows = false;
            this.Priest_dataGridView.AllowUserToOrderColumns = true;
            this.Priest_dataGridView.AllowUserToResizeRows = false;
            this.Priest_dataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Priest_dataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.Priest_dataGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            this.Priest_dataGridView.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Rubik", 10.8F);
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Priest_dataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.Priest_dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Rubik", 10.8F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Priest_dataGridView.DefaultCellStyle = dataGridViewCellStyle2;
            this.Priest_dataGridView.EnableHeadersVisualStyles = false;
            this.Priest_dataGridView.GridColor = System.Drawing.Color.DimGray;
            this.Priest_dataGridView.Location = new System.Drawing.Point(19, 158);
            this.Priest_dataGridView.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Priest_dataGridView.MultiSelect = false;
            this.Priest_dataGridView.Name = "Priest_dataGridView";
            this.Priest_dataGridView.ReadOnly = true;
            this.Priest_dataGridView.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Rubik", 10.8F);
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Priest_dataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.Priest_dataGridView.RowHeadersVisible = false;
            this.Priest_dataGridView.RowHeadersWidth = 25;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Priest_dataGridView.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.Priest_dataGridView.RowTemplate.ReadOnly = true;
            this.Priest_dataGridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Priest_dataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.Priest_dataGridView.Size = new System.Drawing.Size(396, 432);
            this.Priest_dataGridView.StandardTab = true;
            this.Priest_dataGridView.TabIndex = 451;
            this.Priest_dataGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Priest_dataGridView_CellClick);
            this.Priest_dataGridView.RowHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.Priest_dataGridView_RowHeaderMouseClick);
            // 
            // Delete_button
            // 
            this.Delete_button.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.Delete_button.BackColor = System.Drawing.Color.Transparent;
            this.Delete_button.BackgroundImage = global::MyWorkApplication.Properties.Resources.Delete_CD;
            this.Delete_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Delete_button.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.Delete_button.FlatAppearance.BorderSize = 0;
            this.Delete_button.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.Delete_button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.Delete_button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.Delete_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Delete_button.ForeColor = System.Drawing.Color.White;
            this.Delete_button.Location = new System.Drawing.Point(153, 595);
            this.Delete_button.Margin = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.Delete_button.Name = "Delete_button";
            this.Delete_button.Size = new System.Drawing.Size(99, 36);
            this.Delete_button.TabIndex = 10;
            this.Delete_button.Text = " ";
            this.Delete_button.UseVisualStyleBackColor = false;
            this.Delete_button.Click += new System.EventHandler(this.DeletePriest_button_Click);
            this.Delete_button.MouseEnter += new System.EventHandler(this.delete_button_MouseEnter);
            this.Delete_button.MouseLeave += new System.EventHandler(this.delete_button_MouseLeave);
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
            this.Save_button.Location = new System.Drawing.Point(163, 115);
            this.Save_button.Margin = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.Save_button.Name = "Save_button";
            this.Save_button.Size = new System.Drawing.Size(89, 33);
            this.Save_button.TabIndex = 8;
            this.Save_button.UseVisualStyleBackColor = false;
            this.Save_button.Click += new System.EventHandler(this.InsertPriest_button_Click);
            this.Save_button.MouseEnter += new System.EventHandler(this.Add_button_MouseEnter);
            this.Save_button.MouseLeave += new System.EventHandler(this.Add_button_MouseLeave);
            // 
            // PriestName_textBox
            // 
            this.PriestName_textBox.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.PriestName_textBox.BackColor = System.Drawing.Color.White;
            this.PriestName_textBox.ForeColor = System.Drawing.Color.Black;
            this.PriestName_textBox.Location = new System.Drawing.Point(28, 79);
            this.PriestName_textBox.Margin = new System.Windows.Forms.Padding(4, 7, 4, 7);
            this.PriestName_textBox.Name = "PriestName_textBox";
            this.PriestName_textBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.PriestName_textBox.Size = new System.Drawing.Size(320, 29);
            this.PriestName_textBox.TabIndex = 318;
            this.PriestName_textBox.TextChanged += new System.EventHandler(this.PriestName_textBox_TextChanged);
            // 
            // label48
            // 
            this.label48.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label48.AutoSize = true;
            this.label48.BackColor = System.Drawing.Color.Transparent;
            this.label48.ForeColor = System.Drawing.Color.Black;
            this.label48.Location = new System.Drawing.Point(354, 79);
            this.label48.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label48.Name = "label48";
            this.label48.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label48.Size = new System.Drawing.Size(66, 27);
            this.label48.TabIndex = 253;
            this.label48.Text = "الكاهن:";
            this.label48.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Close_button
            // 
            this.Close_button.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.Close_button.BackColor = System.Drawing.Color.Transparent;
            this.Close_button.BackgroundImage = global::MyWorkApplication.Properties.Resources.Exit_D;
            this.Close_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Close_button.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.Close_button.FlatAppearance.BorderSize = 0;
            this.Close_button.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.Close_button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.Close_button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.Close_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Close_button.Font = new System.Drawing.Font("Avenir LT Std 65 Medium", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Close_button.Location = new System.Drawing.Point(399, 12);
            this.Close_button.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.Close_button.Name = "Close_button";
            this.Close_button.Size = new System.Drawing.Size(26, 34);
            this.Close_button.TabIndex = 448;
            this.Close_button.UseVisualStyleBackColor = false;
            this.Close_button.Click += new System.EventHandler(this.Close_button_Click);
            this.Close_button.MouseEnter += new System.EventHandler(this.Close_button_MouseEnter);
            this.Close_button.MouseLeave += new System.EventHandler(this.Close_button_MouseLeave);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Rubik", 14F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(52, 21);
            this.label2.Margin = new System.Windows.Forms.Padding(0);
            this.label2.Name = "label2";
            this.label2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label2.Size = new System.Drawing.Size(164, 37);
            this.label2.TabIndex = 450;
            this.label2.Text = "الكاهن المحلي";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Rubik", 14F);
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(216, 21);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label1.Size = new System.Drawing.Size(152, 37);
            this.label1.TabIndex = 449;
            this.label1.Text = "إضافة / حذف";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel4.ColumnCount = 4;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tableLayoutPanel4.Controls.Add(this.label2, 2, 0);
            this.tableLayoutPanel4.Controls.Add(this.label1, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.Close_button, 0, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(429, 58);
            this.tableLayoutPanel4.TabIndex = 452;
            // 
            // AddNewPriest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 27F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.BackgroundImage = global::MyWorkApplication.Properties.Resources.Background_Border;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(429, 646);
            this.Controls.Add(this.tableLayoutPanel4);
            this.Controls.Add(this.Delete_button);
            this.Controls.Add(this.Save_button);
            this.Controls.Add(this.label48);
            this.Controls.Add(this.PriestName_textBox);
            this.Controls.Add(this.Priest_dataGridView);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Rubik", 10.8F);
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "AddNewPriest";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Load += new System.EventHandler(this.AddNewPriest_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Priest_dataGridView)).EndInit();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.DataGridView Priest_dataGridView;
        private System.Windows.Forms.Button Delete_button;
        private System.Windows.Forms.Button Save_button;
        private System.Windows.Forms.TextBox PriestName_textBox;
        private System.Windows.Forms.Label label48;
        private System.Windows.Forms.Button Close_button;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
    }
}