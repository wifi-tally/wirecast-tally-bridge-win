using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WirecastTallyBridge
{
    public partial class Form1 : Form
    {
        internal TcpServer server;
        internal WirecastWatcher wirecast;

        public Form1()
        {
            InitializeComponent();
            SetWirecastDisconnected();
            SetTcpServerDisconnected();
            TcpServer.RunWorkerAsync();
            Wirecast.RunWorkerAsync();
        }

        internal void SetTcpServer(TcpServer newServer)
        {
            server = newServer;
        }

        internal void SetWirecastWatcher(WirecastWatcher newWirecast)
        {
            wirecast = newWirecast;
        }

        void SetTcpServerLabel(string text)
        {
            if (ServerLabel.InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(() => ServerLabel.Text = text));
            }
            else
            {
                ServerLabel.Text = text;
            }
        }

        void SetTcpServerText(string text)
        {
            if (ServerState.InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(() => ServerState.Text = text));
            } else
            {
                ServerState.Text = text;
            }
        }

        void SetWirecastText(string text)
        {
            if (WirecastState.InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(() => WirecastState.Text = text));
            } else
            {
                WirecastState.Text = text;
            }
        }

        internal void SetWirecastConnected()
        {
            SetWirecastText("Connnected");
        }

        internal void SetWirecastDisconnected()
        {
            SetWirecastText("Disconnnected");
        }

        internal void SetWirecastNoDocument()
        {
            SetWirecastText("NoDocumentOpen");
        }

        internal void SetTcpServerConnectionCount(uint number)
        {
            SetTcpServerText(string.Format("{0} connections",  number));
        }

        internal void SetTcpServerListening(uint port)
        {
            SetTcpServerLabel(string.Format("TcpServer (:{0})", port));
            SetTcpServerText("no connections");
        }

        internal void SetTcpServerDisconnected()
        {
            SetTcpServerText("Disconnected");
        }

        internal void WriteLog(string line)
        {
            BeginInvoke(new MethodInvoker(() => LogBox.Text += line + "\r\n"));
            Console.Out.WriteLineAsync(line);
        }

        internal void WriteError(string line)
        {
            BeginInvoke(new MethodInvoker(() => LogBox.Text += line + "\r\n"));
            Console.Error.WriteLineAsync(line);
        }

        public void ServerBroadcast(string message)
        {
            if(server != null)
            {
                server.Broadcast(message);
            }
        }
        
        private void TcpServer_DoWork(object sender, DoWorkEventArgs e)
        {
            new TcpServer(this, "0.0.0.0", 1234);
        }

        private void TcpServer_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // this should usually not happen. Consider process crashed. It should restart.
            SetTcpServerDisconnected();
        }

        private void Wirecast_DoWork(object sender, DoWorkEventArgs e)
        {
            new WirecastWatcher(this);
        }

        private void Wirecast_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // this should usually not happen. Consider process crashed. It should restart.
            SetWirecastDisconnected();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
