namespace MyWorkApplication
{
    partial class Encrypt
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Encrypt));
            this.Result = new System.Windows.Forms.RichTextBox();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.Browse = new System.Windows.Forms.Button();
            this.Decrypt_btn = new System.Windows.Forms.Button();
            this.Encrypt_btn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Result
            // 
            this.Result.Location = new System.Drawing.Point(856, 49);
            this.Result.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.Result.Name = "Result";
            this.Result.Size = new System.Drawing.Size(250, 515);
            this.Result.TabIndex = 9;
            this.Result.Text = "";
            // 
            // txtFileName
            // 
            this.txtFileName.Location = new System.Drawing.Point(246, 289);
            this.txtFileName.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(544, 29);
            this.txtFileName.TabIndex = 8;
            // 
            // Browse
            // 
            this.Browse.Location = new System.Drawing.Point(246, 132);
            this.Browse.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.Browse.Name = "Browse";
            this.Browse.Size = new System.Drawing.Size(544, 125);
            this.Browse.TabIndex = 7;
            this.Browse.Text = "Get Properites";
            this.Browse.UseVisualStyleBackColor = true;
            this.Browse.Click += new System.EventHandler(this.Browse_Click);
            // 
            // Decrypt_btn
            // 
            this.Decrypt_btn.Location = new System.Drawing.Point(246, 349);
            this.Decrypt_btn.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.Decrypt_btn.Name = "Decrypt_btn";
            this.Decrypt_btn.Size = new System.Drawing.Size(214, 137);
            this.Decrypt_btn.TabIndex = 6;
            this.Decrypt_btn.Text = "Decrypt";
            this.Decrypt_btn.UseVisualStyleBackColor = true;
            this.Decrypt_btn.Click += new System.EventHandler(this.Decrypt_btn_Click);
            // 
            // Encrypt_btn
            // 
            this.Encrypt_btn.Location = new System.Drawing.Point(570, 348);
            this.Encrypt_btn.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.Encrypt_btn.Name = "Encrypt_btn";
            this.Encrypt_btn.Size = new System.Drawing.Size(220, 138);
            this.Encrypt_btn.TabIndex = 5;
            this.Encrypt_btn.Text = "Encrypt";
            this.Encrypt_btn.UseVisualStyleBackColor = true;
            this.Encrypt_btn.Click += new System.EventHandler(this.Encrypt_btn_Click);
            // 
            // Encrypt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 27F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1161, 628);
            this.Controls.Add(this.Result);
            this.Controls.Add(this.txtFileName);
            this.Controls.Add(this.Browse);
            this.Controls.Add(this.Decrypt_btn);
            this.Controls.Add(this.Encrypt_btn);
            this.Font = new System.Drawing.Font("Rubik", 10.8F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.Name = "Encrypt";
            this.Text = "Encrypt";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox Result;
        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.Button Browse;
        private System.Windows.Forms.Button Decrypt_btn;
        private System.Windows.Forms.Button Encrypt_btn;
    }
}