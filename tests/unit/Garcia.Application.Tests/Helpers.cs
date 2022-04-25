using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Garcia.Application.Contracts.Persistence;
using Moq;

namespace Garcia.Application.Tests
{
    public static class Helpers
    {
        public static Mock<IAsyncRepository<TestEntity, long>> InitializeRepositoryInstance()
        {
            var list = new List<TestEntity>()
            {
                new TestEntity
                {
                    Id = 1,
                    Name = "Test1"
                },
                new TestEntity
                {
                    Id = 2,
                    Name = "Test2"
                },
                new TestEntity
                {
                    Id = 3,
                    Name = "Test3"
                },
                new TestEntity
                {
                    Id = 4,
                    Name = "Test4"
                }
            };

            var mockRepository = new Mock<IAsyncRepository<TestEntity, long>>();
            mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(list);
            mockRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(list.Find(x => x.Id == 1));

            mockRepository.Setup(repo => repo.AddAsync(It.IsAny<TestEntity>()))
                .ReturnsAsync(
                    (TestEntity entity) =>
                    {
                        list.Add(entity);
                        return 1;
                    });

            mockRepository.Setup(repo => repo.UpdateAsync(It.IsAny<TestEntity>()))
                .ReturnsAsync(1);

            mockRepository.Setup(repo => repo.DeleteAsync(It.IsAny<TestEntity>()))
                .ReturnsAsync(
                    (TestEntity entity) =>
                    {
                        list.Remove(entity);
                        return 1;
                    });

            return mockRepository;
        }
    }
}
