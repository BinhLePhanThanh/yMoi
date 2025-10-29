using DocumentFormat.OpenXml.Wordprocessing;
using System.Text;

namespace yMoi.Util
{
    public static class Utils
    {
        public static string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            else
                return builder.ToString().ToUpper();
        }

        public static string CamelCaseFromTitleCase(string name)
        {
            return Char.ToLowerInvariant(name[0]) + name.Substring(1);
        }

        public static string GenerateBNCode()
        {
            string chars = "0123456789";

            Random random = new Random();

            string randomString = new string(Enumerable.Repeat(chars, 8)
                .Select(s => s[random.Next(s.Length)]).ToArray());


            return "BN-" + randomString;
        }

        public static string GenerateNBNCode()
        {
            string chars = "0123456789";

            Random random = new Random();

            string randomString = new string(Enumerable.Repeat(chars, 8)
                .Select(s => s[random.Next(s.Length)]).ToArray());


            return "NBN-" + randomString;
        }
    }
}