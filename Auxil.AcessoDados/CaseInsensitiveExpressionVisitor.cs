using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using NHibernate.Linq.Visitors;
namespace Auxil.AcessoDados
{
    public class CaseInsensitiveExpressionVisitor : ExpressionVisitor
    {
        
        protected override  Expression VisitMemberAccess(MemberExpression node)
        {
            if (insideContains)
            {
                if (node.Type == typeof(String))
                {
                    var methodInfo = typeof(String).GetMethod("ToLower", new Type[] { });
                    var expression = Expression.Call(node, methodInfo);
                    return expression;
                }
            }
            return base.VisitMemberAccess(node);
        }

        private Boolean insideContains = false;

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Method.Name == "Contains")
            {
                if (insideContains) throw new NotSupportedException();
                insideContains = true;
                var result = base.VisitMethodCall(node);
                insideContains = false;
                return result;
            }
            return base.VisitMethodCall(node);
        }
    }
}
