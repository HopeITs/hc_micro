using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Bunifu.Framework.UI;
using MyWorkApplication.Properties;
using TabControl = Jacksiro.MdiTab.TabControl;
using Zuby.ADGV;

namespace MyWorkApplication.Classes
{
    public class NewTheme
    {
        public void Visit_User_ToNight(Form form) {
            foreach (Control f in form.Controls)
                if (f.GetType() == typeof(TableLayoutPanel))  
                {
                    f.BackColor = Color.FromArgb(96, 96, 96);
                    f.ForeColor = Color.White;
                    foreach (Control c in f.Controls)
                    {
                        if (c.GetType() == typeof(TableLayoutPanel)) /// inner table
                        {
                            c.BackColor = Color.FromArgb(96, 96, 96);
                            c.ForeColor = Color.White;

                            foreach (Control aa in c.Controls)
                                if (aa.GetType() == typeof(Label) || aa.GetType() == typeof(Button) ||
                                    aa.GetType() == typeof(CheckBox) || aa.GetType() == typeof(RadioButton))
                                {
                                    aa.ForeColor = Color.White;
                                    aa.BackColor = Color.Transparent;
                                    if (aa.Name.Contains("Header"))
                                    {
                                        aa.BackColor = Color.FromArgb(189, 189, 189);
                                        aa.ForeColor = Color.FromArgb(75, 75, 75);
                                    }
                                }
                                else if (aa.GetType() == typeof(TextBox) || aa.GetType() == typeof(ComboBox))
                                {
                                    aa.BackColor = Color.FromArgb(75, 75, 75);
                                    aa.ForeColor = Color.White;
                                }
                             
                        }
                        else if (c.GetType() == typeof(AdvancedDataGridView))
                        {
                            var MP_dataGridView = c as AdvancedDataGridView;
                            MP_dataGridView.BackgroundColor =
                                MP_dataGridView.ColumnHeadersDefaultCellStyle.BackColor =
                                    MP_dataGridView.DefaultCellStyle.BackColor =
                                        MP_dataGridView.RowsDefaultCellStyle.BackColor =
                                            Color.FromArgb(75, 75, 75);
                            MP_dataGridView.AlternatingRowsDefaultCellStyle.BackColor =
                                Color.FromArgb(96, 96, 96);

                            MP_dataGridView.ForeColor =
                                MP_dataGridView.ColumnHeadersDefaultCellStyle.ForeColor =
                                    MP_dataGridView.DefaultCellStyle.ForeColor =
                                        MP_dataGridView.RowsDefaultCellStyle.ForeColor =
                                            MP_dataGridView.AlternatingRowsDefaultCellStyle.ForeColor =
                                                Color.FromArgb(189, 189, 189);

                            MP_dataGridView.GridColor = Color.FromArgb(189, 189, 189);

                            MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor =
                                MP_dataGridView.DefaultCellStyle.SelectionBackColor
                                    = MP_dataGridView.RowsDefaultCellStyle.SelectionBackColor =
                                        MP_dataGridView.AlternatingRowsDefaultCellStyle.SelectionBackColor =
                                            Color.FromArgb(56, 56, 56);
                            MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionForeColor =
                                MP_dataGridView.DefaultCellStyle.SelectionForeColor
                                    = MP_dataGridView.RowsDefaultCellStyle.SelectionForeColor =
                                        MP_dataGridView.AlternatingRowsDefaultCellStyle.SelectionForeColor =
                                            Color.FromArgb(189, 189, 189);

                            for (var i = 0; i < MP_dataGridView.Columns.Count; i++)
                            {
                                MP_dataGridView.Columns[i].HeaderCell.Style.BackColor =
                                    Color.FromArgb(75, 75, 75);
                                MP_dataGridView.Columns[i].HeaderCell.Style.ForeColor =
                                    Color.FromArgb(189, 189, 189);
                            }
                        }
                        else if (c.GetType() == typeof(AdvancedDataGridViewSearchToolBar))
                        {
                            //var searchToolBar = c as AdvancedDataGridViewSearchToolBar;
                            //searchToolBar.BackColor = Color.FromArgb(75, 75, 75);
                            //searchToolBar.ForeColor = Color.White;

                            c.BackColor = Color.FromArgb(96, 96, 96);
                            c.ForeColor = Color.White;
                            
                            foreach (Control aa in c.Controls)
                                if (aa.GetType() == typeof(Label) || aa.GetType() == typeof(Button) ||
                                    aa.GetType() == typeof(CheckBox) || aa.GetType() == typeof(RadioButton))
                                {
                                    aa.ForeColor = Color.White;
                                    aa.BackColor = Color.Transparent;
                                }
                                else if (aa.GetType() == typeof(TextBox) || aa.GetType() == typeof(ComboBox))
                                {
                                    aa.BackColor = Color.FromArgb(75, 75, 75);
                                    aa.ForeColor = Color.White;
                                } 
                        }
                    }
                }

            form.Refresh();
        }
        public void Visit_User_ToLight(Form form) {
            foreach (Control f in form.Controls)
                if (f.GetType() == typeof(TableLayoutPanel))  
                {
                    f.BackColor = Color.FromArgb(240, 240, 240);
                    f.ForeColor = Color.Black;
                    foreach (Control c in f.Controls)
                    {
                        if (c.GetType() == typeof(TableLayoutPanel)) /// inner table
                        {
                            c.BackColor = Color.FromArgb(240, 240, 240);
                            c.ForeColor = Color.Black;

                            foreach (Control aa in c.Controls)
                                if (aa.GetType() == typeof(Label) || aa.GetType() == typeof(Button) ||
                                    aa.GetType() == typeof(CheckBox) || aa.GetType() == typeof(RadioButton))
                                {
                                    aa.ForeColor = Color.Black;
                                    aa.BackColor = Color.Transparent;
                                }
                                else if (aa.GetType() == typeof(TextBox) || aa.GetType() == typeof(ComboBox))
                                {
                                    aa.BackColor = Color.White;
                                    aa.ForeColor = Color.Black;
                                }


                        }
                        else if (c.GetType() == typeof(AdvancedDataGridView))
                        {
                            var MP_dataGridView = c as AdvancedDataGridView;
                            MP_dataGridView.BackgroundColor =
                                MP_dataGridView.ColumnHeadersDefaultCellStyle.BackColor =
                                    MP_dataGridView.DefaultCellStyle.BackColor =
                                        MP_dataGridView.RowsDefaultCellStyle.BackColor = Color.White;
                            MP_dataGridView.AlternatingRowsDefaultCellStyle.BackColor =
                                Color.FromArgb(240, 240, 240);

                            MP_dataGridView.ForeColor =
                                MP_dataGridView.ColumnHeadersDefaultCellStyle.ForeColor =
                                    MP_dataGridView.DefaultCellStyle.ForeColor =
                                        MP_dataGridView.RowsDefaultCellStyle.ForeColor =
                                            MP_dataGridView.AlternatingRowsDefaultCellStyle.ForeColor =
                                                Color.Black;

                            MP_dataGridView.GridColor = Color.FromArgb(96, 96, 96);

                            MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor =
                                MP_dataGridView.DefaultCellStyle.SelectionBackColor
                                    = MP_dataGridView.RowsDefaultCellStyle.SelectionBackColor =
                                        MP_dataGridView.AlternatingRowsDefaultCellStyle.SelectionBackColor =
                                            Color.FromArgb(189, 189, 189);
                            MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionForeColor =
                                MP_dataGridView.DefaultCellStyle.SelectionForeColor
                                    = MP_dataGridView.RowsDefaultCellStyle.SelectionForeColor =
                                        MP_dataGridView.AlternatingRowsDefaultCellStyle.SelectionForeColor =
                                            Color.Black;

                            for (var i = 0; i < MP_dataGridView.Columns.Count; i++)
                            {
                                MP_dataGridView.Columns[i].HeaderCell.Style.BackColor = Color.White;
                                MP_dataGridView.Columns[i].HeaderCell.Style.ForeColor = Color.Black;
                            }
                        }
                        else if (c.GetType() == typeof(AdvancedDataGridViewSearchToolBar))
                        {
                            //var searchToolBar = c as AdvancedDataGridViewSearchToolBar;
                            //searchToolBar.BackColor = Color.White;
                            //searchToolBar.ForeColor = Color.Black;

                            c.BackColor = Color.FromArgb(240, 240, 240);
                            c.ForeColor = Color.Black;

                            foreach (Control aa in c.Controls)
                                if (aa.GetType() == typeof(Label) || aa.GetType() == typeof(Button) ||
                                    aa.GetType() == typeof(CheckBox) || aa.GetType() == typeof(RadioButton))
                                {
                                    aa.ForeColor = Color.Black;
                                    aa.BackColor = Color.Transparent;
                                }
                                else if (aa.GetType() == typeof(TextBox) || aa.GetType() == typeof(ComboBox))
                                {
                                    aa.BackColor = Color.White;
                                    aa.ForeColor = Color.Black;
                                }
                        }

                    }
                }

            form.Refresh();
        }

        private ToolStripItem[] GetAllChildren(ToolStripItem item)
        {
            var Items = new List<ToolStripItem> {item};
            if (item is ToolStripMenuItem)
                foreach (ToolStripItem i in ((ToolStripMenuItem) item).DropDownItems)
                    Items.AddRange(GetAllChildren(i));
            else if (item is ToolStripSplitButton)
                foreach (ToolStripItem i in ((ToolStripSplitButton) item).DropDownItems)
                    Items.AddRange(GetAllChildren(i));
            else if (item is ToolStripDropDownButton)
                foreach (ToolStripItem i in ((ToolStripDropDownButton) item).DropDownItems)
                    Items.AddRange(GetAllChildren(i));
            return Items.ToArray();
        }

        public void Main_ToNight(Form form, bool has_notification)
        {
            form.BackColor = Color.FromArgb(83, 83, 83);
            form.ForeColor = Color.White;
            foreach (Control c in form.Controls)
                if (c.GetType() == typeof(Panel))
                {
                    if (c.Name == "TopLine_panel" || c.Name == "BottomLine_panel" || c.Name == "panel3")
                    {
                        c.ForeColor = Color.Black;
                        c.BackColor = Color.Black;
                        continue;
                    } 

                        c.BackColor = Color.FromArgb(83, 83, 83);
                        c.ForeColor = Color.White;

                        foreach (Control aa in c.Controls)
                            if (aa.GetType() == typeof(Label) || aa.GetType() == typeof(Button) ||
                                aa.GetType() == typeof(CheckBox))
                            {
                                aa.ForeColor = Color.White;
                                if (aa.Name == "Notifications_button" && has_notification)
                                    aa.BackgroundImage = Resources.Red_Bell_DD;
                                else if (aa.Name == "Notifications_button" && !has_notification)
                                    aa.BackgroundImage = Resources.Bell_DD;
                                else if (aa.Name == "X_button")
                                    aa.BackgroundImage = Resources.X_Dark;
                                else if (aa.Name == "R_button")
                                    aa.BackgroundImage = Resources.R_Dark;
                                else if (aa.Name == "M_button")
                                    aa.BackgroundImage = Resources.M_Dark;
                            }
                            else if (aa.GetType() == typeof(PictureBox))
                            {
                                if (aa.Name == "Logo_Large_pictureBox")
                                    aa.BackgroundImage = Resources.Logo_Large_D;
                            }
                            else if (aa.GetType() == typeof(TableLayoutPanel))
                            {
                                aa.BackColor = Color.FromArgb(83, 83, 83);
                                aa.ForeColor = Color.White;
                                foreach (Control bb in aa.Controls)
                                    if (bb.GetType() == typeof(Label))
                                        bb.ForeColor = Color.White;

                                    else if (bb.Name == "Logo_Small_pictureBox")
                                        bb.BackgroundImage = Resources.Logo_Small_D;
                            } 
                }
                else if (c.GetType() == typeof(PictureBox))
                {
                    c.BackColor = Color.FromArgb(83, 83, 83);
                    c.ForeColor = Color.White;
                    if (c.Name == "Home_panel")
                    {
                        c.ForeColor = Color.Black;
                        c.BackColor = Color.White;
                        continue;
                    }
                }
                else if (c.GetType() == typeof(MenuStrip))
                {
                    var menuStrip1 = c as MenuStrip;
                    menuStrip1.Renderer = new MenuStripRenderer();
                    menuStrip1.BackColor = Color.FromArgb(83, 83, 83);
                    menuStrip1.ForeColor = Color.White;
                }
                else if (c is TabControl)
                {
                    var tabControl = c as TabControl;
                    tabControl.BackColor = Color.FromArgb(31, 31, 31);
                    tabControl.BackHighColor = Color.FromArgb(31, 31, 31);
                    tabControl.BackLowColor = Color.FromArgb(31, 31, 31);

                    tabControl.BorderColor = Color.FromArgb(83, 83, 83);
                    tabControl.BorderColorDisabled = Color.FromArgb(83, 83, 83);

                    tabControl.ControlButtonBackHighColor = Color.FromArgb(83, 83, 83);
                    tabControl.ControlButtonBackLowColor = Color.FromArgb(83, 83, 83);
                    tabControl.ControlButtonBorderColor = Color.FromArgb(189, 189, 189);
                    tabControl.ControlButtonForeColor = Color.FromArgb(189, 189, 189);

                    tabControl.ForeColor = Color.FromArgb(189, 189, 189);
                    tabControl.ForeColorDisabled = Color.FromArgb(189, 189, 189);

                    tabControl.TabBackHighColor = Color.FromArgb(83, 83, 83);
                    tabControl.TabBackHighColorDisabled = Color.FromArgb(50, 50, 50);
                    tabControl.TabBackLowColor = Color.FromArgb(83, 83, 83);
                    tabControl.TabBackLowColorDisabled = Color.FromArgb(50, 50, 50);
                    tabControl.TabCloseButtonImage = Resources.Exit_L;
                    tabControl.TabCloseButtonImageDisabled = Resources.Exit_L;
                    tabControl.TabCloseButtonImageHot = Resources.Exit_L;
                }

            form.Refresh();
        }

        public void Main_ToLight(Form form, bool has_notification)
        {
            form.BackColor = Color.FromArgb(240, 240, 240);
            form.ForeColor = Color.Black;
            foreach (Control c in form.Controls)
                if (c.GetType() == typeof(Panel))
                {
                    if (c.Name == "TopLine_panel" || c.Name == "BottomLine_panel" || c.Name == "panel3")
                    {
                        c.ForeColor = Color.FromArgb(189, 189, 189);
                        c.BackColor = Color.FromArgb(189, 189, 189);
                        continue;
                    }
                     
                        c.BackColor = Color.FromArgb(240, 240, 240);
                        c.ForeColor = Color.Black;

                        foreach (Control aa in c.Controls)
                            if (aa.GetType() == typeof(Label) || aa.GetType() == typeof(Button) ||
                                aa.GetType() == typeof(CheckBox))
                            {
                                aa.ForeColor = Color.Black;

                                if (aa.Name == "Notifications_button" && has_notification)
                                    aa.BackgroundImage = Resources.Bell_Red_L;
                                else if (aa.Name == "Notifications_button" && !has_notification)
                                    aa.BackgroundImage = Resources.Bell_L;
                                else if (aa.Name == "X_button")
                                    aa.BackgroundImage = Resources.X_Light;
                                else if (aa.Name == "R_button")
                                    aa.BackgroundImage = Resources.R_Light;
                                else if (aa.Name == "M_button")
                                    aa.BackgroundImage = Resources.M_Light;
                            }
                            else if (aa.GetType() == typeof(PictureBox))
                            {
                                if (aa.Name == "Logo_Large_pictureBox")
                                    aa.BackgroundImage = Resources.Logo_Large_L;
                            }
                            else if (aa.GetType() == typeof(TableLayoutPanel))
                            {
                                aa.BackColor = Color.FromArgb(240, 240, 240);
                                aa.ForeColor = Color.Black;
                                foreach (Control bb in aa.Controls)
                                    if (bb.GetType() == typeof(Label))
                                        bb.ForeColor = Color.Black;
                                    else if (bb.Name == "Logo_Small_pictureBox")
                                        bb.BackgroundImage = Resources.Logo_Small_L;
                            } 
                }
                else if (c.GetType() == typeof(PictureBox))
                {
                    c.BackColor = Color.FromArgb(240, 240, 240);
                    c.ForeColor = Color.Black;
                    if (c.Name == "Home_panel")
                    {
                        c.ForeColor = Color.Black;
                        c.BackColor = Color.White;
                        continue;
                    }
                }
                else if (c.GetType() == typeof(MenuStrip))
                {
                    var menuStrip1 = c as MenuStrip;
                    menuStrip1.Renderer = new MenuStripRenderer(0);
                    menuStrip1.BackColor = Color.FromArgb(240, 240, 240);
                    menuStrip1.ForeColor = Color.Black;
                }
                else if (c is TabControl)
                {
                    var tabControl = c as TabControl;
                    tabControl.BackColor = Color.FromArgb(189, 189, 189);
                    tabControl.BackHighColor = Color.FromArgb(189, 189, 189);
                    tabControl.BackLowColor = Color.FromArgb(189, 189, 189);

                    tabControl.BorderColor = Color.FromArgb(240, 240, 240);
                    tabControl.BorderColorDisabled = Color.FromArgb(240, 240, 240);

                    tabControl.ControlButtonBackHighColor = Color.FromArgb(240, 240, 240);
                    tabControl.ControlButtonBackLowColor = Color.FromArgb(240, 240, 240);
                    tabControl.ControlButtonBorderColor = Color.FromArgb(48, 48, 48);
                    tabControl.ControlButtonForeColor = Color.FromArgb(48, 48, 48);

                    tabControl.ForeColor = Color.Black;
                    tabControl.ForeColorDisabled = Color.Black;

                    tabControl.TabBackHighColor = Color.FromArgb(240, 240, 240);
                    tabControl.TabBackHighColorDisabled = Color.FromArgb(217, 216, 214);
                    tabControl.TabBackLowColor = Color.FromArgb(240, 240, 240);
                    tabControl.TabBackLowColorDisabled = Color.FromArgb(217, 216, 214);

                    //tabControl.TabCloseButtonBackHighColor = Color.FromArgb(48, 48, 48);
                    //tabControl.TabCloseButtonBackHighColorDisabled = Color.FromArgb(48, 48, 48);
                    //tabControl.TabCloseButtonBackHighColorHot = Color.FromArgb(48, 48, 48);
                    //tabControl.TabCloseButtonBackLowColor = Color.FromArgb(48, 48, 48);
                    //tabControl.TabCloseButtonBackLowColorDisabled = Color.FromArgb(48, 48, 48);
                    //tabControl.TabCloseButtonBackLowColorHot = Color.FromArgb(48, 48, 48);
                    //tabControl.TabCloseButtonBorderColor = Color.FromArgb(48, 48, 48);
                    //tabControl.TabCloseButtonBorderColorDisabled = Color.FromArgb(48, 48, 48);
                    //tabControl.TabCloseButtonBorderColorHot = Color.FromArgb(48, 48, 48);
                    //tabControl.TabCloseButtonForeColor = Color.FromArgb(48, 48, 48);
                    //tabControl.TabCloseButtonForeColorDisabled = Color.FromArgb(48, 48, 48);
                    //tabControl.TabCloseButtonForeColorHot = Color.FromArgb(48, 48, 48);

                    tabControl.TabCloseButtonImage = Resources.Exit_D;
                    tabControl.TabCloseButtonImageDisabled = Resources.Exit_D;
                    tabControl.TabCloseButtonImageHot = Resources.Exit_D;
                }

            form.Refresh();
        }

        public void Application_ToNight(Form form)
        {
            foreach (Control f in form.Controls)
                if (f.GetType() == typeof(Panel)) /// main panel
                {
                    f.BackColor = Color.FromArgb(96, 96, 96);
                    f.ForeColor = Color.White;
                    foreach (Control c in f.Controls)
                        if (c.GetType() == typeof(TableLayoutPanel)) /// first & second
                        {
                            c.BackColor = Color.FromArgb(96, 96, 96);
                            c.ForeColor = Color.White;

                            foreach (Control aa in c.Controls)
                                if (aa.GetType() == typeof(Label) || aa.GetType() == typeof(Button) ||
                                    aa.GetType() == typeof(CheckBox) || aa.GetType() == typeof(RadioButton))
                                {
                                    aa.ForeColor = Color.White;
                                    if (aa.Name == "SaveFamily_button" || aa.Name == "SaveWork_button")
                                    {
                                        aa.BackgroundImage = Resources.Save2_L;
                                    }
                                    else if (aa.Name.Contains("Header"))
                                    {
                                        aa.BackColor = Color.FromArgb(189, 189, 189);
                                        aa.ForeColor = Color.FromArgb(75, 75, 75);
                                    }
                                    else if (aa.Name == "OverallSyrian_label")
                                    {
                                        aa.BackColor = Color.FromArgb(75, 75, 75);
                                    }
                                }
                                else if (aa.GetType() == typeof(TextBox) || aa.GetType() == typeof(ComboBox))
                                {
                                    aa.BackColor = Color.FromArgb(75, 75, 75);
                                    aa.ForeColor = Color.White;
                                }
                                else if (aa.GetType() == typeof(BCDateTimePicker))
                                {
                                    var dd = aa as BCDateTimePicker;
                                    dd.BackColor = Color.FromArgb(75, 75, 75);
                                    dd.ForeColor = Color.White;
                                }
                                else if (aa.GetType() == typeof(DataGridView))
                                {
                                    var MP_dataGridView = aa as DataGridView;
                                    MP_dataGridView.BackgroundColor =
                                        MP_dataGridView.ColumnHeadersDefaultCellStyle.BackColor =
                                            MP_dataGridView.DefaultCellStyle.BackColor =
                                                MP_dataGridView.RowsDefaultCellStyle.BackColor =
                                                    Color.FromArgb(75, 75, 75);
                                    MP_dataGridView.ForeColor =
                                        MP_dataGridView.ColumnHeadersDefaultCellStyle.ForeColor =
                                            MP_dataGridView.DefaultCellStyle.ForeColor =
                                                MP_dataGridView.RowsDefaultCellStyle.ForeColor = Color.White;

                                    MP_dataGridView.GridColor = Color.FromArgb(189, 189, 189);

                                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor =
                                        MP_dataGridView.DefaultCellStyle.SelectionBackColor
                                            = MP_dataGridView.RowsDefaultCellStyle.SelectionBackColor =
                                                Color.FromArgb(96, 96, 96);
                                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionForeColor =
                                        MP_dataGridView.DefaultCellStyle.SelectionForeColor
                                            = MP_dataGridView.RowsDefaultCellStyle.SelectionForeColor = Color.White;


                                    for (var i = 0; i < MP_dataGridView.Columns.Count; i++)
                                    {
                                        MP_dataGridView.Columns[i].HeaderCell.Style.BackColor =
                                            Color.FromArgb(75, 75, 75);
                                        MP_dataGridView.Columns[i].HeaderCell.Style.ForeColor = Color.White;
                                        for (var j = 0; j < MP_dataGridView.Rows.Count; j++)
                                        {
                                            MP_dataGridView.Rows[j].Cells[i].Style.BackColor =
                                                Color.FromArgb(75, 75, 75);
                                            MP_dataGridView.Rows[j].Cells[i].Style.ForeColor = Color.White;
                                        }
                                    }
                                }
                                else if (aa.GetType() == typeof(TableLayoutPanel))
                                {
                                    aa.BackColor = Color.FromArgb(96, 96, 96);
                                    aa.ForeColor = Color.White;
                                    foreach (Control bb in aa.Controls)
                                        if (bb.GetType() == typeof(Label) || bb.GetType() == typeof(Button) ||
                                            bb.GetType() == typeof(CheckBox) || bb.GetType() == typeof(RadioButton))
                                        {
                                            bb.ForeColor = Color.White;
                                            bb.BackColor = Color.Transparent;
                                            if (bb.Name == "Save_Beneficiary_Update_button" ||
                                                bb.Name == "Save_MP_Update_button")
                                                bb.BackgroundImage = Resources.Save2_L;
                                            else if (bb.Name == "Delete_MP_button" ||
                                                     bb.Name == "Delete_Beneficiary_button")
                                                bb.BackgroundImage = Resources.Delete2_L;
                                            else if (bb.Name == "AddPriest_button" ||
                                                     bb.Name == "NewEducation_button" ||
                                                     bb.Name == "NewLanguage_button" || bb.Name == "NewSkill_button" ||
                                                     bb.Name == "NewWork_button")
                                                bb.BackgroundImage = Resources.Plus_Sq_L_24;
                                            else if (bb.Name == "AddImage_button")
                                                bb.BackgroundImage = Resources.Plus_Sq_L;
                                            else if (bb.Name == "DeleteImage_button")
                                                bb.BackgroundImage = Resources.Trash_L;
                                        }
                                        else if (bb.GetType() == typeof(TextBox) || bb.GetType() == typeof(ComboBox))
                                        {
                                            bb.BackColor = Color.FromArgb(75, 75, 75);
                                            bb.ForeColor = Color.White;
                                        }
                                        else if (bb.GetType() == typeof(BCDateTimePicker))
                                        {
                                            var dd = bb as BCDateTimePicker;
                                            dd.BackColor = Color.FromArgb(75, 75, 75);
                                            dd.ForeColor = Color.White;
                                        }
                                        else if (bb.GetType() == typeof(TableLayoutPanel))
                                        {
                                            bb.BackColor = Color.FromArgb(96, 96, 96);
                                            bb.ForeColor = Color.White;
                                            foreach (Control zx in bb.Controls)
                                                if (zx.GetType() == typeof(Label) || zx.GetType() == typeof(Button) ||
                                                    zx.GetType() == typeof(CheckBox) ||
                                                    zx.GetType() == typeof(RadioButton))
                                                {
                                                    zx.ForeColor = Color.White;
                                                }
                                                else if (zx.GetType() == typeof(TextBox) ||
                                                         zx.GetType() == typeof(ComboBox))
                                                {
                                                    zx.BackColor = Color.FromArgb(75, 75, 75);
                                                    zx.ForeColor = Color.White;
                                                }
                                        }
                                }
                                else if (aa.GetType() == typeof(GroupBox))
                                {
                                    aa.BackColor = Color.FromArgb(96, 96, 96);
                                    aa.ForeColor = Color.White;
                                    foreach (Control bb in aa.Controls)
                                        if (bb.GetType() == typeof(TableLayoutPanel))
                                        {
                                            bb.BackColor = Color.FromArgb(96, 96, 96);
                                            bb.ForeColor = Color.White;
                                            foreach (Control mm in bb.Controls)
                                                if (mm.GetType() == typeof(Label) || mm.GetType() == typeof(Button) ||
                                                    mm.GetType() == typeof(CheckBox) ||
                                                    mm.GetType() == typeof(RadioButton))
                                                {
                                                    mm.ForeColor = Color.White;
                                                }
                                                else if (mm.GetType() == typeof(TextBox) ||
                                                         mm.GetType() == typeof(ComboBox))
                                                {
                                                    mm.BackColor = Color.FromArgb(75, 75, 75);
                                                    mm.ForeColor = Color.White;
                                                }
                                        }
                                }
                        }
                }
        }

        public void Application_ToLight(Form form)
        {
            foreach (Control f in form.Controls)
                if (f.GetType() == typeof(Panel)) /// main panel
                {
                    f.BackColor = Color.FromArgb(240, 240, 240);
                    f.ForeColor = Color.Black;
                    foreach (Control c in f.Controls)
                        if (c.GetType() == typeof(TableLayoutPanel)) /// first & second
                        {
                            c.BackColor = Color.FromArgb(240, 240, 240);
                            c.ForeColor = Color.Black;

                            foreach (Control aa in c.Controls)
                                if (aa.GetType() == typeof(Label) || aa.GetType() == typeof(Button) ||
                                    aa.GetType() == typeof(CheckBox) || aa.GetType() == typeof(RadioButton))
                                {
                                    aa.ForeColor = Color.Black;
                                    if (aa.Name == "SaveFamily_button" || aa.Name == "SaveWork_button")
                                    {
                                        aa.BackgroundImage = Resources.Save2_D;
                                    }
                                    else if (aa.Name.Contains("Header"))
                                    {
                                        aa.BackColor = Color.FromArgb(96, 96, 96);
                                        aa.ForeColor = Color.White;
                                    }
                                    else if (aa.Name == "OverallSyrian_label")
                                    {
                                        aa.BackColor = Color.White;
                                    }
                                }
                                else if (aa.GetType() == typeof(TextBox) || aa.GetType() == typeof(ComboBox))
                                {
                                    aa.BackColor = Color.White;
                                    aa.ForeColor = Color.Black;
                                }
                                else if (aa.GetType() == typeof(BCDateTimePicker))
                                {
                                    var dd = aa as BCDateTimePicker;
                                    dd.BackColor = Color.White;
                                    dd.ForeColor = Color.Black;
                                }
                                else if (aa.GetType() == typeof(DataGridView))
                                {
                                    var MP_dataGridView = aa as DataGridView;
                                    MP_dataGridView.BackgroundColor =
                                        MP_dataGridView.ColumnHeadersDefaultCellStyle.BackColor =
                                            MP_dataGridView.DefaultCellStyle.BackColor =
                                                MP_dataGridView.RowsDefaultCellStyle.BackColor = Color.White;

                                    MP_dataGridView.ForeColor =
                                        MP_dataGridView.ColumnHeadersDefaultCellStyle.ForeColor =
                                            MP_dataGridView.DefaultCellStyle.ForeColor =
                                                MP_dataGridView.RowsDefaultCellStyle.ForeColor = Color.Black;

                                    MP_dataGridView.GridColor = Color.FromArgb(96, 96, 96);

                                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor =
                                        MP_dataGridView.DefaultCellStyle.SelectionBackColor
                                            = MP_dataGridView.RowsDefaultCellStyle.SelectionBackColor =
                                                Color.FromArgb(189, 189, 189);
                                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionForeColor =
                                        MP_dataGridView.DefaultCellStyle.SelectionForeColor
                                            = MP_dataGridView.RowsDefaultCellStyle.SelectionForeColor = Color.Black;

                                    for (var i = 0; i < MP_dataGridView.Columns.Count; i++)
                                    {
                                        MP_dataGridView.Columns[i].HeaderCell.Style.BackColor = Color.White;
                                        MP_dataGridView.Columns[i].HeaderCell.Style.ForeColor = Color.Black;
                                        for (var j = 0; j < MP_dataGridView.Rows.Count; j++)
                                        {
                                            MP_dataGridView.Rows[j].Cells[i].Style.BackColor = Color.White;
                                            MP_dataGridView.Rows[j].Cells[i].Style.ForeColor = Color.Black;
                                        }
                                    }
                                }
                                else if (aa.GetType() == typeof(TableLayoutPanel))
                                {
                                    aa.BackColor = Color.FromArgb(240, 240, 240);
                                    aa.ForeColor = Color.Black;
                                    foreach (Control bb in aa.Controls)
                                        if (bb.GetType() == typeof(Label) || bb.GetType() == typeof(Button) ||
                                            bb.GetType() == typeof(CheckBox) || bb.GetType() == typeof(RadioButton))
                                        {
                                            bb.ForeColor = Color.Black;
                                            bb.BackColor = Color.Transparent;

                                            if (bb.Name == "Save_Beneficiary_Update_button" ||
                                                bb.Name == "Save_MP_Update_button")
                                                bb.BackgroundImage = Resources.Save2_D;
                                            else if (bb.Name == "Delete_MP_button" ||
                                                     bb.Name == "Delete_Beneficiary_button")
                                                bb.BackgroundImage = Resources.Delete2_D;
                                            else if (bb.Name == "AddPriest_button" ||
                                                     bb.Name == "NewEducation_button" ||
                                                     bb.Name == "NewLanguage_button" || bb.Name == "NewSkill_button" ||
                                                     bb.Name == "NewWork_button")
                                                bb.BackgroundImage = Resources.Plus_Sq_D_24;
                                            else if (bb.Name == "AddImage_button")
                                                bb.BackgroundImage = Resources.Plus_Sq_D;
                                            else if (bb.Name == "DeleteImage_button")
                                                bb.BackgroundImage = Resources.Trash_D;
                                        }
                                        else if (bb.GetType() == typeof(TextBox) || bb.GetType() == typeof(ComboBox))
                                        {
                                            bb.BackColor = Color.White;
                                            bb.ForeColor = Color.Black;
                                        }
                                        else if (bb.GetType() == typeof(BCDateTimePicker))
                                        {
                                            var dd = bb as BCDateTimePicker;
                                            dd.BackColor = Color.White;
                                            dd.ForeColor = Color.Black;
                                        }
                                        else if (bb.GetType() == typeof(TableLayoutPanel))
                                        {
                                            bb.BackColor = Color.FromArgb(240, 240, 240);
                                            bb.ForeColor = Color.Black;
                                            foreach (Control zx in bb.Controls)
                                                if (zx.GetType() == typeof(Label) || zx.GetType() == typeof(Button) ||
                                                    zx.GetType() == typeof(CheckBox) ||
                                                    zx.GetType() == typeof(RadioButton))
                                                {
                                                    zx.ForeColor = Color.Black;
                                                }
                                                else if (zx.GetType() == typeof(TextBox) ||
                                                         zx.GetType() == typeof(ComboBox))
                                                {
                                                    zx.BackColor = Color.White;
                                                    zx.ForeColor = Color.Black;
                                                }
                                        }
                                }
                                else if (aa.GetType() == typeof(GroupBox))
                                {
                                    aa.BackColor = Color.FromArgb(240, 240, 240);
                                    aa.ForeColor = Color.Black;
                                    foreach (Control bb in aa.Controls)
                                        if (bb.GetType() == typeof(TableLayoutPanel))
                                        {
                                            bb.BackColor = Color.FromArgb(240, 240, 240);
                                            bb.ForeColor = Color.Black;
                                            foreach (Control zx in bb.Controls)
                                                if (zx.GetType() == typeof(Label) || zx.GetType() == typeof(Button) ||
                                                    zx.GetType() == typeof(CheckBox) ||
                                                    zx.GetType() == typeof(RadioButton))
                                                {
                                                    zx.ForeColor = Color.Black;
                                                }
                                                else if (zx.GetType() == typeof(TextBox) ||
                                                         zx.GetType() == typeof(ComboBox))
                                                {
                                                    zx.BackColor = Color.White;
                                                    zx.ForeColor = Color.Black;
                                                }
                                        }
                                }
                        }
                }
        }

        public void Visit_ToNight(Form form)
        {
            foreach (Control f in form.Controls)
                if (f.GetType() == typeof(Panel)) /// main panel
                {
                    f.BackColor = Color.FromArgb(96, 96, 96);
                    f.ForeColor = Color.White;
                    foreach (Control c in f.Controls)
                        if (c.GetType() == typeof(TableLayoutPanel)) /// main table
                        {
                            c.BackColor = Color.FromArgb(96, 96, 96);
                            c.ForeColor = Color.White;

                            foreach (Control aa in c.Controls)
                                if (aa.GetType() == typeof(Label) || aa.GetType() == typeof(Button) ||
                                    aa.GetType() == typeof(CheckBox) || aa.GetType() == typeof(RadioButton))
                                {
                                    aa.ForeColor = Color.White;
                                    aa.BackColor = Color.Transparent;
                                    if (aa.Name.Contains("Header"))
                                    {
                                        aa.BackColor = Color.FromArgb(189, 189, 189);
                                        aa.ForeColor = Color.FromArgb(75, 75, 75);
                                    }
                                }
                                else if (aa.GetType() == typeof(TextBox) || aa.GetType() == typeof(ComboBox))
                                {
                                    aa.BackColor = Color.FromArgb(75, 75, 75);
                                    aa.ForeColor = Color.White;
                                }
                                else if (aa.GetType() == typeof(BunifuDatepicker))
                                {
                                    var dd = aa as BunifuDatepicker;
                                    dd.BackColor = Color.FromArgb(75, 75, 75);
                                    dd.ForeColor = Color.White;
                                }
                                else if (aa.GetType() == typeof(BCDateTimePicker))
                                {
                                    var dd = aa as BCDateTimePicker;
                                    dd.BackColor = Color.FromArgb(75, 75, 75);
                                    dd.ForeColor = Color.White;
                                }
                                else if (aa.GetType() == typeof(AdvancedDataGridView))
                                { 
                                    var MP_dataGridView = aa as AdvancedDataGridView; 

                                    MP_dataGridView.BackgroundColor =
                                        MP_dataGridView.ColumnHeadersDefaultCellStyle.BackColor =
                                            MP_dataGridView.DefaultCellStyle.BackColor =
                                                MP_dataGridView.RowsDefaultCellStyle.BackColor =
                                                    Color.FromArgb(75, 75, 75);

                                    MP_dataGridView.ForeColor =
                                        MP_dataGridView.ColumnHeadersDefaultCellStyle.ForeColor =
                                            MP_dataGridView.DefaultCellStyle.ForeColor =
                                                MP_dataGridView.RowsDefaultCellStyle.ForeColor = Color.White;

                                    MP_dataGridView.GridColor = Color.FromArgb(189, 189, 189);

                                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor =
                                        MP_dataGridView.DefaultCellStyle.SelectionBackColor
                                            = MP_dataGridView.RowsDefaultCellStyle.SelectionBackColor =
                                                Color.FromArgb(96, 96, 96);
                                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionForeColor =
                                        MP_dataGridView.DefaultCellStyle.SelectionForeColor
                                            = MP_dataGridView.RowsDefaultCellStyle.SelectionForeColor = Color.White;


                                    for (var i = 0; i < MP_dataGridView.Columns.Count; i++)
                                    {
                                        MP_dataGridView.Columns[i].HeaderCell.Style.BackColor =
                                            Color.FromArgb(75, 75, 75);
                                        MP_dataGridView.Columns[i].HeaderCell.Style.ForeColor = Color.White;
                                        for (var j = 0; j < MP_dataGridView.Rows.Count; j++)
                                        {
                                            MP_dataGridView.Rows[j].Cells[i].Style.BackColor =
                                                Color.FromArgb(75, 75, 75);
                                            MP_dataGridView.Rows[j].Cells[i].Style.ForeColor = Color.White;
                                        }
                                    }
                                }
                                
                                else if (aa.GetType() == typeof(TableLayoutPanel)) // inner table
                                {
                                    aa.BackColor = Color.FromArgb(96, 96, 96);
                                    aa.ForeColor = Color.White;
                                    foreach (Control bb in aa.Controls)
                                        if (bb.GetType() == typeof(Label) || bb.GetType() == typeof(Button) ||
                                            bb.GetType() == typeof(CheckBox) || bb.GetType() == typeof(RadioButton))
                                        {
                                            bb.ForeColor = Color.White;
                                            if (bb.Name == "InsertVisit_button" || bb.Name == "Save_button")
                                                bb.BackgroundImage = Resources.Save2_L;
                                            else if (bb.Name == "Delete_button")
                                                bb.BackgroundImage = Resources.Delete2_L;
                                        }
                                        else if (bb.GetType() == typeof(TextBox) || bb.GetType() == typeof(ComboBox))
                                        {
                                            bb.BackColor = Color.FromArgb(75, 75, 75);
                                            bb.ForeColor = Color.White;
                                        }
                                        else if (bb.GetType() == typeof(PictureBox))
                                        {  
                                                bb.BackgroundImage = Resources.Exit_L; 
                                        }
                                }
                        }
                }

            form.Refresh();
        }

        public void Visit_ToLight(Form form)
        {
            foreach (Control f in form.Controls)
                if (f.GetType() == typeof(Panel)) /// main panel
                {
                    f.BackColor = Color.FromArgb(240, 240, 240);
                    f.ForeColor = Color.Black;
                    foreach (Control c in f.Controls)
                        if (c.GetType() == typeof(TableLayoutPanel)) /// main table
                        {
                            c.BackColor = Color.FromArgb(240, 240, 240);
                            c.ForeColor = Color.Black;

                            foreach (Control aa in c.Controls)
                                if (aa.GetType() == typeof(Label) || aa.GetType() == typeof(Button) ||
                                    aa.GetType() == typeof(CheckBox) || aa.GetType() == typeof(RadioButton))
                                {
                                    if (aa.Name.Contains("Header"))
                                    {
                                        aa.BackColor = Color.FromArgb(96, 96, 96);
                                        aa.ForeColor = Color.White;
                                    }

                                    aa.ForeColor = Color.Black;
                                    aa.BackColor = Color.Transparent;
                                }
                                else if (aa.GetType() == typeof(TextBox) || aa.GetType() == typeof(ComboBox))
                                {
                                    aa.BackColor = Color.White;
                                    aa.ForeColor = Color.Black;
                                }
                                else if (aa.GetType() == typeof(BunifuDatepicker))
                                {
                                    var dd = aa as BunifuDatepicker;
                                    dd.BackColor = Color.White;
                                    dd.ForeColor = Color.Black;
                                }
                                else if (aa.GetType() == typeof(BCDateTimePicker))
                                {
                                    var dd = aa as BCDateTimePicker;
                                    dd.BackColor = Color.White;
                                    dd.ForeColor = Color.Black;
                                }
                                else if (aa.GetType() == typeof(AdvancedDataGridView))
                                {
                                    var MP_dataGridView = aa as AdvancedDataGridView;
                                    MP_dataGridView.BackgroundColor =
                                        MP_dataGridView.ColumnHeadersDefaultCellStyle.BackColor =
                                            MP_dataGridView.DefaultCellStyle.BackColor =
                                                MP_dataGridView.RowsDefaultCellStyle.BackColor = Color.White;

                                    MP_dataGridView.ForeColor =
                                        MP_dataGridView.ColumnHeadersDefaultCellStyle.ForeColor =
                                            MP_dataGridView.DefaultCellStyle.ForeColor =
                                                MP_dataGridView.RowsDefaultCellStyle.ForeColor = Color.Black;

                                    MP_dataGridView.GridColor = Color.FromArgb(96, 96, 96);

                                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor =
                                        MP_dataGridView.DefaultCellStyle.SelectionBackColor
                                            = MP_dataGridView.RowsDefaultCellStyle.SelectionBackColor =
                                                Color.FromArgb(189, 189, 189);
                                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionForeColor =
                                        MP_dataGridView.DefaultCellStyle.SelectionForeColor
                                            = MP_dataGridView.RowsDefaultCellStyle.SelectionForeColor = Color.Black;


                                    for (var i = 0; i < MP_dataGridView.Columns.Count; i++)
                                    {
                                        MP_dataGridView.Columns[i].HeaderCell.Style.BackColor = Color.White;
                                        MP_dataGridView.Columns[i].HeaderCell.Style.ForeColor = Color.Black;
                                        for (var j = 0; j < MP_dataGridView.Rows.Count; j++)
                                        {
                                            MP_dataGridView.Rows[j].Cells[i].Style.BackColor = Color.White;
                                            MP_dataGridView.Rows[j].Cells[i].Style.ForeColor = Color.Black;
                                        }
                                    }
                                }

                                else if (aa.GetType() == typeof(TableLayoutPanel)) // inner table
                                {
                                    aa.BackColor = Color.FromArgb(240, 240, 240);
                                    aa.ForeColor = Color.Black;
                                    foreach (Control bb in aa.Controls)
                                        if (bb.GetType() == typeof(Label) || bb.GetType() == typeof(Button) ||
                                            bb.GetType() == typeof(CheckBox) || bb.GetType() == typeof(RadioButton))
                                        {
                                            bb.ForeColor = Color.Black;
                                            bb.BackColor = Color.Transparent;
                                            if (bb.Name == "InsertVisit_button" || bb.Name == "Save_button")
                                                bb.BackgroundImage = Resources.Save2_D;
                                            else if (bb.Name == "Delete_button")
                                                bb.BackgroundImage = Resources.Delete2_D;
                                        }
                                        else if (bb.GetType() == typeof(TextBox) || bb.GetType() == typeof(ComboBox))
                                        {
                                            bb.BackColor = Color.White;
                                            bb.ForeColor = Color.Black;
                                        }
                                        else if (bb.GetType() == typeof(PictureBox))
                                        {
                                            bb.BackgroundImage = Resources.Exit_D;
                                        }
                                }
                        }
                }

            form.Refresh();
        }

        public void Evaluation_ToNight(Form form)
        {
            form.BackColor = Color.FromArgb(96, 96, 96);
            form.ForeColor = Color.FromArgb(189, 189, 189);
            foreach (Control c in form.Controls)
                if (c.GetType() == typeof(Panel))
                {
                    c.BackColor = Color.FromArgb(96, 96, 96);
                    c.ForeColor = Color.FromArgb(189, 189, 189);

                    foreach (Control aa in c.Controls)
                        if (aa.GetType() == typeof(Panel))
                        {
                            aa.BackColor = Color.FromArgb(96, 96, 96);
                            aa.ForeColor = Color.FromArgb(189, 189, 189);
                            foreach (Control sq in aa.Controls)
                                if (sq.GetType() == typeof(Label) || sq.GetType() == typeof(Button))
                                {
                                    sq.ForeColor = Color.White;
                                    if (sq.Name == "Old_Header_label" || sq.Name == "M_Header_label")
                                    {
                                        sq.BackColor = Color.FromArgb(189, 189, 189);
                                        sq.ForeColor = Color.FromArgb(75, 75, 75);
                                    }
                                }
                                else if (sq.GetType() == typeof(TextBox) || sq.GetType() == typeof(ComboBox))
                                {
                                    sq.BackColor = Color.FromArgb(75, 75, 75);
                                    sq.ForeColor = Color.FromArgb(189, 189, 189);
                                }
                                else if (sq.GetType() == typeof(BCDateTimePicker))
                                {
                                    var dd = sq as BCDateTimePicker;
                                    dd.BackColor = Color.FromArgb(75, 75, 75);
                                    dd.ForeColor = Color.White;
                                }
                                else if (sq.GetType() == typeof(DataGridView))
                                {
                                    var MP_dataGridView = sq as DataGridView;
                                    MP_dataGridView.BackgroundColor =
                                        MP_dataGridView.ColumnHeadersDefaultCellStyle.BackColor =
                                            MP_dataGridView.DefaultCellStyle.BackColor =
                                                MP_dataGridView.RowsDefaultCellStyle.BackColor =
                                                    Color.FromArgb(75, 75, 75);
                                    MP_dataGridView.AlternatingRowsDefaultCellStyle.BackColor =
                                        Color.FromArgb(96, 96, 96);

                                    MP_dataGridView.ForeColor =
                                        MP_dataGridView.ColumnHeadersDefaultCellStyle.ForeColor =
                                            MP_dataGridView.DefaultCellStyle.ForeColor =
                                                MP_dataGridView.RowsDefaultCellStyle.ForeColor =
                                                    MP_dataGridView.AlternatingRowsDefaultCellStyle.ForeColor =
                                                        Color.FromArgb(189, 189, 189);

                                    MP_dataGridView.GridColor = Color.FromArgb(189, 189, 189);

                                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor =
                                        MP_dataGridView.DefaultCellStyle.SelectionBackColor
                                            = MP_dataGridView.RowsDefaultCellStyle.SelectionBackColor =
                                                MP_dataGridView.AlternatingRowsDefaultCellStyle.SelectionBackColor =
                                                    Color.FromArgb(56, 56, 56);
                                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionForeColor =
                                        MP_dataGridView.DefaultCellStyle.SelectionForeColor
                                            = MP_dataGridView.RowsDefaultCellStyle.SelectionForeColor =
                                                MP_dataGridView.AlternatingRowsDefaultCellStyle.SelectionForeColor =
                                                    Color.FromArgb(189, 189, 189);

                                    for (var i = 0; i < MP_dataGridView.Columns.Count; i++)
                                    {
                                        MP_dataGridView.Columns[i].HeaderCell.Style.BackColor =
                                            Color.FromArgb(75, 75, 75);
                                        MP_dataGridView.Columns[i].HeaderCell.Style.ForeColor =
                                            Color.FromArgb(189, 189, 189);
                                    }
                                }
                                else if (sq.GetType() == typeof(TableLayoutPanel)) /// table
                                {
                                    sq.BackColor = Color.FromArgb(96, 96, 96);
                                    sq.ForeColor = Color.FromArgb(189, 189, 189);

                                    foreach (Control bb in sq.Controls)
                                        if (bb.GetType() == typeof(Label) || bb.GetType() == typeof(CheckBox) ||
                                            bb.GetType() == typeof(RadioButton))
                                        {
                                            bb.ForeColor = Color.White;
                                            if (bb.Name.Contains("Header_label"))
                                            {
                                                bb.BackColor = Color.FromArgb(189, 189, 189);
                                                bb.ForeColor = Color.FromArgb(75, 75, 75);
                                            }
                                        }
                                        else if (bb.GetType() == typeof(TextBox) || bb.GetType() == typeof(ComboBox))
                                        {
                                            bb.BackColor = Color.FromArgb(75, 75, 75);
                                            bb.ForeColor = Color.FromArgb(189, 189, 189);
                                        }
                                        else if (bb.GetType() == typeof(TrackBar))
                                        {
                                            bb.BackColor = Color.FromArgb(96, 96, 96);
                                            bb.ForeColor = Color.FromArgb(189, 189, 189);
                                        }
                                        else if (bb.GetType() == typeof(Button))
                                        {
                                            if (bb.Name == "Save_button")
                                                bb.BackgroundImage = Resources.Save2_L;
                                        }
                                }
                        }
                }
        }

        public void Evaluation_ToLight(Form form)
        {
            form.BackColor = Color.FromArgb(240, 240, 240);
            form.ForeColor = Color.Black;
            foreach (Control c in form.Controls)
                if (c.GetType() == typeof(Panel))
                {
                    c.BackColor = Color.FromArgb(240, 240, 240);
                    c.ForeColor = Color.Black;

                    foreach (Control aa in c.Controls)
                        if (aa.GetType() == typeof(Panel))
                        {
                            aa.BackColor = Color.FromArgb(240, 240, 240);
                            aa.ForeColor = Color.Black;
                            foreach (Control sq in aa.Controls)
                                if (sq.GetType() == typeof(Label) || sq.GetType() == typeof(Button))
                                {
                                    sq.ForeColor = Color.Black;
                                    if (sq.Name.Contains("Header_label"))
                                    {
                                        sq.BackColor = Color.FromArgb(96, 96, 96);
                                        sq.ForeColor = Color.White;
                                    }
                                }
                                else if (sq.GetType() == typeof(TextBox) || sq.GetType() == typeof(ComboBox))
                                {
                                    sq.BackColor = Color.White;
                                    sq.ForeColor = Color.Black;
                                }
                                else if (sq.GetType() == typeof(BCDateTimePicker))
                                {
                                    var dd = sq as BCDateTimePicker;
                                    dd.BackColor = Color.White;
                                    dd.ForeColor = Color.Black;
                                }
                                else if (sq.GetType() == typeof(DataGridView))
                                {
                                    var MP_dataGridView = sq as DataGridView;
                                    MP_dataGridView.BackgroundColor =
                                        MP_dataGridView.ColumnHeadersDefaultCellStyle.BackColor =
                                            MP_dataGridView.DefaultCellStyle.BackColor =
                                                MP_dataGridView.RowsDefaultCellStyle.BackColor = Color.White;
                                    MP_dataGridView.AlternatingRowsDefaultCellStyle.BackColor =
                                        Color.FromArgb(240, 240, 240);

                                    MP_dataGridView.ForeColor =
                                        MP_dataGridView.ColumnHeadersDefaultCellStyle.ForeColor =
                                            MP_dataGridView.DefaultCellStyle.ForeColor =
                                                MP_dataGridView.RowsDefaultCellStyle.ForeColor =
                                                    MP_dataGridView.AlternatingRowsDefaultCellStyle.ForeColor =
                                                        Color.Black;

                                    MP_dataGridView.GridColor = Color.FromArgb(96, 96, 96);

                                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor =
                                        MP_dataGridView.DefaultCellStyle.SelectionBackColor
                                            = MP_dataGridView.RowsDefaultCellStyle.SelectionBackColor =
                                                MP_dataGridView.AlternatingRowsDefaultCellStyle.SelectionBackColor =
                                                    Color.FromArgb(189, 189, 189);
                                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionForeColor =
                                        MP_dataGridView.DefaultCellStyle.SelectionForeColor
                                            = MP_dataGridView.RowsDefaultCellStyle.SelectionForeColor =
                                                MP_dataGridView.AlternatingRowsDefaultCellStyle.SelectionForeColor =
                                                    Color.Black;

                                    for (var i = 0; i < MP_dataGridView.Columns.Count; i++)
                                    {
                                        MP_dataGridView.Columns[i].HeaderCell.Style.BackColor = Color.White;
                                        MP_dataGridView.Columns[i].HeaderCell.Style.ForeColor = Color.Black;
                                    }
                                }
                                else if (sq.GetType() == typeof(TableLayoutPanel)) /// table
                                {
                                    sq.BackColor = Color.FromArgb(240, 240, 240);
                                    sq.ForeColor = Color.Black;

                                    foreach (Control bb in sq.Controls)
                                        if (bb.GetType() == typeof(Label) || bb.GetType() == typeof(CheckBox) ||
                                            bb.GetType() == typeof(RadioButton))
                                        {
                                            bb.ForeColor = Color.Black;
                                            if (bb.Name.Contains("Header_label"))
                                            {
                                                bb.BackColor = Color.FromArgb(96, 96, 96);
                                                bb.ForeColor = Color.White;
                                            }
                                        }
                                        else if (bb.GetType() == typeof(TrackBar))
                                        {
                                            bb.BackColor = Color.FromArgb(240, 240, 240);
                                            bb.ForeColor = Color.Black;
                                        }
                                        else if (bb.GetType() == typeof(Button))
                                        {
                                            if (bb.Name == "Save_button")
                                                bb.BackgroundImage = Resources.Save2_D;
                                        }
                                        else if (bb.GetType() == typeof(TextBox) || bb.GetType() == typeof(ComboBox))
                                        {
                                            bb.BackColor = Color.White;
                                            bb.ForeColor = Color.Black;
                                        }
                                }
                        }
                }
        }

        public void ME_Visit_ToNight(Form form)
        {
            foreach (Control f in form.Controls)
                if (f.GetType() == typeof(Panel)) /// main panel
                {
                    f.BackColor = Color.FromArgb(96, 96, 96);
                    f.ForeColor = Color.White;
                    foreach (Control c in f.Controls)
                        if (c.GetType() == typeof(TableLayoutPanel)) /// tables 
                        {
                            c.BackColor = Color.FromArgb(96, 96, 96);
                            c.ForeColor = Color.White;

                            foreach (Control aa in c.Controls)
                                if (aa.GetType() == typeof(Label) || aa.GetType() == typeof(Button))
                                {
                                    aa.ForeColor = Color.White;
                                    aa.BackColor = Color.Transparent;
                                    if (aa.Name.Contains("Header"))
                                    {
                                        aa.BackColor = Color.FromArgb(189, 189, 189);
                                        aa.ForeColor = Color.FromArgb(75, 75, 75);
                                    }
                                    else if (aa.Name == "Save_button")
                                    {
                                        aa.BackgroundImage = Resources.Save2_L;
                                    }
                                    else if (aa.Name == "Delete_button")
                                    {
                                        aa.BackgroundImage = Resources.Delete2_L;
                                    }
                                }
                                else if (aa.GetType() == typeof(TextBox) || aa.GetType() == typeof(ComboBox))
                                {
                                    aa.BackColor = Color.FromArgb(75, 75, 75);
                                    aa.ForeColor = Color.White;
                                }
                                else if (aa.GetType() == typeof(BunifuDatepicker))
                                {
                                    var dd = aa as BunifuDatepicker;
                                    dd.BackColor = Color.FromArgb(75, 75, 75);
                                    dd.ForeColor = Color.White;
                                }
                                else if (aa.GetType() == typeof(BCDateTimePicker))
                                {
                                    var dd = aa as BCDateTimePicker;
                                    dd.BackColor = Color.FromArgb(75, 75, 75);
                                    dd.ForeColor = Color.White;
                                }
                                else if (aa.GetType() == typeof(TableLayoutPanel)) // inner table
                                {
                                    aa.BackColor = Color.FromArgb(96, 96, 96);
                                    aa.ForeColor = Color.White;
                                    foreach (Control bb in aa.Controls)
                                        if (bb.GetType() == typeof(Label) || bb.GetType() == typeof(Button) ||
                                            bb.GetType() == typeof(CheckBox) || bb.GetType() == typeof(RadioButton))
                                        {
                                            if (bb.GetType() == typeof(Label) && aa.Name.Contains("_h"))
                                            {
                                                bb.BackColor = Color.FromArgb(189, 189, 189);
                                                bb.ForeColor = Color.FromArgb(75, 75, 75);
                                            }
                                            else
                                            {
                                                bb.ForeColor = Color.White;
                                                if (bb.Name.Contains("id")) //Make it invisible//
                                                    bb.BackColor = bb.ForeColor = Color.FromArgb(96, 96, 96);
                                            }
                                        }
                                        else if (bb.GetType() == typeof(TextBox) || bb.GetType() == typeof(ComboBox))
                                        {
                                            bb.BackColor = Color.FromArgb(75, 75, 75);
                                            bb.ForeColor = Color.White;
                                        }
                                }
                        }
                }

            form.Refresh();
        }

        public void ME_Visit_ToLight(Form form)
        {
            foreach (Control f in form.Controls)
                if (f.GetType() == typeof(Panel)) /// main panel
                {
                    f.BackColor = Color.FromArgb(240, 240, 240);
                    f.ForeColor = Color.Black;
                    foreach (Control c in f.Controls)
                        if (c.GetType() == typeof(TableLayoutPanel)) /// tables
                        {
                            c.BackColor = Color.FromArgb(240, 240, 240);
                            c.ForeColor = Color.Black;

                            foreach (Control aa in c.Controls)
                                if (aa.GetType() == typeof(Label) || aa.GetType() == typeof(Button))
                                {
                                    aa.ForeColor = Color.Black;
                                    aa.BackColor = Color.Transparent;
                                    if (aa.Name.Contains("Header"))
                                    {
                                        aa.BackColor = Color.FromArgb(96, 96, 96);
                                        aa.ForeColor = Color.White;
                                    }
                                    else if (aa.Name == "Save_button")
                                    {
                                        aa.BackgroundImage = Resources.Save2_D;
                                    }
                                    else if (aa.Name == "Delete_button")
                                    {
                                        aa.BackgroundImage = Resources.Delete2_D;
                                    }
                                }
                                else if (aa.GetType() == typeof(TextBox) || aa.GetType() == typeof(ComboBox))
                                {
                                    aa.BackColor = Color.White;
                                    aa.ForeColor = Color.Black;
                                }
                                else if (aa.GetType() == typeof(BunifuDatepicker))
                                {
                                    var dd = aa as BunifuDatepicker;
                                    dd.BackColor = Color.White;
                                    dd.ForeColor = Color.Black;
                                }
                                else if (aa.GetType() == typeof(BCDateTimePicker))
                                {
                                    var dd = aa as BCDateTimePicker;
                                    dd.BackColor = Color.White;
                                    dd.ForeColor = Color.Black;
                                }
                                else if (aa.GetType() == typeof(TableLayoutPanel)) // inner table
                                {
                                    aa.BackColor = Color.FromArgb(240, 240, 240);
                                    aa.ForeColor = Color.Black;
                                    foreach (Control bb in aa.Controls)
                                        if (bb.GetType() == typeof(Label) || bb.GetType() == typeof(Button) ||
                                            bb.GetType() == typeof(CheckBox) || bb.GetType() == typeof(RadioButton))
                                        {
                                            if (bb.GetType() == typeof(Label) && aa.Name.Contains("_h"))
                                            {
                                                bb.BackColor = Color.FromArgb(96, 96, 96);
                                                bb.ForeColor = Color.White;
                                            }
                                            else
                                            {
                                                bb.ForeColor = Color.Black;
                                                bb.BackColor = Color.Transparent;
                                                if (bb.Name.Contains("id")) //Make it invisible
                                                    bb.BackColor = bb.ForeColor = Color.FromArgb(240, 240, 240);
                                            }
                                        }
                                        else if (bb.GetType() == typeof(TextBox) || bb.GetType() == typeof(ComboBox))
                                        {
                                            bb.BackColor = Color.White;
                                            bb.ForeColor = Color.Black;
                                        }
                                }
                        }
                }

            form.Refresh();
        }

        public void Attachment_ToNight(Form form)
        {
            foreach (Control f in form.Controls)
                if (f.GetType() == typeof(Panel)) /// main panel
                {
                    f.BackColor = Color.FromArgb(96, 96, 96);
                    f.ForeColor = Color.White;
                    foreach (Control c in f.Controls)
                        if (c.GetType() == typeof(TableLayoutPanel))
                        {
                            /// main table
                            c.BackColor = Color.FromArgb(96, 96, 96);
                            c.ForeColor = Color.FromArgb(189, 189, 189);
                            foreach (Control aa in c.Controls)
                                if (aa.GetType() == typeof(Label) || aa.GetType() == typeof(Button) ||
                                    aa.GetType() == typeof(CheckBox) || aa.GetType() == typeof(RadioButton))
                                {
                                    aa.ForeColor = Color.White;
                                }
                                else if (aa.GetType() == typeof(TextBox) || aa.GetType() == typeof(ComboBox))
                                {
                                    aa.BackColor = Color.FromArgb(75, 75, 75);
                                    aa.ForeColor = Color.White;
                                }
                                else if (aa.GetType() == typeof(Panel))
                                {
                                    aa.BackColor = Color.FromArgb(75, 75, 75);
                                    aa.ForeColor = Color.FromArgb(189, 189, 189);
                                    if (aa.Controls["pictureBox1"] != null) 
                                        aa.Controls["pictureBox1"].BackColor = Color.FromArgb(75, 75, 75);
                                }
                                else if (aa.GetType() == typeof(TableLayoutPanel))
                                {
                                    if (aa.Name == "Bottom_tableLayoutPanel")
                                    {
                                        aa.BackColor = Color.FromArgb(50, 50, 50);
                                        aa.ForeColor = Color.FromArgb(189, 189, 189);
                                        foreach (Control bb in aa.Controls)
                                        {
                                            if (bb.Name == "AddImage_button")
                                                bb.BackgroundImage = Resources.Plus_Sq_L;
                                            else if (bb.Name == "DeleteImage_button")
                                                bb.BackgroundImage = Resources.Trash_L;
                                            else if (aa.Name == "DeleteImage_button")
                                                bb.BackgroundImage = Resources.FitToScreen_L;
                                            ////   left
                                            ////   right
                                            ////   first
                                            ////   last
                                            if (bb.GetType() == typeof(Label) || bb.GetType() == typeof(Button) ||
                                                bb.GetType() == typeof(CheckBox) || bb.GetType() == typeof(RadioButton))
                                            {
                                                bb.ForeColor = Color.White;
                                            }
                                            else if (bb.GetType() == typeof(TextBox) ||
                                                     bb.GetType() == typeof(ComboBox))
                                            {
                                                bb.BackColor = Color.FromArgb(75, 75, 75);
                                                bb.ForeColor = Color.White;
                                            }
                                        }
                                    }
                                }
                        }
                }

            form.Refresh();
        }

        public void Attachment_ToLight(Form form)
        {
            foreach (Control f in form.Controls)
                if (f.GetType() == typeof(Panel)) /// main panel
                {
                    f.BackColor = Color.FromArgb(96, 96, 96);
                    f.ForeColor = Color.White;
                    foreach (Control c in f.Controls)
                        if (c.GetType() == typeof(TableLayoutPanel))
                        {
                            /// main table

                            c.BackColor = Color.FromArgb(240, 240, 240);
                            c.ForeColor = Color.Black;
                            foreach (Control aa in c.Controls)
                                if (aa.GetType() == typeof(Label) || aa.GetType() == typeof(Button) ||
                                    aa.GetType() == typeof(CheckBox) || aa.GetType() == typeof(RadioButton))
                                {
                                    aa.ForeColor = Color.Black;
                                }
                                else if (aa.GetType() == typeof(TextBox) || aa.GetType() == typeof(ComboBox))
                                {
                                    aa.BackColor = Color.White;
                                    aa.ForeColor = Color.Black;
                                }
                                else if (aa.GetType() == typeof(Panel))
                                {
                                    aa.BackColor = Color.FromArgb(240, 240, 240);
                                    aa.ForeColor = Color.Black;
                                    if(aa.Controls["pictureBox1"] != null)
                                        aa.Controls["pictureBox1"].BackColor = Color.FromArgb(240, 240, 240);
                                }
                                else if (aa.GetType() == typeof(TableLayoutPanel))
                                {
                                    if (aa.Name == "Bottom_tableLayoutPanel")
                                    {
                                        aa.BackColor = Color.FromArgb(189, 189, 189);
                                        aa.ForeColor = Color.Black;
                                        foreach (Control bb in aa.Controls)
                                        {
                                            if (bb.Name == "AddImage_button")
                                                bb.BackgroundImage = Resources.Plus_Sq_D;
                                            else if (bb.Name == "DeleteImage_button")
                                                bb.BackgroundImage = Resources.Trash_D;
                                            else if (bb.Name == "DeleteImage_button")
                                                bb.BackgroundImage = Resources.FitToScreen_D;
                                            ////   left
                                            ////   right
                                            ////   first
                                            ////   last
                                            if (bb.GetType() == typeof(Label) || bb.GetType() == typeof(Button) ||
                                                bb.GetType() == typeof(CheckBox) || bb.GetType() == typeof(RadioButton))
                                            {
                                                bb.ForeColor = Color.Black;
                                            }
                                            else if (bb.GetType() == typeof(TextBox) ||
                                                     bb.GetType() == typeof(ComboBox))
                                            {
                                                bb.BackColor = Color.White;
                                                bb.ForeColor = Color.Black;
                                            }
                                        }
                                    }
                                }
                        }
                }

            form.Refresh();
        }

        public void Loan_ToNight(Form form)
        {
            form.BackColor = Color.FromArgb(96, 96, 96);
            form.ForeColor = Color.White;
            foreach (Control f in form.Controls)
                if (f.GetType() == typeof(Panel)) /// main panel
                {
                    f.BackColor = Color.FromArgb(96, 96, 96);
                    f.ForeColor = Color.White;
                    foreach (Control c in f.Controls)
                        if (c.GetType() == typeof(TableLayoutPanel)) /// main table
                        {
                            c.BackColor = Color.FromArgb(96, 96, 96);
                            c.ForeColor = Color.White;

                            foreach (Control aa in c.Controls)
                                if (aa.GetType() == typeof(Label) || aa.GetType() == typeof(Button) ||
                                    aa.GetType() == typeof(CheckBox) || aa.GetType() == typeof(RadioButton))
                                {
                                    aa.ForeColor = Color.White;
                                    aa.BackColor = Color.Transparent;
                                }
                                else if (aa.GetType() == typeof(TextBox) || aa.GetType() == typeof(ComboBox) ||
                                         aa.GetType() == typeof(MaskedTextBox))
                                {
                                    aa.BackColor = Color.FromArgb(75, 75, 75);
                                    aa.ForeColor = Color.White;
                                }
                                else if (aa.GetType() == typeof(BCDateTimePicker))
                                {
                                    var dd = aa as BCDateTimePicker;
                                    dd.BackColor = Color.FromArgb(75, 75, 75);
                                    dd.ForeColor = Color.White;
                                }
                                else if (aa.GetType() == typeof(DataGridView))
                                {
                                    var MP_dataGridView = aa as DataGridView;
                                    MP_dataGridView.BackgroundColor =
                                        MP_dataGridView.ColumnHeadersDefaultCellStyle.BackColor =
                                            MP_dataGridView.DefaultCellStyle.BackColor =
                                                MP_dataGridView.RowsDefaultCellStyle.BackColor =
                                                    Color.FromArgb(75, 75, 75);

                                    MP_dataGridView.ForeColor =
                                        MP_dataGridView.ColumnHeadersDefaultCellStyle.ForeColor =
                                            MP_dataGridView.DefaultCellStyle.ForeColor =
                                                MP_dataGridView.RowsDefaultCellStyle.ForeColor = Color.White;

                                    MP_dataGridView.GridColor = Color.FromArgb(189, 189, 189);

                                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor =
                                        MP_dataGridView.DefaultCellStyle.SelectionBackColor
                                            = MP_dataGridView.RowsDefaultCellStyle.SelectionBackColor =
                                                Color.FromArgb(96, 96, 96);
                                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionForeColor =
                                        MP_dataGridView.DefaultCellStyle.SelectionForeColor
                                            = MP_dataGridView.RowsDefaultCellStyle.SelectionForeColor = Color.White;


                                    for (var i = 0; i < MP_dataGridView.Columns.Count; i++)
                                    {
                                        MP_dataGridView.Columns[i].HeaderCell.Style.BackColor =
                                            Color.FromArgb(75, 75, 75);
                                        MP_dataGridView.Columns[i].HeaderCell.Style.ForeColor = Color.White;
                                        for (var j = 0; j < MP_dataGridView.Rows.Count; j++)
                                        {
                                            MP_dataGridView.Rows[j].Cells[i].Style.BackColor =
                                                Color.FromArgb(75, 75, 75);
                                            MP_dataGridView.Rows[j].Cells[i].Style.ForeColor = Color.White;
                                        }
                                    }
                                }
                                else if (aa.GetType() == typeof(TableLayoutPanel)) // inner table
                                {
                                    aa.BackColor = Color.FromArgb(96, 96, 96);
                                    aa.ForeColor = Color.White;
                                    foreach (Control bb in aa.Controls)
                                    {
                                        if (bb.GetType() == typeof(Label) || bb.GetType() == typeof(Button) ||
                                            bb.GetType() == typeof(CheckBox) || bb.GetType() == typeof(RadioButton))
                                        {
                                            bb.ForeColor = Color.White;
                                            if (bb.Name.Contains("Add") || bb.Name.Contains("Save"))
                                                bb.BackgroundImage = Resources.Save2_L;
                                            else if (bb.Name.Contains("Delete"))
                                                bb.BackgroundImage = Resources.Delete2_L;
                                        }
                                    }
                                }
                        }
                }

            form.Refresh();
        }

        public void Loan_ToLight(Form form)
        {
            form.BackColor = Color.FromArgb(240, 240, 240);
            form.ForeColor = Color.Black;
            foreach (Control f in form.Controls)
                if (f.GetType() == typeof(Panel)) /// main panel
                {
                    f.BackColor = Color.FromArgb(240, 240, 240);
                    f.ForeColor = Color.Black;
                    foreach (Control c in f.Controls)
                        if (c.GetType() == typeof(TableLayoutPanel)) /// main table
                        {
                            c.BackColor = Color.FromArgb(240, 240, 240);
                            c.ForeColor = Color.Black;

                            foreach (Control aa in c.Controls)
                                if (aa.GetType() == typeof(Label) || 
                                    aa.GetType() == typeof(CheckBox) || aa.GetType() == typeof(RadioButton))
                                {
                                    aa.ForeColor = Color.Black;
                                    aa.BackColor = Color.Transparent;
                                }
                            else if(aa.GetType() == typeof(Button))
                                {
                                    aa.ForeColor = Color.White;
                                    aa.BackColor = Color.Transparent;
                                }
                                else if (aa.GetType() == typeof(TextBox) || aa.GetType() == typeof(ComboBox) ||
                                         aa.GetType() == typeof(MaskedTextBox))
                                {
                                    aa.BackColor = Color.White;
                                    aa.ForeColor = Color.Black;
                                }
                                else if (aa.GetType() == typeof(BCDateTimePicker))
                                {
                                    var dd = aa as BCDateTimePicker;
                                    dd.BackColor = Color.White;
                                    dd.ForeColor = Color.Black;
                                }
                                else if (aa.GetType() == typeof(DataGridView))
                                {
                                    var MP_dataGridView = aa as DataGridView;
                                    MP_dataGridView.BackgroundColor =
                                        MP_dataGridView.ColumnHeadersDefaultCellStyle.BackColor =
                                            MP_dataGridView.DefaultCellStyle.BackColor =
                                                MP_dataGridView.RowsDefaultCellStyle.BackColor = Color.White;

                                    MP_dataGridView.ForeColor =
                                        MP_dataGridView.ColumnHeadersDefaultCellStyle.ForeColor =
                                            MP_dataGridView.DefaultCellStyle.ForeColor =
                                                MP_dataGridView.RowsDefaultCellStyle.ForeColor = Color.Black;

                                    MP_dataGridView.GridColor = Color.FromArgb(96, 96, 96);

                                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor =
                                        MP_dataGridView.DefaultCellStyle.SelectionBackColor
                                            = MP_dataGridView.RowsDefaultCellStyle.SelectionBackColor =
                                                Color.FromArgb(189, 189, 189);
                                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionForeColor =
                                        MP_dataGridView.DefaultCellStyle.SelectionForeColor
                                            = MP_dataGridView.RowsDefaultCellStyle.SelectionForeColor = Color.Black;


                                    for (var i = 0; i < MP_dataGridView.Columns.Count; i++)
                                    {
                                        MP_dataGridView.Columns[i].HeaderCell.Style.BackColor = Color.White;
                                        MP_dataGridView.Columns[i].HeaderCell.Style.ForeColor = Color.Black;
                                        for (var j = 0; j < MP_dataGridView.Rows.Count; j++)
                                        {
                                            MP_dataGridView.Rows[j].Cells[i].Style.BackColor = Color.White;
                                            MP_dataGridView.Rows[j].Cells[i].Style.ForeColor = Color.Black;
                                        }
                                    }
                                }
                                else if (aa.GetType() == typeof(TableLayoutPanel)) // inner table
                                {
                                    aa.BackColor = Color.FromArgb(240, 240, 240);
                                    aa.ForeColor = Color.Black;
                                    foreach (Control bb in aa.Controls)
                                        if (bb.GetType() == typeof(Label) || bb.GetType() == typeof(Button) ||
                                            bb.GetType() == typeof(CheckBox) || bb.GetType() == typeof(RadioButton))
                                        {
                                            bb.ForeColor = Color.Black; 
                                            if (bb.Name.Contains("Add") || bb.Name.Contains("Save"))
                                                bb.BackgroundImage = Resources.Save2_D; 
                                            else if (bb.Name.Contains("Delete"))
                                                bb.BackgroundImage = Resources.Delete2_D;
                                        }
                                }
                        }
                }

            form.Refresh();
        }

        public void Tasks_ToNight(Form form)
        {
            form.BackColor = Color.FromArgb(96, 96, 96);
            form.ForeColor = Color.White;
            foreach (Control c in form.Controls)
                if (c.GetType() == typeof(TableLayoutPanel))
                {
                    c.BackColor = Color.FromArgb(96, 96, 96);
                    c.ForeColor = Color.White;
                    foreach (Control aa in c.Controls)
                        if (aa.GetType() == typeof(Label) || aa.GetType() == typeof(Button) ||
                            aa.GetType() == typeof(CheckBox) || aa.GetType() == typeof(RadioButton))
                        {
                            aa.ForeColor = Color.White;
                            aa.BackColor = Color.Transparent;

                            if (aa.Name == "Refresh_button")
                                aa.BackgroundImage = Resources.Refresh2_L;
                            if (aa.Name == "ExportToExcel_button")
                                aa.BackgroundImage = Resources.Excel_L;
                        }
                        else if (aa.GetType() == typeof(TextBox) || aa.GetType() == typeof(ComboBox))
                        {
                            aa.BackColor = Color.FromArgb(75, 75, 75);
                            aa.ForeColor = Color.White;
                        }
                }
                else if (c.GetType() == typeof(Panel))
                {
                    c.BackColor = Color.FromArgb(96, 96, 96);
                    c.ForeColor = Color.White;

                    foreach (Control bb in c.Controls)
                        if (bb.GetType() == typeof(TableLayoutPanel)) /// combo
                        {
                            if (bb.Name.Contains("Filter"))
                                bb.BackColor = Color.FromArgb(75, 75, 75);
                            else
                                bb.BackColor = Color.FromArgb(96, 96, 96);
                            bb.ForeColor = Color.White;

                            foreach (Control aa in bb.Controls)
                                if (aa.GetType() == typeof(TextBox) || aa.GetType() == typeof(ComboBox))
                                {
                                    aa.BackColor = Color.FromArgb(75, 75, 75);
                                    aa.ForeColor = Color.White;
                                    if (aa.Name.Contains("Filter"))
                                        aa.BackColor = Color.FromArgb(75, 75, 75);
                                    else
                                        aa.BackColor = Color.FromArgb(96, 96, 96);
                                    aa.ForeColor = Color.White;
                                }
                                else if (aa.GetType() == typeof(DataGridView))
                                {
                                    var MP_dataGridView = aa as DataGridView;
                                    MP_dataGridView.BackgroundColor =
                                        MP_dataGridView.ColumnHeadersDefaultCellStyle.BackColor =
                                            MP_dataGridView.DefaultCellStyle.BackColor =
                                                MP_dataGridView.RowsDefaultCellStyle.BackColor =
                                                    Color.FromArgb(75, 75, 75);
                                    MP_dataGridView.AlternatingRowsDefaultCellStyle.BackColor =
                                        Color.FromArgb(96, 96, 96);

                                    MP_dataGridView.ForeColor =
                                        MP_dataGridView.ColumnHeadersDefaultCellStyle.ForeColor =
                                            MP_dataGridView.DefaultCellStyle.ForeColor =
                                                MP_dataGridView.RowsDefaultCellStyle.ForeColor =
                                                    MP_dataGridView.AlternatingRowsDefaultCellStyle.ForeColor =
                                                        Color.FromArgb(189, 189, 189);

                                    MP_dataGridView.GridColor = Color.FromArgb(189, 189, 189);

                                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor =
                                        MP_dataGridView.DefaultCellStyle.SelectionBackColor
                                            = MP_dataGridView.RowsDefaultCellStyle.SelectionBackColor =
                                                MP_dataGridView.AlternatingRowsDefaultCellStyle.SelectionBackColor =
                                                    Color.FromArgb(56, 56, 56);
                                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionForeColor =
                                        MP_dataGridView.DefaultCellStyle.SelectionForeColor
                                            = MP_dataGridView.RowsDefaultCellStyle.SelectionForeColor =
                                                MP_dataGridView.AlternatingRowsDefaultCellStyle.SelectionForeColor =
                                                    Color.FromArgb(189, 189, 189);

                                    for (var i = 0; i < MP_dataGridView.Columns.Count; i++)
                                    {
                                        MP_dataGridView.Columns[i].HeaderCell.Style.BackColor =
                                            Color.FromArgb(75, 75, 75);
                                        MP_dataGridView.Columns[i].HeaderCell.Style.ForeColor =
                                            Color.FromArgb(189, 189, 189);
                                    }
                                }
                        }
                }
        }

        public void Tasks_ToLight(Form form)
        {
            form.BackColor = Color.FromArgb(240, 240, 240);
            form.ForeColor = Color.Black;
            foreach (Control c in form.Controls)
                if (c.GetType() == typeof(TableLayoutPanel))
                {
                    c.BackColor = Color.FromArgb(240, 240, 240);
                    c.ForeColor = Color.Black;

                    foreach (Control aa in c.Controls)
                        if (aa.GetType() == typeof(Label) || aa.GetType() == typeof(Button) ||
                            aa.GetType() == typeof(CheckBox) || aa.GetType() == typeof(RadioButton))
                        {
                            aa.ForeColor = Color.Black;
                            aa.BackColor = Color.Transparent;

                            if (aa.Name == "Refresh_button")
                                aa.BackgroundImage = Resources.Refresh2_D;
                            else if (aa.Name == "ExportToExcel_button")
                                aa.BackgroundImage = Resources.Excel_D;
                        }
                        else if (aa.GetType() == typeof(TextBox) || aa.GetType() == typeof(ComboBox))
                        {
                            aa.BackColor = Color.White;
                            aa.ForeColor = Color.Black;
                        }
                }
                else if (c.GetType() == typeof(Panel))
                {
                    // grid panel & filters panel
                    c.BackColor = Color.FromArgb(240, 240, 240);
                    c.ForeColor = Color.Black;

                    foreach (Control bb in c.Controls)
                        if (bb.GetType() == typeof(TableLayoutPanel)) /// for combo
                        {
                            bb.BackColor = Color.FromArgb(240, 240, 240);
                            bb.ForeColor = Color.Black;

                            foreach (Control aa in bb.Controls)
                                if (aa.GetType() == typeof(TextBox) || aa.GetType() == typeof(ComboBox))
                                {
                                    aa.BackColor = Color.White;
                                    aa.ForeColor = Color.Black;
                                }
                                else if (aa.GetType() == typeof(DataGridView))
                                {
                                    var MP_dataGridView = aa as DataGridView;
                                    MP_dataGridView.BackgroundColor =
                                        MP_dataGridView.ColumnHeadersDefaultCellStyle.BackColor =
                                            MP_dataGridView.DefaultCellStyle.BackColor =
                                                MP_dataGridView.RowsDefaultCellStyle.BackColor = Color.White;
                                    MP_dataGridView.AlternatingRowsDefaultCellStyle.BackColor =
                                        Color.FromArgb(217, 216, 214);

                                    MP_dataGridView.ForeColor =
                                        MP_dataGridView.ColumnHeadersDefaultCellStyle.ForeColor =
                                            MP_dataGridView.DefaultCellStyle.ForeColor =
                                                MP_dataGridView.RowsDefaultCellStyle.ForeColor =
                                                    MP_dataGridView.AlternatingRowsDefaultCellStyle.ForeColor =
                                                        Color.Black;

                                    MP_dataGridView.GridColor = Color.FromArgb(96, 96, 96);

                                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor =
                                        MP_dataGridView.DefaultCellStyle.SelectionBackColor
                                            = MP_dataGridView.RowsDefaultCellStyle.SelectionBackColor =
                                                MP_dataGridView.AlternatingRowsDefaultCellStyle.SelectionBackColor =
                                                    Color.FromArgb(189, 189, 189);
                                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionForeColor =
                                        MP_dataGridView.DefaultCellStyle.SelectionForeColor
                                            = MP_dataGridView.RowsDefaultCellStyle.SelectionForeColor =
                                                MP_dataGridView.AlternatingRowsDefaultCellStyle.SelectionForeColor =
                                                    Color.Black;

                                    for (var i = 0; i < MP_dataGridView.Columns.Count; i++)
                                    {
                                        MP_dataGridView.Columns[i].HeaderCell.Style.BackColor = Color.White;
                                        MP_dataGridView.Columns[i].HeaderCell.Style.ForeColor = Color.Black;
                                    }
                                }
                        }
                }
        }

        public void Search_ToNight(Form form, bool Show)
        {
            form.BackColor = Color.FromArgb(96, 96, 96);
            form.ForeColor = Color.FromArgb(189, 189, 189);
            foreach (Control c in form.Controls)
                if (c.GetType() == typeof(TableLayoutPanel)) /// main table
                {
                    c.BackColor = Color.FromArgb(96, 96, 96);
                    c.ForeColor = Color.FromArgb(189, 189, 189);

                    foreach (Control aa in c.Controls)
                        if (aa.GetType() == typeof(Label) || aa.GetType() == typeof(Button) ||
                            aa.GetType() == typeof(CheckBox) || aa.GetType() == typeof(RadioButton))
                        {
                            aa.ForeColor = Color.White;
                            //   aa.BackColor = Color.Transparent;

                            if (aa.Name == "Delete_button")
                                aa.BackgroundImage = Resources.Trash_L;
                            else if (aa.Name == "ExportToExcel_button")
                                aa.BackgroundImage = Resources.Excel_L;
                        }
                        else if (aa.GetType() == typeof(TextBox) || aa.GetType() == typeof(ComboBox))
                        {
                            aa.BackColor = Color.FromArgb(75, 75, 75);
                            aa.ForeColor = Color.FromArgb(189, 189, 189);
                        }
                }
                else if (c.GetType() == typeof(Panel))
                {
                    c.BackColor = Color.FromArgb(96, 96, 96);
                    c.ForeColor = Color.FromArgb(189, 189, 189);

                    foreach (Control aa in c.Controls)
                        if (aa.GetType() == typeof(Label) || aa.GetType() == typeof(Button) ||
                            aa.GetType() == typeof(CheckBox) || aa.GetType() == typeof(RadioButton))
                        {
                            aa.ForeColor = Color.White;
                            if (aa.Name == "ShowHide_button")
                            {
                                if (Show)
                                    aa.BackgroundImage = Resources.Show2_L;
                                else aa.BackgroundImage = Resources.Hide2_L;
                            }
                        }
                        else if (aa.GetType() == typeof(TextBox) || aa.GetType() == typeof(ComboBox))
                        {
                            aa.BackColor = Color.FromArgb(75, 75, 75);
                            aa.ForeColor = Color.FromArgb(189, 189, 189);
                        }
                        else if (aa.GetType() == typeof(TableLayoutPanel)) /// filter table
                        {
                            aa.BackColor = Color.FromArgb(96, 96, 96);
                            aa.ForeColor = Color.FromArgb(189, 189, 189);

                            foreach (Control zz in aa.Controls)
                                if (zz.GetType() == typeof(Label) || zz.GetType() == typeof(Button) ||
                                    zz.GetType() == typeof(CheckBox) || zz.GetType() == typeof(RadioButton))
                                {
                                    zz.ForeColor = Color.White;
                                }
                                else if (zz.GetType() == typeof(TextBox) || zz.GetType() == typeof(ComboBox))
                                {
                                    zz.BackColor = Color.FromArgb(75, 75, 75);
                                    zz.ForeColor = Color.FromArgb(189, 189, 189);
                                }
                                else if (zz.GetType() == typeof(PresentationControls.CheckBoxComboBox)
                                            || zz.GetType() == typeof(PresentationControls.CheckBoxComboBoxItem)
                                            || zz.GetType() == typeof(PresentationControls.CheckBoxComboBoxItemList)
                                            || zz.GetType() == typeof(PresentationControls.CheckBoxComboBoxListControl)
                                            || zz.GetType() == typeof(PresentationControls.CheckBoxComboBoxListControlContainer))
                                {
                                    zz.BackColor = Color.FromArgb(75, 75, 75);
                                    zz.ForeColor = Color.FromArgb(189, 189, 189);
                                    zz.Font = new System.Drawing.Font("Rubik", 10F);
                                }
                        }
                }
                else if (c.GetType() == typeof(DataGridView))
                {
                    var MP_dataGridView = c as DataGridView;
                    MP_dataGridView.BackgroundColor = MP_dataGridView.ColumnHeadersDefaultCellStyle.BackColor =
                        MP_dataGridView.DefaultCellStyle.BackColor =
                            MP_dataGridView.RowsDefaultCellStyle.BackColor = Color.FromArgb(75, 75, 75);
                    MP_dataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(96, 96, 96);

                    MP_dataGridView.ForeColor = MP_dataGridView.ColumnHeadersDefaultCellStyle.ForeColor =
                        MP_dataGridView.DefaultCellStyle.ForeColor =
                            MP_dataGridView.RowsDefaultCellStyle.ForeColor =
                                MP_dataGridView.AlternatingRowsDefaultCellStyle.ForeColor =
                                    Color.FromArgb(189, 189, 189);

                    MP_dataGridView.GridColor = Color.FromArgb(189, 189, 189);

                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor =
                        MP_dataGridView.DefaultCellStyle.SelectionBackColor
                            = MP_dataGridView.RowsDefaultCellStyle.SelectionBackColor =
                                MP_dataGridView.AlternatingRowsDefaultCellStyle.SelectionBackColor =
                                    Color.FromArgb(56, 56, 56);
                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionForeColor =
                        MP_dataGridView.DefaultCellStyle.SelectionForeColor
                            = MP_dataGridView.RowsDefaultCellStyle.SelectionForeColor =
                                MP_dataGridView.AlternatingRowsDefaultCellStyle.SelectionForeColor =
                                    Color.FromArgb(189, 189, 189);

                    for (var i = 0; i < MP_dataGridView.Columns.Count; i++)
                    {
                        MP_dataGridView.Columns[i].HeaderCell.Style.BackColor = Color.FromArgb(75, 75, 75);
                        MP_dataGridView.Columns[i].HeaderCell.Style.ForeColor = Color.FromArgb(189, 189, 189);
                    }
                }
                else if (c.GetType() == typeof(AdvancedDataGridView))
                {
                    var MP_dataGridView = c as AdvancedDataGridView;
                    MP_dataGridView.BackgroundColor = MP_dataGridView.ColumnHeadersDefaultCellStyle.BackColor =
                        MP_dataGridView.DefaultCellStyle.BackColor =
                            MP_dataGridView.RowsDefaultCellStyle.BackColor = Color.FromArgb(75, 75, 75);
                    MP_dataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(96, 96, 96);

                    MP_dataGridView.ForeColor = MP_dataGridView.ColumnHeadersDefaultCellStyle.ForeColor =
                        MP_dataGridView.DefaultCellStyle.ForeColor =
                            MP_dataGridView.RowsDefaultCellStyle.ForeColor =
                                MP_dataGridView.AlternatingRowsDefaultCellStyle.ForeColor =
                                    Color.FromArgb(189, 189, 189);

                    MP_dataGridView.GridColor = Color.FromArgb(189, 189, 189);

                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor =
                        MP_dataGridView.DefaultCellStyle.SelectionBackColor
                            = MP_dataGridView.RowsDefaultCellStyle.SelectionBackColor =
                                MP_dataGridView.AlternatingRowsDefaultCellStyle.SelectionBackColor =
                                    Color.FromArgb(56, 56, 56);
                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionForeColor =
                        MP_dataGridView.DefaultCellStyle.SelectionForeColor
                            = MP_dataGridView.RowsDefaultCellStyle.SelectionForeColor =
                                MP_dataGridView.AlternatingRowsDefaultCellStyle.SelectionForeColor =
                                    Color.FromArgb(189, 189, 189);

                    for (var i = 0; i < MP_dataGridView.Columns.Count; i++)
                    {
                        MP_dataGridView.Columns[i].HeaderCell.Style.BackColor = Color.FromArgb(75, 75, 75);
                        MP_dataGridView.Columns[i].HeaderCell.Style.ForeColor = Color.FromArgb(189, 189, 189);
                    }
                }
        }

        public void Search_ToLight(Form form, bool Show)
        {
            form.BackColor = Color.FromArgb(240, 240, 240);
            form.ForeColor = Color.Black;
            foreach (Control c in form.Controls)
                if (c.GetType() == typeof(TableLayoutPanel)) /// main table
                {
                    c.BackColor = Color.FromArgb(240, 240, 240);
                    c.ForeColor = Color.Black;

                    foreach (Control aa in c.Controls)
                        if (aa.GetType() == typeof(Label) || aa.GetType() == typeof(Button) ||
                            aa.GetType() == typeof(CheckBox) || aa.GetType() == typeof(RadioButton))
                        {
                            aa.ForeColor = Color.Black;
                            //   aa.BackColor = Color.Transparent;

                            if (aa.Name == "Delete_button")
                                aa.BackgroundImage = Resources.Trash_D;
                            else if (aa.Name == "ExportToExcel_button")
                                aa.BackgroundImage = Resources.Excel_D;
                        }
                        else if (aa.GetType() == typeof(TextBox) || aa.GetType() == typeof(ComboBox))
                        {
                            aa.BackColor = Color.White;
                            aa.ForeColor = Color.Black;
                        }
                }
                else if (c.GetType() == typeof(Panel))
                {
                    c.BackColor = Color.FromArgb(240, 240, 240);
                    c.ForeColor = Color.Black;

                    foreach (Control aa in c.Controls)
                        if (aa.GetType() == typeof(Label) || aa.GetType() == typeof(Button) ||
                            aa.GetType() == typeof(CheckBox) || aa.GetType() == typeof(RadioButton))
                        {
                            aa.ForeColor = Color.Black;
                            if (aa.Name == "ShowHide_button")
                            {
                                if (Show)
                                    aa.BackgroundImage = Resources.Show2_D;
                                else aa.BackgroundImage = Resources.Hide2_D;
                            }
                        }
                        else if (aa.GetType() == typeof(TextBox) || aa.GetType() == typeof(ComboBox))
                        {
                            aa.BackColor = Color.White;
                            aa.ForeColor = Color.Black;
                        }
                        else if (aa.GetType() == typeof(TableLayoutPanel)) /// filter table
                        {
                            aa.BackColor = Color.FromArgb(240, 240, 240);
                            aa.ForeColor = Color.Black;

                            foreach (Control zz in aa.Controls)
                                if (zz.GetType() == typeof(Label) || zz.GetType() == typeof(Button) ||
                                    zz.GetType() == typeof(CheckBox) || zz.GetType() == typeof(RadioButton))
                                {
                                    zz.ForeColor = Color.Black;
                                }
                                else if (zz.GetType() == typeof(TextBox) || zz.GetType() == typeof(ComboBox))
                                {
                                    zz.BackColor = Color.White;
                                    zz.ForeColor = Color.Black;
                                }
                                else if (zz.GetType() == typeof(PresentationControls.CheckBoxComboBox)
                                            || zz.GetType() == typeof(PresentationControls.CheckBoxComboBoxItem)
                                            || zz.GetType() == typeof(PresentationControls.CheckBoxComboBoxItemList)
                                            || zz.GetType() == typeof(PresentationControls.CheckBoxComboBoxListControl)
                                            || zz.GetType() == typeof(PresentationControls.CheckBoxComboBoxListControlContainer))
                                {
                                    zz.BackColor = Color.White;
                                    zz.ForeColor = Color.Black;
                                    zz.Font = new System.Drawing.Font("Rubik", 10F);
                                }
                        }
                }
                else if (c.GetType() == typeof(DataGridView))
                {
                    var MP_dataGridView = c as DataGridView;
                    MP_dataGridView.BackgroundColor = MP_dataGridView.ColumnHeadersDefaultCellStyle.BackColor =
                        MP_dataGridView.DefaultCellStyle.BackColor =
                            MP_dataGridView.RowsDefaultCellStyle.BackColor = Color.White;
                    MP_dataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);

                    MP_dataGridView.ForeColor = MP_dataGridView.ColumnHeadersDefaultCellStyle.ForeColor =
                        MP_dataGridView.DefaultCellStyle.ForeColor =
                            MP_dataGridView.RowsDefaultCellStyle.ForeColor =
                                MP_dataGridView.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;

                    MP_dataGridView.GridColor = Color.FromArgb(96, 96, 96);

                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor =
                        MP_dataGridView.DefaultCellStyle.SelectionBackColor
                            = MP_dataGridView.RowsDefaultCellStyle.SelectionBackColor =
                                MP_dataGridView.AlternatingRowsDefaultCellStyle.SelectionBackColor =
                                    Color.FromArgb(189, 189, 189);
                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionForeColor =
                        MP_dataGridView.DefaultCellStyle.SelectionForeColor
                            = MP_dataGridView.RowsDefaultCellStyle.SelectionForeColor =
                                MP_dataGridView.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.Black;

                    for (var i = 0; i < MP_dataGridView.Columns.Count; i++)
                    {
                        MP_dataGridView.Columns[i].HeaderCell.Style.BackColor = Color.White;
                        MP_dataGridView.Columns[i].HeaderCell.Style.ForeColor = Color.Black;
                    }
                }
                else if (c.GetType() == typeof(AdvancedDataGridView))
                {
                    var MP_dataGridView = c as AdvancedDataGridView;
                    MP_dataGridView.BackgroundColor = MP_dataGridView.ColumnHeadersDefaultCellStyle.BackColor =
                        MP_dataGridView.DefaultCellStyle.BackColor =
                            MP_dataGridView.RowsDefaultCellStyle.BackColor = Color.White;
                    MP_dataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);

                    MP_dataGridView.ForeColor = MP_dataGridView.ColumnHeadersDefaultCellStyle.ForeColor =
                        MP_dataGridView.DefaultCellStyle.ForeColor =
                            MP_dataGridView.RowsDefaultCellStyle.ForeColor =
                                MP_dataGridView.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;

                    MP_dataGridView.GridColor = Color.FromArgb(96, 96, 96);

                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor =
                        MP_dataGridView.DefaultCellStyle.SelectionBackColor
                            = MP_dataGridView.RowsDefaultCellStyle.SelectionBackColor =
                                MP_dataGridView.AlternatingRowsDefaultCellStyle.SelectionBackColor =
                                    Color.FromArgb(189, 189, 189);
                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionForeColor =
                        MP_dataGridView.DefaultCellStyle.SelectionForeColor
                            = MP_dataGridView.RowsDefaultCellStyle.SelectionForeColor =
                                MP_dataGridView.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.Black;

                    for (var i = 0; i < MP_dataGridView.Columns.Count; i++)
                    {
                        MP_dataGridView.Columns[i].HeaderCell.Style.BackColor = Color.White;
                        MP_dataGridView.Columns[i].HeaderCell.Style.ForeColor = Color.Black;
                    }
                }
        }

        public void Statistics_ToNight(Form form, bool Show)
        {
            form.BackColor = Color.FromArgb(96, 96, 96);
            form.ForeColor = Color.FromArgb(189, 189, 189);
            foreach (Control c in form.Controls)
                if (c.GetType() == typeof(TableLayoutPanel)) /// main table
                {
                    c.BackColor = Color.FromArgb(96, 96, 96);
                    c.ForeColor = Color.FromArgb(189, 189, 189);

                    foreach (Control aa in c.Controls)
                        if (aa.GetType() == typeof(Label) || aa.GetType() == typeof(Button) ||
                            aa.GetType() == typeof(CheckBox) || aa.GetType() == typeof(RadioButton))
                        {
                            aa.ForeColor = Color.White;

                            if (aa.Name == "ExportToExcel_button")
                                aa.BackgroundImage = Resources.Excel_L;
                        }
                        else if (aa.GetType() == typeof(TextBox) || aa.GetType() == typeof(ComboBox))
                        {
                            aa.BackColor = Color.FromArgb(75, 75, 75);
                            aa.ForeColor = Color.FromArgb(189, 189, 189);
                        }
                }
                else if (c.GetType() == typeof(Panel))
                {
                    c.BackColor = Color.FromArgb(96, 96, 96);
                    c.ForeColor = Color.FromArgb(189, 189, 189);

                    foreach (Control aa in c.Controls)
                        if (aa.GetType() == typeof(Label) || aa.GetType() == typeof(Button) ||
                            aa.GetType() == typeof(CheckBox) || aa.GetType() == typeof(RadioButton))
                        {
                            aa.ForeColor = Color.White;

                            if (aa.Name == "AdvancedSearch_button")
                                if (Show)
                                    aa.BackgroundImage = Resources.Show2_L;
                                else aa.BackgroundImage = Resources.Hide2_L;
                        }
                        else if (aa.GetType() == typeof(TextBox) || aa.GetType() == typeof(ComboBox))
                        {
                            aa.BackColor = Color.FromArgb(75, 75, 75);
                            aa.ForeColor = Color.FromArgb(189, 189, 189);
                        }
                        else if (aa.GetType() == typeof(TableLayoutPanel)) /// filter table
                        {
                            aa.BackColor = Color.FromArgb(96, 96, 96);
                            aa.ForeColor = Color.FromArgb(189, 189, 189);

                            foreach (Control bb in aa.Controls)
                                if (bb.GetType() == typeof(Label) || bb.GetType() == typeof(Button) ||
                                    bb.GetType() == typeof(CheckBox) || bb.GetType() == typeof(RadioButton))
                                {
                                    bb.ForeColor = Color.White;
                                }
                                else if (bb.GetType() == typeof(TextBox) || bb.GetType() == typeof(ComboBox))
                                {
                                    bb.BackColor = Color.FromArgb(75, 75, 75);
                                    bb.ForeColor = Color.FromArgb(189, 189, 189);
                                }
                                else if (bb.GetType() == typeof(PresentationControls.CheckBoxComboBox)
                                    || bb.GetType() == typeof(PresentationControls.CheckBoxComboBoxItem)
                                    || bb.GetType() == typeof(PresentationControls.CheckBoxComboBoxItemList)
                                    || bb.GetType() == typeof(PresentationControls.CheckBoxComboBoxListControl)
                                    || bb.GetType() == typeof(PresentationControls.CheckBoxComboBoxListControlContainer))
                                            {
                                                bb.BackColor = Color.FromArgb(75, 75, 75);
                                                bb.ForeColor = Color.FromArgb(189, 189, 189);
                                                bb.Font = new System.Drawing.Font("Rubik", 10F);
                                }
                        }
                }
                else if (c.GetType() == typeof(Chart))
                {
                    var chart = c as Chart;
                    chart.BackColor = Color.FromArgb(75, 75, 75);
                    chart.ForeColor = Color.White;

                    for (int s = 0; s < chart.Series.Count; s++)
                    {
                        if (s % 2 == 0) chart.Series[s].Color =Color.FromArgb(96, 96, 96); 
                        else chart.Series[s].Color = Color.FromArgb(189, 189, 189);
                        chart.Series[s].LabelForeColor = Color.FromArgb(189, 189, 189);
                        chart.Series[s].ShadowColor = Color.FromArgb(189, 189, 189);
                        chart.Series[s].ShadowOffset = 2;
                        chart.Series[s].BorderColor = Color.FromArgb(233, 233, 233);
                    }

                    foreach (var ca in chart.ChartAreas)
                    {
                        ca.BackColor = Color.FromArgb(75, 75, 75);
                        ca.BackHatchStyle = ChartHatchStyle.None;
                        ca.BorderColor = Color.FromArgb(189, 189, 189);

                        ca.AxisX.InterlacedColor = Color.FromArgb(96, 96, 96);
                        ca.AxisX.LineColor = Color.FromArgb(189, 189, 189);
                        ca.AxisX.TitleForeColor = Color.FromArgb(189, 189, 189);
                        ca.AxisX.LabelStyle.ForeColor = Color.FromArgb(189, 189, 189);
                        ca.AxisX.MajorGrid.LineColor = Color.FromArgb(96, 96, 96);
                        ca.AxisX.MinorGrid.LineColor = Color.FromArgb(96, 96, 96);

                        ca.AxisY.InterlacedColor = Color.FromArgb(96, 96, 96);
                        ca.AxisY.LineColor = Color.FromArgb(189, 189, 189);
                        ca.AxisY.TitleForeColor = Color.FromArgb(189, 189, 189);
                        ca.AxisY.LabelStyle.ForeColor = Color.FromArgb(189, 189, 189);
                        ca.AxisY.MajorGrid.LineColor = Color.FromArgb(96, 96, 96);
                        ca.AxisY.MinorGrid.LineColor = Color.FromArgb(96, 96, 96);
                    }

                    foreach (var l in chart.Legends)
                    {
                        l.BackColor = Color.FromArgb(75, 75, 75);
                        l.ForeColor = Color.White;
                    }
                }
        }

        public void Statistics_ToLight(Form form, bool Show)
        {
            form.BackColor = Color.FromArgb(240, 240, 240);
            form.ForeColor = Color.Black;
            foreach (Control c in form.Controls)
                if (c.GetType() == typeof(TableLayoutPanel)) /// main table
                {
                    c.BackColor = Color.FromArgb(240, 240, 240);
                    c.ForeColor = Color.Black;

                    foreach (Control aa in c.Controls)
                        if (aa.GetType() == typeof(Label) || aa.GetType() == typeof(Button) ||
                            aa.GetType() == typeof(CheckBox) || aa.GetType() == typeof(RadioButton))
                        {
                            aa.ForeColor = Color.Black;
                            if (aa.Name == "ExportToExcel_button")
                                aa.BackgroundImage = Resources.Excel_D;
                        }
                        else if (aa.GetType() == typeof(TextBox) || aa.GetType() == typeof(ComboBox))
                        {
                            aa.BackColor = Color.White;
                            aa.ForeColor = Color.Black;
                        }
                }
                else if (c.GetType() == typeof(Panel))
                {
                    c.BackColor = Color.FromArgb(240, 240, 240);
                    c.ForeColor = Color.Black;

                    foreach (Control aa in c.Controls)
                        if (aa.GetType() == typeof(Label) || aa.GetType() == typeof(Button) ||
                            aa.GetType() == typeof(CheckBox) || aa.GetType() == typeof(RadioButton))
                        {
                            aa.ForeColor = Color.Black;
                            if (aa.Name == "AdvancedSearch_button")
                                if (Show)
                                    aa.BackgroundImage = Resources.Show2_D;
                                else aa.BackgroundImage = Resources.Hide2_D;
                        }
                        else if (aa.GetType() == typeof(TextBox) || aa.GetType() == typeof(ComboBox))
                        {
                            aa.BackColor = Color.White;
                            aa.ForeColor = Color.Black;
                        }
                        else if (aa.GetType() == typeof(TableLayoutPanel)) /// filter table
                        {
                            aa.BackColor = Color.FromArgb(240, 240, 240);
                            aa.ForeColor = Color.Black;

                            foreach (Control bb in aa.Controls)
                                if (bb.GetType() == typeof(Label) || bb.GetType() == typeof(Button) ||
                                    bb.GetType() == typeof(CheckBox) || bb.GetType() == typeof(RadioButton))
                                {
                                    bb.ForeColor = Color.Black;
                                }
                                else if (bb.GetType() == typeof(TextBox) || bb.GetType() == typeof(ComboBox))
                                {
                                    bb.BackColor = Color.White;
                                    bb.ForeColor = Color.Black;
                                }
                                else if (bb.GetType() == typeof(PresentationControls.CheckBoxComboBox)
                                        || bb.GetType() == typeof(PresentationControls.CheckBoxComboBoxItem)
                                        || bb.GetType() == typeof(PresentationControls.CheckBoxComboBoxItemList)
                                        || bb.GetType() == typeof(PresentationControls.CheckBoxComboBoxListControl)
                                        || bb.GetType() == typeof(PresentationControls.CheckBoxComboBoxListControlContainer))
                                {
                                    bb.BackColor = Color.White;
                                    bb.ForeColor = Color.Black;
                                    bb.Font = new System.Drawing.Font("Rubik", 10F);
                                }
                        }
                }
                else if (c.GetType() == typeof(Chart))
                {
                    var chart = c as Chart;
                    chart.BackColor = Color.White;
                    chart.ForeColor = Color.Black;

                    for(int s = 0;s< chart.Series.Count; s++)
                    {
                        if (s%2==0) chart.Series[s].Color = Color.FromArgb(129, 20, 20); //زوجي
                        else chart.Series[s].Color = Color.FromArgb(96, 96, 96); //فردي

                        chart.Series[s].LabelForeColor = Color.Black;
                        chart.Series[s].ShadowColor = Color.Black;
                        chart.Series[s].ShadowOffset = 2;
                        chart.Series[s].BorderColor = Color.FromArgb(75, 75, 75);

                    }

                    foreach (var ca in chart.ChartAreas)
                    {
                        ca.BackColor = Color.FromArgb(224, 224, 224);
                        ca.BackHatchStyle = ChartHatchStyle.None;
                        ca.BorderColor = Color.FromArgb(75, 75, 75);

                        ca.AxisX.InterlacedColor = Color.Black;
                        ca.AxisX.LineColor = Color.Black;
                        ca.AxisX.TitleForeColor = Color.Black;
                        ca.AxisX.LabelStyle.ForeColor = Color.Black;
                        ca.AxisX.MajorGrid.LineColor = Color.FromArgb(189, 189, 189);
                        ca.AxisX.MinorGrid.LineColor = Color.FromArgb(96, 96, 96);

                        ca.AxisY.InterlacedColor = Color.Black;
                        ca.AxisY.LineColor = Color.Black;
                        ca.AxisY.TitleForeColor = Color.Black;
                        ca.AxisY.LabelStyle.ForeColor = Color.Black;
                        ca.AxisY.MajorGrid.LineColor = Color.FromArgb(189, 189, 189);
                        ca.AxisY.MinorGrid.LineColor = Color.FromArgb(96, 96, 96);
                    }

                    foreach (var l in chart.Legends)
                    {
                        l.BackColor = Color.White;
                        l.ForeColor = Color.Black;
                    }
                }
        }

        public void Timeline_ToNight(Form form)
        {
            form.BackColor = Color.FromArgb(96, 96, 96);
            form.ForeColor = Color.FromArgb(189, 189, 189);
            foreach (Control c in form.Controls)
                if (c.GetType() == typeof(TableLayoutPanel)) /// main table
                {
                    c.BackColor = Color.FromArgb(96, 96, 96);
                    c.ForeColor = Color.FromArgb(189, 189, 189);

                    foreach (Control aa in c.Controls)
                        if (aa.GetType() == typeof(Label) || aa.GetType() == typeof(Button) ||
                            aa.GetType() == typeof(CheckBox) || aa.GetType() == typeof(RadioButton))
                        {
                            aa.ForeColor = Color.White;
                        }
                        else if (aa.GetType() == typeof(TextBox) || aa.GetType() == typeof(ComboBox))
                        {
                            aa.BackColor = Color.FromArgb(75, 75, 75);
                            aa.ForeColor = Color.FromArgb(189, 189, 189);
                        }
                        else if (aa.GetType() == typeof(DataGridView))
                        {
                            var MP_dataGridView = aa as DataGridView;
                            MP_dataGridView.BackgroundColor = MP_dataGridView.ColumnHeadersDefaultCellStyle.BackColor =
                                MP_dataGridView.DefaultCellStyle.BackColor =
                                    MP_dataGridView.RowsDefaultCellStyle.BackColor = Color.FromArgb(75, 75, 75);
                            MP_dataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(96, 96, 96);

                            MP_dataGridView.ForeColor = MP_dataGridView.ColumnHeadersDefaultCellStyle.ForeColor =
                                MP_dataGridView.DefaultCellStyle.ForeColor =
                                    MP_dataGridView.RowsDefaultCellStyle.ForeColor =
                                        MP_dataGridView.AlternatingRowsDefaultCellStyle.ForeColor =
                                            Color.FromArgb(189, 189, 189);

                            MP_dataGridView.GridColor = Color.FromArgb(189, 189, 189);

                            MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor =
                                MP_dataGridView.DefaultCellStyle.SelectionBackColor
                                    = MP_dataGridView.RowsDefaultCellStyle.SelectionBackColor =
                                        MP_dataGridView.AlternatingRowsDefaultCellStyle.SelectionBackColor =
                                            Color.FromArgb(56, 56, 56);
                            MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionForeColor =
                                MP_dataGridView.DefaultCellStyle.SelectionForeColor
                                    = MP_dataGridView.RowsDefaultCellStyle.SelectionForeColor =
                                        MP_dataGridView.AlternatingRowsDefaultCellStyle.SelectionForeColor =
                                            Color.FromArgb(189, 189, 189);

                            for (var i = 0; i < MP_dataGridView.Columns.Count; i++)
                            {
                                MP_dataGridView.Columns[i].HeaderCell.Style.BackColor = Color.FromArgb(75, 75, 75);
                                MP_dataGridView.Columns[i].HeaderCell.Style.ForeColor = Color.FromArgb(189, 189, 189);
                            }
                        }
                }
                else if (c.GetType() == typeof(Chart))
                {
                    var chart = c as Chart;
                    chart.BackColor = Color.FromArgb(75, 75, 75);
                    chart.ForeColor = Color.White;

                    foreach (var s in chart.Series)
                    {
                        //if (s.Name != "6")
                        s.Color = Color.FromArgb(189, 189, 189);
                        //else s.Color = Color.FromArgb(96, 96, 96);
                        s.LabelForeColor = Color.FromArgb(189, 189, 189);
                    }

                    chart.Legends[0].BackColor = Color.FromArgb(75, 75, 75);
                    chart.Legends[0].ForeColor = Color.White;
                }
        }

        public void Timeline_ToLight(Form form)
        {
            form.BackColor = Color.FromArgb(240, 240, 240);
            form.ForeColor = Color.Black;
            foreach (Control c in form.Controls)
                if (c.GetType() == typeof(TableLayoutPanel)) /// main table
                {
                    c.BackColor = Color.FromArgb(240, 240, 240);
                    c.ForeColor = Color.Black;

                    foreach (Control aa in c.Controls)
                        if (aa.GetType() == typeof(Label) || aa.GetType() == typeof(Button) ||
                            aa.GetType() == typeof(CheckBox) || aa.GetType() == typeof(RadioButton))
                        {
                            aa.ForeColor = Color.Black;
                        }
                        else if (aa.GetType() == typeof(TextBox) || aa.GetType() == typeof(ComboBox))
                        {
                            aa.BackColor = Color.White;
                            aa.ForeColor = Color.Black;
                        }
                        else if (aa.GetType() == typeof(DataGridView))
                        {
                            var MP_dataGridView = aa as DataGridView;
                            MP_dataGridView.BackgroundColor = MP_dataGridView.ColumnHeadersDefaultCellStyle.BackColor =
                                MP_dataGridView.DefaultCellStyle.BackColor =
                                    MP_dataGridView.RowsDefaultCellStyle.BackColor = Color.White;
                            MP_dataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);

                            MP_dataGridView.ForeColor = MP_dataGridView.ColumnHeadersDefaultCellStyle.ForeColor =
                                MP_dataGridView.DefaultCellStyle.ForeColor =
                                    MP_dataGridView.RowsDefaultCellStyle.ForeColor =
                                        MP_dataGridView.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;

                            MP_dataGridView.GridColor = Color.FromArgb(96, 96, 96);

                            MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor =
                                MP_dataGridView.DefaultCellStyle.SelectionBackColor
                                    = MP_dataGridView.RowsDefaultCellStyle.SelectionBackColor =
                                        MP_dataGridView.AlternatingRowsDefaultCellStyle.SelectionBackColor =
                                            Color.FromArgb(189, 189, 189);
                            MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionForeColor =
                                MP_dataGridView.DefaultCellStyle.SelectionForeColor
                                    = MP_dataGridView.RowsDefaultCellStyle.SelectionForeColor =
                                        MP_dataGridView.AlternatingRowsDefaultCellStyle.SelectionForeColor =
                                            Color.Black;

                            for (var i = 0; i < MP_dataGridView.Columns.Count; i++)
                            {
                                MP_dataGridView.Columns[i].HeaderCell.Style.BackColor = Color.White;
                                MP_dataGridView.Columns[i].HeaderCell.Style.ForeColor = Color.Black;
                            }
                        }
                }
                else if (c.GetType() == typeof(Chart))
                {
                    var chart = c as Chart;
                    chart.BackColor = Color.White;
                    chart.ForeColor = Color.Black;

                    foreach (var s in chart.Series)
                    {
                        if (s.Name == "6") s.Color = Color.FromArgb(217, 216, 214);
                        else s.Color = Color.FromArgb(129, 20, 20);
                        s.LabelForeColor = Color.Black;
                    }

                    chart.Legends[0].BackColor = Color.White;
                    chart.Legends[0].ForeColor = Color.Black;
                }
        }

        public void AdminPanel_ToNight(Form form, bool Show)
        {
            form.BackColor = Color.FromArgb(96, 96, 96);
            form.ForeColor = Color.FromArgb(189, 189, 189);
            foreach (Control c in form.Controls)
                if (c.GetType() == typeof(Panel))
                {
                    c.BackColor = Color.FromArgb(96, 96, 96);
                    c.ForeColor = Color.FromArgb(189, 189, 189);

                    foreach (Control aa in c.Controls)
                        if (aa.GetType() == typeof(Label) || aa.GetType() == typeof(Button) ||
                            aa.GetType() == typeof(CheckBox) || aa.GetType() == typeof(RadioButton))
                        {
                            aa.ForeColor = Color.White;
                            aa.BackColor = Color.Transparent;
                        }
                        else if (aa.GetType() == typeof(TextBox) || aa.GetType() == typeof(ComboBox))
                        {
                            aa.BackColor = Color.FromArgb(75, 75, 75);
                            aa.ForeColor = Color.FromArgb(189, 189, 189);
                        }
                        else if (aa.GetType() == typeof(TableLayoutPanel)) /// filter table
                        {
                            aa.BackColor = Color.FromArgb(96, 96, 96);
                            aa.ForeColor = Color.White;

                            foreach (Control bb in aa.Controls)
                                if (bb.GetType() == typeof(Label) || bb.GetType() == typeof(CheckBox) ||
                                    bb.GetType() == typeof(RadioButton))
                                {
                                    bb.ForeColor = Color.White;
                                    bb.BackColor = Color.Transparent;
                                }
                                else if (bb.GetType() == typeof(TextBox) || bb.GetType() == typeof(ComboBox))
                                {
                                    bb.BackColor = Color.FromArgb(75, 75, 75);
                                    bb.ForeColor = Color.FromArgb(189, 189, 189);
                                }
                                else if (bb.GetType() == typeof(PresentationControls.CheckBoxComboBox)
                                        || bb.GetType() == typeof(PresentationControls.CheckBoxComboBoxItem)
                                        || bb.GetType() == typeof(PresentationControls.CheckBoxComboBoxItemList)
                                        || bb.GetType() == typeof(PresentationControls.CheckBoxComboBoxListControl)
                                        || bb.GetType() == typeof(PresentationControls.CheckBoxComboBoxListControlContainer))
                                {
                                    bb.BackColor = Color.FromArgb(75, 75, 75);
                                    bb.ForeColor = Color.FromArgb(189, 189, 189);
                                    bb.Font = new System.Drawing.Font("Rubik", 10F);
                                }
                        }
                        else if (c.GetType() == typeof(AdvancedDataGridView))
                        {
                            var MP_dataGridView = c as AdvancedDataGridView;
                            MP_dataGridView.ColumnHeadersDefaultCellStyle.BackColor =
                                MP_dataGridView.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(75, 75, 75);

                            MP_dataGridView.ColumnHeadersDefaultCellStyle.ForeColor =
                                MP_dataGridView.RowHeadersDefaultCellStyle.ForeColor = Color.FromArgb(189, 189, 189);

                            MP_dataGridView.GridColor = Color.FromArgb(189, 189, 189);

                            MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor =
                                MP_dataGridView.DefaultCellStyle.SelectionBackColor
                                    = MP_dataGridView.RowsDefaultCellStyle.SelectionBackColor =
                                        MP_dataGridView.AlternatingRowsDefaultCellStyle.SelectionBackColor =
                                            Color.FromArgb(56, 56, 56);
                            MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionForeColor =
                                MP_dataGridView.DefaultCellStyle.SelectionForeColor
                                    = MP_dataGridView.RowsDefaultCellStyle.SelectionForeColor =
                                        MP_dataGridView.AlternatingRowsDefaultCellStyle.SelectionForeColor =
                                            Color.FromArgb(189, 189, 189);

                            for (var i = 0; i < MP_dataGridView.Columns.Count; i++)
                            {
                                MP_dataGridView.Columns[i].HeaderCell.Style.BackColor = Color.FromArgb(75, 75, 75);
                                MP_dataGridView.Columns[i].HeaderCell.Style.ForeColor = Color.FromArgb(189, 189, 189);
                            }
                        }
                        else if (c.GetType() == typeof(TableLayoutPanel)) /// colors table
                        {
                            c.BackColor = Color.FromArgb(96, 96, 96);
                            c.ForeColor = Color.White;

                            foreach (Control bb in c.Controls)
                            {
                                if (bb.GetType() == typeof(Label) || bb.GetType() == typeof(CheckBox) ||
                                    bb.GetType() == typeof(RadioButton))
                                {
                                    bb.ForeColor = Color.White;
                                    bb.BackColor = Color.Transparent;
                                }

                                if (bb.GetType() == typeof(Button))
                                {
                                    bb.ForeColor = Color.White;
                                    if (bb.Name == "Refresh_button")
                                    {
                                        bb.BackgroundImage = Resources.Refresh2_L;
                                    }
                                    else if (bb.Name == "ExportToExcel_button")
                                    {
                                        bb.BackgroundImage = Resources.Excel_L;
                                    } 
                                    else if (bb.Name == "ShowHide_button")
                                    {
                                        if (Show)
                                            bb.BackgroundImage = Resources.Show2_L;
                                        else bb.BackgroundImage = Resources.Hide2_L;
                                    }
                                }
                                else if (bb.GetType() == typeof(TextBox) || bb.GetType() == typeof(ComboBox))
                                {
                                    bb.BackColor = Color.FromArgb(75, 75, 75);
                                    bb.ForeColor = Color.FromArgb(189, 189, 189);
                                }
                            }
                        }
                }
        }

        public void AdminPanel_ToLight(Form form, bool Show)
        {
            form.BackColor = Color.FromArgb(240, 240, 240);
            form.ForeColor = Color.Black;
            foreach (Control c in form.Controls)
                if (c.GetType() == typeof(Panel))
                {
                    c.BackColor = Color.FromArgb(240, 240, 240);
                    c.ForeColor = Color.Black;

                    foreach (Control aa in c.Controls)
                        if (aa.GetType() == typeof(Label) || aa.GetType() == typeof(Button) ||
                            aa.GetType() == typeof(CheckBox) || aa.GetType() == typeof(RadioButton))
                        {
                            aa.ForeColor = Color.Black;
                            aa.BackColor = Color.Transparent;
                        }
                        else if (aa.GetType() == typeof(TextBox) || aa.GetType() == typeof(ComboBox))
                        {
                            aa.BackColor = Color.White;
                            aa.ForeColor = Color.Black;
                        }
                        else if (aa.GetType() == typeof(TableLayoutPanel))
                        {
                            aa.BackColor = Color.FromArgb(240, 240, 240);
                            aa.ForeColor = Color.Black;

                            foreach (Control bb in aa.Controls)
                                if (bb.GetType() == typeof(Label) || bb.GetType() == typeof(CheckBox) ||
                                    bb.GetType() == typeof(RadioButton))
                                {
                                    bb.ForeColor = Color.Black;
                                    bb.BackColor = Color.Transparent;
                                }
                                else if (bb.GetType() == typeof(TextBox) || bb.GetType() == typeof(ComboBox))
                                {
                                    bb.BackColor = Color.White;
                                    bb.ForeColor = Color.Black;
                                }
                                else if (bb.GetType() == typeof(PresentationControls.CheckBoxComboBox)
                                                || bb.GetType() == typeof(PresentationControls.CheckBoxComboBoxItem)
                                                || bb.GetType() == typeof(PresentationControls.CheckBoxComboBoxItemList)
                                                || bb.GetType() == typeof(PresentationControls.CheckBoxComboBoxListControl)
                                                || bb.GetType() == typeof(PresentationControls.CheckBoxComboBoxListControlContainer))
                                {
                                    bb.BackColor = Color.White;
                                    bb.ForeColor = Color.Black;
                                    bb.Font = new System.Drawing.Font("Rubik", 10F);
                                }
                        }
                }
                else if (c.GetType() == typeof(AdvancedDataGridView))
                {
                    var MP_dataGridView = c as AdvancedDataGridView;
                    MP_dataGridView.ColumnHeadersDefaultCellStyle.BackColor =
                        MP_dataGridView.RowHeadersDefaultCellStyle.BackColor = Color.White;

                    MP_dataGridView.ColumnHeadersDefaultCellStyle.ForeColor =
                        MP_dataGridView.RowHeadersDefaultCellStyle.ForeColor = Color.Black;
                    MP_dataGridView.GridColor = Color.FromArgb(189, 189, 189);

                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor =
                        MP_dataGridView.DefaultCellStyle.SelectionBackColor
                            = MP_dataGridView.RowsDefaultCellStyle.SelectionBackColor =
                                MP_dataGridView.AlternatingRowsDefaultCellStyle.SelectionBackColor =
                                    Color.FromArgb(217, 216, 214);
                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionForeColor =
                        MP_dataGridView.DefaultCellStyle.SelectionForeColor
                            = MP_dataGridView.RowsDefaultCellStyle.SelectionForeColor =
                                MP_dataGridView.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.Black;

                    for (var i = 0; i < MP_dataGridView.Columns.Count; i++)
                    {
                        MP_dataGridView.Columns[i].HeaderCell.Style.BackColor = Color.White;
                        MP_dataGridView.Columns[i].HeaderCell.Style.ForeColor = Color.Black;
                    }
                }
                else if (c.GetType() == typeof(TableLayoutPanel))
                {
                    c.BackColor = Color.FromArgb(240, 240, 240);
                    c.ForeColor = Color.Black;

                    foreach (Control bb in c.Controls)
                        if (bb.GetType() == typeof(Label) || bb.GetType() == typeof(CheckBox) ||
                            bb.GetType() == typeof(RadioButton))
                        {
                            bb.ForeColor = Color.Black;
                            bb.BackColor = Color.Transparent;
                        }
                        else if (bb.GetType() == typeof(Button))
                        {
                            bb.ForeColor = Color.Black;
                            if (bb.Name == "Refresh_button")
                                bb.BackgroundImage = Resources.Refresh2_D;
                            else if (bb.Name == "ExportToExcel_button")
                                bb.BackgroundImage = Resources.Excel_D;
                            else if (bb.Name == "ShowHide_button")
                            {
                                if (Show)
                                    bb.BackgroundImage = Resources.Show2_D;
                                else bb.BackgroundImage = Resources.Hide2_D;
                            }
                        }
                        else if (bb.GetType() == typeof(TextBox) || bb.GetType() == typeof(ComboBox))
                        {
                            bb.BackColor = Color.White;
                            bb.ForeColor = Color.Black;
                        }
                }
        }

        public void NotificationBox_ToNight(Form form)
        {
            form.BackColor = Color.FromArgb(96, 96, 96);
            form.ForeColor = Color.White;
            form.BackgroundImage = Resources.Back_600_D;
            foreach (Control c in form.Controls)
                if (c.GetType() == typeof(Label) || c.GetType() == typeof(Button) || c.GetType() == typeof(CheckBox) ||
                    c.GetType() == typeof(RadioButton))
                {
                    c.ForeColor = Color.White;
                }
                else if (c.GetType() == typeof(TextBox) || c.GetType() == typeof(ComboBox))
                {
                    c.BackColor = Color.FromArgb(75, 75, 75);
                    c.ForeColor = Color.White;
                }
                else if (c.GetType() == typeof(DataGridView))
                {
                    var MP_dataGridView = c as DataGridView;
                    MP_dataGridView.BackgroundColor = MP_dataGridView.ColumnHeadersDefaultCellStyle.BackColor =
                        MP_dataGridView.DefaultCellStyle.BackColor =
                            MP_dataGridView.RowsDefaultCellStyle.BackColor = Color.FromArgb(75, 75, 75);

                    MP_dataGridView.ForeColor = MP_dataGridView.ColumnHeadersDefaultCellStyle.ForeColor =
                        MP_dataGridView.DefaultCellStyle.ForeColor =
                            MP_dataGridView.RowsDefaultCellStyle.ForeColor = Color.White;

                    MP_dataGridView.GridColor = Color.FromArgb(189, 189, 189);

                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor =
                        MP_dataGridView.DefaultCellStyle.SelectionBackColor
                            = MP_dataGridView.RowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(96, 96, 96);
                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionForeColor =
                        MP_dataGridView.DefaultCellStyle.SelectionForeColor
                            = MP_dataGridView.RowsDefaultCellStyle.SelectionForeColor = Color.White;


                    for (var i = 0; i < MP_dataGridView.Columns.Count; i++)
                    {
                        MP_dataGridView.Columns[i].HeaderCell.Style.BackColor = Color.FromArgb(75, 75, 75);
                        MP_dataGridView.Columns[i].HeaderCell.Style.ForeColor = Color.White;
                        for (var j = 0; j < MP_dataGridView.Rows.Count; j++)
                        {
                            MP_dataGridView.Rows[j].Cells[i].Style.BackColor = Color.FromArgb(75, 75, 75);
                            MP_dataGridView.Rows[j].Cells[i].Style.ForeColor = Color.White;
                        }
                    }
                }
        }

        public void NotificationBox_ToLight(Form form)
        {
            form.BackColor = Color.FromArgb(240, 240, 240);
            form.ForeColor = Color.Black;
            form.BackgroundImage = Resources.Back_600;
            foreach (Control c in form.Controls)
                if (c.GetType() == typeof(Label) || c.GetType() == typeof(Button) || c.GetType() == typeof(CheckBox) ||
                    c.GetType() == typeof(RadioButton))
                {
                    c.ForeColor = Color.Black;
                    c.BackColor = Color.Transparent;

                    if (c.Name == "Refresh_button")
                        c.BackgroundImage = Resources.Refresh2_D;
                }
                else if (c.GetType() == typeof(TextBox) || c.GetType() == typeof(ComboBox))
                {
                    c.BackColor = Color.White;
                    c.ForeColor = Color.Black;
                }
                else if (c.GetType() == typeof(DataGridView))
                {
                    var MP_dataGridView = c as DataGridView;
                    MP_dataGridView.BackgroundColor = MP_dataGridView.ColumnHeadersDefaultCellStyle.BackColor =
                        MP_dataGridView.DefaultCellStyle.BackColor =
                            MP_dataGridView.RowsDefaultCellStyle.BackColor = Color.White;
                    MP_dataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(217, 216, 214);

                    MP_dataGridView.ForeColor = MP_dataGridView.ColumnHeadersDefaultCellStyle.ForeColor =
                        MP_dataGridView.DefaultCellStyle.ForeColor =
                            MP_dataGridView.RowsDefaultCellStyle.ForeColor =
                                MP_dataGridView.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;

                    MP_dataGridView.GridColor = Color.FromArgb(96, 96, 96);

                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor =
                        MP_dataGridView.DefaultCellStyle.SelectionBackColor
                            = MP_dataGridView.RowsDefaultCellStyle.SelectionBackColor =
                                MP_dataGridView.AlternatingRowsDefaultCellStyle.SelectionBackColor =
                                    Color.FromArgb(189, 189, 189);
                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionForeColor =
                        MP_dataGridView.DefaultCellStyle.SelectionForeColor
                            = MP_dataGridView.RowsDefaultCellStyle.SelectionForeColor =
                                MP_dataGridView.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.Black;

                    for (var i = 0; i < MP_dataGridView.Columns.Count; i++)
                    {
                        MP_dataGridView.Columns[i].HeaderCell.Style.BackColor = Color.White;
                        MP_dataGridView.Columns[i].HeaderCell.Style.ForeColor = Color.Black;
                    }
                }
        }

        public void MessageBox_ToNight(Form form)
        {
            form.BackColor = Color.FromArgb(96, 96, 96);
            form.ForeColor = Color.White;
            form.BackgroundImage = Resources.Back_700_D;

            foreach (Control c in form.Controls)
                if (c.GetType() == typeof(Label) || c.GetType() == typeof(Button) || c.GetType() == typeof(CheckBox) ||
                    c.GetType() == typeof(RadioButton))
                {
                    c.ForeColor = Color.White;
                    if (c.Name == "Close_button")
                        c.BackgroundImage = Resources.Exit_L;
                    else if (c.Name.Contains("_button"))
                        c.BackgroundImage = Properties.Resources.BtnBackground_L;
                }
                else if (c.GetType() == typeof(TextBox) || c.GetType() == typeof(ComboBox))
                {
                    c.BackColor = Color.FromArgb(75, 75, 75);
                    c.ForeColor = Color.White;
                }
                else if (c.GetType() == typeof(TableLayoutPanel)) /// colors table
                {
                    c.BackColor = Color.FromArgb(96, 96, 96);
                    c.ForeColor = Color.White;

                    foreach (Control bb in c.Controls)
                        if (bb.GetType() == typeof(Label))
                        {
                            bb.ForeColor = Color.White;
                        }
                        else if (bb.GetType() == typeof(TextBox) || bb.GetType() == typeof(RichTextBox) ||
                                 bb.GetType() == typeof(ComboBox))
                        {
                            bb.BackColor = Color.FromArgb(75, 75, 75);
                            bb.ForeColor = Color.White;
                        }
                }
        }

        public void MessageBox_ToLight(Form form)
        {
            form.BackColor = Color.FromArgb(240, 240, 240);
            form.ForeColor = Color.Black;
            form.BackgroundImage = Resources.Back_700;
            foreach (Control c in form.Controls)
                if (c.GetType() == typeof(Label) || c.GetType() == typeof(Button) || c.GetType() == typeof(CheckBox) ||
                    c.GetType() == typeof(RadioButton))
                {
                    c.ForeColor = Color.Black;
                    c.BackColor = Color.Transparent;
                    if (c.Name == "Close_button")
                        c.BackgroundImage = Resources.Exit_D;
                    else if (c.Name.Contains("_button"))
                        c.BackgroundImage = Properties.Resources.BtnBackground_D;
                }
                else if (c.GetType() == typeof(TableLayoutPanel))
                {
                    c.BackColor = Color.Transparent;
                    c.ForeColor = Color.Black;

                    foreach (Control bb in c.Controls)
                        if (bb.GetType() == typeof(Label))
                        {
                            bb.ForeColor = Color.Black;
                            bb.BackColor = Color.Transparent;
                        }
                        else if (bb.GetType() == typeof(TextBox) || bb.GetType() == typeof(RichTextBox) ||
                                 bb.GetType() == typeof(ComboBox))
                        {
                            bb.BackColor = Color.White;
                            bb.ForeColor = Color.Black;
                        }
                }
        }

        public void ShowDetails_Form_ToNight(Form form)
        {
            form.BackColor = Color.FromArgb(96, 96, 96);
            form.ForeColor = Color.White;
            form.BackgroundImage = Resources.Back_700_D;
            foreach (Control c in form.Controls)
                if (c.GetType() == typeof(Label) || c.GetType() == typeof(Button) || c.GetType() == typeof(CheckBox) ||
                    c.GetType() == typeof(RadioButton))
                {
                    c.ForeColor = Color.White;
                }
                else if (c.GetType() == typeof(AdvancedDataGridView))
                {
                    var MP_dataGridView = c as AdvancedDataGridView;
                    MP_dataGridView.BackgroundColor =
                        MP_dataGridView.ColumnHeadersDefaultCellStyle.BackColor =
                            MP_dataGridView.DefaultCellStyle.BackColor =
                                MP_dataGridView.RowsDefaultCellStyle.BackColor =
                                    Color.FromArgb(75, 75, 75);
                    MP_dataGridView.AlternatingRowsDefaultCellStyle.BackColor =
                        Color.FromArgb(96, 96, 96);

                    MP_dataGridView.ForeColor =
                        MP_dataGridView.ColumnHeadersDefaultCellStyle.ForeColor =
                            MP_dataGridView.DefaultCellStyle.ForeColor =
                                MP_dataGridView.RowsDefaultCellStyle.ForeColor =
                                    MP_dataGridView.AlternatingRowsDefaultCellStyle.ForeColor =
                                        Color.FromArgb(189, 189, 189);

                    MP_dataGridView.GridColor = Color.FromArgb(189, 189, 189);

                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor =
                        MP_dataGridView.DefaultCellStyle.SelectionBackColor
                            = MP_dataGridView.RowsDefaultCellStyle.SelectionBackColor =
                                MP_dataGridView.AlternatingRowsDefaultCellStyle.SelectionBackColor =
                                    Color.FromArgb(56, 56, 56);
                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionForeColor =
                        MP_dataGridView.DefaultCellStyle.SelectionForeColor
                            = MP_dataGridView.RowsDefaultCellStyle.SelectionForeColor =
                                MP_dataGridView.AlternatingRowsDefaultCellStyle.SelectionForeColor =
                                    Color.FromArgb(189, 189, 189);

                    for (var i = 0; i < MP_dataGridView.Columns.Count; i++)
                    {
                        MP_dataGridView.Columns[i].HeaderCell.Style.BackColor =
                            Color.FromArgb(75, 75, 75);
                        MP_dataGridView.Columns[i].HeaderCell.Style.ForeColor =
                            Color.FromArgb(189, 189, 189);
                    }
                }
                else if (c.GetType() == typeof(AdvancedDataGridViewSearchToolBar))
                { 
                    c.BackColor = Color.FromArgb(96, 96, 96);
                    c.ForeColor = Color.White;

                    foreach (Control aa in c.Controls)
                        if (aa.GetType() == typeof(Label) || aa.GetType() == typeof(Button) ||
                            aa.GetType() == typeof(CheckBox) || aa.GetType() == typeof(RadioButton))
                        {
                            aa.ForeColor = Color.White;
                            aa.BackColor = Color.Transparent;
                        }
                        else if (aa.GetType() == typeof(TextBox) || aa.GetType() == typeof(ComboBox))
                        {
                            aa.BackColor = Color.FromArgb(75, 75, 75);
                            aa.ForeColor = Color.White;
                        }
                }
                else if (c.GetType() == typeof(TableLayoutPanel)) /// main table
            {
                c.BackColor = Color.FromArgb(96, 96, 96);
                c.ForeColor = Color.FromArgb(189, 189, 189);

                foreach (Control aa in c.Controls)
                    if (aa.GetType() == typeof(Label) || aa.GetType() == typeof(Button) ||
                        aa.GetType() == typeof(CheckBox) || aa.GetType() == typeof(RadioButton))
                    {
                        aa.ForeColor = Color.White;
                        //   aa.BackColor = Color.Transparent;

                        if (aa.Name == "Delete_button")
                            aa.BackgroundImage = Resources.Trash_L;
                        else if (aa.Name == "ExportToExcel_button")
                            aa.BackgroundImage = Resources.Excel_L;
                    }
                    else if (aa.GetType() == typeof(TextBox) || aa.GetType() == typeof(ComboBox))
                    {
                        aa.BackColor = Color.FromArgb(75, 75, 75);
                        aa.ForeColor = Color.FromArgb(189, 189, 189);
                    }
            }
        }

        public void ShowDetails_Form_ToLight(Form form)
        {
            form.BackColor = Color.FromArgb(240, 240, 240);
            form.ForeColor = Color.Black;
            form.BackgroundImage = Resources.Back_700;
            foreach (Control c in form.Controls)
                if (c.GetType() == typeof(Label) || c.GetType() == typeof(Button) || c.GetType() == typeof(CheckBox) ||
                    c.GetType() == typeof(RadioButton))
                {
                    c.ForeColor = Color.Black;
                }
                else if (c.GetType() == typeof(AdvancedDataGridView))
                {
                    var MP_dataGridView = c as AdvancedDataGridView;
                    MP_dataGridView.BackgroundColor =
                        MP_dataGridView.ColumnHeadersDefaultCellStyle.BackColor =
                            MP_dataGridView.DefaultCellStyle.BackColor =
                                MP_dataGridView.RowsDefaultCellStyle.BackColor = Color.White;
                    MP_dataGridView.AlternatingRowsDefaultCellStyle.BackColor =
                        Color.FromArgb(240, 240, 240);

                    MP_dataGridView.ForeColor =
                        MP_dataGridView.ColumnHeadersDefaultCellStyle.ForeColor =
                            MP_dataGridView.DefaultCellStyle.ForeColor =
                                MP_dataGridView.RowsDefaultCellStyle.ForeColor =
                                    MP_dataGridView.AlternatingRowsDefaultCellStyle.ForeColor =
                                        Color.Black;

                    MP_dataGridView.GridColor = Color.FromArgb(96, 96, 96);

                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor =
                        MP_dataGridView.DefaultCellStyle.SelectionBackColor
                            = MP_dataGridView.RowsDefaultCellStyle.SelectionBackColor =
                                MP_dataGridView.AlternatingRowsDefaultCellStyle.SelectionBackColor =
                                    Color.FromArgb(189, 189, 189);
                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionForeColor =
                        MP_dataGridView.DefaultCellStyle.SelectionForeColor
                            = MP_dataGridView.RowsDefaultCellStyle.SelectionForeColor =
                                MP_dataGridView.AlternatingRowsDefaultCellStyle.SelectionForeColor =
                                    Color.Black;

                    for (var i = 0; i < MP_dataGridView.Columns.Count; i++)
                    {
                        MP_dataGridView.Columns[i].HeaderCell.Style.BackColor = Color.White;
                        MP_dataGridView.Columns[i].HeaderCell.Style.ForeColor = Color.Black;
                    }
                }
                else if (c.GetType() == typeof(AdvancedDataGridViewSearchToolBar))
                {
                    //var searchToolBar = c as AdvancedDataGridViewSearchToolBar;
                    //searchToolBar.BackColor = Color.White;
                    //searchToolBar.ForeColor = Color.Black;

                    c.BackColor = Color.FromArgb(240, 240, 240);
                    c.ForeColor = Color.Black;

                    foreach (Control aa in c.Controls)
                        if (aa.GetType() == typeof(Label) || aa.GetType() == typeof(Button) ||
                            aa.GetType() == typeof(CheckBox) || aa.GetType() == typeof(RadioButton))
                        {
                            aa.ForeColor = Color.Black;
                            aa.BackColor = Color.Transparent;
                        }
                        else if (aa.GetType() == typeof(TextBox) || aa.GetType() == typeof(ComboBox))
                        {
                            aa.BackColor = Color.White;
                            aa.ForeColor = Color.Black;
                        }
                }
                else if (c.GetType() == typeof(TableLayoutPanel)) /// main table
            {
                c.BackColor = Color.FromArgb(240, 240, 240);
                c.ForeColor = Color.Black;

                foreach (Control aa in c.Controls)
                    if (aa.GetType() == typeof(Label) || aa.GetType() == typeof(Button) ||
                        aa.GetType() == typeof(CheckBox) || aa.GetType() == typeof(RadioButton))
                    {
                        aa.ForeColor = Color.Black;
                        //   aa.BackColor = Color.Transparent;

                        if (aa.Name == "Delete_button")
                            aa.BackgroundImage = Resources.Trash_D;
                        else if (aa.Name == "ExportToExcel_button")
                            aa.BackgroundImage = Resources.Excel_D;
                    }
                    else if (aa.GetType() == typeof(TextBox) || aa.GetType() == typeof(ComboBox))
                    {
                        aa.BackColor = Color.White;
                        aa.ForeColor = Color.Black;
                    }
            }
        }

        public void Category_ToNight(Form form)
        {
            form.BackColor = Color.FromArgb(96, 96, 96);
            form.ForeColor = Color.White;
            form.BackgroundImage = Resources.Background_Border_D;
            foreach (Control c in form.Controls)
                if (c.GetType() == typeof(TableLayoutPanel)) /// header table
                {
                    c.BackColor = Color.Transparent;
                    c.ForeColor = Color.White;
                    foreach (Control aa in c.Controls)
                        if (aa.GetType() == typeof(Label) || aa.GetType() == typeof(Button) ||
                            aa.GetType() == typeof(CheckBox) || aa.GetType() == typeof(RadioButton))
                        {
                            aa.ForeColor = Color.White;
                            aa.BackColor = Color.Transparent;
                            if (aa.Name == "Close_button")
                                aa.BackgroundImage = Resources.Exit_L;
                        }
                        else if (aa.GetType() == typeof(TextBox) || aa.GetType() == typeof(ComboBox))
                        {
                            aa.BackColor = Color.FromArgb(75, 75, 75);
                            aa.ForeColor = Color.White;
                        }
                }
                else if (c.GetType() == typeof(Label) || c.GetType() == typeof(Button) ||
                         c.GetType() == typeof(CheckBox) || c.GetType() == typeof(RadioButton))
                {
                    c.ForeColor = Color.White;
                    if (c.Name == "Save_button")
                        c.BackgroundImage = Resources.Save_CL;
                    else if (c.Name == "Delete_button")
                        c.BackgroundImage = Resources.Delete_CL;
                }
                else if (c.GetType() == typeof(TextBox) || c.GetType() == typeof(ComboBox))
                {
                    c.BackColor = Color.FromArgb(75, 75, 75);
                    c.ForeColor = Color.White;
                }
                else if (c.GetType() == typeof(DataGridView))
                {
                    var MP_dataGridView = c as DataGridView;
                    MP_dataGridView.BackgroundColor = MP_dataGridView.ColumnHeadersDefaultCellStyle.BackColor =
                        MP_dataGridView.DefaultCellStyle.BackColor =
                            MP_dataGridView.RowsDefaultCellStyle.BackColor = Color.FromArgb(75, 75, 75);

                    MP_dataGridView.ForeColor = MP_dataGridView.ColumnHeadersDefaultCellStyle.ForeColor =
                        MP_dataGridView.DefaultCellStyle.ForeColor =
                            MP_dataGridView.RowsDefaultCellStyle.ForeColor = Color.White;

                    MP_dataGridView.GridColor = Color.FromArgb(189, 189, 189);

                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor =
                        MP_dataGridView.DefaultCellStyle.SelectionBackColor
                            = MP_dataGridView.RowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(96, 96, 96);
                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionForeColor =
                        MP_dataGridView.DefaultCellStyle.SelectionForeColor
                            = MP_dataGridView.RowsDefaultCellStyle.SelectionForeColor = Color.White;

                    for (var i = 0; i < MP_dataGridView.Columns.Count; i++)
                    {
                        MP_dataGridView.Columns[i].HeaderCell.Style.BackColor = Color.FromArgb(75, 75, 75);
                        MP_dataGridView.Columns[i].HeaderCell.Style.ForeColor = Color.White;
                        for (var j = 0; j < MP_dataGridView.Rows.Count; j++)
                        {
                            MP_dataGridView.Rows[j].Cells[i].Style.BackColor = Color.FromArgb(75, 75, 75);
                            MP_dataGridView.Rows[j].Cells[i].Style.ForeColor = Color.White;
                        }
                    }
                }
        }

        public void Category_ToLight(Form form)
        {
            form.BackColor = Color.FromArgb(240, 240, 240);
            form.ForeColor = Color.Black;
            form.BackgroundImage = Resources.Background_Border;
            foreach (Control c in form.Controls)
                if (c.GetType() == typeof(TableLayoutPanel)) /// header table
                {
                    c.BackColor = Color.Transparent;
                    c.ForeColor = Color.Black;
                    foreach (Control aa in c.Controls)
                        if (aa.GetType() == typeof(Label) || aa.GetType() == typeof(Button) ||
                            aa.GetType() == typeof(CheckBox) || aa.GetType() == typeof(RadioButton))
                        {
                            aa.ForeColor = Color.Black;
                            aa.BackColor = Color.Transparent;
                            if (aa.Name == "Close_button")
                                aa.BackgroundImage = Resources.Exit_D;
                        }
                        else if (aa.GetType() == typeof(TextBox) || aa.GetType() == typeof(ComboBox))
                        {
                            aa.BackColor = Color.White;
                            aa.ForeColor = Color.Black;
                        }
                }
                else if (c.GetType() == typeof(Label) || c.GetType() == typeof(Button) ||
                         c.GetType() == typeof(CheckBox) || c.GetType() == typeof(RadioButton))
                {
                    c.ForeColor = Color.Black;
                    c.BackColor = Color.Transparent;

                    if (c.Name == "Save_button")
                        c.BackgroundImage = Resources.Save_CD;
                    else if (c.Name == "Delete_button")
                        c.BackgroundImage = Resources.Delete_CD;
                }
                else if (c.GetType() == typeof(TextBox) || c.GetType() == typeof(ComboBox))
                {
                    c.BackColor = Color.White;
                    c.ForeColor = Color.Black;
                }
                else if (c.GetType() == typeof(DataGridView))
                {
                    var MP_dataGridView = c as DataGridView;
                    MP_dataGridView.BackgroundColor = MP_dataGridView.ColumnHeadersDefaultCellStyle.BackColor =
                        MP_dataGridView.DefaultCellStyle.BackColor =
                            MP_dataGridView.RowsDefaultCellStyle.BackColor = Color.White;
                    MP_dataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(217, 216, 214);

                    MP_dataGridView.ForeColor = MP_dataGridView.ColumnHeadersDefaultCellStyle.ForeColor =
                        MP_dataGridView.DefaultCellStyle.ForeColor =
                            MP_dataGridView.RowsDefaultCellStyle.ForeColor =
                                MP_dataGridView.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;

                    MP_dataGridView.GridColor = Color.FromArgb(96, 96, 96);

                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor =
                        MP_dataGridView.DefaultCellStyle.SelectionBackColor
                            = MP_dataGridView.RowsDefaultCellStyle.SelectionBackColor =
                                MP_dataGridView.AlternatingRowsDefaultCellStyle.SelectionBackColor =
                                    Color.FromArgb(189, 189, 189);
                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionForeColor =
                        MP_dataGridView.DefaultCellStyle.SelectionForeColor
                            = MP_dataGridView.RowsDefaultCellStyle.SelectionForeColor =
                                MP_dataGridView.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.Black;

                    for (var i = 0; i < MP_dataGridView.Columns.Count; i++)
                    {
                        MP_dataGridView.Columns[i].HeaderCell.Style.BackColor = Color.White;
                        MP_dataGridView.Columns[i].HeaderCell.Style.ForeColor = Color.Black;
                    }
                }
        }

        public void About_ToNight(Form form)
        {
            form.BackColor = Color.FromArgb(96, 96, 96);
            form.ForeColor = Color.White;
            foreach (Control c in form.Controls)
                if (c.GetType() == typeof(Panel)) /// header table
                {
                    c.BackColor = Color.Transparent;
                    c.ForeColor = Color.White;
                    foreach (Control aa in c.Controls)
                    {
                        if (aa.GetType() == typeof(Label) || aa.GetType() == typeof(Button) ||
                            aa.GetType() == typeof(CheckBox) || aa.GetType() == typeof(Panel))
                        {
                            aa.ForeColor = Color.White;
                            aa.BackColor = Color.Transparent;
                            if (aa.Name == "Close_button")
                                aa.BackgroundImage = Resources.Exit_L;
                        }
                        else if (aa.GetType() == typeof(TextBox) || aa.GetType() == typeof(ComboBox))
                        {
                            aa.BackColor = Color.FromArgb(75, 75, 75);
                            aa.ForeColor = Color.White;
                        }

                        if (aa.GetType() == typeof(TableLayoutPanel)) /// header table
                        {
                            aa.BackColor = Color.Transparent;
                            aa.ForeColor = Color.White;
                            foreach (Control bb in aa.Controls)
                                if (bb.GetType() == typeof(PictureBox))
                                    if (bb.Name == "Logo_Large_pictureBox")
                                        bb.BackgroundImage = Resources.Logo_Large_D;
                        }
                    }
                }
        }

        public void About_ToLight(Form form)
        {
            form.BackColor = Color.FromArgb(240, 240, 240);
            form.ForeColor = Color.Black;
            foreach (Control c in form.Controls)
                if (c.GetType() == typeof(Panel)) /// header table
                {
                    c.BackColor = Color.Transparent;
                    c.ForeColor = Color.Black;
                    foreach (Control aa in c.Controls)
                    {
                        if (aa.GetType() == typeof(Label) || aa.GetType() == typeof(Button) ||
                            aa.GetType() == typeof(CheckBox) || aa.GetType() == typeof(Panel))
                        {
                            aa.ForeColor = Color.Black;
                            aa.BackColor = Color.Transparent;
                            if (aa.Name == "Close_button")
                                aa.BackgroundImage = Resources.Exit_D;
                        }
                        else if (aa.GetType() == typeof(TextBox) || aa.GetType() == typeof(ComboBox))
                        {
                            aa.BackColor = Color.White;
                            aa.ForeColor = Color.Black;
                        }

                        if (aa.GetType() == typeof(TableLayoutPanel)) /// header table
                        {
                            aa.BackColor = Color.Transparent;
                            aa.ForeColor = Color.Black;
                            foreach (Control bb in aa.Controls)
                                if (bb.GetType() == typeof(PictureBox))
                                    if (bb.Name == "Logo_Large_pictureBox")
                                        bb.BackgroundImage = Resources.Logo_Large_L;
                        }
                    }
                }
        }

        public void User_ToNight(Form form)
        {
            form.BackColor = Color.FromArgb(96, 96, 96);
            form.ForeColor = Color.White;

            foreach (Control c in form.Controls)
                if (c.GetType() == typeof(TableLayoutPanel)) /// 3 Tables
                {
                    c.BackColor = Color.FromArgb(96, 96, 96);
                    c.ForeColor = Color.White;
                    foreach (Control aa in c.Controls)
                        if (aa.GetType() == typeof(Label) || aa.GetType() == typeof(Button) ||
                            aa.GetType() == typeof(CheckBox) || aa.GetType() == typeof(RadioButton))
                        {
                            aa.ForeColor = Color.White;
                            if (aa.Name.Contains("Header"))
                            {
                                aa.BackColor = Color.FromArgb(189, 189, 189);
                                aa.ForeColor = Color.FromArgb(75, 75, 75);
                            }
                            else if (aa.Name.Contains("Save_button") || aa.Name.Contains("New_button"))
                            {
                                aa.BackColor = Color.FromArgb(189, 189, 189);
                                aa.ForeColor = Color.FromArgb(75, 75, 75);
                            }
                        }
                        else if (aa.GetType() == typeof(TextBox) || aa.GetType() == typeof(ComboBox))
                        {
                            aa.BackColor = Color.FromArgb(75, 75, 75);
                            aa.ForeColor = Color.White;
                        }
                        else if (aa.GetType() == typeof(DataGridView))
                        {
                            var MP_dataGridView = aa as DataGridView;
                            MP_dataGridView.BackgroundColor = MP_dataGridView.ColumnHeadersDefaultCellStyle.BackColor =
                                MP_dataGridView.DefaultCellStyle.BackColor =
                                    MP_dataGridView.RowsDefaultCellStyle.BackColor = Color.FromArgb(75, 75, 75);
                            MP_dataGridView.ForeColor = MP_dataGridView.ColumnHeadersDefaultCellStyle.ForeColor =
                                MP_dataGridView.DefaultCellStyle.ForeColor =
                                    MP_dataGridView.RowsDefaultCellStyle.ForeColor = Color.White;

                            MP_dataGridView.GridColor = Color.FromArgb(189, 189, 189);

                            MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor =
                                MP_dataGridView.DefaultCellStyle.SelectionBackColor
                                    = MP_dataGridView.RowsDefaultCellStyle.SelectionBackColor =
                                        Color.FromArgb(96, 96, 96);
                            MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionForeColor =
                                MP_dataGridView.DefaultCellStyle.SelectionForeColor
                                    = MP_dataGridView.RowsDefaultCellStyle.SelectionForeColor = Color.White;


                            for (var i = 0; i < MP_dataGridView.Columns.Count; i++)
                            {
                                MP_dataGridView.Columns[i].HeaderCell.Style.BackColor = Color.FromArgb(75, 75, 75);
                                MP_dataGridView.Columns[i].HeaderCell.Style.ForeColor = Color.White;
                                for (var j = 0; j < MP_dataGridView.Rows.Count; j++)
                                {
                                    MP_dataGridView.Rows[j].Cells[i].Style.BackColor = Color.FromArgb(75, 75, 75);
                                    MP_dataGridView.Rows[j].Cells[i].Style.ForeColor = Color.White;
                                }
                            }
                        }
                        else if (aa.GetType() == typeof(TableLayoutPanel))
                        {
                            aa.BackColor = Color.FromArgb(96, 96, 96);
                            aa.ForeColor = Color.White;
                            foreach (Control bb in aa.Controls)
                                if (bb.GetType() == typeof(Label) || bb.GetType() == typeof(Button) ||
                                    bb.GetType() == typeof(CheckBox) || bb.GetType() == typeof(RadioButton))
                                {
                                    bb.ForeColor = Color.White;
                                    if (bb.Name == "AddImage_button")
                                        bb.BackgroundImage = Resources.Plus_Sq_L;
                                    else if (bb.Name == "DeleteImage_button")
                                        bb.BackgroundImage = Resources.Trash_L;
                                }
                        }
                }
        }

        public void User_ToLight(Form form)
        {
            form.BackColor = Color.FromArgb(240, 240, 240);
            form.ForeColor = Color.Black;
            foreach (Control c in form.Controls)
                if (c.GetType() == typeof(TableLayoutPanel)) /// 3 tables
                {
                    c.BackColor = Color.FromArgb(240, 240, 240);
                    c.ForeColor = Color.Black;
                    foreach (Control aa in c.Controls)
                        if (aa.GetType() == typeof(Label) || aa.GetType() == typeof(Button) ||
                            aa.GetType() == typeof(CheckBox) || aa.GetType() == typeof(RadioButton))
                        {
                            aa.ForeColor = Color.Black;
                            if (aa.Name.Contains("Header"))
                            {
                                aa.BackColor = Color.Transparent;
                            }
                            else if (aa.Name.Contains("Save_button") || aa.Name.Contains("New_button"))
                            {
                                aa.BackColor = Color.FromArgb(96, 96, 96);
                                aa.ForeColor = Color.White;
                            }
                        }
                        else if (aa.GetType() == typeof(TextBox) || aa.GetType() == typeof(ComboBox))
                        {
                            aa.BackColor = Color.White;
                            aa.ForeColor = Color.Black;
                        }
                        else if (aa.GetType() == typeof(DataGridView))
                        {
                            var MP_dataGridView = aa as DataGridView;
                            MP_dataGridView.BackgroundColor = MP_dataGridView.ColumnHeadersDefaultCellStyle.BackColor =
                                MP_dataGridView.DefaultCellStyle.BackColor =
                                    MP_dataGridView.RowsDefaultCellStyle.BackColor = Color.White;

                            MP_dataGridView.ForeColor = MP_dataGridView.ColumnHeadersDefaultCellStyle.ForeColor =
                                MP_dataGridView.DefaultCellStyle.ForeColor =
                                    MP_dataGridView.RowsDefaultCellStyle.ForeColor = Color.Black;

                            MP_dataGridView.GridColor = Color.FromArgb(96, 96, 96);

                            MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor =
                                MP_dataGridView.DefaultCellStyle.SelectionBackColor
                                    = MP_dataGridView.RowsDefaultCellStyle.SelectionBackColor =
                                        Color.FromArgb(189, 189, 189);
                            MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionForeColor =
                                MP_dataGridView.DefaultCellStyle.SelectionForeColor
                                    = MP_dataGridView.RowsDefaultCellStyle.SelectionForeColor = Color.Black;

                            for (var i = 0; i < MP_dataGridView.Columns.Count; i++)
                            {
                                MP_dataGridView.Columns[i].HeaderCell.Style.BackColor = Color.White;
                                MP_dataGridView.Columns[i].HeaderCell.Style.ForeColor = Color.Black;
                                for (var j = 0; j < MP_dataGridView.Rows.Count; j++)
                                {
                                    MP_dataGridView.Rows[j].Cells[i].Style.BackColor = Color.White;
                                    MP_dataGridView.Rows[j].Cells[i].Style.ForeColor = Color.Black;
                                }
                            }
                        }
                        else if (aa.GetType() == typeof(TableLayoutPanel))
                        {
                            aa.BackColor = Color.FromArgb(240, 240, 240);
                            aa.ForeColor = Color.Black;
                            foreach (Control bb in aa.Controls)
                                if (bb.GetType() == typeof(Label) || bb.GetType() == typeof(Button) ||
                                    bb.GetType() == typeof(CheckBox) || bb.GetType() == typeof(RadioButton))
                                {
                                    bb.ForeColor = Color.Black;
                                    if (bb.Name == "AddImage_button")
                                        bb.BackgroundImage = Resources.Plus_Sq_D;
                                    else if (bb.Name == "DeleteImage_button")
                                        bb.BackgroundImage = Resources.Trash_D;
                                }
                                else if (bb.GetType() == typeof(TextBox) || bb.GetType() == typeof(ComboBox))
                                {
                                    bb.BackColor = Color.White;
                                    bb.ForeColor = Color.Black;
                                }
                        }
                }
        }

        public void ChooseVisit_ToNight(Form form)
        {
            form.BackColor = Color.FromArgb(96, 96, 96);
            form.ForeColor = Color.White;
            form.BackgroundImage = Resources.Back_700_D;
            foreach (Control c in form.Controls)
                //if (c.GetType() == typeof(TableLayoutPanel)) /// 
                //{
                //    c.BackColor = Color.Transparent;
                //    c.ForeColor = Color.White;
                //    foreach (Control aa in c.Controls) 
                //} 
                if (c.GetType() == typeof(Label) ||
                            c.GetType() == typeof(CheckBox) || c.GetType() == typeof(RadioButton))
                {
                    c.ForeColor = Color.White;
                    c.BackColor = Color.Transparent;

                }
                else if (c.GetType() == typeof(TextBox) || c.GetType() == typeof(ComboBox))
                {
                    c.BackColor = Color.FromArgb(75, 75, 75);
                    c.ForeColor = Color.White;
                }
                else if (c.GetType() == typeof(DataGridView))
                {
                    var MP_dataGridView = c as DataGridView;
                    MP_dataGridView.BackgroundColor = MP_dataGridView.ColumnHeadersDefaultCellStyle.BackColor =
                        MP_dataGridView.DefaultCellStyle.BackColor =
                            MP_dataGridView.RowsDefaultCellStyle.BackColor = Color.FromArgb(75, 75, 75);

                    MP_dataGridView.ForeColor = MP_dataGridView.ColumnHeadersDefaultCellStyle.ForeColor =
                        MP_dataGridView.DefaultCellStyle.ForeColor =
                            MP_dataGridView.RowsDefaultCellStyle.ForeColor = Color.White;

                    MP_dataGridView.GridColor = Color.FromArgb(189, 189, 189);

                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor =
                        MP_dataGridView.DefaultCellStyle.SelectionBackColor
                            = MP_dataGridView.RowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(96, 96, 96);
                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionForeColor =
                        MP_dataGridView.DefaultCellStyle.SelectionForeColor
                            = MP_dataGridView.RowsDefaultCellStyle.SelectionForeColor = Color.White;

                    for (var i = 0; i < MP_dataGridView.Columns.Count; i++)
                    {
                        MP_dataGridView.Columns[i].HeaderCell.Style.BackColor = Color.FromArgb(75, 75, 75);
                        MP_dataGridView.Columns[i].HeaderCell.Style.ForeColor = Color.White;
                        for (var j = 0; j < MP_dataGridView.Rows.Count; j++)
                        {
                            MP_dataGridView.Rows[j].Cells[i].Style.BackColor = Color.FromArgb(75, 75, 75);
                            MP_dataGridView.Rows[j].Cells[i].Style.ForeColor = Color.White;
                        }
                    }
                }
                else if(c.GetType() == typeof(Button))
                {
                    if (c.Name == "Close_button")
                        c.BackgroundImage = Resources.Exit_L;
                }
        }
        public void ChooseVisit_ToLight(Form form)
        {
            form.BackColor = Color.FromArgb(240, 240, 240);
            form.ForeColor = Color.Black;
            form.BackgroundImage = Resources.Back_700;
            foreach (Control c in form.Controls)
                if (c.GetType() == typeof(Label) ||  c.GetType() == typeof(CheckBox) || c.GetType() == typeof(RadioButton))
                {
                    c.ForeColor = Color.Black;
                    c.BackColor = Color.Transparent;
                }
                else if (c.GetType() == typeof(TextBox) || c.GetType() == typeof(ComboBox))
                {
                    c.BackColor = Color.White;
                    c.ForeColor = Color.Black;
                }
                else if (c.GetType() == typeof(DataGridView))
                {
                    var MP_dataGridView = c as DataGridView;
                    MP_dataGridView.BackgroundColor = MP_dataGridView.ColumnHeadersDefaultCellStyle.BackColor =
                        MP_dataGridView.DefaultCellStyle.BackColor =
                            MP_dataGridView.RowsDefaultCellStyle.BackColor = Color.White;
                    MP_dataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(217, 216, 214);

                    MP_dataGridView.ForeColor = MP_dataGridView.ColumnHeadersDefaultCellStyle.ForeColor =
                        MP_dataGridView.DefaultCellStyle.ForeColor =
                            MP_dataGridView.RowsDefaultCellStyle.ForeColor =
                                MP_dataGridView.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;

                    MP_dataGridView.GridColor = Color.FromArgb(96, 96, 96);

                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor =
                        MP_dataGridView.DefaultCellStyle.SelectionBackColor
                            = MP_dataGridView.RowsDefaultCellStyle.SelectionBackColor =
                                MP_dataGridView.AlternatingRowsDefaultCellStyle.SelectionBackColor =
                                    Color.FromArgb(189, 189, 189);
                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionForeColor =
                        MP_dataGridView.DefaultCellStyle.SelectionForeColor
                            = MP_dataGridView.RowsDefaultCellStyle.SelectionForeColor =
                                MP_dataGridView.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.Black;

                    for (var i = 0; i < MP_dataGridView.Columns.Count; i++)
                    {
                        MP_dataGridView.Columns[i].HeaderCell.Style.BackColor = Color.White;
                        MP_dataGridView.Columns[i].HeaderCell.Style.ForeColor = Color.Black;
                    }
                }
                else if (c.GetType() == typeof(Button))
                {
                    if (c.Name == "Close_button")
                        c.BackgroundImage = Resources.Exit_D;
                }
            //if (c.GetType() == typeof(TableLayoutPanel)) /// table
            //{
            //    c.BackColor = Color.Transparent;
            //    c.ForeColor = Color.Black;
            //    foreach (Control aa in c.Controls)
            //        if (aa.GetType() == typeof(Label) || 
            //            aa.GetType() == typeof(CheckBox) || aa.GetType() == typeof(RadioButton))
            //        {
            //            aa.ForeColor = Color.Black;
            //            aa.BackColor = Color.Transparent;
            //        }
            //        else if (aa.GetType() == typeof(TextBox) || aa.GetType() == typeof(ComboBox))
            //        {
            //            aa.BackColor = Color.White;
            //            aa.ForeColor = Color.Black;
            //        } 
            //        else if (aa.GetType() == typeof(DataGridView))
            //        {
            //            var MP_dataGridView = aa as DataGridView;
            //            MP_dataGridView.BackgroundColor = MP_dataGridView.ColumnHeadersDefaultCellStyle.BackColor =
            //                MP_dataGridView.DefaultCellStyle.BackColor =
            //                    MP_dataGridView.RowsDefaultCellStyle.BackColor = Color.White;
            //            MP_dataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(217, 216, 214);

            //            MP_dataGridView.ForeColor = MP_dataGridView.ColumnHeadersDefaultCellStyle.ForeColor =
            //                MP_dataGridView.DefaultCellStyle.ForeColor =
            //                    MP_dataGridView.RowsDefaultCellStyle.ForeColor =
            //                        MP_dataGridView.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;

            //            MP_dataGridView.GridColor = Color.FromArgb(96, 96, 96);

            //            MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor =
            //                MP_dataGridView.DefaultCellStyle.SelectionBackColor
            //                    = MP_dataGridView.RowsDefaultCellStyle.SelectionBackColor =
            //                        MP_dataGridView.AlternatingRowsDefaultCellStyle.SelectionBackColor =
            //                            Color.FromArgb(189, 189, 189);
            //            MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionForeColor =
            //                MP_dataGridView.DefaultCellStyle.SelectionForeColor
            //                    = MP_dataGridView.RowsDefaultCellStyle.SelectionForeColor =
            //                        MP_dataGridView.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.Black;

            //            for (var i = 0; i < MP_dataGridView.Columns.Count; i++)
            //            {
            //                MP_dataGridView.Columns[i].HeaderCell.Style.BackColor = Color.White;
            //                MP_dataGridView.Columns[i].HeaderCell.Style.ForeColor = Color.Black;
            //            }
            //        } 
            //}

        }
         
    }
}