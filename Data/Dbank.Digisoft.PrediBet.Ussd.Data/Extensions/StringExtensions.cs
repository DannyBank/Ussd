using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Dbank.Digisoft.PrediBet.Ussd.Data.Extensions
{
    public static class StringExtensions
    {
        //returns a uniformly random ulong between ulong.Min inclusive and ulong.Max inclusive
        public static ulong NextULong(this Random rng)
        {
            byte[] buf = new byte[8];
            rng.NextBytes(buf);
            return BitConverter.ToUInt64(buf, 0);
        }

        //returns a uniformly random ulong between ulong.Min and Max without modulo bias
        public static ulong NextULong(this Random rng, ulong max, bool inclusiveUpperBound = false)
        {
            return rng.NextULong(ulong.MinValue, max, inclusiveUpperBound);
        }

        //returns a uniformly random ulong between Min and Max without modulo bias
        public static ulong NextULong(this Random rng, ulong min, ulong max, bool inclusiveUpperBound = false)
        {
            ulong range = max - min;

            if (inclusiveUpperBound)
            {
                if (range == ulong.MaxValue)
                {
                    return rng.NextULong();
                }

                range++;
            }

            if (range <= 0)
            {
                throw new ArgumentOutOfRangeException("Max must be greater than min when inclusiveUpperBound is false, and greater than or equal to when true", "max");
            }

            ulong limit = ulong.MaxValue - ulong.MaxValue % range;
            ulong r;
            do
            {
                r = rng.NextULong();
            } while (r > limit);

            return r % range + min;
        }

        //returns a uniformly random long between long.Min inclusive and long.Max inclusive
        public static long NextLong(this Random rng)
        {
            byte[] buf = new byte[8];
            rng.NextBytes(buf);
            return BitConverter.ToInt64(buf, 0);
        }

        //returns a uniformly random long between long.Min and Max without modulo bias
        public static long NextLong(this Random rng, long max, bool inclusiveUpperBound = false)
        {
            return rng.NextLong(long.MinValue, max, inclusiveUpperBound);
        }

        //returns a uniformly random long between Min and Max without modulo bias
        public static long NextLong(this Random rng, long min, long max, bool inclusiveUpperBound = false)
        {
            ulong range = (ulong) (max - min);

            if (inclusiveUpperBound)
            {
                if (range == ulong.MaxValue)
                {
                    return rng.NextLong();
                }

                range++;
            }

            if (range <= 0)
            {
                throw new ArgumentOutOfRangeException("Max must be greater than min when inclusiveUpperBound is false, and greater than or equal to when true", "max");
            }

            ulong limit = ulong.MaxValue - ulong.MaxValue % range;
            ulong r;
            do
            {
                r = rng.NextULong();
            } while (r > limit);

            return (long) (r % range + (ulong) min);
        }

        public static bool IsNumeric(this string data)
        {
            return long.TryParse(data, out _);
        }

        public static string Sanitize(this string s)
        {
            return s.Trim(" ".ToCharArray()).Replace(" ", "");
        }

        public static string CleanSpecials(this string s)
        {
            return s.Replace("+", "").Replace("$", "").Replace("-", "");
        }

        public static string Sha256(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;
            using (SHA256 shA256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(input);
                return Convert.ToBase64String(shA256.ComputeHash(bytes));
            }
        }

        public static long GenerateTransactionId(this string msisdn)
        {
            try
            {
                return long.Parse(msisdn.Substring(3) + DateTime.Now.Ticks.ToString().Substring(9));
            }
            catch
            {
                //Possible Fix for System.OverflowException
                return (long) NextULong(new Random());
            }
        }

        public static string RemoveIntl(this string msisdn)
        {
            return $"0{msisdn.Substring(3, msisdn.Length - 3)}";
        }

        public static bool IsValidMsisdn(this string entry)
        {
            var regex = @"^[0-9-\)\( ]{9,15}$";
            if (Regex.Match(entry, regex).Success)
            {
                var subCode = int.Parse(entry.TrimStart('0').TrimStart("231".ToCharArray()).Substring(0, 2));
                return (subCode == 88 || subCode == 55);
            }

            return false;
        }

        [DebuggerStepThrough]
        public static string ToIntMsisdn(this string input, IntCode country, bool CheckMTN = true)
        {
            var regex = @"^[0-9-\)\( ]{9,15}$";
            if (Regex.Match(input, regex).Success)
            {
                string phoneNumber = input.Replace("-", "").Replace(" ", "").Replace("+", "")
                    .Replace("(", "").Replace(")", "").TrimStart('0');
                if (phoneNumber.Length >= 8 && phoneNumber.Length <= 9)
                {
                    switch (country)
                    {
                        case IntCode.GHA:
                            phoneNumber = "233" + phoneNumber;
                            break;
                        case IntCode.LIB:
                            phoneNumber = "231" + phoneNumber;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(country), country, null);
                    }
                }

                //Check for Numbers Validity
                //var subCode = int.Parse(phoneNumber.Substring(3, 2));
                if (CheckMTN)
                {
                    var subCode = int.Parse(phoneNumber.Substring(0, 5));
                    var isNotMtnLibCode = (subCode != 23188 && subCode != 23155);
                    var isNotMtnGhCode = (subCode != 23324 && subCode != 23355 && subCode != 23354);
                    switch (country)
                    {
                        case IntCode.GHA:
                            if (isNotMtnGhCode)
                            {
                                throw new InvalidOperationException($"The number {phoneNumber} is an Invalid Phone Number Passed.");
                            }

                            break;
                        case IntCode.LIB:
                            if (isNotMtnLibCode || phoneNumber.Length != 12)
                            {
                                throw new InvalidOperationException($"The number {phoneNumber} is an Invalid Phone Number Passed.");
                            }

                            break;
                    }
                }

                return phoneNumber;
            }

            throw new InvalidOperationException("Invalid Phone Number Passed");
        }

        public static string ToOriginName(this string input)
        {
            var origin = input.Replace("-", "").Replace("_", "").Replace(".", "").Replace(" ", "").ToUpper();
            if (origin.Length > 10)
            {
                return origin.Substring(0, 10);
            }

            return origin;
        }

        public static string RandomString(this Random random, int size)
        {
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }


        public static string Hyphenate(this string input)
        {
            var origin = input.Replace("_", "-")
                .Replace("  ", "-")
                .Replace(" -", "-")
                .Replace("--", "-")
                .Replace(" ", "-").ToUpper();
            return origin;
        }

        public static string Abbreviate(this string input)
        {
            var parts = input.RemoveSpecialCharacters().Split(' ').Where(s => !string.IsNullOrWhiteSpace(s));
            return string.Join("", parts.Select(s => s.FirstOrDefault().ToString().ToUpper()));
        }

        public static string RemoveSpecialCharacters(this string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_' || c == ' ')
                {
                    sb.Append(c);
                }
            }

            return sb.ToString();
        }

        public static byte[] GetHash(this string inputString)
        {
            HashAlgorithm algorithm = MD5.Create(); //or use SHA256.Create();
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        public static string GetHashString(this string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }

        public static string RemoveUnwantedFromDATypeToLower(this string input)
        {
            return input.Replace(" ", "").Replace("-", "").Replace("_", "").ToLower();
        }

        public static string ConvertToCamelCase(this string input)
        {
            TextInfo txtInfo = new CultureInfo("en-us", false).TextInfo;
            return input = txtInfo.ToTitleCase(input);
        }
    }

    public enum IntCode
    {
        GHA = 1,
        LIB = 2
    }
}