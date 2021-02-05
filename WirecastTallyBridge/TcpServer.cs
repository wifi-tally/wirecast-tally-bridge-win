using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace WirecastTallyBridge
{
    public class TcpServer
    {
        TcpListener server = null;
        private Form1 app = null;

        List<TcpClient> clients = new List<TcpClient>();
        private Byte[] lastBroadcast = null;
        public TcpServer(Form1 form, string ip, int port)
        {
            app = form;
            app.SetTcpServer(this);
            IPAddress localAddr = IPAddress.Parse(ip);
            server = new TcpListener(localAddr, port);
            server.Start();
            app.SetTcpServerListening((uint)port);
            StartListener();
        }
        public void StartListener()
        {
            try
            {
                app.WriteLog("Server waiting for connections...");
                while (true)
                {
                    TcpClient client = server.AcceptTcpClient();
                    clients.Add(client);
                    app.WriteLog(string.Format("A new client connected. {0}", client.GetHashCode()));
                    app.SetTcpServerConnectionCount((uint)clients.Count);
                    if (lastBroadcast != null)
                    {
                        Send(client, lastBroadcast);
                    }
                    Thread t = new Thread(new ParameterizedThreadStart(HandleClient));
                    t.Start(client);
                }
            }
            catch (SocketException e)
            {
                app.WriteError(string.Format("SocketException: {0}", e));
            }
            finally
            {
                app.SetTcpServerDisconnected();
                server.Stop();
            }
        }

        public void HandleClient(Object obj)
        {
            TcpClient client = (TcpClient)obj;
            var stream = client.GetStream();
            Byte[] bytes = new Byte[256];
            int i;
            try
            {
                while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                   // just discard. We are not listening for commands.
                }

                // when 0 bytes are returned this means the socket has been closed peacefully
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.ToString());
                
            }
            finally
            {
                app.WriteLog("Client disconnected.");
                Remove(client);
                client.Close();
            }
        }

        public void Broadcast(string message)
        {
            Byte[] broadcast = System.Text.Encoding.UTF8.GetBytes(message + "\n");
            if (hasMessageChanged(broadcast))
            {
                lastBroadcast = broadcast;

                // we use a copy, so we can remove clients that do not respond
                List<TcpClient> clonedClients = clients.GetRange(0, clients.Count);
                foreach (TcpClient client in clonedClients)
                {
                    Send(client, broadcast);
                }
            }
        }

        private void Send(TcpClient client, Byte[] message)
        {
            try
            {
                var stream = client.GetStream();
                stream.Write(message, 0, message.Length);
            }
            catch (Exception e)
            {
                if (IsCausedBySocket(e))
                {
                    Console.Error.WriteLine("Error when writing to socket: {0}.", e);
                    app.WriteLog("Disconnecting client.");
                    Remove(client);
                }
                else
                {
                    throw e;
                }

            }
        }

        private void Remove(TcpClient client)
        {
            try
            {
                // we try to be nice
                client.Close();

            }
            catch (Exception e)
            {
                if (IsCausedBySocket(e))
                {
                    // if "being nice" does not work, we get rude
                    client.Dispose();
                    Remove(client);
                }
                else
                {
                    throw e;
                }
            }
            clients.Remove(client);
            app.SetTcpServerConnectionCount((uint)clients.Count);
        }

        private static Boolean IsCausedBySocket(Exception e)
        {
            while (e != null)
            {
                if (e is SocketException)
                {
                    return true;
                }
                e = e.InnerException;
            }
            return false;
        }

        private Boolean hasMessageChanged(Byte[] broadcast)
        {
            if (lastBroadcast == null && broadcast != null)
            {
                return true;
            }
            if (lastBroadcast.Length != broadcast.Length)
            {
                return true;
            }
            for (int i = 0; i < lastBroadcast.Length; i++)
            {
                if (lastBroadcast[i] != broadcast[i])
                {
                    return true;
                }
            }
            return false;
        }
    }

}