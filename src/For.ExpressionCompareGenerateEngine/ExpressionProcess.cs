using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using For.ExpressionCompareGenerateEngine.Model;
using System.Reflection;
namespace For.ExpressionCompareGenerateEngine
{
    public class ExpressionProcess
    {
        /// <summary>
        /// 產生委派公式
        /// </summary>
        /// <param name="posfix"></param>
        /// <returns></returns>
        public Func<T, TResult> GenerateFunc<T, TResult>(string[] posfix)
        {
            var stack = new Stack<Expression>();
            var position = 0;
            //create instance
            var instanceExpr = Expression.Parameter(typeof(T), "p");
            while (!string.IsNullOrEmpty(posfix[position]))
            {
                var op = posfix[position];
                if (!Const.OperatorPriorityMap.ContainsKey(op)) //是運算元進行堆疊處理
                {
                    stack.Push(stack.Count > 0
                        ? ProcessOperatorExpr<T>(stack.Peek().Type, instanceExpr, op) //有堆疊，應該是屬性，取得對應的型別建立expr
                        : ProcessOperatorExpr<T>(typeof(double), instanceExpr, op));//沒有堆畾，應該是進行四則運算，使用double進行處理
                }
                else //是運算子進行公式產生
                {
                    var rightExpr = stack.Pop();
                    var leftExpr = stack.Pop();
                    stack = CreateExpression(stack, leftExpr, rightExpr, op);
                }
                position += 1;
            }
            //編譯
            Expression<Func<T, TResult>> lambda = Expression.Lambda<Func<T, TResult>>(stack.Pop(), instanceExpr);
            return lambda.Compile();
        }

        /// <summary>
        /// 建立比較處理的Expression
        /// </summary>
        /// <param name="stack"></param>
        /// <param name="leftExpr"></param>
        /// <param name="rightExpr"></param>
        /// <param name="op"></param>
        /// <returns></returns>
        private static Stack<Expression> CreateExpression(Stack<Expression> stack, Expression leftExpr, Expression rightExpr, string op)
        {
            switch (op)
            {
                case "+":
                    stack.Push(Expression.Add(leftExpr, rightExpr));
                    break;
                case "-":
                    stack.Push(Expression.Subtract(leftExpr, rightExpr));
                    break;
                case "*":
                    stack.Push(Expression.Multiply(leftExpr, rightExpr));
                    break;
                case "/":
                    stack.Push(Expression.Divide(leftExpr, rightExpr));
                    break;
                case ">":
                    stack.Push(Expression.GreaterThan(leftExpr, rightExpr));
                    break;
                case "<":
                    stack.Push(Expression.LessThan(leftExpr, rightExpr));
                    break;
                case ">=":
                    stack.Push(Expression.GreaterThanOrEqual(leftExpr, rightExpr));
                    break;
                case "<=":
                    stack.Push(Expression.LessThanOrEqual(leftExpr, rightExpr));
                    break;
                case "=":
                    stack.Push(Expression.Equal(leftExpr, rightExpr));
                    break;
                case "&":
                    stack.Push(Expression.And(leftExpr, rightExpr));
                    break;
                case "|":
                    stack.Push(Expression.Or(leftExpr, rightExpr));
                    break;
            }
            return stack;
        }

        /// <summary>
        /// 建立運算元的Expression
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <param name="instanceExpr"></param>
        /// <param name="op"></param>
        /// <returns></returns>
        private static Expression ProcessOperatorExpr<T>(Type type, ParameterExpression instanceExpr, string op)
        {
            var expr = op.StartsWith(".") ? 
                Expression.Property(instanceExpr, typeof(T).GetTypeInfo().GetProperty(op.Replace(".", ""))) : 
                CreateValueExpression(type, op);
            return expr;
        }

        /// <summary>
        /// 建立常數運算元的Expression
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static Expression CreateValueExpression(Type type, string value)
        {
            Expression result;
            switch (type.Name.ToLower())
            {
                case "int":
                case "int32":
                case "int64":
                    result = Expression.Constant(int.Parse(value));
                    break;
                case "string":
                    result = Expression.Constant(value);
                    break;
                case "bool":
                case "boolean":
                    result = Expression.Constant(bool.Parse(value));
                    break;
                default:
                    throw new Exception();
            }
            return result;
        }

    }
}
