using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Popoupa.Core.CategorizerStuff.PopoHashTable
{
    public class PopoTable<T>
    {
        public PopoTable()
        {
            Contents = new List<PopoPair<T>>[100];
        }

        public PopoTable(List<Expense> expenseList, Func<Expense, T> selector)
        {
            Contents = GetPopoTableFromList(expenseList, selector);
        }
        public List<PopoPair<T>>[] Contents { get; set; }

        public void Add(string key, PopoPair<T> toAddPopoPair)
        {
            var index = PopoHasher(key);
            if (this.Contents[index] is null) this.Contents[index] = new List<PopoPair<T>>();
            var additionHappened = false;
            foreach (PopoPair<T> popoTablePair in this.Contents[index])
            {
                if (popoTablePair.KeyDescription == key)
                {
                    foreach (T listElement in toAddPopoPair.KeyValue) popoTablePair.KeyValue.Add(listElement);
                    additionHappened = true;
                    break;
                }
            }
            if (!additionHappened) this.Contents[index].Add(toAddPopoPair);
        }

        public List<PopoPair<T>> Get(string key)
        {
            var index = PopoHasher(key);
            var value = this.Contents[index];
            return value;
        }

        private int PopoHasher(string key)
        {
            var min = key[0];
            var max = key[key.Length - 1];
            var middle = key[key.Length / 2];
            var hasherIndex = (min + max + middle) * 31 % 100;
            return hasherIndex;
        }

        public static List<PopoPair<T>>[] GetPopoTableFromList(List<Expense> expenseList, Func<Expense, T> selector)
        {
            PopoTable<T> popoTable = new();
            foreach (Expense expense in expenseList)
            {
                var value = selector.Invoke(expense);
                PopoPair<T> popoPair = new(expense.Description, value);
                popoTable.Add(expense.Description, popoPair);
            }
            return popoTable.Contents;
        }

    }
}
