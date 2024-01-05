using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
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
            PhysicalHealth,
            HappyHours,
            UnusualExpenses,
        }

        public Expense(string description, DateTime date, decimal amount, CategoryState category)
        {
            Date = date;
            Description = description;
            Amount = amount;
            Category = category;   
        }
        public string Description { get; }
        public DateTime Date { get; }
        public decimal Amount { get; }
        public CategoryState Category { get; set; }
        public Guid Id { get; }

        public static bool Validate(Expense expense)
        {
            if (string.IsNullOrEmpty(expense.Description) || string.IsNullOrWhiteSpace(expense.Description)) return false;
            if (expense.Date > DateTime.UtcNow) return false;
            if (expense.Amount > 0) return false;
            return true;
        }
    }
}
