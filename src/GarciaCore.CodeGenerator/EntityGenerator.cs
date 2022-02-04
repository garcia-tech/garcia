using System;
using System.Linq;

namespace GarciaCore.CodeGenerator
{
    public abstract class Generator
    {
        protected ItemPropertyType[] notNullablePropertyTypes = new ItemPropertyType[] { ItemPropertyType.String, ItemPropertyType.Class, ItemPropertyType.Unknown };

        public virtual string Generate<T>(Item item) where T : BaseTemplate
        {
            var template = CreateItem<T>();
            template.Item = item;
            var text = template.TransformText();
            return text;
        }

        protected virtual T CreateItem<T>() where T : BaseTemplate
        {
            return Activator.CreateInstance<T>();
        }

        protected virtual string GetInnerTypeClassName(string innerTypeName)
        {
            return innerTypeName;
        }

        public virtual string GetInnerTypeName(ItemProperty property, bool useCollections = true)
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

                    if (useCollections)
                    {
                        switch (property.MappingType)
                        {
                            case ItemPropertyMappingType.Property:
                                typeName = name;
                                break;
                            case ItemPropertyMappingType.List:
                                typeName = "List<" + name + ">";
                                break;
                            case ItemPropertyMappingType.Array:
                                typeName = name + "[]";
                                break;
                        }
                    }
                    else
                    {
                        typeName = name;
                    }
                    break;
            }

            if (property.IsNullable && !this.notNullablePropertyTypes.Contains(property.Type))
            {
                typeName = typeName + "?";
            }

            return typeName;
        }
    }

    public abstract class Generator<T> : Generator where T : BaseTemplate
    {
        public virtual string Generate(Item item)
        {
            return Generate<T>(item);
        }
    }

    public interface IEntityGenerator<T> where T : BaseTemplate
    {
        string Generate(Item item);
    }

    public class EntityGenerator : Generator<EntityTemplate>, IEntityGenerator<EntityTemplate>
    {
    }
}