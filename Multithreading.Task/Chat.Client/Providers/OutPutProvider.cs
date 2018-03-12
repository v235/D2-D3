using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Client.Providers
{
    class OutPutProvider : IOutPutProvider
    {
        public void PrintMessage(string message)
        {
            Console.WriteLine("Says:{0}", message);
        }
    }
}
