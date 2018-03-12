using Chat.Client.Providers;
using ServerParam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chat.Client.Managers
{
    class ClientManager
    {
        Params _serverParam;
        ClientProvider _clientProvider;
        WordProvider _wordProvider;
        readonly IOutPutProvider _outPutProvider;

        public ClientManager(IOutPutProvider outPutProvider)
        {
            _outPutProvider = outPutProvider;
        }

        public void Start()
        {
            _serverParam = new Params();
            _wordProvider = new WordProvider("words.txt");
            _wordProvider.GetRandomDictionary(3);
            for (int i = 0; i < 5; i++)
            {
                startClient(i);
                Thread.Sleep(5000);
            }
            Thread.Sleep(2000);
            var referee = new ClientReferee(_wordProvider);
            referee.StartGame(_serverParam.Adress, _serverParam.Port);
        }

        private void startClient(int i)
        {
            _clientProvider = new ClientProvider(_wordProvider, _wordProvider.GetRandomDictionary(3));
            _clientProvider.NewMessage += _clientProvider_NewMessage;
            Thread newThread = new Thread(new ParameterizedThreadStart(_clientProvider.StartClient));
            newThread.Name = Guid.NewGuid().ToString();
            newThread.Start(_serverParam);
        }
        private void _clientProvider_NewMessage(object sender, Events.NewMessageEventArgs e)
        {
            _outPutProvider.PrintMessage(e.Message);
        }
    }
}
