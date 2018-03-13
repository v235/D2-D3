using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionMapper.Task
{
    public class Mapper<TSource, TDestination>
    {
        Func<TSource, TDestination> _mapFunction;
        internal Mapper(Func<TSource, TDestination> mapFunction)
        {
            _mapFunction = mapFunction;
        }
        public TDestination Map(TSource source)
        {
            return _mapFunction(source);
        }
    }
}
