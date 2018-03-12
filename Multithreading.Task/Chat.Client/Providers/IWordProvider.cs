using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Client.Providers
{
    internal interface IWordProvider
    {
        string GetWord(string letter, List<string> expetWords, IEnumerable<string> words);
        string GetRandomWord();
    }
}
