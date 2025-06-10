using System;
using System.Windows.Forms;

namespace MyWorkApplication.Classes
{
    public partial class UserControlMenu : UserControl
    {
        public delegate void CheckedChanged(int iIndex, bool bChecked);

        public EventHandler DoneEvent;

        private MenuControl m_pMenuControl = new MenuControl();

        public UserControlMenu()
        {
            InitializeComponent();
        }

        public event CheckedChanged CheckedChangedEnent;

        public virtual void OnCheckedChanged(int iIndex, bool bChecked)
        {
            if (CheckedChangedEnent != null)
                CheckedChangedEnent(iIndex, bChecked);
        }

        public virtual void OnDone()
        {
            if (DoneEvent != null)
                DoneEvent(this, EventArgs.Empty);
        }

        private void buttonDone_Click(object sender, EventArgs e)
        {
            Parent.Focus();
        }

        public void Initialize(DataGridView pDataGridView)
        {
            m_pMenuControl = new MenuControl();

            foreach (DataGridViewColumn c in pDataGridView.Columns) m_pMenuControl.Add(c.HeaderText, c.Visible);

            m_pMenuControl.Prepare(CreateGraphics());

            Width = m_pMenuControl.Width;
            Height = m_pMenuControl.Height;

            timer1.Enabled = true;
        }

        private void UserControlMenu_Paint(object sender, PaintEventArgs e)
        {
            m_pMenuControl.Draw(e.Graphics);
        }

        private void UserControlMenu_MouseMove(object sender, MouseEventArgs e)
        {
            if (m_pMenuControl.HitTestMouseMove(e.X, e.Y)) m_pMenuControl.Draw(CreateGraphics());
        }

        private void UserControlMenu_MouseDown(object sender, MouseEventArgs e)
        {
            if (m_pMenuControl.HitTestMouseDown(e.X, e.Y))
            {
                if (m_pMenuControl.Done)
                {
                    OnDone();
                }
                else
                {
                    var iHitIndex = m_pMenuControl.HitIndex;
                    if (iHitIndex != -1)
                    {
                        var bChecked = m_pMenuControl.ChangeChecked(iHitIndex, CreateGraphics());
                        OnCheckedChanged(iHitIndex, bChecked);
                    }
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            var pPoint = PointToClient(Cursor.Position);
            if (m_pMenuControl.HitTestMouseMove(pPoint.X, pPoint.Y))
                m_pMenuControl.Draw(CreateGraphics());
        }
    }
}