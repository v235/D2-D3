using ServerParam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chat.Server
{
    class ServerObject
    {
        static TcpListener _tcpListener; 
        List<ClientObject> _clients = new List<ClientObject>();

        protected internal void AddConnection(ClientObject clientObject)
        {
            _clients.Add(clientObject);
        }

        protected internal void RemoveConnection(string id)
        {
            ClientObject client = _clients.FirstOrDefault(c => c.Id == id);
            if (client != null)
                _clients.Remove(client);
        }
        protected internal void StartServer(object serverParam)
        {
            try
            {
                Params param = serverParam as Params;
                IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(param.Adress), param.Port);
                _tcpListener = new TcpListener(ipPoint);
                _tcpListener.Start();
                Console.WriteLine("Server running! Waiting for connection...");
                while (true)
                {
                    TcpClient tcpClient = _tcpListener.AcceptTcpClient();

                    ClientObject clientObject = new ClientObject(tcpClient, this);
                    Thread clientThread = new Thread(new ThreadStart(clientObject.Process));
                    clientThread.Start();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        protected internal void BroadcastMessage(string message, string id)
        {
            byte[] data = Encoding.Unicode.GetBytes(message);
            for (int i = 0; i < _clients.Count; i++)
            {
                if (_clients[i].Id != id) 
                {
                    _clients[i].Stream.Write(data, 0, data.Length); 
                }
            }
        }

        protected internal void Disconnect()
        {
            _tcpListener.Stop(); 

            for (int i = 0; i < _clients.Count; i++)
            {
                _clients[i].Close(); 
            }
            Environment.Exit(0); 
        }
    }
}
