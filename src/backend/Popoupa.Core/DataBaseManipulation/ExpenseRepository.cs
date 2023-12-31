using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Popoupa.Core.Expense;

namespace Popoupa.Core.DataBaseManipulation
{
    public class ExpenseRepository : IRepository<Expense>
    {
        private readonly string PopoupaDataBase = "User ID=nina;Password=nina100%;Host=192.168.0.2;Port=5432;Database=popoupa;Pooling=true;Min Pool Size=5;Max Pool Size=10;Connection Lifetime=60;";
        public Guid Add(Expense expense)
        {
            const string sqlOrder = @"
                INSERT 
                INTO 
                expenses (expense_date, expense_description, expense_amount, category_id) 
                VALUES 
                (@Date, @Description, @Amount, @Category)
                RETURNING expense_id";

            using (var connection = new SqlConnection(PopoupaDataBase))
            {
               var expenseId = connection.QuerySingle<Guid>(sqlOrder, new { Date = expense.Date, Description = expense.Description, Amount = expense.Amount, Category = expense.Category });
               return expenseId;
            }
        }

        public void Delete(Guid expense_Id)
        {
            string sqlOrder = @"
                DELETE 
                FROM 
                expenses 
                WHERE 
                expense_id = @id";
            using (var connection = new SqlConnection(PopoupaDataBase))
            {
                connection.Execute(sqlOrder, new { id = expense_Id});
            }
        }

        public Expense Get(Guid expense_Id)
        {
            const string sqlOrder = @"
                SELECT
                expense_description AS Description,
                expense_amount AS Amount,
                expense_date as Date,
                category_id as Category,
                FROM expenses 
                WHERE expense_id = @id";
            using (var connection = new SqlConnection(PopoupaDataBase))
            {
                var expense = connection.QueryFirstOrDefault<Expense>(sqlOrder, new {id = expense_Id});
                return expense;
            }
        }

        public IEnumerable<Expense> GetAll()
        {
            const string sqlOrder = @"
                SELECT
                expense_description AS Description,
                expense_amount AS Amount,
                expense_date as Date,
                category_id as Category,
                FROM
                expenses
            ";
            using (var connection = new SqlConnection(PopoupaDataBase))
            {
                var expenseList = connection.Query<Expense>(sqlOrder);
                return expenseList.ToList();
            };
        }

        public void Update(Expense expense) 
        {
           const string sqlOrder = @"
                UPDATE 
                expenses
                SET 
                expense_description = @Description,
                expense_amount = @Amount,
                expense_date = @Date,
                category_id = @Category, 
                WHERE 
                expense_id = @id";
            using (var connection = new SqlConnection(PopoupaDataBase))
            {
                connection.Execute(sqlOrder, new { Date = expense.Date, Description = expense.Description, Amount = expense.Amount, Category = (int)expense.Category, id = expense.Id });
            };
        }
            
        public void UpdateAllExpensesWhitSameCategory(Expense categorizedExpense, Guid import_ID)
        {
            const string sqlOrder = @"
                UPDATE expenses
                SET
                category_id = @ExpensesCategory
                FROM
                imports
                WHERE
                expenses.expense_id = imports.expense_id
                AND expenses.expense_description = @ExpensesDescription
                AND imports.import_id = @ImportID
                ";
            using (var connection = new SqlConnection(PopoupaDataBase))
            {
                connection.Execute(sqlOrder, new { ExpensesCategory = (int)categorizedExpense.Category, ExpensesDescription = categorizedExpense.Description, ImportID = import_ID});
            };
        }
        public List<Guid> AddMany(IReadOnlyList<Expense> expenses)
        {
            const string sqlOrder = @"
                INSERT 
                INTO 
                expenses (expense_date, expense_description, expense_amount, category_id) 
                VALUES 
                (@Date, @Description, @Amount, @Category)
                RETURNING expense_id";
            var expenses_Ids = new List<Guid>();
            using (var connection = new SqlConnection(PopoupaDataBase))
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        foreach (Expense expense in expenses)
                        {
                            var id = connection.QuerySingle<Guid>(sqlOrder, new
                            {
                                Date = expense.Date,
                                Description = expense.Description,
                                Amount = expense.Amount,
                                Category = expense.Category,
                            }, transaction);
                            expenses_Ids.Add(id);
                        }
                        transaction.Commit();
                    }
                    catch (InvalidOperationException)
                    {
                        transaction.Rollback();
                        throw new Exception("The operation has already been commited or rolledback");
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw new Exception("There were problems in the DataBase");
                    }
                    
                }
                return expenses_Ids;
            }
        }

        public List<int> GetAllCategoriesAssociatedWhitADescription(Expense expense)
        {
            const string sqlOrder = @"
            SELECT
            category_id
            FROM    
            expenses
            WHERE   
            expense_description = @ExpenseDescription
            ";
            using (var connection = new SqlConnection(PopoupaDataBase))
            {
                var categoryList = connection.Query<int>(sqlOrder, new
                {
                    ExpenseDescription = expense.Description
                });
                return categoryList.ToList();
            };
        }

        public List<Expense> GetAllExpensesFromTheSameBankStatement(Guid bankStatementId)
        {
            const string sqlOrder = @"
                SELECT
                expense_description AS Description,
                expense_amount AS Amount,
                expense_date as Date,
                category_id as Category,
                expenses.expense_id as Id,
                bank_statement_id
                FROM
                expenses
                INNER JOIN 
                imports
                ON
                imports.expense_id = expenses.expense_id
                WHERE
                bank_statement_id = @BankStatementID
            ";
            using (var connection = new SqlConnection(PopoupaDataBase))
            {
                var expensesList = connection.Query<Expense>(sqlOrder, new
                {
                    BankStatementID = bankStatementId
                });
                return expensesList.ToList();
            };
        }
    }
}
