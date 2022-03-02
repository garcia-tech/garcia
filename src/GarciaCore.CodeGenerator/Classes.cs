using System.Collections.Generic;

namespace GarciaCore.CodeGenerator
{
    public class Item
    {
        public string Name { get; set; }
        public List<ItemProperty> Properties { get; set; } = new List<ItemProperty>();
        public bool IsEnum { get; set; }
        public IdType IdType { get; set; }

        public override string ToString()
        {
            return this.Name;
        }
    }

    public class ItemProperty
    {
        public string Name { get; set; }
        public bool IsNullable { get; set; }
        public ItemPropertyType Type { get; set; }
        public ItemPropertyMappingType MappingType { get; set; }
        public Item? InnerType { get; set; }
        public int MinLength { get; set; }
        public int MaxLength { get; set; }
        public int? ItemId { get; set; }
        public string RegularExpressionValidation { get; set; }
        public Item Item { get; set; }
        public AccessorType AccessorType { get; set; }
        //public bool IsUnicode { get; set; }
        //public bool MvcIgnore { get; set; }
        //public bool MvcListIgnore { get; set; }
        //public bool NotSelected { get; set; }
        //public bool NotSaved { get; set; }
        //public bool AppendToToString { get; set; }
        //public string FieldName { get; set; }
        //public bool AddOnChange { get; set; }
        //public string EditGroupName { get; set; }
    }

    public enum IdType
    {
        Int = 0,
        Long,
        Guid,
        ObjectId
    }

    public enum ItemPropertyType
    {
        Integer = 0,
        Double,
        Float,
        Decimal,
        DateTime,
        TimeSpan,
        String,
        Char,
        Class,
        Unknown,
        Boolean,
        Enum
    }

    public enum ItemPropertyMappingType
    {
        Property = 0,
        List,
        Array
    }

    public enum AccessorType
    {
        Public = 0,
        Private,
        Protected,
        Internal,
        ProtectedInternal
    }
}