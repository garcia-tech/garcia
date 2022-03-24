using System.Collections.Generic;

namespace GarciaCore.CodeGenerator
{
    public class Item
    {
        public string Name { get; set; }
        public List<ItemProperty> Properties { get; set; } = new List<ItemProperty>();
        public bool IsEnum { get; set; }
        public IdType IdType { get; set; }
        public bool AddApplication { get; set; }

        public Item()
        {
        }

        public Item(string name, bool isEnum, IdType idType, bool addApplication)
        {
            Name = name;
            IsEnum = isEnum;
            IdType = idType;
            AddApplication = addApplication;
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}