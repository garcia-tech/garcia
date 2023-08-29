namespace Garcia.Domain.PostgreSql
{
    public interface IDynamicEntity
    {
        string Properties { get; set; }
        void AddProperty(string key, string value);
        void RemoveProperty(string key);
        void AddProperties(IEnumerable<KeyValuePair<string, string>> newProperties);
    }
}
