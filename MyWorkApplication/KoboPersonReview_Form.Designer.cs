namespace MyWorkApplication
{
    partial class KoboPersonReview_Form
    {
        private System.ComponentModel.IContainer components=null;
        protected override void Dispose(bool disposing)
        { if(disposing && components!=null) components.Dispose(); base.Dispose(disposing); }
        private void InitializeComponent()
        {
            this.layout=new System.Windows.Forms.TableLayoutPanel();
            this.title=new System.Windows.Forms.Label();
            this.refresh=new System.Windows.Forms.Button();
            this.queue=new System.Windows.Forms.DataGridView();
            this.sourceLabel=new System.Windows.Forms.Label();
            this.candidates=new System.Windows.Forms.DataGridView();
            this.actions=new System.Windows.Forms.TableLayoutPanel();
            this.reasonLabel=new System.Windows.Forms.Label();
            this.reason=new System.Windows.Forms.TextBox();
            this.confirm=new System.Windows.Forms.Button();
            this.defer=new System.Windows.Forms.Button();
            this.status=new System.Windows.Forms.Label();
            this.SuspendLayout(); this.layout.SuspendLayout(); this.actions.SuspendLayout();
            this.layout.Dock=System.Windows.Forms.DockStyle.Fill;
            this.layout.Padding=new System.Windows.Forms.Padding(18);
            this.layout.ColumnCount=1; this.layout.RowCount=8;
            this.layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent,100F));
            this.layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute,44F));
            this.layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute,38F));
            this.layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent,45F));
            this.layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute,60F));
            this.layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent,55F));
            this.layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute,26F));
            this.layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute,44F));
            this.layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute,60F));
            this.layout.Controls.Add(this.title,0,0); this.layout.Controls.Add(this.refresh,0,1);
            this.layout.Controls.Add(this.queue,0,2); this.layout.Controls.Add(this.sourceLabel,0,3);
            this.layout.Controls.Add(this.candidates,0,4); this.layout.Controls.Add(this.reasonLabel,0,5);
            this.layout.Controls.Add(this.actions,0,6); this.layout.Controls.Add(this.status,0,7);
            this.title.Text="مطابقة المستفيدين في زيارات كوبو";
            this.title.Font=new System.Drawing.Font("Microsoft Sans Serif",18F,System.Drawing.FontStyle.Bold);
            this.title.Dock=System.Windows.Forms.DockStyle.Fill;
            this.title.TextAlign=System.Drawing.ContentAlignment.MiddleCenter;
            this.refresh.Size=new System.Drawing.Size(36,32);
            this.refresh.BackgroundImage=global::MyWorkApplication.Properties.Resources.Refresh2_D;
            this.refresh.BackgroundImageLayout=System.Windows.Forms.ImageLayout.Zoom;
            this.refresh.AccessibleName="تحديث"; this.refresh.Click+=new System.EventHandler(this.Refresh_Click);
            this.queue.Dock=System.Windows.Forms.DockStyle.Fill;
            this.queue.ReadOnly=true; this.queue.AllowUserToAddRows=false; this.queue.AllowUserToDeleteRows=false;
            this.queue.MultiSelect=false; this.queue.RowHeadersVisible=false;
            this.queue.SelectionMode=System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.queue.AutoSizeColumnsMode=System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.queue.EnableHeadersVisualStyles=false;
            this.queue.ColumnHeadersHeightSizeMode=System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.queue.SelectionChanged+=new System.EventHandler(this.Queue_Changed);
            this.sourceLabel.Dock=System.Windows.Forms.DockStyle.Fill;
            this.sourceLabel.TextAlign=System.Drawing.ContentAlignment.MiddleLeft;
            this.candidates.Dock=System.Windows.Forms.DockStyle.Fill;
            this.candidates.ReadOnly=true; this.candidates.AllowUserToAddRows=false; this.candidates.AllowUserToDeleteRows=false;
            this.candidates.MultiSelect=false; this.candidates.RowHeadersVisible=false;
            this.candidates.SelectionMode=System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.candidates.AutoSizeColumnsMode=System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.candidates.EnableHeadersVisualStyles=false;
            this.candidates.ColumnHeadersHeightSizeMode=System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.candidates.CellClick+=new System.Windows.Forms.DataGridViewCellEventHandler(this.Candidate_Click);
            this.candidates.KeyDown+=new System.Windows.Forms.KeyEventHandler(this.Candidate_KeyDown);
            this.reasonLabel.Text="سبب القرار"; this.reasonLabel.Dock=System.Windows.Forms.DockStyle.Fill;
            this.reasonLabel.TextAlign=System.Drawing.ContentAlignment.MiddleLeft;
            this.actions.Dock=System.Windows.Forms.DockStyle.Fill; this.actions.Margin=new System.Windows.Forms.Padding(0);
            this.actions.ColumnCount=3; this.actions.RowCount=1;
            this.actions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent,100F));
            this.actions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute,166F));
            this.actions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute,130F));
            this.actions.Controls.Add(this.reason,0,0); this.actions.Controls.Add(this.confirm,1,0); this.actions.Controls.Add(this.defer,2,0);
            this.reason.Multiline=true; this.reason.Height=34;
            this.reason.Anchor=System.Windows.Forms.AnchorStyles.Top|System.Windows.Forms.AnchorStyles.Left|System.Windows.Forms.AnchorStyles.Right;
            this.confirm.Text="اعتماد الشخص المحدد"; this.confirm.Enabled=false; this.confirm.Dock=System.Windows.Forms.DockStyle.Fill;
            this.confirm.FlatStyle=System.Windows.Forms.FlatStyle.Flat; this.confirm.Click+=new System.EventHandler(this.Confirm_Click);
            this.defer.Text="القرار لاحقاً"; this.defer.Dock=System.Windows.Forms.DockStyle.Fill;
            this.defer.FlatStyle=System.Windows.Forms.FlatStyle.Flat; this.defer.Click+=new System.EventHandler(this.Defer_Click);
            this.status.Dock=System.Windows.Forms.DockStyle.Fill; this.status.TextAlign=System.Drawing.ContentAlignment.MiddleLeft;
            this.ClientSize=new System.Drawing.Size(1080,740); this.MinimumSize=new System.Drawing.Size(900,650);
            this.Font=new System.Drawing.Font("Microsoft Sans Serif",11F);
            this.AutoScaleMode=System.Windows.Forms.AutoScaleMode.Font; this.RightToLeft=System.Windows.Forms.RightToLeft.Yes;
            this.StartPosition=System.Windows.Forms.FormStartPosition.CenterParent;
            this.Name="KoboPersonReview_Form"; this.Text="مطابقة المستفيدين";
            this.Controls.Add(this.layout); this.Load+=new System.EventHandler(this.Form_Load);
            this.FormClosing+=new System.Windows.Forms.FormClosingEventHandler(this.Form_Closing);
            this.actions.ResumeLayout(false); this.actions.PerformLayout(); this.layout.ResumeLayout(false); this.ResumeLayout(false);
        }
        private System.Windows.Forms.TableLayoutPanel layout,actions;
        private System.Windows.Forms.Label title,sourceLabel,reasonLabel,status;
        private System.Windows.Forms.Button refresh,confirm,defer;
        private System.Windows.Forms.TextBox reason;
        private System.Windows.Forms.DataGridView queue,candidates;
    }
}
