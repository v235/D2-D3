using Chat.Server.Providers;
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
        static void Main(string[] args)
        {
            Params serverParam = new Params();
            ServerProvider server = new ServerProvider(serverParam.Adress, serverParam.Port, 10);
            server.Start();
            Console.ReadKey();
        }
    }
}