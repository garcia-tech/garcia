using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json.Linq;

namespace Garcia.Domain.PostgreSql
{
    public abstract class DynamicEntity : Entity<long>, IDynamicEntity
    {
        [Column(TypeName = "jsonb")]
        public string Properties { get; set; }

        public void AddProperty(string key, string value)
        {
            var properties = JObject.Parse(Properties);
            var existingProperty = properties.ContainsKey(key);

            if (existingProperty)
            {
                properties[key] = value;
                return;
            }
            properties.Add(key, value);
            Properties = properties.ToString();
        }

        public void RemoveProperty(string key)
        {
            var properties = JObject.Parse(Properties);
            var existingProperty = properties.ContainsKey(key);

            if (existingProperty)
            {
                properties.Remove(key);
            }

            Properties = properties.ToString();

        }

        public void AddProperties(IEnumerable<KeyValuePair<string, string>> newProperties)
        {
            var properties = JObject.Parse(Properties);


            foreach (var property in newProperties)
            {
                var existingProperty = properties.ContainsKey(property.Key);

                if (existingProperty)
                {
                    properties[property.Key] = property.Value;
                    return;
                }

                properties.Add(property.Key, property.Value);
            }

            Properties = properties.ToString();
        }

        public string GetPropertyValueOrDefault(string key)
        {
            var properties = JObject.Parse(Properties);
            return properties.Value<string>(key);
        }
    }
}