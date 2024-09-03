using System.ComponentModel;
using Microsoft.SemanticKernel;

namespace TransactionChecker.Plugins
{
    public class DatePlugin
    {
        [KernelFunction, Description("Get the start date of a date string in the format MMMM yyyy")]
        public string GetStartDateForMonthYearString([Description("The date in string format")]string dateString)
        {
            var dt = DateTime.Parse(dateString);
            return dt.ToString("yyyy-MM-01");
        }

        [KernelFunction, Description("Get the end date of a date string in the format MMMM yyyy")]
        public string GetEndDateForMonthYearString([Description("The date in string format")]string dateString)
        {
            var dt = DateTime.Parse(dateString);
            var lastDay = DateTime.DaysInMonth(dt.Year, dt.Month);
            return dt.ToString($"yyyy-MM-{lastDay}");
        }







        /*[KernelFunction("GetLastMonth")]
        [Description("Get the last month")]
        public string GetLastMonth()
        {
            var dt = DateTime.Now.AddMonths(-1);
            return dt.ToString("MMMM yyyy");
        }

        [KernelFunction("GetMonthsAgo")]
        [Description("Get last n months ago")]
        public string GetMonthsAgo([Description("how many months ago")]int months)
        {
            var dt = DateTime.Now.AddMonths(-months);
            return dt.ToString("MMMM yyyy");
        }

        [KernelFunction("GetMonth")]
        [Description("Get a date in the format MMMM, yyyy when only the month name is provided without the year")]
        public string GetMonth([Description("The month name")]string monthName)
        {
            // pick a day we know will always exist
            var date = $"1 {monthName} {DateTime.Now.Year}";

            // parse it to a datetime
            var dt = DateTime.Parse(date);

            // if the date would occur in the future, assume the user means the previous
            // occurrence of the month
            if (dt > DateTime.Now)
                dt = dt.AddYears(-1);

            return dt.ToString("MMMM yyyy");
        }

        [KernelFunction("GetDateTime")]
        [Description("Convert a date in string format and to a DateTime")]
        public DateTime GetDateTime([Description("The date in string format")]string date)
        {
            return DateTime.Parse(date);
        }*/
    }
}