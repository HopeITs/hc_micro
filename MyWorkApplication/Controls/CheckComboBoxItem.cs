namespace MyWorkApplication.Classes
{
    /// <summary>
    ///     This class wraps the list items in the combo box
    /// </summary>
    public class CheckComboBoxItem
    {
        /// <summary>
        ///     C'tor - creates a CheckComboBoxItem
        /// </summary>
        /// <param name="text">Label of the check box in the drop down list</param>
        /// <param name="initialCheckState">Initial value for the check box (true=checked)</param>
        public CheckComboBoxItem(string text, bool initialCheckState)
        {
            CheckState = initialCheckState;
            Text = text;
        }

        /// <summary>
        ///     Gets the check value (true=checked)
        /// </summary>
        public bool CheckState { get; set; }

        /// <summary>
        ///     Gets the label of the check box
        /// </summary>
        public string Text { get; set; } = "";

        /// <summary>
        ///     User defined data
        /// </summary>
        public object Tag { get; set; } = null;

        /// <summary>
        ///     This is used to keep the edit control portion of the combo box consistent
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Select Options";
        }
    }
}