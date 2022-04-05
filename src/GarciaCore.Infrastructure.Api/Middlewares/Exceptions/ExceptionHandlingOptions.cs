using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarciaCore.Infrastructure.Api.Middlewares.Exceptions
{
    public class ExceptionHandlingOptions
    {
        public string DefaultExceptionTitle { get; set; }
        public string ResponseContentType { get; set; }
    }
}
