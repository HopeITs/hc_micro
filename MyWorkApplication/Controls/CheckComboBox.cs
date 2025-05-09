using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace MyWorkApplication.Classes
{
    /// <summary>
    ///     Inherits from ComboBox and handles DrawItem and SelectedIndexChanged events to create an
    ///     owner drawn combo box drop-down.  The contents of the dropdown are rendered using the
    ///     CheckBoxRenderer class.
    /// </summary>
    public class CheckComboBox : ComboBox
    {
        /// <summary>
        ///     C'tor
        /// </summary>
        public CheckComboBox()
        {
            DrawMode = DrawMode.OwnerDrawFixed;
            DrawItem += CheckComboBox_DrawItem;
            SelectedIndexChanged += CheckComboBox_SelectedIndexChanged;
            SelectedText = "Select Options";
        }

        /// <summary>
        ///     Invoked when the selected index is changed on the dropdown.  This sets the check state
        ///     of the CheckComboBoxItem and fires the public event CheckStateChanged using the
        ///     CheckComboBoxItem as the event sender.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var item = (CheckComboBoxItem) SelectedItem;
            item.CheckState = !item.CheckState;
            if (CheckStateChanged != null)
                CheckStateChanged(item, e);
        }

        /// <summary>
        ///     Invoked when the ComboBox has to render the drop-down items.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckComboBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            // make sure the index is valid (sanity check)
            if (e.Index == -1) return;

            // test the item to see if its a CheckComboBoxItem
            if (!(Items[e.Index] is CheckComboBoxItem))
            {
                // it's not, so just render it as a default string
                e.Graphics.DrawString(
                    Items[e.Index].ToString(),
                    Font,
                    Brushes.Black,
                    new Point(e.Bounds.X, e.Bounds.Y));
                return;
            }

            // get the CheckComboBoxItem from the collection
            var box = (CheckComboBoxItem) Items[e.Index];

            // render it
            CheckBoxRenderer.RenderMatchingApplicationState = true;
            CheckBoxRenderer.DrawCheckBox(
                e.Graphics,
                new Point(e.Bounds.X, e.Bounds.Y),
                e.Bounds,
                box.Text,
                Font,
                (e.State & DrawItemState.Focus) == 0,
                box.CheckState ? CheckBoxState.CheckedNormal : CheckBoxState.UncheckedNormal);
        }

        /// <summary>
        ///     Fired when the user clicks a check box item in the drop-down list
        /// </summary>
        public event EventHandler CheckStateChanged;
    }
}