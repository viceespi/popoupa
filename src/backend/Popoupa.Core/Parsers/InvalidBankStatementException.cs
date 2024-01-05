using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Popoupa.Core.Parsers
{
    public class InvalidBankStatementException : Exception
    {
        public InvalidBankStatementException(int CsvLineReference, string message) : base(message)
        {
            Line = CsvLineReference;
            Message = message;
        }

        public int Line { get; }
        public string Message { get; }
    }
}

