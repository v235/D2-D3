using ServerParam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chat.Client
{
    class ClientProvider
    {
        static string _userName;
        static TcpClient _client;
        static NetworkStream _stream;

        public void StartClient(string adress, int port)
        {
            _userName = Thread.CurrentThread.Name;
            _client = new TcpClient();
            try
            {
                _client.Connect(adress, port);
                Console.WriteLine("Client is running...");
                _stream = _client.GetStream();

                string message = _userName;
                byte[] data = Encoding.Unicode.GetBytes(message);
                _stream.Write(data, 0, data.Length);

                Thread receiveThread = new Thread(new ThreadStart(ReceiveMessage));
                receiveThread.Start(); 
                Console.WriteLine("Hello!, {0}", _userName);
                Console.WriteLine("Now you can exchange messages!");
                Console.WriteLine("Please, start your conversation");
                SendMessage();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Disconnect();
            }
        }

        private void SendMessage()
        {
            while (true)
            {
                string message = Console.ReadLine();
                byte[] data = Encoding.Unicode.GetBytes(message);
                _stream.Write(data, 0, data.Length);
            }
        }

        private void ReceiveMessage()
        {
            while (true)
            {
                try
                {
                    byte[] data = new byte[64];
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    do
                    {
                        bytes = _stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (_stream.DataAvailable);

                    string message = builder.ToString();
                    Console.WriteLine(message);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Disconnect();
                }
            }
        }

        private void Disconnect()
        {
            if (_stream != null)
                _stream.Close();
            if (_client != null)
                _client.Close();
            Environment.Exit(0);
        }

    }
}
