using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Garcia.Application.ElasticSearch.Models;

namespace Garcia.Infrastructure.Tests
{
    public class TestDocument : ElasticSearchDocument<string>
    {
        public TestDocument()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Name { get; set; }
    }
}
