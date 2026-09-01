using System.Windows.Forms;

namespace MyWorkApplication.Controls
{
    public class NoFocusButton : Button
    {
        public NoFocusButton()
        {
            SetStyle(ControlStyles.Selectable, false);
        }

        protected override bool ShowFocusCues
        {
            get { return false; }
        }

        protected override bool ShowKeyboardCues
        {
            get { return false; }
        }

        public override void NotifyDefault(bool value)
        {
            base.NotifyDefault(false);
        }
    }
}
