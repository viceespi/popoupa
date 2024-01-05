using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Popoupa.Core.CategorizerStuff;
using static Popoupa.Core.Expense;
using Popoupa.Core.Parsers;

namespace Popoupa.Core.UnitTests.UnitTests
{
    public class CategorizerTests
    {
        [Fact]
        public void CategorizingManually_OneExpense_AndGettingTheCategoryChangedToTheInformedOne()
        {
            var inputString = "Data,Valor,Identificador,Descrição\r\n,01/07/2023,-2.00,64a03490-9a14-4167-87d7-0da6c9fed56e,Compra no débito - Imperio Paes e Doces\r\n";
            UTF8Encoding encoder = new UTF8Encoding();
            byte[] csvFile = encoder.GetBytes(inputString);
            var parser = new NubankCSVParser();
            var expenseArray = parser.Parse(csvFile, encoder);
            var categorizer = new ExpenseCategorizer();
            categorizer.ManualCategorizer(expenseArray[0], CategoryState.HappyHours);
            Assert.True(expenseArray[0].Category == CategoryState.HappyHours);

        }

        [Fact]

        public void CategorizingManuallyAndThenAutomatically_ThreeExpensesWhitTheSameKeyValueInAArrayContainingSevenExpenses_AndExpectingEachOneToBeCategorizedWhitTheManuallyChosenCategory()
        {
            var inputString = "Data,Valor,Identificador,Descrição\r\n01/08/2023,899.80,64c9741b-f65f-4047-9496-41bfb90ca978,Resgate RDB\r\n01/08/2023,-899.80,64c97431-4933-4d51-87c2-db051116fe0c,Pagamento de fatura\r\n02/08/2023,-7.65,64ca28cc-d05e-4df6-b2fe-bdec7c008af6,Compra no débito - Imperio Paes e Doces\r\n03/08/2023,-107.36,64cbcc4d-2094-461b-a2e6-848cc90f6e64,Compra no débito - Restaurante-Pizzaria\r\n09/08/2023,-129.67,64d40ca2-f62d-4e2b-b16d-952d127a07db,Compra no débito - Muffato Votuporanga\r\n09/08/2023,-429.67,64ef6501-08c5-4319-9eac-0e95d58e1562,Compra no débito - Muffato Votuporanga\r\n09/08/2023,-229.67,64f0ce51-e5b4-454c-910b-2976c7147745,Compra no débito - Muffato Votuporanga";
            UTF8Encoding encoder = new UTF8Encoding();
            byte[] csvFile = encoder.GetBytes(inputString);
            var parser = new NubankCSVParser();
            var expenseArray = parser.Parse(csvFile, encoder);
            var categorizer = new ExpenseCategorizer();
            categorizer.ManualCategorizer(expenseArray[5], CategoryState.MarketEssentials);
            categorizer.AutomaticCategorization(expenseArray, expenseArray[5]);
            Assert.True(expenseArray[3].Category == CategoryState.MarketEssentials && expenseArray[4].Category == CategoryState.MarketEssentials && expenseArray[5].Category == CategoryState.MarketEssentials);
        }

        [Fact]

        public void ChoosingToCategorize_AnExpenseThatDoesNotExistsInTheExpenseArray_ExpectingAExceptionThrow()
        {
            var inputString = "Data,Valor,Identificador,Descrição\r\n01/08/2023,899.80,64c9741b-f65f-4047-9496-41bfb90ca978,Resgate RDB\r\n01/08/2023,-899.80,64c97431-4933-4d51-87c2-db051116fe0c,Pagamento de fatura\r\n02/08/2023,-7.65,64ca28cc-d05e-4df6-b2fe-bdec7c008af6,Compra no débito - Imperio Paes e Doces\r\n03/08/2023,-107.36,64cbcc4d-2094-461b-a2e6-848cc90f6e64,Compra no débito - Restaurante-Pizzaria\r\n09/08/2023,-129.67,64d40ca2-f62d-4e2b-b16d-952d127a07db,Compra no débito - Muffato Votuporanga\r\n09/08/2023,-429.67,64ef6501-08c5-4319-9eac-0e95d58e1562,Compra no débito - Muffato Votuporanga\r\n09/08/2023,-229.67,64f0ce51-e5b4-454c-910b-2976c7147745,Compra no débito - Muffato Votuporanga";
            UTF8Encoding encoder = new UTF8Encoding();
            byte[] csvFile = encoder.GetBytes(inputString);
            var parser = new NubankCSVParser();
            var expenseArray = parser.Parse(csvFile, encoder);
            var date = new DateTime(2023, 1, 5);
            var fakeExpense = new Expense("Pamonharia da Nenega", date, 20000, CategoryState.Uncategorized);
            var categorizer = new ExpenseCategorizer();
            Exception exception = Assert.Throws<Exception>(() =>
            {
                categorizer.AutomaticCategorization(expenseArray, fakeExpense);
            });
            Assert.Equal("An expense that doesn't exists was chosen.", exception.Message);
        }

        [Fact]

        public void TryingToGetTheListOfCategoriesOnTheArchive_OfAnExpenseDescriptionThatHas5CategoriesTotalBeingTwoOfOneCategoryAndThreeOfAnotherOneCategory_ExpectingToGetAListOfCategoriesWhitTheCorrectAmountAndType()
        {
            var inputString = "Data,Valor,Identificador,Descrição\r\n01/08/2023,899.80,64c9741b-f65f-4047-9496-41bfb90ca978,Resgate RDB\r\n01/08/2023,-899.80,64c97431-4933-4d51-87c2-db051116fe0c,Pagamento de fatura\r\n02/08/2023,-7.65,64ca28cc-d05e-4df6-b2fe-bdec7c008af6,Compra no débito - Imperio Paes e Doces\r\n03/08/2023,-107.36,64cbcc4d-2094-461b-a2e6-848cc90f6e64,Compra no débito - Restaurante-Pizzaria\r\n09/08/2023,-129.67,64d40ca2-f62d-4e2b-b16d-952d127a07db,Compra no débito - Muffato Votuporanga\r\n09/08/2023,-429.67,64ef6501-08c5-4319-9cac-0e95d58e1562,Compra no débito - Muffato Votuporanga\r\n09/08/2023,-229.67,64f0ce51-e5b4-454c-910b-2976c7147745,Compra no débito - Muffato Votuporanga\r\n30/08/2023,500.00,64ef6501-08c5-4319-9eac-0e95d58e1562,Transferência Recebida - Maria da Glória Santos Barros - •••.402.175-•• - NU PAGAMENTOS - IP (0260) Agência: 1 Conta: 86775356-3\r\n30/08/2023,-25.00,64ef81b8-7bac-4c67-9d13-d4a1bd9da622,Compra no débito - Max Gourmet Restaurant\r\n30/08/2023,-159.51,64ef8225-8a07-4625-9ab0-a792570da9bb,Compra no débito - Auto Posto Parceirao\r\n30/08/2023,-42.00,64efe154-5578-4c90-92da-6d0c72d352dc,Compra no débito - Punch Burguer\r\n31/08/2023,11983.87,64f0730d-7046-4c32-87bd-00b8c6345a0c,Transferência Recebida - THADEU FERNANDES SILVA BARROS - •••.914.856-•• - BCO BRADESCO S.A. (0237) Agência: 25 Conta: 33715-3\r\n31/08/2023,-38.90,64f08eb8-7aba-4590-88c3-aa42b3c4498e,Compra no débito - 3535drogasil\r\n31/08/2023,-442.55,64f0ce51-e5b4-454c-910b-2976c7147745,Transferência enviada pelo Pix - PET CENTER PARTICIPACOES S A  - 18.328.118/0001-09 - ITAÚ UNIBANCO S.A. (0341) Agência: 38 Conta: 4171-1\r\n09/08/2023,-529.67,64ef6501-08c5-4319-9bac-0e95d58e1562,Compra no débito - Muffato Votuporanga\r\n09/08/2023,-349.67,64f0ce51-e5b4-454c-912b-2976c7147745,Compra no débito - Muffato Votuporanga";
            UTF8Encoding encoder = new UTF8Encoding();
            byte[] csvFile = encoder.GetBytes(inputString);
            var parser = new NubankCSVParser();
            var expenseArchiveList = parser.Parse(csvFile, encoder);
            var categorizer = new ExpenseCategorizer();
            categorizer.ManualCategorizer(expenseArchiveList[3], CategoryState.HappyHours);
            categorizer.ManualCategorizer(expenseArchiveList[4], CategoryState.MarketEssentials);
            categorizer.ManualCategorizer(expenseArchiveList[5], CategoryState.HappyHours);
            categorizer.ManualCategorizer(expenseArchiveList[11], CategoryState.MarketEssentials);
            categorizer.ManualCategorizer(expenseArchiveList[12], CategoryState.MarketEssentials);
            var muffatoCategoryList = categorizer.GetListOfCategories(expenseArchiveList, expenseArchiveList[4]);
            var toCheckCategoryList = new List<CategoryState> { CategoryState.HappyHours, CategoryState.MarketEssentials, CategoryState.HappyHours, CategoryState.MarketEssentials, CategoryState.MarketEssentials };
            Assert.Equal(muffatoCategoryList, toCheckCategoryList);
        }

        [Fact]

        public void TryingToGetTheListOfCategoriesOnTheArchive_OfAnExpenseDescriptionThatDoesNotExistsInTheArchive_ExpectingThenAEmptyListBecauseItIsTheFirstTimeThatThisExpenseIsBeingRegistered()
        {
            var inputString = "Data,Valor,Identificador,Descrição\r\n01/08/2023,899.80,64c9741b-f65f-4047-9496-41bfb90ca978,Resgate RDB\r\n01/08/2023,-899.80,64c97431-4933-4d51-87c2-db051116fe0c,Pagamento de fatura\r\n02/08/2023,-7.65,64ca28cc-d05e-4df6-b2fe-bdec7c008af6,Compra no débito - Imperio Paes e Doces\r\n03/08/2023,-107.36,64cbcc4d-2094-461b-a2e6-848cc90f6e64,Compra no débito - Restaurante-Pizzaria\r\n09/08/2023,-129.67,64d40ca2-f62d-4e2b-b16d-952d127a07db,Compra no débito - Muffato Votuporanga\r\n09/08/2023,-429.67,64ef6501-08c5-4319-9cac-0e95d58e1562,Compra no débito - Muffato Votuporanga\r\n09/08/2023,-229.67,64f0ce51-e5b4-454c-910b-2976c7147745,Compra no débito - Muffato Votuporanga\r\n30/08/2023,500.00,64ef6501-08c5-4319-9eac-0e95d58e1562,Transferência Recebida - Maria da Glória Santos Barros - •••.402.175-•• - NU PAGAMENTOS - IP (0260) Agência: 1 Conta: 86775356-3\r\n30/08/2023,-25.00,64ef81b8-7bac-4c67-9d13-d4a1bd9da622,Compra no débito - Max Gourmet Restaurant\r\n30/08/2023,-159.51,64ef8225-8a07-4625-9ab0-a792570da9bb,Compra no débito - Auto Posto Parceirao\r\n30/08/2023,-42.00,64efe154-5578-4c90-92da-6d0c72d352dc,Compra no débito - Punch Burguer\r\n31/08/2023,11983.87,64f0730d-7046-4c32-87bd-00b8c6345a0c,Transferência Recebida - THADEU FERNANDES SILVA BARROS - •••.914.856-•• - BCO BRADESCO S.A. (0237) Agência: 25 Conta: 33715-3\r\n31/08/2023,-38.90,64f08eb8-7aba-4590-88c3-aa42b3c4498e,Compra no débito - 3535drogasil\r\n31/08/2023,-442.55,64f0ce51-e5b4-454c-910b-2976c7147745,Transferência enviada pelo Pix - PET CENTER PARTICIPACOES S A  - 18.328.118/0001-09 - ITAÚ UNIBANCO S.A. (0341) Agência: 38 Conta: 4171-1\r\n09/08/2023,-529.67,64ef6501-08c5-4319-9bac-0e95d58e1562,Compra no débito - Muffato Votuporanga\r\n09/08/2023,-349.67,64f0ce51-e5b4-454c-912b-2976c7147745,Compra no débito - Muffato Votuporanga";
            UTF8Encoding encoder = new UTF8Encoding();
            byte[] csvFile = encoder.GetBytes(inputString);
            var parser = new NubankCSVParser();
            var expenseArchiveList = parser.Parse(csvFile, encoder);
            var date = new DateTime(2023, 1, 5);
            var fakeExpense = new Expense("Pamonharia da Nenega", date, 20000, CategoryState.Uncategorized);
            var categorizer = new ExpenseCategorizer();
            var pamonhariaNenegaCategoryList = categorizer.GetListOfCategories(expenseArchiveList, fakeExpense);
            var toCheckCategoryList = new List<CategoryState> { CategoryState.Uncategorized };
            Assert.Equal(pamonhariaNenegaCategoryList, toCheckCategoryList);
        }

        [Fact]

        public void CheckingIfADescriptionHasMoreThanOneCategoryAssociatedInTheArchive_UsingAnExpense_ExpectingThatTheResultIsTrue()
        {
            var inputString = "Data,Valor,Identificador,Descrição\r\n01/08/2023,899.80,64c9741b-f65f-4047-9496-41bfb90ca978,Resgate RDB\r\n01/08/2023,-899.80,64c97431-4933-4d51-87c2-db051116fe0c,Pagamento de fatura\r\n02/08/2023,-7.65,64ca28cc-d05e-4df6-b2fe-bdec7c008af6,Compra no débito - Imperio Paes e Doces\r\n03/08/2023,-107.36,64cbcc4d-2094-461b-a2e6-848cc90f6e64,Compra no débito - Restaurante-Pizzaria\r\n09/08/2023,-129.67,64d40ca2-f62d-4e2b-b16d-952d127a07db,Compra no débito - Muffato Votuporanga\r\n09/08/2023,-429.67,64ef6501-08c5-4319-9cac-0e95d58e1562,Compra no débito - Muffato Votuporanga\r\n09/08/2023,-229.67,64f0ce51-e5b4-454c-910b-2976c7147745,Compra no débito - Muffato Votuporanga\r\n30/08/2023,500.00,64ef6501-08c5-4319-9eac-0e95d58e1562,Transferência Recebida - Maria da Glória Santos Barros - •••.402.175-•• - NU PAGAMENTOS - IP (0260) Agência: 1 Conta: 86775356-3\r\n30/08/2023,-25.00,64ef81b8-7bac-4c67-9d13-d4a1bd9da622,Compra no débito - Max Gourmet Restaurant\r\n30/08/2023,-159.51,64ef8225-8a07-4625-9ab0-a792570da9bb,Compra no débito - Auto Posto Parceirao\r\n30/08/2023,-42.00,64efe154-5578-4c90-92da-6d0c72d352dc,Compra no débito - Punch Burguer\r\n31/08/2023,11983.87,64f0730d-7046-4c32-87bd-00b8c6345a0c,Transferência Recebida - THADEU FERNANDES SILVA BARROS - •••.914.856-•• - BCO BRADESCO S.A. (0237) Agência: 25 Conta: 33715-3\r\n31/08/2023,-38.90,64f08eb8-7aba-4590-88c3-aa42b3c4498e,Compra no débito - 3535drogasil\r\n31/08/2023,-442.55,64f0ce51-e5b4-454c-910b-2976c7147745,Transferência enviada pelo Pix - PET CENTER PARTICIPACOES S A  - 18.328.118/0001-09 - ITAÚ UNIBANCO S.A. (0341) Agência: 38 Conta: 4171-1\r\n09/08/2023,-529.67,64ef6501-08c5-4319-9bac-0e95d58e1562,Compra no débito - Muffato Votuporanga\r\n09/08/2023,-349.67,64f0ce51-e5b4-454c-912b-2976c7147745,Compra no débito - Muffato Votuporanga";
            UTF8Encoding encoder = new UTF8Encoding();
            byte[] csvFile = encoder.GetBytes(inputString);
            var parser = new NubankCSVParser();
            var expenseArchiveList = parser.Parse(csvFile, encoder);
            var categorizer = new ExpenseCategorizer();
            categorizer.ManualCategorizer(expenseArchiveList[3], CategoryState.HappyHours);
            categorizer.ManualCategorizer(expenseArchiveList[4], CategoryState.MarketEssentials);
            categorizer.ManualCategorizer(expenseArchiveList[5], CategoryState.HappyHours);
            categorizer.ManualCategorizer(expenseArchiveList[11], CategoryState.MarketEssentials);
            categorizer.ManualCategorizer(expenseArchiveList[12], CategoryState.MarketEssentials);
            var hasMoreThanOneCategory = categorizer.CheckIfExpenseHasMultipleCategoriesAssociated(expenseArchiveList, expenseArchiveList[3]);
            Assert.True(hasMoreThanOneCategory);
        }

        [Fact]
        public void CheckingIfADescriptionHasMoreThanOneCategoryAssociatedInTheArchive_UsingAnExpenseThatHasOnlyOne_ExpectingTheResultIsFalse()
        {
            var inputString = "Data,Valor,Identificador,Descrição\r\n01/08/2023,899.80,64c9741b-f65f-4047-9496-41bfb90ca978,Resgate RDB\r\n01/08/2023,-899.80,64c97431-4933-4d51-87c2-db051116fe0c,Pagamento de fatura\r\n02/08/2023,-7.65,64ca28cc-d05e-4df6-b2fe-bdec7c008af6,Compra no débito - Imperio Paes e Doces\r\n03/08/2023,-107.36,64cbcc4d-2094-461b-a2e6-848cc90f6e64,Compra no débito - Restaurante-Pizzaria\r\n09/08/2023,-129.67,64d40ca2-f62d-4e2b-b16d-952d127a07db,Compra no débito - Muffato Votuporanga\r\n09/08/2023,-429.67,64ef6501-08c5-4319-9cac-0e95d58e1562,Compra no débito - Muffato Votuporanga\r\n09/08/2023,-229.67,64f0ce51-e5b4-454c-910b-2976c7147745,Compra no débito - Muffato Votuporanga\r\n30/08/2023,500.00,64ef6501-08c5-4319-9eac-0e95d58e1562,Transferência Recebida - Maria da Glória Santos Barros - •••.402.175-•• - NU PAGAMENTOS - IP (0260) Agência: 1 Conta: 86775356-3\r\n30/08/2023,-25.00,64ef81b8-7bac-4c67-9d13-d4a1bd9da622,Compra no débito - Max Gourmet Restaurant\r\n30/08/2023,-159.51,64ef8225-8a07-4625-9ab0-a792570da9bb,Compra no débito - Auto Posto Parceirao\r\n30/08/2023,-42.00,64efe154-5578-4c90-92da-6d0c72d352dc,Compra no débito - Punch Burguer\r\n31/08/2023,11983.87,64f0730d-7046-4c32-87bd-00b8c6345a0c,Transferência Recebida - THADEU FERNANDES SILVA BARROS - •••.914.856-•• - BCO BRADESCO S.A. (0237) Agência: 25 Conta: 33715-3\r\n31/08/2023,-38.90,64f08eb8-7aba-4590-88c3-aa42b3c4498e,Compra no débito - 3535drogasil\r\n31/08/2023,-442.55,64f0ce51-e5b4-454c-910b-2976c7147745,Transferência enviada pelo Pix - PET CENTER PARTICIPACOES S A  - 18.328.118/0001-09 - ITAÚ UNIBANCO S.A. (0341) Agência: 38 Conta: 4171-1\r\n09/08/2023,-529.67,64ef6501-08c5-4319-9bac-0e95d58e1562,Compra no débito - Muffato Votuporanga\r\n09/08/2023,-349.67,64f0ce51-e5b4-454c-912b-2976c7147745,Compra no débito - Muffato Votuporanga";
            UTF8Encoding encoder = new UTF8Encoding();
            byte[] csvFile = encoder.GetBytes(inputString);
            var parser = new NubankCSVParser();
            var expenseArchiveList = parser.Parse(csvFile, encoder);
            var categorizer = new ExpenseCategorizer();
            categorizer.ManualCategorizer(expenseArchiveList[3], CategoryState.MarketEssentials);
            categorizer.AutomaticCategorization(expenseArchiveList, expenseArchiveList[3]);
            var hasMoreThanOneCategory = categorizer.CheckIfExpenseHasMultipleCategoriesAssociated(expenseArchiveList, expenseArchiveList[3]);
            Assert.False(hasMoreThanOneCategory);
        }

        [Fact]

        public void CheckingIfADescriptionHasMoreThanOneCategoryAssociatedInTheArchive_UsingAnExpenseThatIsNotInTheArchive_ExpectingTheResultIsFalse()
        {
            var inputString = "Data,Valor,Identificador,Descrição\r\n01/08/2023,899.80,64c9741b-f65f-4047-9496-41bfb90ca978,Resgate RDB\r\n01/08/2023,-899.80,64c97431-4933-4d51-87c2-db051116fe0c,Pagamento de fatura\r\n02/08/2023,-7.65,64ca28cc-d05e-4df6-b2fe-bdec7c008af6,Compra no débito - Imperio Paes e Doces\r\n03/08/2023,-107.36,64cbcc4d-2094-461b-a2e6-848cc90f6e64,Compra no débito - Restaurante-Pizzaria\r\n09/08/2023,-129.67,64d40ca2-f62d-4e2b-b16d-952d127a07db,Compra no débito - Muffato Votuporanga\r\n09/08/2023,-429.67,64ef6501-08c5-4319-9cac-0e95d58e1562,Compra no débito - Muffato Votuporanga\r\n09/08/2023,-229.67,64f0ce51-e5b4-454c-910b-2976c7147745,Compra no débito - Muffato Votuporanga\r\n30/08/2023,500.00,64ef6501-08c5-4319-9eac-0e95d58e1562,Transferência Recebida - Maria da Glória Santos Barros - •••.402.175-•• - NU PAGAMENTOS - IP (0260) Agência: 1 Conta: 86775356-3\r\n30/08/2023,-25.00,64ef81b8-7bac-4c67-9d13-d4a1bd9da622,Compra no débito - Max Gourmet Restaurant\r\n30/08/2023,-159.51,64ef8225-8a07-4625-9ab0-a792570da9bb,Compra no débito - Auto Posto Parceirao\r\n30/08/2023,-42.00,64efe154-5578-4c90-92da-6d0c72d352dc,Compra no débito - Punch Burguer\r\n31/08/2023,11983.87,64f0730d-7046-4c32-87bd-00b8c6345a0c,Transferência Recebida - THADEU FERNANDES SILVA BARROS - •••.914.856-•• - BCO BRADESCO S.A. (0237) Agência: 25 Conta: 33715-3\r\n31/08/2023,-38.90,64f08eb8-7aba-4590-88c3-aa42b3c4498e,Compra no débito - 3535drogasil\r\n31/08/2023,-442.55,64f0ce51-e5b4-454c-910b-2976c7147745,Transferência enviada pelo Pix - PET CENTER PARTICIPACOES S A  - 18.328.118/0001-09 - ITAÚ UNIBANCO S.A. (0341) Agência: 38 Conta: 4171-1\r\n09/08/2023,-529.67,64ef6501-08c5-4319-9bac-0e95d58e1562,Compra no débito - Muffato Votuporanga\r\n09/08/2023,-349.67,64f0ce51-e5b4-454c-912b-2976c7147745,Compra no débito - Muffato Votuporanga";
            UTF8Encoding encoder = new UTF8Encoding();
            byte[] csvFile = encoder.GetBytes(inputString);
            var parser = new NubankCSVParser();
            var expenseArchiveList = parser.Parse(csvFile, encoder);
            var date = new DateTime(2023, 1, 5);
            var fakeExpense = new Expense("Pamonharia da Nenega", date, 20000, CategoryState.Uncategorized);
            var categorizer = new ExpenseCategorizer();
            var hasMoreThanOneCategory = categorizer.CheckIfExpenseHasMultipleCategoriesAssociated(expenseArchiveList, fakeExpense);
            Assert.False(hasMoreThanOneCategory);
        }

    }
}
