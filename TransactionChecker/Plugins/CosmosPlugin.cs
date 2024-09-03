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
            [Description("The start date for the transaction query")] string startDate,
            [Description("The end date for the transaction query")] string endDate)
        {
            var cosmosClient = new CosmosClient(_configuration["CosmosConnectionString"]);
            var database = cosmosClient.GetDatabase("transactions");
            var container = database.GetContainer("byCategory");

            var query = new QueryDefinition("SELECT * FROM c WHERE LOWER(c.Merchant) = @merchant AND c.Date >= @startDate AND c.Date <= @endDate")
                .WithParameter("@merchant", merchant.ToLower())
                .WithParameter("@startDate", startDate)
                .WithParameter("@endDate", endDate);

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