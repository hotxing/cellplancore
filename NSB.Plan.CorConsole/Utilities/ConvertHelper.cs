using System;
using System.Globalization;
using System.Text;
using System.Threading;

namespace LengZX.SharePart.Utilities
{
    public sealed class ConvertHelper
    {
        #region 补足位数

        /// <summary>
        /// 指定字符串的固定长度，如果字符串小于固定长度，
        /// 则在字符串的前面补足零，可设置的固定长度最大为9位
        /// </summary>
        /// <param name="text">原始字符串</param>
        /// <param name="limitedLength">字符串的固定长度</param>
        public static string RepairZero(string text, int limitedLength)
        {
            //补足0的字符串
            string temp = "";

            //补足0
            for (int i = 0; i < limitedLength - text.Length; i++)
            {
                temp += "0";
            }

            //连接text
            temp += text;

            //返回补足0的字符串
            return temp;
        }

        #endregion 补足位数

        #region 各进制数间转换

        /// <summary>
        /// 实现各进制数间的转换。ConvertBase("15",10,16)表示将十进制数15转换为16进制的数。
        /// </summary>
        /// <param name="value">要转换的值,即原值</param>
        /// <param name="from">原值的进制,只能是2,8,10,16四个值。</param>
        /// <param name="to">要转换到的目标进制，只能是2,8,10,16四个值。</param>
        public static string ConvertBase(string value, int from, int to)
        {
            try
            {
                int intValue = Convert.ToInt32(value, from); //先转成10进制
                string result = Convert.ToString(intValue, to); //再转成目标进制
                if (to == 2)
                {
                    int resultLength = result.Length; //获取二进制的长度
                    switch (resultLength)
                    {
                        case 7:
                            result = "0" + result;
                            break;

                        case 6:
                            result = "00" + result;
                            break;

                        case 5:
                            result = "000" + result;
                            break;

                        case 4:
                            result = "0000" + result;
                            break;

                        case 3:
                            result = "00000" + result;
                            break;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                //LogHelper.WriteTraceLog(TraceLogLevel.Error, ex.Message);
                return "0";
            }
        }

        #endregion 各进制数间转换

        #region 使用指定字符集将string转换成byte[]

        /// <summary>
        /// 使用指定字符集将string转换成byte[]
        /// </summary>
        /// <param name="text">要转换的字符串</param>
        /// <param name="encoding">字符编码</param>
        public static byte[] StringToBytes(string text, Encoding encoding)
        {
            return encoding.GetBytes(text);
        }

        #endregion 使用指定字符集将string转换成byte[]

        #region 使用指定字符集将byte[]转换成string

        /// <summary>
        /// 使用指定字符集将byte[]转换成string
        /// </summary>
        /// <param name="bytes">要转换的字节数组</param>
        /// <param name="encoding">字符编码</param>
        public static string BytesToString(byte[] bytes, Encoding encoding)
        {
            return encoding.GetString(bytes);
        }

        #endregion 使用指定字符集将byte[]转换成string

        #region 将byte[]转换成int

        /// <summary>
        /// 将byte[]转换成int
        /// </summary>
        /// <param name="data">需要转换成整数的byte数组</param>
        public static int BytesToInt32(byte[] data)
        {
            //如果传入的字节数组长度小于4,则返回0
            if (data.Length < 4)
            {
                return 0;
            }

            //定义要返回的整数
            int num = 0;

            //如果传入的字节数组长度大于4,需要进行处理
            if (data.Length >= 4)
            {
                //创建一个临时缓冲区
                byte[] tempBuffer = new byte[4];

                //将传入的字节数组的前4个字节复制到临时缓冲区
                Buffer.BlockCopy(data, 0, tempBuffer, 0, 4);

                //将临时缓冲区的值转换成整数，并赋给num
                num = BitConverter.ToInt32(tempBuffer, 0);
            }

            //返回整数
            return num;
        }

        #endregion 将byte[]转换成int

        #region Miscellaneous converting routines.

        // ------------------------------------------------------------------

        /// <summary>
        /// Toes the culture info.
        /// </summary>
        /// <param name="languageCode">The language code.</param>
        /// <returns></returns>
        public static CultureInfo ToCultureInfo(
            string languageCode)
        {
            return ToCultureInfo(
                languageCode,
                CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Toes the culture info.
        /// </summary>
        /// <param name="languageCode">The language code.</param>
        /// <param name="fallbackTo">The fallback to.</param>
        /// <returns></returns>
        public static CultureInfo ToCultureInfo(
            string languageCode,
            CultureInfo fallbackTo)
        {
            if (string.IsNullOrEmpty(languageCode) ||
                languageCode.Trim().Length < 2)
            {
                return fallbackTo;
            }
            else
            {
                string c4;
                string c2;

                if (languageCode.Length == 2)
                {
                    c2 = languageCode;
                    c4 = c2 + @"-" + c2;
                }
                else if (languageCode.Length == 4)
                {
                    c2 = languageCode.Substring(0, 2);
                    c4 = languageCode;
                }
                else
                {
                    c2 = languageCode.Substring(0, 2);
                    c4 = c2 + @"-" + c2;
                }

                try
                {
                    CultureInfo info = new CultureInfo(
                        c4);
                    return info;
                }
                catch (ArgumentException)
                {
                    try
                    {
                        // if languageCode 4 failed, try languageCode 2.
                        CultureInfo info = new CultureInfo(
                            c2);
                        return info;
                    }
                    catch (ArgumentException y)
                    {
                        //LogCentral.Current.LogWarn(
                        //    string.Format(
                        //    @"No suitable culture for language '{0}' found.",
                        //    languageCode),
                        //    y);

                        return fallbackTo;
                    }
                }
            }
        }

        /// <summary>
        /// Use this to convert a SQL-timestamp field to a printable
        /// string (e.g. for debugging purposes).
        /// </summary>
        /// <param name="buffer">The buffer to convert.</param>
        /// <returns>
        /// Returns the textual representation of the buffer
        /// or a NULL string if the buffer is NULL or a NULL
        /// if the buffer is empty.
        /// </returns>
        public static string ToString(
            byte[] buffer)
        {
            if (buffer == null)
            {
                return null;
            }
            else if (buffer.Length <= 0)
            {
                return null;
            }
            else
            {
                StringBuilder s = new StringBuilder();

                foreach (byte b in buffer)
                {
                    if (s.Length > 0)
                    {
                        s.Append(@"-");
                    }

                    s.Append(Convert.ToString(b));
                }

                return s.ToString();
            }
        }

        // ------------------------------------------------------------------

        #endregion Miscellaneous converting routines.

        #region Converting routines with default fallbacks.

        // ------------------------------------------------------------------

        /// <summary>
        /// Convert a string to a boolean, returns FALSE if fails.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <returns></returns>
        public static bool ToBoolean(
            object o)
        {
            return ToBoolean(o, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Convert a string to a boolean, returns FALSE if fails.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <param name="provider">The provider.</param>
        /// <returns></returns>
        public static bool ToBoolean(
            object o,
            IFormatProvider provider)
        {
            return ToBoolean(o, false, provider);
        }

        /// <summary>
        /// Convert a string to a date time, returns DateTime.MinValue if fails.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <returns></returns>
        public static DateTime ToDateTime(
            object o)
        {
            return ToDateTime(o, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Convert a string to a date time, returns DateTime.MinValue if fails.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <param name="provider">The provider.</param>
        /// <returns></returns>
        public static DateTime ToDateTime(
            object o,
            IFormatProvider provider)
        {
            return ToDateTime(o, DateTime.MinValue, provider);
        }

        /// <summary>
        /// Convert a string to a decimal, returns zero if fails.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <returns></returns>
        public static decimal ToDecimal(
            object o)
        {
            return ToDecimal(o, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Convert a string to a decimal, returns zero if fails.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <param name="provider">The provider.</param>
        /// <returns></returns>
        public static decimal ToDecimal(
            object o,
            IFormatProvider provider)
        {
            return ToDecimal(o, decimal.Zero, provider);
        }

        /// <summary>
        /// Convert a string to a double, returns 0.0 if fails.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <returns></returns>
        public static double ToDouble(
            object o)
        {
            return ToDouble(o, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Convert a string to a double, returns 0.0 if fails.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <param name="provider">The provider.</param>
        /// <returns></returns>
        public static double ToDouble(
            object o,
            IFormatProvider provider)
        {
            return ToDouble(o, 0.0, provider);
        }

        /// <summary>
        /// Toes the GUID.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <returns></returns>
        public static Guid ToGuid(
            object o)
        {
            return ToGuid(o, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Toes the GUID.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <param name="provider">The provider.</param>
        /// <returns></returns>
        public static Guid ToGuid(
            object o,
            IFormatProvider provider)
        {
            return ToGuid(o, Guid.Empty, provider);
        }

        /// <summary>
        /// Convert a string to an integer, returns 0 if fails.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <returns></returns>
        public static int ToInt32(
            object o)
        {
            return ToInt32(o, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Convert a string to an integer, returns 0 if fails.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <param name="provider">The provider.</param>
        /// <returns></returns>
        public static int ToInt32(
            object o,
            IFormatProvider provider)
        {
            return ToInt32(o, 0, provider);
        }

        /// <summary>
        /// Convert a string to an integer, returns 0 if fails.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <returns></returns>
        public static long ToInt64(
            object o)
        {
            return ToInt64(o, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Convert a string to an integer, returns 0 if fails.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <param name="provider">The provider.</param>
        /// <returns></returns>
        public static long ToInt64(
            object o,
            IFormatProvider provider)
        {
            return ToInt64(o, 0, provider);
        }

        /// <summary>
        /// Convert to a string.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <returns></returns>
        public static string ToString(
            object o)
        {
            return ToString(o, (string)null);
        }

        /// <summary>
        /// Convert to a string.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <param name="provider">The provider.</param>
        /// <returns></returns>
        public static string ToString(
            object o,
            IFormatProvider provider)
        {
            return ToString(o, null, provider);
        }

        /// <summary>
        /// Toes the T.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <returns></returns>
        public static T ToT<T>(
            object o)
        {
            return ToT<T>(o, default(T));
        }

        // ------------------------------------------------------------------

        #endregion Converting routines with default fallbacks.

        #region Converting routines with user-defined fallbacks.

        // ------------------------------------------------------------------

        /// <summary>
        /// Convert a string to a boolean, returns FALSE if fails.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <param name="fallbackTo">if set to <c>true</c> [fallback to].</param>
        /// <returns></returns>
        public static bool ToBoolean(
            object o,
            bool fallbackTo)
        {
            return ToBoolean(o, fallbackTo, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Convert a string to a boolean, returns FALSE if fails.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <param name="fallbackTo">if set to <c>true</c> [fallback to].</param>
        /// <param name="provider">The provider.</param>
        /// <returns></returns>
        public static bool ToBoolean(
            object o,
            bool fallbackTo,
            IFormatProvider provider)
        {
            if (o == null)
            {
                return fallbackTo;
            }
            // This is the fastest way, see
            else if (o.GetType() == typeof(bool))
            {
                return (bool)o;
            }
            else if (IsBoolean(o, provider))
            {
                try
                {
                    string s =
                        Convert.ToString(o, provider).Trim().ToLowerInvariant();

                    if (s.Length <= 0)
                    {
                        return fallbackTo;
                    }
                    else if (
                        string.Compare(s, bool.TrueString, true) == 0 ||
                        s == @"1" ||
                        s == @"-1")
                    {
                        return true;
                    }
                    else if (
                        string.Compare(s, bool.FalseString, true) == 0 ||
                        s == @"0")
                    {
                        return false;
                    }
                    else
                    {
                        return bool.Parse(Convert.ToString(o, provider));
                    }
                }
                catch (FormatException)
                {
                    return fallbackTo;
                }
            }
            else
            {
                return fallbackTo;
            }
        }

        /// <summary>
        /// Convert a string to a date time,
        /// returns DateTime.MinValue if fails.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <param name="fallbackTo">The fallback to.</param>
        /// <returns></returns>
        public static DateTime ToDateTime(
            object o,
            DateTime fallbackTo)
        {
            return ToDateTime(o, fallbackTo, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Convert a string to a date time,
        /// returns DateTime.MinValue if fails.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <param name="fallbackTo">The fallback to.</param>
        /// <param name="provider">The provider.</param>
        /// <returns></returns>
        public static DateTime ToDateTime(
            object o,
            DateTime fallbackTo,
            IFormatProvider provider)
        {
            if (o == null)
            {
                return fallbackTo;
            }
            // This is the fastest way, see
            else if (o.GetType() == typeof(DateTime))
            {
                return (DateTime)o;
            }
            else if (IsDateTime(o, provider))
            {
                return Convert.ToDateTime(o, provider);
            }
            else
            {
                return fallbackTo;
            }
        }

        /// <summary>
        /// Convert a string to a decimal, returns zero if fails.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <param name="fallbackTo">The fallback to.</param>
        /// <returns></returns>
        public static decimal ToDecimal(
            object o,
            decimal fallbackTo)
        {
            return ToDecimal(o, fallbackTo, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Convert a string to a decimal, returns zero if fails.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <param name="fallbackTo">The fallback to.</param>
        /// <param name="provider">The provider.</param>
        /// <returns></returns>
        public static decimal ToDecimal(
            object o,
            decimal fallbackTo,
            IFormatProvider provider)
        {
            if (o == null)
            {
                return fallbackTo;
            }
            // This is the fastest way, see
            else if (o.GetType() == typeof(decimal))
            {
                return (decimal)o;
            }
            else if (IsDecimal(o, provider))
            {
                return Convert.ToDecimal(o, provider);
            }
            else
            {
                return fallbackTo;
            }
        }

        /// <summary>
        /// Convert a string to a double, returns 0.0 if fails.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <param name="fallbackTo">The fallback to.</param>
        /// <returns></returns>
        public static double ToDouble(
            object o,
            double fallbackTo)
        {
            return ToDouble(o, fallbackTo, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Convert a string to a double, returns 0.0 if fails.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <param name="fallbackTo">The fallback to.</param>
        /// <param name="provider">The provider.</param>
        /// <returns></returns>
        public static double ToDouble(
            object o,
            double fallbackTo,
            IFormatProvider provider)
        {
            if (o == null)
            {
                return fallbackTo;
            }
            // This is the fastest way, see
            else if (o.GetType() == typeof(double))
            {
                return (double)o;
            }
            else if (IsFloat(o, provider))
            {
                return Convert.ToDouble(o, provider);
            }
            else
            {
                return fallbackTo;
            }
        }

        /// <summary>
        /// Toes the GUID.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <param name="fallbackTo">The fallback to.</param>
        /// <returns></returns>
        public static Guid ToGuid(
            object o,
            Guid fallbackTo)
        {
            return ToGuid(o, fallbackTo, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Toes the GUID.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <param name="fallbackTo">The fallback to.</param>
        /// <param name="provider">The provider.</param>
        /// <returns></returns>
        public static Guid ToGuid(
            object o,
            Guid fallbackTo,
            IFormatProvider provider)
        {
            if (o == null)
            {
                return fallbackTo;
            }
            // This is the fastest way, see
            else if (o.GetType() == typeof(Guid))
            {
                return (Guid)o;
            }
            else if (IsGuid(o, provider))
            {
                if (o is byte[])
                {
                    return new Guid(o as byte[]);
                }
                else
                {
                    return new Guid(Convert.ToString(o, provider));
                }
            }
            else
            {
                return fallbackTo;
            }
        }

        /// <summary>
        /// Convert a string to an integer, returns 0 if fails.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <param name="fallbackTo">The fallback to.</param>
        /// <returns></returns>
        public static int ToInt32(
            object o,
            int fallbackTo)
        {
            return ToInt32(o, fallbackTo, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Convert a string to an integer, returns 0 if fails.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <param name="fallbackTo">The fallback to.</param>
        /// <param name="provider">The provider.</param>
        /// <returns></returns>
        public static int ToInt32(
            object o,
            int fallbackTo,
            IFormatProvider provider)
        {
            if (o == null)
            {
                return fallbackTo;
            }
            // This is the fastest way, see
            else if (o.GetType() == typeof(int))
            {
                return (int)o;
            }
            else if (IsInteger(o, provider))
            {
                return Convert.ToInt32(o, provider);
            }
            else if (o is Enum)
            {
                return (int)o;
            }
            else
            {
                return fallbackTo;
            }
        }

        /// <summary>
        /// Convert a string to an integer, returns 0 if fails.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <param name="fallbackTo">The fallback to.</param>
        /// <returns></returns>
        public static long ToInt64(
            object o,
            long fallbackTo)
        {
            return ToInt64(o, fallbackTo, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Convert a string to an integer, returns 0 if fails.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <param name="fallbackTo">The fallback to.</param>
        /// <param name="provider">The provider.</param>
        /// <returns></returns>
        public static long ToInt64(
            object o,
            long fallbackTo,
            IFormatProvider provider)
        {
            if (o == null)
            {
                return fallbackTo;
            }
            // This is the fastest way, see
            else if (o.GetType() == typeof(long))
            {
                return (long)o;
            }
            else if (IsInt64(o, provider))
            {
                return Convert.ToInt64(o, provider);
            }
            else
            {
                return fallbackTo;
            }
        }

        /// <summary>
        /// Convert to a string.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <param name="fallbackTo">The fallback to.</param>
        /// <returns></returns>
        public static string ToString(
            object o,
            string fallbackTo)
        {
            return ToString(o, fallbackTo, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Convert to a string.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <param name="fallbackTo">The fallback to.</param>
        /// <param name="provider">The provider.</param>
        /// <returns></returns>
        public static string ToString(
            object o,
            string fallbackTo,
            IFormatProvider provider)
        {
            if (o == null)
            {
                return fallbackTo;
            }
            // This is the fastest way, see
            else if (o.GetType() == typeof(string))
            {
                return (string)o;
            }
            else
            {
                return Convert.ToString(o, provider);
            }
        }

        /// <summary>
        /// Toes the T.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <param name="fallbackTo">The fallback to.</param>
        /// <returns></returns>
        public static T ToT<T>(
            object o,
            T fallbackTo)
        {
            if (o == null)
            {
                return fallbackTo;
            }
            // This is the fastest way, see
            else if (o.GetType() == typeof(T))
            {
                return (T)o;
            }
            else if (typeof(T).IsEnum)
            {
                if (Enum.IsDefined(typeof(T), o))
                {
                    return (T)Enum.Parse(typeof(T), o.ToString(), true);
                }
                else
                {
                    return fallbackTo;
                }
            }
            else
            {
                return fallbackTo;
            }
        }

        // ------------------------------------------------------------------

        #endregion Converting routines with user-defined fallbacks.

        #region Conversion checkings.

        // ------------------------------------------------------------------

        /// <summary>
        /// Checks whether a string contains a valid boolean.
        /// </summary>
        /// <param name="o">The string to check.</param>
        /// <returns>
        /// Returns TRUE if is a boolean, FALSE if not.
        /// </returns>
        public static bool IsBoolean(
            object o)
        {
            return IsBoolean(o, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Checks whether a string contains a valid boolean.
        /// </summary>
        /// <param name="o">The string to check.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>
        /// Returns TRUE if is a boolean, FALSE if not.
        /// </returns>
        public static bool IsBoolean(
            object o,
            IFormatProvider provider)
        {
            try
            {
                if (o == null)
                {
                    return false;
                }
                // This is the fastest way, see
                else if (o.GetType() == typeof(bool))
                {
                    return true;
                }
                else
                {
                    string s =
                        Convert.ToString(o, provider).Trim().ToLowerInvariant();

                    if (s.Length <= 0)
                    {
                        return false;
                    }
                    else if (o is bool)
                    {
                        return true;
                    }
                    else if (
                        string.Compare(s, bool.TrueString, true) == 0 ||
                        s == @"1" ||
                        s == @"-1")
                    {
                        return true;
                    }
                    else if (
                        string.Compare(s, bool.FalseString, true) == 0 ||
                        s == @"0")
                    {
                        return true;
                    }
                    else
                    {
                        bool.Parse(Convert.ToString(o, provider));
                    }
                }
            }
            catch (FormatException)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Checks whether a string contains a valid currency number.
        /// </summary>
        /// <param name="o">The string to check.</param>
        /// <returns>
        /// Returns TRUE if the string contains a currency,
        /// FALSE if not.
        /// </returns>
        public static bool IsCurrency(
            object o)
        {
            return IsCurrency(o, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Checks whether a string contains a valid currency number.
        /// </summary>
        /// <param name="o">The string to check.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>
        /// Returns TRUE if the string contains a currency,
        /// FALSE if not.
        /// </returns>
        public static bool IsCurrency(
            object o,
            IFormatProvider provider)
        {
            return DoIsNumeric(o, NumberStyles.Currency, provider);
        }

        /// <summary>
        /// Checks whether a string contains a valid date and/or time.
        /// </summary>
        /// <param name="o">The string to check.</param>
        /// <returns>
        /// Returns TRUE if is date/time, FALSE if not.
        /// </returns>
        public static bool IsDateTime(
            object o)
        {
            return IsDateTime(o, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Checks whether a string contains a valid date and/or time.
        /// </summary>
        /// <param name="o">The string to check.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>
        /// Returns TRUE if is date/time, FALSE if not.
        /// </returns>
        public static bool IsDateTime(
            object o,
            IFormatProvider provider)
        {
            if (o == null ||
                Convert.ToString(o, provider).Trim().Length <= 0)
            {
                return false;
            }
            // This is the fastest way, see
            else if (o.GetType() == typeof(DateTime))
            {
                return true;
            }
            else
            {
                DateTime r;

                return DateTime.TryParse(
                    Convert.ToString(o, provider),
                    provider,
                    DateTimeStyles.None,
                    out r);
            }
        }

        /// <summary>
        /// Checks whether a string contains a valid decimal.
        /// </summary>
        /// <param name="o">The string to check.</param>
        /// <returns>
        /// Returns TRUE if the string contains a decimal,
        /// FALSE if not.
        /// </returns>
        public static bool IsDecimal(
            object o)
        {
            return IsDecimal(o, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Checks whether a string contains a valid decimal.
        /// </summary>
        /// <param name="o">The string to check.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>
        /// Returns TRUE if the string contains a decimal,
        /// FALSE if not.
        /// </returns>
        public static bool IsDecimal(
            object o,
            IFormatProvider provider)
        {
            if (o == null)
            {
                return false;
            }
            // This is the fastest way, see
            else if (o.GetType() == typeof(decimal))
            {
                return true;
            }
            else
            {
                return DoIsNumeric(
                    o,
                    floatNumberStyle,
                    provider);
            }
        }

        /// <summary>
        /// Checks whether a string contains a valid double.
        /// </summary>
        /// <param name="o">The string to check.</param>
        /// <returns>
        /// Returns TRUE if the string contains a double,
        /// FALSE if not.
        /// </returns>
        public static bool IsDouble(
            object o)
        {
            return IsDouble(o, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Checks whether a string contains a valid double.
        /// </summary>
        /// <param name="o">The string to check.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>
        /// Returns TRUE if the string contains a double,
        /// FALSE if not.
        /// </returns>
        public static bool IsDouble(
            object o,
            IFormatProvider provider)
        {
            if (o == null)
            {
                return false;
            }
            // This is the fastest way, see
            else if (o.GetType() == typeof(double))
            {
                return true;
            }
            // This is the fastest way, see
            else if (o.GetType() == typeof(float))
            {
                return true;
            }
            else
            {
                return DoIsNumeric(o,
                    floatNumberStyle,
                    provider);
            }
        }

        /// <summary>
        /// Checks whether a string contains a valid float.
        /// </summary>
        /// <param name="o">The string to check.</param>
        /// <returns>
        /// Returns TRUE if the string contains a float,
        /// FALSE if not.
        /// </returns>
        public static bool IsFloat(
            object o)
        {
            return IsFloat(o, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Checks whether a string contains a valid float.
        /// </summary>
        /// <param name="o">The string to check.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>
        /// Returns TRUE if the string contains a float,
        /// FALSE if not.
        /// </returns>
        public static bool IsFloat(
            object o,
            IFormatProvider provider)
        {
            if (o == null)
            {
                return false;
            }
            // This is the fastest way, see
            else if (o.GetType() == typeof(float))
            {
                return true;
            }
            else
            {
                return DoIsNumeric(o,
                    floatNumberStyle,
                    provider);
            }
        }

        /// <summary>
        /// Checks whether a string contains a valid float.
        /// </summary>
        /// <param name="o">The string to check.</param>
        /// <returns>
        /// Returns TRUE if the string contains a float, FALSE if not.
        /// </returns>
        public static bool IsGuid(
            object o)
        {
            return IsGuid(o, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Checks whether a string contains a valid float.
        /// </summary>
        /// <param name="o">The string to check.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>
        /// Returns TRUE if the string contains a float, FALSE if not.
        /// </returns>
        public static bool IsGuid(
            object o,
            IFormatProvider provider)
        {
            if (o == null)
            {
                return false;
            }
            // This is the fastest way, see
            else if (o.GetType() == typeof(Guid))
            {
                return true;
            }
            else if (o is byte[])
            {
                try
                {
                    Guid ignore = new Guid(o as byte[]);
                    return true;
                }
                catch (ArgumentException)
                {
                    return false;
                }
            }
            else
            {
                try
                {
                    Guid ignore = new Guid(o.ToString());
                    return true;
                }
                catch (ArgumentException)
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Checks whether a string contains a valid integer.
        /// </summary>
        /// <param name="o">The string to check.</param>
        /// <returns>
        /// Returns TRUE if the string contains an integer,
        /// FALSE if not.
        /// </returns>
        public static bool IsInt32(
            object o)
        {
            return IsInt32(o, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Checks whether a string contains a valid integer.
        /// </summary>
        /// <param name="o">The string to check.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>
        /// Returns TRUE if the string contains an integer,
        /// FALSE if not.
        /// </returns>
        public static bool IsInt32(
            object o,
            IFormatProvider provider)
        {
            if (o == null)
            {
                return false;
            }
            // This is the fastest way, see
            else if (o.GetType() == typeof(Int32))
            {
                return true;
            }
            else
            {
                return DoIsNumeric(o, NumberStyles.Integer, provider);
            }
        }

        /// <summary>
        /// Checks whether a string contains a valid integer.
        /// </summary>
        /// <param name="o">The string to check.</param>
        /// <returns>
        /// Returns TRUE if the string contains an integer,
        /// FALSE if not.
        /// </returns>
        public static bool IsInt64(
            object o)
        {
            return IsInt64(o, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Checks whether a string contains a valid integer.
        /// </summary>
        /// <param name="o">The string to check.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>
        /// Returns TRUE if the string contains an integer,
        /// FALSE if not.
        /// </returns>
        public static bool IsInt64(
            object o,
            IFormatProvider provider)
        {
            if (o == null)
            {
                return false;
            }
            // This is the fastest way, see
            else if (o.GetType() == typeof(Int64))
            {
                return true;
            }
            else
            {
                return DoIsNumeric(o, NumberStyles.Integer, provider);
            }
        }

        /// <summary>
        /// Checks whether a string contains a valid integer.
        /// </summary>
        /// <param name="o">The string to check.</param>
        /// <returns>
        /// Returns TRUE if the string contains an integer,
        /// FALSE if not.
        /// </returns>
        public static bool IsInteger(
            object o)
        {
            return IsInteger(o, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Checks whether a string contains a valid integer.
        /// </summary>
        /// <param name="o">The string to check.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>
        /// Returns TRUE if the string contains an integer,
        /// FALSE if not.
        /// </returns>
        public static bool IsInteger(
            object o,
            IFormatProvider provider)
        {
            if (o == null)
            {
                return false;
            }
            // This is the fastest way, see
            else if (o.GetType() == typeof(int))
            {
                return true;
            }
            // This is the fastest way, see
            else if (o.GetType() == typeof(long))
            {
                return true;
            }
            else if (o is Enum)
            {
                return true;
            }
            else
            {
                return DoIsNumeric(o, NumberStyles.Integer, provider);
            }
        }

        /// <summary>
        /// Checks whether a string contains a valid float.
        /// </summary>
        /// <param name="o">The string to check.</param>
        /// <returns>
        /// Returns TRUE if the string contains a float,
        /// FALSE if not.
        /// </returns>
        public static bool IsNumeric(
            object o)
        {
            return IsNumeric(o, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Checks whether a string contains a valid float.
        /// </summary>
        /// <param name="o">The string to check.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>
        /// Returns TRUE if the string contains a float,
        /// FALSE if not.
        /// </returns>
        public static bool IsNumeric(
            object o,
            IFormatProvider provider)
        {
            return DoIsNumeric(o,
                floatNumberStyle,
                provider);
        }

        /// <summary>
        /// Does the is numeric.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <param name="styles">The styles.</param>
        /// <param name="provider">The provider.</param>
        /// <returns></returns>
        private static bool DoIsNumeric(
            object o,
            NumberStyles styles,
            IFormatProvider provider)
        {
            if (o == null)
            {
                return false;
            }
            else if (Convert.ToString(o, provider).Length <= 0)
            {
                return false;
            }
            else
            {
                double result;
                return double.TryParse(
                    o.ToString(),
                    styles,
                    provider,
                    out result);
            }
        }

        // ------------------------------------------------------------------

        #endregion Conversion checkings.

        #region Formatting routines.

        // ------------------------------------------------------------------

        /// <summary>
        /// Formats WITH currency symbol, default precision.
        /// </summary>
        /// <param name="val">The val.</param>
        /// <returns></returns>
        public static string FormatCurrency(
            decimal val)
        {
            // "C": With currency symbol.
            return FormatCurrency(val, Thread.CurrentThread.CurrentCulture);
        }

        /// <summary>
        /// Formats WITH currency symbol, default precision.
        /// </summary>
        /// <param name="val">The val.</param>
        /// <param name="provider">The provider.</param>
        /// <returns></returns>
        public static string FormatCurrency(
            decimal val,
            IFormatProvider provider)
        {
            // "C": With currency symbol.
            return val.ToString(@"C", provider);
        }

        /// <summary>
        /// Formats WITH currency symbol.
        /// </summary>
        /// <param name="val">The val.</param>
        /// <param name="precision">Gives the number of decimals digits
        /// after the point.</param>
        /// <returns></returns>
        public static string FormatCurrency(
            decimal val,
            int precision)
        {
            return FormatCurrency(
                val,
                precision,
                Thread.CurrentThread.CurrentCulture);
        }

        /// <summary>
        /// Formats WITH currency symbol.
        /// </summary>
        /// <param name="val">The val.</param>
        /// <param name="precision">Gives the number of decimals digits
        /// after the point.</param>
        /// <param name="provider">The provider.</param>
        /// <returns></returns>
        public static string FormatCurrency(
            decimal val,
            int precision,
            IFormatProvider provider)
        {
            NumberFormatInfo nfi =
                (provider.GetFormat(typeof(NumberFormatInfo)) as
                    NumberFormatInfo).Clone() as NumberFormatInfo;
            nfi.CurrencyDecimalDigits = precision;

            // "C": With currency symbol.
            return val.ToString(@"C", nfi);
        }

        /// <summary>
        /// WITH or WITHOUT currency symbol, default precision.
        /// </summary>
        /// <param name="val">The val.</param>
        /// <param name="addCurrencySymbol">if set to <c>true</c> [add currency symbol].</param>
        /// <returns></returns>
        public static string FormatCurrency(
            decimal val,
            bool addCurrencySymbol)
        {
            return FormatCurrency(
                val,
                addCurrencySymbol,
                Thread.CurrentThread.CurrentCulture);
        }

        /// <summary>
        /// WITH or WITHOUT currency symbol, default precision.
        /// </summary>
        /// <param name="val">The val.</param>
        /// <param name="addCurrencySymbol">if set to <c>true</c> [add currency symbol].</param>
        /// <param name="provider">The provider.</param>
        /// <returns></returns>
        public static string FormatCurrency(
            decimal val,
            bool addCurrencySymbol,
            IFormatProvider provider)
        {
            if (addCurrencySymbol)
            {
                return FormatCurrency(val);
            }
            else
            {
                return val.ToString(@"n", provider);
            }
        }

        /// <summary>
        /// WITH or WITHOUT currency symbol, user-defined precision.
        /// </summary>
        /// <param name="val">The val.</param>
        /// <param name="precision">The precision.</param>
        /// <param name="addCurrencySymbol">if set to <c>true</c> [add currency symbol].</param>
        /// <returns></returns>
        public static string FormatCurrency(
            decimal val,
            int precision,
            bool addCurrencySymbol)
        {
            return FormatCurrency(
                val,
                precision,
                addCurrencySymbol,
                Thread.CurrentThread.CurrentCulture);
        }

        /// <summary>
        /// WITH or WITHOUT currency symbol, user-defined precision.
        /// </summary>
        /// <param name="val">The val.</param>
        /// <param name="precision">The precision.</param>
        /// <param name="addCurrencySymbol">if set to <c>true</c> [add currency symbol].</param>
        /// <param name="provider">The provider.</param>
        /// <returns></returns>
        public static string FormatCurrency(
            decimal val,
            int precision,
            bool addCurrencySymbol,
            IFormatProvider provider)
        {
            if (addCurrencySymbol)
            {
                return FormatCurrency(val, precision, provider);
            }
            else
            {
                NumberFormatInfo nfi =
                    (provider.GetFormat(typeof(NumberFormatInfo)) as
                        NumberFormatInfo).Clone() as NumberFormatInfo;
                nfi.NumberDecimalDigits = precision;

                return val.ToString(@"n", nfi);
            }
        }

        /// <summary>
        /// Formats the decimal.
        /// </summary>
        /// <param name="val">The val.</param>
        /// <returns></returns>
        public static string FormatDecimal(
            decimal val)
        {
            return FormatDecimal(val, Thread.CurrentThread.CurrentCulture);
        }

        /// <summary>
        /// Formats the decimal.
        /// </summary>
        /// <param name="val">The val.</param>
        /// <param name="provider">The provider.</param>
        /// <returns></returns>
        public static string FormatDecimal(
            decimal val,
            IFormatProvider provider)
        {
            return val.ToString(@"D", provider);
        }

        // ------------------------------------------------------------------

        #endregion Formatting routines.

        #region Private helper.

        // ------------------------------------------------------------------

        /// <summary>
        ///
        /// </summary>
        private static readonly NumberStyles floatNumberStyle =
            NumberStyles.Float |
            NumberStyles.Number |
            NumberStyles.AllowThousands |
            NumberStyles.AllowDecimalPoint |
            NumberStyles.AllowLeadingSign |
            NumberStyles.AllowLeadingWhite |
            NumberStyles.AllowTrailingWhite;

        // ------------------------------------------------------------------

        #endregion Private helper.
    }
}