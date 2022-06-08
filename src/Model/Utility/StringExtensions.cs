using System;
using System.Text;

namespace DegreeClassEstimator.Model
{
    public static class StringExtensions
    {
        public static string Base64Encode(this string baseString, Encoding encoding)
        {
            if (string.IsNullOrWhiteSpace(baseString))
            {
                return baseString;
            }
            byte[] bytes = encoding.GetBytes(baseString);
            return Convert.ToBase64String(bytes);
        }

        public static string Base64Decode(this string encodedString, Encoding encoding)
        {
            if (string.IsNullOrWhiteSpace(encodedString))
            {
                return encodedString;
            }
            byte[] bytes = Convert.FromBase64String(encodedString);
            return encoding.GetString(bytes);
        }
    }
}
