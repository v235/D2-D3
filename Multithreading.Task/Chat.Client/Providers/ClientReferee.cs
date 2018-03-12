using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Client.Providers
{
    class ClientReferee
    {
        TcpClient _client;
        NetworkStream _stream;
        readonly IWordProvider _wordProvider;

        public ClientReferee(IWordProvider wordProvider)
        {
            _wordProvider = wordProvider;

        }

        public void StartGame(string adress, int port)
        {
            _client = new TcpClient();
            try
            {
                _client.Connect(adress, port);
                _stream = _client.GetStream();
                string message = string.Concat("referee:", _wordProvider.GetRandomWord());
                byte[] data = Encoding.Unicode.GetBytes(message);
                _stream.Write(data, 0, data.Length);
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

        private void Disconnect()
        {
            if (_stream != null)
                _stream.Close();
            if (_client != null)
                _client.Close();
        }
    }
}
