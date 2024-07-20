using System.ComponentModel;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;

namespace TransactionChecker.Plugins
{
    public sealed class CosmosPlugin
    {
        private readonly IConfiguration _configuration;

        public CosmosPlugin(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [KernelFunction("byCategoryInDateRange")]
        [Description("Get sum of amount for transactions by category for a date range")]
        public async Task<decimal> ByCategoryInDateRange(
            [Description("The category to filter by")]string category,
            [Description("The target month year to query by")]string targetMonthYear)
        {
            var startDate = DateTime.Parse(targetMonthYear);
            var endDate = startDate.AddMonths(1).AddDays(-1);

            var cosmosClient = new CosmosClient(_configuration["CosmosConnectionString"]);
            var database = cosmosClient.GetDatabase("transactions");
            var container = database.GetContainer("byCategory");

            var query = new QueryDefinition("SELECT VALUE SUM(c.Amount) FROM c WHERE c.Category = @category AND c.Date >= @startDate AND c.Date <= @endDate")
                .WithParameter("@category", category)
                .WithParameter("@startDate", startDate)
                .WithParameter("@endDate", endDate);

            var queryIterator = container.GetItemQueryIterator<decimal>(query);
            if (queryIterator.HasMoreResults)
            {
                var response = await queryIterator.ReadNextAsync();
                return Math.Abs(response.Resource.First());
            }

            return decimal.Zero;
        }

        [KernelFunction("byMerchantInDateRange")]
        [Description("Get sum of amount for transactions by category for a date range")]
        public async Task<decimal> ByMerchantInDateRange(
            [Description("The merchant to filter by")]string merchant,
            [Description("The target month year to query by")]string targetMonthYear)
        {
            var startDate = DateTime.Parse(targetMonthYear);
            var endDate = startDate.AddMonths(1).AddDays(-1);
            
            var cosmosClient = new CosmosClient(_configuration["CosmosConnectionString"]);
            var database = cosmosClient.GetDatabase("transactions");
            var container = database.GetContainer("byCategory");

            var query = new QueryDefinition("SELECT VALUE SUM(c.Amount) FROM c WHERE c.Merchant = @merchant AND c.Date >= @startDate AND c.Date <= @endDate")
                .WithParameter("@merchant", merchant)
                .WithParameter("@startDate", startDate)
                .WithParameter("@endDate", endDate);

            var queryIterator = container.GetItemQueryIterator<decimal>(query);
            if (queryIterator.HasMoreResults)
            {
                var response = await queryIterator.ReadNextAsync();
                return Math.Abs(response.Resource.First());
            }

            return decimal.Zero;
        }
    }
}