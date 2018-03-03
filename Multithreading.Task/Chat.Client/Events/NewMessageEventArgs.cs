using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Client.Events
{
    internal class NewMessageEventArgs: EventArgs
    {
        public String Message { get; private set; }

        public NewMessageEventArgs(string message)
        {
            Message = message;
        }



    }
}
