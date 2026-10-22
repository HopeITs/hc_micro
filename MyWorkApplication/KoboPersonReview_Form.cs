using MyWorkApplication.Classes;
using MyWorkApplication.Properties;
using System;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyWorkApplication
{
    public partial class KoboPersonReview_Form : Form
    {
        private readonly KoboPersonReview review=new KoboPersonReview();
        private bool loading,busy;
        private int submission,project,revision;
        private string sourceHash,name,selectedHash;
        private int? selectedPerson;
        public KoboPersonReview_Form() { InitializeComponent(); }
        private void Form_Load(object sender,EventArgs e) { Check_Theme(); Reload(); }
        private void Form_Closing(object sender,FormClosingEventArgs e) { if(busy) e.Cancel=true; }
        private void Refresh_Click(object sender,EventArgs e) { Reload(); }
        public void Check_Theme()
        {
            bool dark=Settings.Default.theme=="Dark";
            BackColor=dark?NewTheme.light_grey3:NewTheme.light_grey1;
            ForeColor=dark?Color.White:NewTheme.light_grey4;
            title.ForeColor=dark?Color.White:NewTheme.light_red;
            refresh.BackgroundImage=dark?Resources.Refresh2_L:Resources.Refresh2_D;
            reason.BackColor=dark?NewTheme.dark_grey2:Color.White; reason.ForeColor=ForeColor;
            foreach(Button button in new Button[]{refresh,confirm,defer})
            { button.BackColor=BackColor; button.ForeColor=ForeColor; button.FlatAppearance.BorderColor=NewTheme.light_grey2; }
            foreach(DataGridView grid in new DataGridView[]{queue,candidates})
            {
                grid.BackgroundColor=dark?NewTheme.dark_grey2:Color.White;
                grid.DefaultCellStyle.BackColor=grid.BackgroundColor; grid.DefaultCellStyle.ForeColor=ForeColor;
                grid.DefaultCellStyle.SelectionBackColor=NewTheme.light_grey2; grid.DefaultCellStyle.SelectionForeColor=Color.Black;
                grid.ColumnHeadersDefaultCellStyle.BackColor=NewTheme.light_red; grid.ColumnHeadersDefaultCellStyle.ForeColor=Color.White;
                grid.ColumnHeadersDefaultCellStyle.SelectionBackColor=NewTheme.light_red; grid.ColumnHeadersDefaultCellStyle.SelectionForeColor=Color.White;
            }
        }
        private void Reload()
        {
            if(busy) return;
            try
            {
                loading=true; queue.DataSource=review.List();
                string[] headers={"رقم السجل","المشروع عند تسجيل الحالة","الاسم في كوبو عند تسجيل الحالة","الحالة","تفاصيل المزامنة"};
                for(int i=0;i<queue.Columns.Count && i<headers.Length;i++) queue.Columns[i].HeaderText=headers[i];
                foreach(DataGridViewRow row in queue.Rows)
                {
                    string state=row.Cells["State"].Value.ToString();
                    row.Cells["State"].Value=state=="confirmed"?"تم الاعتماد":state=="deferred"?"مؤجل":state=="stale"?"تغيرت البيانات":"بانتظار المطابقة";
                }
                queue.ClearSelection(); queue.CurrentCell=null; ResetSelection(); status.Text="";
            }
            catch(Exception ex) { status.Text=ex.Message; }
            finally { loading=false; }
        }
        private void ResetSelection()
        {
            submission=0; selectedPerson=null; selectedHash=null; confirm.Enabled=false; defer.Enabled=false;
            candidates.DataSource=null; sourceLabel.Text=""; reason.Clear();
        }
        private void Queue_Changed(object sender,EventArgs e)
        {
            if(loading || busy) return;
            ResetSelection(); if(queue.CurrentRow==null) return;
            try
            {
                submission=Convert.ToInt32(queue.CurrentRow.Cells["Submission_ID"].Value);
                DataTable rows=review.Load(submission,out project,out name,out sourceHash,out revision);
                candidates.DataSource=rows;
                string[] headers={"رقم الشخص","العلاقة بالمشروع","الاسم الأول","اسم الأب","الكنية"};
                for(int i=0;i<candidates.Columns.Count;i++) candidates.Columns[i].HeaderText=headers[i];
                candidates.ClearSelection(); candidates.CurrentCell=null;
                sourceLabel.Text="المشروع: "+project+"   |   الاسم في كوبو: "+name+Environment.NewLine+
                    rows.ExtendedProperties["Form"]+"   |   سجل كوبو: "+rows.ExtendedProperties["Kobo_ID"]+
                    "   |   التاريخ: "+rows.ExtendedProperties["Date"];
                defer.Enabled=true; status.Text=rows.Rows.Count==0?"لا يوجد مستفيد أو شريك مرتبط بهذا المشروع. راجع بيانات المشروع قبل المتابعة.":"";
            }
            catch(Exception ex) { submission=0; status.Text=ex.Message; }
        }
        private void Candidate_Click(object sender,DataGridViewCellEventArgs e)
        { if(e.RowIndex>=0 && !busy) SelectCandidate(candidates.Rows[e.RowIndex]); }
        private void Candidate_KeyDown(object sender,KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter && candidates.CurrentRow!=null && !busy)
            { SelectCandidate(candidates.CurrentRow); e.Handled=true; }
            else if(e.KeyCode==Keys.Up || e.KeyCode==Keys.Down || e.KeyCode==Keys.Home || e.KeyCode==Keys.End)
            { selectedPerson=null; selectedHash=null; confirm.Enabled=false; }
        }
        private void SelectCandidate(DataGridViewRow row)
        {
            DataRowView value=row.DataBoundItem as DataRowView; if(value==null || submission==0) return;
            selectedPerson=Convert.ToInt32(value.Row["Person_ID"]); selectedHash=KoboPersonReview.RelationshipHash(value.Row);
            confirm.Enabled=true;
            status.Text="الشخص المحدد: "+value.Row["P_FirstName"]+" "+value.Row["P_FatherName"]+" "+value.Row["P_LastName"]+" ("+selectedPerson+")";
        }
        private async void Confirm_Click(object sender,EventArgs e) { await Save(true); }
        private async void Defer_Click(object sender,EventArgs e) { await Save(false); }
        private async Task Save(bool confirmChoice)
        {
            if(busy || submission==0 || (confirmChoice && !selectedPerson.HasValue)) return;
            if(confirmChoice && string.IsNullOrWhiteSpace(reason.Text)) { status.Text="يرجى كتابة سبب اختيار الشخص."; return; }
            if(confirmChoice && MessageBox.Show(this,"اعتماد الشخص رقم "+selectedPerson+" للمشروع "+project+" لهذه الاستمارة فقط؟",
                "تأكيد المطابقة",MessageBoxButtons.YesNo,MessageBoxIcon.Question,MessageBoxDefaultButton.Button2,
                MessageBoxOptions.RtlReading|MessageBoxOptions.RightAlign)!=DialogResult.Yes) return;
            int id=submission,p=project,v=revision; string h=sourceHash,rh=selectedHash,r=reason.Text;
            int? person=confirmChoice?selectedPerson:null;
            busy=true; layout.Enabled=false;
            try
            {
                string message=await Task.Run(()=>review.Decide(id,p,h,v,person,rh,r));
                busy=false; Reload(); status.Text=message;
            }
            catch(Exception ex) { status.Text=ex.Message; }
            finally { busy=false; layout.Enabled=true; }
        }
    }
}
