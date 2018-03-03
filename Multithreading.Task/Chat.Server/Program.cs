using ServerParam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chat.Server
{
    class Program
    {
        static ServerObject _server; 
        static Thread _listenThread;
        static void Main(string[] args)
        {
            try
            {
                Params serverParam = new Params();
                _server = new ServerObject();
                _listenThread = new Thread(new ParameterizedThreadStart(_server.StartServer));
                _listenThread.Start(serverParam);
            }
            catch (Exception ex)
            {
                _server.Disconnect();
                Console.WriteLine(ex.Message);
            }

            Console.ReadKey();
        }
    }
}