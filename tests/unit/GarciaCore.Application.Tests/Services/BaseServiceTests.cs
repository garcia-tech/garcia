using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using GarciaCore.Application.Contracts.Persistence;
using GarciaCore.Application.Services;
using Moq;
using Shouldly;

namespace GarciaCore.Application.Tests.Services
{
    public class BaseServiceTests
    {
        private readonly Mock<IAsyncRepository<TestEntity, long>> _mockRepository;
        private readonly BaseService<IAsyncRepository<TestEntity, long>, TestEntity, TestDto, long> _service;

        public BaseServiceTests()
        {
            _mockRepository = Helpers.InitializeRepositoryInstance();
            _service = new(_mockRepository.Object);
        }

        [Fact]
        public async Task GetAllAsync_Should_Success()
        {
            var response = await _service.GetAllAsync();
            response.Success.ShouldBeTrue();
            response.Result.Count().ShouldBeGreaterThan(0);
            var firstDto = response.Result.FirstOrDefault();
            firstDto.ShouldBeOfType<TestDto>();
        }

        [Theory(DisplayName = "GetIdById should be successful when the id entered is valid")]
        [InlineData(1)]
        public async Task GetById_Should_Success(long id)
        {
            var response = await _service.GetByIdAsync(id);
            response.Success.ShouldBeTrue();
            response.Result.ShouldNotBeNull();
            response.Result.ShouldBeOfType<TestDto>();
        }

        [Fact]
        public async Task AddAsync_Should_Success()
        {
            var testData = new TestEntity
            {
                Name = "TestData",
                Id = 10
            };

            var response = await _service.AddAsync(testData);
            response.Success.ShouldBeTrue();
            response.Status.ShouldBe(System.Net.HttpStatusCode.Created);
            response.Result.ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task UpdateAsync_Should_Success()
        {
            object testData = new
            {
                Name = "UpdatedTestData",
                Id = 1
            };

            var response = await _service.UpdateAsync(1, testData);
            response.Success.ShouldBeTrue();
            response.Status.ShouldBe(System.Net.HttpStatusCode.OK);
            response.Result.ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task DeleteAsync_Should_Success()
        {
            var response = await _service.DeleteAsync(1);
            response.Success.ShouldBeTrue();
            response.Status.ShouldBe(System.Net.HttpStatusCode.OK);
            response.Result.ShouldBeGreaterThan(0);
        }

        [Theory(DisplayName = "UpdateAsync should not be successful when the id entered does not match any entity")]
        [InlineData(120)]
        public async Task UpdateAsync_Should_Not_Be_Successful(long id)
        {
            var response = await _service.UpdateAsync(id, new {});
            response.Success.ShouldBeFalse();
            response.Status.ShouldBe(System.Net.HttpStatusCode.NotFound);
        }

        [Theory(DisplayName = "DeleteAsync should not be successful when the id entered does not match any entity")]
        [InlineData(120)]
        public async Task DeleteAsync_Should_Not_Be_Successful(long id)
        {
            var response = await _service.DeleteAsync(id);
            response.Success.ShouldBeFalse();
            response.Status.ShouldBe(System.Net.HttpStatusCode.NotFound);
        }

    }
}
