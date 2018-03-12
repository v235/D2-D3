using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Chat.Client.Providers
{
    internal class WordProvider : IWordProvider
    {
        string _res;
        IEnumerable<string> _words;
        List<string> _usedWords=new List<string>();

        public WordProvider(string path)
        {
            _words = File.ReadAllLines(path);
        }

        public string[] GetRandomDictionary(int parts)
        {
            var collectionOfArray = _words.Select((item, index) => new { index, item }).GroupBy(x => x.index % parts)
                .Select(x => x.Select(y => y.item)).ToArray();
            Random rnd = new Random();
            return collectionOfArray[rnd.Next(0, parts)].ToArray();
        }

        public string GetWord(string letter, List<string> expetWords, IEnumerable<string> words)
        {
            try
                {
                lock (_usedWords)
                {
                    _usedWords.AddRange(expetWords);
                    _res = words.Except(_usedWords.Distinct(), StringComparer.OrdinalIgnoreCase)
                        .FirstOrDefault(w => w.StartsWith(letter, StringComparison.OrdinalIgnoreCase));
                    if (_res != null)
                        _usedWords.Add(_res);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
            return _res;
        }
        public string GetRandomWord()
        {
            Random rnd = new Random();
            return _words.ElementAt(rnd.Next(0, _words.Count()));
        }
    }
}
