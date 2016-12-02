using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SecuredChat
{
    public partial class Welcome : Form
    {
        String ip;
        int port;
        bool isServer;
        bool isReadyToStart;

        public string Ip
        {
            get
            {
                return ip;
            }
        }

        public int Port
        {
            get
            {
                return port;
            }
        }

        public bool IsServer
        {
            get
            {
                return isServer;
            }
        }

        public bool IsReadyToStart
        {
            get
            {
                return isReadyToStart;
            }
        }

        public Welcome()
        {
            InitializeComponent();
        }

        private void radStartClient_CheckedChanged(object sender, EventArgs e)
        {
            txtIP.Enabled = radStartClient.Checked;
        }

        private void butStart_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtPort.Text))
            {
                MessageBox.Show("Please specify port", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
                
            if(radStartClient.Checked && String.IsNullOrEmpty(txtIP.Text))
            {
                MessageBox.Show("Please specify IP address", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            isServer = radStartServer.Checked;
            port = Convert.ToInt32(txtPort.Text);
            ip = txtIP.Text;
            isReadyToStart = true;
            Close();
        }

        private void Welcome_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!isReadyToStart)
                Application.Exit();
        }
    }
}
