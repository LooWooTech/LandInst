using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Loowoo.LandInst.Common
{
    public static class Extendions
    {
        public static int? AsNullOrInt(this object obj)
        {
            if (obj == null)
            {
                return null;
            }

            int result = 0;

            if (int.TryParse(obj.ToString(), out result))
            {
                return result;
            }
            return null;
        }

        public static DateTime? AsNullOrDate(this object obj)
        {
            var value = obj;
            if (value == null)
            {
                return null;
            }
            DateTime result = DateTime.MinValue;
            if (DateTime.TryParse(value.ToString(), out result))
            {
                return result;
            }
            return null;
        }

        public static string AsString(this object obj)
        {
            return obj == null ? null : obj.ToString();
        }

        public static TEnum AsEnum<TEnum>(this object obj) where TEnum : struct
        {
            if (obj == null)
            {
                return default(TEnum);
            }

            TEnum result = default(TEnum);

            Enum.TryParse<TEnum>(obj.ToString(), out result);

            return result;
        }
    }
}