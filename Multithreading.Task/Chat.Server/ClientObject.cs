using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Server
{
    class ClientObject
    {
        string _userName;
        TcpClient _client;
        ServerObject _server;

        protected internal string Id { get; set; }
        protected internal NetworkStream Stream { get; private set; }

        public ClientObject(TcpClient client, ServerObject server)
        {
            Id = Guid.NewGuid().ToString();
            _client = client;
            _server = server;
            server.AddConnection(this);
        }

        public void Process()
        {
            try
            {
                Stream = _client.GetStream();
                string message = GetMessage();
                _userName = message;
                message = string.Concat(_userName, " entered to the chat");
                Console.WriteLine(message);

                while (true)
                {
                    try
                    {
                        message = GetMessage();
                        message = String.Format("{0}: {1}", _userName, message);
                        Console.WriteLine(message);
                        _server.BroadcastMessage(message, this.Id);
                    }
                    catch
                    {
                        message = String.Format("{0}: has left", _userName);
                        Console.WriteLine(message);
                        _server.BroadcastMessage(message, this.Id);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                _server.RemoveConnection(this.Id);
                Close();
            }
        }

        private string GetMessage()
        {
            byte[] data = new byte[64];
            StringBuilder builder = new StringBuilder();
            int bytes = 0;
            do
            {
                bytes = Stream.Read(data, 0, data.Length);
                builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
            }
            while (Stream.DataAvailable);

            return builder.ToString();
        }

        protected internal void Close()
        {
            if (Stream != null)
                Stream.Close();
            if (_client != null)
                _client.Close();
        }
    }
}
