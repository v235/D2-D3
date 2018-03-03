using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerParam
{
    public class Params
    {
        public string Adress { get; private set; }
        public int Port { get; private set; }

        public Params()
        {
            Adress = "127.0.0.1";
            Port = 9005;
        }
    }
}
