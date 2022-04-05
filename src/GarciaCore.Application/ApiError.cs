using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GarciaCore.Application
{
    public class ApiError
    {
        private IList<string> _errorMessageList;

        public ApiError()
        {

        }

        public ApiError(string title, string detail)
        {
            Title = title;
            Detail = detail;
        }

        public string Id { get; } = Guid.NewGuid().ToString();
        public string Code { get; set; }
        public IList<string> Links { get; set; }
        public string Title { get; set; }
        public string Detail { get; set; }
        public DateTimeOffset CreatedOn { get; } = DateTime.UtcNow;
        public int? StatusCode { get; private set; }

        public IList<string> ErrorMessageList
        {
            get => _errorMessageList;
            set => _errorMessageList = value;
        }

        public void AddErrors(string errorMessage)
        {
            ErrorMessageList ??= new List<string>();
            ErrorMessageList.Add(errorMessage);
        }

        public void AddLink(string link)
        {
            Links ??= new List<string>();
            Links.Add(link);
        }
        
        public void SetStatusCode(HttpStatusCode code)
        {
            StatusCode = (int)code;
        }

        public override string ToString()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            return JsonSerializer.Serialize(this, options);
        }

    }
}
