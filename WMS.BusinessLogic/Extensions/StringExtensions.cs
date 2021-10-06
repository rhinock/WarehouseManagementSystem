using System;

namespace WMS.BusinessLogic.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Check for empty strings, tabs and new lines
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsEmpty(this string str)
        {
            if (str == null)
                return true;

            return string.IsNullOrEmpty(str.Replace("\n", "").Replace("\t", "").Trim());
        }
        public static string Reverse(this string str)
        {
            var charArray = str.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
    }
}
