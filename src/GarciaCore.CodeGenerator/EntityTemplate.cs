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
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    
    #line 1 "C:\Users\vehbi\source\repos\garciacore\src\GarciaCore.CodeGenerator\EntityTemplate.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "17.0.0.0")]
    public partial class EntityTemplate : BaseTemplate
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public override string TransformText()
        {
            this.Write("\n");
            this.Write("\n");
            this.Write("\n");
            this.Write("\n");
            this.Write("\n");
            this.Write("\n\nnamespace ");
            
            #line 1 "C:\Users\vehbi\source\repos\garciacore\src\GarciaCore.CodeGenerator\EntityTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Namespace));
            
            #line default
            #line hidden
            this.Write("\n{\n    public partial class ");
            
            #line 1 "C:\Users\vehbi\source\repos\garciacore\src\GarciaCore.CodeGenerator\EntityTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Item.Name));
            
            #line default
            #line hidden
            this.Write(" : ");
            
            #line 1 "C:\Users\vehbi\source\repos\garciacore\src\GarciaCore.CodeGenerator\EntityTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(BaseClass));
            
            #line default
            #line hidden
            this.Write("\n    {\n");
            
            #line 1 "C:\Users\vehbi\source\repos\garciacore\src\GarciaCore.CodeGenerator\EntityTemplate.tt"

        foreach (var property in Item.Properties)
	    {
            string innerTypeName = this.generator.GetInnerTypeName(property);

            
            #line default
            #line hidden
            this.Write("\n        public ");
            
            #line 1 "C:\Users\vehbi\source\repos\garciacore\src\GarciaCore.CodeGenerator\EntityTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(innerTypeName));
            
            #line default
            #line hidden
            this.Write(" _");
            
            #line 1 "C:\Users\vehbi\source\repos\garciacore\src\GarciaCore.CodeGenerator\EntityTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(property.Name.ToPascalCase()));
            
            #line default
            #line hidden
            this.Write(";\n");
            
            #line 1 "C:\Users\vehbi\source\repos\garciacore\src\GarciaCore.CodeGenerator\EntityTemplate.tt"

        }

            
            #line default
            #line hidden
            this.Write("\n    }\n}\n\n");
            return this.GenerationEnvironment.ToString();
        }
        
        #line 1 "C:\Users\vehbi\source\repos\garciacore\src\GarciaCore.CodeGenerator\EntityTemplate.tt"

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
