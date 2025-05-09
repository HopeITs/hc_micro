using System;
using System.Drawing;
using System.Windows.Forms;

namespace MyWorkApplication.Classes
{
    internal class DataGridViewColumnSelector
    {
        /// <summary>
        ///     The max height of the popup
        /// </summary>
        public int MaxHeight = 300;

        // a CheckedListBox containing the column header text and checkboxes
        private CheckedListBox mCheckedListBox;

        // the DataGridView to which the DataGridViewColumnSelector is attached

        // a ToolStripDropDown object used to show the popup
        private readonly ToolStripDropDown mPopup;

        private readonly UserControlMenu pUserControl1 = new UserControlMenu();

        /// <summary>
        ///     The width of the popup
        /// </summary>
        public int Width = 250;

        // The constructor creates an instance of CheckedListBox and ToolStripDropDown.
        // the CheckedListBox is hosted by ToolStripControlHost, which in turn is
        // added to ToolStripDropDown.
        public DataGridViewColumnSelector()
        {
            //mCheckedListBox = new CheckedListBox();
            //mCheckedListBox.CheckOnClick = true;
            //mCheckedListBox.ItemCheck += new ItemCheckEventHandler(mCheckedListBox_ItemCheck); 
            //ToolStripControlHost mControlHost = new ToolStripControlHost(mCheckedListBox);

            pUserControl1.DoneEvent += OnDone;
            pUserControl1.CheckedChangedEnent += CheckedChangedEnent;
            var mControlHost = new ToolStripControlHost(pUserControl1);
            mControlHost.Padding = Padding.Empty;
            mControlHost.Margin = Padding.Empty;
            mControlHost.AutoSize = false;

            mPopup = new ToolStripDropDown();
            mPopup.Padding = Padding.Empty;

            //mPopup.SetAutoScrollMargin(5, 5);
            //mPopup.AutoScrollOffset = new Point(5,5);
             
            mPopup.AutoClose = true;
            mPopup.Items.Add(mControlHost);
        }

        public DataGridViewColumnSelector(DataGridView dgv) : this()
        {
            DataGridView = dgv;
        }

        /// <summary>
        ///     Gets or sets the DataGridView to which the DataGridViewColumnSelector is attached
        /// </summary>
        public DataGridView DataGridView
        {
            get;
            set;
            // Attach CellMouseClick handler to DataGridView
            //    if (mDataGridView != null) mDataGridView.CellMouseClick += new DataGridViewCellMouseEventHandler(mDataGridView_CellMouseClick);
        }

        // When user right-clicks the cell origin, it clears and fill the CheckedListBox with
        // columns header text. Then it shows the popup. 
        // In this way the CheckedListBox items are always refreshed to reflect changes occurred in 
        // DataGridView columns (column additions or name changes and so on).
        public void mDataGridView_MouseClick(object sender, MouseEventArgs e)
        {
            //if (e.Button == MouseButtons.Right)e.X /// e.Y
            //{
            // && e.RowIndex == -1 && e.ColumnIndex == -1//
            //mCheckedListBox.Items.Clear();
            //foreach (DataGridViewColumn c in mDataGridView.Columns){
            //    mCheckedListBox.Items.Add(c.HeaderText, c.Visible);
            //}
            //int PreferredHeight = (mCheckedListBox.Items.Count * 16) + 7;
            //mCheckedListBox.Height = (PreferredHeight < MaxHeight) ? PreferredHeight : MaxHeight;
            //mCheckedListBox.Width = this.Width;

            pUserControl1.Initialize(DataGridView); 
            mPopup.Show(DataGridView.PointToScreen(new Point(1, 0))); 

            //}
        }

        private void CheckedChangedEnent(int iIndex, bool bChecked)
        {
            DataGridView.Columns[iIndex].Visible = bChecked;
        }

        private void OnDone(object sender, EventArgs e)
        {
            mPopup.AutoClose = false;
            mPopup.Close();
            mPopup.AutoClose = true;
        }

        // When user checks / unchecks a checkbox, the related column visibility is 
        // switched.
        private void mCheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            DataGridView.Columns[e.Index].Visible = e.NewValue == CheckState.Checked;
        }
    }
}