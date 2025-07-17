namespace MyWorkApplication
{
    partial class Loans_Form
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Loans_Form));
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.SavePayments_button = new System.Windows.Forms.Button();
            this.Payment_dataGridView = new System.Windows.Forms.DataGridView();
            this.Pay_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Pay_Loan_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Pay_Amount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Pay_DueDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Pay_RecievedOnDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Pay_Note = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Pay_IsPaid = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.P_DeleteRow = new System.Windows.Forms.DataGridViewButtonColumn();
            this.P_InDataBase = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.LoanAmount_textBox = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.MicroProject_ID_textBox = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.Person_Name_textBox = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.tableLayoutPanel21 = new System.Windows.Forms.TableLayoutPanel();
            this.NewGroup_button = new System.Windows.Forms.Button();
            this.Group_comboBox = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.PaymentAmount_textBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.dolarAmount_textBox = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.Note_textBox = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.Rate_textBox = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.ReturnedAmount1_textBox = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ReturnedAmount2_label = new System.Windows.Forms.Label();
            this.PaidAmount_label = new System.Windows.Forms.Label();
            this.RemainOfReturned_label = new System.Windows.Forms.Label();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.PaymentsCount_label = new System.Windows.Forms.Label();
            this.PaidPaymentsCount_label = new System.Windows.Forms.Label();
            this.RemainOfPaymentsCount_label = new System.Windows.Forms.Label();
            this.LoanDate_DateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.AddLoan_button = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.ReceiptNo_textBox = new System.Windows.Forms.TextBox();
            this.DeleteLoan_button = new System.Windows.Forms.Button();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.attachments_toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addNotesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Payment_dataGridView)).BeginInit();
            this.tableLayoutPanel21.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.tableLayoutPanel4);
            this.panel1.Controls.Add(this.tableLayoutPanel3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1262, 676);
            this.panel1.TabIndex = 0;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel4.ColumnCount = 7;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10.91902F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 26.6606F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.44403F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 132F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 23.18023F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15.01365F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.64786F));
            this.tableLayoutPanel4.Controls.Add(this.SavePayments_button, 3, 16);
            this.tableLayoutPanel4.Controls.Add(this.Payment_dataGridView, 1, 15);
            this.tableLayoutPanel4.Controls.Add(this.label1, 5, 1);
            this.tableLayoutPanel4.Controls.Add(this.LoanAmount_textBox, 4, 1);
            this.tableLayoutPanel4.Controls.Add(this.label21, 5, 0);
            this.tableLayoutPanel4.Controls.Add(this.MicroProject_ID_textBox, 4, 0);
            this.tableLayoutPanel4.Controls.Add(this.label20, 2, 0);
            this.tableLayoutPanel4.Controls.Add(this.Person_Name_textBox, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.label14, 1, 9);
            this.tableLayoutPanel4.Controls.Add(this.label17, 5, 5);
            this.tableLayoutPanel4.Controls.Add(this.label8, 5, 4);
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel21, 4, 4);
            this.tableLayoutPanel4.Controls.Add(this.label5, 5, 3);
            this.tableLayoutPanel4.Controls.Add(this.PaymentAmount_textBox, 4, 3);
            this.tableLayoutPanel4.Controls.Add(this.label7, 5, 6);
            this.tableLayoutPanel4.Controls.Add(this.dolarAmount_textBox, 1, 1);
            this.tableLayoutPanel4.Controls.Add(this.label16, 2, 1);
            this.tableLayoutPanel4.Controls.Add(this.label6, 4, 9);
            this.tableLayoutPanel4.Controls.Add(this.Note_textBox, 1, 3);
            this.tableLayoutPanel4.Controls.Add(this.label15, 2, 3);
            this.tableLayoutPanel4.Controls.Add(this.label4, 2, 2);
            this.tableLayoutPanel4.Controls.Add(this.Rate_textBox, 1, 2);
            this.tableLayoutPanel4.Controls.Add(this.label18, 5, 2);
            this.tableLayoutPanel4.Controls.Add(this.ReturnedAmount1_textBox, 4, 2);
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel2, 4, 10);
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel5, 1, 10);
            this.tableLayoutPanel4.Controls.Add(this.LoanDate_DateTimePicker, 4, 5);
            this.tableLayoutPanel4.Controls.Add(this.AddLoan_button, 3, 7);
            this.tableLayoutPanel4.Controls.Add(this.label3, 1, 14);
            this.tableLayoutPanel4.Controls.Add(this.ReceiptNo_textBox, 4, 6);
            this.tableLayoutPanel4.Controls.Add(this.DeleteLoan_button, 2, 7);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel4.ForeColor = System.Drawing.Color.Black;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 17;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.969471F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.969473F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.969473F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.969473F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.969473F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.969473F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.969473F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.328695F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 1.666199F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.839199F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.839199F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.839198F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.839198F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.83535F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.81873F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 36.04766F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.554121F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(1241, 1236);
            this.tableLayoutPanel4.TabIndex = 1;
            // 
            // SavePayments_button
            // 
            this.SavePayments_button.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.SavePayments_button.BackColor = System.Drawing.Color.Transparent;
            this.SavePayments_button.BackgroundImage = global::MyWorkApplication.Properties.Resources.Save2_D;
            this.SavePayments_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.SavePayments_button.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.SavePayments_button.FlatAppearance.BorderSize = 0;
            this.SavePayments_button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.SavePayments_button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.SavePayments_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SavePayments_button.ForeColor = System.Drawing.Color.Black;
            this.SavePayments_button.Location = new System.Drawing.Point(547, 1176);
            this.SavePayments_button.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.SavePayments_button.Name = "SavePayments_button";
            this.SavePayments_button.Size = new System.Drawing.Size(126, 58);
            this.SavePayments_button.TabIndex = 13;
            this.toolTip1.SetToolTip(this.SavePayments_button, "حفظ الدفعات");
            this.SavePayments_button.UseVisualStyleBackColor = false;
            this.SavePayments_button.Click += new System.EventHandler(this.SavePayments_button_Click);
            this.SavePayments_button.MouseEnter += new System.EventHandler(this.SavePayments_button_MouseEnter);
            this.SavePayments_button.MouseLeave += new System.EventHandler(this.SavePayments_button_MouseLeave);
            // 
            // Payment_dataGridView
            // 
            this.Payment_dataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.Payment_dataGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            this.Payment_dataGridView.BackgroundColor = System.Drawing.Color.White;
            this.Payment_dataGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Payment_dataGridView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Rubik", 10.8F);
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Payment_dataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.Payment_dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Payment_dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Pay_ID,
            this.Pay_Loan_ID,
            this.Pay_Amount,
            this.Pay_DueDate,
            this.Pay_RecievedOnDate,
            this.Pay_Note,
            this.Pay_IsPaid,
            this.P_DeleteRow,
            this.P_InDataBase});
            this.tableLayoutPanel4.SetColumnSpan(this.Payment_dataGridView, 5);
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Rubik", 10.8F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Payment_dataGridView.DefaultCellStyle = dataGridViewCellStyle2;
            this.Payment_dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Payment_dataGridView.EnableHeadersVisualStyles = false;
            this.Payment_dataGridView.GridColor = System.Drawing.Color.DarkGray;
            this.Payment_dataGridView.Location = new System.Drawing.Point(124, 713);
            this.Payment_dataGridView.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Payment_dataGridView.Name = "Payment_dataGridView";
            this.Payment_dataGridView.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Payment_dataGridView.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Rubik", 10.8F);
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Payment_dataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.Payment_dataGridView.RowHeadersVisible = false;
            this.Payment_dataGridView.RowHeadersWidth = 51;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Payment_dataGridView.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.Payment_dataGridView.RowTemplate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Payment_dataGridView.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Payment_dataGridView.RowTemplate.Height = 26;
            this.Payment_dataGridView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.Payment_dataGridView.Size = new System.Drawing.Size(972, 431);
            this.Payment_dataGridView.TabIndex = 20;
            this.Payment_dataGridView.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.Payment_dataGridView_CellBeginEdit);
            this.Payment_dataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Payment_dataGridView_CellContentClick);
            this.Payment_dataGridView.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.Payment_dataGridView_CellEndEdit);
            this.Payment_dataGridView.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.Payment_dataGridView_CellPainting);
            this.Payment_dataGridView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.Payment_dataGridView_DataError);
            this.Payment_dataGridView.Scroll += new System.Windows.Forms.ScrollEventHandler(this.Payment_dataGridView_Scroll);
            this.Payment_dataGridView.Enter += new System.EventHandler(this.Payment_dataGridView_Enter);
            // 
            // Pay_ID
            // 
            this.Pay_ID.FillWeight = 30F;
            this.Pay_ID.HeaderText = "Pay_ID";
            this.Pay_ID.MinimumWidth = 10;
            this.Pay_ID.Name = "Pay_ID";
            this.Pay_ID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Pay_ID.Visible = false;
            // 
            // Pay_Loan_ID
            // 
            this.Pay_Loan_ID.HeaderText = "Pay_Loan_ID";
            this.Pay_Loan_ID.MinimumWidth = 6;
            this.Pay_Loan_ID.Name = "Pay_Loan_ID";
            this.Pay_Loan_ID.Visible = false;
            // 
            // Pay_Amount
            // 
            this.Pay_Amount.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Pay_Amount.FillWeight = 10F;
            this.Pay_Amount.HeaderText = "القسط";
            this.Pay_Amount.MinimumWidth = 6;
            this.Pay_Amount.Name = "Pay_Amount";
            // 
            // Pay_DueDate
            // 
            this.Pay_DueDate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Pay_DueDate.FillWeight = 10F;
            this.Pay_DueDate.HeaderText = "تاريخ الدفع";
            this.Pay_DueDate.MinimumWidth = 6;
            this.Pay_DueDate.Name = "Pay_DueDate";
            // 
            // Pay_RecievedOnDate
            // 
            this.Pay_RecievedOnDate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Pay_RecievedOnDate.FillWeight = 10F;
            this.Pay_RecievedOnDate.HeaderText = "تاريخ الدفع الفعلي";
            this.Pay_RecievedOnDate.MinimumWidth = 6;
            this.Pay_RecievedOnDate.Name = "Pay_RecievedOnDate";
            // 
            // Pay_Note
            // 
            this.Pay_Note.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Pay_Note.FillWeight = 15F;
            this.Pay_Note.HeaderText = "ملاحظات";
            this.Pay_Note.MinimumWidth = 6;
            this.Pay_Note.Name = "Pay_Note";
            // 
            // Pay_IsPaid
            // 
            this.Pay_IsPaid.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Pay_IsPaid.FillWeight = 5F;
            this.Pay_IsPaid.HeaderText = "مدفوع";
            this.Pay_IsPaid.MinimumWidth = 6;
            this.Pay_IsPaid.Name = "Pay_IsPaid";
            // 
            // P_DeleteRow
            // 
            this.P_DeleteRow.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.P_DeleteRow.FillWeight = 26.9583F;
            this.P_DeleteRow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.P_DeleteRow.HeaderText = " ";
            this.P_DeleteRow.MinimumWidth = 50;
            this.P_DeleteRow.Name = "P_DeleteRow";
            this.P_DeleteRow.Width = 50;
            // 
            // P_InDataBase
            // 
            this.P_InDataBase.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.P_InDataBase.FillWeight = 1F;
            this.P_InDataBase.HeaderText = "P_InDataBase";
            this.P_InDataBase.MinimumWidth = 50;
            this.P_InDataBase.Name = "P_InDataBase";
            this.P_InDataBase.Visible = false;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.label1.Location = new System.Drawing.Point(936, 58);
            this.label1.Name = "label1";
            this.label1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label1.Size = new System.Drawing.Size(104, 27);
            this.label1.TabIndex = 403;
            this.label1.Text = "مبلغ القرض:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LoanAmount_textBox
            // 
            this.LoanAmount_textBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.LoanAmount_textBox.ForeColor = System.Drawing.Color.Black;
            this.LoanAmount_textBox.Location = new System.Drawing.Point(679, 57);
            this.LoanAmount_textBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.LoanAmount_textBox.Name = "LoanAmount_textBox";
            this.LoanAmount_textBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.LoanAmount_textBox.Size = new System.Drawing.Size(251, 29);
            this.LoanAmount_textBox.TabIndex = 2;
            this.LoanAmount_textBox.TextChanged += new System.EventHandler(this.Loan_Amount_textBox_TextChanged);
            this.LoanAmount_textBox.Leave += new System.EventHandler(this.PaymentAmount_textBox_Leave);
            // 
            // label21
            // 
            this.label21.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label21.AutoSize = true;
            this.label21.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.label21.Location = new System.Drawing.Point(936, 10);
            this.label21.Name = "label21";
            this.label21.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label21.Size = new System.Drawing.Size(110, 27);
            this.label21.TabIndex = 0;
            this.label21.Text = "رقم المشروع:";
            this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MicroProject_ID_textBox
            // 
            this.MicroProject_ID_textBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.MicroProject_ID_textBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.MicroProject_ID_textBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.MicroProject_ID_textBox.BackColor = System.Drawing.Color.White;
            this.MicroProject_ID_textBox.ForeColor = System.Drawing.Color.Black;
            this.MicroProject_ID_textBox.Location = new System.Drawing.Point(679, 9);
            this.MicroProject_ID_textBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MicroProject_ID_textBox.Name = "MicroProject_ID_textBox";
            this.MicroProject_ID_textBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.MicroProject_ID_textBox.Size = new System.Drawing.Size(251, 29);
            this.MicroProject_ID_textBox.TabIndex = 1;
            this.MicroProject_ID_textBox.Leave += new System.EventHandler(this.MicroProject_ID_textBox2_Leave);
            // 
            // label20
            // 
            this.label20.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label20.AutoSize = true;
            this.label20.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.label20.Location = new System.Drawing.Point(420, 0);
            this.label20.Name = "label20";
            this.label20.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label20.Size = new System.Drawing.Size(87, 48);
            this.label20.TabIndex = 15;
            this.label20.Text = "اسم المستفيد:";
            this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Person_Name_textBox
            // 
            this.Person_Name_textBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.Person_Name_textBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.Person_Name_textBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.Person_Name_textBox.BackColor = System.Drawing.Color.White;
            this.Person_Name_textBox.ForeColor = System.Drawing.Color.Black;
            this.Person_Name_textBox.Location = new System.Drawing.Point(124, 9);
            this.Person_Name_textBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Person_Name_textBox.Name = "Person_Name_textBox";
            this.Person_Name_textBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Person_Name_textBox.Size = new System.Drawing.Size(290, 29);
            this.Person_Name_textBox.TabIndex = 1;
            this.Person_Name_textBox.TabStop = false;
            this.Person_Name_textBox.Leave += new System.EventHandler(this.Person_Name_textBox2_Leave);
            // 
            // label14
            // 
            this.label14.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label14.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label14.Font = new System.Drawing.Font("Rubik", 12F, System.Drawing.FontStyle.Bold);
            this.label14.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.label14.Location = new System.Drawing.Point(124, 433);
            this.label14.Name = "label14";
            this.label14.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label14.Size = new System.Drawing.Size(290, 46);
            this.label14.TabIndex = 263;
            this.label14.Text = "عدد الدفعات:";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label17
            // 
            this.label17.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label17.AutoSize = true;
            this.label17.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.label17.Location = new System.Drawing.Point(936, 250);
            this.label17.Name = "label17";
            this.label17.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label17.Size = new System.Drawing.Size(154, 27);
            this.label17.TabIndex = 276;
            this.label17.Text = "تاريخ تسليم القرض:";
            // 
            // label8
            // 
            this.label8.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.label8.Location = new System.Drawing.Point(936, 202);
            this.label8.Name = "label8";
            this.label8.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label8.Size = new System.Drawing.Size(127, 27);
            this.label8.TabIndex = 277;
            this.label8.Text = "اسم المجموعة:";
            // 
            // tableLayoutPanel21
            // 
            this.tableLayoutPanel21.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel21.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel21.ColumnCount = 2;
            this.tableLayoutPanel21.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 43F));
            this.tableLayoutPanel21.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel21.Controls.Add(this.NewGroup_button, 0, 0);
            this.tableLayoutPanel21.Controls.Add(this.Group_comboBox, 1, 0);
            this.tableLayoutPanel21.Location = new System.Drawing.Point(679, 193);
            this.tableLayoutPanel21.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.tableLayoutPanel21.Name = "tableLayoutPanel21";
            this.tableLayoutPanel21.RowCount = 1;
            this.tableLayoutPanel21.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel21.Size = new System.Drawing.Size(251, 46);
            this.tableLayoutPanel21.TabIndex = 5;
            this.tableLayoutPanel21.TabStop = true;
            // 
            // NewGroup_button
            // 
            this.NewGroup_button.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.NewGroup_button.BackColor = System.Drawing.Color.Transparent;
            this.NewGroup_button.BackgroundImage = global::MyWorkApplication.Properties.Resources.Plus_Sq_D;
            this.NewGroup_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.NewGroup_button.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.NewGroup_button.FlatAppearance.BorderSize = 0;
            this.NewGroup_button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.NewGroup_button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.NewGroup_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.NewGroup_button.ForeColor = System.Drawing.Color.White;
            this.NewGroup_button.Location = new System.Drawing.Point(10, 8);
            this.NewGroup_button.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.NewGroup_button.MaximumSize = new System.Drawing.Size(30, 30);
            this.NewGroup_button.MinimumSize = new System.Drawing.Size(30, 30);
            this.NewGroup_button.Name = "NewGroup_button";
            this.NewGroup_button.Size = new System.Drawing.Size(30, 30);
            this.NewGroup_button.TabIndex = 7;
            this.toolTip1.SetToolTip(this.NewGroup_button, "التحكم بالمجموعات");
            this.NewGroup_button.UseVisualStyleBackColor = false;
            this.NewGroup_button.Click += new System.EventHandler(this.AddGroup_button_Click);
            // 
            // Group_comboBox
            // 
            this.Group_comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.Group_comboBox.BackColor = System.Drawing.Color.White;
            this.Group_comboBox.FormattingEnabled = true;
            this.Group_comboBox.Location = new System.Drawing.Point(46, 11);
            this.Group_comboBox.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.Group_comboBox.Name = "Group_comboBox";
            this.Group_comboBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Group_comboBox.Size = new System.Drawing.Size(202, 35);
            this.Group_comboBox.TabIndex = 6;
            this.Group_comboBox.SelectedIndexChanged += new System.EventHandler(this.Group_comboBox_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.label5.Location = new System.Drawing.Point(936, 154);
            this.label5.Name = "label5";
            this.label5.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label5.Size = new System.Drawing.Size(67, 27);
            this.label5.TabIndex = 465;
            this.label5.Text = "القسط:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // PaymentAmount_textBox
            // 
            this.PaymentAmount_textBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.PaymentAmount_textBox.ForeColor = System.Drawing.Color.Black;
            this.PaymentAmount_textBox.Location = new System.Drawing.Point(679, 153);
            this.PaymentAmount_textBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.PaymentAmount_textBox.Name = "PaymentAmount_textBox";
            this.PaymentAmount_textBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.PaymentAmount_textBox.Size = new System.Drawing.Size(251, 29);
            this.PaymentAmount_textBox.TabIndex = 4;
            this.PaymentAmount_textBox.TextChanged += new System.EventHandler(this.Payment_Amount_TextChanged);
            this.PaymentAmount_textBox.Leave += new System.EventHandler(this.PaymentAmount_textBox_Leave);
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.label7.Location = new System.Drawing.Point(936, 298);
            this.label7.Name = "label7";
            this.label7.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label7.Size = new System.Drawing.Size(102, 27);
            this.label7.TabIndex = 410;
            this.label7.Text = "رقم الإيصال:";
            // 
            // dolarAmount_textBox
            // 
            this.dolarAmount_textBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.dolarAmount_textBox.BackColor = System.Drawing.Color.White;
            this.dolarAmount_textBox.ForeColor = System.Drawing.Color.Black;
            this.dolarAmount_textBox.Location = new System.Drawing.Point(124, 57);
            this.dolarAmount_textBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dolarAmount_textBox.Name = "dolarAmount_textBox";
            this.dolarAmount_textBox.ReadOnly = true;
            this.dolarAmount_textBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.dolarAmount_textBox.Size = new System.Drawing.Size(290, 29);
            this.dolarAmount_textBox.TabIndex = 475;
            this.dolarAmount_textBox.TabStop = false;
            // 
            // label16
            // 
            this.label16.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label16.AutoSize = true;
            this.label16.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.label16.Location = new System.Drawing.Point(420, 48);
            this.label16.Name = "label16";
            this.label16.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label16.Size = new System.Drawing.Size(99, 48);
            this.label16.TabIndex = 476;
            this.label16.Text = "مبلغ القرض ($):";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label6.Font = new System.Drawing.Font("Rubik", 12F, System.Drawing.FontStyle.Bold);
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.label6.Location = new System.Drawing.Point(679, 433);
            this.label6.Name = "label6";
            this.label6.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label6.Size = new System.Drawing.Size(251, 46);
            this.label6.TabIndex = 410;
            this.label6.Text = "المبلغ المستحق:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Note_textBox
            // 
            this.Note_textBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Note_textBox.BackColor = System.Drawing.Color.White;
            this.Note_textBox.ForeColor = System.Drawing.Color.Black;
            this.Note_textBox.Location = new System.Drawing.Point(124, 146);
            this.Note_textBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Note_textBox.Multiline = true;
            this.Note_textBox.Name = "Note_textBox";
            this.Note_textBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tableLayoutPanel4.SetRowSpan(this.Note_textBox, 4);
            this.Note_textBox.Size = new System.Drawing.Size(290, 188);
            this.Note_textBox.TabIndex = 10;
            // 
            // label15
            // 
            this.label15.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label15.AutoSize = true;
            this.label15.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.label15.Location = new System.Drawing.Point(420, 154);
            this.label15.Name = "label15";
            this.label15.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label15.Size = new System.Drawing.Size(87, 27);
            this.label15.TabIndex = 466;
            this.label15.Text = "ملاحظات:";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.label4.Location = new System.Drawing.Point(420, 106);
            this.label4.Name = "label4";
            this.label4.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label4.Size = new System.Drawing.Size(95, 27);
            this.label4.TabIndex = 464;
            this.label4.Text = "النسبة (%):";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Rate_textBox
            // 
            this.Rate_textBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.Rate_textBox.BackColor = System.Drawing.Color.White;
            this.Rate_textBox.ForeColor = System.Drawing.Color.Black;
            this.Rate_textBox.Location = new System.Drawing.Point(124, 105);
            this.Rate_textBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Rate_textBox.Name = "Rate_textBox";
            this.Rate_textBox.ReadOnly = true;
            this.Rate_textBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Rate_textBox.Size = new System.Drawing.Size(290, 29);
            this.Rate_textBox.TabIndex = 476;
            this.Rate_textBox.TabStop = false;
            // 
            // label18
            // 
            this.label18.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label18.AutoSize = true;
            this.label18.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.label18.Location = new System.Drawing.Point(936, 106);
            this.label18.Name = "label18";
            this.label18.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label18.Size = new System.Drawing.Size(136, 27);
            this.label18.TabIndex = 477;
            this.label18.Text = "المبلغ المستحق:";
            // 
            // ReturnedAmount1_textBox
            // 
            this.ReturnedAmount1_textBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ReturnedAmount1_textBox.ForeColor = System.Drawing.Color.Black;
            this.ReturnedAmount1_textBox.Location = new System.Drawing.Point(679, 105);
            this.ReturnedAmount1_textBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ReturnedAmount1_textBox.Name = "ReturnedAmount1_textBox";
            this.ReturnedAmount1_textBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.ReturnedAmount1_textBox.Size = new System.Drawing.Size(251, 29);
            this.ReturnedAmount1_textBox.TabIndex = 3;
            this.ReturnedAmount1_textBox.TextChanged += new System.EventHandler(this.ReturnedAmount_textBox_TextChanged);
            this.ReturnedAmount1_textBox.Leave += new System.EventHandler(this.PaymentAmount_textBox_Leave);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 61.96078F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 38.03922F));
            this.tableLayoutPanel2.Controls.Add(this.label10, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.label9, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.label2, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.ReturnedAmount2_label, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.PaidAmount_label, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.RemainOfReturned_label, 0, 2);
            this.tableLayoutPanel2.Font = new System.Drawing.Font("Rubik", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanel2.Location = new System.Drawing.Point(679, 481);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel4.SetRowSpan(this.tableLayoutPanel2, 3);
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(251, 134);
            this.tableLayoutPanel2.TabIndex = 481;
            // 
            // label10
            // 
            this.label10.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label10.AutoSize = true;
            this.label10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.label10.Location = new System.Drawing.Point(158, 95);
            this.label10.Name = "label10";
            this.label10.Padding = new System.Windows.Forms.Padding(1);
            this.label10.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label10.Size = new System.Drawing.Size(82, 32);
            this.label10.TabIndex = 483;
            this.label10.Text = "المتبقي:";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label9
            // 
            this.label9.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.label9.Location = new System.Drawing.Point(158, 44);
            this.label9.Name = "label9";
            this.label9.Padding = new System.Windows.Forms.Padding(1);
            this.label9.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label9.Size = new System.Drawing.Size(85, 44);
            this.label9.TabIndex = 482;
            this.label9.Text = "المدفوع:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.label2.Location = new System.Drawing.Point(158, 6);
            this.label2.Name = "label2";
            this.label2.Padding = new System.Windows.Forms.Padding(1);
            this.label2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label2.Size = new System.Drawing.Size(65, 32);
            this.label2.TabIndex = 481;
            this.label2.Text = "الكلي:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ReturnedAmount2_label
            // 
            this.ReturnedAmount2_label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ReturnedAmount2_label.AutoSize = true;
            this.ReturnedAmount2_label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.ReturnedAmount2_label.Location = new System.Drawing.Point(3, 6);
            this.ReturnedAmount2_label.Name = "ReturnedAmount2_label";
            this.ReturnedAmount2_label.Padding = new System.Windows.Forms.Padding(1);
            this.ReturnedAmount2_label.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.ReturnedAmount2_label.Size = new System.Drawing.Size(149, 32);
            this.ReturnedAmount2_label.TabIndex = 478;
            this.ReturnedAmount2_label.Text = "000,000";
            this.ReturnedAmount2_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PaidAmount_label
            // 
            this.PaidAmount_label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.PaidAmount_label.AutoSize = true;
            this.PaidAmount_label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.PaidAmount_label.Location = new System.Drawing.Point(3, 50);
            this.PaidAmount_label.Name = "PaidAmount_label";
            this.PaidAmount_label.Padding = new System.Windows.Forms.Padding(1);
            this.PaidAmount_label.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.PaidAmount_label.Size = new System.Drawing.Size(149, 32);
            this.PaidAmount_label.TabIndex = 479;
            this.PaidAmount_label.Text = "000,000";
            this.PaidAmount_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // RemainOfReturned_label
            // 
            this.RemainOfReturned_label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.RemainOfReturned_label.AutoSize = true;
            this.RemainOfReturned_label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.RemainOfReturned_label.Location = new System.Drawing.Point(3, 95);
            this.RemainOfReturned_label.Name = "RemainOfReturned_label";
            this.RemainOfReturned_label.Padding = new System.Windows.Forms.Padding(1);
            this.RemainOfReturned_label.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RemainOfReturned_label.Size = new System.Drawing.Size(149, 32);
            this.RemainOfReturned_label.TabIndex = 480;
            this.RemainOfReturned_label.Text = "000,000";
            this.RemainOfReturned_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel5.ColumnCount = 2;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 64.70588F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35.29412F));
            this.tableLayoutPanel5.Controls.Add(this.label13, 1, 2);
            this.tableLayoutPanel5.Controls.Add(this.label12, 1, 1);
            this.tableLayoutPanel5.Controls.Add(this.label11, 1, 0);
            this.tableLayoutPanel5.Controls.Add(this.PaymentsCount_label, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.PaidPaymentsCount_label, 0, 1);
            this.tableLayoutPanel5.Controls.Add(this.RemainOfPaymentsCount_label, 0, 2);
            this.tableLayoutPanel5.Font = new System.Drawing.Font("Rubik", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanel5.Location = new System.Drawing.Point(124, 481);
            this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 3;
            this.tableLayoutPanel4.SetRowSpan(this.tableLayoutPanel5, 3);
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(290, 134);
            this.tableLayoutPanel5.TabIndex = 482;
            // 
            // label13
            // 
            this.label13.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label13.AutoSize = true;
            this.label13.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.label13.Location = new System.Drawing.Point(190, 95);
            this.label13.Name = "label13";
            this.label13.Padding = new System.Windows.Forms.Padding(1);
            this.label13.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label13.Size = new System.Drawing.Size(82, 32);
            this.label13.TabIndex = 484;
            this.label13.Text = "المتبقي:";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label12
            // 
            this.label12.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label12.AutoSize = true;
            this.label12.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.label12.Location = new System.Drawing.Point(190, 50);
            this.label12.Name = "label12";
            this.label12.Padding = new System.Windows.Forms.Padding(1);
            this.label12.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label12.Size = new System.Drawing.Size(91, 32);
            this.label12.TabIndex = 483;
            this.label12.Text = "المدفوع:";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label11
            // 
            this.label11.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label11.AutoSize = true;
            this.label11.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.label11.Location = new System.Drawing.Point(190, 6);
            this.label11.Name = "label11";
            this.label11.Padding = new System.Windows.Forms.Padding(1);
            this.label11.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label11.Size = new System.Drawing.Size(65, 32);
            this.label11.TabIndex = 482;
            this.label11.Text = "الكلي:";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PaymentsCount_label
            // 
            this.PaymentsCount_label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.PaymentsCount_label.AutoSize = true;
            this.PaymentsCount_label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.PaymentsCount_label.Location = new System.Drawing.Point(3, 6);
            this.PaymentsCount_label.Name = "PaymentsCount_label";
            this.PaymentsCount_label.Padding = new System.Windows.Forms.Padding(1);
            this.PaymentsCount_label.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.PaymentsCount_label.Size = new System.Drawing.Size(181, 32);
            this.PaymentsCount_label.TabIndex = 471;
            this.PaymentsCount_label.Text = "00.00";
            this.PaymentsCount_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PaidPaymentsCount_label
            // 
            this.PaidPaymentsCount_label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.PaidPaymentsCount_label.AutoSize = true;
            this.PaidPaymentsCount_label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.PaidPaymentsCount_label.Location = new System.Drawing.Point(3, 50);
            this.PaidPaymentsCount_label.Name = "PaidPaymentsCount_label";
            this.PaidPaymentsCount_label.Padding = new System.Windows.Forms.Padding(1);
            this.PaidPaymentsCount_label.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.PaidPaymentsCount_label.Size = new System.Drawing.Size(181, 32);
            this.PaidPaymentsCount_label.TabIndex = 472;
            this.PaidPaymentsCount_label.Text = "00.00";
            this.PaidPaymentsCount_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // RemainOfPaymentsCount_label
            // 
            this.RemainOfPaymentsCount_label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.RemainOfPaymentsCount_label.AutoSize = true;
            this.RemainOfPaymentsCount_label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.RemainOfPaymentsCount_label.Location = new System.Drawing.Point(3, 95);
            this.RemainOfPaymentsCount_label.Name = "RemainOfPaymentsCount_label";
            this.RemainOfPaymentsCount_label.Padding = new System.Windows.Forms.Padding(1);
            this.RemainOfPaymentsCount_label.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RemainOfPaymentsCount_label.Size = new System.Drawing.Size(181, 32);
            this.RemainOfPaymentsCount_label.TabIndex = 468;
            this.RemainOfPaymentsCount_label.Text = "00.00";
            this.RemainOfPaymentsCount_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LoanDate_DateTimePicker
            // 
            this.LoanDate_DateTimePicker.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LoanDate_DateTimePicker.CustomFormat = "yyyy/MM/dd";
            this.LoanDate_DateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.LoanDate_DateTimePicker.Location = new System.Drawing.Point(679, 242);
            this.LoanDate_DateTimePicker.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.LoanDate_DateTimePicker.Name = "LoanDate_DateTimePicker";
            this.LoanDate_DateTimePicker.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.LoanDate_DateTimePicker.RightToLeftLayout = true;
            this.LoanDate_DateTimePicker.Size = new System.Drawing.Size(251, 29);
            this.LoanDate_DateTimePicker.TabIndex = 8;
            // 
            // AddLoan_button
            // 
            this.AddLoan_button.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.AddLoan_button.BackColor = System.Drawing.Color.Transparent;
            this.AddLoan_button.BackgroundImage = global::MyWorkApplication.Properties.Resources.Save2_D;
            this.AddLoan_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.AddLoan_button.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.AddLoan_button.FlatAppearance.BorderSize = 0;
            this.AddLoan_button.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.AddLoan_button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.AddLoan_button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.AddLoan_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AddLoan_button.ForeColor = System.Drawing.Color.Black;
            this.AddLoan_button.Location = new System.Drawing.Point(547, 350);
            this.AddLoan_button.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.AddLoan_button.Name = "AddLoan_button";
            this.AddLoan_button.Size = new System.Drawing.Size(121, 49);
            this.AddLoan_button.TabIndex = 11;
            this.toolTip1.SetToolTip(this.AddLoan_button, "حفظ القرض");
            this.AddLoan_button.UseVisualStyleBackColor = false;
            this.AddLoan_button.Click += new System.EventHandler(this.AddLoan_button_Click);
            this.AddLoan_button.MouseEnter += new System.EventHandler(this.AddLoan_button_MouseEnter);
            this.AddLoan_button.MouseLeave += new System.EventHandler(this.AddLoan_button_MouseLeave);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.tableLayoutPanel4.SetColumnSpan(this.label3, 5);
            this.label3.Font = new System.Drawing.Font("Rubik", 14F, System.Drawing.FontStyle.Bold);
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(124, 663);
            this.label3.Name = "label3";
            this.label3.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label3.Size = new System.Drawing.Size(972, 46);
            this.label3.TabIndex = 406;
            this.label3.Text = "الدفعات :";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ReceiptNo_textBox
            // 
            this.ReceiptNo_textBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ReceiptNo_textBox.BackColor = System.Drawing.Color.White;
            this.ReceiptNo_textBox.ForeColor = System.Drawing.Color.Black;
            this.ReceiptNo_textBox.Location = new System.Drawing.Point(679, 297);
            this.ReceiptNo_textBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ReceiptNo_textBox.Name = "ReceiptNo_textBox";
            this.ReceiptNo_textBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.ReceiptNo_textBox.Size = new System.Drawing.Size(251, 29);
            this.ReceiptNo_textBox.TabIndex = 9;
            // 
            // DeleteLoan_button
            // 
            this.DeleteLoan_button.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.DeleteLoan_button.BackColor = System.Drawing.Color.Transparent;
            this.DeleteLoan_button.BackgroundImage = global::MyWorkApplication.Properties.Resources.Delete2_D;
            this.DeleteLoan_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.DeleteLoan_button.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.DeleteLoan_button.FlatAppearance.BorderSize = 0;
            this.DeleteLoan_button.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.DeleteLoan_button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.DeleteLoan_button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.DeleteLoan_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DeleteLoan_button.ForeColor = System.Drawing.Color.Black;
            this.DeleteLoan_button.Location = new System.Drawing.Point(420, 350);
            this.DeleteLoan_button.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.DeleteLoan_button.Name = "DeleteLoan_button";
            this.DeleteLoan_button.Size = new System.Drawing.Size(121, 49);
            this.DeleteLoan_button.TabIndex = 11;
            this.toolTip1.SetToolTip(this.DeleteLoan_button, "حذف القرض");
            this.DeleteLoan_button.UseVisualStyleBackColor = false;
            this.DeleteLoan_button.Visible = false;
            this.DeleteLoan_button.Click += new System.EventHandler(this.DeleteLoan_button_Click);
            this.DeleteLoan_button.MouseEnter += new System.EventHandler(this.DeleteLoan_button_MouseEnter);
            this.DeleteLoan_button.MouseLeave += new System.EventHandler(this.DeleteLoan_button_MouseLeave);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 1236);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(1241, 39);
            this.tableLayoutPanel3.TabIndex = 462;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.refreshToolStripMenuItem,
            this.attachments_toolStripMenuItem,
            this.addNotesToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(203, 76);
            // 
            // refreshToolStripMenuItem
            // 
            this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            this.refreshToolStripMenuItem.Size = new System.Drawing.Size(202, 24);
            this.refreshToolStripMenuItem.Text = "Refresh";
            this.refreshToolStripMenuItem.Click += new System.EventHandler(this.refreshToolStripMenuItem_Click);
            // 
            // attachments_toolStripMenuItem
            // 
            this.attachments_toolStripMenuItem.Name = "attachments_toolStripMenuItem";
            this.attachments_toolStripMenuItem.Size = new System.Drawing.Size(202, 24);
            this.attachments_toolStripMenuItem.Text = "Show Attachments";
            this.attachments_toolStripMenuItem.Click += new System.EventHandler(this.attachments_toolStripMenuItem_Click);
            // 
            // addNotesToolStripMenuItem
            // 
            this.addNotesToolStripMenuItem.Name = "addNotesToolStripMenuItem";
            this.addNotesToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.addNotesToolStripMenuItem.Size = new System.Drawing.Size(202, 24);
            this.addNotesToolStripMenuItem.Text = "Add Notes";
            this.addNotesToolStripMenuItem.Click += new System.EventHandler(this.addNotesToolStripMenuItem_Click);
            // 
            // Loans_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 27F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(1262, 676);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Rubik", 10.8F);
            this.ForeColor = System.Drawing.Color.Black;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Loans_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.LoanPayments_Form_Load);
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Payment_dataGridView)).EndInit();
            this.tableLayoutPanel21.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox LoanAmount_textBox;
        private System.Windows.Forms.TextBox MicroProject_ID_textBox;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView Payment_dataGridView;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button AddLoan_button;
        private System.Windows.Forms.TextBox Person_Name_textBox;
        private System.Windows.Forms.Button DeleteLoan_button;
        private System.Windows.Forms.Button SavePayments_button;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox PaymentAmount_textBox;
        private System.Windows.Forms.TextBox Rate_textBox;
        private System.Windows.Forms.TextBox ReceiptNo_textBox;
        private System.Windows.Forms.Label label8;
        private Classes.BCDateTimePicker LoanDateTaken_dateTimePicker;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox Note_textBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn Pay_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Pay_Loan_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Pay_Amount;
        private System.Windows.Forms.DataGridViewTextBoxColumn Pay_DueDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn Pay_RecievedOnDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn Pay_Note;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Pay_IsPaid;
        private System.Windows.Forms.DataGridViewButtonColumn P_DeleteRow;
        private System.Windows.Forms.DataGridViewTextBoxColumn P_InDataBase;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel21;
        private System.Windows.Forms.Button NewGroup_button;
        private System.Windows.Forms.ComboBox Group_comboBox;
        private System.Windows.Forms.TextBox dolarAmount_textBox;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox ReturnedAmount1_textBox;
        private System.Windows.Forms.Label ReturnedAmount2_label;
        private System.Windows.Forms.Label PaidAmount_label;
        private System.Windows.Forms.Label RemainOfReturned_label;
        private System.Windows.Forms.Label PaymentsCount_label;
        private System.Windows.Forms.Label PaidPaymentsCount_label;
        private System.Windows.Forms.Label RemainOfPaymentsCount_label;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.DateTimePicker LoanDate_DateTimePicker;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem attachments_toolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addNotesToolStripMenuItem;
    }
}