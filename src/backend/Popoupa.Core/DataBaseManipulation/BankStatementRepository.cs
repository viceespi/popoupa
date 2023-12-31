using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Popoupa.Core.DataBaseManipulation
{
    public class BankStatementRepository : IRepository<BankStatement>
    {
        private readonly string PopoupaDataBase = "User ID=nina;Password=nina100%;Host=192.168.0.2;Port=5432;Database=popoupa;Pooling=true;Min Pool Size=5;Max Pool Size=10;Connection Lifetime=60;";
        public Guid Add(BankStatement bankStatement)
        {
            const string sqlOrder = @"
                INSERT 
                INTO 
                bank_statements (import_date, month_of_reference, bank_name)
                VALUES 
                (@ImportDate, @MonthOfReference, @BankName)
                RETURNING 
                bank_statement_id";

            using (var connection = new SqlConnection(PopoupaDataBase))
            {
                var bankStatementId = connection.QuerySingle<Guid>(sqlOrder, new { ImportDate = bankStatement.ImportDate, MonthOfReference = bankStatement.MonthOfReference, BankName = bankStatement.BankName });
                return bankStatementId;
            }
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public BankStatement Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BankStatement> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Update(BankStatement entity)
        {
            throw new NotImplementedException();
        }

        public void AddToImportTable(BankStatement bankStatement, List<Guid> expensesIdList) 
        {
            const string sqlOrder = @"
            INSERT
            INTO
            imports (bank_statement_id, expense_id)
            VALUES (@BankStatementId, @ExpenseId)";
            using (var connection = new SqlConnection(PopoupaDataBase))
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        foreach (Guid id in expensesIdList)
                        {
                            connection.Execute(sqlOrder, new { BankStatementId = bankStatement.BankStatementId, ExpenseId = id });
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
            }
        }
    }
}
