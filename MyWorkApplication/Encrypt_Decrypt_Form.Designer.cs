namespace MyWorkApplication
{
    partial class Encrypt_Decrypt_Form
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
            this.ClearText_textBox = new System.Windows.Forms.TextBox();
            this.Name_lable = new System.Windows.Forms.Label();
            this.CypherText_textBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.encrypt_button = new System.Windows.Forms.Button();
            this.decrypt_button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ClearText_textBox
            // 
            this.ClearText_textBox.BackColor = System.Drawing.Color.White;
            this.ClearText_textBox.Font = new System.Drawing.Font("Janna LT", 10F);
            this.ClearText_textBox.ForeColor = System.Drawing.Color.Black;
            this.ClearText_textBox.Location = new System.Drawing.Point(22, 76);
            this.ClearText_textBox.Margin = new System.Windows.Forms.Padding(0);
            this.ClearText_textBox.Multiline = true;
            this.ClearText_textBox.Name = "ClearText_textBox";
            this.ClearText_textBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.ClearText_textBox.Size = new System.Drawing.Size(214, 126);
            this.ClearText_textBox.TabIndex = 481;
            // 
            // Name_lable
            // 
            this.Name_lable.AutoSize = true;
            this.Name_lable.BackColor = System.Drawing.Color.Transparent;
            this.Name_lable.Font = new System.Drawing.Font("Janna LT", 10.5F);
            this.Name_lable.ForeColor = System.Drawing.Color.Black;
            this.Name_lable.Location = new System.Drawing.Point(22, 36);
            this.Name_lable.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Name_lable.Name = "Name_lable";
            this.Name_lable.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Name_lable.Size = new System.Drawing.Size(71, 26);
            this.Name_lable.TabIndex = 482;
            this.Name_lable.Text = "clear text";
            this.Name_lable.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CypherText_textBox
            // 
            this.CypherText_textBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CypherText_textBox.BackColor = System.Drawing.Color.White;
            this.CypherText_textBox.Font = new System.Drawing.Font("Janna LT", 10F);
            this.CypherText_textBox.ForeColor = System.Drawing.Color.Black;
            this.CypherText_textBox.Location = new System.Drawing.Point(290, 76);
            this.CypherText_textBox.Margin = new System.Windows.Forms.Padding(0);
            this.CypherText_textBox.Multiline = true;
            this.CypherText_textBox.Name = "CypherText_textBox";
            this.CypherText_textBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.CypherText_textBox.Size = new System.Drawing.Size(214, 126);
            this.CypherText_textBox.TabIndex = 483;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Janna LT", 10.5F);
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(290, 36);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label1.Size = new System.Drawing.Size(106, 26);
            this.label1.TabIndex = 484;
            this.label1.Text = "encrypted text";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // encrypt_button
            // 
            this.encrypt_button.BackColor = System.Drawing.Color.DimGray;
            this.encrypt_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.encrypt_button.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.encrypt_button.FlatAppearance.BorderSize = 0;
            this.encrypt_button.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.encrypt_button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.encrypt_button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.encrypt_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.encrypt_button.ForeColor = System.Drawing.Color.White;
            this.encrypt_button.Location = new System.Drawing.Point(145, 217);
            this.encrypt_button.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.encrypt_button.Name = "encrypt_button";
            this.encrypt_button.Size = new System.Drawing.Size(90, 30);
            this.encrypt_button.TabIndex = 485;
            this.encrypt_button.Text = "Encrypt";
            this.encrypt_button.UseVisualStyleBackColor = false;
            this.encrypt_button.Click += new System.EventHandler(this.encrypt_button_Click);
            // 
            // decrypt_button
            // 
            this.decrypt_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.decrypt_button.BackColor = System.Drawing.Color.DimGray;
            this.decrypt_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.decrypt_button.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.decrypt_button.FlatAppearance.BorderSize = 0;
            this.decrypt_button.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.decrypt_button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.decrypt_button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.decrypt_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.decrypt_button.ForeColor = System.Drawing.Color.White;
            this.decrypt_button.Location = new System.Drawing.Point(413, 217);
            this.decrypt_button.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.decrypt_button.Name = "decrypt_button";
            this.decrypt_button.Size = new System.Drawing.Size(90, 30);
            this.decrypt_button.TabIndex = 486;
            this.decrypt_button.Text = "Decrypt";
            this.decrypt_button.UseVisualStyleBackColor = false;
            this.decrypt_button.Click += new System.EventHandler(this.decrypt_button_Click);
            // 
            // Encrypt_Decrypt_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(531, 329);
            this.Controls.Add(this.decrypt_button);
            this.Controls.Add(this.encrypt_button);
            this.Controls.Add(this.CypherText_textBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ClearText_textBox);
            this.Controls.Add(this.Name_lable);
            this.Name = "Encrypt_Decrypt_Form";
            this.Text = "Encrypt_Decrypt_Form";
            this.Load += new System.EventHandler(this.Encrypt_Decrypt_Form_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox ClearText_textBox;
        private System.Windows.Forms.Label Name_lable;
        private System.Windows.Forms.TextBox CypherText_textBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button encrypt_button;
        private System.Windows.Forms.Button decrypt_button;
    }
}