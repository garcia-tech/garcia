using System.Threading.Tasks;
using Garcia.Infrastructure.ElasticSearch;
using Microsoft.Extensions.Options;
using Moq;
using Nest;
using Shouldly;
using Xunit;

namespace Garcia.Infrastructure.Tests
{
    public class ElasticServiceTests
    {
        private static readonly Mock<IOptions<ElasticSearchSettings>> _mockOptions = new();
        private static ElasticSearchConnectionFactory _connectionFactory;
        private static ElasticSearchService<TestDocument, string> _service;

        public ElasticServiceTests()
        {
            var settings = new ElasticSearchSettings
            {
                Uri = "http://localhost:9200/"
            };

            _mockOptions.Setup(s => s.Value).Returns(settings);
            _connectionFactory = new ElasticSearchConnectionFactory(_mockOptions.Object);
            _service = new ElasticSearchService<TestDocument, string>(_connectionFactory);
        }

        [Fact]
        public async Task SetDocumentAsync_Should_Success()
        {
            var document = new TestDocument();
            var result = await _service.SetDocumentAsync(document);
            result.ShouldBeGreaterThanOrEqualTo(0);
        }

        [Fact]
        public async Task SearchAsync_And_Overloads_Should_Success()
        {
            var document = new TestDocument();
            document.Name = "testDoc";
            var setResult = await _service.SetDocumentAsync(document);
            setResult.ShouldBeGreaterThanOrEqualTo(0);
            var result = await _service.SearchAsync(x => x.Name, "testDoc");
            result.Documents.Count.ShouldBeGreaterThan(0);
            var query = new SearchDescriptor<TestDocument>()
                .Query(q => q
                    .Match(m => m
                    .Field(x => x.Name)
                    .Query("testDoc")
                    )
                );
            result = await _service.SearchAsync(query);
            result.Documents.Count.ShouldBeGreaterThan(0);
            var request = new SearchRequest<TestDocument>()
            {
                Query = new MatchAllQuery()
            };
            result = await _service.SearchAsync(request);
            result.Documents.Count.ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task SearchMultiMatchAsync_Should_Success()
        {
            var document = new TestDocument();
            document.Name = "testMulti";
            var setResult = await _service.SetDocumentAsync(document);
            setResult.ShouldBeGreaterThanOrEqualTo(0);
            var result = await _service.SearchMultiMatchAsync(x =>
                    x.Fields(f => f.Name, f => f.Id),
                    "testMulti"
                );
            result.Documents.Count.ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Success()
        {
            var document = new TestDocument();
            document.Id = "1";
            var setResult = await _service.SetDocumentAsync(document);
            setResult.ShouldBeGreaterThanOrEqualTo(0);
            var result = await _service.GetByIdAsync("1");
            result.ShouldNotBeNull();
        }

        [Fact]
        public async Task DeleteAsync_Should_Success()
        {
            var document = new TestDocument();
            document.Id = "1";
            var setResult = await _service.SetDocumentAsync(document);
            setResult.ShouldBeGreaterThanOrEqualTo(0);
            var result = await _service.GetByIdAsync("1");
            result.ShouldNotBeNull();
            await _service.DeleteAsync(result);
            result = await _service.GetByIdAsync("1");
            result.ShouldBeNull();
        }
    }
}
