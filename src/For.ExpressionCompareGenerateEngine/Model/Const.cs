using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace For.ExpressionCompareGenerateEngine.Model
{
    internal class Const
    {
        /// <summary>
        /// 運算子parsing使用
        /// </summary>
        internal static readonly string[] Operator = { ">", "<", "=", "(", ")", "|", "&", "+", "-", "*", "/" };
        /// <summary>
        /// 運算子優先層級
        /// </summary>
        internal static readonly Dictionary<string, Priority> OperatorPriorityMap = new Dictionary<string, Priority>()
        {
            { "(", new Priority(){ OP = "(", ISP = 0,  ICP=20 } },
            { ")", new Priority(){ OP = ")", ISP = 19, ICP=19 } },
            { ">", new Priority(){ OP = ">", ISP = 13, ICP=13 } },
            { "<", new Priority(){ OP = "<", ISP = 13, ICP=13 } },
            { ">=", new Priority(){ OP = ">=", ISP = 13, ICP=13 } },
            { "<=", new Priority(){ OP = "<=", ISP = 13, ICP=13 } },
            { "=", new Priority(){ OP = "=", ISP = 13, ICP=13 } },
            { "|", new Priority(){ OP = "|", ISP = 10, ICP=10 } },
            { "&", new Priority(){ OP = "&", ISP = 10, ICP=10 } },
            { "+", new Priority(){ OP = "+", ISP = 14, ICP=14 } },
            { "-", new Priority(){ OP = "-", ISP = 14, ICP=14 } },
            { "*", new Priority(){ OP = "*", ISP = 15, ICP=15 } },
            { "/", new Priority(){ OP = "/", ISP = 15, ICP=15 } },
        };
    }
}
