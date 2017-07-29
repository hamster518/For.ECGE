using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace For.ExpressionCompareGenerateEngine.Model
{
    internal class Priority
    {
        internal string OP { get; set; }
        internal int ISP { get; set; } //In-Stack
        internal int ICP { get; set; } // In-Coming
    }
}
