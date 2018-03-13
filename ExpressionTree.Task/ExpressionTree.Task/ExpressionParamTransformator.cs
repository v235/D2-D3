using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionTree.Task
{
    internal static class ChangeParametrToConstantExt
    {
        internal static Expression ChangeParametrToConstant(this Expression expression, Dictionary<string, int> paramToChange)
        {
            return new ExpressionParamTransformator(paramToChange).VisitAndConvert(expression,"");
        }
    }

    internal class ExpressionParamTransformator : ExpressionVisitor
    {
        Dictionary<string, int> _paramToChange;
        public ExpressionParamTransformator(Dictionary<string, int> paramToChange)
        {
            _paramToChange = paramToChange;
        }
        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            return Expression.Lambda<T>(Visit(node.Body), node.Parameters);
        }
        protected override Expression VisitParameter(ParameterExpression node)
        {
            foreach (var param in _paramToChange)
            {
                if (node.Name == param.Key)
                {
                    return Expression.Constant(param.Value);
                }
            }
            return base.VisitParameter(node); 
        }

    }
}
