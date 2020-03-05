using Microsoft.Extensions.Logging;
using Moq;
using Polyrific.Project.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Polyrific.Project.Test.Core
{
    public class BaseServiceTests
    {
        private readonly Mock<IRepository<DummyTestEntity>> _repository;
        private readonly Mock<ILogger<DummyTestService>> _logger;

        public BaseServiceTests()
        {
            _repository = new Mock<IRepository<DummyTestEntity>>();
            _logger = new Mock<ILogger<DummyTestService>>();
        }

        [Fact]
        public async void Delete_Success()
        {
            _repository.Setup(r => r.Delete(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var service = new DummyTestService(_repository.Object, _logger.Object);
            var result = await service.Delete(1);

            Assert.True(result.Success);
            Assert.Equal(LogLevel.Information, _logger.Invocations[0].Arguments[0]);
        }

        [Fact]
        public async void Delete_ThrowException()
        {
            _repository.Setup(r => r.Delete(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new DummyException());

            var service = new DummyTestService(_repository.Object, _logger.Object);
            var result = await service.Delete(1);

            Assert.False(result.Success);
            Assert.StartsWith($"Failed to delete {nameof(DummyTestEntity)}", result.Errors.First());
            Assert.Equal(LogLevel.Error, _logger.Invocations[0].Arguments[0]);
        }

        [Fact]
        public async void Get_Success()
        {
            _repository.Setup(r => r.GetById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((int id, CancellationToken token) => new DummyTestEntity { Id = id });

            var service = new DummyTestService(_repository.Object, _logger.Object);
            var entity = await service.Get(1);

            Assert.NotNull(entity);
            Assert.Equal(1, entity.Id);
        }

        [Fact]
        public async void Get_ThrowException()
        {
            _repository.Setup(r => r.GetById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new DummyException());

            var service = new DummyTestService(_repository.Object, _logger.Object);
            var entity = await service.Get(1);

            Assert.Null(entity);
            Assert.Equal(LogLevel.Error, _logger.Invocations[0].Arguments[0]);
        }

        [Fact]
        public async void GetPageData_Success()
        {
            var page = 1;
            var pageSize = 5;
            var total = 6;

            _repository.Setup(r => r.GetBySpec(It.IsAny<ISpecification<DummyTestEntity>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() =>
                {
                    var items = new List<DummyTestEntity>();
                    for (int i = 0; i < pageSize; i++)
                    {
                        items.Add(new DummyTestEntity());
                    }

                    return items;
                });

            _repository.Setup(r => r.CountBySpec(It.IsAny<ISpecification<DummyTestEntity>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(total);

            var service = new DummyTestService(_repository.Object, _logger.Object);
            var pagingResult = await service.GetPageData(page, pageSize);

            Assert.NotEmpty(pagingResult.Items);
            Assert.Equal(pageSize, pagingResult.Items.Count());
            Assert.Equal(page, pagingResult.Page);
            Assert.Equal(pageSize, pagingResult.PageSize);
            Assert.Equal(total, pagingResult.TotalCount);
        }

        [Fact]
        public async void GetPageData_ThrowException()
        {
            _repository.Setup(r => r.GetBySpec(It.IsAny<ISpecification<DummyTestEntity>>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new DummyException());

            var service = new DummyTestService(_repository.Object, _logger.Object);
            var pagingResult = await service.GetPageData(1, 5);

            Assert.Empty(pagingResult.Items);
            Assert.Equal(LogLevel.Error, _logger.Invocations[0].Arguments[0]);
        }

        [Fact]
        public async void Save_New_CreateSuccess()
        {
            _repository.Setup(r => r.Create(It.IsAny<DummyTestEntity>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            _repository.Setup(r => r.GetById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DummyTestEntity { Id = 1 });

            var service = new DummyTestService(_repository.Object, _logger.Object);
            var result = await service.Save(new DummyTestEntity(), true);

            Assert.True(result.Success);
            Assert.NotNull(result.Item);
            Assert.Equal(1, result.Item.Id);
            Assert.Equal(LogLevel.Information, _logger.Invocations[0].Arguments[0]);
        }

        [Fact]
        public async void Save_New_NotCreate()
        {
            var service = new DummyTestService(_repository.Object, _logger.Object);
            var result = await service.Save(new DummyTestEntity());

            Assert.False(result.Success);
            Assert.StartsWith($"{nameof(DummyTestEntity)} was not created", result.Errors.First());
        }

        [Fact]
        public async void Save_New_ThrowException()
        {
            _repository.Setup(r => r.Create(It.IsAny<DummyTestEntity>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new DummyException());

            var service = new DummyTestService(_repository.Object, _logger.Object);
            var result = await service.Save(new DummyTestEntity(), true);

            Assert.False(result.Success);
            Assert.StartsWith($"Failed to create {nameof(DummyTestEntity)}", result.Errors.First());
            Assert.Equal(LogLevel.Error, _logger.Invocations[0].Arguments[0]);
        }

        [Fact]
        public async void Save_NotFound_NotCreate()
        {
            _repository.Setup(r => r.GetById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((DummyTestEntity)null);

            var service = new DummyTestService(_repository.Object, _logger.Object);
            var result = await service.Save(new DummyTestEntity { Id = 1 });

            Assert.False(result.Success);
            Assert.StartsWith($"{nameof(DummyTestEntity)} (Id = 1) doesn't exist", result.Errors.First());
        }

        [Fact]
        public async void Save_Existing_UpdateSuccess()
        {
            _repository.Setup(r => r.GetById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DummyTestEntity { Id = 1 });

            _repository.Setup(r => r.Update(It.IsAny<DummyTestEntity>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var service = new DummyTestService(_repository.Object, _logger.Object);
            var result = await service.Save(new DummyTestEntity() { Id = 1 });

            Assert.True(result.Success);
            Assert.NotNull(result.Item);
            Assert.Equal(1, result.Item.Id);
            Assert.Equal(LogLevel.Information, _logger.Invocations[0].Arguments[0]);
        }

        [Fact]
        public async void Save_Existing_ThrowException()
        {
            _repository.Setup(r => r.GetById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DummyTestEntity { Id = 1 });

            _repository.Setup(r => r.Update(It.IsAny<DummyTestEntity>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new DummyException());

            var service = new DummyTestService(_repository.Object, _logger.Object);
            var result = await service.Save(new DummyTestEntity() { Id = 1 });

            Assert.False(result.Success);
            Assert.StartsWith($"Failed to update {nameof(DummyTestEntity)} (Id = 1)", result.Errors.First());
            Assert.Equal(LogLevel.Error, _logger.Invocations[0].Arguments[0]);
        }
    }

    
}
