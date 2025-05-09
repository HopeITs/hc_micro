using System.Collections.Generic;

namespace MyWorkApplication.Classes
{
    /// <summary>
    ///     Class used for demo purposes. This could be anything listed in a CheckBoxComboBox.
    /// </summary>
    public class Status
    {
        public Status(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        /// <summary>
        ///     Now used to return the Name.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Name;
        }
    }

    /// <summary>
    ///     Class used for demo purposes. A list of "Status".
    ///     This represents the custom "IList" datasource of anything listed in a CheckBoxComboBox.
    /// </summary>
    public class StatusList : List<Status>
    {
    }
}