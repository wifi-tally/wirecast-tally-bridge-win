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
        List<TcpClient> clients = new List<TcpClient>();
        private Byte[] lastBroadcast = null;
        public TcpServer(string ip, int port)
        {
            IPAddress localAddr = IPAddress.Parse(ip);
            server = new TcpListener(localAddr, port);
            server.Start();

            Thread t = new Thread(new ThreadStart(StartListener));
            t.Start();
        }
        public void StartListener()
        {
            try
            {
                Console.WriteLine("Server waiting for connections...");
                while (true)
                {
                    TcpClient client = server.AcceptTcpClient();
                    clients.Add(client);
                    Console.WriteLine("A new client connected. {0}", client.GetHashCode());
                    if (lastBroadcast != null) {
                        Send(client, lastBroadcast);
                    }
                    // Thread t = new Thread(new ParameterizedThreadStart(HandleDevice));
                    // t.Start(client);
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
                server.Stop();
            }
        }
        
        public void Broadcast(string message) {
            Byte[] broadcast = System.Text.Encoding.UTF8.GetBytes(message + "\n");
            if (hasMessageChanged(broadcast)) {
                lastBroadcast = broadcast;

                // we use a copy, so we can remove clients that do not respond
                List <TcpClient> clonedClients = clients.GetRange(0, clients.Count);
                foreach (TcpClient client in clonedClients) {
                    Send(client, broadcast);
                }
            }
        }

        private void Send(TcpClient client, Byte[] message) {
            try {
                var stream = client.GetStream();
                stream.Write(message, 0, message.Length);
            } catch (Exception e) {
                if (IsCausedBySocket(e)) {
                    Console.Error.WriteLine("Error when writing to socket: {0}.", e);
                    Console.Error.WriteLine("Cutting connection now");
                    Remove(client);
                } else {
                    throw e;
                }

            }
        }

        private void Remove(TcpClient client) {
            Console.WriteLine("Removed a client.");
            try {
                // we try to be nice
                client.Close();

            } catch (Exception e) {
                if (IsCausedBySocket(e)) {
                    // if "being nice" does not work, we get rude
                    client.Dispose();
                    Remove(client);
                } else {
                    throw e;
                }
            }
            clients.Remove(client);
        }
        
        private static Boolean IsCausedBySocket(Exception e) {
            while(e != null) {
                if(e is SocketException) {
                    return true;
                }
                e = e.InnerException;
            }
            return false;
        }

        private Boolean hasMessageChanged(Byte[] broadcast) {
            if (lastBroadcast == null && broadcast != null) {
                return true;
            }
            if (lastBroadcast.Length != broadcast.Length) {
                return true;
            }
            for (int i=0; i<lastBroadcast.Length; i++) {
                if (lastBroadcast[i] != broadcast[i]) {
                    return true;
                }
            }
            return false;
        }
    }

}