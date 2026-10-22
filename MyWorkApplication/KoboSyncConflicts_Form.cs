using MyWorkApplication.Classes;
using MyWorkApplication.Properties;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace MyWorkApplication
{
    public partial class KoboSyncConflicts_Form : Form
    {
        private readonly KoboConflictReview review = new KoboConflictReview();
        private void MatchPeople_Click(object sender,EventArgs e)
        {
            if(busy) return;
            using(KoboPersonReview_Form form=new KoboPersonReview_Form()) form.ShowDialog(this);
            Reload();
        }
        private DataTable records, differences;
        private bool loading, busy;
        public KoboSyncConflicts_Form() : this(null) { }
        public KoboSyncConflicts_Form(MainForm mainForm)
        {
            InitializeComponent();
            filter.SelectedIndex = 0;
            decision.SelectedIndex = 3;
        }

        private void Form_Load(object sender, EventArgs e) { Check_Theme(); Reload(); }
        public void Check_Theme()
        {
            bool dark = Settings.Default.theme == "Dark";
            try
            {
                NewTheme theme = new NewTheme();
                if(dark) theme.ApplicationNew_ToNight(this); else theme.ApplicationNew_ToLight(this);
            }
            catch { }
            title.ForeColor = dark ? NewTheme.light_white : NewTheme.light_red;
            refresh.BackgroundImage = dark ? Resources.Refresh2_L : Resources.Refresh2_D;
            changedOnly.ForeColor = dark ? NewTheme.light_white : NewTheme.light_red;
            changedOnly.FlatAppearance.CheckedBackColor = dark ? NewTheme.dark_grey2 : Color.White;
            changedOnly.FlatAppearance.MouseOverBackColor = dark ? NewTheme.light_grey4 : NewTheme.light_grey1;
            apply.BackColor = dark ? NewTheme.light_grey4 : NewTheme.light_grey1;
            matchPeople.BackColor = dark ? NewTheme.light_grey4 : NewTheme.light_grey1;
            matchPeople.ForeColor = dark ? Color.White : NewTheme.light_grey4;
            apply.ForeColor = dark ? Color.White : NewTheme.light_grey4;
            apply.FlatAppearance.BorderColor = NewTheme.light_grey2;
            apply.FlatAppearance.MouseOverBackColor = NewTheme.light_grey2;
            foreach(DataGridView grid in new DataGridView[] { conflicts,comparison,history })
            {
                grid.BackgroundColor = dark ? NewTheme.dark_grey2 : Color.White;
                grid.DefaultCellStyle.BackColor = grid.BackgroundColor;
                grid.DefaultCellStyle.ForeColor = dark ? Color.White : NewTheme.light_grey4;
                grid.DefaultCellStyle.SelectionBackColor = dark ? NewTheme.light_grey4 : NewTheme.light_grey2;
                grid.DefaultCellStyle.SelectionForeColor = dark ? Color.White : Color.Black;
                grid.ColumnHeadersDefaultCellStyle.BackColor = dark ? NewTheme.light_grey4 : NewTheme.light_red;
                grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                grid.ColumnHeadersDefaultCellStyle.SelectionBackColor = grid.ColumnHeadersDefaultCellStyle.BackColor;
                grid.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
                grid.RowHeadersDefaultCellStyle.SelectionBackColor = grid.DefaultCellStyle.SelectionBackColor;
                grid.RowHeadersDefaultCellStyle.SelectionForeColor = grid.DefaultCellStyle.SelectionForeColor;
                grid.RowsDefaultCellStyle.SelectionBackColor = grid.DefaultCellStyle.SelectionBackColor;
                grid.RowsDefaultCellStyle.SelectionForeColor = grid.DefaultCellStyle.SelectionForeColor;
                grid.AlternatingRowsDefaultCellStyle.SelectionBackColor = grid.DefaultCellStyle.SelectionBackColor;
                grid.AlternatingRowsDefaultCellStyle.SelectionForeColor = grid.DefaultCellStyle.SelectionForeColor;
                foreach(DataGridViewColumn column in grid.Columns)
                {
                    column.HeaderCell.Style.SelectionBackColor = grid.ColumnHeadersDefaultCellStyle.BackColor;
                    column.HeaderCell.Style.SelectionForeColor = Color.White;
                }
            }
        }

        private void Combo_DrawItem(object sender,DrawItemEventArgs e)
        {
            ComboBox combo=(ComboBox)sender;
            bool dark=Settings.Default.theme=="Dark";
            Color background=(e.State & DrawItemState.Selected)!=0 ? NewTheme.light_grey2 : (dark?NewTheme.dark_grey2:Color.White);
            Color foreground=(e.State & DrawItemState.Selected)!=0 ? NewTheme.light_grey4 : (dark?Color.White:NewTheme.light_grey4);
            using(SolidBrush brush=new SolidBrush(background)) e.Graphics.FillRectangle(brush,e.Bounds);
            string text=e.Index>=0?combo.GetItemText(combo.Items[e.Index]):combo.Text;
            Rectangle bounds=e.Bounds; bounds.Inflate(-5,0);
            TextRenderer.DrawText(e.Graphics,text,combo.Font,bounds,foreground,
                TextFormatFlags.Right|TextFormatFlags.RightToLeft|TextFormatFlags.VerticalCenter|TextFormatFlags.EndEllipsis);
        }

        private void Reload()
        {
            if(busy) return;
            try
            {
                loading=true; records=review.List();
                records.DefaultView.RowFilter=filter.SelectedIndex==0?"State <> 'resolved'":"";
                conflicts.DataSource=records.DefaultView;
                loading=false; Conflict_Selected(this,EventArgs.Empty);
                if(Selected()==null) status.Text="لا توجد تعارضات ضمن العرض المحدد.";
            }
            catch(Exception ex) { loading=false; apply.Enabled=false; status.Text=ex.Message; MessageBox.Show(ex.Message,"Sync Conflicts"); }
        }
        private DataRow Selected()
        {
            DataRowView selected=conflicts.CurrentRow==null?null:conflicts.CurrentRow.DataBoundItem as DataRowView;
            return selected==null?null:selected.Row;
        }
        private void Refresh_Click(object sender,EventArgs e) { Reload(); }
        private void Filter_Changed(object sender,EventArgs e) { if(records!=null) Reload(); }
        private void Differences_Changed(object sender,EventArgs e)
        { if(differences!=null) differences.DefaultView.RowFilter=changedOnly.Checked?"Changed = true":""; }

        private void Conflict_Selected(object sender,EventArgs e)
        {
            if(loading || busy) return;
            try
            {
                DataRow row=Selected();
                apply.Enabled=row!=null && (row["Applied_At"]==DBNull.Value || row["State"].ToString()=="stale");
                comparison.DataSource=null; history.DataSource=null; reason.Text="";
                if(row==null) { identity.Text=""; return; }
                differences=KoboConflictPolicy.Compare(row["Existing_JSON"].ToString(),row["Incoming_JSON"].ToString());
                foreach(DataRow field in differences.Rows)
                {
                    string key=field["Field"].ToString(),leftUid=row["Existing_UID"].ToString(),rightUid=row["Incoming_UID"].ToString();
                    string left=KoboReviewLabels.Label(leftUid,key),right=KoboReviewLabels.Label(rightUid,key);
                    field["Question"]=left==right?left:(left+" / "+right);
                    if(left!=right) field["Changed"]=true;
                    field["Existing"]=KoboReviewLabels.Value(leftUid,key,field["Existing"].ToString());
                    field["Incoming"]=KoboReviewLabels.Value(rightUid,key,field["Incoming"].ToString());
                }
                comparison.DataSource=differences.DefaultView; Differences_Changed(this,EventArgs.Empty);
                identity.Text="المشروع: " + row["Project"] + " | الشخص: " + row["Person"] + " | الزيارة: " + row["Visit_ID"] + " | " + row["Form"];
                status.Text=KoboSyncUserMessages.Conflict(row["Visit_ID"].ToString(),row["Existing"].ToString(),row["Incoming"].ToString(),row["State"].ToString());
                reason.Text=row["Reason"].ToString();
                string[] decisions={"replace","keep_both","ignore","defer"};
                int index=Array.IndexOf(decisions,row["Decision"].ToString()); decision.SelectedIndex=index<0?3:index;
                decision.Enabled=row["Applied_At"]==DBNull.Value;
                history.DataSource=review.History(Convert.ToInt32(row["ID"]));
            }
            catch(Exception ex) { apply.Enabled=false; status.Text=ex.Message; }
        }

        private async void Apply_Click(object sender,EventArgs e)
        {
            if(busy) return;
            DataRow row=Selected(); if(row==null) return;
            try
            {
                string[] choices={"replace","keep_both","ignore","defer"};
                string action=choices[decision.SelectedIndex];
                KoboConflictPolicy.ValidateDecision(action,reason.Text);
                if(MessageBox.Show("اعتماد القرار المحدد لهذا التعارض؟", "Sync Conflicts",MessageBoxButtons.YesNo,MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2,MessageBoxOptions.RightAlign|MessageBoxOptions.RtlReading)!=DialogResult.Yes) return;
                busy=true; apply.Enabled=false; UseWaitCursor=true;
                int id=Convert.ToInt32(row["ID"]);
                int revision=Convert.ToInt32(row["Revision"]);
                string explanation=reason.Text;
                string ih=KoboConflictPolicy.Hash(row["Incoming_JSON"].ToString()),eh=KoboConflictPolicy.Hash(row["Existing_JSON"].ToString());
                await Task.Run(delegate { review.Decide(id,action,explanation,ih,eh,revision); });
            }
            catch(Exception ex) { MessageBox.Show(ex.Message,"Sync Conflicts",MessageBoxButtons.OK,MessageBoxIcon.Warning); }
            finally { busy=false; UseWaitCursor=false; Reload(); }
        }
        private void Grid_DataError(object sender,DataGridViewDataErrorEventArgs e) { e.ThrowException=false; }
        private void Grid_CellFormatting(object sender,DataGridViewCellFormattingEventArgs e)
        {
            if(e.Value==null) return;
            DataGridView grid=(DataGridView)sender;
            string column=grid.Columns[e.ColumnIndex].DataPropertyName;
            if(column!="State" && column!="القرار" && column!="الحدث") return;
            string[] codes={"open","deferred","approved","resolved","stale","blocked","replace","keep_both","ignore","defer","decision","applied","re-reviewed"};
            string[] labels={"بانتظار المراجعة","مؤجل","معتمد للتطبيق","محسوم","البيانات تغيرت","يتطلب تدخلاً","استبدال الحالي","الاحتفاظ بكليهما","تجاهل الوارد","القرار لاحقاً","اعتماد القرار","تطبيق القرار","إعادة المراجعة"};
            int index=Array.IndexOf(codes,e.Value.ToString());
            if(index>=0) { e.Value=labels[index]; e.FormattingApplied=true; }
        }
        private void Form_Closing(object sender,FormClosingEventArgs e) { if(busy) e.Cancel=true; }
    }
}
