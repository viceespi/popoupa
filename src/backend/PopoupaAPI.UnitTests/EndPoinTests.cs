using Microsoft.AspNetCore.Mvc.Testing;
using Popoupa.API.APIClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace PopoupaAPI.IntegrationTests
{

    public class IntegrationTests : IClassFixture<WebApplicationFactory<Popoupa.API.Program>>
    {
        private readonly WebApplicationFactory<Popoupa.API.Program> _factory;

        public IntegrationTests(WebApplicationFactory<Popoupa.API.Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Can_Get_Endpoint_Response()
        {
            // Arrange
            var client = _factory.CreateClient();
            var bankstatment = new BankStatementMessageObject
            {
                BSFormatType = "nubank",
                BSContent = """
                Data,Valor,Identificador,Descrição
                01/07/2023,-2.00,64a03490-9a14-4167-87d7-0da6c9fed56e,Compra no débito - Imperio Paes e Doces
                01/07/2023,-32.29,64a0847a-5b01-4065-be9f-61b2c031a0b1,Compra no débito - Acai da Barra
                01/07/2023,-157.89,64a0a76d-248e-47d4-afe1-d6a2c5f91a29,Compra no débito via NuPay - iFood
                02/07/2023,-80.00,64a16e22-2cdc-42cd-809c-6b212a48c57a,Compra no débito - Perene Cafe Bistro
                02/07/2023,-61.63,64a1c2fe-ac85-487a-b5ec-750b8de3bdc9,Transferência enviada pelo Pix - CLARO - 40.432.544/0001-47 - ITAÚ UNIBANCO S.A. (0341) Agência: 911 Conta: 6206-5
                02/07/2023,-61.66,64a1c30c-ebad-4b42-ae85-54f1e7151e52,Transferência enviada pelo Pix - CLARO - 40.432.544/0001-47 - ITAÚ UNIBANCO S.A. (0341) Agência: 911 Conta: 6206-5
                02/07/2023,-65.40,64a1cd87-1a58-4a1a-b392-4649bf75efb9,Compra no débito - Hc e Alimentos
                02/07/2023,-130.00,64a1d6f7-634d-4630-9cf8-c0ab7bc2d0cf,Transferência enviada pelo Pix - PAYU BRASIL - 08.965.639/0001-13 - ITAÚ UNIBANCO S.A. (0341) Agência: 145 Conta: 83551-8
                02/07/2023,-9.90,64a1d761-f244-4ec1-9161-6007e1939e45,Transferência enviada pelo Pix - PAYU BRASIL - 08.965.639/0001-13 - ITAÚ UNIBANCO S.A. (0341) Agência: 145 Conta: 83551-8
                02/07/2023,-38.10,64a22fd9-6b75-4812-964d-bd9061e37796,Compra no débito via NuPay - iFood
                03/07/2023,-2.00,64a2c33b-dd10-4e0c-a2d1-37985c85ded6,Compra no débito - Imperio Paes e Doces
                03/07/2023,-314.89,64a2d52c-0621-4c5d-bb92-61829836857b,Pagamento de boleto efetuado - ELEKTRO
                03/07/2023,-44.23,64a2d620-c3ea-4a2c-8adc-537042005e42,Pagamento de boleto efetuado - SUPERINTENDENCIA DE AGUA ESGOTOS E MEIO AMBIE
                03/07/2023,-271.04,64a2d661-b120-4590-9b0f-07ae330f9852,Pagamento de boleto efetuado - CESB - CENTRO DE EDUCACAO SUPERIOR DE BR
                03/07/2023,-1504.00,64a2d66e-2295-4256-8670-88e99b2c1c77,Pagamento de boleto efetuado - PJBANK PAGAMENTOS S A
                03/07/2023,-1300.00,64a2d74a-195e-4b8e-acd5-cb266d661be7,Transferência enviada pelo Pix - GILSON FERNANDES DA SILVA BARROS - •••.363.521-•• - CAIXA ECONOMICA FEDERAL (0104) Agência: 4038 Conta: 1288000000773429934-3
                03/07/2023,-2066.25,64a2db98-f5b4-4447-aa92-e83a3a65ee0a,Pagamento de fatura
                03/07/2023,-16.60,64a2e5f9-d296-45f9-acab-a7c4864edee1,Transferência enviada pelo Pix - MAURICIO JUNIOR COIMBRA DA SILVA 40039124894 - 41.845.996/0001-13 - COOP SICREDI PLANALT DAS ÁGUAS Agência: 703 Conta: 6151-5
                03/07/2023,-115.00,64a2fa56-3810-4375-a2d6-80b7e5baed11,Compra no débito - Comercial Agricola An
                03/07/2023,-348.00,64a30964-947c-49ea-a90e-480663cbf674,Compra no débito - Pinheiral
                03/07/2023,-215.00,64a30f57-0161-4074-8223-27e75e7b75cd,Compra no débito - Madeiranit Votuporanga
                03/07/2023,-101.27,64a318c2-cf3b-49cd-b044-1c18f5eeb2a4,Compra no débito - Tc Commar Cafeteria
                03/07/2023,-173.35,64a31fd3-6a06-427a-8d82-4d3eb8299f0d,Transferência enviada pelo Pix - RICARDO MORAES SILVA - •••.346.108-•• - BCO DO BRASIL S.A. (0001) Agência: 268 Conta: 36731-1
                03/07/2023,-147.00,64a3239f-8038-4c11-a0b3-c2f17658c22f,Transferência enviada pelo Pix - ANTONIO APARECIDO RONCOLATO - •••.630.708-•• - BCO RENDIMENTO S.A. (0633) Agência: 1 Conta: 220582000-3
                03/07/2023,-1350.00,64a34ae5-63fe-4e62-9402-29ed8cb808e7,Transferência enviada pelo Pix - ROGERIO ANTONIO DA COSTA PIVA - •••.255.478-•• - BCO SANTANDER (BRASIL) S.A. (0033) Agência: 91 Conta: 1029266-2
                04/07/2023,-92.00,64a38e30-d7e7-413a-a1f1-1c5305b9b1cc,Compra no débito via NuPay - iFood
                04/07/2023,-110.00,64a40f21-7da9-4be3-a67d-c0345fae03cd,Compra no débito - Lucia Carla Dantas Mor
                04/07/2023,-110.00,64a4335b-688f-4582-87a1-84bd417c1885,Transferência enviada pelo Pix - Marcio Lissone - •••.364.588-•• - NU PAGAMENTOS - IP (0260) Agência: 1 Conta: 85825264-7
                04/07/2023,-63.41,64a43d5c-bf37-4885-bf41-90cf738e06ee,Compra no débito - Panificadora Aquiles
                04/07/2023,-33.87,64a44467-01b2-4b74-aeab-4f556a2c8d64,Compra no débito - Raia
                04/07/2023,-100.00,64a48e25-6b3f-450d-a528-59ede5669f61,Transferência enviada pelo Pix - VICENZO ESPINDOLA VAZ DE LIMA - •••.863.871-•• - NU PAGAMENTOS - IP (0260) Agência: 1 Conta: 12817886-9
                04/07/2023,-40.00,64a4b333-76f8-4783-8bc3-3b1f47f86242,Transferência enviada pelo Pix - ELIANE TOFANELI - •••.970.148-•• - BCO SANTANDER (BRASIL) S.A. (0033) Agência: 3311 Conta: 1087581-6
                04/07/2023,-121.99,64a4b830-ddb5-44a1-9390-778d8773bc04,Compra no débito - Thiaguinho Espetos e J
                05/07/2023,-200.00,64a55a95-5de9-4d38-ad0e-1e99555cbd94,Aplicação RDB
                05/07/2023,-102.35,64a5c862-73e8-486f-9aaa-0876244eac14,Compra no débito - Cafe Caipira
                05/07/2023,-15.00,64a5c89e-7b63-4219-9982-590fa9edbc55,Compra no débito - Cafe Caipira
                05/07/2023,-127.00,64a60721-252d-453f-a8eb-59ba24cd67c5,Transferência enviada pelo Pix - Umai Oriental House Restaurante LTDA - 41.529.115/0001-55 - PAGSEGURO INTERNET IP S.A. (0290) Agência: 1 Conta: 48799193-7
                06/07/2023,-2.00,64a6ab50-22a2-4793-afd5-0cd3a7f5fe39,Compra no débito - Mp *Katiaapdfmeavenida
                06/07/2023,-22.60,64a703ff-28ee-4ce8-b737-0464da25abce,Compra no débito - Acai da Barra
                06/07/2023,-253.97,64a70b88-28d8-4299-b284-cf66cfc786f7,Transferência enviada pelo Pix - IRMAOS MUFFATO CIA LTDA - 76.430.438/0001-71 - BCO BRADESCO S.A. (0237) Agência: 3099 Conta: 3883-0
                06/07/2023,-93.75,64a71118-4319-4924-aacc-911b43c683f6,Transferência enviada pelo Pix - IRMAOS MUFFATO CIA LTDA - 76.430.438/0001-71 - BCO BRADESCO S.A. (0237) Agência: 3099 Conta: 3883-0
                06/07/2023,-36.88,64a72cd1-ed39-4df3-a531-a7d35dd068a4,Compra no débito - Nutrial Racoes
                07/07/2023,-29.00,64a811c0-5ac7-43c1-8020-9378b978908f,Pagamento de fatura
                08/07/2023,-815.00,64a9587c-6371-4f8f-acc9-57bf0d5f4c07,Transferência enviada pelo Pix - MARIA DA GLORIA SANTOS BARROS - •••.402.175-•• - ITAÚ UNIBANCO S.A. (0341) Agência: 8580 Conta: 18868-1
                08/07/2023,-68.97,64a9d01d-fd6a-4f9c-b4b8-1e542aefa221,Compra no débito via NuPay - iFood
                09/07/2023,-80.00,64aacfdd-178e-4c35-ac23-245771220ff4,Compra no débito - Posto Banespinha Expre
                09/07/2023,-221.53,64aad42e-f32e-4e24-9107-458b47af5354,Transferência enviada pelo Pix - Raia Drogasil - 61.585.865/0001-51 - ADYEN LATIN AMERICA Agência: 1 Conta: 100000037-1
                09/07/2023,-60.65,64aad7bf-2925-45c9-b1e2-0abf4e9254ef,Transferência enviada pelo Pix - IRMAOS MUFFATO CIA LTDA - 76.430.438/0001-71 - BCO BRADESCO S.A. (0237) Agência: 3099 Conta: 3883-0
                09/07/2023,-74.00,64aae1ad-f569-4b94-8a61-ec63c0aead46,Transferência enviada pelo Pix - THIAGO CORREA DA COSTA 33390035800 - 43.366.346/0001-10 - NU PAGAMENTOS - IP (0260) Agência: 1 Conta: 89606631-8
                09/07/2023,-45.45,64aae8df-6419-44d2-a300-b1df9f3c84fb,Compra no débito - Multi Frango
                10/07/2023,-30.00,64abe772-15b2-4cdd-800a-610a46f8b116,Transferência enviada pelo Pix - Inês Aparecida Perinelli Barboza - •••.333.908-•• - NU PAGAMENTOS - IP (0260) Agência: 1 Conta: 97355030-7
                11/07/2023,-6.50,64ad3549-1bb7-4ace-b20f-e332b6725961,Compra no débito - Imperio Paes e Doces
                11/07/2023,-109.00,64ad80fd-992d-4c44-b990-72af559826ac,Compra no débito - Pag*Umaiorientalhouse
                11/07/2023,-45.00,64ad83d7-f625-4621-b21e-3b37c485eef3,Compra no débito - Elav Lavanderia
                11/07/2023,-90.00,64ad89c6-af34-404b-aa95-c259f1ee94e2,Compra no débito - David Celulares
                11/07/2023,-56.00,64ad89f0-39c9-47e8-a572-263de46b57ac,Compra no débito - David Celulares
                12/07/2023,-3712.50,64aec0f6-bd44-4afb-b178-050b36fb93c4,Transferência enviada pelo Pix - GISELE LEMES BIZO - •••.979.768-•• - BCO DO BRASIL S.A. (0001) Agência: 5840 Conta: 2129-6
                12/07/2023,-87.01,64aeda66-f89d-4c48-beea-b631111b5f90,Compra no débito - Santo Gostinho Rest
                12/07/2023,-577.57,64aedd46-9961-4cae-b334-e47b8875d365,Compra no débito - Nutrial Racoes
                12/07/2023,-438.87,64aef2e6-3832-43b5-aca5-32aa1d61f8ad,Compra no débito - Ricardo Moraes Silva
                12/07/2023,-22.70,64aef544-ce07-489f-b169-601c8501ceda,Compra no débito - Acai da Barra
                12/07/2023,-140.00,64af1562-621c-4266-a2d3-08df12af0a3e,Transferência enviada pelo Pix - Felipe Do Prado Rezende Di Pierro - •••.244.036-•• - NU PAGAMENTOS - IP (0260) Agência: 1 Conta: 18881469-4
                12/07/2023,-76.62,64af17fb-e2c6-4602-83dc-07468ebc2c26,Transferência enviada pelo Pix - PIX Marketplace - 10.573.521/0001-91 - MERCADO PAGO IP LTDA. (0323) Agência: 1 Conta: 2918333524-0
                12/07/2023,-170.83,64af4719-825b-482c-b646-d524e8b9aada,Compra no débito - Baltazar Espetaria
                13/07/2023,-84.90,64b01dfe-5132-4854-bb68-f41f803a977a,Compra no débito - Restaurante-Pizzaria
                14/07/2023,-98.98,64b16c0d-f9c5-4875-b85d-2fb5945ef095,Compra no débito - Restaurante-Pizzaria
                14/07/2023,-106.00,64b1a73d-8c44-4515-a987-2d823e6c0b48,Compra no débito - Sabor de Chocolate
                14/07/2023,-51.15,64b1ab08-d04b-4c8d-8480-6325128396c6,Compra no débito - Panificadora Aquiles
                15/07/2023,-200.00,64b2871f-b15f-4974-b0dd-1d87b920ea75,Transferência enviada pelo Pix - MARIA DA GLORIA SANTOS BARROS - •••.402.175-•• - NU PAGAMENTOS - IP (0260) Agência: 1 Conta: 86775356-3
                15/07/2023,-158.00,64b34af9-7ca0-43e9-8e4c-868e448ff275,Transferência enviada pelo Pix - Umai Oriental House Restaurante LTDA - 41.529.115/0001-55 - PAGSEGURO INTERNET IP S.A. (0290) Agência: 1 Conta: 48799193-7
                16/07/2023,-144.00,64b46ec0-1e05-48fa-b9e8-71055a799a08,Compra no débito - Napoli Pizzeria
                17/07/2023,-18.39,64b55bd1-a73b-4beb-b18b-4cc95156e0f1,Compra no débito - Marcelo Supermercado
                18/07/2023,-300.00,64b72882-5c4a-4b6d-a143-7f34b6b4a30b,Transferência enviada pelo Pix - BRUNA REZENDE RIBEIRO - •••.258.716-•• - CAIXA ECONOMICA FEDERAL (0104) Agência: 1537 Conta: 1288000000853665618-0
                25/07/2023,600.00,64bfa894-67c7-4adc-8535-8c976406194a,Transferência recebida pelo Pix - VICENZO ESPINDOLA VAZ DE LIMA - •••.863.871-•• - SICOOB CREDIMED - CCLA DE UBERABA LTDA Agência: 4130 Conta: 12070-7
                25/07/2023,-1237.50,64bfe616-621b-4224-9e85-1af04c4df1f5,Transferência enviada pelo Pix - GISELE LEMES BIZO - •••.979.768-•• - BCO DO BRASIL S.A. (0001) Agência: 5840 Conta: 2129-6
                26/07/2023,-400.00,64c11a46-dd5f-4c41-94ec-2cfde01c9e80,Compra de BDR - MSFT34
                26/07/2023,65.74,64c11a47-f760-42ca-b7ec-4cad039c28ba,Compra de BDR - MSFT34
                26/07/2023,-312.75,64c1223c-9045-439e-818e-88bf8b8945f9,Aplicação Fundo - Nu Seleção Cautela
                29/07/2023,-2000.00,64c5040d-760e-409f-b121-744bfb59e123,Transferência enviada pelo Pix - IRENE ISABEL DA SILVA BARROS - •••.618.305-•• - CAIXA ECONOMICA FEDERAL (0104) Agência: 2384 Conta: 1288000000764905735-7
                29/07/2023,-80.00,64c5b7ce-4c11-4a0d-8d55-b92206500cf5,Transferência enviada pelo Pix - Lorena Rodrigues de Oliveira Tosta - •••.049.316-•• - NU PAGAMENTOS - IP (0260) Agência: 1 Conta: 48917744-6
                30/07/2023,-3470.00,64c65e6f-b436-4160-aebc-d6c802778ed2,Aplicação RDB
                31/07/2023,8080.37,64c7949d-63a4-4b10-b831-b5ee9ad3b153,Transferência Recebida - THADEU FERNANDES SILVA BARROS - •••.914.856-•• - BCO BRADESCO S.A. (0237) Agência: 25 Conta: 33715-3
                31/07/2023,-50.84,64c7a30c-4c38-42a4-bb02-41137a11f0ce,Pagamento de boleto efetuado - SUPERINTENDENCIA DE AGUA ESGOTOS E MEIO AMBIE
                31/07/2023,-308.35,64c7a336-11ef-41a2-b9db-4de3f0374e25,Pagamento de boleto efetuado - ELEKTRO
                31/07/2023,-1504.00,64c7a435-79ae-4f93-b967-9ab120406ef1,Pagamento de boleto efetuado - PJBANK PAGAMENTOS S A
                31/07/2023,-271.02,64c7a4c3-4152-4e89-b4b7-04dae0e74fd2,Pagamento de boleto efetuado - CESB - CENTRO DE EDUCACAO SUPERIOR DE BR
                31/07/2023,-68.00,64c7a5ab-11f0-4754-b343-b8713ca7f583,Transferência enviada pelo Pix - CLARO - 40.432.544/0001-47 - ITAÚ UNIBANCO S.A. (0341) Agência: 911 Conta: 6206-5
                31/07/2023,-1300.00,64c7a61e-3980-4bf5-8f3f-93d2635b976b,Transferência enviada pelo Pix - GILSON FERNANDES DA SILVA BARROS - •••.363.521-•• - CAIXA ECONOMICA FEDERAL (0104) Agência: 4038 Conta: 1288000000773429934-3
                31/07/2023,-3682.10,64c7a658-74ef-4844-9ed3-5939d6195b06,Pagamento de fatura
                31/07/2023,-119.90,64c7a716-dd2f-499c-808b-fbc8d5e32857,Pagamento de boleto efetuado - NET RUBI SERVICOS DE TECNOLOGI
                31/07/2023,-159.00,64c82f26-893f-4b1c-b841-eb9889063db1,Compra no débito - Topper Burger
                31/07/2023,-200.00,64c839a0-a9ff-4296-be9e-6402ac0505a2,Transferência enviada pelo Pix - Alexsander Luiz Ayres Pontes - •••.930.838-•• - NU PAGAMENTOS - IP (0260) Agência: 1 Conta: 95024762-9
                31/07/2023,200.00,64c83bf2-6459-4c9a-abb5-43a35c29687e,Resgate RDB
                
                """,
                BSDate = 01032023,

            };
            var payload = JsonSerializer.Serialize(bankstatment);
            var content = new StringContent(payload, Encoding.UTF8, MediaTypeNames.Application.Json);

            // Act

            var response = await client.PostAsync("/bankstatements", content);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
                                                // Add more assertions as needed based on your API response.
        }

        [Fact]

        public async Task BankstatementEndpoint_ServerSidedError()
        {
            // Arrange
            var client = _factory.CreateClient();
            var bankstatment = new BankStatementMessageObject
            {
                BSFormatType = "nubank",
                BSContent = """
                Data,Valor,Identificador,Descriço
                01/07/2023,-2.00,64a03490-9a14-4167-87d7-0da6c9fed56e,Compra no débito - Imperio Paes e Doces
                01/07/2023,-32.29,64a0847a-5b01-4065-be9f-61b2c031a0b1,Compra no débito - Acai da Barra
                01/07/2023,-157.89,64a0a76d-248e-47d4-afe1-d6a2c5f91a29,Compra no débito via NuPay - iFood
                02/07/2023,-80.00,64a16e22-2cdc-42cd-809c-6b212a48c57a,Compra no débito - Perene Cafe Bistro
                02/07/2023,-61.63,64a1c2fe-ac85-487a-b5ec-750b8de3bdc9,Transferência enviada pelo Pix - CLARO - 40.432.544/0001-47 - ITAÚ UNIBANCO S.A. (0341) Agência: 911 Conta: 6206-5
                02/07/2023,-61.66,64a1c30c-ebad-4b42-ae85-54f1e7151e52,Transferência enviada pelo Pix - CLARO - 40.432.544/0001-47 - ITAÚ UNIBANCO S.A. (0341) Agência: 911 Conta: 6206-5
                02/07/2023,-65.40,64a1cd87-1a58-4a1a-b392-4649bf75efb9,Compra no débito - Hc e Alimentos
                02/07/2023,-130.00,64a1d6f7-634d-4630-9cf8-c0ab7bc2d0cf,Transferência enviada pelo Pix - PAYU BRASIL - 08.965.639/0001-13 - ITAÚ UNIBANCO S.A. (0341) Agência: 145 Conta: 83551-8
                02/07/2023,-9.90,64a1d761-f244-4ec1-9161-6007e1939e45,Transferência enviada pelo Pix - PAYU BRASIL - 08.965.639/0001-13 - ITAÚ UNIBANCO S.A. (0341) Agência: 145 Conta: 83551-8
                02/07/2023,-38.10,64a22fd9-6b75-4812-964d-bd9061e37796,Compra no débito via NuPay - iFood
                03/07/2023,-2.00,64a2c33b-dd10-4e0c-a2d1-37985c85ded6,Compra no débito - Imperio Paes e Doces
                
                """,
                BSDate = 01032023,

            };
            var payload = JsonSerializer.Serialize(bankstatment);
            var content = new StringContent(payload, Encoding.UTF8, MediaTypeNames.Application.Json);

            // Act

            var response = await client.PostAsync("/bankstatements", content);

            // Assert

            Assert.Equal(HttpStatusCode.BadGateway, response.StatusCode);
        }

        [Fact]

        public async Task CheckingBankstatementsEndpointStatusCodeResponse_WhenSendingABankstatementOfABankThatIsNotYetImplemented_ExpectingStatusCode422()
        {
            // Arrange
            var client = _factory.CreateClient();
            var bankstatment = new BankStatementMessageObject
            {
                BSFormatType = "sicoob",
                BSContent = """
      
                
                """,
                BSDate = 01032023,

            };
            var payload = JsonSerializer.Serialize(bankstatment);
            var content = new StringContent(payload, Encoding.UTF8, MediaTypeNames.Application.Json);

            // Act

            var response = await client.PostAsync("/bankstatements", content);

            // Assert

            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
        }

        [Fact]

        public async Task CheckingBankstatementsEndpointStatusCodeResponse_WhenSendingABankstatementOfABankThatWontBeImplemented_ExpectingStatusCode400()
        {
            // Arrange
            var client = _factory.CreateClient();
            var bankstatment = new BankStatementMessageObject
            {
                BSFormatType = "calamitybank",
                BSContent = """
      
                
                """,
                BSDate = 01032023,

            };
            var payload = JsonSerializer.Serialize(bankstatment);
            var content = new StringContent(payload, Encoding.UTF8, MediaTypeNames.Application.Json);

            // Act

            var response = await client.PostAsync("/bankstatements", content);

            // Assert

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]

        public async Task CheckingCategoriesEndpointStatusCodeResponde_WhenSendingARequest_ExpectingTheCompleteCategoriesList()
        {
            // Arrange
            var client = _factory.CreateClient();
            string expectedList = """["Uncategorized","Housing","Pets","MarketEssentials","DrugstoreExpenses","MentalHealth","PhysicalHealth","HappyHours","UnusualExpenses"]""";


            // Act

            var response = await client.GetAsync("/categories");
            var content = response.Content.ReadAsStringAsync();
            var endPointMessage = content.Result;


            // Assert

            Assert.Equal(expectedList, endPointMessage);
        }
    }

}
