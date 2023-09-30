using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Popoupa.Core
{
    public class InvalidBankStatementException : Exception
    {
        public InvalidBankStatementException(int line, string message)
        {
            Line = line;
            Message = message;
        }

        public int Line { get; }
        public string Message { get; }
    }
}
