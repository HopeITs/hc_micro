using System.Windows.Forms;

namespace MyWorkApplication.Classes
{
    internal static class ReportBinding
    {
        public static void Clear(BindingSource source)
        {
            // BindingSource otherwise tries to apply the old sort/filter to its new,
            // empty IBindingList, which has none of the previous table's properties.
            source.Sort = null;
            source.Filter = null;
            source.DataSource = null;
        }
    }
}
