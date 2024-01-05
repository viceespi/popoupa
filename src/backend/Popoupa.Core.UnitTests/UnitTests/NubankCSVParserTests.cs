using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Popoupa.Core.Parsers;

namespace Popoupa.Core.UnitTests.UnitTests
{
    public class NubankCSVParserTests
    {

        [Fact]
        public void Parse_CSVFileWhitFiveLinesBeingTheHeaderAndFourExpenses_FourExpensesReturnedInList()
        {
            var inputstring = "Data,Valor,Identificador,Descrição\r\n,01/07/2023,-2.00,64a03490-9a14-4167-87d7-0da6c9fed56e,Compra no débito - Imperio Paes e Doces\r\n01/07/2023,-32.29,64a0847a-5b01-4065-be9f-61b2c031a0b1,Compra no débito - Acai da Barra\r\n01/07/2023,-157.89,64a0a76d-248e-47d4-afe1-d6a2c5f91a29,Compra no débito via NuPay - iFood\r\n02/07/2023,-80.00,64a16e22-2cdc-42cd-809c-6b212a48c57a,Compra no débito - Perene Cafe Bistro\r\n";
            UTF8Encoding encoder = new UTF8Encoding();
            byte[] csvfile = encoder.GetBytes(inputstring);
            var parser = new NubankCSVParser();
            var result = parser.Parse(csvfile, encoder);
            Assert.Equal(4, result.Count);
        }

        [Fact]

        public void Parse_CSVFileWhitoutHeader_NoHeaderInvalidBankStatementExceptionExpected()
        {
            var inputstring = ",01/07/2023,-2.00,64a03490-9a14-4167-87d7-0da6c9fed56e,Compra no débito - Imperio Paes e Doces\r\n01/07/2023,-32.29,64a0847a-5b01-4065-be9f-61b2c031a0b1,Compra no débito - Acai da Barra\r\n01/07/2023,-157.89,64a0a76d-248e-47d4-afe1-d6a2c5f91a29,Compra no débito via NuPay - iFood\r\n02/07/2023,-80.00,64a16e22-2cdc-42cd-809c-6b212a48c57a,Compra no débito - Perene Cafe Bistro\r\n";
            UTF8Encoding encoder = new UTF8Encoding();
            byte[] csvfile = encoder.GetBytes(inputstring);
            var parser = new NubankCSVParser();
            Assert.Throws<InvalidBankStatementException>(() => parser.Parse(csvfile, encoder));
        }

        [Fact]

        public void Parse_CSVFileWhitAHeaderWhitLengthSmallerThanTheIndicated_LengthOutOfBoundsInvalidBankStatementExceptionExpected()
        {
            var inputstring = "Data,Valor,Identidade,Descrição\r\n,01/07/2023,-2.00,64a03490-9a14-4167-87d7-0da6c9fed56e,Compra no débito - Imperio Paes e Doces\r\n01/07/2023,-32.29,64a0847a-5b01-4065-be9f-61b2c031a0b1,Compra no débito - Acai da Barra\r\n01/07/2023,-157.89,64a0a76d-248e-47d4-afe1-d6a2c5f91a29,Compra no débito via NuPay - iFood\r\n02/07/2023,-80.00,64a16e22-2cdc-42cd-809c-6b212a48c57a,Compra no débito - Perene Cafe Bistro\r\n";
            UTF8Encoding encoder = new UTF8Encoding();
            byte[] csvfile = encoder.GetBytes(inputstring);
            var parser = new NubankCSVParser();
            Assert.Throws<InvalidBankStatementException>(() => parser.Parse(csvfile, encoder));
        }

        [Fact]

        public void Parse_CSVFileWhitInvalidLineByDateBeingAbsent_InvalidLineInvalidBankStatementExceptionExpected()
        {
            var inputstring = "Data,Valor,Identificador,Descrição\r\n,,-2.00,64a03490-9a14-4167-87d7-0da6c9fed56e,Compra no débito - Imperio Paes e Doces\r\n01/07/2023,-32.29,64a0847a-5b01-4065-be9f-61b2c031a0b1,Compra no débito - Acai da Barra\r\n01/07/2023,-157.89,64a0a76d-248e-47d4-afe1-d6a2c5f91a29,Compra no débito via NuPay - iFood\r\n02/07/2023,-80.00,64a16e22-2cdc-42cd-809c-6b212a48c57a,Compra no débito - Perene Cafe Bistro\r\n";
            UTF8Encoding encoder = new UTF8Encoding();
            byte[] csvfile = encoder.GetBytes(inputstring);
            var parser = new NubankCSVParser();
            Assert.Throws<InvalidBankStatementException>(() => parser.Parse(csvfile, encoder));
        }

        [Fact]

        public void Parse_CSVFileWhitInvalidDateFormat_InvalidDateInvalidBankStatementExceptionExpected()
        {
            var inputstring = "Data,Valor,Identificador,Descrição\r\n,primeiro de julho de dois mil e vinte três,-2.00,64a03490-9a14-4167-87d7-0da6c9fed56e,Compra no débito - Imperio Paes e Doces\r\n01/07/2023,-32.29,64a0847a-5b01-4065-be9f-61b2c031a0b1,Compra no débito - Acai da Barra\r\n01/07/2023,-157.89,64a0a76d-248e-47d4-afe1-d6a2c5f91a29,Compra no débito via NuPay - iFood\r\n02/07/2023,-80.00,64a16e22-2cdc-42cd-809c-6b212a48c57a,Compra no débito - Perene Cafe Bistro\r\n";
            UTF8Encoding encoder = new UTF8Encoding();
            byte[] csvfile = encoder.GetBytes(inputstring);
            var parser = new NubankCSVParser();
            Assert.Throws<InvalidBankStatementException>(() => parser.Parse(csvfile, encoder));
        }

        [Fact]

        public void Parse_CSVFileWhitInvalidLineByExpenseAmountBeingAbsent_InvalidLineInvalidBankStatementExceptionExpected()
        {
            var inputstring = "Data,Valor,Identificador,Descrição\r\n,01/07/2023,,64a03490-9a14-4167-87d7-0da6c9fed56e,Compra no débito - Imperio Paes e Doces\r\n01/07/2023,-32.29,64a0847a-5b01-4065-be9f-61b2c031a0b1,Compra no débito - Acai da Barra\r\n01/07/2023,-157.89,64a0a76d-248e-47d4-afe1-d6a2c5f91a29,Compra no débito via NuPay - iFood\r\n02/07/2023,-80.00,64a16e22-2cdc-42cd-809c-6b212a48c57a,Compra no débito - Perene Cafe Bistro\r\n";
            UTF8Encoding encoder = new UTF8Encoding();
            byte[] csvfile = encoder.GetBytes(inputstring);
            var parser = new NubankCSVParser();
            Assert.Throws<InvalidBankStatementException>(() => parser.Parse(csvfile, encoder));
        }

        [Fact]

        public void Parse_CSVFileWhitInvalidExpenseAmountFormat_InvalidExpenseAmountInvalidBankStatementExceptionExpected()
        {
            var inputstring = "Data,Valor,Identificador,Descrição\r\n,01/07/2023,-dois reais,64a03490-9a14-4167-87d7-0da6c9fed56e,Compra no débito - Imperio Paes e Doces\r\n01/07/2023,-32.29,64a0847a-5b01-4065-be9f-61b2c031a0b1,Compra no débito - Acai da Barra\r\n01/07/2023,-157.89,64a0a76d-248e-47d4-afe1-d6a2c5f91a29,Compra no débito via NuPay - iFood\r\n02/07/2023,-80.00,64a16e22-2cdc-42cd-809c-6b212a48c57a,Compra no débito - Perene Cafe Bistro\r\n";
            UTF8Encoding encoder = new UTF8Encoding();
            byte[] csvfile = encoder.GetBytes(inputstring);
            var parser = new NubankCSVParser();
            Assert.Throws<InvalidBankStatementException>(() => parser.Parse(csvfile, encoder));
        }

        [Fact]

        public void Parse_CSVFileWhitInvalidLineByExpenseDescriptionBeingAbsent_InvalidLineInvalidBankStatementExceptionExpected()
        {
            var inputstring = "Data,Valor,Identificador,Descrição\r\n,01/07/2023,-2.00,64a03490-9a14-4167-87d7-0da6c9fed56e,\r\n01/07/2023,-32.29,64a0847a-5b01-4065-be9f-61b2c031a0b1,Compra no débito - Acai da Barra\r\n01/07/2023,-157.89,64a0a76d-248e-47d4-afe1-d6a2c5f91a29,Compra no débito via NuPay - iFood\r\n02/07/2023,-80.00,64a16e22-2cdc-42cd-809c-6b212a48c57a,Compra no débito - Perene Cafe Bistro\r\n";
            UTF8Encoding encoder = new UTF8Encoding();
            byte[] csvfile = encoder.GetBytes(inputstring);
            var parser = new NubankCSVParser();
            Assert.Throws<InvalidBankStatementException>(() => parser.Parse(csvfile, encoder));
        }
    }



}
