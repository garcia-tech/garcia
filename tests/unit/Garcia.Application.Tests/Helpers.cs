﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
            mockRepository.Setup(repo => repo.GetAllAsync(false)).ReturnsAsync(list);
            mockRepository.Setup(repo => repo.GetByIdAsync(1, false)).ReturnsAsync(list.Find(x => x.Id == 1));

            mockRepository.Setup(repo => repo.AddAsync(It.IsAny<TestEntity>()))
                .ReturnsAsync(
                    (TestEntity entity) =>
                    {
                        list.Add(entity);
                        return 1;
                    });

            mockRepository.Setup(repo => repo.UpdateAsync(It.IsAny<TestEntity>()))
                .ReturnsAsync(1);

            mockRepository.Setup(repo => repo.DeleteAsync(It.IsAny<TestEntity>(), true))
                .ReturnsAsync(
                    (TestEntity entity) =>
                    {
                        list.Remove(entity);
                        return 1;
                    });

            return mockRepository;
        }

        public static Mock<IAsyncRepository<TestUser, long>> InitializeRepositoryInstance(string testusername, string testpassword)
        {
            var repository = new Mock<IAsyncRepository<TestUser, long>>();

            var list = new List<TestUser>()
            {
                new TestUser
                {
                    Id = 1,
                    Username = testusername,
                    Password = testpassword
                },
            };

            var returns = (Expression<Func<TestUser, bool>> expression, bool getSoftDeletes) =>
            {
                var result = list.Where(expression.Compile());
                return result.ToList();
            };

            repository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<TestUser, bool>>>(), false))
                .ReturnsAsync(returns);
            repository.Setup(x => x.AddAsync(It.IsAny<TestUser>()))
                .ReturnsAsync(1);
            return repository;
        }
    }
}
