using System;

namespace WMS.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Проверка на пустые строки, отступы и переводы строки
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsEmpty(this string str)
        {
            if (str == null)
                return true;

            return string.IsNullOrEmpty(str.Replace("\n", "").Replace("\t", "").Trim());
        }
        /// <summary>
        /// Реверс строки
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Reverse(this string str)
        {
            var charArray = str.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
    }
}
