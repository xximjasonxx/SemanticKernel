using System.ComponentModel;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using TransactionChecker.Models;

namespace TransactionChecker.Plugins
{
    public sealed class CosmosPlugin
    {
        private readonly IConfiguration _configuration;

        public CosmosPlugin(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [KernelFunction("transactionsForMerchantInDateRange")]
        [Description("Get transactions for a merchant in a date range")]
        public async Task<List<Transaction>> TransactionsByMerchantInDateRange(
            [Description("The merchant to filter by")] string merchant,
            [Description("The start date for the transaction query")] string startDateString,
            [Description("The end date for the transaction query")] string endDateString)
        {
            var startDate = DateTime.Parse(startDateString);
            var endDate = DateTime.Parse(endDateString);

            return await QueryTransactions(merchant, "Merchant", startDate, endDate);
        }

        [KernelFunction("transactionsForCategoryInDateRange")]
        [Description("Get transactions for a catetgory in a date range")]
        public async Task<List<Transaction>> TransactionsByCategoryInDateRange(
            [Description("The category to filter by")] string merchant,
            [Description("The start date for the transaction query")] string startDateString,
            [Description("The end date for the transaction query")] string endDateString)
        {
            var startDate = DateTime.Parse(startDateString);
            var endDate = DateTime.Parse(endDateString);

            return await QueryTransactions(merchant, "Category", startDate, endDate);
        }

        [KernelFunction("transactionsForDateRange")]
        [Description("Get transactions for a date range")]
        public async Task<List<Transaction>> TransactionsInDateRange(
            [Description("The start date for the transaction query")] string startDateString,
            [Description("The end date for the transaction query")] string endDateString)
        {
            var startDate = DateTime.Parse(startDateString);
            var endDate = DateTime.Parse(endDateString);

            return await QueryTransactions(string.Empty, string.Empty, startDate, endDate);
        }

        async Task<List<Transaction>> QueryTransactions(string filterValue, string filterFieldName, DateTime startDate, DateTime endDate)
        {
            var cosmosClient = new CosmosClient(_configuration["CosmosConnectionString"]);
            var database = cosmosClient.GetDatabase("transactions");
            var container = database.GetContainer("generatedTransactions");

            var startDateIsoString = startDate.ToString("o");
            var endDateIsoString = endDate.ToString("o");
            var query = string.IsNullOrEmpty(filterValue) == false
                ? new QueryDefinition($"SELECT * FROM c WHERE LOWER(c.{filterFieldName}) = @filterValue AND c.Date >= @startDate AND c.Date <= @endDate")
                    .WithParameter("@filterValue", filterValue.ToLower())
                    .WithParameter("@startDate", startDateIsoString)
                    .WithParameter("@endDate", endDateIsoString)
                : new QueryDefinition("SELECT * FROM c WHERE c.Date >= @startDate AND c.Date <= @endDate")
                    .WithParameter("@startDate", startDateIsoString)
                    .WithParameter("@endDate", endDateIsoString);

            var queryIterator = container.GetItemQueryIterator<Transaction>(query);
            var results = new List<Transaction>();

            while (queryIterator.HasMoreResults)
            {
                FeedResponse<Transaction> response = await queryIterator.ReadNextAsync();
                results.AddRange(response);
            }

            return results;
        }
    }
}