using Microsoft.AspNetCore.Mvc;
using Popoupa.API.APIClasses;
using Popoupa.Core;
using Popoupa.Core.CategorizerStuff;
using Popoupa.Core.DataBaseManipulation;
using Popoupa.Core.Parsers;
using System.Text;
using System.Transactions;
using static Popoupa.Core.Expense;

namespace Popoupa.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder();
        var app = builder.Build();

        app.MapPost("/bankstatements", ([FromBody] BankStatementMessageObject bankStatementObject) =>
        {
            switch (bankStatementObject.BSFormatType)
            {
                case "nubank":
                    {
                        try
                        {
                            var expenseRepository = new ExpenseRepository();
                            var bankStatementRepository = new BankStatementRepository();
                            var parser = new NubankCSVParser();
                            var content = Encoding.UTF8.GetBytes(bankStatementObject.BSContent);
                            var bankStatement = parser.Parse(content, Encoding.UTF8, "nubank");
                            try
                            {
                                var expensesIdList = expenseRepository.AddMany(bankStatement.Expenses);
                                bankStatementRepository.AddToImportTable(bankStatement, expensesIdList);
                                return Results.StatusCode(201);
                            }
                            catch (Exception)
                            {
                                return Results.StatusCode(500) ;
                            }
                        }
                        catch(InvalidBankStatementException)
                        {
                            return Results.StatusCode(502);
                        }
                    }
                case "sicoob": return Results.StatusCode(422);
                case "bradesco": return Results.StatusCode(422);
                case "itau": return Results.StatusCode(422);

                default: return Results.StatusCode(400);

            }
        }
        );

        app.MapGet("/categories", () =>
        {
            var enumList = (CategoryState[])Enum.GetValues(typeof(CategoryState));
            var categoriesList = Expense.GetListOfStringCategories(enumList);
            return Results.Ok(categoriesList);
        }
        );

        app.MapPut("/expenses/categorize/{id}", ([FromRoute] int id, [FromBody] Expense expense) =>
        {
            var isValid = Expense.Validate(expense);
            if (isValid)
            {
                foreach (Expense listExpense in DataBase.ExpensesList)
                {
                    if (listExpense.Id == expense.Id)
                    {
                        DataBase.Replace(listExpense, expense);
                        return Results.StatusCode(201);
                    }
                }
            }
            return Results.StatusCode(400);
        });

        app.MapGet("/expenses/categories/{id}", ([FromRoute] Guid id) =>
        {
            foreach (Expense expense in DataBase.ExpensesList)
            {
                if (expense.Id == id)
                {
                    var categorizer = new ExpenseCategorizer();
                    var categoriesList = categorizer.GetListOfCategories(DataBase.ExpensesList, expense);
                    var stringCategoriesLists = Expense.GetListOfStringCategories(categoriesList);
                    return Results.Ok(categoriesList);
                }
            }
            return Results.StatusCode(400);
        }
        );


        app.MapPut("/expenses/categorize", ([FromBody] List<Expense> chosenExpenses, [FromQuery] int intCategory) =>
        {
            if (chosenExpenses.Count < 2) return Results.StatusCode(400);
            var expenseCategorizer = new ExpenseCategorizer();
            var validExpense = expenseCategorizer.CheckIfExpenseHasMultipleCategoriesAssociated(DataBase.ExpensesList, chosenExpenses[0]);
            if (!validExpense) return Results.StatusCode(307);
            foreach (Expense oldExpense in chosenExpenses)
            {
                var newExpense = new Expense(oldExpense.Description, oldExpense.Date, oldExpense.Amount, (CategoryState)intCategory);
                DataBase.Replace(oldExpense, newExpense);
            }
            return Results.StatusCode(200);
        });












        app.Run();

    }
}