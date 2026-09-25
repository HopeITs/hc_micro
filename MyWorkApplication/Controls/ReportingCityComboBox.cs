using System;
using System.ComponentModel;
using System.Linq;
using MyWorkApplication.Classes;

namespace MyWorkApplication.Controls
{
    // Uses the same checkbox popup and rendering as the existing filter comboboxes.
    public class ReportingCityComboBox : PresentationControls.CheckBoxComboBox
    {
        private const string AllLabel = "All permitted cities";
        private string[] permittedCities = new string[0];
        private bool updating;
        private bool pendingCommit;

        public ReportingCityComboBox()
        {
            CheckBoxCheckedChanged += SelectionChanged;
            dropDown.Closed += (sender, args) => CommitSelection();
        }

        public event EventHandler CitiesChanged;
        public event EventHandler CitiesCommitted;

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string[] SelectedCities
        {
            get
            {
                return permittedCities.Where(city => CheckBoxItems.Any(item => item.Checked && Equals(item.ComboBoxItem, city))).ToArray();
            }
        }

        public void InitializeCities(System.Collections.Generic.IEnumerable<string> permitted, string currentCity)
        {
            updating = true;
            try
            {
                permittedCities = CityReportingService.PermittedCities(permitted);
                Items.Clear();
                Items.Add(AllLabel);
                Items.AddRange(permittedCities);
                foreach (var item in CheckBoxItems)
                    item.Checked = Equals(item.ComboBoxItem, currentCity) && permittedCities.Contains(currentCity);
                UpdateAllCheck();
                // Refresh the hidden CSV item even when index zero was already selected.
                SelectedIndex = -1;
                SelectedIndex = 0;
                pendingCommit = false;
            }
            finally { updating = false; }
        }

        private void SelectionChanged(object sender, EventArgs args)
        {
            if (updating) return;
            updating = true;
            try
            {
                var changed = sender as PresentationControls.CheckBoxComboBoxItem;
                if (changed != null && Equals(changed.ComboBoxItem, AllLabel))
                {
                    foreach (var item in CheckBoxItems)
                        if (permittedCities.Contains(item.ComboBoxItem as string)) item.Checked = changed.Checked;
                }
                UpdateAllCheck();
                // Refresh the hidden CSV item even when index zero was already selected.
                SelectedIndex = -1;
                SelectedIndex = 0;
                pendingCommit = true;
            }
            finally { updating = false; }
            if (CitiesChanged != null) CitiesChanged(this, EventArgs.Empty);
        }

        private void UpdateAllCheck()
        {
            var all = CheckBoxItems.FirstOrDefault(item => Equals(item.ComboBoxItem, AllLabel));
            if (all != null) all.Checked = permittedCities.Length > 0 && SelectedCities.Length == permittedCities.Length;
        }

        // Query once when the user finishes checking cities, not once per checkbox.
        internal void CommitSelection()
        {
            if (!pendingCommit) return;
            pendingCommit = false;
            if (CitiesCommitted != null) CitiesCommitted(this, EventArgs.Empty);
        }
    }
}
