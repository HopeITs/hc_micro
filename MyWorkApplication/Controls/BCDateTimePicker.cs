using MyWorkApplication.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace MyWorkApplication.Classes
{
    public class BCDateTimePicker : DateTimePicker
    {
        public BCDateTimePicker()
        {
            SetStyle(ControlStyles.UserPaint, true);
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override Color BackColor
        {
            get => base.BackColor;
            set => base.BackColor = value;
        }

        [Browsable(true)]
        public override Color ForeColor
        {
            get => base.ForeColor;
            set => base.ForeColor = value;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = CreateGraphics();
            var dropDownRectangle = new Rectangle(ClientRectangle.Width - 22, 0, 22, 33);
            Brush bkgBrush, ForeBrush;
            ComboBoxState visualState;

            if (Settings.Default.theme == "Light")
            {
                bkgBrush = new SolidBrush(Color.White);
                ForeBrush = new SolidBrush(Color.Black);
            }
            else
            {
                bkgBrush = new SolidBrush(Color.FromArgb(75, 75, 75));
                ForeBrush = new SolidBrush(Color.White);
            }

            if (Enabled) visualState = ComboBoxState.Normal;
            else visualState = ComboBoxState.Disabled;

            g.FillRectangle(bkgBrush, 0, 0, ClientRectangle.Width, ClientRectangle.Height);
            g.DrawString(Text, Font, ForeBrush, ClientRectangle.Width / 3, 2);

            ComboBoxRenderer.DrawDropDownButton(g, dropDownRectangle, visualState);

            g.Dispose();
            bkgBrush.Dispose();
            ForeBrush.Dispose();
        }

        protected override void OnValueChanged(EventArgs eventargs)
        {
            base.OnValueChanged(eventargs);
            Invalidate();
        }
    }
}