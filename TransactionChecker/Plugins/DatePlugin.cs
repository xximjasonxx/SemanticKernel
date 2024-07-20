using System.ComponentModel;
using Microsoft.SemanticKernel;

namespace TransactionChecker.Plugins
{
    public class DatePlugin
    {
        [KernelFunction("GetLastYear")]
        [Description("Get the last month")]
        public string GetLastMonth()
        {
            var dt = DateTime.Now.AddMonths(-1);
            return dt.ToString("MMMM yyyy");
        }
    }
}