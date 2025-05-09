using System.Drawing;
using System.Windows.Forms;

namespace MyWorkApplication.Classes
{
    public class MenuStripRenderer : ToolStripProfessionalRenderer
    {
        private readonly bool light;
        private readonly Color MainColor = Color.FromArgb(83, 83, 83);
        private readonly Color MainColor_light = Color.FromArgb(240, 240, 240);

        public MenuStripRenderer() : base(new MenuColorTable_Dark())
        {
            light = false;
        }

        public MenuStripRenderer(int i) : base(new MenuColorTable())
        {
            light = true;
        }

        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            if (light)
            {
                var rc = new Rectangle(Point.Empty, e.Item.Size);
                var c = e.Item.Selected ? Color.FromArgb(217, 216, 214) : Color.Transparent;
                using (var brush = new SolidBrush(c))
                {
                    e.Graphics.FillRectangle(brush, rc);
                }
            }
            else
            {
                var rc = new Rectangle(Point.Empty, e.Item.Size);
                var c = e.Item.Selected ? Color.FromArgb(75, 75, 75) : Color.Transparent;
                using (var brush = new SolidBrush(c))
                {
                    e.Graphics.FillRectangle(brush, rc);
                }
            }
        }

        protected override void OnRenderImageMargin(ToolStripRenderEventArgs e)
        {
            if (light)
                using (var brush = new SolidBrush(MainColor_light))
                {
                    e.Graphics.FillRectangle(brush, e.AffectedBounds);
                }
            else
                using (var brush = new SolidBrush(MainColor))
                {
                    e.Graphics.FillRectangle(brush, e.AffectedBounds);
                }
        }

        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            if (light)
            {
                e.Item.ForeColor = Color.Black;
                base.OnRenderItemText(e);
            }
            else
            {
                e.Item.ForeColor = Color.White;
                base.OnRenderItemText(e);
            }
        }

        protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
        {
            base.OnRenderSeparator(e);
        }
    }
}