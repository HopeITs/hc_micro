namespace MyWorkApplication
{
    partial class Chat_Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Chat_Form));
            this.send_Btn = new System.Windows.Forms.Button();
            this.viewMessages_richTextbox = new System.Windows.Forms.RichTextBox();
            this.messageToSend_Textbox = new System.Windows.Forms.TextBox();
            this.onlineClients_Btn = new System.Windows.Forms.Button();
            this.Username_label = new System.Windows.Forms.Label();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.Close_button = new System.Windows.Forms.Button();
            this.tableLayoutPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // send_Btn
            // 
            this.send_Btn.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.send_Btn.BackColor = System.Drawing.Color.White;
            this.send_Btn.BackgroundImage = global::MyWorkApplication.Properties.Resources.Next1;
            this.send_Btn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.send_Btn.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.send_Btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.send_Btn.Location = new System.Drawing.Point(410, 658);
            this.send_Btn.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.send_Btn.Name = "send_Btn";
            this.send_Btn.Size = new System.Drawing.Size(44, 32);
            this.send_Btn.TabIndex = 8;
            this.send_Btn.UseVisualStyleBackColor = false;
            this.send_Btn.Click += new System.EventHandler(this.send_Btn_Click);
            // 
            // viewMessages_richTextbox
            // 
            this.viewMessages_richTextbox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.viewMessages_richTextbox.Location = new System.Drawing.Point(18, 139);
            this.viewMessages_richTextbox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.viewMessages_richTextbox.Name = "viewMessages_richTextbox";
            this.viewMessages_richTextbox.Size = new System.Drawing.Size(424, 492);
            this.viewMessages_richTextbox.TabIndex = 6;
            this.viewMessages_richTextbox.Text = "";
            // 
            // messageToSend_Textbox
            // 
            this.messageToSend_Textbox.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.messageToSend_Textbox.Location = new System.Drawing.Point(6, 658);
            this.messageToSend_Textbox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.messageToSend_Textbox.Name = "messageToSend_Textbox";
            this.messageToSend_Textbox.Size = new System.Drawing.Size(397, 29);
            this.messageToSend_Textbox.TabIndex = 7;
            this.messageToSend_Textbox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.messageToSend_Textbox_KeyDown);
            // 
            // onlineClients_Btn
            // 
            this.onlineClients_Btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.onlineClients_Btn.BackColor = System.Drawing.Color.White;
            this.onlineClients_Btn.BackgroundImage = global::MyWorkApplication.Properties.Resources.User;
            this.onlineClients_Btn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.onlineClients_Btn.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.onlineClients_Btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.onlineClients_Btn.Location = new System.Drawing.Point(398, 92);
            this.onlineClients_Btn.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.onlineClients_Btn.Name = "onlineClients_Btn";
            this.onlineClients_Btn.Size = new System.Drawing.Size(44, 36);
            this.onlineClients_Btn.TabIndex = 9;
            this.onlineClients_Btn.UseVisualStyleBackColor = false;
            this.onlineClients_Btn.Click += new System.EventHandler(this.onlineClients_Btn_Click);
            // 
            // Username_label
            // 
            this.Username_label.AutoSize = true;
            this.Username_label.Font = new System.Drawing.Font("Rubik", 12F, System.Drawing.FontStyle.Bold);
            this.Username_label.Location = new System.Drawing.Point(15, 96);
            this.Username_label.Name = "Username_label";
            this.Username_label.Size = new System.Drawing.Size(123, 30);
            this.Username_label.TabIndex = 10;
            this.Username_label.Text = "username :";
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel4.ColumnCount = 3;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 37F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 37F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanel4.Controls.Add(this.button1, 2, 0);
            this.tableLayoutPanel4.Controls.Add(this.label2, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.Close_button, 0, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(471, 56);
            this.tableLayoutPanel4.TabIndex = 464;
            // 
            // button1
            // 
            this.button1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.button1.BackColor = System.Drawing.Color.Transparent;
            this.button1.BackgroundImage = global::MyWorkApplication.Properties.Resources.Exit_L;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button1.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.button1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(16, 19);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(18, 18);
            this.button1.TabIndex = 451;
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Rubik", 14F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(37, 19);
            this.label2.Margin = new System.Windows.Forms.Padding(0);
            this.label2.Name = "label2";
            this.label2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label2.Size = new System.Drawing.Size(397, 37);
            this.label2.TabIndex = 450;
            this.label2.Text = "Micro Group Chat";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Close_button
            // 
            this.Close_button.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.Close_button.BackColor = System.Drawing.Color.Transparent;
            this.Close_button.BackgroundImage = global::MyWorkApplication.Properties.Resources.Exit_D;
            this.Close_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Close_button.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.Close_button.FlatAppearance.BorderSize = 0;
            this.Close_button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.Close_button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.Close_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Close_button.Location = new System.Drawing.Point(437, 19);
            this.Close_button.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Close_button.Name = "Close_button";
            this.Close_button.Size = new System.Drawing.Size(18, 18);
            this.Close_button.TabIndex = 448;
            this.Close_button.UseVisualStyleBackColor = false;
            this.Close_button.Click += new System.EventHandler(this.Close_button_Click);
            // 
            // Chat_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 27F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.ClientSize = new System.Drawing.Size(471, 722);
            this.Controls.Add(this.tableLayoutPanel4);
            this.Controls.Add(this.Username_label);
            this.Controls.Add(this.onlineClients_Btn);
            this.Controls.Add(this.send_Btn);
            this.Controls.Add(this.viewMessages_richTextbox);
            this.Controls.Add(this.messageToSend_Textbox);
            this.Font = new System.Drawing.Font("Rubik", 10.8F);
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Chat_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Chat_Form_FormClosing);
            this.Load += new System.EventHandler(this.Chat_Form_Load);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button send_Btn;
        private System.Windows.Forms.RichTextBox viewMessages_richTextbox;
        private System.Windows.Forms.TextBox messageToSend_Textbox;
        private System.Windows.Forms.Button onlineClients_Btn;
        private System.Windows.Forms.Label Username_label;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button Close_button;
        private System.Windows.Forms.Button button1;
    }
}