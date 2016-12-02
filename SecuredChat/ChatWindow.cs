using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SecuredChat
{
    public partial class ChatWindow : Form
    {
        Thread ListenerThread;
        TcpListener listener;
        TcpClient client;
        byte[] myReadBuffer = new byte[1024];
        bool isServer;

        public ChatWindow()
        {
            InitializeComponent();

            Welcome wlk = new Welcome();
            wlk.ShowDialog();
            isServer = wlk.IsServer;

            if (!wlk.IsReadyToStart)
                return;

            if (isServer)
            {
                listener = new TcpListener(System.Net.IPAddress.Any, wlk.Port);
                listener.Start();
            }
            else
            {
                client = new TcpClient(wlk.Ip, wlk.Port);
            }            

            ListenerThread = new Thread(DoListen);
            ListenerThread.IsBackground = true;
            ListenerThread.Start(); 
        }

        void DoListen()
        {
            richChat.Text += "System: Connecting..." + Environment.NewLine;
            if(isServer)
                client = listener.AcceptTcpClient();         
            NetworkStream netStream = client.GetStream();
            richChat.Text += "System: Connection established" + Environment.NewLine;

            while (true)
            {   if (netStream.CanRead)
                {
                    int numberOfBytesRead = 0;
                    string str = "";
                    try
                    {
                        numberOfBytesRead = netStream.Read(myReadBuffer, 0, myReadBuffer.Length);
                        str += Encoding.UTF8.GetString(myReadBuffer, 0, numberOfBytesRead);
                        richChat.Text += "Partner: " + str + Environment.NewLine;                      
                    }
                    catch (Exception)
                    {
                        richChat.Text += "System: Partner disconnected" + Environment.NewLine;
                        break;
                    }
                }
                else
                {
                    MessageBox.Show("Can`t read data!");
                }
            }
            netStream.Close();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            NetworkStream netStream = client.GetStream();
            if (netStream.CanWrite)
            {
                byte[] buf = Encoding.UTF8.GetBytes(richMessage.Text);
                netStream.Write(buf, 0, buf.Length);
                netStream.Flush();
            }
            else
            {
                MessageBox.Show("Error while sending message!");
            }
            richChat.Text += "Me: " + richMessage.Text + Environment.NewLine;
            richMessage.Clear();
        }

        private void CheckKeyword(string word, Color color, int startIndex)
        {
            if (this.richChat.Text.Contains(word))
            {
                int index = -1;
                int selectStart = this.richChat.SelectionStart;

                while ((index = this.richChat.Text.IndexOf(word, (index + 1))) != -1)
                {
                    this.richChat.Select((index + startIndex), word.Length);
                    this.richChat.SelectionColor = color;
                    this.richChat.SelectionFont = new Font(richChat.SelectionFont, FontStyle.Bold);
                    this.richChat.Select(selectStart, 0);
                    this.richChat.SelectionColor = Color.Black;
                    this.richChat.SelectionFont = new Font(richChat.SelectionFont, FontStyle.Regular);
                }
            }
        }

        private void richChat_TextChanged(object sender, EventArgs e)
        {
            CheckKeyword("Me:", Color.Blue, 0);
            CheckKeyword("Partner:", Color.Red, 0);
            //CheckKeyword("System:", Color.Purple, 0);
        }
    }
}
