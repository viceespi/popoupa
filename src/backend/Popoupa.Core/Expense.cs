using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Popoupa.Core
{
    
    public record Expense
    {
        public enum CategoryState
        {
            Uncategorized,
            Housing,
            Pets,
            MarketEssentials,
            DrugstoreExpenses,
            MentalHealth,
            PyshicalHealth,
            HappyHours,
            UnusualExpenses,
        }

        public Expense(string description, DateTime date, decimal amount, CategoryState category)
        {
            Description = description;
            Date = date;
            Amount = amount;
            Category = category;
        }
        public string Description { get; }
        public DateTime Date { get; }
        public decimal Amount { get; }
        public CategoryState Category { get; set; }
    }
}
