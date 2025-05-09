using System;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using MyWorkApplication.Classes;
using MyWorkApplication.Properties;

namespace MyWorkApplication
{
    public partial class AboutUs_Form : Form
    {
        public AboutUs_Form()
        {
            InitializeComponent();
        }


        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                VisitLink("https://www.youtube.com/channel/UC777Xf3lHtQufwaYjgxsiRA");
            }
            catch
            {
                MessageBox.Show("Unable to open link that was clicked.");
            }
        }

        private void VisitLink(string url)
        {
            // Change the color of the link text by setting LinkVisited   
            // to true.  
            linkLabel1.LinkVisited = true;
            //Call the Process.Start method to open the default browser   
            //with a URL:  
            Process.Start(url);
        }

        private void AboutUs_Form_Load(object sender, EventArgs e)
        {
            var NewTheme = new NewTheme();
            if (Settings.Default.theme == "Light")
                NewTheme.About_ToLight(this);
            else
                NewTheme.About_ToNight(this);

            panel1.AutoScrollPosition = new Point(0, 0);
            CopyRight_label.Text =
                "Copyright © 2018-2020 HOPE CENTER " + Assembly.GetExecutingAssembly().GetName().Version;
        }

        private void Close_button_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void linkLabel1_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                VisitLink("https://hcsyria.org");
            }
            catch
            {
                MessageBox.Show("Unable to open link that was clicked.");
            }
        }
    }
}