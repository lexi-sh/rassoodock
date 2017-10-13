using System;

namespace Rassoodock.Common.Extensions
{
    public static class StringExtensions
    {
        public static bool EqualsOrdinalIgnoreCase(this string @this, string other) =>
            string.Equals(@this, other, StringComparison.OrdinalIgnoreCase);
    }
}
