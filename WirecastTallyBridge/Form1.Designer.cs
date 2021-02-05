namespace WirecastTallyBridge
{
    partial class Form1
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.ServerState = new System.Windows.Forms.Label();
            this.WirecastState = new System.Windows.Forms.Label();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.TcpServer = new System.ComponentModel.BackgroundWorker();
            this.Wirecast = new System.ComponentModel.BackgroundWorker();
            this.LogBox = new System.Windows.Forms.TextBox();
            this.ServerLabel = new System.Windows.Forms.Label();
            this.WirecastLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ServerState
            // 
            this.ServerState.AutoSize = true;
            this.ServerState.BackColor = System.Drawing.SystemColors.Control;
            this.ServerState.Location = new System.Drawing.Point(126, 23);
            this.ServerState.Name = "ServerState";
            this.ServerState.Size = new System.Drawing.Size(35, 13);
            this.ServerState.TabIndex = 1;
            this.ServerState.Text = "label1";
            // 
            // WirecastState
            // 
            this.WirecastState.AutoSize = true;
            this.WirecastState.Location = new System.Drawing.Point(126, 57);
            this.WirecastState.Name = "WirecastState";
            this.WirecastState.Size = new System.Drawing.Size(35, 13);
            this.WirecastState.TabIndex = 2;
            this.WirecastState.Text = "label2";
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Text = "notifyIcon1";
            this.notifyIcon1.Visible = true;
            // 
            // TcpServer
            // 
            this.TcpServer.DoWork += new System.ComponentModel.DoWorkEventHandler(this.TcpServer_DoWork);
            this.TcpServer.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.TcpServer_RunWorkerCompleted);
            // 
            // Wirecast
            // 
            this.Wirecast.DoWork += new System.ComponentModel.DoWorkEventHandler(this.Wirecast_DoWork);
            this.Wirecast.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.Wirecast_RunWorkerCompleted);
            // 
            // LogBox
            // 
            this.LogBox.AcceptsReturn = true;
            this.LogBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LogBox.Location = new System.Drawing.Point(12, 93);
            this.LogBox.Multiline = true;
            this.LogBox.Name = "LogBox";
            this.LogBox.ReadOnly = true;
            this.LogBox.Size = new System.Drawing.Size(540, 345);
            this.LogBox.TabIndex = 3;
            // 
            // ServerLabel
            // 
            this.ServerLabel.AutoSize = true;
            this.ServerLabel.Location = new System.Drawing.Point(16, 23);
            this.ServerLabel.Name = "ServerLabel";
            this.ServerLabel.Size = new System.Drawing.Size(57, 13);
            this.ServerLabel.TabIndex = 4;
            this.ServerLabel.Text = "TcpServer";
            // 
            // WirecastLabel
            // 
            this.WirecastLabel.AutoSize = true;
            this.WirecastLabel.Location = new System.Drawing.Point(16, 57);
            this.WirecastLabel.Name = "WirecastLabel";
            this.WirecastLabel.Size = new System.Drawing.Size(49, 13);
            this.WirecastLabel.TabIndex = 5;
            this.WirecastLabel.Text = "Wirecast";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(564, 450);
            this.Controls.Add(this.WirecastLabel);
            this.Controls.Add(this.ServerLabel);
            this.Controls.Add(this.LogBox);
            this.Controls.Add(this.WirecastState);
            this.Controls.Add(this.ServerState);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Wirecast Tally Bridge";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label ServerState;
        private System.Windows.Forms.Label WirecastState;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.ComponentModel.BackgroundWorker TcpServer;
        private System.ComponentModel.BackgroundWorker Wirecast;
        private System.Windows.Forms.TextBox LogBox;
        private System.Windows.Forms.Label ServerLabel;
        private System.Windows.Forms.Label WirecastLabel;
    }
}

