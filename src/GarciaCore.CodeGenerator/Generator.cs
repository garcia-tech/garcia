using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GarciaCore.CodeGenerator
{

    public abstract class Generator : IGenerator
    {
        protected ItemPropertyType[] notNullablePropertyTypes = new ItemPropertyType[] { ItemPropertyType.String, ItemPropertyType.Class, ItemPropertyType.Unknown };

        protected virtual string FileNamePrefix { get; } = string.Empty;
        protected abstract string FileNamePostfix { get; }
        protected abstract string FileExtension { get; }
        public abstract GeneratorType GeneratorType { get; }

        public virtual List<IGenerator> Dependencies { get; set; } = new List<IGenerator>();

        public abstract string DefaultBaseClass { get; }

        public virtual string Name { get { return GetType().FullName; } }

        public virtual List<string> Usings { get; set; }

        public virtual async Task<string> Generate<T>(Item item, string @namespace, string baseClass) where T : BaseTemplate
        {
            var template = CreateItem<T>();
            template.Item = item;
            template.BaseClass = baseClass;
            template.Includes = baseClass;
            template.Namespace = @namespace;
            template.Usings = Usings;
            var text = template.TransformText();
            return text;
        }

        protected virtual T CreateItem<T>() where T : BaseTemplate
        {
            var item = Activator.CreateInstance<T>();
            return item;
        }

        protected virtual string GetInnerTypeClassName(string innerTypeName)
        {
            return innerTypeName;
        }

        public virtual string GetInnerTypeName(ItemProperty property, bool useCollections = true, string postfix = "")
        {
            string typeName = "";

            switch (property.Type)
            {
                case ItemPropertyType.Integer:
                    typeName = "int";
                    break;
                case ItemPropertyType.Double:
                    typeName = "double";
                    break;
                case ItemPropertyType.Float:
                    typeName = "float";
                    break;
                case ItemPropertyType.Decimal:
                    typeName = "decimal";
                    break;
                case ItemPropertyType.DateTime:
                    typeName = "DateTime";
                    break;
                case ItemPropertyType.TimeSpan:
                    typeName = "TimeSpan";
                    break;
                case ItemPropertyType.String:
                    typeName = "string";
                    break;
                case ItemPropertyType.Char:
                    typeName = "char";
                    break;
                case ItemPropertyType.Boolean:
                    typeName = "bool";
                    break;
                case ItemPropertyType.Class:
                case ItemPropertyType.Enum:
                    string name = property.InnerType != null ? property.InnerType.Name : property.Name;

                    if (property.Type != ItemPropertyType.Enum)
                    {
                        name = this.GetInnerTypeClassName(name);
                    }

                    //if (useCollections)
                    //{
                    //    switch (property.MappingType)
                    //    {
                    //        case ItemPropertyMappingType.Property:
                    //            typeName = name;
                    //            break;
                    //        case ItemPropertyMappingType.List:
                    //            typeName = "List<" + name + ">";
                    //            break;
                    //        case ItemPropertyMappingType.Array:
                    //            typeName = name + "[]";
                    //            break;
                    //    }
                    //}
                    //else
                    {
                        typeName = name + postfix;
                    }
                    break;
            }

            if (property.IsNullable && !this.notNullablePropertyTypes.Contains(property.Type))
            {
                typeName = typeName + "?";
            }

            if (useCollections)
            {
                switch (property.MappingType)
                {
                    case ItemPropertyMappingType.Property:
                        break;
                    case ItemPropertyMappingType.List:
                        typeName = "List<" + typeName + ">";
                        break;
                        //case ItemPropertyMappingType.Array:
                        //    typeName = typeName + "[]";
                        //    break;
                }
            }

            return typeName;
        }

        public virtual string GetIdTypeName(Item item)
        {
            switch (item.IdType)
            {
                case IdType.Int:
                    return "int";
                case IdType.Long:
                    return "long";
                case IdType.Guid:
                    return "Guid";
                case IdType.ObjectId:
                    return "string";
                default:
                    return string.Empty;
            }
        }

        public virtual string GetIdTypeAttributes(Item item)
        {
            switch (item.IdType)
            {
                case IdType.ObjectId:
                    return "string";
                default:
                    return string.Empty;
            }
        }

        public abstract Task<string> Generate(Item item, string @namespace, string baseClass);

        public virtual async Task<string> GetFileName(Item item)
        {
            return $"{FileNamePrefix}{item.Name}{FileNamePostfix}.{FileExtension}";
        }

        public virtual bool IsApplicationGenerator()
        {
            return GeneratorType.HasFlag(GeneratorType.Api) || GeneratorType.HasFlag(GeneratorType.Command) || GeneratorType.HasFlag(GeneratorType.CommandHandler) || GeneratorType.HasFlag(GeneratorType.Query) || GeneratorType.HasFlag(GeneratorType.Service);
        }
    }
}