using SecuredChat.Properties;
using System;
using System.Drawing;
using System.IO;
using System.Media;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SecuredChat
{
    public partial class ChatWindow : Form
    {
        delegate void SetTextCallback(string text);

        Thread ListenerThread;
        TcpListener listener;
        TcpClient client;
        RSACrypter crypter;
        SoundPlayer sp;
        byte[] myReadBuffer = new byte[2048];
        bool isServer;
        bool isPublicSertificateSent;
        bool isNotifications = true;

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
                this.Text = "[Server] Conversation";
                try
                {
                    listener = new TcpListener(System.Net.IPAddress.Any, wlk.Port);
                    listener.Start();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Cannot start the server.\nDetails: " + ex.Message, "Fatal Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Environment.Exit(1);
                }
            }
            else
            {
                this.Text = "[Client] Conversation";
                try
                {
                    client = new TcpClient(wlk.Ip, wlk.Port);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Cannot connect to the server.\nDetails: " + ex.Message, "Fatal Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Environment.Exit(1);
                }
            }

            AddMessageToRichChat("Welcome to the Secured Chat!" + Environment.NewLine);

            ListenerThread = new Thread(DoListen);
            ListenerThread.IsBackground = true;
            ListenerThread.Start(); 
        }

        void DoListen()
        {
            AddMessageToRichChat("System: Connecting..." + Environment.NewLine);
            if(isServer)
                client = listener.AcceptTcpClient();         
            NetworkStream netStream = client.GetStream();
            AddMessageToRichChat("System: Connection established" + Environment.NewLine);
            EnableInterface(null);

            while (true)
            {
                if (netStream.CanRead)
                {
                    int numberOfBytesRead = 0;
                    string str = "";
                    try
                    {
                        numberOfBytesRead = netStream.Read(myReadBuffer, 0, myReadBuffer.Length);
                        str += Encoding.Unicode.GetString(myReadBuffer, 0, numberOfBytesRead);

                        if (str.Contains("$PUBLICKEY$"))
                        {
                            if (crypter == null)
                                crypter = new RSACrypter();
                            crypter.ThirdPartyPublicKey = str.Replace("$PUBLICKEY$", "");
                            AddMessageToRichChat("System: The partner's public certificate has been accepted" + Environment.NewLine);
                            if (!isPublicSertificateSent)
                            {
                                AddMessageToRichChat("System: Creating RSA-2048 certificates" + Environment.NewLine);
                                AddMessageToRichChat("System: The public certificate has been sent to the partner" + Environment.NewLine);
                                SendMessage("$PUBLICKEY$" + crypter.PublicKey);
                                EnableEncryptionInterface(null);
                            }
                            AddMessageToRichChat("=========================================" + Environment.NewLine);
                            AddMessageToRichChat("System: Secure connection successfully established" + Environment.NewLine);
                            continue;
                        }

                        if (crypter == null)
                        {
                            AddMessageToRichChat("Partner: " + str + Environment.NewLine);
                        }
                        else
                        {
                            byte[] encrypted_buffer = new byte[numberOfBytesRead];
                            for (int i = 0; i < numberOfBytesRead; i++)
                            {
                                encrypted_buffer[i] = myReadBuffer[i];
                            }
                            string message = crypter.decryptMessage(encrypted_buffer);
                            if (!message.Contains("$DISENCRYPT$"))
                                AddMessageToRichChat("Partner: " + message + Environment.NewLine);
                            else
                                DisableEncryptionInterface(null);
                        }
                    }
                    catch (Exception ex)
                    {
                        netStream.Close();
                        AddMessageToRichChat("System: Partner disconnected. Reason: " + ex.Message + Environment.NewLine);
                        DisableInterface(null);
                        break;
                    }
                }
                else
                {
                    MessageBox.Show("Can`t read data!");
                }
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            SendMessage(richMessage.Text, crypter == null ? false : true);
            AddMessageToRichChat("Me: " + richMessage.Text + Environment.NewLine);
            richMessage.Clear();
            richChat.SelectionStart = richChat.Text.Length;
            richChat.ScrollToCaret();
        }

        private void SendMessage(string message, bool isEncrypt = false)
        {
            NetworkStream netStream = client.GetStream();
            if (netStream.CanWrite)
            {
                byte[] buf = isEncrypt ? crypter.encryptMessage(message) : Encoding.Unicode.GetBytes(message);
                netStream.Write(buf, 0, buf.Length);
                netStream.Flush();
            }
            else
            {
                MessageBox.Show("Error while sending message!");
            }
        }

        private void AddMessageToRichChat(string text)
        {
            if (this.richChat.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(AddMessageToRichChat);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.richChat.Text += text;
                this.richChat.SelectionStart = richChat.Text.Length;
                this.richChat.ScrollToCaret();
                if(isNotifications)
                {
                    if(!this.Focused && text.Contains("Partner: "))
                    {
                        notifyIcon1.ShowBalloonTip(7000, "New Message", text, ToolTipIcon.Info);
                        Assembly assembly = Assembly.GetExecutingAssembly();
                        using (Stream s = assembly.GetManifestResourceStream("SecuredChat.icq-message.wav"))
                        {
                            sp = new SoundPlayer(s);
                            sp.Play();
                        }
                    }                        
                }
                
            }
        }

        private void ResetRichMessage(string text)
        {
            if (this.richMessage.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(ResetRichMessage);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.richMessage.Clear();
            }
        }

        private void DisableInterface(string text)
        {
            if (this.richMessage.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(DisableInterface);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.richMessage.Clear();
                richMessage.Enabled = false;
                btnEncrypt.Enabled = false;
                btnSend.Enabled = false;
                btnNotifications.Enabled = false;
            }
        }

        private void EnableInterface(string text)
        {
            if (this.richMessage.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(EnableInterface);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.btnEncrypt.Enabled = true;
                this.richMessage.Enabled = true;
            }
        }

        private void DisableEncryptionInterface(string text)
        {
            if (this.richMessage.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(DisableEncryptionInterface);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                crypter = null;
                isPublicSertificateSent = false;
                btnEncrypt.Image = Resources.lock_open;
                AddMessageToRichChat("=========================================" + Environment.NewLine);
                AddMessageToRichChat("System: Encryption has been disabled" + Environment.NewLine);
                toolTip1.SetToolTip(btnEncrypt, "Enable Secured Mode");
                btnEncrypt.Enabled = true;
            }
        }

        private void EnableEncryptionInterface(string text)
        {
            if (this.richMessage.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(EnableEncryptionInterface);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                btnEncrypt.Image = Resources._lock;
                toolTip1.SetToolTip(btnEncrypt, "Disable Secured Mode");
                btnEncrypt.Enabled = true;
            }
        }

        private void CheckKeyword(string word, Color color, int startIndex)
        {
            if (this.richChat.Text.Contains(word))
            {
                int index = -1;
                int index2 = -1;
                richChat.DeselectAll();
                int selectStart = this.richChat.SelectionStart;

                while ((index = this.richChat.Text.IndexOf(word, (index + 1))) != -1)
                {
                    this.richChat.Select((index + startIndex), word.Length);
                    this.richChat.SelectionColor = color;
                    this.richChat.SelectionFont = new Font(richChat.SelectionFont, FontStyle.Bold);
                    this.richChat.Select(word.Length, index2);
                    this.richChat.SelectionColor = Color.Black;
                    this.richChat.SelectionFont = new Font(richChat.SelectionFont, FontStyle.Regular);
                   
                }                
            }
        }

        private void richChat_TextChanged(object sender, EventArgs e)
        {
            CheckKeyword("Me:", Color.Blue, 0);
            CheckKeyword("Partner:", Color.Red, 0);
            CheckKeyword("System:", Color.DarkGreen, 0);
        }

        private void richMessage_TextChanged(object sender, EventArgs e)
        {
            if (richMessage.Text.Length == 0)
                btnSend.Enabled = false;
            else
                btnSend.Enabled = true;
        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            btnEncrypt.Enabled = false;
            if (crypter == null)
            {
                btnEncrypt.Image = Resources._lock;
                AddMessageToRichChat("System: Initializing encryption procedure" + Environment.NewLine);
                AddMessageToRichChat("System: Creating RSA-2048 certificates" + Environment.NewLine);
                crypter = new RSACrypter();
                AddMessageToRichChat("System: The public certificate has been sent to the partner" + Environment.NewLine);
                SendMessage("$PUBLICKEY$" + crypter.PublicKey);
                isPublicSertificateSent = true;
                toolTip1.SetToolTip(btnEncrypt, "Disable Secured Mode");
                btnEncrypt.Enabled = true;
            }
            else
            {
                SendMessage("$DISENCRYPT$", true);
                DisableEncryptionInterface(null);
            }
        }

        private void btnNotifications_Click(object sender, EventArgs e)
        {
            if(isNotifications)
            {
                isNotifications = false;
                btnNotifications.Image = Resources.sound_mute;
                toolTip1.SetToolTip(btnNotifications, "Disable Notifications");
            }
            else
            {
                isNotifications = true;
                btnNotifications.Image = Resources.sound;
                toolTip1.SetToolTip(btnNotifications, "Enable Notifications");
            }
        }
    }
}
