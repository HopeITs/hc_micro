namespace MyWorkApplication
{
    partial class KoboSync_Form
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.mainLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.title_label = new System.Windows.Forms.Label();
            this.result_tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.Status_label = new System.Windows.Forms.Label();
            this.Status_value_label = new System.Windows.Forms.Label();
            this.Forms_label = new System.Windows.Forms.Label();
            this.Forms_value_label = new System.Windows.Forms.Label();
            this.Submissions_label = new System.Windows.Forms.Label();
            this.Submissions_value_label = new System.Windows.Forms.Label();
            this.Inserted_label = new System.Windows.Forms.Label();
            this.Inserted_value_label = new System.Windows.Forms.Label();
            this.Updated_label = new System.Windows.Forms.Label();
            this.Updated_value_label = new System.Windows.Forms.Label();
            this.Unchanged_label = new System.Windows.Forms.Label();
            this.Unchanged_value_label = new System.Windows.Forms.Label();
            this.Deleted_label = new System.Windows.Forms.Label();
            this.Deleted_value_label = new System.Windows.Forms.Label();
            this.Errors_label = new System.Windows.Forms.Label();
            this.Errors_value_label = new System.Windows.Forms.Label();
            this.bottom_tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.Sync_button = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.Sync_backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.mainLayoutPanel.SuspendLayout();
            this.result_tableLayoutPanel.SuspendLayout();
            this.bottom_tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainLayoutPanel
            // 
            this.mainLayoutPanel.ColumnCount = 1;
            this.mainLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainLayoutPanel.Controls.Add(this.title_label, 0, 0);
            this.mainLayoutPanel.Controls.Add(this.result_tableLayoutPanel, 0, 1);
            this.mainLayoutPanel.Controls.Add(this.bottom_tableLayoutPanel, 0, 2);
            this.mainLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.mainLayoutPanel.Name = "mainLayoutPanel";
            this.mainLayoutPanel.Padding = new System.Windows.Forms.Padding(55, 28, 55, 28);
            this.mainLayoutPanel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.mainLayoutPanel.RowCount = 3;
            this.mainLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 58F));
            this.mainLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            this.mainLayoutPanel.Size = new System.Drawing.Size(780, 400);
            this.mainLayoutPanel.TabIndex = 0;
            // 
            // title_label
            // 
            this.title_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.title_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.title_label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(54)))), ((int)(((byte)(48)))));
            this.title_label.Location = new System.Drawing.Point(58, 28);
            this.title_label.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.title_label.Name = "title_label";
            this.title_label.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.title_label.Size = new System.Drawing.Size(664, 48);
            this.title_label.TabIndex = 0;
            this.title_label.Text = "مزامنة الزيارات  من كوبو";
            this.title_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // result_tableLayoutPanel
            // 
            this.result_tableLayoutPanel.ColumnCount = 5;
            this.result_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.result_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.result_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.result_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.result_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.result_tableLayoutPanel.Controls.Add(this.Status_label, 0, 0);
            this.result_tableLayoutPanel.Controls.Add(this.Status_value_label, 1, 0);
            this.result_tableLayoutPanel.Controls.Add(this.Forms_label, 3, 0);
            this.result_tableLayoutPanel.Controls.Add(this.Forms_value_label, 4, 0);
            this.result_tableLayoutPanel.Controls.Add(this.Submissions_label, 0, 1);
            this.result_tableLayoutPanel.Controls.Add(this.Submissions_value_label, 1, 1);
            this.result_tableLayoutPanel.Controls.Add(this.Inserted_label, 3, 1);
            this.result_tableLayoutPanel.Controls.Add(this.Inserted_value_label, 4, 1);
            this.result_tableLayoutPanel.Controls.Add(this.Updated_label, 0, 2);
            this.result_tableLayoutPanel.Controls.Add(this.Updated_value_label, 1, 2);
            this.result_tableLayoutPanel.Controls.Add(this.Unchanged_label, 3, 2);
            this.result_tableLayoutPanel.Controls.Add(this.Unchanged_value_label, 4, 2);
            this.result_tableLayoutPanel.Controls.Add(this.Deleted_label, 0, 3);
            this.result_tableLayoutPanel.Controls.Add(this.Deleted_value_label, 1, 3);
            this.result_tableLayoutPanel.Controls.Add(this.Errors_label, 3, 3);
            this.result_tableLayoutPanel.Controls.Add(this.Errors_value_label, 4, 3);
            this.result_tableLayoutPanel.Location = new System.Drawing.Point(58, 89);
            this.result_tableLayoutPanel.Margin = new System.Windows.Forms.Padding(3, 3, 3, 18);
            this.result_tableLayoutPanel.Name = "result_tableLayoutPanel";
            this.result_tableLayoutPanel.Padding = new System.Windows.Forms.Padding(0, 8, 0, 8);
            this.result_tableLayoutPanel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.result_tableLayoutPanel.RowCount = 4;
            this.result_tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.result_tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.result_tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.result_tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.result_tableLayoutPanel.Size = new System.Drawing.Size(664, 178);
            this.result_tableLayoutPanel.TabIndex = 2;
            // 
            // Status_label
            // 
            this.Status_label.AutoSize = true;
            this.Status_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Status_label.Location = new System.Drawing.Point(532, 11);
            this.Status_label.Margin = new System.Windows.Forms.Padding(8, 3, 8, 3);
            this.Status_label.Name = "Status_label";
            this.Status_label.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Status_label.Size = new System.Drawing.Size(124, 34);
            this.Status_label.TabIndex = 0;
            this.Status_label.Text = "حالة المزامنة:";
            this.Status_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Status_value_label
            // 
            this.Status_value_label.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Status_value_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Status_value_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Status_value_label.Location = new System.Drawing.Point(358, 11);
            this.Status_value_label.Margin = new System.Windows.Forms.Padding(3);
            this.Status_value_label.Name = "Status_value_label";
            this.Status_value_label.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Status_value_label.Size = new System.Drawing.Size(163, 34);
            this.Status_value_label.TabIndex = 1;
            this.Status_value_label.Text = "جاهز";
            this.Status_value_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Forms_label
            // 
            this.Forms_label.AutoSize = true;
            this.Forms_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Forms_label.Location = new System.Drawing.Point(178, 11);
            this.Forms_label.Margin = new System.Windows.Forms.Padding(8, 3, 8, 3);
            this.Forms_label.Name = "Forms_label";
            this.Forms_label.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Forms_label.Size = new System.Drawing.Size(134, 34);
            this.Forms_label.TabIndex = 2;
            this.Forms_label.Text = "عدد النماذج:";
            this.Forms_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Forms_value_label
            // 
            this.Forms_value_label.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Forms_value_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Forms_value_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Forms_value_label.Location = new System.Drawing.Point(3, 11);
            this.Forms_value_label.Margin = new System.Windows.Forms.Padding(3);
            this.Forms_value_label.Name = "Forms_value_label";
            this.Forms_value_label.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Forms_value_label.Size = new System.Drawing.Size(164, 34);
            this.Forms_value_label.TabIndex = 3;
            this.Forms_value_label.Text = "0";
            this.Forms_value_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Submissions_label
            // 
            this.Submissions_label.AutoSize = true;
            this.Submissions_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Submissions_label.Location = new System.Drawing.Point(532, 51);
            this.Submissions_label.Margin = new System.Windows.Forms.Padding(8, 3, 8, 3);
            this.Submissions_label.Name = "Submissions_label";
            this.Submissions_label.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Submissions_label.Size = new System.Drawing.Size(124, 34);
            this.Submissions_label.TabIndex = 4;
            this.Submissions_label.Text = "عدد السجلات الكلي:";
            this.Submissions_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Submissions_value_label
            // 
            this.Submissions_value_label.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Submissions_value_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Submissions_value_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Submissions_value_label.Location = new System.Drawing.Point(358, 51);
            this.Submissions_value_label.Margin = new System.Windows.Forms.Padding(3);
            this.Submissions_value_label.Name = "Submissions_value_label";
            this.Submissions_value_label.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Submissions_value_label.Size = new System.Drawing.Size(163, 34);
            this.Submissions_value_label.TabIndex = 5;
            this.Submissions_value_label.Text = "0";
            this.Submissions_value_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Inserted_label
            // 
            this.Inserted_label.AutoSize = true;
            this.Inserted_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Inserted_label.Location = new System.Drawing.Point(178, 51);
            this.Inserted_label.Margin = new System.Windows.Forms.Padding(8, 3, 8, 3);
            this.Inserted_label.Name = "Inserted_label";
            this.Inserted_label.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Inserted_label.Size = new System.Drawing.Size(134, 34);
            this.Inserted_label.TabIndex = 6;
            this.Inserted_label.Text = "عدد السجلات المضافة:";
            this.Inserted_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Inserted_value_label
            // 
            this.Inserted_value_label.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Inserted_value_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Inserted_value_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Inserted_value_label.Location = new System.Drawing.Point(3, 51);
            this.Inserted_value_label.Margin = new System.Windows.Forms.Padding(3);
            this.Inserted_value_label.Name = "Inserted_value_label";
            this.Inserted_value_label.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Inserted_value_label.Size = new System.Drawing.Size(164, 34);
            this.Inserted_value_label.TabIndex = 7;
            this.Inserted_value_label.Text = "0";
            this.Inserted_value_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Updated_label
            // 
            this.Updated_label.AutoSize = true;
            this.Updated_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Updated_label.Location = new System.Drawing.Point(532, 91);
            this.Updated_label.Margin = new System.Windows.Forms.Padding(8, 3, 8, 3);
            this.Updated_label.Name = "Updated_label";
            this.Updated_label.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Updated_label.Size = new System.Drawing.Size(124, 34);
            this.Updated_label.TabIndex = 8;
            this.Updated_label.Text = "عدد السجلات المحدثة:";
            this.Updated_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Updated_value_label
            // 
            this.Updated_value_label.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Updated_value_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Updated_value_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Updated_value_label.Location = new System.Drawing.Point(358, 91);
            this.Updated_value_label.Margin = new System.Windows.Forms.Padding(3);
            this.Updated_value_label.Name = "Updated_value_label";
            this.Updated_value_label.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Updated_value_label.Size = new System.Drawing.Size(163, 34);
            this.Updated_value_label.TabIndex = 9;
            this.Updated_value_label.Text = "0";
            this.Updated_value_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Unchanged_label
            // 
            this.Unchanged_label.AutoSize = true;
            this.Unchanged_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Unchanged_label.Location = new System.Drawing.Point(178, 91);
            this.Unchanged_label.Margin = new System.Windows.Forms.Padding(8, 3, 8, 3);
            this.Unchanged_label.Name = "Unchanged_label";
            this.Unchanged_label.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Unchanged_label.Size = new System.Drawing.Size(134, 34);
            this.Unchanged_label.TabIndex = 10;
            this.Unchanged_label.Text = "عدد السجلات بدون تغيير:";
            this.Unchanged_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Unchanged_value_label
            // 
            this.Unchanged_value_label.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Unchanged_value_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Unchanged_value_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Unchanged_value_label.Location = new System.Drawing.Point(3, 91);
            this.Unchanged_value_label.Margin = new System.Windows.Forms.Padding(3);
            this.Unchanged_value_label.Name = "Unchanged_value_label";
            this.Unchanged_value_label.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Unchanged_value_label.Size = new System.Drawing.Size(164, 34);
            this.Unchanged_value_label.TabIndex = 11;
            this.Unchanged_value_label.Text = "0";
            this.Unchanged_value_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Deleted_label
            // 
            this.Deleted_label.AutoSize = true;
            this.Deleted_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Deleted_label.Location = new System.Drawing.Point(532, 131);
            this.Deleted_label.Margin = new System.Windows.Forms.Padding(8, 3, 8, 3);
            this.Deleted_label.Name = "Deleted_label";
            this.Deleted_label.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Deleted_label.Size = new System.Drawing.Size(124, 36);
            this.Deleted_label.TabIndex = 12;
            this.Deleted_label.Text = "عدد السجلات المحذوفة:";
            this.Deleted_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Deleted_value_label
            // 
            this.Deleted_value_label.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Deleted_value_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Deleted_value_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Deleted_value_label.Location = new System.Drawing.Point(358, 131);
            this.Deleted_value_label.Margin = new System.Windows.Forms.Padding(3);
            this.Deleted_value_label.Name = "Deleted_value_label";
            this.Deleted_value_label.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Deleted_value_label.Size = new System.Drawing.Size(163, 36);
            this.Deleted_value_label.TabIndex = 13;
            this.Deleted_value_label.Text = "0";
            this.Deleted_value_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Errors_label
            // 
            this.Errors_label.AutoSize = true;
            this.Errors_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Errors_label.Location = new System.Drawing.Point(178, 131);
            this.Errors_label.Margin = new System.Windows.Forms.Padding(8, 3, 8, 3);
            this.Errors_label.Name = "Errors_label";
            this.Errors_label.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Errors_label.Size = new System.Drawing.Size(134, 36);
            this.Errors_label.TabIndex = 14;
            this.Errors_label.Text = "عدد الأخطاء:";
            this.Errors_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Errors_value_label
            // 
            this.Errors_value_label.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Errors_value_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Errors_value_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Errors_value_label.Location = new System.Drawing.Point(3, 131);
            this.Errors_value_label.Margin = new System.Windows.Forms.Padding(3);
            this.Errors_value_label.Name = "Errors_value_label";
            this.Errors_value_label.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Errors_value_label.Size = new System.Drawing.Size(164, 36);
            this.Errors_value_label.TabIndex = 15;
            this.Errors_value_label.Text = "0";
            this.Errors_value_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // bottom_tableLayoutPanel
            // 
            this.bottom_tableLayoutPanel.ColumnCount = 2;
            this.bottom_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.bottom_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 556F));
            this.bottom_tableLayoutPanel.Controls.Add(this.Sync_button, 0, 0);
            this.bottom_tableLayoutPanel.Controls.Add(this.progressBar1, 1, 0);
            this.bottom_tableLayoutPanel.Location = new System.Drawing.Point(58, 311);
            this.bottom_tableLayoutPanel.Name = "bottom_tableLayoutPanel";
            this.bottom_tableLayoutPanel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.bottom_tableLayoutPanel.RowCount = 1;
            this.bottom_tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.bottom_tableLayoutPanel.Size = new System.Drawing.Size(664, 58);
            this.bottom_tableLayoutPanel.TabIndex = 4;
            // 
            // Sync_button
            // 
            this.Sync_button.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Sync_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Sync_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Sync_button.Location = new System.Drawing.Point(559, 15);
            this.Sync_button.Name = "Sync_button";
            this.Sync_button.Size = new System.Drawing.Size(102, 28);
            this.Sync_button.TabIndex = 2;
            this.Sync_button.Text = "مزامنة الزيارات";
            this.Sync_button.UseVisualStyleBackColor = true;
            this.Sync_button.Click += new System.EventHandler(this.Sync_button_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(3, 18);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.progressBar1.Size = new System.Drawing.Size(550, 21);
            this.progressBar1.TabIndex = 1;
            // 
            // Sync_backgroundWorker
            // 
            this.Sync_backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.Sync_backgroundWorker_DoWork);
            this.Sync_backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.Sync_backgroundWorker_RunWorkerCompleted);
            // 
            // KoboSync_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.ClientSize = new System.Drawing.Size(780, 400);
            this.Controls.Add(this.mainLayoutPanel);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(650, 360);
            this.Name = "KoboSync_Form";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "مزامنة زيارات كوبو";
            this.Load += new System.EventHandler(this.KoboSync_Form_Load);
            this.mainLayoutPanel.ResumeLayout(false);
            this.result_tableLayoutPanel.ResumeLayout(false);
            this.result_tableLayoutPanel.PerformLayout();
            this.bottom_tableLayoutPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel mainLayoutPanel;
        private System.Windows.Forms.Label title_label;
        private System.Windows.Forms.TableLayoutPanel result_tableLayoutPanel;
        private System.Windows.Forms.Label Status_label;
        private System.Windows.Forms.Label Status_value_label;
        private System.Windows.Forms.Label Forms_label;
        private System.Windows.Forms.Label Forms_value_label;
        private System.Windows.Forms.Label Submissions_label;
        private System.Windows.Forms.Label Submissions_value_label;
        private System.Windows.Forms.Label Inserted_label;
        private System.Windows.Forms.Label Inserted_value_label;
        private System.Windows.Forms.Label Updated_label;
        private System.Windows.Forms.Label Updated_value_label;
        private System.Windows.Forms.Label Unchanged_label;
        private System.Windows.Forms.Label Unchanged_value_label;
        private System.Windows.Forms.Label Deleted_label;
        private System.Windows.Forms.Label Deleted_value_label;
        private System.Windows.Forms.Label Errors_label;
        private System.Windows.Forms.Label Errors_value_label;
        private System.Windows.Forms.TableLayoutPanel bottom_tableLayoutPanel;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button Sync_button;
        private System.ComponentModel.BackgroundWorker Sync_backgroundWorker;
    }
}
