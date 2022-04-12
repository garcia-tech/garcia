using System.Collections.Generic;

namespace GarciaCore.CodeGenerator
{
    public class ItemModel
    {
        public string Name { get; set; }
        public List<ItemPropertyModel> Properties { get; set; } = new List<ItemPropertyModel>();
        public bool IsEnum { get; set; }
        public string IdType { get; set; }
        public bool AddApplication { get; set; }
        public bool MultipartUpload { get; set; }
    }
}