namespace GarciaCore.CodeGenerator
{
    public class ItemPropertyModel
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string MappingType { get; set; }
        public string InnerType { get; set; }
        public bool IsNullable { get; set; }
    }
}