using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loowoo.LandInst.Common
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());

            var attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;

            return attribute == null ? value.ToString() : attribute.Description;
        }

        public static T ToEnum<T>(this string description)
        {
            var type = typeof(T);
            if (!type.IsEnum) throw new InvalidOperationException();
            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute != null)
                {
                    if (attribute.Description == description)
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name == description)
                        return (T)field.GetValue(null);
                }
            }
            throw new ArgumentException("Not found.", "description");
            // or return default(T);
        }

        public static IEnumerable<string> GetDescriptions(this Type type)
        {
            if (!type.IsEnum) throw new InvalidOperationException();
            foreach (var v in Enum.GetNames(type))
            {
                var value = (Enum)Enum.Parse(type, v);
                var text = value.GetDescription();
                yield return text;
            }
        }

        public static Dictionary<int, string> GetValueAndDescriptions(this Type type)
        {
            if (!type.IsEnum) throw new InvalidOperationException();
            var dict = new Dictionary<int, string>();
            foreach (var v in Enum.GetNames(type))
            {
                var value = (Enum)Enum.Parse(type, v);
                var text = value.GetDescription();
                dict.Add(Convert.ToInt32(value), text);
            }
            return dict;
        }
    }
}
