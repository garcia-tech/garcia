using System.Collections.Generic;

namespace GarciaCore.CodeGenerator
{
    public class ItemsModel
    {
        public List<ItemModel> Items { get; set; }
    }

    public class ItemModel
    {
        public string Name { get; set; }
        public List<ItemPropertyModel> Properties { get; set; } = new List<ItemPropertyModel>();
        public bool IsEnum { get; set; }
        public string IdType { get; set; }
        public bool AddApplication { get; set; }
        public bool MultipartUpload { get; internal set; }
    }

    public class ItemPropertyModel
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string MappingType { get; set; }
        public string InnerType { get; set; }
    }
}