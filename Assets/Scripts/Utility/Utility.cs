namespace Cocktailor
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using UnityEngine;
    
    public class Utility
    {
        private const string format = "yyyy/MM/dd HH:mm:ss";
        private static IFormatProvider provider;
        private static readonly CultureInfo cultureInfo = CultureInfo.CurrentCulture;
        
        public static string DateTimeToString(DateTime dateTime)
        {
            return dateTime.ToString(format);
        }

        public static DateTime StringToDateTime(string dateTimeString)
        {
            DateTime dateTime;
            var success = DateTime.TryParseExact(dateTimeString, format, provider, DateTimeStyles.None, out dateTime);

            if (success) return dateTime;
            return DateTime.Now;
        }
    }
}