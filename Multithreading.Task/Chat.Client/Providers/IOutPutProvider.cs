using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Client.Providers
{
    internal interface IOutPutProvider
    {
        void PrintMessage(string message);
    }
}
