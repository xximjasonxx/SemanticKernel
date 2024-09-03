using Microsoft.SemanticKernel;
using System.ComponentModel;
using TransactionChecker.Models;

namespace TransactionChecker.Plugins
{
    public class TransactionsPlugin
    {
        [KernelFunction("SumTransactions")]
        [Description("Get the total amount represented by a set of transactions")]
        public decimal SumTransactions([Description("The transactions to sum")] List<Transaction> transactions)
        {
            return transactions.Sum(t => t.Amount);
        }
    }
}
