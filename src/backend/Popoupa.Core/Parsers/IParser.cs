using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Popoupa.Core.Parsers
{
    public interface IParser
    {
        public BankStatement Parse(byte[] fileContents, Encoding fileEncoding, string bankName);
    }
}
