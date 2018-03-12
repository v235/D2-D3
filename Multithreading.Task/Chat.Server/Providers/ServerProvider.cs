using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chat.Server.Providers
{
    internal class ServerProvider
    {
        Socket _serverSocket;
        string _adress;
        int _port;
        int _clientListenCount;

        class ConnectionInfo
        {
            public Socket Socket;
            public Thread Thread;
        }

        Thread _acceptThread;
        List<ConnectionInfo> _connections = new List<ConnectionInfo>();
        List<ConnectionInfo> _exceptConnections = new List<ConnectionInfo>();
        List<string> _allRecivedMessages = new List<string>();

        public ServerProvider(string adress, int port, int clientListenCount)
        {
            _adress = adress;
            _port = port;
            _clientListenCount = clientListenCount;
        }

        public void Start()
        {
            SetupServerSocket();
            _acceptThread = new Thread(AcceptConnections);
            _acceptThread.IsBackground = true;
            _acceptThread.Start();
        }

        private void SetupServerSocket()
        {
            IPEndPoint myEndpoint = new IPEndPoint(IPAddress.Parse(_adress), _port);
            _serverSocket = new Socket(myEndpoint.Address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _serverSocket.Bind(myEndpoint);
            _serverSocket.Listen(_clientListenCount);
            Console.WriteLine("Server started! Waiting for connections...");
        }
        private void AcceptConnections()
        {
            while (true)
            {
                Socket socket = _serverSocket.Accept();
                ConnectionInfo connection = new ConnectionInfo();
                connection.Socket = socket;

                connection.Thread = new Thread(ProcessConnection);
                connection.Thread.IsBackground = true;
                connection.Thread.Start(connection);

                Console.WriteLine("SomeOne is connected");

                lock (_connections) _connections.Add(connection);

            }
        }

        private void ProcessConnection(object state)
        {
            ConnectionInfo connection = (ConnectionInfo)state;
            ConnectionInfo conToSend = null;
            byte[] buffer = new byte[255];
            int bytesRead = 0;
            try
            {
                while (true)
                {

                    bytesRead = connection.Socket.Receive(buffer);
                    if (bytesRead > 0)
                    {
                        StringBuilder builder = new StringBuilder();
                        builder.Append(Encoding.Unicode.GetString(buffer, 0, bytesRead));
                        string recivedMessge = builder.ToString();
                        if (recivedMessge.LastIndexOf(":") > 0)
                        {
                            string onlyMessage = recivedMessge.Substring(recivedMessge.LastIndexOf(':') + 1);
                            if (!onlyMessage.Equals("exit1"))
                            {
                                Console.WriteLine(recivedMessge);
                                lock (_connections)
                                {
                                    _allRecivedMessages.Add(recivedMessge);
                                    SendResponce(GetNextSender(connection), buffer, bytesRead);
                                }
                            }
                            else
                            {
                                lock (_connections)
                                {
                                    if (_connections.Count() > 1)
                                    {
                                        _connections.Remove(connection);
                                        _exceptConnections.Remove(connection);
                                        buffer = Encoding.Unicode.GetBytes(_allRecivedMessages.Last());
                                        SendResponce(_connections.Last(), buffer, buffer.Length);
                                    }
                                    else
                                    {
                                        Console.WriteLine(string.Concat(_allRecivedMessages.Last().Substring(0, _allRecivedMessages.Last().LastIndexOf(":") + 1), "WIN!!!"));
                                        _connections.Remove(connection);
                                    }
                                }

                            }
                        }
                    }
                    else return;
                }
            }
            catch (SocketException exc)
            {
                Console.WriteLine("Socket exception: " +
                    exc.SocketErrorCode);
            }
            catch (Exception exc)
            {
                Console.WriteLine("Exception: " + exc);
            }
            finally
            {
                connection.Socket.Close();
                lock (_connections) _connections.Remove(
                    connection);
            }
        }
        private void SendResponce(ConnectionInfo conToSend, byte[] buffer, int bytesRead)
        {
            if (conToSend != null)
            {
                conToSend.Socket.Send(
                buffer, bytesRead,
                SocketFlags.None);
            }
        }
        private ConnectionInfo GetNextSender(ConnectionInfo connection)
        {
            ConnectionInfo conToSend;
            if (_exceptConnections.Count < _connections.Count)
            {
                conToSend = _connections.Except(_exceptConnections).FirstOrDefault(c => !c.Equals(connection));
                _exceptConnections.Add(conToSend);
                return conToSend;
            }
            _exceptConnections.RemoveAt(0);
            conToSend = _connections.Except(_exceptConnections).FirstOrDefault();
            _exceptConnections.Add(conToSend);
            return conToSend;

        }
    }
}
