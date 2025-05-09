namespace MyWorkApplication
{
    partial class NewIdea_Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewIdea_Form));
            this.top_label = new System.Windows.Forms.Label();
            this.Subject_TextBox = new System.Windows.Forms.TextBox();
            this.Send_Button = new System.Windows.Forms.Button();
            this.NewIdea_TextBox = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // top_label
            // 
            this.top_label.BackColor = System.Drawing.Color.Transparent;
            this.top_label.Dock = System.Windows.Forms.DockStyle.Top;
            this.top_label.Font = new System.Drawing.Font("Rubik", 14F, System.Drawing.FontStyle.Bold);
            this.top_label.ForeColor = System.Drawing.Color.Black;
            this.top_label.Location = new System.Drawing.Point(0, 0);
            this.top_label.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.top_label.Name = "top_label";
            this.top_label.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.top_label.Size = new System.Drawing.Size(496, 42);
            this.top_label.TabIndex = 458;
            this.top_label.Text = "Send your feedback, or Got New Idea ?";
            this.top_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Subject_TextBox
            // 
            this.Subject_TextBox.BackColor = System.Drawing.Color.White;
            this.Subject_TextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Subject_TextBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.Subject_TextBox.Location = new System.Drawing.Point(0, 42);
            this.Subject_TextBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Subject_TextBox.Name = "Subject_TextBox";
            this.Subject_TextBox.ReadOnly = true;
            this.Subject_TextBox.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.Subject_TextBox.Size = new System.Drawing.Size(496, 29);
            this.Subject_TextBox.TabIndex = 2;
            // 
            // Send_Button
            // 
            this.Send_Button.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Send_Button.BackColor = System.Drawing.Color.Transparent;
            this.Send_Button.BackgroundImage = global::MyWorkApplication.Properties.Resources.BtnBackground_D;
            this.Send_Button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Send_Button.FlatAppearance.BorderSize = 0;
            this.Send_Button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.Send_Button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.Send_Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Send_Button.Font = new System.Drawing.Font("Rubik", 12F, System.Drawing.FontStyle.Bold);
            this.Send_Button.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.Send_Button.Location = new System.Drawing.Point(341, 4);
            this.Send_Button.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Send_Button.Name = "Send_Button";
            this.Send_Button.Size = new System.Drawing.Size(107, 39);
            this.Send_Button.TabIndex = 3;
            this.Send_Button.Text = "SEND";
            this.Send_Button.UseVisualStyleBackColor = false;
            this.Send_Button.Click += new System.EventHandler(this.Send_Button_Click);
            this.Send_Button.MouseEnter += new System.EventHandler(this.Send_Button_MouseEnter);
            this.Send_Button.MouseLeave += new System.EventHandler(this.Send_Button_MouseLeave);
            // 
            // NewIdea_TextBox
            // 
            this.NewIdea_TextBox.BackColor = System.Drawing.Color.White;
            this.NewIdea_TextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.NewIdea_TextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.NewIdea_TextBox.Location = new System.Drawing.Point(0, 71);
            this.NewIdea_TextBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.NewIdea_TextBox.Multiline = true;
            this.NewIdea_TextBox.Name = "NewIdea_TextBox";
            this.NewIdea_TextBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.NewIdea_TextBox.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.NewIdea_TextBox.Size = new System.Drawing.Size(496, 226);
            this.NewIdea_TextBox.TabIndex = 1;
            this.NewIdea_TextBox.Enter += new System.EventHandler(this.NewIdea_TextBox_Enter);
            this.NewIdea_TextBox.Leave += new System.EventHandler(this.NewIdea_TextBox_Leave);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 789F));
            this.tableLayoutPanel1.Controls.Add(this.Send_Button, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 297);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(496, 48);
            this.tableLayoutPanel1.TabIndex = 459;
            // 
            // NewIdea_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 27F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.BackgroundImage = global::MyWorkApplication.Properties.Resources.Background_Border;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(496, 345);
            this.Controls.Add(this.NewIdea_TextBox);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.Subject_TextBox);
            this.Controls.Add(this.top_label);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Rubik", 10.8F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "NewIdea_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.NewIdea_Form_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label top_label;
        private System.Windows.Forms.TextBox Subject_TextBox;
        private System.Windows.Forms.Button Send_Button;
        private System.Windows.Forms.TextBox NewIdea_TextBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}