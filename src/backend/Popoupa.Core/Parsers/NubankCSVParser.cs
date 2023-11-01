using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Popoupa.Core.Parsers
{
    public class NubankCSVParser : IParser
    {
        private static readonly CultureInfo Brazilian = new CultureInfo("pt-BR");
        public  List<Expense> Parse(byte[] fileContents, Encoding fileEncoding)
        {
            var expensesArray = new List<Expense>();
            var fileString = fileEncoding.GetString(fileContents);
            CheckIfFileHasHeader(fileString);
            var csvLines = fileString.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            for (int lineIndex = 1; lineIndex < csvLines.Length; lineIndex++)
            {
                var csvLineReference = lineIndex + 1;
                var individualLine = GetIndividualCSVLines(csvLines, lineIndex, csvLineReference);
                var lineContents = individualLine.Split(',', '\r', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                CheckIfLineIsValid(lineContents, csvLineReference);
                var expenseAmount = GetExpenseAmount(lineContents, csvLineReference);
                if (expenseAmount >= 0) continue;
                var expenseDate = GetExpenseDate(lineContents, csvLineReference);
                var expenseDescription = GetExpenseDescription(lineContents, csvLineReference);
                var expense = new Expense(expenseDescription, expenseDate, expenseAmount, Expense.CategoryState.Uncategorized);
                expensesArray.Add(expense);
            }
            return expensesArray;
        }

        private static void CheckIfFileHasHeader(string filestring)
        {
            const string header = "Data,Valor,Identificador,Descrição\r\n";
            try
            {
                var toValidateHeader = filestring.Substring(0, 36);
                var hasHeader = header == toValidateHeader;
                if (!hasHeader)
                {
                    throw new InvalidBankStatementException(1, "Invalid file, it has no header");
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new InvalidBankStatementException(1, "Invalid length indicated in the substring method. To validate string length < indicated length");
            }
        }

        private static string GetIndividualCSVLines(string[] csvLines, int lineIndex, int CsvLineReference)
        {
            try
            {
                var lineContents = csvLines[lineIndex];
                return lineContents;
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new InvalidBankStatementException(CsvLineReference, "Index out of bounds");
            }
        }

        private static void CheckIfLineIsValid(string[] lineContents, int CsvLineReference)
        {
            if (lineContents.Length != 4)
            {
                throw new InvalidBankStatementException(CsvLineReference, "The line is invalid. Only lines whit 4 informations are accepted");
            }
        }

        private static DateTime GetExpenseDate(string[] lineContents, int CsvLineReference)
        {
            try
            {
                var dateAsString = lineContents[0];
                try
                {
                    var date = DateTime.ParseExact(dateAsString, "dd/MM/yyyy", Brazilian);
                    return date;
                }
                catch (ArgumentNullException)
                {
                    throw new InvalidBankStatementException(CsvLineReference, "The date or the format of the date have the null value");
                }
                catch (FormatException)
                {
                    throw new InvalidBankStatementException(CsvLineReference, "Invalid format of the date");
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new InvalidBankStatementException(CsvLineReference, "Index out of bounds");
            }
        }

        private static decimal GetExpenseAmount(string[] lineContents, int CsvLineReference)
        {
            try
            {
                var amountAsString = lineContents[1];
                try
                {
                    var expenseAmount = decimal.Parse(amountAsString);
                    return expenseAmount;
                }
                catch (ArgumentNullException)
                {
                    throw new InvalidBankStatementException(CsvLineReference, "The amount indicated in the file is null");
                }
                catch (FormatException)
                {
                    throw new InvalidBankStatementException(CsvLineReference, "Invalid format of the value of the expense");
                }
                catch (OverflowException)
                {
                    throw new InvalidBankStatementException(CsvLineReference, "Value of the expense is invalid, may be lesser than the supported min for decimals or greater than the supported max");
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new InvalidBankStatementException(CsvLineReference, "Index out of bounds");
            }
        }

        private static string GetExpenseDescription(string[] lineContents, int CsvLineReference)
        {
            try
            {
                var expenseDescription = lineContents[3];
                return expenseDescription;
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new InvalidBankStatementException(CsvLineReference, "Index out of bounds");
            }

        }

    }
}
