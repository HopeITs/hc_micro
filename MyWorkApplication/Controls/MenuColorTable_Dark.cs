using System.Drawing;
using System.Windows.Forms;

namespace MyWorkApplication.Classes
{
    internal class MenuColorTable_Dark : ProfessionalColorTable
    {
        public MenuColorTable_Dark()
        {
            UseSystemColors = false;
        }

        public override Color ToolStripDropDownBackground => Color.FromArgb(83, 83, 83);

        public override Color ImageMarginGradientBegin => Color.FromArgb(83, 83, 83);

        public override Color ImageMarginGradientMiddle => Color.FromArgb(83, 83, 83);

        public override Color ImageMarginGradientEnd => Color.FromArgb(83, 83, 83);

        public override Color MenuBorder => Color.FromArgb(83, 83, 83);

        public override Color MenuItemBorder => Color.FromArgb(83, 83, 83);

        public override Color MenuItemSelected => Color.FromArgb(75, 75, 75);

        public override Color MenuItemSelectedGradientBegin => Color.FromArgb(75, 75, 75);

        public override Color MenuItemSelectedGradientEnd => Color.FromArgb(75, 75, 75);

        public override Color MenuStripGradientBegin => Color.FromArgb(83, 83, 83);

        public override Color MenuStripGradientEnd => Color.FromArgb(83, 83, 83);
    }
}