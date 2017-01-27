using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

/*
 * BE AWARE THAT: Chuck Norris doesn't initiate conversations with a TCP handshake. He uses a TCP roundhouse kick.
 */

namespace SynchronousTCPClientLibrary
{
    public class TCPClient
    {
        private static Boolean connected = false;
        private static Socket socket;
        private static Boolean run;

        public delegate void onDataReceivedEvent(string message);
        public delegate void onConnectEvent(Socket socket);
        public delegate void onDisconnectEvent(Socket socket);

        public static event onDataReceivedEvent onDataReceived;
        public static event onConnectEvent onConnected;
        public static event onDisconnectEvent onDisconnect;

        public static void connectFromNewThread(string address, int port)
        {
                new Thread(() => connect(Dns.GetHostAddresses(address)[0], port)).Start();
        }

        public static void connectFromNewThread(string address,string port)
        {
            connectFromNewThread(address, int.Parse(port));
        }

        public static void connect(String address, int port)
        {
            connect(Dns.GetHostAddresses(address)[0], port);
        }

        public static void connect(string address, string port)
        {
            connect(address, int.Parse(port));
        }

        public static void connect(IPAddress ipAddress, int port)
        {
            byte[] bytes = new byte[1024];
            run = true;
            try
            {
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);
                socket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    socket.Connect(remoteEP);
                    if (onConnected != null)
                        onConnected.Invoke(socket);
                    connected = true;
                    Debug.WriteLine("Connected to " +
                        socket.RemoteEndPoint);

                    while (run)
                    {
                        int bytesRec = 0;
                        try
                        {
                           bytesRec = socket.Receive(bytes);
                        }
                        catch {
                            if (onDisconnect != null)
                                onDisconnect.Invoke(socket);
                            connected = false;
                            break;
                        }
                        

                        if (bytesRec == 0)
                        {
                            socket.Shutdown(SocketShutdown.Both);
                            socket.Close();

                            if (onDisconnect != null)
                                onDisconnect.Invoke(socket);
                            connected = false;
                            break;
                        }
                        if (onDataReceived != null)
                            onDataReceived.Invoke(Encoding.ASCII.GetString(bytes, 0, bytesRec));
                    }
                }
                catch (ArgumentNullException ex)
                {
                    Debug.WriteLine( ex.ToString());
                }
                catch (SocketException ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
                catch (Exception ex)
                {
                    Debug.WriteLine( ex.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        public static void send(string message)
        {
            socket.Send(Encoding.ASCII.GetBytes(message));
        }

        public static void sendln(string message)
        {
            send(message + Environment.NewLine);
        }

        public static void stopClient()
        {
            run = false;
            connected = false;
            try
            {
                socket.Shutdown(SocketShutdown.Both);
            }
            catch (Exception) { }
            socket.Close();
        }

        public static bool Connected
        {
            get { return connected; }
        }
    }
}
