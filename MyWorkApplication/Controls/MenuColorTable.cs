using System.Drawing;
using System.Windows.Forms;

namespace MyWorkApplication.Classes
{
    internal class MenuColorTable : ProfessionalColorTable
    {
        public MenuColorTable()
        {
            UseSystemColors = false;
        }

        public override Color MenuBorder => Color.White;

        public override Color MenuItemBorder => Color.White;

        public override Color MenuItemSelected => Color.FromArgb(217, 216, 214);

        public override Color MenuItemSelectedGradientBegin => Color.FromArgb(198, 198, 198);

        public override Color MenuItemSelectedGradientEnd => Color.FromArgb(198, 198, 198);

        public override Color MenuStripGradientBegin => Color.FromArgb(240, 240, 240);

        public override Color MenuStripGradientEnd => Color.FromArgb(240, 240, 240);
    }
}