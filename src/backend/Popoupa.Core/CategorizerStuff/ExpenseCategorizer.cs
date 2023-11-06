using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Popoupa.Core.Expense;
using Popoupa.Core.CategorizerStuff.PopoHashTable;

namespace Popoupa.Core.CategorizerStuff
{
    public class ExpenseCategorizer
    {
        
        public void ManualCategorizer(Expense expense, CategoryState userExpenseCategory)
        {
            expense.Category = userExpenseCategory;
        }

        public void AutomaticCategorization(List<Expense> expenseList, Expense chosenExpense)
        {
            PopoTable<Expense> popoTable = new(expenseList, expense => expense);
            List<PopoPair<Expense>> bucket = popoTable.Get(chosenExpense.Description);
            var chosenExpenseInBucket = false;
            try
            {
                foreach (PopoPair<Expense> popoPair in bucket)
                {
                    if (popoPair.KeyDescription == chosenExpense.Description)
                    {
                        chosenExpenseInBucket = true;
                        foreach (Expense element in popoPair.KeyValue) element.Category = chosenExpense.Category;
                    }
                }
                if (!chosenExpenseInBucket) throw new Exception("An expense that doesn't exists was chosen.");
            }
            catch (ArgumentNullException) 
            {
                throw new Exception("An expense that doesn't exists was chosen.");
            }
        }
        public List<CategoryState> GetListOfCategories(List<Expense> expensesArchive, Expense chosenExpense)
        {
            PopoTable<CategoryState> popoTable = new(expensesArchive, expense => expense.Category);
            List<PopoPair<CategoryState>> bucket = popoTable.Get(chosenExpense.Description) ?? new List<PopoPair<CategoryState>>();
            foreach (PopoPair<CategoryState> popoPair in bucket)
            {
                if (popoPair.KeyDescription == chosenExpense.Description) return popoPair.KeyValue;
            }
            var newPopopair = new PopoPair<CategoryState>(chosenExpense.Description, chosenExpense.Category);
            bucket.Add(newPopopair);
            return newPopopair.KeyValue;
        }
        
        public bool CheckIfExpenseHasMultipleCategoriesAssociated(List<Expense> expensesArchive, Expense chosenExpense)
        {
            var categoryList = GetListOfCategories(expensesArchive, chosenExpense);
            if (categoryList.Count < 2) return false;
            var firstCategory = categoryList[0];
            var hasDifferentCategory = false;
            foreach (CategoryState category in categoryList)
            {
                if (category != firstCategory) hasDifferentCategory = true;
            }
            return hasDifferentCategory;
        }

    }


}

 