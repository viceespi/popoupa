using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;

using System.Threading.Tasks;

namespace Popoupa.Core
{
    public class NubankCSVParser
    {
        public List<Expense> Parse(byte[] fileContents, Encoding fileEncoding)
        {
            var expensesArray = new List<Expense>();
            var fileString = fileEncoding.GetString(fileContents);
            var csvLines = GetLines(fileString);
            var index = 0;
            foreach (string line in csvLines)
            {
                if (line != csvLines[0])
                {
                    var separatedLine = GetColumns(line);
                    Decimal expenseAmount;
                    var isExpense = GetExpenseAmount(separatedLine, index, out expenseAmount);
                    if (isExpense)
                    {
                        var expenseDate = GetExpenseDate(separatedLine, index);
                        var expenseDescription = GetExpenseDescription(separatedLine, index);
                        var expense = new Expense(expenseDescription, expenseDate, expenseAmount);
                        expensesArray.Add(expense);
                        index += 1;
                    }
                    else
                    {
                        index += 1;
                    }
                    
                }
            }
            return expensesArray;

        }

        public List<string> GetLines(string filestring)
        {
            StringBuilder line = new StringBuilder();
            List<string> lines = new List<string>();
            int index = 0;
          
            while (index != filestring.Length)
            {
                char character = filestring[index];
                if (character != '\n')
                {
                    line.Append(character);
                    index += 1;
                }
                else
                {
                    lines.Add(line.ToString());
                    index += 1;
                    line.Clear();
                }
                
            }
            return lines;
        }

        public List<string> GetColumns(string line)
        {
            StringBuilder column = new StringBuilder();
            List<string> separatedline = new List<string>();
            int index = 0;
            
            while (index != line.Length)
            {
                char character = line[index];
                if (character == ',' || character == '\r')
                {
                    separatedline.Add(column.ToString());
                    index += 1;
                    column.Clear();
                    
                }
                else
                {
                    column.Append(character);
                    index += 1;
                }
            }
            return separatedline;
        }

        public DateTime GetExpenseDate(List<string> lineContents, int lineIndex)
        {
            CultureInfo brazilian = new CultureInfo("pt-BR");
            var dateAsString = lineContents[0];
            var line = lineIndex + 1;
            try
            {
                var date = DateTime.ParseExact(dateAsString, "dd/MM/yyyy", brazilian);
                return date;
            }
            catch(ArgumentNullException)
            {
                throw new InvalidBankStatementException(line, "No date present in the line");
            }
            catch(FormatException)
            {
                throw new InvalidBankStatementException(line, "Invalid format of the date");
            }
        }

        public bool GetExpenseAmount(List<string> lineContents, int lineIdex, out decimal Amount)
        {
            var amountAsString = lineContents[1];
            var line = lineIdex + 1;
            if (amountAsString[0] == '-')
            {
                try
                {
                    var expenseAmount = decimal.Parse(amountAsString);
                    Amount = expenseAmount;
                    return true;
                }
                catch(ArgumentNullException)
                {
                    throw new InvalidBankStatementException(line, "No value of the expense present in the line");
                }
                catch(FormatException)
                {
                    throw new InvalidBankStatementException(line, "Invalid format of the value of the expense");
                }
                catch (OverflowException)
                {
                    throw new InvalidBankStatementException(line, "Invalid value of the expense");
                }

            }
            else
            {
                Amount = 0;
                return false;
            }
            
        }

        public string GetExpenseDescription(List<string> lineContents, int lineIdex)
        {
            var expenseDescription = lineContents[3];
            var line = lineIdex + 1;
            if (string.IsNullOrEmpty(expenseDescription))
            {
                throw new InvalidBankStatementException(line, "Invalid file, expense has no description");
            }
            return expenseDescription;
        }

    }
}
