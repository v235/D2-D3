using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ExpressionTree.Task
{  
    internal class ExpressionIncDecTransformator : ExpressionVisitor
    {
        protected override Expression VisitBinary(BinaryExpression node)
        {
            ParameterExpression param = null;
            ConstantExpression constant = null;

            if ((node.NodeType == ExpressionType.Add) || (node.NodeType == ExpressionType.Subtract))
            {
                param = GetParam(node);
                constant = GetConstant(node);
                if (param != null && constant != null && constant.Type == typeof(int) && (int)constant.Value == 1)
                {
                    if (node.NodeType == ExpressionType.Add)
                        return Expression.Increment(param);
                    return Expression.Decrement(param);
                }
            }
            return base.VisitBinary(node);
        }

        private ParameterExpression GetParam(BinaryExpression node)
        {
            if (node.Left.NodeType == ExpressionType.Parameter)
            {
                return (ParameterExpression)node.Left;
            }
            return null;
        }
        private ConstantExpression GetConstant(BinaryExpression node)
        {
            if (node.Right.NodeType == ExpressionType.Constant)
            {
               return(ConstantExpression)node.Right;
            }
            return null;
        }
    }
}
