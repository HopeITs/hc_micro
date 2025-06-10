namespace MyWorkApplication
{
    partial class Application_MessageBox
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Application_MessageBox));
            this.Name_label = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.Old_button = new System.Windows.Forms.Button();
            this.New_button = new System.Windows.Forms.Button();
            this.Back_button = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Name_label
            // 
            this.Name_label.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.Name_label.AutoSize = true;
            this.Name_label.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.SetColumnSpan(this.Name_label, 2);
            this.Name_label.ForeColor = System.Drawing.Color.Black;
            this.Name_label.Location = new System.Drawing.Point(257, 24);
            this.Name_label.Margin = new System.Windows.Forms.Padding(0);
            this.Name_label.Name = "Name_label";
            this.Name_label.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Name_label.Size = new System.Drawing.Size(220, 27);
            this.Name_label.TabIndex = 449;
            this.Name_label.Text = "هذا المستفيد موجود مسبقاً";
            this.Name_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.SetColumnSpan(this.label2, 2);
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(49, 59);
            this.label2.Margin = new System.Windows.Forms.Padding(0);
            this.label2.Name = "label2";
            this.label2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label2.Size = new System.Drawing.Size(428, 44);
            this.label2.TabIndex = 449;
            this.label2.Text = "هل تريد إظهار استمارة المستفيد أو إدخال استمارة جديدة له؟";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.BackgroundImage = global::MyWorkApplication.Properties.Resources.info;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox1.Location = new System.Drawing.Point(480, 23);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(50, 29);
            this.pictureBox1.TabIndex = 450;
            this.pictureBox1.TabStop = false;
            // 
            // Old_button
            // 
            this.Old_button.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.Old_button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(210)))), ((int)(((byte)(210)))));
            this.Old_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Old_button.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.Old_button.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.Old_button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.Old_button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.Old_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Old_button.ForeColor = System.Drawing.Color.Black;
            this.Old_button.Location = new System.Drawing.Point(389, 140);
            this.Old_button.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.Old_button.MinimumSize = new System.Drawing.Size(157, 43);
            this.Old_button.Name = "Old_button";
            this.Old_button.Size = new System.Drawing.Size(157, 43);
            this.Old_button.TabIndex = 460;
            this.Old_button.Text = "إظهار القديمة";
            this.Old_button.UseVisualStyleBackColor = false;
            this.Old_button.Click += new System.EventHandler(this.Old_button_Click);
            // 
            // New_button
            // 
            this.New_button.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.New_button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(210)))), ((int)(((byte)(210)))));
            this.New_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.New_button.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.New_button.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.New_button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.New_button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.New_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.New_button.ForeColor = System.Drawing.Color.Black;
            this.New_button.Location = new System.Drawing.Point(204, 140);
            this.New_button.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.New_button.MinimumSize = new System.Drawing.Size(157, 43);
            this.New_button.Name = "New_button";
            this.New_button.Size = new System.Drawing.Size(157, 43);
            this.New_button.TabIndex = 460;
            this.New_button.Text = "إدخال جديدة";
            this.New_button.UseVisualStyleBackColor = false;
            this.New_button.Click += new System.EventHandler(this.New_button_Click);
            // 
            // Back_button
            // 
            this.Back_button.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.Back_button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(210)))), ((int)(((byte)(210)))));
            this.Back_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Back_button.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.Back_button.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.Back_button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.Back_button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.Back_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Back_button.ForeColor = System.Drawing.Color.Black;
            this.Back_button.Location = new System.Drawing.Point(18, 140);
            this.Back_button.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.Back_button.MinimumSize = new System.Drawing.Size(157, 43);
            this.Back_button.Name = "Back_button";
            this.Back_button.Size = new System.Drawing.Size(157, 43);
            this.Back_button.TabIndex = 460;
            this.Back_button.Text = "تراجع";
            this.Back_button.UseVisualStyleBackColor = false;
            this.Back_button.Click += new System.EventHandler(this.Back_button_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 26.41649F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 61.35247F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.23104F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tableLayoutPanel1.Controls.Add(this.pictureBox1, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.label2, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.Name_label, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 17F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.00001F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(572, 103);
            this.tableLayoutPanel1.TabIndex = 461;
            // 
            // Application_MessageBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 27F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.ClientSize = new System.Drawing.Size(572, 201);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.Back_button);
            this.Controls.Add(this.New_button);
            this.Controls.Add(this.Old_button);
            this.Font = new System.Drawing.Font("Rubik", 10.8F);
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.MinimumSize = new System.Drawing.Size(588, 238);
            this.Name = "Application_MessageBox";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.Application_MessageBox_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label Name_label;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button Old_button;
        private System.Windows.Forms.Button New_button;
        private System.Windows.Forms.Button Back_button;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}