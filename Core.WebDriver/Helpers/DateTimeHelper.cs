using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Taf.UI.Core.Helpers
{
    public class DateTimeHelper
    {
        public static string MonthName(DateTime date)
        {
            CultureInfo ci = new CultureInfo("en-GB");

            return date.ToString("MMMM", ci);
        }

        public static int MonthNumberByShortName(string shortMonthName)
        {
            Dictionary<string, int> monthNumber = new Dictionary<string, int>
            {
                { "Jan", 1},
                { "Feb", 2},
                { "Mar", 3},
                { "Apr", 4},
                { "May", 5},
                { "Jun", 6},
                { "Jul", 7},
                { "Aug", 8},
                { "Sep", 9},
                { "Oct", 10},
                { "Nov", 11},
                { "Dec", 12}
            };

            return monthNumber.Keys.Contains(shortMonthName)
                ? monthNumber[shortMonthName]
                : -1;
        }

        public static int GetMonthNumber(string shortMonthNameAndYear)
        {
            string[] monthYear = shortMonthNameAndYear.Split(" ");

            return monthYear.Length > 0
                ? MonthNumberByShortName(monthYear[0])
                : -1;
        }

        public static int GetYear(string shortMonthNameAndYear)
        {
            string[] monthYear = shortMonthNameAndYear.Split(" ");

            string year = monthYear.Length > 1 ? monthYear[1]: "-1";

            return int.TryParse(year, out int result)
                ? result
                : -1;
        }
    }
}
