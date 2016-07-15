using System;

namespace NSUtils
{
    public static class ExtensionMethodsDate
    {
        public static bool IsThisDate(this DateTime actualDate, DateTime compareDate)
        {
            return actualDate.Day == compareDate.Day && actualDate.Month == compareDate.Month && actualDate.Year == compareDate.Year;
        }

        public static string ToShortDateString(this DateTime date)
        {
            return string.Format("{0}/{1}/{2}", date.Day, date.Month, date.Year);
        }

        public static DateTime ToShortDate(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day);
        }
    }
}
