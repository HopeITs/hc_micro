using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyWorkApplication.Controls
{
    public class CalendarColumn : DataGridViewColumn
    {
        public CalendarColumn() : base(new CalendarCell()) { }

        public override DataGridViewCell CellTemplate
        {
            get { return base.CellTemplate; }
            set
            {
                if (value != null && !value.GetType().IsAssignableFrom(typeof(CalendarCell)))
                    throw new InvalidCastException("Must be a CalendarCell");
                base.CellTemplate = value;
            }
        }
    }

    public class CalendarCell : DataGridViewTextBoxCell
    {
        public CalendarCell() : base()
        {
            this.Style.Format = "dd-MM-yyyy"; // عرض التاريخ يوم-شهر-سنة
        }

        public override void InitializeEditingControl(int rowIndex, object initialFormattedValue,
            DataGridViewCellStyle dataGridViewCellStyle)
        {
            base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
            CalendarEditingControl ctl = DataGridView.EditingControl as CalendarEditingControl;
            if (this.Value == null || this.Value == DBNull.Value)
                ctl.Value = DateTime.Today;
            else
                ctl.Value = (DateTime)this.Value;
        }

        public override Type EditType => typeof(CalendarEditingControl);
        public override Type ValueType => typeof(DateTime);
        public override object DefaultNewRowValue => DateTime.Today;
    }

    class CalendarEditingControl : DateTimePicker, IDataGridViewEditingControl
    {
        DataGridView dataGridView;
        private bool valueChanged = false;
        int rowIndex;

        public CalendarEditingControl()
        {
            this.Format = DateTimePickerFormat.Custom;
            this.CustomFormat = "dd-MM-yyyy"; // صيغة التاريخ
        }

        public object EditingControlFormattedValue
        {
            get { return this.Value.ToString("dd-MM-yyyy"); }
            set
            {
                if (value is string s)
                {
                    this.Value = DateTime.TryParse(s, out DateTime dt) ? dt : DateTime.Today;
                }
            }
        }

        public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
            => EditingControlFormattedValue;

        public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle)
        {
            this.Font = dataGridViewCellStyle.Font;
            this.CalendarForeColor = dataGridViewCellStyle.ForeColor;
            this.CalendarMonthBackground = dataGridViewCellStyle.BackColor;
        }

        public int EditingControlRowIndex { get => rowIndex; set => rowIndex = value; }
        public bool EditingControlWantsInputKey(Keys key, bool dataGridViewWantsInputKey) => true;
        public void PrepareEditingControlForEdit(bool selectAll) { }
        public bool RepositionEditingControlOnValueChange => false;
        public DataGridView EditingControlDataGridView { get => dataGridView; set => dataGridView = value; }
        public bool EditingControlValueChanged { get => valueChanged; set => valueChanged = value; }
        public Cursor EditingPanelCursor => base.Cursor;

        protected override void OnValueChanged(EventArgs eventargs)
        {
            valueChanged = true;
            this.EditingControlDataGridView.NotifyCurrentCellDirty(true);
            base.OnValueChanged(eventargs);
        }
    }

}
