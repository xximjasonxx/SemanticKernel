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

        [KernelFunction("GetStartDateForMonthName")]
        [Description("Get start date for month name")]
        public string GetStartDateForMonthName([Description("The month name to be converted")]string monthName)
        {
            var dateString = $"1 {monthName} {DateTime.Now.Year}";
            var dt = DateTime.Parse(dateString);

            if (dt > DateTime.Now)
            {
                dt = dt.AddYears(-1);
            }

            return dt.ToString("yyyy-MM-dd");
        }

        [KernelFunction("GetEndDateForMonthName")]
        [Description("Get end date for month name")]
        public string GetEndDateForMonthName([Description("The month name to be converted")] string monthName)
        {
            var dateString = $"1 {monthName} {DateTime.Now.Year}";
            var dt = DateTime.Parse(dateString);

            if (dt > DateTime.Now)
            {
                dt = dt.AddYears(-1);
            }

            return dt.AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
        }

        [KernelFunction("GetStartDateForYear")]
        [Description("Given a 4 digit year string return the first day of the year in string format")]
        public string GetStartDateForYear([Description("The year to be converted")] string year)
        {
            var dt = DateTime.Parse($"1 January {year}");
            return dt.ToString("yyyy-MM-dd");
        }

        [KernelFunction("GetEndDateForYear")]
        [Description("Given a 4 digit year string return the last day of the year in string format")]
        public string GetEndDateForYear([Description("The year to be converted")] string year)
        {
            var dt = DateTime.Parse($"31 December {year}");
            return dt.ToString("yyyy-MM-dd");
        }
    }
}