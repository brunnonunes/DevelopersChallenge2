using System.Security.Cryptography;
using System.Text;

namespace OfxDocumentReader.App.Utilities
{
    public static class ExtensionMethods
    {
        private static byte[] GetHash(this string inputString)
        {
            HashAlgorithm algorithm = SHA256.Create();
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        public static string GetHashString(this string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }

        public static string FormatDate(this string inputString)
        {
            string year = inputString.Substring(0, 4);

            string month = inputString.Substring(4, 2);

            string day = inputString.Substring(6, 2);

            string date = $"{day}/{month}/{year}";

            return date;
        }
    }
}
