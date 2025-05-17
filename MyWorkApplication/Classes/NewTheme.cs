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
        public static Color light_white       = Color.FromArgb(255, 255, 255);
        public static Color light_transparent = Color.Transparent;

        public static Color light_grey1  = Color.FromArgb(240, 240, 240);
        public static Color light_grey11 = Color.FromArgb(214, 214, 214);
        public static Color light_grey2  = Color.FromArgb(189, 189, 189); 
        
        public static Color light_grey3 = Color.FromArgb(96, 96, 96);
        public static Color light_grey4 = Color.FromArgb(60, 60, 60);
        public static Color light_grey5 = Color.FromArgb(48, 48, 48);

        public static Color dark_grey0 = Color.FromArgb(233, 233, 233);
        public static Color dark_grey1 = Color.FromArgb(83, 83, 83);
        public static Color dark_grey2 = Color.FromArgb(75, 75, 75);
        public static Color dark_grey3 = Color.FromArgb(56, 56, 56);
        public static Color dark_grey4 = Color.FromArgb(31, 31, 31);

        private ToolStripItem[] GetAllChildren(ToolStripItem item)
        {
            var Items = new List<ToolStripItem> { item };
            if (item is ToolStripMenuItem)
                foreach (ToolStripItem i in ((ToolStripMenuItem)item).DropDownItems)
                    Items.AddRange(GetAllChildren(i));
            else if (item is ToolStripSplitButton)
                foreach (ToolStripItem i in ((ToolStripSplitButton)item).DropDownItems)
                    Items.AddRange(GetAllChildren(i));
            else if (item is ToolStripDropDownButton)
                foreach (ToolStripItem i in ((ToolStripDropDownButton)item).DropDownItems)
                    Items.AddRange(GetAllChildren(i));
            return Items.ToArray();
        }
        
        public void Visit_User_ToNight(Form form) {
            foreach (Control f in form.Controls)
                if (f.GetType() == typeof(TableLayoutPanel))  
                {
                    f.BackColor = light_grey3;
                    f.ForeColor = light_white;
                    foreach (Control c in f.Controls)
                    {
                        if (c.GetType() == typeof(TableLayoutPanel)) /// inner table
                        {
                            c.BackColor = light_grey3;
                            c.ForeColor = light_white;

                            foreach (Control aa in c.Controls)
                                if (aa.GetType() == typeof(Label) || aa.GetType() == typeof(Button) ||
                                    aa.GetType() == typeof(CheckBox) || aa.GetType() == typeof(RadioButton))
                                {
                                    aa.ForeColor = light_white;
                                    aa.BackColor = light_transparent;
                                    if (aa.Name.Contains("Header"))
                                    {
                                        aa.BackColor = light_grey2;
                                        aa.ForeColor = dark_grey2;
                                    }
                                }
                                else if (aa.GetType() == typeof(TextBox) || aa.GetType() == typeof(ComboBox))
                                {
                                    aa.BackColor = dark_grey2;
                                    aa.ForeColor = light_white;
                                }
                             
                        }
                        else if (c.GetType() == typeof(AdvancedDataGridView))
                        {
                            var MP_dataGridView = c as AdvancedDataGridView;
                            MP_dataGridView.BackgroundColor =
                                MP_dataGridView.ColumnHeadersDefaultCellStyle.BackColor =
                                    MP_dataGridView.DefaultCellStyle.BackColor =
                                        MP_dataGridView.RowsDefaultCellStyle.BackColor =
                                            dark_grey2;
                            MP_dataGridView.AlternatingRowsDefaultCellStyle.BackColor =
                                light_grey3;

                            MP_dataGridView.ForeColor =
                                MP_dataGridView.ColumnHeadersDefaultCellStyle.ForeColor =
                                    MP_dataGridView.DefaultCellStyle.ForeColor =
                                        MP_dataGridView.RowsDefaultCellStyle.ForeColor =
                                            MP_dataGridView.AlternatingRowsDefaultCellStyle.ForeColor =
                                                light_grey2;

                            MP_dataGridView.GridColor = light_grey2;

                            MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor =
                                MP_dataGridView.DefaultCellStyle.SelectionBackColor
                                    = MP_dataGridView.RowsDefaultCellStyle.SelectionBackColor =
                                        MP_dataGridView.AlternatingRowsDefaultCellStyle.SelectionBackColor =
                                            dark_grey3;
                            MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionForeColor =
                                MP_dataGridView.DefaultCellStyle.SelectionForeColor
                                    = MP_dataGridView.RowsDefaultCellStyle.SelectionForeColor =
                                        MP_dataGridView.AlternatingRowsDefaultCellStyle.SelectionForeColor =
                                            light_grey2;

                            for (var i = 0; i < MP_dataGridView.Columns.Count; i++)
                            {
                                MP_dataGridView.Columns[i].HeaderCell.Style.BackColor =
                                    dark_grey2;
                                MP_dataGridView.Columns[i].HeaderCell.Style.ForeColor =
                                    light_grey2;
                            }
                        }
                        else if (c.GetType() == typeof(AdvancedDataGridViewSearchToolBar))
                        {
                            //var searchToolBar = c as AdvancedDataGridViewSearchToolBar;
                            //searchToolBar.BackColor = dark_grey1;
                            //searchToolBar.ForeColor = light_white;

                            c.BackColor = light_grey3;
                            c.ForeColor = light_white;
                            
                            foreach (Control aa in c.Controls)
                                if (aa.GetType() == typeof(Label) || aa.GetType() == typeof(Button) ||
                                    aa.GetType() == typeof(CheckBox) || aa.GetType() == typeof(RadioButton))
                                {
                                    aa.ForeColor = light_white;
                                    aa.BackColor = light_transparent;
                                }
                                else if (aa.GetType() == typeof(TextBox) || aa.GetType() == typeof(ComboBox))
                                {
                                    aa.BackColor = dark_grey2;
                                    aa.ForeColor = light_white;
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
                    f.BackColor = light_grey1;
                    f.ForeColor = light_grey4;
                    foreach (Control c in f.Controls)
                    {
                        if (c.GetType() == typeof(TableLayoutPanel)) /// inner table
                        {
                            c.BackColor = light_grey1;
                            c.ForeColor = light_grey4;

                            foreach (Control aa in c.Controls)
                                if (aa.GetType() == typeof(Label) || aa.GetType() == typeof(Button) ||
                                    aa.GetType() == typeof(CheckBox) || aa.GetType() == typeof(RadioButton))
                                {
                                    aa.ForeColor = light_grey4;
                                    aa.BackColor = light_transparent;
                                }
                                else if (aa.GetType() == typeof(TextBox) || aa.GetType() == typeof(ComboBox))
                                {
                                    aa.BackColor = light_white;
                                    aa.ForeColor = light_grey4;
                                }


                        }
                        else if (c.GetType() == typeof(AdvancedDataGridView))
                        {
                            var MP_dataGridView = c as AdvancedDataGridView;
                            MP_dataGridView.BackgroundColor =
                                MP_dataGridView.ColumnHeadersDefaultCellStyle.BackColor =
                                    MP_dataGridView.DefaultCellStyle.BackColor =
                                        MP_dataGridView.RowsDefaultCellStyle.BackColor = light_white;
                            MP_dataGridView.AlternatingRowsDefaultCellStyle.BackColor =
                                light_grey1;

                            MP_dataGridView.ForeColor =
                                MP_dataGridView.ColumnHeadersDefaultCellStyle.ForeColor =
                                    MP_dataGridView.DefaultCellStyle.ForeColor =
                                        MP_dataGridView.RowsDefaultCellStyle.ForeColor =
                                            MP_dataGridView.AlternatingRowsDefaultCellStyle.ForeColor =
                                                light_grey4;

                            MP_dataGridView.GridColor = light_grey3;

                            MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor =
                                MP_dataGridView.DefaultCellStyle.SelectionBackColor
                                    = MP_dataGridView.RowsDefaultCellStyle.SelectionBackColor =
                                        MP_dataGridView.AlternatingRowsDefaultCellStyle.SelectionBackColor =
                                            light_grey2;
                            MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionForeColor =
                                MP_dataGridView.DefaultCellStyle.SelectionForeColor
                                    = MP_dataGridView.RowsDefaultCellStyle.SelectionForeColor =
                                        MP_dataGridView.AlternatingRowsDefaultCellStyle.SelectionForeColor =
                                            light_grey4;

                            for (var i = 0; i < MP_dataGridView.Columns.Count; i++)
                            {
                                MP_dataGridView.Columns[i].HeaderCell.Style.BackColor = light_white;
                                MP_dataGridView.Columns[i].HeaderCell.Style.ForeColor = light_grey4;
                            }
                        }
                        else if (c.GetType() == typeof(AdvancedDataGridViewSearchToolBar))
                        {
                            //var searchToolBar = c as AdvancedDataGridViewSearchToolBar;
                            //searchToolBar.BackColor = light_white;
                            //searchToolBar.ForeColor = light_grey4;

                            c.BackColor = light_grey1;
                            c.ForeColor = light_grey4;

                            foreach (Control aa in c.Controls)
                                if (aa.GetType() == typeof(Label) || aa.GetType() == typeof(Button) ||
                                    aa.GetType() == typeof(CheckBox) || aa.GetType() == typeof(RadioButton))
                                {
                                    aa.ForeColor = light_grey4;
                                    aa.BackColor = light_transparent;
                                }
                                else if (aa.GetType() == typeof(TextBox) || aa.GetType() == typeof(ComboBox))
                                {
                                    aa.BackColor = light_white;
                                    aa.ForeColor = light_grey4;
                                }
                        }

                    }
                }

            form.Refresh();
        }

        public void Main_ToNight(Form form, bool has_notification)
        {
            form.BackColor = dark_grey1;
            form.ForeColor = light_white;
            foreach (Control c in form.Controls)
                if (c.GetType() == typeof(Panel))
                {
                    if (c.Name == "TopLine_panel" || c.Name == "BottomLine_panel" || c.Name == "panel3")
                    {
                        c.ForeColor = light_grey4;
                        c.BackColor = light_grey4;
                        continue;
                    } 

                        c.BackColor = dark_grey1;
                        c.ForeColor = light_white;

                        foreach (Control aa in c.Controls)
                            if (aa.GetType() == typeof(Label) || aa.GetType() == typeof(Button) ||
                                aa.GetType() == typeof(CheckBox))
                            {
                                aa.ForeColor = light_white;
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
                                aa.BackColor = dark_grey1;
                                aa.ForeColor = light_white;
                                foreach (Control bb in aa.Controls)
                                    if (bb.GetType() == typeof(Label))
                                        bb.ForeColor = light_white;

                                    else if (bb.Name == "Logo_Small_pictureBox")
                                        bb.BackgroundImage = Resources.Logo_Small_D;
                            } 
                }
                else if (c.GetType() == typeof(PictureBox))
                {
                    c.BackColor = dark_grey1;
                    c.ForeColor = light_white;
                    if (c.Name == "Home_panel")
                    {
                        c.ForeColor = light_grey4;
                        c.BackColor = light_white;
                        continue;
                    }
                }
                else if (c.GetType() == typeof(MenuStrip))
                {
                    var menuStrip1 = c as MenuStrip;
                    menuStrip1.Renderer = new MenuStripRenderer();
                    menuStrip1.BackColor = dark_grey1;
                    menuStrip1.ForeColor = light_white;
                }
                else if (c is TabControl)
                {
                    var tabControl = c as TabControl;
                    tabControl.BackColor = dark_grey4;
                    tabControl.BackHighColor = dark_grey4;
                    tabControl.BackLowColor = dark_grey4;

                    tabControl.BorderColor = dark_grey1;
                    tabControl.BorderColorDisabled = dark_grey1;

                    tabControl.ControlButtonBackHighColor = dark_grey1;
                    tabControl.ControlButtonBackLowColor = dark_grey1;
                    tabControl.ControlButtonBorderColor = light_grey2;
                    tabControl.ControlButtonForeColor = light_grey2;

                    tabControl.ForeColor = light_grey2;
                    tabControl.ForeColorDisabled = light_grey2;

                    tabControl.TabBackHighColor = dark_grey1;
                    tabControl.TabBackHighColorDisabled = dark_grey3;
                    tabControl.TabBackLowColor = dark_grey1;
                    tabControl.TabBackLowColorDisabled = dark_grey3;
                    tabControl.TabCloseButtonImage = Resources.Exit_L;
                    tabControl.TabCloseButtonImageDisabled = Resources.Exit_L;
                    tabControl.TabCloseButtonImageHot = Resources.Exit_L;
                }

            form.Refresh();
        }
        public void Main_ToLight(Form form, bool has_notification)
        {
            form.BackColor = light_grey1;
            form.ForeColor = light_grey4;
            foreach (Control c in form.Controls)
                if (c.GetType() == typeof(Panel))
                {
                    if (c.Name == "TopLine_panel" || c.Name == "BottomLine_panel" || c.Name == "panel3")
                    {
                        c.ForeColor = light_grey2;
                        c.BackColor = light_grey2;
                        continue;
                    }
                     
                        c.BackColor = light_grey1;
                        c.ForeColor = light_grey4;

                        foreach (Control aa in c.Controls)
                            if (aa.GetType() == typeof(Label) || aa.GetType() == typeof(Button) ||
                                aa.GetType() == typeof(CheckBox))
                            {
                                aa.ForeColor = light_grey4;

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
                                aa.BackColor = light_grey1;
                                aa.ForeColor = light_grey4;
                                foreach (Control bb in aa.Controls)
                                    if (bb.GetType() == typeof(Label))
                                        bb.ForeColor = light_grey4;
                                    else if (bb.Name == "Logo_Small_pictureBox")
                                        bb.BackgroundImage = Resources.Logo_Small_L;
                            } 
                }
                else if (c.GetType() == typeof(PictureBox))
                {
                    c.BackColor = light_grey1;
                    c.ForeColor = light_grey4;
                    if (c.Name == "Home_panel")
                    {
                        c.ForeColor = light_grey4;
                        c.BackColor = light_white;
                        continue;
                    }
                }
                else if (c.GetType() == typeof(MenuStrip))
                {
                    var menuStrip1 = c as MenuStrip;
                    menuStrip1.Renderer = new MenuStripRenderer(0);
                    menuStrip1.BackColor = light_grey1;
                    menuStrip1.ForeColor = light_grey4;
                }
                else if (c is TabControl)
                {
                    var tabControl = c as TabControl;
                    tabControl.BackColor = light_grey2;
                    tabControl.BackHighColor = light_grey2;
                    tabControl.BackLowColor = light_grey2;

                    tabControl.BorderColor = light_grey1;
                    tabControl.BorderColorDisabled = light_grey1;

                    tabControl.ControlButtonBackHighColor = light_grey1;
                    tabControl.ControlButtonBackLowColor = light_grey1;
                    tabControl.ControlButtonBorderColor = light_grey5;
                    tabControl.ControlButtonForeColor = light_grey5;

                    tabControl.ForeColor = light_grey4;
                    tabControl.ForeColorDisabled = light_grey4;

                    tabControl.TabBackHighColor = light_grey1;
                    tabControl.TabBackHighColorDisabled = light_grey11;
                    tabControl.TabBackLowColor = light_grey1;
                    tabControl.TabBackLowColorDisabled = light_grey11;

                    //tabControl.TabCloseButtonBackHighColor = light_grey5;
                    //tabControl.TabCloseButtonBackHighColorDisabled = light_grey5;
                    //tabControl.TabCloseButtonBackHighColorHot = light_grey5;
                    //tabControl.TabCloseButtonBackLowColor = light_grey5;
                    //tabControl.TabCloseButtonBackLowColorDisabled = light_grey5;
                    //tabControl.TabCloseButtonBackLowColorHot = light_grey5;
                    //tabControl.TabCloseButtonBorderColor = light_grey5;
                    //tabControl.TabCloseButtonBorderColorDisabled = light_grey5;
                    //tabControl.TabCloseButtonBorderColorHot = light_grey5;
                    //tabControl.TabCloseButtonForeColor = light_grey5;
                    //tabControl.TabCloseButtonForeColorDisabled = light_grey5;
                    //tabControl.TabCloseButtonForeColorHot = light_grey5;

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
                    f.BackColor = light_grey3;
                    f.ForeColor = light_white;
                    foreach (Control c in f.Controls)
                        if (c.GetType() == typeof(TableLayoutPanel)) /// first & second
                        {
                            c.BackColor = light_grey3;
                            c.ForeColor = light_white;

                            foreach (Control aa in c.Controls)
                                if (aa.GetType() == typeof(Label) || aa.GetType() == typeof(Button) ||
                                    aa.GetType() == typeof(CheckBox) || aa.GetType() == typeof(RadioButton))
                                {
                                    aa.ForeColor = light_white;
                                    if (aa.Name == "SaveFamily_button" || aa.Name == "SaveWork_button")
                                    {
                                        aa.BackgroundImage = Resources.Save2_L;
                                    }
                                    else if (aa.Name.Contains("Header"))
                                    {
                                        aa.BackColor = light_grey2;
                                        aa.ForeColor = dark_grey2;
                                    }
                                    else if (aa.Name == "OverallSyrian_label")
                                    {
                                        aa.BackColor = dark_grey2;
                                    }
                                }
                                else if (aa.GetType() == typeof(TextBox) || aa.GetType() == typeof(ComboBox))
                                {
                                    aa.BackColor = dark_grey2;
                                    aa.ForeColor = light_white;
                                }
                                else if (aa.GetType() == typeof(BCDateTimePicker))
                                {
                                    var dd = aa as BCDateTimePicker;
                                    dd.BackColor = dark_grey2;
                                    dd.ForeColor = light_white;
                                }
                                else if (aa.GetType() == typeof(DataGridView))
                                {
                                    var MP_dataGridView = aa as DataGridView;
                                    MP_dataGridView.BackgroundColor =
                                        MP_dataGridView.ColumnHeadersDefaultCellStyle.BackColor =
                                            MP_dataGridView.DefaultCellStyle.BackColor =
                                                MP_dataGridView.RowsDefaultCellStyle.BackColor =
                                                    dark_grey2;
                                    MP_dataGridView.ForeColor =
                                        MP_dataGridView.ColumnHeadersDefaultCellStyle.ForeColor =
                                            MP_dataGridView.DefaultCellStyle.ForeColor =
                                                MP_dataGridView.RowsDefaultCellStyle.ForeColor = light_white;

                                    MP_dataGridView.GridColor = light_grey2;

                                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor =
                                        MP_dataGridView.DefaultCellStyle.SelectionBackColor
                                            = MP_dataGridView.RowsDefaultCellStyle.SelectionBackColor =
                                                light_grey3;
                                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionForeColor =
                                        MP_dataGridView.DefaultCellStyle.SelectionForeColor
                                            = MP_dataGridView.RowsDefaultCellStyle.SelectionForeColor = light_white;


                                    for (var i = 0; i < MP_dataGridView.Columns.Count; i++)
                                    {
                                        MP_dataGridView.Columns[i].HeaderCell.Style.BackColor =
                                            dark_grey2;
                                        MP_dataGridView.Columns[i].HeaderCell.Style.ForeColor = light_white;
                                        for (var j = 0; j < MP_dataGridView.Rows.Count; j++)
                                        {
                                            MP_dataGridView.Rows[j].Cells[i].Style.BackColor =
                                                dark_grey2;
                                            MP_dataGridView.Rows[j].Cells[i].Style.ForeColor = light_white;
                                        }
                                    }
                                }
                                else if (aa.GetType() == typeof(TableLayoutPanel))
                                {
                                    aa.BackColor = light_grey3;
                                    aa.ForeColor = light_white;
                                    foreach (Control bb in aa.Controls)
                                        if (bb.GetType() == typeof(Label) || bb.GetType() == typeof(Button) ||
                                            bb.GetType() == typeof(CheckBox) || bb.GetType() == typeof(RadioButton))
                                        {
                                            bb.ForeColor = light_white;
                                            bb.BackColor = light_transparent;
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
                                            bb.BackColor = dark_grey2;
                                            bb.ForeColor = light_white;
                                        }
                                        else if (bb.GetType() == typeof(BCDateTimePicker))
                                        {
                                            var dd = bb as BCDateTimePicker;
                                            dd.BackColor = dark_grey2;
                                            dd.ForeColor = light_white;
                                        }
                                        else if (bb.GetType() == typeof(TableLayoutPanel))
                                        {
                                            bb.BackColor = light_grey3;
                                            bb.ForeColor = light_white;
                                            foreach (Control zx in bb.Controls)
                                                if (zx.GetType() == typeof(Label) || zx.GetType() == typeof(Button) ||
                                                    zx.GetType() == typeof(CheckBox) ||
                                                    zx.GetType() == typeof(RadioButton))
                                                {
                                                    zx.ForeColor = light_white;
                                                }
                                                else if (zx.GetType() == typeof(TextBox) ||
                                                         zx.GetType() == typeof(ComboBox))
                                                {
                                                    zx.BackColor = dark_grey2;
                                                    zx.ForeColor = light_white;
                                                }
                                        }
                                }
                                else if (aa.GetType() == typeof(GroupBox))
                                {
                                    aa.BackColor = light_grey3;
                                    aa.ForeColor = light_white;
                                    foreach (Control bb in aa.Controls)
                                        if (bb.GetType() == typeof(TableLayoutPanel))
                                        {
                                            bb.BackColor = light_grey3;
                                            bb.ForeColor = light_white;
                                            foreach (Control mm in bb.Controls)
                                                if (mm.GetType() == typeof(Label) || mm.GetType() == typeof(Button) ||
                                                    mm.GetType() == typeof(CheckBox) ||
                                                    mm.GetType() == typeof(RadioButton))
                                                {
                                                    mm.ForeColor = light_white;
                                                }
                                                else if (mm.GetType() == typeof(TextBox) ||
                                                         mm.GetType() == typeof(ComboBox))
                                                {
                                                    mm.BackColor = dark_grey2;
                                                    mm.ForeColor = light_white;
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
                    f.BackColor = light_grey1;
                    f.ForeColor = light_grey4;
                    foreach (Control c in f.Controls)
                        if (c.GetType() == typeof(TableLayoutPanel)) /// first & second
                        {
                            c.BackColor = light_grey1;
                            c.ForeColor = light_grey4;

                            foreach (Control aa in c.Controls)
                                if (aa.GetType() == typeof(Label) || aa.GetType() == typeof(Button) ||
                                    aa.GetType() == typeof(CheckBox) || aa.GetType() == typeof(RadioButton))
                                {
                                    aa.ForeColor = light_grey4;
                                    if (aa.Name == "SaveFamily_button" || aa.Name == "SaveWork_button")
                                    {
                                        aa.BackgroundImage = Resources.Save2_D;
                                    }
                                    else if (aa.Name.Contains("Header"))
                                    {
                                        aa.BackColor = light_grey3;
                                        aa.ForeColor = light_white;
                                    }
                                    else if (aa.Name == "OverallSyrian_label")
                                    {
                                        aa.BackColor = light_white;
                                    }
                                }
                                else if (aa.GetType() == typeof(TextBox) || aa.GetType() == typeof(ComboBox))
                                {
                                    aa.BackColor = light_white;
                                    aa.ForeColor = light_grey4;
                                }
                                else if (aa.GetType() == typeof(BCDateTimePicker))
                                {
                                    var dd = aa as BCDateTimePicker;
                                    dd.BackColor = light_white;
                                    dd.ForeColor = light_grey4;
                                }
                                else if (aa.GetType() == typeof(DataGridView))
                                {
                                    var MP_dataGridView = aa as DataGridView;
                                    MP_dataGridView.BackgroundColor =
                                        MP_dataGridView.ColumnHeadersDefaultCellStyle.BackColor =
                                            MP_dataGridView.DefaultCellStyle.BackColor =
                                                MP_dataGridView.RowsDefaultCellStyle.BackColor = light_white;

                                    MP_dataGridView.ForeColor =
                                        MP_dataGridView.ColumnHeadersDefaultCellStyle.ForeColor =
                                            MP_dataGridView.DefaultCellStyle.ForeColor =
                                                MP_dataGridView.RowsDefaultCellStyle.ForeColor = light_grey4;

                                    MP_dataGridView.GridColor = light_grey3;

                                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor =
                                        MP_dataGridView.DefaultCellStyle.SelectionBackColor
                                            = MP_dataGridView.RowsDefaultCellStyle.SelectionBackColor =
                                                light_grey2;
                                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionForeColor =
                                        MP_dataGridView.DefaultCellStyle.SelectionForeColor
                                            = MP_dataGridView.RowsDefaultCellStyle.SelectionForeColor = light_grey4;

                                    for (var i = 0; i < MP_dataGridView.Columns.Count; i++)
                                    {
                                        MP_dataGridView.Columns[i].HeaderCell.Style.BackColor = light_white;
                                        MP_dataGridView.Columns[i].HeaderCell.Style.ForeColor = light_grey4;
                                        for (var j = 0; j < MP_dataGridView.Rows.Count; j++)
                                        {
                                            MP_dataGridView.Rows[j].Cells[i].Style.BackColor = light_white;
                                            MP_dataGridView.Rows[j].Cells[i].Style.ForeColor = light_grey4;
                                        }
                                    }
                                }
                                else if (aa.GetType() == typeof(TableLayoutPanel))
                                {
                                    aa.BackColor = light_grey1;
                                    aa.ForeColor = light_grey4;
                                    foreach (Control bb in aa.Controls)
                                        if (bb.GetType() == typeof(Label) || bb.GetType() == typeof(Button) ||
                                            bb.GetType() == typeof(CheckBox) || bb.GetType() == typeof(RadioButton))
                                        {
                                            bb.ForeColor = light_grey4;
                                            bb.BackColor = light_transparent;

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
                                            bb.BackColor = light_white;
                                            bb.ForeColor = light_grey4;
                                        }
                                        else if (bb.GetType() == typeof(BCDateTimePicker))
                                        {
                                            var dd = bb as BCDateTimePicker;
                                            dd.BackColor = light_white;
                                            dd.ForeColor = light_grey4;
                                        }
                                        else if (bb.GetType() == typeof(TableLayoutPanel))
                                        {
                                            bb.BackColor = light_grey1;
                                            bb.ForeColor = light_grey4;
                                            foreach (Control zx in bb.Controls)
                                                if (zx.GetType() == typeof(Label) || zx.GetType() == typeof(Button) ||
                                                    zx.GetType() == typeof(CheckBox) ||
                                                    zx.GetType() == typeof(RadioButton))
                                                {
                                                    zx.ForeColor = light_grey4;
                                                }
                                                else if (zx.GetType() == typeof(TextBox) ||
                                                         zx.GetType() == typeof(ComboBox))
                                                {
                                                    zx.BackColor = light_white;
                                                    zx.ForeColor = light_grey4;
                                                }
                                        }
                                }
                                else if (aa.GetType() == typeof(GroupBox))
                                {
                                    aa.BackColor = light_grey1;
                                    aa.ForeColor = light_grey4;
                                    foreach (Control bb in aa.Controls)
                                        if (bb.GetType() == typeof(TableLayoutPanel))
                                        {
                                            bb.BackColor = light_grey1;
                                            bb.ForeColor = light_grey4;
                                            foreach (Control zx in bb.Controls)
                                                if (zx.GetType() == typeof(Label) || zx.GetType() == typeof(Button) ||
                                                    zx.GetType() == typeof(CheckBox) ||
                                                    zx.GetType() == typeof(RadioButton))
                                                {
                                                    zx.ForeColor = light_grey4;
                                                }
                                                else if (zx.GetType() == typeof(TextBox) ||
                                                         zx.GetType() == typeof(ComboBox))
                                                {
                                                    zx.BackColor = light_white;
                                                    zx.ForeColor = light_grey4;
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
                    f.BackColor = light_grey3;
                    f.ForeColor = light_white;
                    foreach (Control c in f.Controls)
                        if (c.GetType() == typeof(TableLayoutPanel)) /// main table
                        {
                            c.BackColor = light_grey3;
                            c.ForeColor = light_white;

                            foreach (Control aa in c.Controls)
                                if (aa.GetType() == typeof(Label) || aa.GetType() == typeof(Button) ||
                                    aa.GetType() == typeof(CheckBox) || aa.GetType() == typeof(RadioButton))
                                {
                                    aa.ForeColor = light_white;
                                    aa.BackColor = light_transparent;
                                    if (aa.Name.Contains("Header"))
                                    {
                                        aa.BackColor = light_grey2;
                                        aa.ForeColor = dark_grey2;
                                    }
                                }
                                else if (aa.GetType() == typeof(TextBox) || aa.GetType() == typeof(ComboBox))
                                {
                                    aa.BackColor = dark_grey2;
                                    aa.ForeColor = light_white;
                                }
                                else if (aa.GetType() == typeof(BunifuDatepicker))
                                {
                                    var dd = aa as BunifuDatepicker;
                                    dd.BackColor = dark_grey2;
                                    dd.ForeColor = light_white;
                                }
                                else if (aa.GetType() == typeof(BCDateTimePicker))
                                {
                                    var dd = aa as BCDateTimePicker;
                                    dd.BackColor = dark_grey2;
                                    dd.ForeColor = light_white;
                                }
                                else if (aa.GetType() == typeof(AdvancedDataGridView))
                                { 
                                    var MP_dataGridView = aa as AdvancedDataGridView; 

                                    MP_dataGridView.BackgroundColor =
                                        MP_dataGridView.ColumnHeadersDefaultCellStyle.BackColor =
                                            MP_dataGridView.DefaultCellStyle.BackColor =
                                                MP_dataGridView.RowsDefaultCellStyle.BackColor =
                                                    dark_grey2;

                                    MP_dataGridView.ForeColor =
                                        MP_dataGridView.ColumnHeadersDefaultCellStyle.ForeColor =
                                            MP_dataGridView.DefaultCellStyle.ForeColor =
                                                MP_dataGridView.RowsDefaultCellStyle.ForeColor = light_white;

                                    MP_dataGridView.GridColor = light_grey2;

                                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor =
                                        MP_dataGridView.DefaultCellStyle.SelectionBackColor
                                            = MP_dataGridView.RowsDefaultCellStyle.SelectionBackColor =
                                                light_grey3;
                                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionForeColor =
                                        MP_dataGridView.DefaultCellStyle.SelectionForeColor
                                            = MP_dataGridView.RowsDefaultCellStyle.SelectionForeColor = light_white;


                                    for (var i = 0; i < MP_dataGridView.Columns.Count; i++)
                                    {
                                        MP_dataGridView.Columns[i].HeaderCell.Style.BackColor =
                                            dark_grey2;
                                        MP_dataGridView.Columns[i].HeaderCell.Style.ForeColor = light_white;
                                        for (var j = 0; j < MP_dataGridView.Rows.Count; j++)
                                        {
                                            MP_dataGridView.Rows[j].Cells[i].Style.BackColor =
                                                dark_grey2;
                                            MP_dataGridView.Rows[j].Cells[i].Style.ForeColor = light_white;
                                        }
                                    }
                                }
                                
                                else if (aa.GetType() == typeof(TableLayoutPanel)) // inner table
                                {
                                    aa.BackColor = light_grey3;
                                    aa.ForeColor = light_white;
                                    foreach (Control bb in aa.Controls)
                                        if (bb.GetType() == typeof(Label) || bb.GetType() == typeof(Button) ||
                                            bb.GetType() == typeof(CheckBox) || bb.GetType() == typeof(RadioButton))
                                        {
                                            bb.ForeColor = light_white;
                                            if (bb.Name == "InsertVisit_button" || bb.Name == "Save_button")
                                                bb.BackgroundImage = Resources.Save2_L;
                                            else if (bb.Name == "Delete_button")
                                                bb.BackgroundImage = Resources.Delete2_L;
                                        }
                                        else if (bb.GetType() == typeof(TextBox) || bb.GetType() == typeof(ComboBox))
                                        {
                                            bb.BackColor = dark_grey2;
                                            bb.ForeColor = light_white;
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
                    f.BackColor = light_grey1;
                    f.ForeColor = light_grey4;
                    foreach (Control c in f.Controls)
                        if (c.GetType() == typeof(TableLayoutPanel)) /// main table
                        {
                            c.BackColor = light_grey1;
                            c.ForeColor = light_grey4;

                            foreach (Control aa in c.Controls)
                                if (aa.GetType() == typeof(Label) || aa.GetType() == typeof(Button) ||
                                    aa.GetType() == typeof(CheckBox) || aa.GetType() == typeof(RadioButton))
                                {
                                    if (aa.Name.Contains("Header"))
                                    {
                                        aa.BackColor = light_grey3;
                                        aa.ForeColor = light_white;
                                    }

                                    aa.ForeColor = light_grey4;
                                    aa.BackColor = light_transparent;
                                }
                                else if (aa.GetType() == typeof(TextBox) || aa.GetType() == typeof(ComboBox))
                                {
                                    aa.BackColor = light_white;
                                    aa.ForeColor = light_grey4;
                                }
                                else if (aa.GetType() == typeof(BunifuDatepicker))
                                {
                                    var dd = aa as BunifuDatepicker;
                                    dd.BackColor = light_white;
                                    dd.ForeColor = light_grey4;
                                }
                                else if (aa.GetType() == typeof(BCDateTimePicker))
                                {
                                    var dd = aa as BCDateTimePicker;
                                    dd.BackColor = light_white;
                                    dd.ForeColor = light_grey4;
                                }
                                else if (aa.GetType() == typeof(AdvancedDataGridView))
                                {
                                    var MP_dataGridView = aa as AdvancedDataGridView;
                                    MP_dataGridView.BackgroundColor =
                                        MP_dataGridView.ColumnHeadersDefaultCellStyle.BackColor =
                                            MP_dataGridView.DefaultCellStyle.BackColor =
                                                MP_dataGridView.RowsDefaultCellStyle.BackColor = light_white;

                                    MP_dataGridView.ForeColor =
                                        MP_dataGridView.ColumnHeadersDefaultCellStyle.ForeColor =
                                            MP_dataGridView.DefaultCellStyle.ForeColor =
                                                MP_dataGridView.RowsDefaultCellStyle.ForeColor = light_grey4;

                                    MP_dataGridView.GridColor = light_grey3;

                                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor =
                                        MP_dataGridView.DefaultCellStyle.SelectionBackColor
                                            = MP_dataGridView.RowsDefaultCellStyle.SelectionBackColor =
                                                light_grey2;
                                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionForeColor =
                                        MP_dataGridView.DefaultCellStyle.SelectionForeColor
                                            = MP_dataGridView.RowsDefaultCellStyle.SelectionForeColor = light_grey4;


                                    for (var i = 0; i < MP_dataGridView.Columns.Count; i++)
                                    {
                                        MP_dataGridView.Columns[i].HeaderCell.Style.BackColor = light_white;
                                        MP_dataGridView.Columns[i].HeaderCell.Style.ForeColor = light_grey4;
                                        for (var j = 0; j < MP_dataGridView.Rows.Count; j++)
                                        {
                                            MP_dataGridView.Rows[j].Cells[i].Style.BackColor = light_white;
                                            MP_dataGridView.Rows[j].Cells[i].Style.ForeColor = light_grey4;
                                        }
                                    }
                                }

                                else if (aa.GetType() == typeof(TableLayoutPanel)) // inner table
                                {
                                    aa.BackColor = light_grey1;
                                    aa.ForeColor = light_grey4;
                                    foreach (Control bb in aa.Controls)
                                        if (bb.GetType() == typeof(Label) || bb.GetType() == typeof(Button) ||
                                            bb.GetType() == typeof(CheckBox) || bb.GetType() == typeof(RadioButton))
                                        {
                                            bb.ForeColor = light_grey4;
                                            bb.BackColor = light_transparent;
                                            if (bb.Name == "InsertVisit_button" || bb.Name == "Save_button")
                                                bb.BackgroundImage = Resources.Save2_D;
                                            else if (bb.Name == "Delete_button")
                                                bb.BackgroundImage = Resources.Delete2_D;
                                        }
                                        else if (bb.GetType() == typeof(TextBox) || bb.GetType() == typeof(ComboBox))
                                        {
                                            bb.BackColor = light_white;
                                            bb.ForeColor = light_grey4;
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

        public void ME_Visit_ToNight(Form form)
        {
            foreach (Control f in form.Controls)
                if (f.GetType() == typeof(Panel)) /// main panel
                {
                    f.BackColor = light_grey3;
                    f.ForeColor = light_white;
                    foreach (Control c in f.Controls)
                        if (c.GetType() == typeof(TableLayoutPanel)) /// tables 
                        {
                            c.BackColor = light_grey3;
                            c.ForeColor = light_white;

                            foreach (Control aa in c.Controls)
                                if (aa.GetType() == typeof(Label) || aa.GetType() == typeof(Button))
                                {
                                    aa.ForeColor = light_white;
                                    aa.BackColor = light_transparent;
                                    if (aa.Name.Contains("Header"))
                                    {
                                        aa.BackColor = light_grey2;
                                        aa.ForeColor = dark_grey2;
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
                                    aa.BackColor = dark_grey2;
                                    aa.ForeColor = light_white;
                                }
                                else if (aa.GetType() == typeof(BunifuDatepicker))
                                {
                                    var dd = aa as BunifuDatepicker;
                                    dd.BackColor = dark_grey2;
                                    dd.ForeColor = light_white;
                                }
                                else if (aa.GetType() == typeof(BCDateTimePicker))
                                {
                                    var dd = aa as BCDateTimePicker;
                                    dd.BackColor = dark_grey2;
                                    dd.ForeColor = light_white;
                                }
                                else if (aa.GetType() == typeof(TableLayoutPanel)) // inner table
                                {
                                    aa.BackColor = light_grey3;
                                    aa.ForeColor = light_white;
                                    foreach (Control bb in aa.Controls)
                                        if (bb.GetType() == typeof(Label) || bb.GetType() == typeof(Button) ||
                                            bb.GetType() == typeof(CheckBox) || bb.GetType() == typeof(RadioButton))
                                        {
                                            if (bb.GetType() == typeof(Label) && aa.Name.Contains("_h"))
                                            {
                                                bb.BackColor = light_grey2;
                                                bb.ForeColor = dark_grey2;
                                            }
                                            else
                                            {
                                                bb.ForeColor = light_white;
                                                if (bb.Name.Contains("id")) //Make it invisible//
                                                    bb.BackColor = bb.ForeColor = light_grey3;
                                            }
                                        }
                                        else if (bb.GetType() == typeof(TextBox) || bb.GetType() == typeof(ComboBox))
                                        {
                                            bb.BackColor = dark_grey2;
                                            bb.ForeColor = light_white;
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
                    f.BackColor = light_grey1;
                    f.ForeColor = light_grey4;
                    foreach (Control c in f.Controls)
                        if (c.GetType() == typeof(TableLayoutPanel)) /// tables
                        {
                            c.BackColor = light_grey1;
                            c.ForeColor = light_grey4;

                            foreach (Control aa in c.Controls)
                                if (aa.GetType() == typeof(Label) || aa.GetType() == typeof(Button))
                                {
                                    aa.ForeColor = light_grey4;
                                    aa.BackColor = light_transparent;
                                    if (aa.Name.Contains("Header"))
                                    {
                                        aa.BackColor = light_grey3;
                                        aa.ForeColor = light_white;
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
                                    aa.BackColor = light_white;
                                    aa.ForeColor = light_grey4;
                                }
                                else if (aa.GetType() == typeof(BunifuDatepicker))
                                {
                                    var dd = aa as BunifuDatepicker;
                                    dd.BackColor = light_white;
                                    dd.ForeColor = light_grey4;
                                }
                                else if (aa.GetType() == typeof(BCDateTimePicker))
                                {
                                    var dd = aa as BCDateTimePicker;
                                    dd.BackColor = light_white;
                                    dd.ForeColor = light_grey4;
                                }
                                else if (aa.GetType() == typeof(TableLayoutPanel)) // inner table
                                {
                                    aa.BackColor = light_grey1;
                                    aa.ForeColor = light_grey4;
                                    foreach (Control bb in aa.Controls)
                                        if (bb.GetType() == typeof(Label) || bb.GetType() == typeof(Button) ||
                                            bb.GetType() == typeof(CheckBox) || bb.GetType() == typeof(RadioButton))
                                        {
                                            if (bb.GetType() == typeof(Label) && aa.Name.Contains("_h"))
                                            {
                                                bb.BackColor = light_grey3;
                                                bb.ForeColor = light_white;
                                            }
                                            else
                                            {
                                                bb.ForeColor = light_grey4;
                                                bb.BackColor = light_transparent;
                                                if (bb.Name.Contains("id")) //Make it invisible
                                                    bb.BackColor = bb.ForeColor = light_grey1;
                                            }
                                        }
                                        else if (bb.GetType() == typeof(TextBox) || bb.GetType() == typeof(ComboBox))
                                        {
                                            bb.BackColor = light_white;
                                            bb.ForeColor = light_grey4;
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
                    f.BackColor = light_grey3;
                    f.ForeColor = light_white;
                    foreach (Control c in f.Controls)
                        if (c.GetType() == typeof(TableLayoutPanel))
                        {
                            /// main table
                            c.BackColor = light_grey3;
                            c.ForeColor = light_grey2;
                            foreach (Control aa in c.Controls)
                                if (aa.GetType() == typeof(Label) || aa.GetType() == typeof(Button) ||
                                    aa.GetType() == typeof(CheckBox) || aa.GetType() == typeof(RadioButton))
                                {
                                    aa.ForeColor = light_white;
                                }
                                else if (aa.GetType() == typeof(TextBox) || aa.GetType() == typeof(ComboBox))
                                {
                                    aa.BackColor = dark_grey2;
                                    aa.ForeColor = light_white;
                                }
                                else if (aa.GetType() == typeof(Panel))
                                {
                                    aa.BackColor = dark_grey2;
                                    aa.ForeColor = light_grey2;
                                    if (aa.Controls["pictureBox1"] != null) 
                                        aa.Controls["pictureBox1"].BackColor = dark_grey2;
                                }
                                else if (aa.GetType() == typeof(TableLayoutPanel))
                                {
                                    if (aa.Name == "Bottom_tableLayoutPanel")
                                    {
                                        aa.BackColor = dark_grey3;
                                        aa.ForeColor = light_grey2;
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
                                                bb.ForeColor = light_white;
                                            }
                                            else if (bb.GetType() == typeof(TextBox) ||
                                                     bb.GetType() == typeof(ComboBox))
                                            {
                                                bb.BackColor = dark_grey2;
                                                bb.ForeColor = light_white;
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
                    f.BackColor = light_grey3;
                    f.ForeColor = light_white;
                    foreach (Control c in f.Controls)
                        if (c.GetType() == typeof(TableLayoutPanel))
                        {
                            /// main table

                            c.BackColor = light_grey1;
                            c.ForeColor = light_grey4;
                            foreach (Control aa in c.Controls)
                                if (aa.GetType() == typeof(Label) || aa.GetType() == typeof(Button) ||
                                    aa.GetType() == typeof(CheckBox) || aa.GetType() == typeof(RadioButton))
                                {
                                    aa.ForeColor = light_grey4;
                                }
                                else if (aa.GetType() == typeof(TextBox) || aa.GetType() == typeof(ComboBox))
                                {
                                    aa.BackColor = light_white;
                                    aa.ForeColor = light_grey4;
                                }
                                else if (aa.GetType() == typeof(Panel))
                                {
                                    aa.BackColor = light_grey1;
                                    aa.ForeColor = light_grey4;
                                    if(aa.Controls["pictureBox1"] != null)
                                        aa.Controls["pictureBox1"].BackColor = light_grey1;
                                }
                                else if (aa.GetType() == typeof(TableLayoutPanel))
                                {
                                    if (aa.Name == "Bottom_tableLayoutPanel")
                                    {
                                        aa.BackColor = light_grey2;
                                        aa.ForeColor = light_grey4;
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
                                                bb.ForeColor = light_grey4;
                                            }
                                            else if (bb.GetType() == typeof(TextBox) ||
                                                     bb.GetType() == typeof(ComboBox))
                                            {
                                                bb.BackColor = light_white;
                                                bb.ForeColor = light_grey4;
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
            form.BackColor = light_grey3;
            form.ForeColor = light_white;
            foreach (Control f in form.Controls)
                if (f.GetType() == typeof(Panel)) /// main panel
                {
                    f.BackColor = light_grey3;
                    f.ForeColor = light_white;
                    foreach (Control c in f.Controls)
                        if (c.GetType() == typeof(TableLayoutPanel)) /// main table
                        {
                            c.BackColor = light_grey3;
                            c.ForeColor = light_white;

                            foreach (Control aa in c.Controls)
                                if (aa.GetType() == typeof(Label) || aa.GetType() == typeof(Button) ||
                                    aa.GetType() == typeof(CheckBox) || aa.GetType() == typeof(RadioButton))
                                {
                                    aa.ForeColor = light_white;
                                    aa.BackColor = light_transparent;
                                }
                                else if (aa.GetType() == typeof(TextBox) || aa.GetType() == typeof(ComboBox) ||
                                         aa.GetType() == typeof(MaskedTextBox))
                                {
                                    aa.BackColor = dark_grey2;
                                    aa.ForeColor = light_white;
                                }
                                else if (aa.GetType() == typeof(BCDateTimePicker))
                                {
                                    var dd = aa as BCDateTimePicker;
                                    dd.BackColor = dark_grey2;
                                    dd.ForeColor = light_white;
                                }
                                else if (aa.GetType() == typeof(DataGridView))
                                {
                                    var MP_dataGridView = aa as DataGridView;
                                    MP_dataGridView.BackgroundColor =
                                        MP_dataGridView.ColumnHeadersDefaultCellStyle.BackColor =
                                            MP_dataGridView.DefaultCellStyle.BackColor =
                                                MP_dataGridView.RowsDefaultCellStyle.BackColor =
                                                    dark_grey2;

                                    MP_dataGridView.ForeColor =
                                        MP_dataGridView.ColumnHeadersDefaultCellStyle.ForeColor =
                                            MP_dataGridView.DefaultCellStyle.ForeColor =
                                                MP_dataGridView.RowsDefaultCellStyle.ForeColor = light_white;

                                    MP_dataGridView.GridColor = light_grey2;

                                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor =
                                        MP_dataGridView.DefaultCellStyle.SelectionBackColor
                                            = MP_dataGridView.RowsDefaultCellStyle.SelectionBackColor =
                                                light_grey3;
                                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionForeColor =
                                        MP_dataGridView.DefaultCellStyle.SelectionForeColor
                                            = MP_dataGridView.RowsDefaultCellStyle.SelectionForeColor = light_white;


                                    for (var i = 0; i < MP_dataGridView.Columns.Count; i++)
                                    {
                                        MP_dataGridView.Columns[i].HeaderCell.Style.BackColor =
                                            dark_grey2;
                                        MP_dataGridView.Columns[i].HeaderCell.Style.ForeColor = light_white;
                                        for (var j = 0; j < MP_dataGridView.Rows.Count; j++)
                                        {
                                            MP_dataGridView.Rows[j].Cells[i].Style.BackColor =
                                                dark_grey2;
                                            MP_dataGridView.Rows[j].Cells[i].Style.ForeColor = light_white;
                                        }
                                    }
                                }
                                else if (aa.GetType() == typeof(TableLayoutPanel)) // inner table
                                {
                                    aa.BackColor = light_grey3;
                                    aa.ForeColor = light_white;
                                    foreach (Control bb in aa.Controls)
                                    {
                                        if (bb.GetType() == typeof(Label) || bb.GetType() == typeof(Button) ||
                                            bb.GetType() == typeof(CheckBox) || bb.GetType() == typeof(RadioButton))
                                        {
                                            bb.ForeColor = light_white;
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
            form.BackColor = light_grey1;
            form.ForeColor = light_grey4;
            foreach (Control f in form.Controls)
                if (f.GetType() == typeof(Panel)) /// main panel
                {
                    f.BackColor = light_grey1;
                    f.ForeColor = light_grey4;
                    foreach (Control c in f.Controls)
                        if (c.GetType() == typeof(TableLayoutPanel)) /// main table
                        {
                            c.BackColor = light_grey1;
                            c.ForeColor = light_grey4;

                            foreach (Control aa in c.Controls)
                                if (aa.GetType() == typeof(Label) || 
                                    aa.GetType() == typeof(CheckBox) || aa.GetType() == typeof(RadioButton))
                                {
                                    aa.ForeColor = light_grey4;
                                    aa.BackColor = light_transparent;
                                }
                            else if(aa.GetType() == typeof(Button))
                                {
                                    aa.ForeColor = light_white;
                                    aa.BackColor = light_transparent;
                                }
                                else if (aa.GetType() == typeof(TextBox) || aa.GetType() == typeof(ComboBox) ||
                                         aa.GetType() == typeof(MaskedTextBox))
                                {
                                    aa.BackColor = light_white;
                                    aa.ForeColor = light_grey4;
                                }
                                else if (aa.GetType() == typeof(BCDateTimePicker))
                                {
                                    var dd = aa as BCDateTimePicker;
                                    dd.BackColor = light_white;
                                    dd.ForeColor = light_grey4;
                                }
                                else if (aa.GetType() == typeof(DataGridView))
                                {
                                    var MP_dataGridView = aa as DataGridView;
                                    MP_dataGridView.BackgroundColor =
                                        MP_dataGridView.ColumnHeadersDefaultCellStyle.BackColor =
                                            MP_dataGridView.DefaultCellStyle.BackColor =
                                                MP_dataGridView.RowsDefaultCellStyle.BackColor = light_white;

                                    MP_dataGridView.ForeColor =
                                        MP_dataGridView.ColumnHeadersDefaultCellStyle.ForeColor =
                                            MP_dataGridView.DefaultCellStyle.ForeColor =
                                                MP_dataGridView.RowsDefaultCellStyle.ForeColor = light_grey4;

                                    MP_dataGridView.GridColor = light_grey3;

                                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor =
                                        MP_dataGridView.DefaultCellStyle.SelectionBackColor
                                            = MP_dataGridView.RowsDefaultCellStyle.SelectionBackColor =
                                                light_grey2;
                                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionForeColor =
                                        MP_dataGridView.DefaultCellStyle.SelectionForeColor
                                            = MP_dataGridView.RowsDefaultCellStyle.SelectionForeColor = light_grey4;


                                    for (var i = 0; i < MP_dataGridView.Columns.Count; i++)
                                    {
                                        MP_dataGridView.Columns[i].HeaderCell.Style.BackColor = light_white;
                                        MP_dataGridView.Columns[i].HeaderCell.Style.ForeColor = light_grey4;
                                        for (var j = 0; j < MP_dataGridView.Rows.Count; j++)
                                        {
                                            MP_dataGridView.Rows[j].Cells[i].Style.BackColor = light_white;
                                            MP_dataGridView.Rows[j].Cells[i].Style.ForeColor = light_grey4;
                                        }
                                    }
                                }
                                else if (aa.GetType() == typeof(TableLayoutPanel)) // inner table
                                {
                                    aa.BackColor = light_grey1;
                                    aa.ForeColor = light_grey4;
                                    foreach (Control bb in aa.Controls)
                                        if (bb.GetType() == typeof(Label) || bb.GetType() == typeof(Button) ||
                                            bb.GetType() == typeof(CheckBox) || bb.GetType() == typeof(RadioButton))
                                        {
                                            bb.ForeColor = light_grey4; 
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
            form.BackColor = light_grey3;
            form.ForeColor = light_white;
            foreach (Control c in form.Controls)
                if (c.GetType() == typeof(TableLayoutPanel))
                {
                    c.BackColor = light_grey3;
                    c.ForeColor = light_white;
                    foreach (Control aa in c.Controls)
                        if (aa.GetType() == typeof(Label) || aa.GetType() == typeof(Button) ||
                            aa.GetType() == typeof(CheckBox) || aa.GetType() == typeof(RadioButton))
                        {
                            aa.ForeColor = light_white;
                            aa.BackColor = light_transparent;

                            if (aa.Name == "Refresh_button")
                                aa.BackgroundImage = Resources.Refresh2_L;
                            if (aa.Name == "ExportToExcel_button")
                                aa.BackgroundImage = Resources.Excel_L;
                        }
                        else if (aa.GetType() == typeof(TextBox) || aa.GetType() == typeof(ComboBox))
                        {
                            aa.BackColor = dark_grey2;
                            aa.ForeColor = light_white;
                        }
                }
                else if (c.GetType() == typeof(Panel))
                {
                    c.BackColor = light_grey3;
                    c.ForeColor = light_white;

                    foreach (Control bb in c.Controls)
                        if (bb.GetType() == typeof(TableLayoutPanel)) /// combo
                        {
                            if (bb.Name.Contains("Filter"))
                                bb.BackColor = dark_grey2;
                            else
                                bb.BackColor = light_grey3;
                            bb.ForeColor = light_white;

                            foreach (Control aa in bb.Controls)
                                if (aa.GetType() == typeof(TextBox) || aa.GetType() == typeof(ComboBox))
                                {
                                    aa.BackColor = dark_grey2;
                                    aa.ForeColor = light_white;
                                    if (aa.Name.Contains("Filter"))
                                        aa.BackColor = dark_grey2;
                                    else
                                        aa.BackColor = light_grey3;
                                    aa.ForeColor = light_white;
                                }
                                else if (aa.GetType() == typeof(DataGridView))
                                {
                                    var MP_dataGridView = aa as DataGridView;
                                    MP_dataGridView.BackgroundColor =
                                        MP_dataGridView.ColumnHeadersDefaultCellStyle.BackColor =
                                            MP_dataGridView.DefaultCellStyle.BackColor =
                                                MP_dataGridView.RowsDefaultCellStyle.BackColor =
                                                    dark_grey2;
                                    MP_dataGridView.AlternatingRowsDefaultCellStyle.BackColor =
                                        light_grey3;

                                    MP_dataGridView.ForeColor =
                                        MP_dataGridView.ColumnHeadersDefaultCellStyle.ForeColor =
                                            MP_dataGridView.DefaultCellStyle.ForeColor =
                                                MP_dataGridView.RowsDefaultCellStyle.ForeColor =
                                                    MP_dataGridView.AlternatingRowsDefaultCellStyle.ForeColor =
                                                        light_grey2;

                                    MP_dataGridView.GridColor = light_grey2;

                                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor =
                                        MP_dataGridView.DefaultCellStyle.SelectionBackColor
                                            = MP_dataGridView.RowsDefaultCellStyle.SelectionBackColor =
                                                MP_dataGridView.AlternatingRowsDefaultCellStyle.SelectionBackColor =
                                                    dark_grey3;
                                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionForeColor =
                                        MP_dataGridView.DefaultCellStyle.SelectionForeColor
                                            = MP_dataGridView.RowsDefaultCellStyle.SelectionForeColor =
                                                MP_dataGridView.AlternatingRowsDefaultCellStyle.SelectionForeColor =
                                                    light_grey2;

                                    for (var i = 0; i < MP_dataGridView.Columns.Count; i++)
                                    {
                                        MP_dataGridView.Columns[i].HeaderCell.Style.BackColor =
                                            dark_grey2;
                                        MP_dataGridView.Columns[i].HeaderCell.Style.ForeColor =
                                            light_grey2;
                                    }
                                }
                        }
                }
        }
        public void Tasks_ToLight(Form form)
        {
            form.BackColor = light_grey1;
            form.ForeColor = light_grey4;
            foreach (Control c in form.Controls)
                if (c.GetType() == typeof(TableLayoutPanel))
                {
                    c.BackColor = light_grey1;
                    c.ForeColor = light_grey4;

                    foreach (Control aa in c.Controls)
                        if (aa.GetType() == typeof(Label) || aa.GetType() == typeof(Button) ||
                            aa.GetType() == typeof(CheckBox) || aa.GetType() == typeof(RadioButton))
                        {
                            aa.ForeColor = light_grey4;
                            aa.BackColor = light_transparent;

                            if (aa.Name == "Refresh_button")
                                aa.BackgroundImage = Resources.Refresh2_D;
                            else if (aa.Name == "ExportToExcel_button")
                                aa.BackgroundImage = Resources.Excel_D;
                        }
                        else if (aa.GetType() == typeof(TextBox) || aa.GetType() == typeof(ComboBox))
                        {
                            aa.BackColor = light_white;
                            aa.ForeColor = light_grey4;
                        }
                }
                else if (c.GetType() == typeof(Panel))
                {
                    // grid panel & filters panel
                    c.BackColor = light_grey1;
                    c.ForeColor = light_grey4;

                    foreach (Control bb in c.Controls)
                        if (bb.GetType() == typeof(TableLayoutPanel)) /// for combo
                        {
                            bb.BackColor = light_grey1;
                            bb.ForeColor = light_grey4;

                            foreach (Control aa in bb.Controls)
                                if (aa.GetType() == typeof(TextBox) || aa.GetType() == typeof(ComboBox))
                                {
                                    aa.BackColor = light_white;
                                    aa.ForeColor = light_grey4;
                                }
                                else if (aa.GetType() == typeof(DataGridView))
                                {
                                    var MP_dataGridView = aa as DataGridView;
                                    MP_dataGridView.BackgroundColor =
                                        MP_dataGridView.ColumnHeadersDefaultCellStyle.BackColor =
                                            MP_dataGridView.DefaultCellStyle.BackColor =
                                                MP_dataGridView.RowsDefaultCellStyle.BackColor = light_white;
                                    MP_dataGridView.AlternatingRowsDefaultCellStyle.BackColor =
                                        light_grey11;

                                    MP_dataGridView.ForeColor =
                                        MP_dataGridView.ColumnHeadersDefaultCellStyle.ForeColor =
                                            MP_dataGridView.DefaultCellStyle.ForeColor =
                                                MP_dataGridView.RowsDefaultCellStyle.ForeColor =
                                                    MP_dataGridView.AlternatingRowsDefaultCellStyle.ForeColor =
                                                        light_grey4;

                                    MP_dataGridView.GridColor = light_grey3;

                                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor =
                                        MP_dataGridView.DefaultCellStyle.SelectionBackColor
                                            = MP_dataGridView.RowsDefaultCellStyle.SelectionBackColor =
                                                MP_dataGridView.AlternatingRowsDefaultCellStyle.SelectionBackColor =
                                                    light_grey2;
                                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionForeColor =
                                        MP_dataGridView.DefaultCellStyle.SelectionForeColor
                                            = MP_dataGridView.RowsDefaultCellStyle.SelectionForeColor =
                                                MP_dataGridView.AlternatingRowsDefaultCellStyle.SelectionForeColor =
                                                    light_grey4;

                                    for (var i = 0; i < MP_dataGridView.Columns.Count; i++)
                                    {
                                        MP_dataGridView.Columns[i].HeaderCell.Style.BackColor = light_white;
                                        MP_dataGridView.Columns[i].HeaderCell.Style.ForeColor = light_grey4;
                                    }
                                }
                        }
                }
        }

        public void Search_ToNight(Form form, bool Show)
        {
            form.BackColor = light_grey3;
            form.ForeColor = light_grey2;
            foreach (Control c in form.Controls)
                if (c.GetType() == typeof(TableLayoutPanel)) /// main table
                {
                    c.BackColor = light_grey3;
                    c.ForeColor = light_grey2;

                    foreach (Control aa in c.Controls)
                        if (aa.GetType() == typeof(Label) || aa.GetType() == typeof(Button) ||
                            aa.GetType() == typeof(CheckBox) || aa.GetType() == typeof(RadioButton))
                        {
                            aa.ForeColor = light_white;
                            //   aa.BackColor = light_transparent;

                            if (aa.Name == "Delete_button")
                                aa.BackgroundImage = Resources.Trash_L;
                            else if (aa.Name == "ExportToExcel_button")
                                aa.BackgroundImage = Resources.Excel_L;
                        }
                        else if (aa.GetType() == typeof(TextBox) || aa.GetType() == typeof(ComboBox))
                        {
                            aa.BackColor = dark_grey2;
                            aa.ForeColor = light_grey2;
                        }
                }
                else if (c.GetType() == typeof(Panel))
                {
                    c.BackColor = light_grey3;
                    c.ForeColor = light_grey2;

                    foreach (Control aa in c.Controls)
                        if (aa.GetType() == typeof(Label) || aa.GetType() == typeof(Button) ||
                            aa.GetType() == typeof(CheckBox) || aa.GetType() == typeof(RadioButton))
                        {
                            aa.ForeColor = light_white;
                            if (aa.Name == "ShowHide_button")
                            {
                                if (Show)
                                    aa.BackgroundImage = Resources.Show2_L;
                                else aa.BackgroundImage = Resources.Hide2_L;
                            }
                        }
                        else if (aa.GetType() == typeof(TextBox) || aa.GetType() == typeof(ComboBox))
                        {
                            aa.BackColor = dark_grey2;
                            aa.ForeColor = light_grey2;
                        }
                        else if (aa.GetType() == typeof(TableLayoutPanel)) /// filter table
                        {
                            aa.BackColor = light_grey3;
                            aa.ForeColor = light_grey2;

                            foreach (Control zz in aa.Controls)
                                if (zz.GetType() == typeof(Label) || zz.GetType() == typeof(Button) ||
                                    zz.GetType() == typeof(CheckBox) || zz.GetType() == typeof(RadioButton))
                                {
                                    zz.ForeColor = light_white;
                                }
                                else if (zz.GetType() == typeof(TextBox) || zz.GetType() == typeof(ComboBox))
                                {
                                    zz.BackColor = dark_grey2;
                                    zz.ForeColor = light_grey2;
                                }
                                else if (zz.GetType() == typeof(PresentationControls.CheckBoxComboBox)
                                            || zz.GetType() == typeof(PresentationControls.CheckBoxComboBoxItem)
                                            || zz.GetType() == typeof(PresentationControls.CheckBoxComboBoxItemList)
                                            || zz.GetType() == typeof(PresentationControls.CheckBoxComboBoxListControl)
                                            || zz.GetType() == typeof(PresentationControls.CheckBoxComboBoxListControlContainer))
                                {
                                    zz.BackColor = dark_grey2;
                                    zz.ForeColor = light_grey2;
                                    zz.Font = new System.Drawing.Font("Rubik", 10F);
                                }
                        }
                }
                else if (c.GetType() == typeof(DataGridView))
                {
                    var MP_dataGridView = c as DataGridView;
                    MP_dataGridView.BackgroundColor = MP_dataGridView.ColumnHeadersDefaultCellStyle.BackColor =
                        MP_dataGridView.DefaultCellStyle.BackColor =
                            MP_dataGridView.RowsDefaultCellStyle.BackColor = dark_grey2;
                    MP_dataGridView.AlternatingRowsDefaultCellStyle.BackColor = light_grey3;

                    MP_dataGridView.ForeColor = MP_dataGridView.ColumnHeadersDefaultCellStyle.ForeColor =
                        MP_dataGridView.DefaultCellStyle.ForeColor =
                            MP_dataGridView.RowsDefaultCellStyle.ForeColor =
                                MP_dataGridView.AlternatingRowsDefaultCellStyle.ForeColor =
                                    light_grey2;

                    MP_dataGridView.GridColor = light_grey2;

                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor =
                        MP_dataGridView.DefaultCellStyle.SelectionBackColor
                            = MP_dataGridView.RowsDefaultCellStyle.SelectionBackColor =
                                MP_dataGridView.AlternatingRowsDefaultCellStyle.SelectionBackColor =
                                    dark_grey3;
                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionForeColor =
                        MP_dataGridView.DefaultCellStyle.SelectionForeColor
                            = MP_dataGridView.RowsDefaultCellStyle.SelectionForeColor =
                                MP_dataGridView.AlternatingRowsDefaultCellStyle.SelectionForeColor =
                                    light_grey2;

                    for (var i = 0; i < MP_dataGridView.Columns.Count; i++)
                    {
                        MP_dataGridView.Columns[i].HeaderCell.Style.BackColor = dark_grey2;
                        MP_dataGridView.Columns[i].HeaderCell.Style.ForeColor = light_grey2;
                    }
                }
                else if (c.GetType() == typeof(AdvancedDataGridView))
                {
                    var MP_dataGridView = c as AdvancedDataGridView;
                    MP_dataGridView.BackgroundColor = MP_dataGridView.ColumnHeadersDefaultCellStyle.BackColor =
                        MP_dataGridView.DefaultCellStyle.BackColor =
                            MP_dataGridView.RowsDefaultCellStyle.BackColor = dark_grey2;
                    MP_dataGridView.AlternatingRowsDefaultCellStyle.BackColor = light_grey3;

                    MP_dataGridView.ForeColor = MP_dataGridView.ColumnHeadersDefaultCellStyle.ForeColor =
                        MP_dataGridView.DefaultCellStyle.ForeColor =
                            MP_dataGridView.RowsDefaultCellStyle.ForeColor =
                                MP_dataGridView.AlternatingRowsDefaultCellStyle.ForeColor =
                                    light_grey2;

                    MP_dataGridView.GridColor = light_grey2;

                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor =
                        MP_dataGridView.DefaultCellStyle.SelectionBackColor
                            = MP_dataGridView.RowsDefaultCellStyle.SelectionBackColor =
                                MP_dataGridView.AlternatingRowsDefaultCellStyle.SelectionBackColor =
                                    dark_grey3;
                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionForeColor =
                        MP_dataGridView.DefaultCellStyle.SelectionForeColor
                            = MP_dataGridView.RowsDefaultCellStyle.SelectionForeColor =
                                MP_dataGridView.AlternatingRowsDefaultCellStyle.SelectionForeColor =
                                    light_grey2;

                    for (var i = 0; i < MP_dataGridView.Columns.Count; i++)
                    {
                        MP_dataGridView.Columns[i].HeaderCell.Style.BackColor = dark_grey2;
                        MP_dataGridView.Columns[i].HeaderCell.Style.ForeColor = light_grey2;
                    }
                }
        }
        public void Search_ToLight(Form form, bool Show)
        {
            form.BackColor = light_grey1;
            form.ForeColor = light_grey4;
            foreach (Control c in form.Controls)
                if (c.GetType() == typeof(TableLayoutPanel)) /// main table
                {
                    c.BackColor = light_grey1;
                    c.ForeColor = light_grey4;

                    foreach (Control aa in c.Controls)
                        if (aa.GetType() == typeof(Label) || aa.GetType() == typeof(Button) ||
                            aa.GetType() == typeof(CheckBox) || aa.GetType() == typeof(RadioButton))
                        {
                            aa.ForeColor = light_grey4;
                            //   aa.BackColor = light_transparent;

                            if (aa.Name == "Delete_button")
                                aa.BackgroundImage = Resources.Trash_D;
                            else if (aa.Name == "ExportToExcel_button")
                                aa.BackgroundImage = Resources.Excel_D;
                        }
                        else if (aa.GetType() == typeof(TextBox) || aa.GetType() == typeof(ComboBox))
                        {
                            aa.BackColor = light_white;
                            aa.ForeColor = light_grey4;
                        }
                }
                else if (c.GetType() == typeof(Panel))
                {
                    c.BackColor = light_grey1;
                    c.ForeColor = light_grey4;

                    foreach (Control aa in c.Controls)
                        if (aa.GetType() == typeof(Label) || aa.GetType() == typeof(Button) ||
                            aa.GetType() == typeof(CheckBox) || aa.GetType() == typeof(RadioButton))
                        {
                            aa.ForeColor = light_grey4;
                            if (aa.Name == "ShowHide_button")
                            {
                                if (Show)
                                    aa.BackgroundImage = Resources.Show2_D;
                                else aa.BackgroundImage = Resources.Hide2_D;
                            }
                        }
                        else if (aa.GetType() == typeof(TextBox) || aa.GetType() == typeof(ComboBox))
                        {
                            aa.BackColor = light_white;
                            aa.ForeColor = light_grey4;
                        }
                        else if (aa.GetType() == typeof(TableLayoutPanel)) /// filter table
                        {
                            aa.BackColor = light_grey1;
                            aa.ForeColor = light_grey4;

                            foreach (Control zz in aa.Controls)
                                if (zz.GetType() == typeof(Label) || zz.GetType() == typeof(Button) ||
                                    zz.GetType() == typeof(CheckBox) || zz.GetType() == typeof(RadioButton))
                                {
                                    zz.ForeColor = light_grey4;
                                }
                                else if (zz.GetType() == typeof(TextBox) || zz.GetType() == typeof(ComboBox))
                                {
                                    zz.BackColor = light_white;
                                    zz.ForeColor = light_grey4;
                                }
                                else if (zz.GetType() == typeof(PresentationControls.CheckBoxComboBox)
                                            || zz.GetType() == typeof(PresentationControls.CheckBoxComboBoxItem)
                                            || zz.GetType() == typeof(PresentationControls.CheckBoxComboBoxItemList)
                                            || zz.GetType() == typeof(PresentationControls.CheckBoxComboBoxListControl)
                                            || zz.GetType() == typeof(PresentationControls.CheckBoxComboBoxListControlContainer))
                                {
                                    zz.BackColor = light_white;
                                    zz.ForeColor = light_grey4;
                                    zz.Font = new System.Drawing.Font("Rubik", 10F);
                                }
                        }
                }
                else if (c.GetType() == typeof(DataGridView))
                {
                    var MP_dataGridView = c as DataGridView;
                    MP_dataGridView.BackgroundColor = MP_dataGridView.ColumnHeadersDefaultCellStyle.BackColor =
                        MP_dataGridView.DefaultCellStyle.BackColor =
                            MP_dataGridView.RowsDefaultCellStyle.BackColor = light_white;
                    MP_dataGridView.AlternatingRowsDefaultCellStyle.BackColor = light_grey1;

                    MP_dataGridView.ForeColor = MP_dataGridView.ColumnHeadersDefaultCellStyle.ForeColor =
                        MP_dataGridView.DefaultCellStyle.ForeColor =
                            MP_dataGridView.RowsDefaultCellStyle.ForeColor =
                                MP_dataGridView.AlternatingRowsDefaultCellStyle.ForeColor = light_grey4;

                    MP_dataGridView.GridColor = light_grey3;

                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor =
                        MP_dataGridView.DefaultCellStyle.SelectionBackColor
                            = MP_dataGridView.RowsDefaultCellStyle.SelectionBackColor =
                                MP_dataGridView.AlternatingRowsDefaultCellStyle.SelectionBackColor =
                                    light_grey2;
                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionForeColor =
                        MP_dataGridView.DefaultCellStyle.SelectionForeColor
                            = MP_dataGridView.RowsDefaultCellStyle.SelectionForeColor =
                                MP_dataGridView.AlternatingRowsDefaultCellStyle.SelectionForeColor = light_grey4;

                    for (var i = 0; i < MP_dataGridView.Columns.Count; i++)
                    {
                        MP_dataGridView.Columns[i].HeaderCell.Style.BackColor = light_white;
                        MP_dataGridView.Columns[i].HeaderCell.Style.ForeColor = light_grey4;
                    }
                }
                else if (c.GetType() == typeof(AdvancedDataGridView))
                {
                    var MP_dataGridView = c as AdvancedDataGridView;
                    MP_dataGridView.BackgroundColor = MP_dataGridView.ColumnHeadersDefaultCellStyle.BackColor =
                        MP_dataGridView.DefaultCellStyle.BackColor =
                            MP_dataGridView.RowsDefaultCellStyle.BackColor = light_white;
                    MP_dataGridView.AlternatingRowsDefaultCellStyle.BackColor = light_grey1;

                    MP_dataGridView.ForeColor = MP_dataGridView.ColumnHeadersDefaultCellStyle.ForeColor =
                        MP_dataGridView.DefaultCellStyle.ForeColor =
                            MP_dataGridView.RowsDefaultCellStyle.ForeColor =
                                MP_dataGridView.AlternatingRowsDefaultCellStyle.ForeColor = light_grey4;

                    MP_dataGridView.GridColor = light_grey3;

                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor =
                        MP_dataGridView.DefaultCellStyle.SelectionBackColor
                            = MP_dataGridView.RowsDefaultCellStyle.SelectionBackColor =
                                MP_dataGridView.AlternatingRowsDefaultCellStyle.SelectionBackColor =
                                    light_grey2;
                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionForeColor =
                        MP_dataGridView.DefaultCellStyle.SelectionForeColor
                            = MP_dataGridView.RowsDefaultCellStyle.SelectionForeColor =
                                MP_dataGridView.AlternatingRowsDefaultCellStyle.SelectionForeColor = light_grey4;

                    for (var i = 0; i < MP_dataGridView.Columns.Count; i++)
                    {
                        MP_dataGridView.Columns[i].HeaderCell.Style.BackColor = light_white;
                        MP_dataGridView.Columns[i].HeaderCell.Style.ForeColor = light_grey4;
                    }
                }
        }

        public void Statistics_ToNight(Form form, bool Show)
        {
            form.BackColor = light_grey3;
            form.ForeColor = light_grey2;
            foreach (Control c in form.Controls)
                if (c.GetType() == typeof(TableLayoutPanel)) /// main table
                {
                    c.BackColor = light_grey3;
                    c.ForeColor = light_grey2;

                    foreach (Control aa in c.Controls)
                        if (aa.GetType() == typeof(Label) || aa.GetType() == typeof(Button) ||
                            aa.GetType() == typeof(CheckBox) || aa.GetType() == typeof(RadioButton))
                        {
                            aa.ForeColor = light_white;

                            if (aa.Name == "ExportToExcel_button")
                                aa.BackgroundImage = Resources.Excel_L;
                        }
                        else if (aa.GetType() == typeof(TextBox) || aa.GetType() == typeof(ComboBox))
                        {
                            aa.BackColor = dark_grey2;
                            aa.ForeColor = light_grey2;
                        }
                }
                else if (c.GetType() == typeof(Panel))
                {
                    c.BackColor = light_grey3;
                    c.ForeColor = light_grey2;

                    foreach (Control aa in c.Controls)
                        if (aa.GetType() == typeof(Label) || aa.GetType() == typeof(Button) ||
                            aa.GetType() == typeof(CheckBox) || aa.GetType() == typeof(RadioButton))
                        {
                            aa.ForeColor = light_white;

                            if (aa.Name == "AdvancedSearch_button")
                                if (Show)
                                    aa.BackgroundImage = Resources.Show2_L;
                                else aa.BackgroundImage = Resources.Hide2_L;
                        }
                        else if (aa.GetType() == typeof(TextBox) || aa.GetType() == typeof(ComboBox))
                        {
                            aa.BackColor = dark_grey2;
                            aa.ForeColor = light_grey2;
                        }
                        else if (aa.GetType() == typeof(TableLayoutPanel)) /// filter table
                        {
                            aa.BackColor = light_grey3;
                            aa.ForeColor = light_grey2;

                            foreach (Control bb in aa.Controls)
                                if (bb.GetType() == typeof(Label) || bb.GetType() == typeof(Button) ||
                                    bb.GetType() == typeof(CheckBox) || bb.GetType() == typeof(RadioButton))
                                {
                                    bb.ForeColor = light_white;
                                }
                                else if (bb.GetType() == typeof(TextBox) || bb.GetType() == typeof(ComboBox))
                                {
                                    bb.BackColor = dark_grey2;
                                    bb.ForeColor = light_grey2;
                                }
                                else if (bb.GetType() == typeof(PresentationControls.CheckBoxComboBox)
                                    || bb.GetType() == typeof(PresentationControls.CheckBoxComboBoxItem)
                                    || bb.GetType() == typeof(PresentationControls.CheckBoxComboBoxItemList)
                                    || bb.GetType() == typeof(PresentationControls.CheckBoxComboBoxListControl)
                                    || bb.GetType() == typeof(PresentationControls.CheckBoxComboBoxListControlContainer))
                                            {
                                                bb.BackColor = dark_grey2;
                                                bb.ForeColor = light_grey2;
                                                bb.Font = new System.Drawing.Font("Rubik", 10F);
                                }
                        }
                }
                else if (c.GetType() == typeof(Chart))
                {
                    var chart = c as Chart;
                    chart.BackColor = dark_grey2;
                    chart.ForeColor = light_white;

                    for (int s = 0; s < chart.Series.Count; s++)
                    {
                        if (s % 2 == 0) chart.Series[s].Color =light_grey3; 
                        else chart.Series[s].Color = light_grey2;
                        chart.Series[s].LabelForeColor = light_grey2;
                        chart.Series[s].ShadowColor = light_grey2;
                        chart.Series[s].ShadowOffset = 2;
                        chart.Series[s].BorderColor = dark_grey0;
                    }

                    foreach (var ca in chart.ChartAreas)
                    {
                        ca.BackColor = dark_grey2;
                        ca.BackHatchStyle = ChartHatchStyle.None;
                        ca.BorderColor = light_grey2;

                        ca.AxisX.InterlacedColor = light_grey3;
                        ca.AxisX.LineColor = light_grey2;
                        ca.AxisX.TitleForeColor = light_grey2;
                        ca.AxisX.LabelStyle.ForeColor = light_grey2;
                        ca.AxisX.MajorGrid.LineColor = light_grey3;
                        ca.AxisX.MinorGrid.LineColor = light_grey3;

                        ca.AxisY.InterlacedColor = light_grey3;
                        ca.AxisY.LineColor = light_grey2;
                        ca.AxisY.TitleForeColor = light_grey2;
                        ca.AxisY.LabelStyle.ForeColor = light_grey2;
                        ca.AxisY.MajorGrid.LineColor = light_grey3;
                        ca.AxisY.MinorGrid.LineColor = light_grey3;
                    }

                    foreach (var l in chart.Legends)
                    {
                        l.BackColor = dark_grey2;
                        l.ForeColor = light_white;
                    }
                }
        }
        public void Statistics_ToLight(Form form, bool Show)
        {
            form.BackColor = light_grey1;
            form.ForeColor = light_grey4;
            foreach (Control c in form.Controls)
                if (c.GetType() == typeof(TableLayoutPanel)) /// main table
                {
                    c.BackColor = light_grey1;
                    c.ForeColor = light_grey4;

                    foreach (Control aa in c.Controls)
                        if (aa.GetType() == typeof(Label) || aa.GetType() == typeof(Button) ||
                            aa.GetType() == typeof(CheckBox) || aa.GetType() == typeof(RadioButton))
                        {
                            aa.ForeColor = light_grey4;
                            if (aa.Name == "ExportToExcel_button")
                                aa.BackgroundImage = Resources.Excel_D;
                        }
                        else if (aa.GetType() == typeof(TextBox) || aa.GetType() == typeof(ComboBox))
                        {
                            aa.BackColor = light_white;
                            aa.ForeColor = light_grey4;
                        }
                }
                else if (c.GetType() == typeof(Panel))
                {
                    c.BackColor = light_grey1;
                    c.ForeColor = light_grey4;

                    foreach (Control aa in c.Controls)
                        if (aa.GetType() == typeof(Label) || aa.GetType() == typeof(Button) ||
                            aa.GetType() == typeof(CheckBox) || aa.GetType() == typeof(RadioButton))
                        {
                            aa.ForeColor = light_grey4;
                            if (aa.Name == "AdvancedSearch_button")
                                if (Show)
                                    aa.BackgroundImage = Resources.Show2_D;
                                else aa.BackgroundImage = Resources.Hide2_D;
                        }
                        else if (aa.GetType() == typeof(TextBox) || aa.GetType() == typeof(ComboBox))
                        {
                            aa.BackColor = light_white;
                            aa.ForeColor = light_grey4;
                        }
                        else if (aa.GetType() == typeof(TableLayoutPanel)) /// filter table
                        {
                            aa.BackColor = light_grey1;
                            aa.ForeColor = light_grey4;

                            foreach (Control bb in aa.Controls)
                                if (bb.GetType() == typeof(Label) || bb.GetType() == typeof(Button) ||
                                    bb.GetType() == typeof(CheckBox) || bb.GetType() == typeof(RadioButton))
                                {
                                    bb.ForeColor = light_grey4;
                                }
                                else if (bb.GetType() == typeof(TextBox) || bb.GetType() == typeof(ComboBox))
                                {
                                    bb.BackColor = light_white;
                                    bb.ForeColor = light_grey4;
                                }
                                else if (bb.GetType() == typeof(PresentationControls.CheckBoxComboBox)
                                        || bb.GetType() == typeof(PresentationControls.CheckBoxComboBoxItem)
                                        || bb.GetType() == typeof(PresentationControls.CheckBoxComboBoxItemList)
                                        || bb.GetType() == typeof(PresentationControls.CheckBoxComboBoxListControl)
                                        || bb.GetType() == typeof(PresentationControls.CheckBoxComboBoxListControlContainer))
                                {
                                    bb.BackColor = light_white;
                                    bb.ForeColor = light_grey4;
                                    bb.Font = new System.Drawing.Font("Rubik", 10F);
                                }
                        }
                }
                else if (c.GetType() == typeof(Chart))
                {
                    var chart = c as Chart;
                    chart.BackColor = light_white;
                    chart.ForeColor = light_grey4;

                    for(int s = 0;s< chart.Series.Count; s++)
                    {
                        if (s%2==0) chart.Series[s].Color = Color.FromArgb(129, 20, 20); //زوجي
                        else chart.Series[s].Color = light_grey3; //فردي

                        chart.Series[s].LabelForeColor = light_grey4;
                        chart.Series[s].ShadowColor = light_grey4;
                        chart.Series[s].ShadowOffset = 2;
                        chart.Series[s].BorderColor = dark_grey2;

                    }

                    foreach (var ca in chart.ChartAreas)
                    {
                        ca.BackColor = Color.FromArgb(224, 224, 224);
                        ca.BackHatchStyle = ChartHatchStyle.None;
                        ca.BorderColor = dark_grey2;

                        ca.AxisX.InterlacedColor = light_grey4;
                        ca.AxisX.LineColor = light_grey4;
                        ca.AxisX.TitleForeColor = light_grey4;
                        ca.AxisX.LabelStyle.ForeColor = light_grey4;
                        ca.AxisX.MajorGrid.LineColor = light_grey2;
                        ca.AxisX.MinorGrid.LineColor = light_grey3;

                        ca.AxisY.InterlacedColor = light_grey4;
                        ca.AxisY.LineColor = light_grey4;
                        ca.AxisY.TitleForeColor = light_grey4;
                        ca.AxisY.LabelStyle.ForeColor = light_grey4;
                        ca.AxisY.MajorGrid.LineColor = light_grey2;
                        ca.AxisY.MinorGrid.LineColor = light_grey3;
                    }

                    foreach (var l in chart.Legends)
                    {
                        l.BackColor = light_white;
                        l.ForeColor = light_grey4;
                    }
                }
        }

        public void Timeline_ToNight(Form form)
        {
            form.BackColor = light_grey3;
            form.ForeColor = light_grey2;
            foreach (Control c in form.Controls)
                if (c.GetType() == typeof(TableLayoutPanel)) /// main table
                {
                    c.BackColor = light_grey3;
                    c.ForeColor = light_grey2;

                    foreach (Control aa in c.Controls)
                        if (aa.GetType() == typeof(Label) || aa.GetType() == typeof(Button) ||
                            aa.GetType() == typeof(CheckBox) || aa.GetType() == typeof(RadioButton))
                        {
                            aa.ForeColor = light_white;
                        }
                        else if (aa.GetType() == typeof(TextBox) || aa.GetType() == typeof(ComboBox))
                        {
                            aa.BackColor = dark_grey2;
                            aa.ForeColor = light_grey2;
                        }
                        else if (aa.GetType() == typeof(DataGridView))
                        {
                            var MP_dataGridView = aa as DataGridView;
                            MP_dataGridView.BackgroundColor = MP_dataGridView.ColumnHeadersDefaultCellStyle.BackColor =
                                MP_dataGridView.DefaultCellStyle.BackColor =
                                    MP_dataGridView.RowsDefaultCellStyle.BackColor = dark_grey2;
                            MP_dataGridView.AlternatingRowsDefaultCellStyle.BackColor = light_grey3;

                            MP_dataGridView.ForeColor = MP_dataGridView.ColumnHeadersDefaultCellStyle.ForeColor =
                                MP_dataGridView.DefaultCellStyle.ForeColor =
                                    MP_dataGridView.RowsDefaultCellStyle.ForeColor =
                                        MP_dataGridView.AlternatingRowsDefaultCellStyle.ForeColor =
                                            light_grey2;

                            MP_dataGridView.GridColor = light_grey2;

                            MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor =
                                MP_dataGridView.DefaultCellStyle.SelectionBackColor
                                    = MP_dataGridView.RowsDefaultCellStyle.SelectionBackColor =
                                        MP_dataGridView.AlternatingRowsDefaultCellStyle.SelectionBackColor =
                                            dark_grey3;
                            MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionForeColor =
                                MP_dataGridView.DefaultCellStyle.SelectionForeColor
                                    = MP_dataGridView.RowsDefaultCellStyle.SelectionForeColor =
                                        MP_dataGridView.AlternatingRowsDefaultCellStyle.SelectionForeColor =
                                            light_grey2;

                            for (var i = 0; i < MP_dataGridView.Columns.Count; i++)
                            {
                                MP_dataGridView.Columns[i].HeaderCell.Style.BackColor = dark_grey2;
                                MP_dataGridView.Columns[i].HeaderCell.Style.ForeColor = light_grey2;
                            }
                        }
                }
                else if (c.GetType() == typeof(Chart))
                {
                    var chart = c as Chart;
                    chart.BackColor = dark_grey2;
                    chart.ForeColor = light_white;

                    foreach (var s in chart.Series)
                    {
                        //if (s.Name != "6")
                        s.Color = light_grey2;
                        //else s.Color = light_grey3;
                        s.LabelForeColor = light_grey2;
                    }

                    chart.Legends[0].BackColor = dark_grey2;
                    chart.Legends[0].ForeColor = light_white;
                }
        }
        public void Timeline_ToLight(Form form)
        {
            form.BackColor = light_grey1;
            form.ForeColor = light_grey4;
            foreach (Control c in form.Controls)
                if (c.GetType() == typeof(TableLayoutPanel)) /// main table
                {
                    c.BackColor = light_grey1;
                    c.ForeColor = light_grey4;

                    foreach (Control aa in c.Controls)
                        if (aa.GetType() == typeof(Label) || aa.GetType() == typeof(Button) ||
                            aa.GetType() == typeof(CheckBox) || aa.GetType() == typeof(RadioButton))
                        {
                            aa.ForeColor = light_grey4;
                        }
                        else if (aa.GetType() == typeof(TextBox) || aa.GetType() == typeof(ComboBox))
                        {
                            aa.BackColor = light_white;
                            aa.ForeColor = light_grey4;
                        }
                        else if (aa.GetType() == typeof(DataGridView))
                        {
                            var MP_dataGridView = aa as DataGridView;
                            MP_dataGridView.BackgroundColor = MP_dataGridView.ColumnHeadersDefaultCellStyle.BackColor =
                                MP_dataGridView.DefaultCellStyle.BackColor =
                                    MP_dataGridView.RowsDefaultCellStyle.BackColor = light_white;
                            MP_dataGridView.AlternatingRowsDefaultCellStyle.BackColor = light_grey1;

                            MP_dataGridView.ForeColor = MP_dataGridView.ColumnHeadersDefaultCellStyle.ForeColor =
                                MP_dataGridView.DefaultCellStyle.ForeColor =
                                    MP_dataGridView.RowsDefaultCellStyle.ForeColor =
                                        MP_dataGridView.AlternatingRowsDefaultCellStyle.ForeColor = light_grey4;

                            MP_dataGridView.GridColor = light_grey3;

                            MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor =
                                MP_dataGridView.DefaultCellStyle.SelectionBackColor
                                    = MP_dataGridView.RowsDefaultCellStyle.SelectionBackColor =
                                        MP_dataGridView.AlternatingRowsDefaultCellStyle.SelectionBackColor =
                                            light_grey2;
                            MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionForeColor =
                                MP_dataGridView.DefaultCellStyle.SelectionForeColor
                                    = MP_dataGridView.RowsDefaultCellStyle.SelectionForeColor =
                                        MP_dataGridView.AlternatingRowsDefaultCellStyle.SelectionForeColor =
                                            light_grey4;

                            for (var i = 0; i < MP_dataGridView.Columns.Count; i++)
                            {
                                MP_dataGridView.Columns[i].HeaderCell.Style.BackColor = light_white;
                                MP_dataGridView.Columns[i].HeaderCell.Style.ForeColor = light_grey4;
                            }
                        }
                }
                else if (c.GetType() == typeof(Chart))
                {
                    var chart = c as Chart;
                    chart.BackColor = light_white;
                    chart.ForeColor = light_grey4;

                    foreach (var s in chart.Series)
                    {
                        if (s.Name == "6") s.Color = light_grey11;
                        else s.Color = Color.FromArgb(129, 20, 20);
                        s.LabelForeColor = light_grey4;
                    }

                    chart.Legends[0].BackColor = light_white;
                    chart.Legends[0].ForeColor = light_grey4;
                }
        }

        public void AdminPanel_ToNight(Form form, bool Show)
        {
            form.BackColor = light_grey3;
            form.ForeColor = light_grey2;
            foreach (Control c in form.Controls)
                if (c.GetType() == typeof(Panel))
                {
                    c.BackColor = light_grey3;
                    c.ForeColor = light_grey2;

                    foreach (Control aa in c.Controls)
                        if (aa.GetType() == typeof(Label) || aa.GetType() == typeof(Button) ||
                            aa.GetType() == typeof(CheckBox) || aa.GetType() == typeof(RadioButton))
                        {
                            aa.ForeColor = light_white;
                            aa.BackColor = light_transparent;
                        }
                        else if (aa.GetType() == typeof(TextBox) || aa.GetType() == typeof(ComboBox))
                        {
                            aa.BackColor = dark_grey2;
                            aa.ForeColor = light_grey2;
                        }
                        else if (aa.GetType() == typeof(TableLayoutPanel)) /// filter table
                        {
                            aa.BackColor = light_grey3;
                            aa.ForeColor = light_white;

                            foreach (Control bb in aa.Controls)
                                if (bb.GetType() == typeof(Label) || bb.GetType() == typeof(CheckBox) ||
                                    bb.GetType() == typeof(RadioButton))
                                {
                                    bb.ForeColor = light_white;
                                    bb.BackColor = light_transparent;
                                }
                                else if (bb.GetType() == typeof(TextBox) || bb.GetType() == typeof(ComboBox))
                                {
                                    bb.BackColor = dark_grey2;
                                    bb.ForeColor = light_grey2;
                                }
                                else if (bb.GetType() == typeof(PresentationControls.CheckBoxComboBox)
                                        || bb.GetType() == typeof(PresentationControls.CheckBoxComboBoxItem)
                                        || bb.GetType() == typeof(PresentationControls.CheckBoxComboBoxItemList)
                                        || bb.GetType() == typeof(PresentationControls.CheckBoxComboBoxListControl)
                                        || bb.GetType() == typeof(PresentationControls.CheckBoxComboBoxListControlContainer))
                                {
                                    bb.BackColor = dark_grey2;
                                    bb.ForeColor = light_grey2;
                                    bb.Font = new System.Drawing.Font("Rubik", 10F);
                                }
                        }
                        else if (c.GetType() == typeof(AdvancedDataGridView))
                        {
                            var MP_dataGridView = c as AdvancedDataGridView;
                            MP_dataGridView.ColumnHeadersDefaultCellStyle.BackColor =
                                MP_dataGridView.RowHeadersDefaultCellStyle.BackColor = dark_grey2;

                            MP_dataGridView.ColumnHeadersDefaultCellStyle.ForeColor =
                                MP_dataGridView.RowHeadersDefaultCellStyle.ForeColor = light_grey2;

                            MP_dataGridView.GridColor = light_grey2;

                            MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor =
                                MP_dataGridView.DefaultCellStyle.SelectionBackColor
                                    = MP_dataGridView.RowsDefaultCellStyle.SelectionBackColor =
                                        MP_dataGridView.AlternatingRowsDefaultCellStyle.SelectionBackColor =
                                            dark_grey3;
                            MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionForeColor =
                                MP_dataGridView.DefaultCellStyle.SelectionForeColor
                                    = MP_dataGridView.RowsDefaultCellStyle.SelectionForeColor =
                                        MP_dataGridView.AlternatingRowsDefaultCellStyle.SelectionForeColor =
                                            light_grey2;

                            for (var i = 0; i < MP_dataGridView.Columns.Count; i++)
                            {
                                MP_dataGridView.Columns[i].HeaderCell.Style.BackColor = dark_grey2;
                                MP_dataGridView.Columns[i].HeaderCell.Style.ForeColor = light_grey2;
                            }
                        }
                        else if (c.GetType() == typeof(TableLayoutPanel)) /// colors table
                        {
                            c.BackColor = light_grey3;
                            c.ForeColor = light_white;

                            foreach (Control bb in c.Controls)
                            {
                                if (bb.GetType() == typeof(Label) || bb.GetType() == typeof(CheckBox) ||
                                    bb.GetType() == typeof(RadioButton))
                                {
                                    bb.ForeColor = light_white;
                                    bb.BackColor = light_transparent;
                                }

                                if (bb.GetType() == typeof(Button))
                                {
                                    bb.ForeColor = light_white;
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
                                    bb.BackColor = dark_grey2;
                                    bb.ForeColor = light_grey2;
                                }
                            }
                        }
                }
        }
        public void AdminPanel_ToLight(Form form, bool Show)
        {
            form.BackColor = light_grey1;
            form.ForeColor = light_grey4;
            foreach (Control c in form.Controls)
                if (c.GetType() == typeof(Panel))
                {
                    c.BackColor = light_grey1;
                    c.ForeColor = light_grey4;

                    foreach (Control aa in c.Controls)
                        if (aa.GetType() == typeof(Label) || aa.GetType() == typeof(Button) ||
                            aa.GetType() == typeof(CheckBox) || aa.GetType() == typeof(RadioButton))
                        {
                            aa.ForeColor = light_grey4;
                            aa.BackColor = light_transparent;
                        }
                        else if (aa.GetType() == typeof(TextBox) || aa.GetType() == typeof(ComboBox))
                        {
                            aa.BackColor = light_white;
                            aa.ForeColor = light_grey4;
                        }
                        else if (aa.GetType() == typeof(TableLayoutPanel))
                        {
                            aa.BackColor = light_grey1;
                            aa.ForeColor = light_grey4;

                            foreach (Control bb in aa.Controls)
                                if (bb.GetType() == typeof(Label) || bb.GetType() == typeof(CheckBox) ||
                                    bb.GetType() == typeof(RadioButton))
                                {
                                    bb.ForeColor = light_grey4;
                                    bb.BackColor = light_transparent;
                                }
                                else if (bb.GetType() == typeof(TextBox) || bb.GetType() == typeof(ComboBox))
                                {
                                    bb.BackColor = light_white;
                                    bb.ForeColor = light_grey4;
                                }
                                else if (bb.GetType() == typeof(PresentationControls.CheckBoxComboBox)
                                                || bb.GetType() == typeof(PresentationControls.CheckBoxComboBoxItem)
                                                || bb.GetType() == typeof(PresentationControls.CheckBoxComboBoxItemList)
                                                || bb.GetType() == typeof(PresentationControls.CheckBoxComboBoxListControl)
                                                || bb.GetType() == typeof(PresentationControls.CheckBoxComboBoxListControlContainer))
                                {
                                    bb.BackColor = light_white;
                                    bb.ForeColor = light_grey4;
                                    bb.Font = new System.Drawing.Font("Rubik", 10F);
                                }
                        }
                }
                else if (c.GetType() == typeof(AdvancedDataGridView))
                {
                    var MP_dataGridView = c as AdvancedDataGridView;
                    MP_dataGridView.ColumnHeadersDefaultCellStyle.BackColor =
                        MP_dataGridView.RowHeadersDefaultCellStyle.BackColor = light_white;

                    MP_dataGridView.ColumnHeadersDefaultCellStyle.ForeColor =
                        MP_dataGridView.RowHeadersDefaultCellStyle.ForeColor = light_grey4;
                    MP_dataGridView.GridColor = light_grey2;

                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor =
                        MP_dataGridView.DefaultCellStyle.SelectionBackColor
                            = MP_dataGridView.RowsDefaultCellStyle.SelectionBackColor =
                                MP_dataGridView.AlternatingRowsDefaultCellStyle.SelectionBackColor =
                                    light_grey11;
                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionForeColor =
                        MP_dataGridView.DefaultCellStyle.SelectionForeColor
                            = MP_dataGridView.RowsDefaultCellStyle.SelectionForeColor =
                                MP_dataGridView.AlternatingRowsDefaultCellStyle.SelectionForeColor = light_grey4;

                    for (var i = 0; i < MP_dataGridView.Columns.Count; i++)
                    {
                        MP_dataGridView.Columns[i].HeaderCell.Style.BackColor = light_white;
                        MP_dataGridView.Columns[i].HeaderCell.Style.ForeColor = light_grey4;
                    }
                }
                else if (c.GetType() == typeof(TableLayoutPanel))
                {
                    c.BackColor = light_grey1;
                    c.ForeColor = light_grey4;

                    foreach (Control bb in c.Controls)
                        if (bb.GetType() == typeof(Label) || bb.GetType() == typeof(CheckBox) ||
                            bb.GetType() == typeof(RadioButton))
                        {
                            bb.ForeColor = light_grey4;
                            bb.BackColor = light_transparent;
                        }
                        else if (bb.GetType() == typeof(Button))
                        {
                            bb.ForeColor = light_grey4;
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
                            bb.BackColor = light_white;
                            bb.ForeColor = light_grey4;
                        }
                }
        }

        public void NotificationBox_ToNight(Form form)
        {
            form.BackColor = light_grey3;
            form.ForeColor = light_white;
            form.BackgroundImage = Resources.Back_600_D;
            foreach (Control c in form.Controls)
                if (c.GetType() == typeof(Label) || c.GetType() == typeof(Button) || c.GetType() == typeof(CheckBox) ||
                    c.GetType() == typeof(RadioButton))
                {
                    c.ForeColor = light_white;
                }
                else if (c.GetType() == typeof(TextBox) || c.GetType() == typeof(ComboBox))
                {
                    c.BackColor = dark_grey2;
                    c.ForeColor = light_white;
                }
                else if (c.GetType() == typeof(DataGridView))
                {
                    var MP_dataGridView = c as DataGridView;
                    MP_dataGridView.BackgroundColor = MP_dataGridView.ColumnHeadersDefaultCellStyle.BackColor =
                        MP_dataGridView.DefaultCellStyle.BackColor =
                            MP_dataGridView.RowsDefaultCellStyle.BackColor = dark_grey2;

                    MP_dataGridView.ForeColor = MP_dataGridView.ColumnHeadersDefaultCellStyle.ForeColor =
                        MP_dataGridView.DefaultCellStyle.ForeColor =
                            MP_dataGridView.RowsDefaultCellStyle.ForeColor = light_white;

                    MP_dataGridView.GridColor = light_grey2;

                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor =
                        MP_dataGridView.DefaultCellStyle.SelectionBackColor
                            = MP_dataGridView.RowsDefaultCellStyle.SelectionBackColor = light_grey3;
                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionForeColor =
                        MP_dataGridView.DefaultCellStyle.SelectionForeColor
                            = MP_dataGridView.RowsDefaultCellStyle.SelectionForeColor = light_white;


                    for (var i = 0; i < MP_dataGridView.Columns.Count; i++)
                    {
                        MP_dataGridView.Columns[i].HeaderCell.Style.BackColor = dark_grey2;
                        MP_dataGridView.Columns[i].HeaderCell.Style.ForeColor = light_white;
                        for (var j = 0; j < MP_dataGridView.Rows.Count; j++)
                        {
                            MP_dataGridView.Rows[j].Cells[i].Style.BackColor = dark_grey2;
                            MP_dataGridView.Rows[j].Cells[i].Style.ForeColor = light_white;
                        }
                    }
                }
        }
        public void NotificationBox_ToLight(Form form)
        {
            form.BackColor = light_grey1;
            form.ForeColor = light_grey4;
            form.BackgroundImage = Resources.Back_600;
            foreach (Control c in form.Controls)
                if (c.GetType() == typeof(Label) || c.GetType() == typeof(Button) || c.GetType() == typeof(CheckBox) ||
                    c.GetType() == typeof(RadioButton))
                {
                    c.ForeColor = light_grey4;
                    c.BackColor = light_transparent;

                    if (c.Name == "Refresh_button")
                        c.BackgroundImage = Resources.Refresh2_D;
                }
                else if (c.GetType() == typeof(TextBox) || c.GetType() == typeof(ComboBox))
                {
                    c.BackColor = light_white;
                    c.ForeColor = light_grey4;
                }
                else if (c.GetType() == typeof(DataGridView))
                {
                    var MP_dataGridView = c as DataGridView;
                    MP_dataGridView.BackgroundColor = MP_dataGridView.ColumnHeadersDefaultCellStyle.BackColor =
                        MP_dataGridView.DefaultCellStyle.BackColor =
                            MP_dataGridView.RowsDefaultCellStyle.BackColor = light_white;
                    MP_dataGridView.AlternatingRowsDefaultCellStyle.BackColor = light_grey11;

                    MP_dataGridView.ForeColor = MP_dataGridView.ColumnHeadersDefaultCellStyle.ForeColor =
                        MP_dataGridView.DefaultCellStyle.ForeColor =
                            MP_dataGridView.RowsDefaultCellStyle.ForeColor =
                                MP_dataGridView.AlternatingRowsDefaultCellStyle.ForeColor = light_grey4;

                    MP_dataGridView.GridColor = light_grey3;

                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor =
                        MP_dataGridView.DefaultCellStyle.SelectionBackColor
                            = MP_dataGridView.RowsDefaultCellStyle.SelectionBackColor =
                                MP_dataGridView.AlternatingRowsDefaultCellStyle.SelectionBackColor =
                                    light_grey2;
                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionForeColor =
                        MP_dataGridView.DefaultCellStyle.SelectionForeColor
                            = MP_dataGridView.RowsDefaultCellStyle.SelectionForeColor =
                                MP_dataGridView.AlternatingRowsDefaultCellStyle.SelectionForeColor = light_grey4;

                    for (var i = 0; i < MP_dataGridView.Columns.Count; i++)
                    {
                        MP_dataGridView.Columns[i].HeaderCell.Style.BackColor = light_white;
                        MP_dataGridView.Columns[i].HeaderCell.Style.ForeColor = light_grey4;
                    }
                }
        }

        public void MessageBox_ToNight(Form form)
        {
            form.BackColor = light_grey3;
            form.ForeColor = light_white;
            form.BackgroundImage = Resources.Back_700_D;

            foreach (Control c in form.Controls)
                if (c.GetType() == typeof(Label) || c.GetType() == typeof(Button) || c.GetType() == typeof(CheckBox) ||
                    c.GetType() == typeof(RadioButton))
                {
                    c.ForeColor = light_white;
                    if (c.Name == "Close_button")
                        c.BackgroundImage = Resources.Exit_L;
                    else if (c.Name.Contains("_button"))
                        c.BackgroundImage = Properties.Resources.BtnBackground_L;
                }
                else if (c.GetType() == typeof(TextBox) || c.GetType() == typeof(ComboBox))
                {
                    c.BackColor = dark_grey2;
                    c.ForeColor = light_white;
                }
                else if (c.GetType() == typeof(TableLayoutPanel)) /// colors table
                {
                    c.BackColor = light_grey3;
                    c.ForeColor = light_white;

                    foreach (Control bb in c.Controls)
                        if (bb.GetType() == typeof(Label))
                        {
                            bb.ForeColor = light_white;
                        }
                        else if (bb.GetType() == typeof(TextBox) || bb.GetType() == typeof(RichTextBox) ||
                                 bb.GetType() == typeof(ComboBox))
                        {
                            bb.BackColor = dark_grey2;
                            bb.ForeColor = light_white;
                        }
                }
        }
        public void MessageBox_ToLight(Form form)
        {
            form.BackColor = light_grey1;
            form.ForeColor = light_grey4;
            form.BackgroundImage = Resources.Back_700;
            foreach (Control c in form.Controls)
                if (c.GetType() == typeof(Label) || c.GetType() == typeof(Button) || c.GetType() == typeof(CheckBox) ||
                    c.GetType() == typeof(RadioButton))
                {
                    c.ForeColor = light_grey4;
                    c.BackColor = light_transparent;
                    if (c.Name == "Close_button")
                        c.BackgroundImage = Resources.Exit_D;
                    else if (c.Name.Contains("_button"))
                        c.BackgroundImage = Properties.Resources.BtnBackground_D;
                }
                else if (c.GetType() == typeof(TableLayoutPanel))
                {
                    c.BackColor = light_transparent;
                    c.ForeColor = light_grey4;

                    foreach (Control bb in c.Controls)
                        if (bb.GetType() == typeof(Label))
                        {
                            bb.ForeColor = light_grey4;
                            bb.BackColor = light_transparent;
                        }
                        else if (bb.GetType() == typeof(TextBox) || bb.GetType() == typeof(RichTextBox) ||
                                 bb.GetType() == typeof(ComboBox))
                        {
                            bb.BackColor = light_white;
                            bb.ForeColor = light_grey4;
                        }
                }
        }

        public void ShowDetails_Form_ToNight(Form form)
        {
            form.BackColor = light_grey3;
            form.ForeColor = light_white;
            form.BackgroundImage = Resources.Back_700_D;
            foreach (Control c in form.Controls)
                if (c.GetType() == typeof(Label) || c.GetType() == typeof(Button) || c.GetType() == typeof(CheckBox) ||
                    c.GetType() == typeof(RadioButton))
                {
                    c.ForeColor = light_white;
                }
                else if (c.GetType() == typeof(AdvancedDataGridView))
                {
                    var MP_dataGridView = c as AdvancedDataGridView;
                    MP_dataGridView.BackgroundColor =
                        MP_dataGridView.ColumnHeadersDefaultCellStyle.BackColor =
                            MP_dataGridView.DefaultCellStyle.BackColor =
                                MP_dataGridView.RowsDefaultCellStyle.BackColor =
                                    dark_grey2;
                    MP_dataGridView.AlternatingRowsDefaultCellStyle.BackColor =
                        light_grey3;

                    MP_dataGridView.ForeColor =
                        MP_dataGridView.ColumnHeadersDefaultCellStyle.ForeColor =
                            MP_dataGridView.DefaultCellStyle.ForeColor =
                                MP_dataGridView.RowsDefaultCellStyle.ForeColor =
                                    MP_dataGridView.AlternatingRowsDefaultCellStyle.ForeColor =
                                        light_grey2;

                    MP_dataGridView.GridColor = light_grey2;

                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor =
                        MP_dataGridView.DefaultCellStyle.SelectionBackColor
                            = MP_dataGridView.RowsDefaultCellStyle.SelectionBackColor =
                                MP_dataGridView.AlternatingRowsDefaultCellStyle.SelectionBackColor =
                                    dark_grey3;
                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionForeColor =
                        MP_dataGridView.DefaultCellStyle.SelectionForeColor
                            = MP_dataGridView.RowsDefaultCellStyle.SelectionForeColor =
                                MP_dataGridView.AlternatingRowsDefaultCellStyle.SelectionForeColor =
                                    light_grey2;

                    for (var i = 0; i < MP_dataGridView.Columns.Count; i++)
                    {
                        MP_dataGridView.Columns[i].HeaderCell.Style.BackColor =
                            dark_grey2;
                        MP_dataGridView.Columns[i].HeaderCell.Style.ForeColor =
                            light_grey2;
                    }
                }
                else if (c.GetType() == typeof(AdvancedDataGridViewSearchToolBar))
                { 
                    c.BackColor = light_grey3;
                    c.ForeColor = light_white;

                    foreach (Control aa in c.Controls)
                        if (aa.GetType() == typeof(Label) || aa.GetType() == typeof(Button) ||
                            aa.GetType() == typeof(CheckBox) || aa.GetType() == typeof(RadioButton))
                        {
                            aa.ForeColor = light_white;
                            aa.BackColor = light_transparent;
                        }
                        else if (aa.GetType() == typeof(TextBox) || aa.GetType() == typeof(ComboBox))
                        {
                            aa.BackColor = dark_grey2;
                            aa.ForeColor = light_white;
                        }
                }
                else if (c.GetType() == typeof(TableLayoutPanel)) /// main table
            {
                c.BackColor = light_grey3;
                c.ForeColor = light_grey2;

                foreach (Control aa in c.Controls)
                    if (aa.GetType() == typeof(Label) || aa.GetType() == typeof(Button) ||
                        aa.GetType() == typeof(CheckBox) || aa.GetType() == typeof(RadioButton))
                    {
                        aa.ForeColor = light_white;
                        //   aa.BackColor = light_transparent;

                        if (aa.Name == "Delete_button")
                            aa.BackgroundImage = Resources.Trash_L;
                        else if (aa.Name == "ExportToExcel_button")
                            aa.BackgroundImage = Resources.Excel_L;
                    }
                    else if (aa.GetType() == typeof(TextBox) || aa.GetType() == typeof(ComboBox))
                    {
                        aa.BackColor = dark_grey2;
                        aa.ForeColor = light_grey2;
                    }
            }
        }
        public void ShowDetails_Form_ToLight(Form form)
        {
            form.BackColor = light_grey1;
            form.ForeColor = light_grey4;
            form.BackgroundImage = Resources.Back_700;
            foreach (Control c in form.Controls)
                if (c.GetType() == typeof(Label) || c.GetType() == typeof(Button) || c.GetType() == typeof(CheckBox) ||
                    c.GetType() == typeof(RadioButton))
                {
                    c.ForeColor = light_grey4;
                }
                else if (c.GetType() == typeof(AdvancedDataGridView))
                {
                    var MP_dataGridView = c as AdvancedDataGridView;
                    MP_dataGridView.BackgroundColor =
                        MP_dataGridView.ColumnHeadersDefaultCellStyle.BackColor =
                            MP_dataGridView.DefaultCellStyle.BackColor =
                                MP_dataGridView.RowsDefaultCellStyle.BackColor = light_white;
                    MP_dataGridView.AlternatingRowsDefaultCellStyle.BackColor =
                        light_grey1;

                    MP_dataGridView.ForeColor =
                        MP_dataGridView.ColumnHeadersDefaultCellStyle.ForeColor =
                            MP_dataGridView.DefaultCellStyle.ForeColor =
                                MP_dataGridView.RowsDefaultCellStyle.ForeColor =
                                    MP_dataGridView.AlternatingRowsDefaultCellStyle.ForeColor =
                                        light_grey4;

                    MP_dataGridView.GridColor = light_grey3;

                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor =
                        MP_dataGridView.DefaultCellStyle.SelectionBackColor
                            = MP_dataGridView.RowsDefaultCellStyle.SelectionBackColor =
                                MP_dataGridView.AlternatingRowsDefaultCellStyle.SelectionBackColor =
                                    light_grey2;
                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionForeColor =
                        MP_dataGridView.DefaultCellStyle.SelectionForeColor
                            = MP_dataGridView.RowsDefaultCellStyle.SelectionForeColor =
                                MP_dataGridView.AlternatingRowsDefaultCellStyle.SelectionForeColor =
                                    light_grey4;

                    for (var i = 0; i < MP_dataGridView.Columns.Count; i++)
                    {
                        MP_dataGridView.Columns[i].HeaderCell.Style.BackColor = light_white;
                        MP_dataGridView.Columns[i].HeaderCell.Style.ForeColor = light_grey4;
                    }
                }
                else if (c.GetType() == typeof(AdvancedDataGridViewSearchToolBar))
                {
                    //var searchToolBar = c as AdvancedDataGridViewSearchToolBar;
                    //searchToolBar.BackColor = light_white;
                    //searchToolBar.ForeColor = light_grey4;

                    c.BackColor = light_grey1;
                    c.ForeColor = light_grey4;

                    foreach (Control aa in c.Controls)
                        if (aa.GetType() == typeof(Label) || aa.GetType() == typeof(Button) ||
                            aa.GetType() == typeof(CheckBox) || aa.GetType() == typeof(RadioButton))
                        {
                            aa.ForeColor = light_grey4;
                            aa.BackColor = light_transparent;
                        }
                        else if (aa.GetType() == typeof(TextBox) || aa.GetType() == typeof(ComboBox))
                        {
                            aa.BackColor = light_white;
                            aa.ForeColor = light_grey4;
                        }
                }
                else if (c.GetType() == typeof(TableLayoutPanel)) /// main table
            {
                c.BackColor = light_grey1;
                c.ForeColor = light_grey4;

                foreach (Control aa in c.Controls)
                    if (aa.GetType() == typeof(Label) || aa.GetType() == typeof(Button) ||
                        aa.GetType() == typeof(CheckBox) || aa.GetType() == typeof(RadioButton))
                    {
                        aa.ForeColor = light_grey4;
                        //   aa.BackColor = light_transparent;

                        if (aa.Name == "Delete_button")
                            aa.BackgroundImage = Resources.Trash_D;
                        else if (aa.Name == "ExportToExcel_button")
                            aa.BackgroundImage = Resources.Excel_D;
                    }
                    else if (aa.GetType() == typeof(TextBox) || aa.GetType() == typeof(ComboBox))
                    {
                        aa.BackColor = light_white;
                        aa.ForeColor = light_grey4;
                    }
            }
        }

        public void Category_ToNight(Form form)
        {
            form.BackColor = light_grey3;
            form.ForeColor = light_white;
            form.BackgroundImage = Resources.Background_Border_D;
            foreach (Control c in form.Controls)
                if (c.GetType() == typeof(TableLayoutPanel)) /// header table
                {
                    c.BackColor = light_transparent;
                    c.ForeColor = light_white;
                    foreach (Control aa in c.Controls)
                        if (aa.GetType() == typeof(Label) || aa.GetType() == typeof(Button) ||
                            aa.GetType() == typeof(CheckBox) || aa.GetType() == typeof(RadioButton))
                        {
                            aa.ForeColor = light_white;
                            aa.BackColor = light_transparent;
                            if (aa.Name == "Close_button")
                                aa.BackgroundImage = Resources.Exit_L;
                        }
                        else if (aa.GetType() == typeof(TextBox) || aa.GetType() == typeof(ComboBox))
                        {
                            aa.BackColor = dark_grey2;
                            aa.ForeColor = light_white;
                        }
                }
                else if (c.GetType() == typeof(Label) || c.GetType() == typeof(Button) ||
                         c.GetType() == typeof(CheckBox) || c.GetType() == typeof(RadioButton))
                {
                    c.ForeColor = light_white;
                    if (c.Name == "Save_button")
                        c.BackgroundImage = Resources.Save_CL;
                    else if (c.Name == "Delete_button")
                        c.BackgroundImage = Resources.Delete_CL;
                }
                else if (c.GetType() == typeof(TextBox) || c.GetType() == typeof(ComboBox))
                {
                    c.BackColor = dark_grey2;
                    c.ForeColor = light_white;
                }
                else if (c.GetType() == typeof(DataGridView))
                {
                    var MP_dataGridView = c as DataGridView;
                    MP_dataGridView.BackgroundColor = MP_dataGridView.ColumnHeadersDefaultCellStyle.BackColor =
                        MP_dataGridView.DefaultCellStyle.BackColor =
                            MP_dataGridView.RowsDefaultCellStyle.BackColor = dark_grey2;

                    MP_dataGridView.ForeColor = MP_dataGridView.ColumnHeadersDefaultCellStyle.ForeColor =
                        MP_dataGridView.DefaultCellStyle.ForeColor =
                            MP_dataGridView.RowsDefaultCellStyle.ForeColor = light_white;

                    MP_dataGridView.GridColor = light_grey2;

                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor =
                        MP_dataGridView.DefaultCellStyle.SelectionBackColor
                            = MP_dataGridView.RowsDefaultCellStyle.SelectionBackColor = light_grey3;
                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionForeColor =
                        MP_dataGridView.DefaultCellStyle.SelectionForeColor
                            = MP_dataGridView.RowsDefaultCellStyle.SelectionForeColor = light_white;

                    for (var i = 0; i < MP_dataGridView.Columns.Count; i++)
                    {
                        MP_dataGridView.Columns[i].HeaderCell.Style.BackColor = dark_grey2;
                        MP_dataGridView.Columns[i].HeaderCell.Style.ForeColor = light_white;
                        for (var j = 0; j < MP_dataGridView.Rows.Count; j++)
                        {
                            MP_dataGridView.Rows[j].Cells[i].Style.BackColor = dark_grey2;
                            MP_dataGridView.Rows[j].Cells[i].Style.ForeColor = light_white;
                        }
                    }
                }
        }
        public void Category_ToLight(Form form)
        {
            form.BackColor = light_grey1;
            form.ForeColor = light_grey4;
            form.BackgroundImage = Resources.Background_Border;
            foreach (Control c in form.Controls)
                if (c.GetType() == typeof(TableLayoutPanel)) /// header table
                {
                    c.BackColor = light_transparent;
                    c.ForeColor = light_grey4;
                    foreach (Control aa in c.Controls)
                        if (aa.GetType() == typeof(Label) || aa.GetType() == typeof(Button) ||
                            aa.GetType() == typeof(CheckBox) || aa.GetType() == typeof(RadioButton))
                        {
                            aa.ForeColor = light_grey4;
                            aa.BackColor = light_transparent;
                            if (aa.Name == "Close_button")
                                aa.BackgroundImage = Resources.Exit_D;
                        }
                        else if (aa.GetType() == typeof(TextBox) || aa.GetType() == typeof(ComboBox))
                        {
                            aa.BackColor = light_white;
                            aa.ForeColor = light_grey4;
                        }
                }
                else if (c.GetType() == typeof(Label) || c.GetType() == typeof(Button) ||
                         c.GetType() == typeof(CheckBox) || c.GetType() == typeof(RadioButton))
                {
                    c.ForeColor = light_grey4;
                    c.BackColor = light_transparent;

                    if (c.Name == "Save_button")
                        c.BackgroundImage = Resources.Save_CD;
                    else if (c.Name == "Delete_button")
                        c.BackgroundImage = Resources.Delete_CD;
                }
                else if (c.GetType() == typeof(TextBox) || c.GetType() == typeof(ComboBox))
                {
                    c.BackColor = light_white;
                    c.ForeColor = light_grey4;
                }
                else if (c.GetType() == typeof(DataGridView))
                {
                    var MP_dataGridView = c as DataGridView;
                    MP_dataGridView.BackgroundColor = MP_dataGridView.ColumnHeadersDefaultCellStyle.BackColor =
                        MP_dataGridView.DefaultCellStyle.BackColor =
                            MP_dataGridView.RowsDefaultCellStyle.BackColor = light_white;
                    MP_dataGridView.AlternatingRowsDefaultCellStyle.BackColor = light_grey11;

                    MP_dataGridView.ForeColor = MP_dataGridView.ColumnHeadersDefaultCellStyle.ForeColor =
                        MP_dataGridView.DefaultCellStyle.ForeColor =
                            MP_dataGridView.RowsDefaultCellStyle.ForeColor =
                                MP_dataGridView.AlternatingRowsDefaultCellStyle.ForeColor = light_grey4;

                    MP_dataGridView.GridColor = light_grey3;

                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor =
                        MP_dataGridView.DefaultCellStyle.SelectionBackColor
                            = MP_dataGridView.RowsDefaultCellStyle.SelectionBackColor =
                                MP_dataGridView.AlternatingRowsDefaultCellStyle.SelectionBackColor =
                                    light_grey2;
                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionForeColor =
                        MP_dataGridView.DefaultCellStyle.SelectionForeColor
                            = MP_dataGridView.RowsDefaultCellStyle.SelectionForeColor =
                                MP_dataGridView.AlternatingRowsDefaultCellStyle.SelectionForeColor = light_grey4;

                    for (var i = 0; i < MP_dataGridView.Columns.Count; i++)
                    {
                        MP_dataGridView.Columns[i].HeaderCell.Style.BackColor = light_white;
                        MP_dataGridView.Columns[i].HeaderCell.Style.ForeColor = light_grey4;
                    }
                }
        }

        public void About_ToNight(Form form)
        {
            form.BackColor = light_grey3;
            form.ForeColor = light_white;
            foreach (Control c in form.Controls)
                if (c.GetType() == typeof(Panel)) /// header table
                {
                    c.BackColor = light_transparent;
                    c.ForeColor = light_white;
                    foreach (Control aa in c.Controls)
                    {
                        if (aa.GetType() == typeof(Label) || aa.GetType() == typeof(Button) ||
                            aa.GetType() == typeof(CheckBox) || aa.GetType() == typeof(Panel))
                        {
                            aa.ForeColor = light_white;
                            aa.BackColor = light_transparent;
                            if (aa.Name == "Close_button")
                                aa.BackgroundImage = Resources.Exit_L;
                        }
                        else if (aa.GetType() == typeof(TextBox) || aa.GetType() == typeof(ComboBox))
                        {
                            aa.BackColor = dark_grey2;
                            aa.ForeColor = light_white;
                        }

                        if (aa.GetType() == typeof(TableLayoutPanel)) /// header table
                        {
                            aa.BackColor = light_transparent;
                            aa.ForeColor = light_white;
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
            form.BackColor = light_grey1;
            form.ForeColor = light_grey4;
            foreach (Control c in form.Controls)
                if (c.GetType() == typeof(Panel)) /// header table
                {
                    c.BackColor = light_transparent;
                    c.ForeColor = light_grey4;
                    foreach (Control aa in c.Controls)
                    {
                        if (aa.GetType() == typeof(Label) || aa.GetType() == typeof(Button) ||
                            aa.GetType() == typeof(CheckBox) || aa.GetType() == typeof(Panel))
                        {
                            aa.ForeColor = light_grey4;
                            aa.BackColor = light_transparent;
                            if (aa.Name == "Close_button")
                                aa.BackgroundImage = Resources.Exit_D;
                        }
                        else if (aa.GetType() == typeof(TextBox) || aa.GetType() == typeof(ComboBox))
                        {
                            aa.BackColor = light_white;
                            aa.ForeColor = light_grey4;
                        }

                        if (aa.GetType() == typeof(TableLayoutPanel)) /// header table
                        {
                            aa.BackColor = light_transparent;
                            aa.ForeColor = light_grey4;
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
            form.BackColor = light_grey3;
            form.ForeColor = light_white;

            foreach (Control c in form.Controls)
                if (c.GetType() == typeof(TableLayoutPanel)) /// 3 Tables
                {
                    c.BackColor = light_grey3;
                    c.ForeColor = light_white;
                    foreach (Control aa in c.Controls)
                        if (aa.GetType() == typeof(Label) || aa.GetType() == typeof(Button) ||
                            aa.GetType() == typeof(CheckBox) || aa.GetType() == typeof(RadioButton))
                        {
                            aa.ForeColor = light_white;
                            if (aa.Name.Contains("Header"))
                            {
                                aa.BackColor = light_grey2;
                                aa.ForeColor = dark_grey2;
                            }
                            else if (aa.Name.Contains("Save_button") || aa.Name.Contains("New_button"))
                            {
                                aa.BackColor = light_grey2;
                                aa.ForeColor = dark_grey2;
                            }
                        }
                        else if (aa.GetType() == typeof(TextBox) || aa.GetType() == typeof(ComboBox))
                        {
                            aa.BackColor = dark_grey2;
                            aa.ForeColor = light_white;
                        }
                        else if (aa.GetType() == typeof(DataGridView))
                        {
                            var MP_dataGridView = aa as DataGridView;
                            MP_dataGridView.BackgroundColor = MP_dataGridView.ColumnHeadersDefaultCellStyle.BackColor =
                                MP_dataGridView.DefaultCellStyle.BackColor =
                                    MP_dataGridView.RowsDefaultCellStyle.BackColor = dark_grey2;
                            MP_dataGridView.ForeColor = MP_dataGridView.ColumnHeadersDefaultCellStyle.ForeColor =
                                MP_dataGridView.DefaultCellStyle.ForeColor =
                                    MP_dataGridView.RowsDefaultCellStyle.ForeColor = light_white;

                            MP_dataGridView.GridColor = light_grey2;

                            MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor =
                                MP_dataGridView.DefaultCellStyle.SelectionBackColor
                                    = MP_dataGridView.RowsDefaultCellStyle.SelectionBackColor =
                                        light_grey3;
                            MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionForeColor =
                                MP_dataGridView.DefaultCellStyle.SelectionForeColor
                                    = MP_dataGridView.RowsDefaultCellStyle.SelectionForeColor = light_white;


                            for (var i = 0; i < MP_dataGridView.Columns.Count; i++)
                            {
                                MP_dataGridView.Columns[i].HeaderCell.Style.BackColor = dark_grey2;
                                MP_dataGridView.Columns[i].HeaderCell.Style.ForeColor = light_white;
                                for (var j = 0; j < MP_dataGridView.Rows.Count; j++)
                                {
                                    MP_dataGridView.Rows[j].Cells[i].Style.BackColor = dark_grey2;
                                    MP_dataGridView.Rows[j].Cells[i].Style.ForeColor = light_white;
                                }
                            }
                        }
                        else if (aa.GetType() == typeof(TableLayoutPanel))
                        {
                            aa.BackColor = light_grey3;
                            aa.ForeColor = light_white;
                            foreach (Control bb in aa.Controls)
                                if (bb.GetType() == typeof(Label) || bb.GetType() == typeof(Button) ||
                                    bb.GetType() == typeof(CheckBox) || bb.GetType() == typeof(RadioButton))
                                {
                                    bb.ForeColor = light_white;
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
            form.BackColor = light_grey1;
            form.ForeColor = light_grey4;
            foreach (Control c in form.Controls)
                if (c.GetType() == typeof(TableLayoutPanel)) /// 3 tables
                {
                    c.BackColor = light_grey1;
                    c.ForeColor = light_grey4;
                    foreach (Control aa in c.Controls)
                        if (aa.GetType() == typeof(Label) || aa.GetType() == typeof(Button) ||
                            aa.GetType() == typeof(CheckBox) || aa.GetType() == typeof(RadioButton))
                        {
                            aa.ForeColor = light_grey4;
                            if (aa.Name.Contains("Header"))
                            {
                                aa.BackColor = light_transparent;
                            }
                            else if (aa.Name.Contains("Save_button") || aa.Name.Contains("New_button"))
                            {
                                aa.BackColor = light_grey3;
                                aa.ForeColor = light_white;
                            }
                        }
                        else if (aa.GetType() == typeof(TextBox) || aa.GetType() == typeof(ComboBox))
                        {
                            aa.BackColor = light_white;
                            aa.ForeColor = light_grey4;
                        }
                        else if (aa.GetType() == typeof(DataGridView))
                        {
                            var MP_dataGridView = aa as DataGridView;
                            MP_dataGridView.BackgroundColor = MP_dataGridView.ColumnHeadersDefaultCellStyle.BackColor =
                                MP_dataGridView.DefaultCellStyle.BackColor =
                                    MP_dataGridView.RowsDefaultCellStyle.BackColor = light_white;

                            MP_dataGridView.ForeColor = MP_dataGridView.ColumnHeadersDefaultCellStyle.ForeColor =
                                MP_dataGridView.DefaultCellStyle.ForeColor =
                                    MP_dataGridView.RowsDefaultCellStyle.ForeColor = light_grey4;

                            MP_dataGridView.GridColor = light_grey3;

                            MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor =
                                MP_dataGridView.DefaultCellStyle.SelectionBackColor
                                    = MP_dataGridView.RowsDefaultCellStyle.SelectionBackColor =
                                        light_grey2;
                            MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionForeColor =
                                MP_dataGridView.DefaultCellStyle.SelectionForeColor
                                    = MP_dataGridView.RowsDefaultCellStyle.SelectionForeColor = light_grey4;

                            for (var i = 0; i < MP_dataGridView.Columns.Count; i++)
                            {
                                MP_dataGridView.Columns[i].HeaderCell.Style.BackColor = light_white;
                                MP_dataGridView.Columns[i].HeaderCell.Style.ForeColor = light_grey4;
                                for (var j = 0; j < MP_dataGridView.Rows.Count; j++)
                                {
                                    MP_dataGridView.Rows[j].Cells[i].Style.BackColor = light_white;
                                    MP_dataGridView.Rows[j].Cells[i].Style.ForeColor = light_grey4;
                                }
                            }
                        }
                        else if (aa.GetType() == typeof(TableLayoutPanel))
                        {
                            aa.BackColor = light_grey1;
                            aa.ForeColor = light_grey4;
                            foreach (Control bb in aa.Controls)
                                if (bb.GetType() == typeof(Label) || bb.GetType() == typeof(Button) ||
                                    bb.GetType() == typeof(CheckBox) || bb.GetType() == typeof(RadioButton))
                                {
                                    bb.ForeColor = light_grey4;
                                    if (bb.Name == "AddImage_button")
                                        bb.BackgroundImage = Resources.Plus_Sq_D;
                                    else if (bb.Name == "DeleteImage_button")
                                        bb.BackgroundImage = Resources.Trash_D;
                                }
                                else if (bb.GetType() == typeof(TextBox) || bb.GetType() == typeof(ComboBox))
                                {
                                    bb.BackColor = light_white;
                                    bb.ForeColor = light_grey4;
                                }
                        }
                }
        }

        public void ChooseVisit_ToNight(Form form)
        {
            form.BackColor = light_grey3;
            form.ForeColor = light_white;
            form.BackgroundImage = Resources.Back_700_D;
            foreach (Control c in form.Controls)
                //if (c.GetType() == typeof(TableLayoutPanel)) /// 
                //{
                //    c.BackColor = light_transparent;
                //    c.ForeColor = light_white;
                //    foreach (Control aa in c.Controls) 
                //} 
                if (c.GetType() == typeof(Label) ||
                            c.GetType() == typeof(CheckBox) || c.GetType() == typeof(RadioButton))
                {
                    c.ForeColor = light_white;
                    c.BackColor = light_transparent;

                }
                else if (c.GetType() == typeof(TextBox) || c.GetType() == typeof(ComboBox))
                {
                    c.BackColor = dark_grey2;
                    c.ForeColor = light_white;
                }
                else if (c.GetType() == typeof(DataGridView))
                {
                    var MP_dataGridView = c as DataGridView;
                    MP_dataGridView.BackgroundColor = MP_dataGridView.ColumnHeadersDefaultCellStyle.BackColor =
                        MP_dataGridView.DefaultCellStyle.BackColor =
                            MP_dataGridView.RowsDefaultCellStyle.BackColor = dark_grey2;

                    MP_dataGridView.ForeColor = MP_dataGridView.ColumnHeadersDefaultCellStyle.ForeColor =
                        MP_dataGridView.DefaultCellStyle.ForeColor =
                            MP_dataGridView.RowsDefaultCellStyle.ForeColor = light_white;

                    MP_dataGridView.GridColor = light_grey2;

                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor =
                        MP_dataGridView.DefaultCellStyle.SelectionBackColor
                            = MP_dataGridView.RowsDefaultCellStyle.SelectionBackColor = light_grey3;
                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionForeColor =
                        MP_dataGridView.DefaultCellStyle.SelectionForeColor
                            = MP_dataGridView.RowsDefaultCellStyle.SelectionForeColor = light_white;

                    for (var i = 0; i < MP_dataGridView.Columns.Count; i++)
                    {
                        MP_dataGridView.Columns[i].HeaderCell.Style.BackColor = dark_grey2;
                        MP_dataGridView.Columns[i].HeaderCell.Style.ForeColor = light_white;
                        for (var j = 0; j < MP_dataGridView.Rows.Count; j++)
                        {
                            MP_dataGridView.Rows[j].Cells[i].Style.BackColor = dark_grey2;
                            MP_dataGridView.Rows[j].Cells[i].Style.ForeColor = light_white;
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
            form.BackColor = light_grey1;
            form.ForeColor = light_grey4;
            form.BackgroundImage = Resources.Back_700;
            foreach (Control c in form.Controls)
                if (c.GetType() == typeof(Label) ||  c.GetType() == typeof(CheckBox) || c.GetType() == typeof(RadioButton))
                {
                    c.ForeColor = light_grey4;
                    c.BackColor = light_transparent;
                }
                else if (c.GetType() == typeof(TextBox) || c.GetType() == typeof(ComboBox))
                {
                    c.BackColor = light_white;
                    c.ForeColor = light_grey4;
                }
                else if (c.GetType() == typeof(DataGridView))
                {
                    var MP_dataGridView = c as DataGridView;
                    MP_dataGridView.BackgroundColor = MP_dataGridView.ColumnHeadersDefaultCellStyle.BackColor =
                        MP_dataGridView.DefaultCellStyle.BackColor =
                            MP_dataGridView.RowsDefaultCellStyle.BackColor = light_white;
                    MP_dataGridView.AlternatingRowsDefaultCellStyle.BackColor = light_grey11;

                    MP_dataGridView.ForeColor = MP_dataGridView.ColumnHeadersDefaultCellStyle.ForeColor =
                        MP_dataGridView.DefaultCellStyle.ForeColor =
                            MP_dataGridView.RowsDefaultCellStyle.ForeColor =
                                MP_dataGridView.AlternatingRowsDefaultCellStyle.ForeColor = light_grey4;

                    MP_dataGridView.GridColor = light_grey3;

                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor =
                        MP_dataGridView.DefaultCellStyle.SelectionBackColor
                            = MP_dataGridView.RowsDefaultCellStyle.SelectionBackColor =
                                MP_dataGridView.AlternatingRowsDefaultCellStyle.SelectionBackColor =
                                    light_grey2;
                    MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionForeColor =
                        MP_dataGridView.DefaultCellStyle.SelectionForeColor
                            = MP_dataGridView.RowsDefaultCellStyle.SelectionForeColor =
                                MP_dataGridView.AlternatingRowsDefaultCellStyle.SelectionForeColor = light_grey4;

                    for (var i = 0; i < MP_dataGridView.Columns.Count; i++)
                    {
                        MP_dataGridView.Columns[i].HeaderCell.Style.BackColor = light_white;
                        MP_dataGridView.Columns[i].HeaderCell.Style.ForeColor = light_grey4;
                    }
                }
                else if (c.GetType() == typeof(Button))
                {
                    if (c.Name == "Close_button")
                        c.BackgroundImage = Resources.Exit_D;
                }
            //if (c.GetType() == typeof(TableLayoutPanel)) /// table
            //{
            //    c.BackColor = light_transparent;
            //    c.ForeColor = light_grey4;
            //    foreach (Control aa in c.Controls)
            //        if (aa.GetType() == typeof(Label) || 
            //            aa.GetType() == typeof(CheckBox) || aa.GetType() == typeof(RadioButton))
            //        {
            //            aa.ForeColor = light_grey4;
            //            aa.BackColor = light_transparent;
            //        }
            //        else if (aa.GetType() == typeof(TextBox) || aa.GetType() == typeof(ComboBox))
            //        {
            //            aa.BackColor = light_white;
            //            aa.ForeColor = light_grey4;
            //        } 
            //        else if (aa.GetType() == typeof(DataGridView))
            //        {
            //            var MP_dataGridView = aa as DataGridView;
            //            MP_dataGridView.BackgroundColor = MP_dataGridView.ColumnHeadersDefaultCellStyle.BackColor =
            //                MP_dataGridView.DefaultCellStyle.BackColor =
            //                    MP_dataGridView.RowsDefaultCellStyle.BackColor = light_white;
            //            MP_dataGridView.AlternatingRowsDefaultCellStyle.BackColor = light_grey11;

            //            MP_dataGridView.ForeColor = MP_dataGridView.ColumnHeadersDefaultCellStyle.ForeColor =
            //                MP_dataGridView.DefaultCellStyle.ForeColor =
            //                    MP_dataGridView.RowsDefaultCellStyle.ForeColor =
            //                        MP_dataGridView.AlternatingRowsDefaultCellStyle.ForeColor = light_grey4;

            //            MP_dataGridView.GridColor = light_grey3;

            //            MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor =
            //                MP_dataGridView.DefaultCellStyle.SelectionBackColor
            //                    = MP_dataGridView.RowsDefaultCellStyle.SelectionBackColor =
            //                        MP_dataGridView.AlternatingRowsDefaultCellStyle.SelectionBackColor =
            //                            light_grey2;
            //            MP_dataGridView.ColumnHeadersDefaultCellStyle.SelectionForeColor =
            //                MP_dataGridView.DefaultCellStyle.SelectionForeColor
            //                    = MP_dataGridView.RowsDefaultCellStyle.SelectionForeColor =
            //                        MP_dataGridView.AlternatingRowsDefaultCellStyle.SelectionForeColor = light_grey4;

            //            for (var i = 0; i < MP_dataGridView.Columns.Count; i++)
            //            {
            //                MP_dataGridView.Columns[i].HeaderCell.Style.BackColor = light_white;
            //                MP_dataGridView.Columns[i].HeaderCell.Style.ForeColor = light_grey4;
            //            }
            //        } 
            //}

        }
         
    }
}