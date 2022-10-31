using System;
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
