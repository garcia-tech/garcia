using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace GarciaCore.Infrastructure
{
    public static class Extensions
    {
        public static Dictionary<string, object> ToDictionary(this NameValueCollection nameValueCollection)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>(nameValueCollection.Count);

            if (nameValueCollection.Count != 0)
            {
                foreach (string key in nameValueCollection.AllKeys)
                {
                    string value = nameValueCollection.Get(key);
                    int? intValue = Helpers.GetValueFromObject<int?>(value);

                    if (intValue.HasValue)
                    {
                        parameters.Add(key, intValue.Value);
                    }
                    else
                    {
                        parameters.Add(key, value);
                    }
                }
            }

            return parameters;
        }

        public static bool IsNullable(this Type type)
        {
            return Nullable.GetUnderlyingType(type) != null;
        }

        public static string ToString(this Dictionary<string, object> source, string keyValueSeparator, string sequenceSeparator)
        {
            if (source == null)
            {
                throw new ArgumentException("Parameter source can not be null.");
            }

            var pairs = source.Select(x => string.Format("{0}{1}{2}{3}{4}", x.Key, keyValueSeparator, "'", x.Value.ToString(), "'"));
            return string.Join(sequenceSeparator, pairs);
        }

        public static bool IsIList(this Type type)
        {
            Type[] interfaces = type.GetInterfaces();
            return interfaces.Contains(typeof(IList));
        }

        public static void AddParameterValue(this Dictionary<string, object> parameters, string key, object value)
        {
            parameters.Add(key, value == null ? DBNull.Value : value);
        }

        public static string GetDescription<T>(this T e)
        {
            string description = null;

            if (e is Enum)
            {
                Type type = e.GetType();
                Array values = System.Enum.GetValues(type);

                foreach (int val in values)
                {
                    if (val == Convert.ToInt32(e))
                    {
                        var memInfo = type.GetMember(type.GetEnumName(val));
                        var descriptionAttributes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                        if (descriptionAttributes.Length > 0)
                        {
                            description = ((DescriptionAttribute)descriptionAttributes[0]).Description;
                        }

                        break;
                    }
                }
            }

            return description;
        }

        public static string RemoveIllegalCharacters(this string input)
        {
            return input?.Replace("<", " ").Replace(">", "").Replace("&amp", "");
        }

        public static string PrepareName(this string name)
        {
            //return name.ToLowerInvariant();
            return name;
        }
    }
}
