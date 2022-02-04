// using System;
// using System.Collections;
// using System.Collections.Generic;
// using System.Linq;
// using System.Reflection;
// using System.Text;
// using System.Threading.Tasks;
// using System.IO;
//
// namespace GarciaCore.CodeGenerator;
//
// public abstract class Generator
// {
//     protected BaseTemplate template;
//     protected Dictionary<string, object> parameters = new Dictionary<string, object>();
//
//     protected ItemPropertyType[] notNullablePropertyTypes = new ItemPropertyType[]
//         {ItemPropertyType.String, ItemPropertyType.Class, ItemPropertyType.Unknown};
//
//     public bool GenerateInnerItems { get; protected set; }
//     protected bool generateEnumCode = false;
//     public Project Project { get; set; }
//     public abstract GeneratorContentType ContentType { get; }
//     public string BaseFolder { get; set; }
//     public abstract GeneratorType GeneratorType { get; }
//     public string BaseNamespace { get; set; }
//
//     public virtual string Namespace
//     {
//         get { return this.BaseNamespace; }
//     }
//
//     public List<string> Includes { get; set; }
//
//     //public Generator()
//     //{
//
//     //}
//
//     //public Generator(string baseFolder) : this(baseFolder, "")
//     //{
//     //    //this.GenerateInnerItems = true;
//     //    //this.BaseFolder = baseFolder;
//     //}
//
//     public Generator(string baseFolder, string baseNamespace)
//     {
//         this.GenerateInnerItems = true;
//         this.BaseFolder = baseFolder;
//         this.BaseNamespace = baseNamespace;
//         this.Includes = new List<string>();
//     }
//
//     protected virtual string InnerGenerate(Item item)
//     {
//         this.template = this.CreateTemplate();
//         this.template.Session = new Microsoft.VisualStudio.TextTemplating.TextTemplatingSession();
//         this.template.Session["Item"] = item;
//         this.template.Session["Namespace"] = this.Namespace;
//         this.template.Session["Includes"] = this.Includes.Aggregate((x, y) => "using " + x + ";\n" + y);
//         this.AddParameters();
//         this.template.Initialize();
//         string text = template.TransformText();
//         return text;
//     }
//
//     public string Generate(Item item)
//     {
//         if (item.IsEnum && !this.generateEnumCode)
//         {
//             return null;
//         }
//
//         string code = this.InnerGenerate(item);
//         return code;
//     }
//
//     public string Generate(Type type)
//     {
//         string text = string.Empty;
//         Item item = CodeGeneratorHelper.GetItemFromType(type);
//
//         if (item != null)
//         {
//             text = this.Generate(item);
//         }
//
//         return text;
//     }
//
//     protected virtual BaseTemplate CreateTemplate()
//     {
//         // TODO: 
//         return null;
//     }
//
//     protected string Replace(string value, string keyName, string keyValue)
//     {
//         return value.Replace("#" + keyName + "#", keyValue);
//     }
//
//     public virtual string GetInnerTypeName(ItemProperty property, bool useCollections = true)
//     {
//         string typeName = "";
//
//         switch (property.Type)
//         {
//             case ItemPropertyType.Integer:
//                 typeName = "int";
//                 break;
//             case ItemPropertyType.Double:
//                 typeName = "double";
//                 break;
//             case ItemPropertyType.Float:
//                 typeName = "float";
//                 break;
//             case ItemPropertyType.Decimal:
//                 typeName = "decimal";
//                 break;
//             case ItemPropertyType.DateTime:
//                 typeName = "DateTime";
//                 break;
//             case ItemPropertyType.TimeSpan:
//                 typeName = "TimeSpan";
//                 break;
//             case ItemPropertyType.String:
//                 typeName = "string";
//                 break;
//             case ItemPropertyType.Char:
//                 typeName = "char";
//                 break;
//             case ItemPropertyType.Boolean:
//                 typeName = "bool";
//                 break;
//             case ItemPropertyType.Class:
//             case ItemPropertyType.Enum:
//                 string name = property.InnerType != null ? property.InnerType.Name : property.Name;
//
//                 if (property.Type != ItemPropertyType.Enum)
//                 {
//                     name = this.GetInnerTypeClassName(name);
//                 }
//
//                 if (useCollections)
//                 {
//                     switch (property.MappingType)
//                     {
//                         case ItemPropertyMappingType.Property:
//                             typeName = name;
//                             break;
//                         case ItemPropertyMappingType.List:
//                             typeName = "List<" + name + ">";
//                             break;
//                         case ItemPropertyMappingType.Array:
//                             typeName = name + "[]";
//                             break;
//                     }
//                 }
//                 else
//                 {
//                     typeName = name;
//                 }
//
//                 break;
//         }
//
//         if (property.IsNullable && !this.notNullablePropertyTypes.Contains(property.Type))
//         {
//             typeName = typeName + "?";
//         }
//
//         return typeName;
//     }
//
//     protected virtual string GetInnerTypeClassName(string innerTypeName)
//     {
//         return innerTypeName;
//     }
//
//     [Obsolete]
//     protected Type GetInnerType(ItemProperty property)
//     {
//         Type type = null;
//
//         switch (property.Type)
//         {
//             case ItemPropertyType.Integer:
//                 type = typeof(int);
//                 break;
//             case ItemPropertyType.Double:
//                 type = typeof(double);
//                 break;
//             case ItemPropertyType.Float:
//                 type = typeof(float);
//                 break;
//             case ItemPropertyType.Decimal:
//                 type = typeof(decimal);
//                 break;
//             case ItemPropertyType.DateTime:
//                 type = typeof(DateTime);
//                 break;
//             case ItemPropertyType.TimeSpan:
//                 type = typeof(TimeSpan);
//                 break;
//             case ItemPropertyType.String:
//                 type = typeof(string);
//                 break;
//             case ItemPropertyType.Char:
//                 type = typeof(char);
//                 break;
//             case ItemPropertyType.Boolean:
//                 type = typeof(bool);
//                 break;
//             case ItemPropertyType.Class:
//             case ItemPropertyType.Enum:
//                 break;
//         }
//
//         return type;
//     }
//
//     protected void AddParameters()
//     {
//         this.AddParameter("FrameworkType", GarciaGeneratorConfiguration.FrameworkType);
//         this.AddParameter("OrmType", GarciaGeneratorConfiguration.OrmType);
//         this.InitializeParameters();
//
//         if (this.template != null && this.template.Session != null && this.parameters != null && parameters.Count != 0)
//         {
//             IDictionaryEnumerator ienum = this.parameters.GetEnumerator();
//
//             while (ienum.MoveNext())
//             {
//                 this.template.Session[ienum.Key.ToString()] = ienum.Value;
//             }
//         }
//     }
//
//     protected void AddParameter(string key, object value)
//     {
//         //if(this.parameters.ContainsKey(key))
//         {
//             this.parameters[key] = value;
//         }
//         //else
//         //{
//         //    this.AddParameter(key,  value);
//         //}
//     }
//
//     internal abstract void InitializeParameters();
//     public abstract string GetFileName(Item item);
//     protected abstract List<string> GetFoldersAndFile(Item item);
//
//     public List<string> GetFolders(Item item, string baseFolder)
//     {
//         //string path = baseFolder + "\\" + this.GetFileName(item);
//         //string code = this.Generate(item);
//
//         //if (!string.IsNullOrEmpty(path))
//         //{
//         //    Directory.CreateDirectory(Path.GetDirectoryName(path));
//         //    File.WriteAllText(path, code);
//         //}
//
//         List<string> value = new List<string>();
//         value.Add(baseFolder);
//
//         if (!string.IsNullOrEmpty(this.BaseFolder))
//         {
//             value.Add(this.BaseFolder);
//         }
//
//         List<string> foldersAndFile = this.GetFoldersAndFile(item);
//         value.AddRange(foldersAndFile);
//         return value;
//     }
// }
//
// public abstract class Generator<T> : Generator
//     where T : BaseTemplate
// {
//     public Generator(string baseFolder, string baseNamespace) : base(baseFolder, baseNamespace)
//     {
//     }
//
//     protected override BaseTemplate CreateTemplate()
//     {
//         return Activator.CreateInstance<T>();
//     }
// }