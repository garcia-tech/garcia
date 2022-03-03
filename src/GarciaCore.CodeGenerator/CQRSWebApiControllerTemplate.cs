// ------------------------------------------------------------------------------
// <auto-generated>
//     Bu kod bir ara� taraf�ndan olu�turuldu.
//     �al��ma Zaman� S�r�m�: 17.0.0.0
//  
//     Bu dosyada yap�lacak de�i�iklikler hatal� davran��a neden olabilir ve
//     kod yeniden �retildi�inde kaybolabilir.
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
    
    #line 1 "C:\Users\Se�kin\source\repos\garciacore\src\GarciaCore.CodeGenerator\CQRSWebApiControllerTemplate.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "17.0.0.0")]
    public partial class CQRSWebApiControllerTemplate : BaseTemplate
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public override string TransformText()
        {
            this.Write("/*\r\n\tThis file was generated automatically by Garcia Framework. \r\n\tDo not edit ma" +
                    "nually. \r\n\tAdd a new partial class with the same name if you want to add extra f" +
                    "unctionality.\r\n*/");
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
            this.Write("\r\nusing MediatR;\r\n\r\nnamespace ");
            
            #line 13 "C:\Users\Se�kin\source\repos\garciacore\src\GarciaCore.CodeGenerator\CQRSWebApiControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Namespace));
            
            #line default
            #line hidden
            this.Write("\r\n{\r\n    public partial class ");
            
            #line 15 "C:\Users\Se�kin\source\repos\garciacore\src\GarciaCore.CodeGenerator\CQRSWebApiControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Item.Name));
            
            #line default
            #line hidden
            this.Write("Controller : ");
            
            #line 15 "C:\Users\Se�kin\source\repos\garciacore\src\GarciaCore.CodeGenerator\CQRSWebApiControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(BaseClass));
            
            #line default
            #line hidden
            this.Write("\r\n    {\r\n        private I");
            
            #line 17 "C:\Users\Se�kin\source\repos\garciacore\src\GarciaCore.CodeGenerator\CQRSWebApiControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Item.Name));
            
            #line default
            #line hidden
            this.Write("Service _");
            
            #line 17 "C:\Users\Se�kin\source\repos\garciacore\src\GarciaCore.CodeGenerator\CQRSWebApiControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Item.Name.ToCamelCase()));
            
            #line default
            #line hidden
            this.Write("Service;\r\n        private I");
            
            #line 18 "C:\Users\Se�kin\source\repos\garciacore\src\GarciaCore.CodeGenerator\CQRSWebApiControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Item.Name));
            
            #line default
            #line hidden
            this.Write("Repository _");
            
            #line 18 "C:\Users\Se�kin\source\repos\garciacore\src\GarciaCore.CodeGenerator\CQRSWebApiControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Item.Name.ToCamelCase()));
            
            #line default
            #line hidden
            this.Write("Repository;\r\n\r\n        public ");
            
            #line 20 "C:\Users\Se�kin\source\repos\garciacore\src\GarciaCore.CodeGenerator\CQRSWebApiControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Item.Name));
            
            #line default
            #line hidden
            this.Write("Controller(IOptions<Settings> settings, ");
            
            #line 20 "C:\Users\Se�kin\source\repos\garciacore\src\GarciaCore.CodeGenerator\CQRSWebApiControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Item.Name));
            
            #line default
            #line hidden
            this.Write("Repository ");
            
            #line 20 "C:\Users\Se�kin\source\repos\garciacore\src\GarciaCore.CodeGenerator\CQRSWebApiControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Item.Name));
            
            #line default
            #line hidden
            this.Write("Repository, I");
            
            #line 20 "C:\Users\Se�kin\source\repos\garciacore\src\GarciaCore.CodeGenerator\CQRSWebApiControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Item.Name));
            
            #line default
            #line hidden
            this.Write("Service ");
            
            #line 20 "C:\Users\Se�kin\source\repos\garciacore\src\GarciaCore.CodeGenerator\CQRSWebApiControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Item.Name));
            
            #line default
            #line hidden
            this.Write("Service, IMediator mediator, IQrCodeService qrCodeService) : base(settings, repos" +
                    "itory, mediator)\r\n        {\r\n            _");
            
            #line 22 "C:\Users\Se�kin\source\repos\garciacore\src\GarciaCore.CodeGenerator\CQRSWebApiControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Item.Name.ToCamelCase()));
            
            #line default
            #line hidden
            this.Write("Service = ");
            
            #line 22 "C:\Users\Se�kin\source\repos\garciacore\src\GarciaCore.CodeGenerator\CQRSWebApiControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Item.Name.ToCamelCase()));
            
            #line default
            #line hidden
            this.Write("Service;\r\n            _");
            
            #line 23 "C:\Users\Se�kin\source\repos\garciacore\src\GarciaCore.CodeGenerator\CQRSWebApiControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Item.Name.ToCamelCase()));
            
            #line default
            #line hidden
            this.Write("Repository = ");
            
            #line 23 "C:\Users\Se�kin\source\repos\garciacore\src\GarciaCore.CodeGenerator\CQRSWebApiControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Item.Name.ToCamelCase()));
            
            #line default
            #line hidden
            this.Write("Repository;\r\n        }\r\n\r\n        [HttpPost(Name = \"Create");
            
            #line 26 "C:\Users\Se�kin\source\repos\garciacore\src\GarciaCore.CodeGenerator\CQRSWebApiControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Item.Name));
            
            #line default
            #line hidden
            this.Write("\")]\r\n        public async Task<ActionResult<");
            
            #line 27 "C:\Users\Se�kin\source\repos\garciacore\src\GarciaCore.CodeGenerator\CQRSWebApiControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Item.Name));
            
            #line default
            #line hidden
            this.Write(">> Create");
            
            #line 27 "C:\Users\Se�kin\source\repos\garciacore\src\GarciaCore.CodeGenerator\CQRSWebApiControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Item.Name));
            
            #line default
            #line hidden
            this.Write("Command(Create");
            
            #line 27 "C:\Users\Se�kin\source\repos\garciacore\src\GarciaCore.CodeGenerator\CQRSWebApiControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Item.Name));
            
            #line default
            #line hidden
            this.Write(@"Command command)
        {
            var response = await _mediator.Send(command);

            if (response.IsValidResponse)
            {
                return StatusCode(response.StatusCode, response.Error);
            }

            return Created($""/");
            
            #line 36 "C:\Users\Se�kin\source\repos\garciacore\src\GarciaCore.CodeGenerator\CQRSWebApiControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Item.Name));
            
            #line default
            #line hidden
            this.Write("s/{command.Id}\", response.Result);\r\n        }\r\n\r\n        [HttpPut(Name = \"Update");
            
            #line 39 "C:\Users\Se�kin\source\repos\garciacore\src\GarciaCore.CodeGenerator\CQRSWebApiControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Item.Name));
            
            #line default
            #line hidden
            this.Write("\")]\r\n        public async Task<ActionResult<");
            
            #line 40 "C:\Users\Se�kin\source\repos\garciacore\src\GarciaCore.CodeGenerator\CQRSWebApiControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Item.Name));
            
            #line default
            #line hidden
            this.Write(">> Update");
            
            #line 40 "C:\Users\Se�kin\source\repos\garciacore\src\GarciaCore.CodeGenerator\CQRSWebApiControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Item.Name));
            
            #line default
            #line hidden
            this.Write("Command(Update");
            
            #line 40 "C:\Users\Se�kin\source\repos\garciacore\src\GarciaCore.CodeGenerator\CQRSWebApiControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Item.Name));
            
            #line default
            #line hidden
            this.Write(@"Command command)
        {
            var response = await _mediator.Send(command);

            if (response.IsValidResponse)
            {
                return StatusCode(response.StatusCode, response.Error);
            }

            return Created($""/");
            
            #line 49 "C:\Users\Se�kin\source\repos\garciacore\src\GarciaCore.CodeGenerator\CQRSWebApiControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Item.Name));
            
            #line default
            #line hidden
            this.Write("s/{command.Id}\", response.Result);\r\n        }\r\n\r\n        [HttpDelete(\"{id}\", Name" +
                    " = \"Delete");
            
            #line 52 "C:\Users\Se�kin\source\repos\garciacore\src\GarciaCore.CodeGenerator\CQRSWebApiControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Item.Name));
            
            #line default
            #line hidden
            this.Write("\")]\r\n        public async Task<ActionResult<");
            
            #line 53 "C:\Users\Se�kin\source\repos\garciacore\src\GarciaCore.CodeGenerator\CQRSWebApiControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Item.Name));
            
            #line default
            #line hidden
            this.Write(">> Delete");
            
            #line 53 "C:\Users\Se�kin\source\repos\garciacore\src\GarciaCore.CodeGenerator\CQRSWebApiControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Item.Name));
            
            #line default
            #line hidden
            this.Write("Command(Delete");
            
            #line 53 "C:\Users\Se�kin\source\repos\garciacore\src\GarciaCore.CodeGenerator\CQRSWebApiControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Item.Name));
            
            #line default
            #line hidden
            this.Write(@"Command command)
        {
            var response = await _mediator.Send(command);

            if (response.IsValidResponse)
            {
                return StatusCode(response.StatusCode, response.Error);
            }

            return NoContent();
        }

        [HttpGet(""api/");
            
            #line 65 "C:\Users\Se�kin\source\repos\garciacore\src\GarciaCore.CodeGenerator\CQRSWebApiControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Item.Name));
            
            #line default
            #line hidden
            this.Write("\", Name = \"GetAll");
            
            #line 65 "C:\Users\Se�kin\source\repos\garciacore\src\GarciaCore.CodeGenerator\CQRSWebApiControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Item.Name));
            
            #line default
            #line hidden
            this.Write("s\")]\r\n        public async Task<List<");
            
            #line 66 "C:\Users\Se�kin\source\repos\garciacore\src\GarciaCore.CodeGenerator\CQRSWebApiControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Item.Name));
            
            #line default
            #line hidden
            this.Write(">> GetAll");
            
            #line 66 "C:\Users\Se�kin\source\repos\garciacore\src\GarciaCore.CodeGenerator\CQRSWebApiControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Item.Name));
            
            #line default
            #line hidden
            this.Write("s()\r\n        {\r\n            var items = await _repository.GetAllAsync();\r\n       " +
                    "     return Ok(items);\r\n        }\r\n\r\n        [HttpGet(\"api/");
            
            #line 72 "C:\Users\Se�kin\source\repos\garciacore\src\GarciaCore.CodeGenerator\CQRSWebApiControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Item.Name));
            
            #line default
            #line hidden
            this.Write("/{id}\", Name = \"Get");
            
            #line 72 "C:\Users\Se�kin\source\repos\garciacore\src\GarciaCore.CodeGenerator\CQRSWebApiControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Item.Name));
            
            #line default
            #line hidden
            this.Write("\")]\r\n        public async Task<");
            
            #line 73 "C:\Users\Se�kin\source\repos\garciacore\src\GarciaCore.CodeGenerator\CQRSWebApiControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Item.Name));
            
            #line default
            #line hidden
            this.Write("> Get");
            
            #line 73 "C:\Users\Se�kin\source\repos\garciacore\src\GarciaCore.CodeGenerator\CQRSWebApiControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Item.Name));
            
            #line default
            #line hidden
            this.Write("(");
            
            #line 73 "C:\Users\Se�kin\source\repos\garciacore\src\GarciaCore.CodeGenerator\CQRSWebApiControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(IdTypeName));
            
            #line default
            #line hidden
            this.Write(" id)\r\n        {\r\n            var item = await _repository.GetAsync(id);\r\n\r\n      " +
                    "      if (item != null)\r\n            {\r\n                return Ok(item);\r\n      " +
                    "      }\r\n\r\n            return NotFound();\r\n        }\r\n");
            
            #line 84 "C:\Users\Se�kin\source\repos\garciacore\src\GarciaCore.CodeGenerator\CQRSWebApiControllerTemplate.tt"

        foreach (var property in Item.Properties.Where(x => x.MappingType == ItemPropertyMappingType.List && x.Type == ItemPropertyType.Class))
	    {
            string innerTypeName = this.generator.GetInnerTypeName(property);

            
            #line default
            #line hidden
            this.Write("        \r\n        [HttpGet(\"api/");
            
            #line 90 "C:\Users\Se�kin\source\repos\garciacore\src\GarciaCore.CodeGenerator\CQRSWebApiControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Item.Name));
            
            #line default
            #line hidden
            this.Write("/{id}/");
            
            #line 90 "C:\Users\Se�kin\source\repos\garciacore\src\GarciaCore.CodeGenerator\CQRSWebApiControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(property.Name.ToPascalCase()));
            
            #line default
            #line hidden
            this.Write("\")]\r\n        public async Task<");
            
            #line 91 "C:\Users\Se�kin\source\repos\garciacore\src\GarciaCore.CodeGenerator\CQRSWebApiControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(innerTypeName));
            
            #line default
            #line hidden
            this.Write("> Get");
            
            #line 91 "C:\Users\Se�kin\source\repos\garciacore\src\GarciaCore.CodeGenerator\CQRSWebApiControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(property.Name.ToPascalCase()));
            
            #line default
            #line hidden
            this.Write("(");
            
            #line 91 "C:\Users\Se�kin\source\repos\garciacore\src\GarciaCore.CodeGenerator\CQRSWebApiControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(IdTypeName));
            
            #line default
            #line hidden
            this.Write(" id)\r\n        {\r\n            var item = await _repository.GetAsync(id);\r\n\r\n      " +
                    "      if (item != null)\r\n            {\r\n                return Ok(item.");
            
            #line 97 "C:\Users\Se�kin\source\repos\garciacore\src\GarciaCore.CodeGenerator\CQRSWebApiControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(property.Name));
            
            #line default
            #line hidden
            this.Write(");\r\n            }\r\n\r\n            return NotFound();\r\n        }\r\n");
            
            #line 102 "C:\Users\Se�kin\source\repos\garciacore\src\GarciaCore.CodeGenerator\CQRSWebApiControllerTemplate.tt"

        }

            
            #line default
            #line hidden
            this.Write("    }\r\n}\r\n\r\n");
            return this.GenerationEnvironment.ToString();
        }
        
        #line 108 "C:\Users\Se�kin\source\repos\garciacore\src\GarciaCore.CodeGenerator\CQRSWebApiControllerTemplate.tt"

    protected override Generator CreateGenerator()
	{
		return new CQRSWebApiControllerGenerator();
	}

        
        #line default
        #line hidden
    }
    
    #line default
    #line hidden
}
