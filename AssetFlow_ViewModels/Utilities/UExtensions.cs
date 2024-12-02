using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_ViewModels.Utilities
{
    public static class UExtensions
    {
        public static string JoinString(this string[] data)
        {
            if (data != null && data.Length > 0)
            {
                return string.Join(", ", data);
            }
            else return "";
        }

        public static string JoinString(this List<string> data, bool addQuote)
        {
            if (data != null && data.Count > 0)
            {
                if (addQuote)
                    return String.Join(", ", data.Select(r => "'" + r + "'").ToList());
                else
                    return string.Join(", ", data);
            }
            else return addQuote ? "''" : "";
        }

        public static List<DateTime> RangeDays(this DateTime startDate, DateTime endDate)
        {
            TimeSpan span = endDate.Subtract(startDate);

            return Enumerable.Range(0, (int)span.TotalDays + 1).Select(d => startDate.AddDays(d)).ToList();
        }

        public static List<DateTime> RangeMonths(this DateTime startDate, DateTime endDate)
        {
            int monthsApart = 12 * (startDate.Year - endDate.Year) + startDate.Month - endDate.Month;
            return Enumerable.Range(0, Math.Abs(monthsApart) + 1).Select(d => startDate.AddMonths(d)).ToList();
        }

        public static string ToTitleCase(this string data)
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            return textInfo.ToTitleCase(string.IsNullOrEmpty(data) ? "" : data.ToLower());
        }

        /// <summary>
        /// Joins list of strings as a new line for database field
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string JoinStringNewLine(this List<string> data)
        {
            if (data == null) return "";
            else return JoinStringNewLine(data.ToArray());
        }

        /// <summary>
        /// Joins array of strings as a new line for database field
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string JoinStringNewLine(this string[] data)
        {
            if (data != null && data.Length > 0)
            {
                return string.Join("\n", data).TrimEnd('\n');
            }
            else return "";
        }

        public static void TryAdd(this List<string> data, string addData)
        {
            if(data != null)
            {
                if (!data.Contains(addData))
                    data.Add(addData);
            }
        }
    }
}
