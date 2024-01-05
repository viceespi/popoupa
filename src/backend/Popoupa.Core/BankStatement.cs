using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Popoupa.Core
{
    public class BankStatement
    {

        public BankStatement(List<Expense> expenses, int referenceMonth, string bankName)
        {
            _expenses = expenses;
            MonthOfReference = referenceMonth;
            BankName = bankName;
            ImportDate = DateTime.UtcNow;
        }

        public BankStatement(List<Expense> expenses, int referenceMonth, string bankName, Guid bankStatementId)
        {
            _expenses = expenses;
            MonthOfReference = referenceMonth;
            BankName = bankName;
            ImportDate = DateTime.UtcNow;
            BankStatementId = bankStatementId;
        }
        private List<Expense> _expenses;

        public IReadOnlyList<Expense> Expenses => _expenses.AsReadOnly();

        public int MonthOfReference { get; }

        public string BankName { get; }

        public DateTime ImportDate { get; }
        public Guid BankStatementId { get; }
    }
}
