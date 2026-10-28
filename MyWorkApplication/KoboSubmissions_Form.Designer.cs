namespace MyWorkApplication
{
    partial class KoboSubmissions_Form
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.mainLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.title_label = new System.Windows.Forms.Label();
            this.filterPanel = new System.Windows.Forms.TableLayoutPanel();
            this.search_label = new System.Windows.Forms.Label();
            this.search_textBox = new System.Windows.Forms.TextBox();
            this.status_label = new System.Windows.Forms.Label();
            this.status_comboBox = new System.Windows.Forms.ComboBox();
            this.Refresh_button = new System.Windows.Forms.Button();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.submissionPanel = new System.Windows.Forms.TableLayoutPanel();
            this.submissionsSection_label = new System.Windows.Forms.Label();
            this.submissions_dataGridView = new System.Windows.Forms.DataGridView();
            this.answersPanel = new System.Windows.Forms.TableLayoutPanel();
            this.answersHeaderPanel = new System.Windows.Forms.TableLayoutPanel();
            this.answersSection_label = new System.Windows.Forms.Label();
            this.Details_label = new System.Windows.Forms.Label();
            this.AnswersCount_label = new System.Windows.Forms.Label();
            this.answers_dataGridView = new System.Windows.Forms.DataGridView();
            this.mainLayoutPanel.SuspendLayout();
            this.filterPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.submissionPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.submissions_dataGridView)).BeginInit();
            this.answersPanel.SuspendLayout();
            this.answersHeaderPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.answers_dataGridView)).BeginInit();
            this.SuspendLayout();
            //
            // mainLayoutPanel
            //
            this.mainLayoutPanel.ColumnCount = 1;
            this.mainLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainLayoutPanel.Controls.Add(this.title_label, 0, 0);
            this.mainLayoutPanel.Controls.Add(this.filterPanel, 0, 1);
            this.mainLayoutPanel.Controls.Add(this.splitContainer, 0, 2);
            this.mainLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainLayoutPanel.Padding = new System.Windows.Forms.Padding(16, 10, 16, 12);
            this.mainLayoutPanel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.mainLayoutPanel.RowCount = 3;
            this.mainLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.mainLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.mainLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainLayoutPanel.Size = new System.Drawing.Size(1280, 790);
            this.mainLayoutPanel.TabIndex = 0;
            //
            // title_label
            //
            this.title_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.title_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 17F, System.Drawing.FontStyle.Bold);
            this.title_label.ForeColor = System.Drawing.Color.FromArgb(165, 54, 48);
            this.title_label.Text = "استعراض إجابات كوبو";
            this.title_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.title_label.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            //
            // filterPanel
            //
            this.filterPanel.ColumnCount = 5;
            this.filterPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 96F));
            this.filterPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.filterPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 88F));
            this.filterPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 174F));
            this.filterPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 110F));
            this.filterPanel.Controls.Add(this.search_label, 0, 0);
            this.filterPanel.Controls.Add(this.search_textBox, 1, 0);
            this.filterPanel.Controls.Add(this.status_label, 2, 0);
            this.filterPanel.Controls.Add(this.status_comboBox, 3, 0);
            this.filterPanel.Controls.Add(this.Refresh_button, 4, 0);
            this.filterPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.filterPanel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.filterPanel.RowCount = 1;
            this.filterPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.filterPanel.TabIndex = 1;
            //
            // search_label
            //
            this.search_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.search_label.Text = "بحث";
            this.search_label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.search_label.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            //
            // search_textBox
            //
            this.search_textBox.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.search_textBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.search_textBox.Size = new System.Drawing.Size(760, 24);
            this.search_textBox.TabIndex = 0;
            //
            // status_label
            //
            this.status_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.status_label.Text = "الحالة";
            this.status_label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.status_label.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            //
            // status_comboBox
            //
            this.status_comboBox.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.status_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.status_comboBox.Items.AddRange(new object[] { "الكل", "تحتاج مراجعة", "مستوردة", "غير مرتبطة", "محذوفة" });
            this.status_comboBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.status_comboBox.Size = new System.Drawing.Size(164, 24);
            this.status_comboBox.TabIndex = 1;
            //
            // Refresh_button
            //
            this.Refresh_button.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Refresh_button.Text = "تحديث";
            this.Refresh_button.Size = new System.Drawing.Size(98, 32);
            this.Refresh_button.TabIndex = 2;
            this.Refresh_button.UseVisualStyleBackColor = true;
            this.Refresh_button.Click += new System.EventHandler(this.Refresh_button_Click);
            //
            // splitContainer
            //
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.splitContainer.Panel1.Controls.Add(this.submissionPanel);
            this.splitContainer.Panel2.Controls.Add(this.answersPanel);
            this.splitContainer.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.splitContainer.SplitterDistance = 300;
            this.splitContainer.TabIndex = 2;
            //
            // submissionPanel
            //
            this.submissionPanel.ColumnCount = 1;
            this.submissionPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.submissionPanel.Controls.Add(this.submissionsSection_label, 0, 0);
            this.submissionPanel.Controls.Add(this.submissions_dataGridView, 0, 1);
            this.submissionPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.submissionPanel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.submissionPanel.RowCount = 2;
            this.submissionPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.submissionPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.submissionPanel.TabIndex = 0;
            //
            // submissionsSection_label
            //
            this.submissionsSection_label.BackColor = System.Drawing.Color.FromArgb(165, 54, 48);
            this.submissionsSection_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.submissionsSection_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.submissionsSection_label.ForeColor = System.Drawing.Color.White;
            this.submissionsSection_label.Padding = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.submissionsSection_label.Text = "الإرسالات";
            this.submissionsSection_label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.submissionsSection_label.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            //
            // submissions_dataGridView
            //
            this.submissions_dataGridView.AllowUserToAddRows = false;
            this.submissions_dataGridView.AllowUserToDeleteRows = false;
            this.submissions_dataGridView.AllowUserToResizeRows = false;
            this.submissions_dataGridView.AutoGenerateColumns = true;
            this.submissions_dataGridView.BackgroundColor = System.Drawing.Color.White;
            this.submissions_dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.submissions_dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.submissions_dataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.submissions_dataGridView.MultiSelect = false;
            this.submissions_dataGridView.ReadOnly = true;
            this.submissions_dataGridView.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.submissions_dataGridView.RowHeadersVisible = false;
            this.submissions_dataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.submissions_dataGridView.TabIndex = 0;
            this.submissions_dataGridView.SelectionChanged += new System.EventHandler(this.Submissions_dataGridView_SelectionChanged);
            //
            // answersPanel
            //
            this.answersPanel.ColumnCount = 1;
            this.answersPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.answersPanel.Controls.Add(this.answersHeaderPanel, 0, 0);
            this.answersPanel.Controls.Add(this.answers_dataGridView, 0, 1);
            this.answersPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.answersPanel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.answersPanel.RowCount = 2;
            this.answersPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.answersPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.answersPanel.TabIndex = 0;
            //
            // answersHeaderPanel
            //
            this.answersHeaderPanel.ColumnCount = 3;
            this.answersHeaderPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.answersHeaderPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.answersHeaderPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 74F));
            this.answersHeaderPanel.Controls.Add(this.answersSection_label, 0, 0);
            this.answersHeaderPanel.Controls.Add(this.Details_label, 1, 0);
            this.answersHeaderPanel.Controls.Add(this.AnswersCount_label, 2, 0);
            this.answersHeaderPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.answersHeaderPanel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.answersHeaderPanel.RowCount = 1;
            this.answersHeaderPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            //
            // answersSection_label
            //
            this.answersSection_label.BackColor = System.Drawing.Color.FromArgb(165, 54, 48);
            this.answersSection_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.answersSection_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.answersSection_label.ForeColor = System.Drawing.Color.White;
            this.answersSection_label.Padding = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.answersSection_label.Text = "الأسئلة والإجابات";
            this.answersSection_label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.answersSection_label.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            //
            // Details_label
            //
            this.Details_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Details_label.AutoEllipsis = true;
            this.Details_label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Details_label.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            //
            // AnswersCount_label
            //
            this.AnswersCount_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AnswersCount_label.Text = "0";
            this.AnswersCount_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.AnswersCount_label.RightToLeft = System.Windows.Forms.RightToLeft.No;
            //
            // answers_dataGridView
            //
            this.answers_dataGridView.AllowUserToAddRows = false;
            this.answers_dataGridView.AllowUserToDeleteRows = false;
            this.answers_dataGridView.AllowUserToResizeRows = false;
            this.answers_dataGridView.AutoGenerateColumns = true;
            this.answers_dataGridView.BackgroundColor = System.Drawing.Color.White;
            this.answers_dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.answers_dataGridView.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.answers_dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.answers_dataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.answers_dataGridView.MultiSelect = false;
            this.answers_dataGridView.ReadOnly = true;
            this.answers_dataGridView.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.answers_dataGridView.RowHeadersVisible = false;
            this.answers_dataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.answers_dataGridView.TabIndex = 1;
            //
            // KoboSubmissions_Form
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1280, 790);
            this.Controls.Add(this.mainLayoutPanel);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.MinimumSize = new System.Drawing.Size(850, 550);
            this.Name = "KoboSubmissions_Form";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "استعراض إجابات كوبو";
            this.Load += new System.EventHandler(this.KoboSubmissions_Form_Load);
            this.mainLayoutPanel.ResumeLayout(false);
            this.filterPanel.ResumeLayout(false);
            this.filterPanel.PerformLayout();
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.submissionPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.submissions_dataGridView)).EndInit();
            this.answersPanel.ResumeLayout(false);
            this.answersHeaderPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.answers_dataGridView)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.TableLayoutPanel mainLayoutPanel;
        private System.Windows.Forms.Label title_label;
        private System.Windows.Forms.TableLayoutPanel filterPanel;
        private System.Windows.Forms.Label search_label;
        private System.Windows.Forms.TextBox search_textBox;
        private System.Windows.Forms.Label status_label;
        private System.Windows.Forms.ComboBox status_comboBox;
        private System.Windows.Forms.Button Refresh_button;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.TableLayoutPanel submissionPanel;
        private System.Windows.Forms.Label submissionsSection_label;
        private System.Windows.Forms.DataGridView submissions_dataGridView;
        private System.Windows.Forms.TableLayoutPanel answersPanel;
        private System.Windows.Forms.TableLayoutPanel answersHeaderPanel;
        private System.Windows.Forms.Label answersSection_label;
        private System.Windows.Forms.Label Details_label;
        private System.Windows.Forms.Label AnswersCount_label;
        private System.Windows.Forms.DataGridView answers_dataGridView;
    }
}
