using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Popoupa.Core
{
    public class Expense
    {
        public Expense(string description, DateTime date, decimal amount)
        {
            Description = description;
            Date = date;
            Amount = amount;
        }
        public string Description { get; }
        public DateTime Date { get; }
        public decimal Amount { get; }
    }
}
