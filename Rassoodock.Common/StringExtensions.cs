using System;
using System.Collections.Generic;
using System.Text;

namespace Rassoodock.Common
{
    public static class StringExtensions
    {

        public static TEnum ParseEnum<TEnum>(this string @this, bool ignoreCase = true)
            where TEnum : struct, IConvertible
        {
            var type = typeof(TEnum);
            try
            {
                return (TEnum)Enum.Parse(type, @this, ignoreCase);
            }
            catch
            {
                throw new ArgumentException($"Cannot parse '{@this}` as value of enum type '{type}'.");
            }
            
        }
    }
}
