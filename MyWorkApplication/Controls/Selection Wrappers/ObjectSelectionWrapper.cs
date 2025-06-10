using System;
using System.ComponentModel;
using System.Data;

namespace MyWorkApplication.Classes.Selection_Wrappers
{
    /// <summary>
    ///     Used together with the ListSelectionWrapper in order to wrap data sources for a CheckBoxComboBox.
    ///     It helps to ensure you don't add an extra "Selected" property to a class that don't really need or want that
    ///     information.
    /// </summary>
    public class ObjectSelectionWrapper<T> : INotifyPropertyChanged
    {
        public ObjectSelectionWrapper(T item, ListSelectionWrapper<T> container)
        {
            _Container = container;
            _Item = item;
        }


        #region PRIVATE PROPERTIES

        /// <summary>
        ///     Used as a count indicator for the item. Not necessarily displayed.
        /// </summary>
        private int _Count;

        /// <summary>
        ///     Is this item selected.
        /// </summary>
        private bool _Selected;

        /// <summary>
        ///     A reference to the wrapped item.
        /// </summary>
        private T _Item;

        /// <summary>
        ///     The containing list for these selections.
        /// </summary>
        private readonly ListSelectionWrapper<T> _Container;

        #endregion

        #region PUBLIC PROPERTIES

        /// <summary>
        ///     An indicator of how many items with the specified status is available for the current filter level.
        ///     Thaught this would make the app a bit more user-friendly and help not to miss items in Statusses
        ///     that are not often used.
        /// </summary>
        public int Count
        {
            get => _Count;
            set => _Count = value;
        }

        /// <summary>
        ///     A reference to the item wrapped.
        /// </summary>
        public T Item
        {
            get => _Item;
            set => _Item = value;
        }

        /// <summary>
        ///     The item display value. If ShowCount is true, it displays the "Name [Count]".
        /// </summary>
        public string Name
        {
            get
            {
                string Name = null;
                if (string.IsNullOrEmpty(_Container.DisplayNameProperty))
                {
                    Name = Item.ToString();
                }
                else if (Item is DataRow) // A specific implementation for DataRow
                {
                    Name = ((DataRow)(object)Item)[_Container.DisplayNameProperty].ToString();
                }
                else
                {
                    var PDs = TypeDescriptor.GetProperties(Item);
                    foreach (PropertyDescriptor PD in PDs)
                        if (PD.Name.CompareTo(_Container.DisplayNameProperty) == 0)
                        {
                            Name = PD.GetValue(Item).ToString();
                            break;
                        }

                    if (string.IsNullOrEmpty(Name))
                    {
                        var PI = Item.GetType().GetProperty(_Container.DisplayNameProperty);
                        if (PI == null)
                            throw new InvalidOperationException(string.Format(
                                "Property {0} cannot be found on {1}.",
                                _Container.DisplayNameProperty,
                                Item.GetType()));
                        Name = PI.GetValue(Item, null).ToString();
                    }
                }

                return _Container.ShowCounts ? string.Format("{0} [{1}]", Name, Count) : Name;
            }
        }

        /// <summary>
        ///     The textbox display value. The names concatenated.
        /// </summary>
        public string NameConcatenated => _Container.SelectedNames;

        /// <summary>
        ///     Indicates whether the item is selected.
        /// </summary>
        public bool Selected
        {
            get => _Selected;
            set
            {
                if (_Selected != value)
                {
                    _Selected = value;
                    OnPropertyChanged("Selected");
                    OnPropertyChanged("NameConcatenated");
                }
            }
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}