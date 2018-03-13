using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionMapper.Task
{
    public static class MappingGenerator
    {

        public static Mapper<TSource, TDestination> Generate<TSource, TDestination>()
        {
            var sourceProp = GetProperties(typeof(TSource));
            var destinationProp = GetProperties(typeof(TDestination));
            var paramExp = Expression.Parameter(typeof(TSource));
            var sourceExp = Expression.Convert(paramExp, typeof(TSource));
            var bindings = new List<MemberBinding>();
            foreach (var dest in destinationProp)
            {
                if (sourceProp.TryGetValue(dest.Key, out var sourceProperty))
                {
                    bindings.Add(Expression.Bind(dest.Value, Expression.Property(sourceExp, sourceProperty)));
                }
            }
            var resultExpr = Expression.MemberInit(Expression.New(typeof(TDestination)), bindings);
            var mapFunction = Expression.Lambda<Func<TSource, TDestination>>(resultExpr, paramExp);
            return new Mapper<TSource, TDestination>(mapFunction.Compile());
        }

        private static Dictionary<string, PropertyInfo> GetProperties(Type objType)
        {
            Dictionary<string, PropertyInfo> props = new Dictionary<string, PropertyInfo>();
            var propertyInfos = objType.GetProperties(
                BindingFlags.Instance | BindingFlags.Public 
                | BindingFlags.SetProperty);
            foreach (var propertyInfo in propertyInfos)
            {
                props.Add(propertyInfo.Name, propertyInfo);
            }
            return props;
        }
    }

}
