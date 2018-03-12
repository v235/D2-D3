using Chat.Client.Managers;
using Chat.Client.Providers;
using System;
using System.Threading;

namespace Chat.Client
{
    class Program
    {


        static void Main(string[] args)
        {
            Thread.Sleep(5000);
            ClientManager cm = new ClientManager(new OutPutProvider());
            cm.Start();

            Console.ReadKey();
        }
    }
}