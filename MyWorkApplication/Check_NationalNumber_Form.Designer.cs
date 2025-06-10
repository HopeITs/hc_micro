namespace MyWorkApplication
{
    partial class Check_NationalNumber_Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Check_NationalNumber_Form));
            this.Header_label = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.NationalNumber_textBox = new System.Windows.Forms.TextBox();
            this.Check_Button = new System.Windows.Forms.Button();
            this.Result_label = new System.Windows.Forms.Label();
            this.Result_button = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Header_label
            // 
            this.Header_label.BackColor = System.Drawing.Color.Transparent;
            this.Header_label.Dock = System.Windows.Forms.DockStyle.Top;
            this.Header_label.Font = new System.Drawing.Font("Rubik", 18F, System.Drawing.FontStyle.Bold);
            this.Header_label.ForeColor = System.Drawing.Color.Black;
            this.Header_label.Location = new System.Drawing.Point(0, 0);
            this.Header_label.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.Header_label.Name = "Header_label";
            this.Header_label.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Header_label.Size = new System.Drawing.Size(461, 43);
            this.Header_label.TabIndex = 463;
            this.Header_label.Text = "Check ID Number";
            this.Header_label.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4.67587F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 71.67487F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 18.71921F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4.67587F));
            this.tableLayoutPanel1.Controls.Add(this.NationalNumber_textBox, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.Check_Button, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.Result_label, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.Result_button, 3, 3);
            this.tableLayoutPanel1.Controls.Add(this.label1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 2, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 43);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 13.55014F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 13.55013F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 13.55013F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 46.30137F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 13.42466F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(461, 362);
            this.tableLayoutPanel1.TabIndex = 471;
            // 
            // NationalNumber_textBox
            // 
            this.NationalNumber_textBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.NationalNumber_textBox.BackColor = System.Drawing.Color.White;
            this.NationalNumber_textBox.Location = new System.Drawing.Point(22, 57);
            this.NationalNumber_textBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.NationalNumber_textBox.Name = "NationalNumber_textBox";
            this.NationalNumber_textBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.NationalNumber_textBox.Size = new System.Drawing.Size(285, 29);
            this.NationalNumber_textBox.TabIndex = 1;
            this.NationalNumber_textBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.NationalNumber_textBox_KeyDown);
            // 
            // Check_Button
            // 
            this.Check_Button.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Check_Button.BackColor = System.Drawing.Color.Transparent;
            this.Check_Button.BackgroundImage = global::MyWorkApplication.Properties.Resources.BtnBackground_D;
            this.Check_Button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.tableLayoutPanel1.SetColumnSpan(this.Check_Button, 2);
            this.Check_Button.FlatAppearance.BorderSize = 0;
            this.Check_Button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.Check_Button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.Check_Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Check_Button.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.Check_Button.Location = new System.Drawing.Point(22, 98);
            this.Check_Button.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Check_Button.MinimumSize = new System.Drawing.Size(110, 43);
            this.Check_Button.Name = "Check_Button";
            this.Check_Button.Size = new System.Drawing.Size(110, 43);
            this.Check_Button.TabIndex = 10;
            this.Check_Button.Text = "Check";
            this.Check_Button.UseVisualStyleBackColor = false;
            this.Check_Button.Click += new System.EventHandler(this.Check_Button_Click);
            this.Check_Button.MouseEnter += new System.EventHandler(this.Check_Button_MouseEnter);
            this.Check_Button.MouseLeave += new System.EventHandler(this.Check_Button_MouseLeave);
            // 
            // Result_label
            // 
            this.Result_label.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Result_label.AutoSize = true;
            this.Result_label.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.SetColumnSpan(this.Result_label, 2);
            this.Result_label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.Result_label.Location = new System.Drawing.Point(22, 146);
            this.Result_label.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Result_label.Name = "Result_label";
            this.Result_label.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Result_label.Size = new System.Drawing.Size(361, 162);
            this.Result_label.TabIndex = 473;
            // 
            // Result_button
            // 
            this.Result_button.BackColor = System.Drawing.Color.Transparent;
            this.Result_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Result_button.Enabled = false;
            this.Result_button.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.Result_button.FlatAppearance.BorderSize = 0;
            this.Result_button.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.Result_button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.Result_button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.Result_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Result_button.ForeColor = System.Drawing.Color.White;
            this.Result_button.Location = new System.Drawing.Point(389, 145);
            this.Result_button.Margin = new System.Windows.Forms.Padding(3, 1, 8, 1);
            this.Result_button.MaximumSize = new System.Drawing.Size(33, 23);
            this.Result_button.MinimumSize = new System.Drawing.Size(33, 23);
            this.Result_button.Name = "Result_button";
            this.Result_button.Size = new System.Drawing.Size(33, 23);
            this.Result_button.TabIndex = 518;
            this.Result_button.UseVisualStyleBackColor = false;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.SetColumnSpan(this.label1, 3);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.label1.Location = new System.Drawing.Point(22, 10);
            this.label1.Name = "label1";
            this.label1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label1.Size = new System.Drawing.Size(416, 27);
            this.label1.TabIndex = 519;
            this.label1.Text = "الرجاء التأكد من وجود انترنت قبل تنفيذ عملية البحث.";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.SetColumnSpan(this.label2, 2);
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.label2.Location = new System.Drawing.Point(313, 58);
            this.label2.Name = "label2";
            this.label2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label2.Size = new System.Drawing.Size(125, 27);
            this.label2.TabIndex = 460;
            this.label2.Text = "الرقم الوطني:";
            // 
            // Check_NationalNumber_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 27F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.BackgroundImage = global::MyWorkApplication.Properties.Resources.Back_700;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(461, 405);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.Header_label);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Rubik", 10.8F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MinimumSize = new System.Drawing.Size(477, 442);
            this.Name = "Check_NationalNumber_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.Check_NationalNumber_Form_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label Header_label;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button Check_Button;
        private System.Windows.Forms.TextBox NationalNumber_textBox;
        private System.Windows.Forms.Label Result_label;
        private System.Windows.Forms.Button Result_button;
        private System.Windows.Forms.Label label1;
    }
}