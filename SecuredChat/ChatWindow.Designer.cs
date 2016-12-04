namespace SecuredChat
{
    partial class ChatWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChatWindow));
            this.richChat = new System.Windows.Forms.RichTextBox();
            this.richMessage = new System.Windows.Forms.RichTextBox();
            this.btnEncrypt = new System.Windows.Forms.Button();
            this.btnSend = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // richChat
            // 
            this.richChat.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richChat.Location = new System.Drawing.Point(12, 12);
            this.richChat.Name = "richChat";
            this.richChat.ReadOnly = true;
            this.richChat.Size = new System.Drawing.Size(331, 271);
            this.richChat.TabIndex = 0;
            this.richChat.Text = "";
            this.richChat.TextChanged += new System.EventHandler(this.richChat_TextChanged);
            // 
            // richMessage
            // 
            this.richMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richMessage.Enabled = false;
            this.richMessage.Location = new System.Drawing.Point(13, 289);
            this.richMessage.MaxLength = 122;
            this.richMessage.Name = "richMessage";
            this.richMessage.Size = new System.Drawing.Size(243, 60);
            this.richMessage.TabIndex = 1;
            this.richMessage.Text = "";
            this.toolTip1.SetToolTip(this.richMessage, "Type your message here");
            this.richMessage.TextChanged += new System.EventHandler(this.richMessage_TextChanged);
            // 
            // btnEncrypt
            // 
            this.btnEncrypt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEncrypt.Enabled = false;
            this.btnEncrypt.Image = global::SecuredChat.Properties.Resources.lock_open;
            this.btnEncrypt.Location = new System.Drawing.Point(259, 289);
            this.btnEncrypt.Name = "btnEncrypt";
            this.btnEncrypt.Size = new System.Drawing.Size(29, 60);
            this.btnEncrypt.TabIndex = 4;
            this.toolTip1.SetToolTip(this.btnEncrypt, "Enable Secured Mode");
            this.btnEncrypt.UseVisualStyleBackColor = true;
            this.btnEncrypt.Click += new System.EventHandler(this.btnEncrypt_Click);
            // 
            // btnSend
            // 
            this.btnSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSend.Enabled = false;
            this.btnSend.Image = global::SecuredChat.Properties.Resources.comment;
            this.btnSend.Location = new System.Drawing.Point(291, 289);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(52, 60);
            this.btnSend.TabIndex = 2;
            this.btnSend.Text = "Send";
            this.btnSend.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSend.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolTip1.SetToolTip(this.btnSend, "Send Message");
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // ChatWindow
            // 
            this.AcceptButton = this.btnSend;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(355, 361);
            this.Controls.Add(this.btnEncrypt);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.richMessage);
            this.Controls.Add(this.richChat);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(350, 400);
            this.Name = "ChatWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Conversation";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox richChat;
        private System.Windows.Forms.RichTextBox richMessage;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Button btnEncrypt;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}

