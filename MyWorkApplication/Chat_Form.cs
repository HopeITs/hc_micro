using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using MyWorkApplication.Properties;

namespace MyWorkApplication
{
    public partial class Chat_Form : Form
    {
        private TcpClient clientSocket = new TcpClient();
        private bool foundIPAddress;
        private string readData;
        private StreamReader sr;
        private StreamWriter sw;
        private string username, password;

        public Chat_Form()
        {
            InitializeComponent();

            send_Btn.Enabled = false;
            onlineClients_Btn.Visible = false;
            // disconnect_Btn.Visible = false;
            // connect_Btn.Visible = true;
        }

        private void Chat_Form_Load(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void connect()
        {
            try
            {
                // try to connect
                clientSocket = new TcpClient();
                username = Program.UserName;
                password = Program.Password;
                Username_label.Text = Program.UserName;

                IPAddress serverIP = null;

                while (!foundIPAddress)
                {
                    viewMessages_richTextbox.Text = "please wait while we can find the server ";
                    var ipendpoint = new IPEndPoint(IPAddress.Any, 0);
                    var udp = new UdpClient(8888);
                    var bytes = udp.Receive(ref ipendpoint);
                    var message = Encoding.ASCII.GetString(bytes);

                    //when recieving th message from the server that contains his ip
                    if (message == "I am the server")
                    {
                        serverIP = ipendpoint.Address;
                        foundIPAddress = true;
                        udp.Close();
                    }
                }

                foundIPAddress = false;

                //connect to the server
                clientSocket.Connect(serverIP, 8888);

                sr = new StreamReader(clientSocket.GetStream());
                sw = new StreamWriter(clientSocket.GetStream());
                //send username & password to the server
                sw.WriteLine(username);
                sw.WriteLine(password);
                sw.Flush();

                readData = null;
                while (readData == null)
                    //wait until recieving the result of validation
                    readData = sr.ReadLine();

                if (readData == "Access Denied")
                {
                    viewMessages_richTextbox.Text = "Access Denied";
                    clientSocket.Close();
                }
                else if (readData == "User is already login")
                {
                    viewMessages_richTextbox.Text = "User is already login";
                    //clientSocket.Close();
                }
                else
                {
                    send_Btn.Enabled = true;
                    onlineClients_Btn.Visible = true;
                    //connect_Btn.Visible = false;
                    // disconnect_Btn.Visible = true;
                    readData = "Conected to Server ...";
                    msg();
                    var ctThread = new Thread(getMessage);
                    ctThread.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //recieve messages from server
        private void getMessage()
        {
            try
            {
                repeat:
                readData = null;
                while (readData == null)
                {
                    readData = sr.ReadLine();
                    if (readData == "Disconnected from server")
                    {
                        clientSocket.Close();
                        return;
                    }
                }

                msg();
                goto repeat;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //print messages after recieving
        private void msg()
        {
            if (InvokeRequired)
                Invoke(new MethodInvoker(msg));
            else
                viewMessages_richTextbox.Text = viewMessages_richTextbox.Text + Environment.NewLine + " >> " + readData;
        }

        private void messageToSend_Textbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //put the message in client's buffer
                sw.WriteLine(messageToSend_Textbox.Text);
                sw.Flush();
                messageToSend_Textbox.Clear();
            }
        }

        private void onlineClients_Btn_Click(object sender, EventArgs e)
        {
            //send a request for knowing online clients
            sw.WriteLine("online clients");
            sw.Flush();
        }

        private void send_Btn_Click(object sender, EventArgs e)
        {
            //put the message in client's buffer
            sw.WriteLine(messageToSend_Textbox.Text);
            sw.Flush();
            messageToSend_Textbox.Clear();
        }

        private void Close_button_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            connect();
        }

        private void Chat_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                sw.WriteLine("disconnect");
                sw.WriteLine(username);
                sw.WriteLine(password);
                sw.Flush();
                viewMessages_richTextbox.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}