using System;
using System.Windows.Forms;

namespace MyWorkApplication
{
    public partial class Application_MessageBox : Form
    {
        private int MicroProject_ID;
        private readonly string name;
        private int Person_ID;

        public Application_MessageBox()
        {
            InitializeComponent();
        }

        public Application_MessageBox(int Person_ID, int MicroProject_ID, string name)
        {
            InitializeComponent();
            this.Person_ID = Person_ID;
            this.MicroProject_ID = MicroProject_ID;
            this.name = name;
        }

        private void Application_MessageBox_Load(object sender, EventArgs e)
        {
            try
            {
                Name_label.Text = name + " " + " موجود مسبقاً ";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Old_button_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult = DialogResult.Yes;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void New_button_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult = DialogResult.No;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Back_button_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}