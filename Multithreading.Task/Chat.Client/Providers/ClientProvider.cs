using Chat.Client.Events;
using Chat.Client.Providers;
using ServerParam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chat.Client.Providers
{
    class ClientProvider 
    {
        TcpClient _client;
        NetworkStream _stream;
        string _threadName;
        Thread _receiveThread;
        string[] _words;
        List<string> _receivedWords=new List<string>();
        bool _recievedmessage=false;
        readonly IWordProvider _wordProvider;

        public event EventHandler<NewMessageEventArgs> NewMessage = delegate { };

        public ClientProvider(IWordProvider wordProvider, string[] words)
        {
            _wordProvider = wordProvider;
            _words = words;
        }
        public void StartClient(object serverParam)
        {
            Params param = serverParam as Params;
            _threadName = string.Concat(Thread.CurrentThread.Name, ":");
            _client = new TcpClient();
            try
            {
                _client.Connect(param.Adress, param.Port);
                _stream = _client.GetStream();

                _receiveThread = new Thread(new ThreadStart(ReceiveMessage));
                _receiveThread.IsBackground = true;
                _receiveThread.Start();
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
            string message = null;
            while (true)
            {
                if (_recievedmessage)
                {
                    string lastWord = _receivedWords.Last();
                    if (lastWord.Length > 1)
                    {
                        message = _wordProvider.GetWord(lastWord.Substring(lastWord.Length - 1, 1), _receivedWords, _words);
                        if (message != null)
                        {
                            message=string.Concat(_threadName, message);
                            byte[] data = Encoding.Unicode.GetBytes(message);
                            _stream.Write(data, 0, data.Length);
                            _recievedmessage = false;
                        }
                        else
                        {
                            NewMessage(this, new NewMessageEventArgs(string.Concat(_threadName, "Sorry, i lose")));
                            message = string.Concat(_threadName, "exit1");
                            byte[] data = Encoding.Unicode.GetBytes(message);
                            _stream.Write(data, 0, data.Length);
                            _recievedmessage = false;
                            _receiveThread.Abort();
                        }
                    }
                }
                Thread.Sleep(5000);
            }
        }

        private void ReceiveMessage()
        {
            while (true)
            {
                try
                {
                    byte[] data = new byte[255];
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    do
                    {
                        bytes = _stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (_stream.DataAvailable);
                    string recivedMessge = builder.ToString();
                    if (recivedMessge != null)
                    {
                        if (recivedMessge.LastIndexOf(":") > 0)
                        {
                            _recievedmessage = true;
                            _receivedWords.Add(recivedMessge.Substring(recivedMessge.LastIndexOf(':') + 1));
                        }
                        NewMessage(this, new NewMessageEventArgs(recivedMessge));
                    }
                }
                catch (Exception ex)
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
            //Environment.Exit(0);
        }

    }
}
