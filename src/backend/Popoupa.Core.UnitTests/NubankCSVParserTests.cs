using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Popoupa.Core.UnitTests
{
    public class NubankCSVParserTests
    {
        private readonly NubankCSVParser sut = new NubankCSVParser();

        [Fact]
        public void GetLines_StringWhitThreeLines_ThreeStringsReturnedInList()
        {
            var inputstring = "Data,Valor,Identificador,Descrição\r\n01/07/2023,-2.00,64a03490-9a14-4167-87d7-0da6c9fed56e,Compra no débito - Imperio Paes e Doces\r\n01/07/2023,-32.29,64a0847a-5b01-4065-be9f-61b2c031a0b1,Compra no débito - Acai da Barra\r\n";
            var result = sut.GetLines(inputstring);
            Assert.Equal(3, result.Count);
        }

        [Fact]
        public void GetExpense_CSVFileWhitFourLines_FourExpensesReturnedInList()
        {
            var inputstring = "Data,Valor,Identificador,Descrição\r\n01/07/2023,-2.00,64a03490-9a14-4167-87d7-0da6c9fed56e,Compra no débito - Imperio Paes e Doces\r\n01/07/2023,-32.29,64a0847a-5b01-4065-be9f-61b2c031a0b1,Compra no débito - Acai da Barra\r\n01/07/2023,-157.89,64a0a76d-248e-47d4-afe1-d6a2c5f91a29,Compra no débito via NuPay - iFood\r\n02/07/2023,-80.00,64a16e22-2cdc-42cd-809c-6b212a48c57a,Compra no débito - Perene Cafe Bistro\r\n";
            UTF8Encoding encoder = new UTF8Encoding();
            byte[] csvfile = encoder.GetBytes(inputstring);
            var result = sut.Parse(csvfile, encoder);
            Assert.Equal(4, result.Count);
        }
    }
}
