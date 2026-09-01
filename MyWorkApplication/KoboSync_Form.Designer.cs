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
            this.components = new System.ComponentModel.Container();
            this.mainLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.title_label = new System.Windows.Forms.Label();
            this.result_tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.ServerSection_label = new System.Windows.Forms.Label();
            this.Status_label = new System.Windows.Forms.Label();
            this.Status_value_label = new System.Windows.Forms.Label();
            this.Existing_label = new System.Windows.Forms.Label();
            this.Existing_value_label = new System.Windows.Forms.Label();
            this.KoboSection_label = new System.Windows.Forms.Label();
            this.Submissions_label = new System.Windows.Forms.Label();
            this.Submissions_value_label = new System.Windows.Forms.Label();
            this.Inserted_label = new System.Windows.Forms.Label();
            this.Inserted_value_label = new System.Windows.Forms.Label();
            this.Updated_label = new System.Windows.Forms.Label();
            this.Updated_value_label = new System.Windows.Forms.Label();
            this.Deleted_label = new System.Windows.Forms.Label();
            this.Deleted_value_label = new System.Windows.Forms.Label();
            this.AfterSyncSection_label = new System.Windows.Forms.Label();
            this.AfterSync_label = new System.Windows.Forms.Label();
            this.AfterSync_value_label = new System.Windows.Forms.Label();
            this.Errors_label = new System.Windows.Forms.Label();
            this.Errors_value_label = new System.Windows.Forms.Label();
            this.bottom_tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.Sync_button = new MyWorkApplication.Controls.NoFocusButton();
            this.progressBorder_panel = new System.Windows.Forms.Panel();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.Sync_backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.Sync_progress_timer = new System.Windows.Forms.Timer(this.components);
            this.mainLayoutPanel.SuspendLayout();
            this.result_tableLayoutPanel.SuspendLayout();
            this.bottom_tableLayoutPanel.SuspendLayout();
            this.progressBorder_panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainLayoutPanel
            // 
            this.mainLayoutPanel.BackColor = System.Drawing.Color.Transparent;
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
            this.mainLayoutPanel.Size = new System.Drawing.Size(780, 527);
            this.mainLayoutPanel.TabIndex = 0;
            this.mainLayoutPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.mainLayoutPanel_Paint);
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
            this.title_label.Text = "مزامنة الزيارات من كوبو";
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
            this.result_tableLayoutPanel.Controls.Add(this.ServerSection_label, 0, 0);
            this.result_tableLayoutPanel.Controls.Add(this.Status_label, 0, 2);
            this.result_tableLayoutPanel.Controls.Add(this.Status_value_label, 1, 2);
            this.result_tableLayoutPanel.Controls.Add(this.Existing_label, 3, 2);
            this.result_tableLayoutPanel.Controls.Add(this.Existing_value_label, 4, 2);
            this.result_tableLayoutPanel.Controls.Add(this.KoboSection_label, 0, 4);
            this.result_tableLayoutPanel.Controls.Add(this.Submissions_label, 0, 6);
            this.result_tableLayoutPanel.Controls.Add(this.Submissions_value_label, 1, 6);
            this.result_tableLayoutPanel.Controls.Add(this.Inserted_label, 3, 6);
            this.result_tableLayoutPanel.Controls.Add(this.Inserted_value_label, 4, 6);
            this.result_tableLayoutPanel.Controls.Add(this.Updated_label, 0, 8);
            this.result_tableLayoutPanel.Controls.Add(this.Updated_value_label, 1, 8);
            this.result_tableLayoutPanel.Controls.Add(this.Deleted_label, 3, 8);
            this.result_tableLayoutPanel.Controls.Add(this.Deleted_value_label, 4, 8);
            this.result_tableLayoutPanel.Controls.Add(this.AfterSyncSection_label, 0, 10);
            this.result_tableLayoutPanel.Controls.Add(this.AfterSync_label, 0, 12);
            this.result_tableLayoutPanel.Controls.Add(this.AfterSync_value_label, 1, 12);
            this.result_tableLayoutPanel.Controls.Add(this.Errors_label, 3, 12);
            this.result_tableLayoutPanel.Controls.Add(this.Errors_value_label, 4, 12);
            this.result_tableLayoutPanel.Location = new System.Drawing.Point(58, 89);
            this.result_tableLayoutPanel.Margin = new System.Windows.Forms.Padding(3, 3, 3, 18);
            this.result_tableLayoutPanel.Name = "result_tableLayoutPanel";
            this.result_tableLayoutPanel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.result_tableLayoutPanel.RowCount = 13;
            this.result_tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.result_tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.result_tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.result_tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.result_tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.result_tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.result_tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.result_tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this.result_tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.result_tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.result_tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.result_tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.result_tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.result_tableLayoutPanel.Size = new System.Drawing.Size(664, 305);
            this.result_tableLayoutPanel.TabIndex = 2;
            // 
            // ServerSection_label
            // 
            this.ServerSection_label.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(54)))), ((int)(((byte)(48)))));
            this.result_tableLayoutPanel.SetColumnSpan(this.ServerSection_label, 5);
            this.ServerSection_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ServerSection_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ServerSection_label.ForeColor = System.Drawing.Color.White;
            this.ServerSection_label.Location = new System.Drawing.Point(3, 0);
            this.ServerSection_label.Name = "ServerSection_label";
            this.ServerSection_label.Padding = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.ServerSection_label.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.ServerSection_label.Size = new System.Drawing.Size(658, 30);
            this.ServerSection_label.TabIndex = 16;
            this.ServerSection_label.Text = "على السيرفر حالياً";
            this.ServerSection_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Status_label
            // 
            this.Status_label.AutoSize = true;
            this.Status_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Status_label.Location = new System.Drawing.Point(531, 40);
            this.Status_label.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.Status_label.Name = "Status_label";
            this.Status_label.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Status_label.Size = new System.Drawing.Size(125, 30);
            this.Status_label.TabIndex = 0;
            this.Status_label.Text = "حالة المزامنة:";
            this.Status_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Status_value_label
            // 
            this.Status_value_label.BackColor = System.Drawing.Color.White;
            this.Status_value_label.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Status_value_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Status_value_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Status_value_label.Location = new System.Drawing.Point(353, 40);
            this.Status_value_label.Name = "Status_value_label";
            this.Status_value_label.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Status_value_label.Size = new System.Drawing.Size(167, 30);
            this.Status_value_label.TabIndex = 1;
            this.Status_value_label.Text = "جاهز";
            this.Status_value_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Existing_label
            // 
            this.Existing_label.AutoSize = true;
            this.Existing_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Existing_label.Location = new System.Drawing.Point(182, 40);
            this.Existing_label.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.Existing_label.Name = "Existing_label";
            this.Existing_label.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Existing_label.Size = new System.Drawing.Size(125, 30);
            this.Existing_label.TabIndex = 19;
            this.Existing_label.Text = "عدد الزيارات الموجودة:";
            this.Existing_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Existing_value_label
            // 
            this.Existing_value_label.BackColor = System.Drawing.Color.White;
            this.Existing_value_label.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Existing_value_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Existing_value_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Existing_value_label.Location = new System.Drawing.Point(3, 40);
            this.Existing_value_label.Name = "Existing_value_label";
            this.Existing_value_label.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Existing_value_label.Size = new System.Drawing.Size(168, 30);
            this.Existing_value_label.TabIndex = 20;
            this.Existing_value_label.Text = "0";
            this.Existing_value_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // KoboSection_label
            // 
            this.KoboSection_label.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(54)))), ((int)(((byte)(48)))));
            this.result_tableLayoutPanel.SetColumnSpan(this.KoboSection_label, 5);
            this.KoboSection_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.KoboSection_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.KoboSection_label.ForeColor = System.Drawing.Color.White;
            this.KoboSection_label.Location = new System.Drawing.Point(3, 100);
            this.KoboSection_label.Name = "KoboSection_label";
            this.KoboSection_label.Padding = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.KoboSection_label.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.KoboSection_label.Size = new System.Drawing.Size(658, 30);
            this.KoboSection_label.TabIndex = 17;
            this.KoboSection_label.Text = "القادمة من كوبو";
            this.KoboSection_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Submissions_label
            // 
            this.Submissions_label.AutoSize = true;
            this.Submissions_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Submissions_label.Location = new System.Drawing.Point(531, 140);
            this.Submissions_label.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.Submissions_label.Name = "Submissions_label";
            this.Submissions_label.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Submissions_label.Size = new System.Drawing.Size(125, 30);
            this.Submissions_label.TabIndex = 4;
            this.Submissions_label.Text = "عدد الزيارات الموجودة:";
            this.Submissions_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Submissions_value_label
            // 
            this.Submissions_value_label.BackColor = System.Drawing.Color.White;
            this.Submissions_value_label.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Submissions_value_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Submissions_value_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Submissions_value_label.Location = new System.Drawing.Point(353, 140);
            this.Submissions_value_label.Name = "Submissions_value_label";
            this.Submissions_value_label.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Submissions_value_label.Size = new System.Drawing.Size(167, 30);
            this.Submissions_value_label.TabIndex = 5;
            this.Submissions_value_label.Text = "0";
            this.Submissions_value_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Inserted_label
            // 
            this.Inserted_label.AutoSize = true;
            this.Inserted_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Inserted_label.Location = new System.Drawing.Point(182, 140);
            this.Inserted_label.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.Inserted_label.Name = "Inserted_label";
            this.Inserted_label.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Inserted_label.Size = new System.Drawing.Size(125, 30);
            this.Inserted_label.TabIndex = 6;
            this.Inserted_label.Text = "عدد الزيارات الجديدة:";
            this.Inserted_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Inserted_value_label
            // 
            this.Inserted_value_label.BackColor = System.Drawing.Color.White;
            this.Inserted_value_label.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Inserted_value_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Inserted_value_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Inserted_value_label.Location = new System.Drawing.Point(3, 140);
            this.Inserted_value_label.Name = "Inserted_value_label";
            this.Inserted_value_label.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Inserted_value_label.Size = new System.Drawing.Size(168, 30);
            this.Inserted_value_label.TabIndex = 7;
            this.Inserted_value_label.Text = "0";
            this.Inserted_value_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Updated_label
            // 
            this.Updated_label.AutoSize = true;
            this.Updated_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Updated_label.Location = new System.Drawing.Point(531, 175);
            this.Updated_label.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.Updated_label.Name = "Updated_label";
            this.Updated_label.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Updated_label.Size = new System.Drawing.Size(125, 30);
            this.Updated_label.TabIndex = 8;
            this.Updated_label.Text = "عدد الزيارات المحدثة:";
            this.Updated_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Updated_value_label
            // 
            this.Updated_value_label.BackColor = System.Drawing.Color.White;
            this.Updated_value_label.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Updated_value_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Updated_value_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Updated_value_label.Location = new System.Drawing.Point(353, 175);
            this.Updated_value_label.Name = "Updated_value_label";
            this.Updated_value_label.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Updated_value_label.Size = new System.Drawing.Size(167, 30);
            this.Updated_value_label.TabIndex = 9;
            this.Updated_value_label.Text = "0";
            this.Updated_value_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Deleted_label
            // 
            this.Deleted_label.AutoSize = true;
            this.Deleted_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Deleted_label.Location = new System.Drawing.Point(182, 175);
            this.Deleted_label.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.Deleted_label.Name = "Deleted_label";
            this.Deleted_label.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Deleted_label.Size = new System.Drawing.Size(125, 30);
            this.Deleted_label.TabIndex = 12;
            this.Deleted_label.Text = "عدد الزيارات المحذوفة:";
            this.Deleted_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Deleted_value_label
            // 
            this.Deleted_value_label.BackColor = System.Drawing.Color.White;
            this.Deleted_value_label.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Deleted_value_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Deleted_value_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Deleted_value_label.Location = new System.Drawing.Point(3, 175);
            this.Deleted_value_label.Name = "Deleted_value_label";
            this.Deleted_value_label.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Deleted_value_label.Size = new System.Drawing.Size(168, 30);
            this.Deleted_value_label.TabIndex = 13;
            this.Deleted_value_label.Text = "0";
            this.Deleted_value_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // AfterSyncSection_label
            // 
            this.AfterSyncSection_label.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(54)))), ((int)(((byte)(48)))));
            this.result_tableLayoutPanel.SetColumnSpan(this.AfterSyncSection_label, 5);
            this.AfterSyncSection_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AfterSyncSection_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AfterSyncSection_label.ForeColor = System.Drawing.Color.White;
            this.AfterSyncSection_label.Location = new System.Drawing.Point(3, 235);
            this.AfterSyncSection_label.Name = "AfterSyncSection_label";
            this.AfterSyncSection_label.Padding = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.AfterSyncSection_label.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.AfterSyncSection_label.Size = new System.Drawing.Size(658, 30);
            this.AfterSyncSection_label.TabIndex = 18;
            this.AfterSyncSection_label.Text = "على السيرفر بعد المزامنة";
            this.AfterSyncSection_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // AfterSync_label
            // 
            this.AfterSync_label.AutoSize = true;
            this.AfterSync_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AfterSync_label.Location = new System.Drawing.Point(531, 275);
            this.AfterSync_label.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.AfterSync_label.Name = "AfterSync_label";
            this.AfterSync_label.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.AfterSync_label.Size = new System.Drawing.Size(125, 30);
            this.AfterSync_label.TabIndex = 21;
            this.AfterSync_label.Text = "عدد الزيارات الكلي:";
            this.AfterSync_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // AfterSync_value_label
            // 
            this.AfterSync_value_label.BackColor = System.Drawing.Color.White;
            this.AfterSync_value_label.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.AfterSync_value_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AfterSync_value_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AfterSync_value_label.Location = new System.Drawing.Point(353, 275);
            this.AfterSync_value_label.Name = "AfterSync_value_label";
            this.AfterSync_value_label.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.AfterSync_value_label.Size = new System.Drawing.Size(167, 30);
            this.AfterSync_value_label.TabIndex = 22;
            this.AfterSync_value_label.Text = "0";
            this.AfterSync_value_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Errors_label
            // 
            this.Errors_label.AutoSize = true;
            this.Errors_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Errors_label.Location = new System.Drawing.Point(182, 275);
            this.Errors_label.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.Errors_label.Name = "Errors_label";
            this.Errors_label.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Errors_label.Size = new System.Drawing.Size(125, 30);
            this.Errors_label.TabIndex = 14;
            this.Errors_label.Text = "عدد الأخطاء:";
            this.Errors_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Errors_value_label
            // 
            this.Errors_value_label.BackColor = System.Drawing.Color.White;
            this.Errors_value_label.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Errors_value_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Errors_value_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Errors_value_label.Location = new System.Drawing.Point(3, 275);
            this.Errors_value_label.Name = "Errors_value_label";
            this.Errors_value_label.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Errors_value_label.Size = new System.Drawing.Size(168, 30);
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
            this.bottom_tableLayoutPanel.Controls.Add(this.progressBorder_panel, 1, 0);
            this.bottom_tableLayoutPanel.Location = new System.Drawing.Point(58, 438);
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
            this.Sync_button.BackColor = System.Drawing.Color.Transparent;
            this.Sync_button.BackgroundImage = global::MyWorkApplication.Properties.Resources.BtnBackground_D;
            this.Sync_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Sync_button.FlatAppearance.BorderSize = 0;
            this.Sync_button.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.Sync_button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.Sync_button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.Sync_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Sync_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Sync_button.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.Sync_button.Location = new System.Drawing.Point(559, 14);
            this.Sync_button.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Sync_button.MaximumSize = new System.Drawing.Size(125, 40);
            this.Sync_button.MinimumSize = new System.Drawing.Size(100, 30);
            this.Sync_button.Name = "Sync_button";
            this.Sync_button.Size = new System.Drawing.Size(102, 30);
            this.Sync_button.TabIndex = 2;
            this.Sync_button.TabStop = false;
            this.Sync_button.Text = " مزامنة";
            this.Sync_button.UseVisualStyleBackColor = false;
            this.Sync_button.Click += new System.EventHandler(this.Sync_button_Click);
            this.Sync_button.MouseEnter += new System.EventHandler(this.Sync_button_MouseEnter);
            this.Sync_button.MouseLeave += new System.EventHandler(this.Sync_button_MouseLeave);
            // 
            // progressBorder_panel
            // 
            this.progressBorder_panel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBorder_panel.BackColor = System.Drawing.Color.White;
            this.progressBorder_panel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.progressBorder_panel.Controls.Add(this.progressBar1);
            this.progressBorder_panel.Location = new System.Drawing.Point(3, 14);
            this.progressBorder_panel.Name = "progressBorder_panel";
            this.progressBorder_panel.Padding = new System.Windows.Forms.Padding(2);
            this.progressBorder_panel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.progressBorder_panel.Size = new System.Drawing.Size(550, 30);
            this.progressBorder_panel.TabIndex = 3;
            // 
            // progressBar1
            // 
            this.progressBar1.BackColor = System.Drawing.Color.White;
            this.progressBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressBar1.Location = new System.Drawing.Point(2, 2);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.progressBar1.Size = new System.Drawing.Size(542, 22);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar1.TabIndex = 1;
            // 
            // Sync_backgroundWorker
            // 
            this.Sync_backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.Sync_backgroundWorker_DoWork);
            this.Sync_backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.Sync_backgroundWorker_RunWorkerCompleted);
            // 
            // Sync_progress_timer
            // 
            this.Sync_progress_timer.Interval = 120;
            this.Sync_progress_timer.Tick += new System.EventHandler(this.Sync_progress_timer_Tick);
            // 
            // KoboSync_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.ClientSize = new System.Drawing.Size(780, 527);
            this.Controls.Add(this.mainLayoutPanel);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(650, 360);
            this.Name = "KoboSync_Form";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.KoboSync_Form_FormClosing);
            this.Load += new System.EventHandler(this.KoboSync_Form_Load);
            this.mainLayoutPanel.ResumeLayout(false);
            this.result_tableLayoutPanel.ResumeLayout(false);
            this.result_tableLayoutPanel.PerformLayout();
            this.bottom_tableLayoutPanel.ResumeLayout(false);
            this.progressBorder_panel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel mainLayoutPanel;
        private System.Windows.Forms.Label title_label;
        private System.Windows.Forms.TableLayoutPanel result_tableLayoutPanel;
        private System.Windows.Forms.Label ServerSection_label;
        private System.Windows.Forms.Label KoboSection_label;
        private System.Windows.Forms.Label AfterSyncSection_label;
        private System.Windows.Forms.Label Status_label;
        private System.Windows.Forms.Label Status_value_label;
        private System.Windows.Forms.Label Existing_label;
        private System.Windows.Forms.Label Existing_value_label;
        private System.Windows.Forms.Label Submissions_label;
        private System.Windows.Forms.Label Submissions_value_label;
        private System.Windows.Forms.Label Inserted_label;
        private System.Windows.Forms.Label Inserted_value_label;
        private System.Windows.Forms.Label Updated_label;
        private System.Windows.Forms.Label Updated_value_label;
        private System.Windows.Forms.Label Deleted_label;
        private System.Windows.Forms.Label Deleted_value_label;
        private System.Windows.Forms.Label AfterSync_label;
        private System.Windows.Forms.Label AfterSync_value_label;
        private System.Windows.Forms.Label Errors_label;
        private System.Windows.Forms.Label Errors_value_label;
        private System.Windows.Forms.TableLayoutPanel bottom_tableLayoutPanel;
        private System.Windows.Forms.Panel progressBorder_panel;
        private System.Windows.Forms.ProgressBar progressBar1;
        private MyWorkApplication.Controls.NoFocusButton Sync_button;
        private System.ComponentModel.BackgroundWorker Sync_backgroundWorker;
        private System.Windows.Forms.Timer Sync_progress_timer;
    }
}
