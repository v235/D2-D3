using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionTree.Task
{
    class Program
    {
        static void Main(string[] args)
        {
            Expression<Func<int, int, int, int>> source_exp = (a, b, c) => c*(a + 1)*(b-2)*(a-1);
            ExpressionIncDecTransformator et = new ExpressionIncDecTransformator();
            Console.WriteLine(et.VisitAndConvert(source_exp, ""));
            Dictionary<string, int> param = new Dictionary<string, int>();
            param.Add("a", 2);
            param.Add("b", 5);
            param.Add("c", 10);
            Console.WriteLine(source_exp.ChangeParametrToConstant(param));
            Console.ReadKey();
        }
    }
}
