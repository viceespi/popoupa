using Popoupa.Core;

namespace Popoupa.API.APIClasses
{
    public static class DataBase
    {

        private static List<Expense> _expensesList = new List<Expense>();


        public static List<Expense> ExpensesList => _expensesList;

        private static List<User> _userList = new List<User>();

        public static List<User> UserList => _userList;
        public static void AddMany(List<Expense> expenses)
        {
            foreach (Expense expense in expenses)
            {
                Add(expense);
            }
        }
        public static void Add(Expense expense)
        {
            ExpensesList.Add(expense);
        }

        public static void Replace(Expense oldExpense, Expense newExpense)
        {
            DataBase.ExpensesList.Remove(oldExpense);
            DataBase.ExpensesList.Add(newExpense);
        }
        
    }
}
