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
    class Program
    {
        static string _serverAdress;
        static int _serverPort;
        static Mutex mutexObj = new Mutex();

        static void Main(string[] args)
        {
            Thread.Sleep(5000);
            Params serverParam = new Params();
            _serverAdress = serverParam.Adress;
            _serverPort = serverParam.Port;
            for (int i=1;i<4; i++)
            {
                Thread newClient = new Thread(new ParameterizedThreadStart(Start));
                newClient.Name = string.Concat("client", i.ToString());
                newClient.Start();
            }

            Console.ReadKey();
        }

        static void Start(object clientProvider)
        {
            mutexObj.WaitOne();
            ClientProvider client = new ClientProvider();
            client.StartClient(_serverAdress, _serverPort);
            mutexObj.ReleaseMutex();
        }
    }
}