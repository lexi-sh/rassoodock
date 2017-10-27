using System;

namespace Rassoodock.Tests.Base
{
    public static class EnhancedRandom
    {
        private static readonly Random Random = new Random();

        public static string Letter()
        {
            var letter = '\0';
            while (!char.IsLetter(letter))
                letter = Convert.ToChar(String(1, 1).ToUpper());
            return letter.ToString();
        }

        public static string String(int minChars = 1, int maxChars = 255)
        {
            var numChars = Random.Next(minChars, maxChars);
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var stringChars = new char[numChars];

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[Random.Next(chars.Length)];
            }

            return new string(stringChars);
        }

        public static char Symbol()
        {
            const string chars = "!?@#()";
            return chars[Random.Next(chars.Length)];
        }

        public static string StringLongerThan(int minChars)
        {
            return String(minChars + 1, minChars + 100);
        }

        public static bool Boolean()
        {
            return Random.NextDouble() > 0.5;
        }

        public static int Integer(int min = 0, int max = int.MaxValue)
        {
            return Random.Next(min, max);
        }

        public static long Long(long min = 0, long max = long.MaxValue)
        {
            var buf = new byte[8];
            Random.NextBytes(buf);
            var longRand = BitConverter.ToInt64(buf, 0);
            return (Math.Abs(longRand % (max - min)) + min);
        }

        public static double Double(double min = 0, double max = double.MaxValue)
        {
            // 0.0 to 1.0
            var randomDouble = Random.NextDouble();
            randomDouble *= max;
            randomDouble += min;
            return randomDouble;
        }

        public static decimal Decimal(double min = 0, double max = double.MaxValue)
        {
            // 0.0 to 1.0
            var randomDouble = Random.NextDouble();
            randomDouble *= max;
            randomDouble += min;

            return Convert.ToDecimal(randomDouble);
        }

        public static DateTime DateTime(
            DateTime? minValue = null,
            DateTime? maxValue = null)
        {
            var dateTime = DateTimeNullable(false, minValue, maxValue);
            if (dateTime.HasValue)
            {
                return dateTime.Value;
            }
            throw new ArgumentException("Random DateTime cannot be nullable from this context");
        }

        public static DateTime? DateTimeNullable(
            bool allowNullable = true,
            DateTime? minValue = null,
            DateTime? maxValue = null)
        {
            if (allowNullable && Boolean())
            {
                return null;
            }
            if (minValue == null)
            {
                minValue = System.DateTime.UtcNow.AddYears(-5);
            }

            if (maxValue == null)
            {
                maxValue = System.DateTime.UtcNow.AddYears(5);
            }

            var daysBetween = new TimeSpan(maxValue.Value.Ticks - minValue.Value.Ticks).Ticks;
            return System.DateTime.SpecifyKind(minValue.Value.AddTicks(Long(max: daysBetween)), DateTimeKind.Utc);
        }

    }
}
