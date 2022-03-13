namespace GarciaCore.CodeGenerator
{
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
}