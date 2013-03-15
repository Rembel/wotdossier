using System;
using System.Globalization;
using System.Threading;

namespace WotDossier.Common
{
    /// <summary>
    /// Represents <see cref="DataFormatter"/> class for formatting data.
    /// </summary>
    public static class DataFormatter
    {
        private const string NEW_LINE_TAG = "<br/>";

        public const string FORMAT_MONEY = "{0:£#,0.00}";
        public const string FORMAT_1_DECIMALS = "{0:#,0.0}";
        public const string FORMAT_2_DECIMALS = "{0:#,0.00}";
        public const string FORMAT_3_DECIMALS = "{0:#,0.000}";
        public const string FORMAT_4_DECIMALS = "{0:#,0.0000}";
        public const string FORMAT_DATE = "dd/MM/yyyy";
        public const string FORMAT_DAY_TIME = "hh:mm tt";
        public const string FORMAT_TIME_SPAN = "HH:mm";
        public const string AM_DESIGNATOR = "a.m.";
        public const string PM_DESIGNATOR = "p.m.";

        private const string FORMAT_DATE_TIME = "G";
        private const string DATE_SEPARATOR = "/";
        private const string FORMAT_NUMERIC = "N";
        private const string FORMAT_NUMERIC_FULL = "{0:" + FORMAT_NUMERIC + "}";
        private const string DECIMAL_SEPARATOR = ".";
        public const string GROUP_SEPARATOR = ",";
        private const Int16 NUMBER_DECIMAL_DIGITS_IN_DECIMAL = 4;
        private const Int16 NUMBER_DECIMAL_DIGITS_IN_NUMERIC = 0;


        private const string REGEX_DECIMALS_COUNT = @"\.(?<decimals>0+)}";
        private const string REGEX_DECIMALS_GROUP = "decimals";

        private static NumberFormatInfo GetNumericFormatInfo()
        {
            var numericFormatInfo = (NumberFormatInfo)Thread.CurrentThread.CurrentCulture.NumberFormat.Clone();
            numericFormatInfo.CurrencyDecimalSeparator = DECIMAL_SEPARATOR;
            numericFormatInfo.CurrencyGroupSeparator = GROUP_SEPARATOR;
            numericFormatInfo.CurrencyDecimalDigits = NUMBER_DECIMAL_DIGITS_IN_NUMERIC;
            numericFormatInfo.NumberDecimalDigits = NUMBER_DECIMAL_DIGITS_IN_NUMERIC;
            numericFormatInfo.NumberGroupSeparator = GROUP_SEPARATOR;
            numericFormatInfo.NumberDecimalSeparator = DECIMAL_SEPARATOR;
            return numericFormatInfo;
        }

        private static NumberFormatInfo GetDecimalFormatInfo()
        {
            var decimalFormatInfo = (NumberFormatInfo)Thread.CurrentThread.CurrentCulture.NumberFormat.Clone();
            decimalFormatInfo.CurrencyDecimalSeparator = DECIMAL_SEPARATOR;
            decimalFormatInfo.CurrencyGroupSeparator = GROUP_SEPARATOR;
            decimalFormatInfo.CurrencyDecimalDigits = NUMBER_DECIMAL_DIGITS_IN_DECIMAL;
            decimalFormatInfo.NumberDecimalSeparator = DECIMAL_SEPARATOR;
            decimalFormatInfo.NumberGroupSeparator = GROUP_SEPARATOR;
            decimalFormatInfo.NumberDecimalDigits = NUMBER_DECIMAL_DIGITS_IN_DECIMAL;
            return decimalFormatInfo;
        }

        private static DateTimeFormatInfo GetDateTimeFormatInfo()
        {
            var datetimeFormatInfo = (DateTimeFormatInfo)Thread.CurrentThread.CurrentCulture.DateTimeFormat.Clone();
            datetimeFormatInfo.DateSeparator = DATE_SEPARATOR;
            datetimeFormatInfo.AMDesignator = AM_DESIGNATOR;
            datetimeFormatInfo.PMDesignator = PM_DESIGNATOR;
            return datetimeFormatInfo;
        }

        #region [Format methods]

        //public static string FormatDecimal(object obj, string defaultFormat)
        //{
        //    decimal? value = null;
        //    string format = defaultFormat ?? FORMAT_2_DECIMALS;
        //    if (obj is decimal)
        //    {
        //        Regex rx = new Regex(REGEX_DECIMALS_COUNT,
        //                  RegexOptions.Compiled | RegexOptions.IgnoreCase);

        //        Match match = rx.Match(format);
        //        if (match.Success)
        //        {
        //            value = BaseCalculationHelper.Round((decimal)obj, match.Groups[REGEX_DECIMALS_GROUP].Length);
        //        }
        //        else
        //        {
        //            value = BaseCalculationHelper.Round((decimal)obj, NUMBER_DECIMAL_DIGITS_IN_DECIMAL);
        //        }
        //    }
        //    return string.Format(GetDecimalFormatInfo(), format, value ?? obj);
        //}

        /// <summary>
        /// Formats time of date values
        /// </summary>
        //public static string FormatTimeOfDay(TimeSpan? timeOfDay, bool isDuration)
        //{
        //    string result = string.Empty;

        //    if (timeOfDay.HasValue)
        //    {
        //        result = new DateTime(timeOfDay.Value.Ticks).ToString(isDuration ? FORMAT_TIME_SPAN : FORMAT_DAY_TIME, GetDateTimeFormatInfo());
        //    }

        //    return result;
        //}

        /// <summary>
        /// Formats time of date values
        /// </summary>
        public static string FormatTimeOfDay(object obj, bool isDuration)
        {
            DateTime? dt = obj as DateTime?;
            if (dt == null)
            {
                return string.Empty;
            }

            return new DateTime(dt.Value.Ticks).ToString(isDuration ? FORMAT_TIME_SPAN : FORMAT_DAY_TIME, GetDateTimeFormatInfo());
        }

        /// <summary>
        /// Formats Date values
        /// </summary>
        public static string FormatDate(object obj, string defaultFormat)
        {
            return FormatDateTime(obj, string.IsNullOrEmpty(defaultFormat) ? FORMAT_DATE : defaultFormat);
        }

        /// <summary>
        /// Formats Date and time values
        /// </summary>
        public static string FormatDateTime(object obj, string defaultFormat)
        {
            DateTime? dt = obj as DateTime?;
            if (dt == null)
            {
                return string.Empty;
            }
            return dt.Value.ToString(string.IsNullOrEmpty(defaultFormat) ? FORMAT_DATE_TIME : defaultFormat, GetDateTimeFormatInfo());
        }

        public static string FormatNumeric(object obj, string defaultFormat)
        {
            if (!string.IsNullOrEmpty(defaultFormat))
            {
                return string.Format(defaultFormat, obj);
            }
            return string.Format(GetNumericFormatInfo(), FORMAT_NUMERIC_FULL, obj);
        }

        #endregion

        /// <summary>
        /// Format the given default message string resolving any 
        /// agrument placeholders found in them.
        /// </summary>
        /// <param name="msg">The message to format.</param>
        /// <param name="args">The array of agruments that will be filled in for parameter
        /// placeholders within the message, or null if none.</param>
        /// <returns>The formatted message (with resolved arguments)</returns>
        public static string FormatString(string msg, object[] args)
        {
            if (msg == null || ((args == null || args.Length == 0)))
            {
                return msg;
            }
            return string.Format(msg, args);
        }

        #region [Parse methods]

        private static object ParseNumber(string value)
        {
            int v;
            bool success = int.TryParse(value, NumberStyles.AllowThousands, GetNumericFormatInfo(), out v);
            if (success)
            {
                return v;
            }
            return 0;
        }

        private static object ParseLong(string value)
        {
            long v;
            bool success = long.TryParse(value, NumberStyles.AllowThousands, GetNumericFormatInfo(), out v);
            if (success)
            {
                return v;
            }
            return 0L;
        }

        private static object ParseDecimal(string value)
        {
            decimal v;
            bool success = decimal.TryParse(value, NumberStyles.Any, GetDecimalFormatInfo(), out v);
            if (success)
            {
                return v;
            }
            return 0.0m;
        }

        private static object ParseDouble(string value)
        {
            double v;
            bool success = double.TryParse(value, NumberStyles.Any, GetDecimalFormatInfo(), out v);
            if (success)
            {
                return v;
            }
            return 0d;
        }

        private static object ParseBoolean(string value)
        {
            bool v;
            bool success = bool.TryParse(value, out v);
            if (success)
            {
                return v;
            }
            return false;
        }

        private static object ParseDateTime(string value)
        {
            DateTime v;
            bool success = DateTime.TryParse(value, GetDateTimeFormatInfo(), DateTimeStyles.AssumeLocal, out v);
            if (success)
            {
                return v;
            }
            return DateTime.Now;
        }
        #endregion

        /// <summary>
        /// Converts the System.string representation of a number to its System.Decimal equivalent using the specified culture-specific format information.
        /// </summary>
        /// <typeparam name="T">Type of convertion</typeparam>
        /// <param name="value">Value to convert</param>
        /// <param name="type">Type of convertion</param>
        /// <returns>The number equivalent to the number contained in s as specified by provider.</returns>
        public static T Parse<T>(string value, Type type)
            where T : new()
        {
            if (string.IsNullOrEmpty(value))
            {
                return new T();
            }
            if (type == typeof(decimal) || type == typeof(decimal?))
            {
                return (T)ParseDecimal(value);
            }
            if (type == typeof(double) || type == typeof(double?))
            {
                return (T)ParseDouble(value);
            }
            if (type == typeof(int) || type == typeof(int?))
            {
                return (T)ParseNumber(value);
            }
            if (type == typeof(long) || type == typeof(long?))
            {
                return (T)ParseLong(value);
            }
            if (type == typeof(DateTime) || type == typeof(DateTime?))
            {
                return (T)ParseDateTime(value);
            }
            if (type == typeof(bool) || type == typeof(bool?))
            {
                return (T)ParseBoolean(value);
            }
            if (type.IsEnum)
            {
                return (T)Enum.Parse(type, value);
            }
            return (T)Convert.ChangeType(value, typeof(T), CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Returns string with all new lines replaced with <br /> tag
        /// </summary>
        /// <param name="str">string</param>
        /// <returns>string</returns>
        public static string ReplaceNewLinesWithBr(string str)
        {
            string result = str;

            if (!string.IsNullOrEmpty(str))
            {
                result = str.Replace(Environment.NewLine, NEW_LINE_TAG);
            }

            return result;
        }
    }
}
