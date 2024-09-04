using System.ComponentModel;
using Microsoft.SemanticKernel;

namespace TransactionChecker.Plugins
{
    public sealed class DatePlugin
    {
        [KernelFunction("GetStartDateForMonthYearString")]
        [Description("Convert a date string in the format MMMM yyyy to a string of the months start date")]
        public string GetStartDateForMonthYearString([Description("The date string to be converted")]string dateString)
        {
            var dt = DateTime.Parse(dateString);
            return dt.ToString("yyyy-MM-01");
        }

        [KernelFunction("GetEndDateForMonthYearString")]
        [Description("Convert a date string in the format MMMM yyyy to a string of the months end date")]
        public string GetEndDateForMonthYearString([Description("The date string to be converted")]string dateString)
        {
            var dt = DateTime.Parse(dateString);
            var lastDay = DateTime.DaysInMonth(dt.Year, dt.Month);
            return dt.ToString($"yyyy-MM-{lastDay}");
        }
    }
}