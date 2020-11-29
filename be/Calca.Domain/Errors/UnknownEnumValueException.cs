using System;
using System.Collections.Generic;
using System.Text;

namespace Calca.Domain.Errors
{
    public class UnknownEnumValueException<TEnum> : Exception
        where TEnum : Enum
    {
        public UnknownEnumValueException(TEnum unknownValue)
            : base($"Unknown value {unknownValue} of enum {typeof(TEnum).Name}")
        {
        }
    }

    public static class UnknownEnum
    {
        public static UnknownEnumValueException<TEnum> IsUnknown<TEnum>(this TEnum unknownValue)
            where TEnum : Enum
        {
            return new UnknownEnumValueException<TEnum>(unknownValue);
        }
    }
}
