using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Popoupa.Core.CategorizerStuff.PopoHashTable
{
    public class PopoPair<T>
    {
        public PopoPair(string expenseDescription, T expenseValue)
        {
            var valueList = new List<T>();
            valueList.Add(expenseValue);
            KeyDescription = expenseDescription;
            KeyValue = valueList;
        }
        public string KeyDescription { get; }

        public List<T> KeyValue { get; set; }
    }

}
