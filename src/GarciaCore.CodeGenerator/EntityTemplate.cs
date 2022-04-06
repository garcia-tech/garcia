// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 17.0.0.0
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace GarciaCore.CodeGenerator
{
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;
    using GarciaCore.CodeGenerator;
    using GarciaCore.Infrastructure;
    using GarciaCore.Application;
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    
    #line 1 "C:\Users\vehbi\source\repos\projects\garciacore\src\GarciaCore.CodeGenerator\EntityTemplate.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "17.0.0.0")]
    public partial class EntityTemplate : BaseTemplate
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public override string TransformText()
        {
            this.Write("/*\r\n\tThis file was generated automatically by Garcia Framework.\r\n*/");
            this.Write("\r\n");
            this.Write(@"using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using GarciaCore.Infrastructure;
using GarciaCore.Domain;
using GarciaCore.Persistence;
using System.Threading;
using System.Threading.Tasks;");
            this.Write("\r\nusing System.ComponentModel.DataAnnotations.Schema;\r\nusing System.Text.Json.Ser" +
                    "ialization;\r\n");
            
            #line 13 "C:\Users\vehbi\source\repos\projects\garciacore\src\GarciaCore.CodeGenerator\EntityTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(GetUsings()));
            
            #line default
            #line hidden
            this.Write("\r\nnamespace ");
            
            #line 14 "C:\Users\vehbi\source\repos\projects\garciacore\src\GarciaCore.CodeGenerator\EntityTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Namespace));
            
            #line default
            #line hidden
            this.Write("\r\n{\r\n    public partial class ");
            
            #line 16 "C:\Users\vehbi\source\repos\projects\garciacore\src\GarciaCore.CodeGenerator\EntityTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Item.Name));
            
            #line default
            #line hidden
            this.Write(" : Entity<");
            
            #line 16 "C:\Users\vehbi\source\repos\projects\garciacore\src\GarciaCore.CodeGenerator\EntityTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(IdTypeName));
            
            #line default
            #line hidden
            this.Write(">\r\n    {\r\n");
            
            #line 18 "C:\Users\vehbi\source\repos\projects\garciacore\src\GarciaCore.CodeGenerator\EntityTemplate.tt"

        foreach (var property in Item.Properties)
	    {
            string innerTypeName = generator.GetInnerTypeName(property);
            
            if (property.Type == ItemPropertyType.Photo && property.MappingType == ItemPropertyMappingType.List)
            {

            
            #line default
            #line hidden
            this.Write("        [JsonIgnore]\r\n        public string ");
            
            #line 27 "C:\Users\vehbi\source\repos\projects\garciacore\src\GarciaCore.CodeGenerator\EntityTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(property.Name.ToPascalCase()));
            
            #line default
            #line hidden
            this.Write("String { get { return string.Join(\',\', ");
            
            #line 27 "C:\Users\vehbi\source\repos\projects\garciacore\src\GarciaCore.CodeGenerator\EntityTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(property.Name.ToPascalCase()));
            
            #line default
            #line hidden
            this.Write("); } set { ");
            
            #line 27 "C:\Users\vehbi\source\repos\projects\garciacore\src\GarciaCore.CodeGenerator\EntityTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(property.Name.ToPascalCase()));
            
            #line default
            #line hidden
            this.Write(" = value.Split(\',\').ToList(); } }\r\n        [NotMapped]\r\n");
            
            #line 29 "C:\Users\vehbi\source\repos\projects\garciacore\src\GarciaCore.CodeGenerator\EntityTemplate.tt"

            }

            
            #line default
            #line hidden
            this.Write("        public ");
            
            #line 32 "C:\Users\vehbi\source\repos\projects\garciacore\src\GarciaCore.CodeGenerator\EntityTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(innerTypeName));
            
            #line default
            #line hidden
            this.Write(" ");
            
            #line 32 "C:\Users\vehbi\source\repos\projects\garciacore\src\GarciaCore.CodeGenerator\EntityTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(property.Name.ToPascalCase()));
            
            #line default
            #line hidden
            this.Write(" { get; set; }\r\n");
            
            #line 33 "C:\Users\vehbi\source\repos\projects\garciacore\src\GarciaCore.CodeGenerator\EntityTemplate.tt"

        }

            
            #line default
            #line hidden
            this.Write("    }\r\n}\r\n");
            return this.GenerationEnvironment.ToString();
        }
        
        #line 38 "C:\Users\vehbi\source\repos\projects\garciacore\src\GarciaCore.CodeGenerator\EntityTemplate.tt"

    protected override Generator CreateGenerator()
	{
		return new EntityGenerator();
	}

        
        #line default
        #line hidden
    }
    
    #line default
    #line hidden
}
