using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IronBarCode;

namespace MyWorkApplication
{
    public partial class QR_Form : Form
    {
        public QR_Form()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //GeneratedBarcode Qrcode = QRCodeWriter.CreateQrCode(textBox1.Text);
            //Qrcode.SaveAsPng("QrCode.png");
            string ImageFileName = "";

            // open file dialog
            OpenFileDialog open = new OpenFileDialog();
            // image filters
            open.Filter = "Image Files(*.jpg; *.png; *.jpeg; *.gif; *.bmp)|*.jpg; *.png; *.jpeg; *.gif; *.bmp";
            if (open.ShowDialog() == DialogResult.OK)
            {
                // display image in picture box
                pictureBox1.Image = new Bitmap(open.FileName);
                // store image file path in class data member. Initialize it as string ImageFileName;
                ImageFileName = open.FileName;
            }

            // Adding a Logo
            GeneratedBarcode Qrcode = QRCodeWriter.CreateQrCodeWithLogo(textBox1.Text, ImageFileName, 300, 0);
            Qrcode.SaveAsPng("D:/QrCodeWithImage.png");
            pictureBox1.ImageLocation = "D:/QrCodeWithImage.png";

            //Save as PDF
            Qrcode.SaveAsPdf("D:/QRWithLogo.pdf");
            //Also Save as HTML
            Qrcode.SaveAsHtmlFile("D:/QRWithLogo.html");
        }
    }
}
