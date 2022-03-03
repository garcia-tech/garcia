using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml;

namespace GarciaCore.Application
{
    public static class Helpers
    {
        public static T GetValueFromObject<T>(object Value)
        {
            if (Value == null || Value == DBNull.Value)
            {
                return default(T);
            }

            try
            {
                return (T)GetValueFromObject(typeof(T), Value);
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        public static T GetValueFromObject<T>(object Value, T DefaultValue)
        {
            if (Value == null || Value == DBNull.Value)
            {
                return DefaultValue;
            }

            try
            {
                return (T)GetValueFromObject(typeof(T), Value);
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        private static object GetValueFromObject(Type type, object value)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(type);

            if (converter == null)
            {
                return null;
            }

            try
            {
                object result = null;

                if (type == typeof(bool) || type == typeof(bool?))
                {
                    int? x = null;

                    try
                    {
                        x = Convert.ToInt32(value);
                    }
                    catch (Exception)
                    {
                    }

                    if (x.HasValue)
                    {
                        result = Convert.ToBoolean(x);
                    }
                    else
                    {
                        result = Convert.ToBoolean(value);
                    }
                }

                result = converter.ConvertFromString(value.ToString());
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static void SetPropertyValue(PropertyInfo property, string value)
        {
            SetPropertyValue(null, property, value);
        }

        public static void SetPropertyValue(PropertyInfo property, object value)
        {
            SetPropertyValue(null, property, value);
        }

        public static void SetPropertyValue(object item, PropertyInfo property, object value)
        {
            if (property != null && value != null)
            {
                if (property.PropertyType.IsEnum)
                {
                    property.SetValue(item, Enum.Parse(property.PropertyType, value.ToString()));
                }
                else
                {
                    Type type = property.PropertyType;

                    if (property.PropertyType.IsNullable())
                    {
                        type = property.PropertyType.GenericTypeArguments?[0];
                    }

                    object temp = value;

                    if (type != value.GetType())
                    {
                        temp = Convert.ChangeType(value, type);
                    }

                    property.SetValue(item, temp);
                }
            }
        }

        public static void SetPropertyValue(object item, string propertyName, object value)
        {
            if (item != null && value != null)
            {
                PropertyInfo[] properties = item.GetType().GetProperties();

                if (properties.Length != 0)
                {
                    foreach (PropertyInfo property in properties)
                    {
                        if (property.Name.Equals(propertyName, StringComparison.InvariantCultureIgnoreCase))
                        {
                            SetPropertyValue(item, property, value);
                            return;
                        }
                    }
                }
            }
        }

        public static string CreateKey(int Length)
        {
            string Key = Guid.NewGuid().ToString().Replace("-", "").Substring(0, Length).ToUpper();
            return Key;
        }

        public static string GetValueFromHtml(string Html, string IdTagName)
        {
            Match rg = Regex.Match(Html, "=\"" + IdTagName + "\">(.*?)</td>");
            string result = rg.Groups[1].ToString();
            return result;
        }

        public static bool IsBasicType(Type Type)
        {
            return Type.IsValueType || Type.Equals(typeof(string));
        }

        public static List<T> ConvertStringToList<T>(string Value, char Seperator = ';')
        {
            List<T> result = null;

            if (!string.IsNullOrEmpty(Value))
            {
                string[] values = Value.Split(Seperator);
                result = ConvertArrayToList<T>(values);
            }

            return result;
        }

        public static List<T> ConvertArrayToList<T>(object[] Values)
        {
            if (Values == null)
            {
                return null;
            }

            List<T> items = new List<T>();

            foreach (object item in Values)
            {
                if (item != null)
                {
                    T value = Helpers.GetValueFromObject<T>(item);

                    if (value != null)
                    {
                        items.Add(value);
                    }
                }
            }

            return items;
        }

        public static string ReadFromFile(string FilePath)
        {
            try
            {
                using (StreamReader sr = new StreamReader(FilePath))
                {
                    String line = sr.ReadToEnd();
                    return line;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static XmlDocument ReadXmlFromFile(string FilePath)
        {
            XmlDocument document = new XmlDocument();
            document.Load(FilePath);
            return document;
        }

        public static string GetImageUrl(string image)
        {
            //if (string.IsNullOrEmpty(image))
            //    return string.Empty;

            //string baseUrl = BuybackSettings.Instance.BaseImageUrl;

            //if (image.StartsWith(baseUrl))
            //    return image;
            //return baseUrl + image;

            // TODO
            return image;
        }

        public static bool CheckPasswordPolicy(string password)
        {
            // TODO
            //if (BuybackSettings.Instance.ForcePasswordPolicy)
            //{
            //    return Regex.Match(password, "^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$").Success;
            //}

            return true;
        }

        public static string ToCamelCase(this string Value)
        {
            try
            {
                if (Value.Length == 0)
                    return Value;
                var temp = Value.Trim().Split(' ');
                var sb = new GarciaCoreStringBuilder();

                foreach (var item in temp)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        sb += (Char.ToUpper(item[0], new CultureInfo("en-US")) + item.Substring(1));
                    }
                }

                string value = sb.ToString();
                return Char.ToLower(value[0], new CultureInfo("en-US")) + value.Substring(1);
            }
            catch (Exception)
            {
                return Value;
            }
        }
    }

    public enum HashAlgorithm
    {
        SHA1 = 0,
        MD5
    }
}