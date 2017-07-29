using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using For.ExpressionCompareGenerateEngine.Model;

namespace For.ExpressionCompareGenerateEngine
{
    public class FormulaProcess
    {
        /// <summary>
        /// 分離公式，取得要處理的佇列
        /// </summary>
        /// <param name="formula"></param>
        /// <returns></returns>
        public string[] SeparateFormula(string formula)
        {
            var charAry = formula.Replace(" ", "").ToCharArray(); //文字轉陣列
            var position = 0; //目前位置
            var length = charAry.Count(); //文字陣列總長度
            var sbOperand = new StringBuilder(); //文字處理運算元暫存
            var sbOperator = new StringBuilder(); //文字處理運算子暫存
            var stack = new Stack<Priority>(); //暫存運算子(比較符號)的堆疊
            var posfix = new string[100]; //後續佇列
            var posPosition = 0;

            while (position < length)
            {
                var @char = charAry[position];

                if (Const.Operator.Contains(@char.ToString()))
                {
                    //如果是運算子，收集運算子(比較符號)
                    sbOperator.Append(@char.ToString());
                    if (sbOperand.Length != 0)
                    {
                        //將暫存運算子放入後續佇列
                        InsertToPosfix(sbOperand.ToString(), ref posfix, ref posPosition);
                        sbOperand.Clear();
                    }
                    //無從判斷連在一起的符號，先進行處理
                    if (@char.ToString() != ">" && @char.ToString() != "<")
                    {
                        ProcessOperator(Const.OperatorPriorityMap[sbOperator.ToString()], stack, ref posfix, ref posPosition);
                        sbOperator.Clear();
                    }

                }
                else
                {
                    //如果是運算元收集運算元
                    sbOperand.Append(@char.ToString());
                    if (sbOperator.Length != 0)
                    {
                        //處理運算元(比較符號)
                        ProcessOperator(Const.OperatorPriorityMap[sbOperator.ToString()], stack, ref posfix, ref posPosition);
                        sbOperator.Clear();
                    }
                }
                position += 1;
            }
            //將剩下的運算元放入後續佇列
            if (sbOperand.Length != 0)
            {
                InsertToPosfix(sbOperand.ToString(), ref posfix, ref posPosition);
            }
            while (stack.Count != 0)
            {
                //剩下的運算子放入後續佇列
                InsertToPosfix(stack.Pop().OP, ref posfix, ref posPosition);
            }

            return posfix;
        }

        /// <summary>
        /// 處理運算符號堆疊
        /// </summary>
        /// <param name="operator"></param>
        /// <param name="stack"></param>
        /// <param name="posfix"></param>
        /// <param name="position"></param>
        private static void ProcessOperator(Priority @operator, Stack<Priority> stack, ref string[] posfix, ref int position)
        {
            while (true)
            {
                if (@operator.OP == ")")
                {
                    var op = stack.Pop().OP;
                    if (op == "(")
                    {
                        break;
                    }
                    InsertToPosfix(op, ref posfix, ref position);
                }
                else if (stack.Count == 0 || @operator.ICP > stack.Peek().ISP)
                {
                    stack.Push(@operator);
                    break;
                }
                else if (@operator.ICP <= stack.Peek().ISP)
                {
                    var op = stack.Pop().OP;
                    InsertToPosfix(op, ref posfix, ref position);
                }
                else
                {
                    break;
                }
            }
        }

        /// <summary>
        /// 收入後序佇列中
        /// </summary>
        /// <param name="op"></param>
        /// <param name="posfix"></param>
        /// <param name="position"></param>
        private static void InsertToPosfix(string op, ref string[] posfix, ref int position)
        {
            while (posfix.Length <= position)
            {
                Array.Resize<string>(ref posfix, posfix.Length + 10);
            }
            posfix[position] = op;
            position++;
        }


        //        /// <summary>
        //        /// 分離公式，取得要處理的各項物件名稱--(簡單處理，僅適用單一比較)
        //        /// </summary>
        //        /// <param name="formula"></param>
        //        /// <returns></returns>
        //        public static Queue<string> SeparateFormula(string formula)
        //        {
        //            var charAry = formula.Replace(" ", "").ToCharArray();
        //            var position = 0;
        //            var length = charAry.Count();
        //            var sbOperand = new StringBuilder();
        //            var sbOperator = new StringBuilder();
        //            var que = new Queue<string>();
        //
        //            while (position < length)
        //            {
        //                var @char = charAry[position];
        //
        //                if (_operator.Contains(@char.ToString()))
        //                {
        //                    //如果是運算元，收集運算元
        //                    sbOperator.Append(@char.ToString());
        //                    if (sbOperand.Length != 0)
        //                    {
        //                        que.Enqueue(sbOperand.ToString());
        //                        sbOperand.Clear();
        //                    }
        //                }
        //                else
        //                {
        //                    //不是運算元，收隻運算子
        //                    sbOperand.Append(@char.ToString());
        //                    if (sbOperator.Length != 0)
        //                    {
        //                        que.Enqueue(sbOperator.ToString());
        //                        sbOperator.Clear();
        //                    }
        //                }
        //                position += 1;
        //            }
        //            //將剩下的值推入que中
        //            if (sbOperand.Length != 0)
        //            {
        //                que.Enqueue(sbOperand.ToString());
        //            }
        //            if (sbOperator.Length != 0)
        //            {
        //                que.Enqueue(sbOperator.ToString());
        //            }
        //            return que;
        //        }
    }
}
