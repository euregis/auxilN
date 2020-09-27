using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;

namespace Auxil.AcessoDados
{
    public class ExpressaoDinamica<T> where T : Auxil.Entidades.Base.BaseID
    {
        private ParameterExpression pExpression = Expression.Parameter(typeof(T), "x");

        private Expression expressao = null;

        public void AddExpressao(Expression e1)
        {
            if (expressao == null)
                expressao = e1;
            else
                expressao =     Expression.AndAlso(expressao, e1);
        }

        public Expression<Func<T, bool>> Montar()
        {
            if (expressao == null) return null;
            return Expression.Lambda<Func<T, bool>>(expressao, new ParameterExpression[] { pExpression });
            //Expression<Func<T, bool>> expr =Expression.Lambda<Func<T, bool>>(expressao, new ParameterExpression[] { pExpression });

            //return (Expression<Func<T, bool>>)new CaseInsensitiveExpressionVisitor().Visit(expr);
        }

        public Expression Propriedade(PropertyInfo propriedade)
        {
            return Expression.Property(pExpression, propriedade);//typeof(T).GetProperty(propriedade));
        }

        public Expression Constante(object obj, Type tipo)
        {
            return Expression.Constant(obj, tipo);
        }
        public Expression MaiorIgual(Expression prop, Expression obj)
        {
            return Expression.GreaterThanOrEqual(prop, obj);
        }
        public Expression MenorIgual(Expression prop, Expression obj)
        {
            return Expression.LessThanOrEqual(prop, obj);
        }
        public Expression Contem(string valor, Expression propriedade)
        {
            MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            var someValue = Expression.Constant(valor, typeof(string));
            var containsMethodExp = Expression.Call(propriedade, method, someValue);
            return containsMethodExp;
        }


    }
}
