using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using Garcia.Application;

namespace Garcia.Application
{
    public static class Extensions
    {
        public static Dictionary<string, object> ToDictionary(this NameValueCollection nameValueCollection)
        {
            var parameters = new Dictionary<string, object>(nameValueCollection.Count);

            if (nameValueCollection.Count != 0)
            {
                foreach (string key in nameValueCollection.AllKeys)
                {
                    string value = nameValueCollection.Get(key);
                    int? intValue = Helpers.GetValueFromObject<int?>(value);

                    if (intValue.HasValue)
                    {
                        parameters.Add(key, intValue.Value);
                        continue;
                    }

                    parameters.Add(key, value);
                }
            }

            return parameters;
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

        public static bool IsIList(this Type type) =>
            type.GetInterfaces().Contains(typeof(IList));

        public static bool IsValid(this DataTable DataTable) => DataTable != null || DataTable.Rows != null || DataTable.Rows.Count != 0;

        public static bool IsValid(this DataSet DataSet) => DataSet != null || DataSet.Tables.Count != 0 || DataSet.Tables[0].IsValid();

        public static string GetDescription<T>(this T e)
        {
            string description = null;

            if (e is Enum)
            {
                Type type = e.GetType();
                Array values = Enum.GetValues(type);

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

        public static void AddParameterWithValue(this DbCommand command, string parameterName, object parameterValue)
        {
            DbParameter parameter = command.CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.Value = parameterValue;
            command.Parameters.Add(parameter);
        }

        public static T GetValue<T>(this DataRow obj, string ColumnName)
        {
            try
            {
                return Helpers.GetValueFromObject<T>(obj[ColumnName]);
            }
            catch (Exception)
            {
                return default;
            }
        }

        public static T GetValue<T>(this Dictionary<string, object> obj, string ColumnName)
        {
            try
            {
                return Helpers.GetValueFromObject<T>(obj[ColumnName]);
            }
            catch (Exception)
            {
                return default;
            }
        }

        public static Dictionary<T, K> GetValue<T, K>(this Dictionary<string, object> obj, string ColumnName)
        {
            try
            {
                bool conversionResult = obj.TryGetValue(ColumnName, out object convertedValue);

                if (conversionResult)
                {
                    Type type = typeof(T);

                    if (type.IsEnum)
                    {
                        return ((Dictionary<string, K>)convertedValue).ToDictionary(x => (T)Enum.Parse(typeof(T), x.Key), x => x.Value);
                    }

                    return convertedValue as Dictionary<T, K>;
                }

                return null;
            }

            catch (Exception)
            {
                return null;
            }
        }

        public static string ToUnicode(this string text)
        {
            string[] olds = { "Ğ", "ğ", "Ü", "ü", "Ş", "ş", "İ", "ı", "Ö", "ö", "Ç", "ç" };
            string[] news = { "G", "g", "U", "u", "S", "s", "I", "i", "O", "o", "C", "c" };

            for (int i = 0; i < olds.Length; i++)
            {
                text = text.Replace(olds[i], news[i]);
            }

            text = text.Replace(" ", "");
            return text;
        }

        public static string ToLowerUnicode(this string text) => text.ToLower().ToUnicode();

        public static string ToUpperUnicode(this string text) => text.ToUpper().ToUnicode();

        public static string ToTitleCase(this string Value, CultureInfo CultureInfo = null)
        {
            if (string.IsNullOrEmpty(Value))
            {
                return string.Empty;
            }

            string[] splittedString = Value.Trim().Split(' ');

            if (splittedString == null || splittedString.Length == 0)
            {
                return string.Empty;
            }

            if (CultureInfo == null)
            {
                CultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture;
            }

            if (splittedString.Length == 1)
            {
                return CultureInfo.TextInfo.ToTitleCase(Value.Trim());
            }

            StringBuilder stringBuilder = new StringBuilder();

            foreach (string str in splittedString)
            {
                stringBuilder.Append(CultureInfo.TextInfo.ToTitleCase(str.Trim()));
                stringBuilder.Append(' ');
            }

            return stringBuilder.ToString().Trim();
        }

        public static string ToSplittedTitleCase(this string Value)
        {
            char[] chars = Value.ToCharArray();

            var resultData = new GarciaStringBuilder();

            foreach (char item in chars)
            {
                if (char.IsUpper(item))
                {
                    resultData += " ";
                }

                resultData += item;
            }

            return resultData.ToString().Trim(' ');
        }

        public static string ToPascalCase(this string Value, CultureInfo CultureInfo = null)
        {
            if (Value.Length == 0)
            {
                return Value;
            }

            return char.ToUpper(Value[0], new CultureInfo("en-US")) + Value.Substring(1);
        }

        public static string ToCamelCase(this string Value, CultureInfo CultureInfo = null)
        {
            if (Value.Length == 0)
            {
                return Value;
            }

            return char.ToLower(Value[0], new CultureInfo("en-US")) + Value.Substring(1);
        }

        public static string GetString(this DataRow obj, string ColumnName) => obj.GetValue<string>(ColumnName);

        public static int GetInt(this DataRow obj, string ColumnName) => obj.GetValue<int>(ColumnName);

        public static double GetDouble(this DataRow obj, string ColumnName) => obj.GetValue<double>(ColumnName);

        public static long GetLong(this DataRow obj, string ColumnName) => obj.GetValue<long>(ColumnName);

        public static float GetFloat(this DataRow obj, string ColumnName) => obj.GetValue<float>(ColumnName);

        public static DateTime GetDateTime(this DataRow obj, string ColumnName) => obj.GetValue<DateTime>(ColumnName);

        public static Guid GetGuid(this DataRow obj, string ColumnName) => obj.GetValue<Guid>(ColumnName);

        public static bool HasAttribute<T>(this Enum EnumValue)
         where T : Attribute
        {
            MemberInfo[] memberInfo = EnumValue.GetType().GetMember(EnumValue.ToString());
            object[] attributes = memberInfo[0].GetCustomAttributes(typeof(T), false);
            return attributes.Length != 0;
        }

        public static bool HasAttribute<T>(this MemberInfo memberInfo)
            where T : Attribute => Attribute.IsDefined(memberInfo, typeof(T));

        public static bool IsBasicType(this Type Type) => Helpers.IsBasicType(Type);

        public static bool IsNullable(this Type type) => Nullable.GetUnderlyingType(type) != null;

        public static bool IsCollection(this Type type) => type.GetInterfaces().Contains(typeof(IList));

        public static void AddParameterValue(this Dictionary<string, object> parameters, string key, object value)
        {
            parameters.Add(key, value == null ? DBNull.Value : value);
        }

        public static T GetPropertyValue<T>(this Type type, string propertyName)
        {
            PropertyInfo property = type.GetProperty(propertyName, BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
            object value = null;

            if (property != null)
            {
                value = property.GetValue(null);
            }

            return Helpers.GetValueFromObject<T>(value);
        }

        public static string ReplaceWithPrefix(this string Value, string KeyName, string KeyValue) => Value.Replace("#" + KeyName + "#", KeyValue);

        public static string RemoveWhiteSpaces(this string Value) => Value.Replace(" ", "");
    }
}