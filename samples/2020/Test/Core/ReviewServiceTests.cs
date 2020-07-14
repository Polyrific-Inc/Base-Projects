using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Entities;
using Core.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Polyrific.Project.Core;
using Polyrific.Project.Core.Exceptions;
using Xunit;

namespace Test.Core
{
    public class ReviewServiceTests
    {
        private readonly Mock<IRepository<Review>> _reviewRepository;
        private readonly Mock<IEventStorage> _eventStorage;
        private readonly Mock<ILogger<ReviewService>> _logger;

        public ReviewServiceTests()
        {
            _reviewRepository = new Mock<IRepository<Review>>();
            _eventStorage = new Mock<IEventStorage>();
            _logger = new Mock<ILogger<ReviewService>>();
        }

        [Fact]
        public async void SaveReview_New_Success()
        {
            _reviewRepository.Setup(r => r.Create(It.IsAny<Review>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<CancellationToken>())).ReturnsAsync((Review review, string userEmail, string userDisplayName, bool fillUpdatedInfo, CancellationToken cancellationToken) =>
            {
                review.Id = 1;
                return review.Id;
            });
            _reviewRepository.Setup(r => r.GetById(1, It.IsAny<CancellationToken>())).ReturnsAsync(new Review
            {
                Id = 1
            });

            var service = new ReviewService(_reviewRepository.Object, _logger.Object, _eventStorage.Object);
            var result = await service.Save(new Review(), true);

            Assert.Equal(1, result.Item.Id);
        }

        [Fact]
        public async void SaveReview_UpdateExist_Success()
        {
            _reviewRepository.Setup(r => r.GetById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((int id, CancellationToken cancellationToken) => new Review { Id = id });
            _reviewRepository.Setup(r => r.Update(It.IsAny<Review>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var service = new ReviewService(_reviewRepository.Object, _logger.Object, _eventStorage.Object);
            var result = await service.Save(new Review { Id = 1 });

            _reviewRepository.Verify(r => r.Update(It.IsAny<Review>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()));
            Assert.Equal(1, result.Item.Id);
            _eventStorage.Verify(r => r.EmitEvent(It.IsAny<SaveEntityEvent<Review>>()), Times.Once);
        }

        [Fact]
        public async void SaveReview_UpdateNotExist_CreateNew()
        {
            _reviewRepository.Setup(r => r.GetById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Review)null);
            _reviewRepository.Setup(r => r.Create(It.IsAny<Review>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<CancellationToken>())).ReturnsAsync(
                (Review review, string userEmail, string userDisplayName, bool fillUpdatedInfo, CancellationToken cancellationToken) =>
            {
                review.Id = 1;
                _reviewRepository.Setup(r => r.GetById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync((int id, CancellationToken cancellationToken) => new Review { Id = id });
                return review.Id;
            });

            var service = new ReviewService(_reviewRepository.Object, _logger.Object, _eventStorage.Object);
            var result = await service.Save(new Review { Id = 1 }, true);

            _reviewRepository.Verify(r => r.Create(It.IsAny<Review>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()));
            Assert.Equal(1, result.Item.Id);
            _eventStorage.Verify(r => r.EmitEvent(It.IsAny<SaveEntityEvent<Review>>()), Times.Once);
        }

        [Fact]
        public async void SaveReview_UpdateNotExist_ThrowException()
        {
            _reviewRepository.Setup(r => r.GetById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Review)null);
            
            var service = new ReviewService(_reviewRepository.Object, _logger.Object, _eventStorage.Object);
            var result = await service.Save(new Review { Id = 1 }, false);

            Assert.Equal("Review (Id = 1) doesn't exist", result.Errors.First());
            _eventStorage.Verify(r => r.EmitEvent(It.IsAny<SaveEntityEvent<Review>>()), Times.Never);
        }

        [Fact]
        public async void GetReviews_ReturnsItems()
        {
            _reviewRepository.Setup(r => r.GetBySpec(It.IsAny<ISpecification<Review>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Review> {
                    new Review { Id = 1 },
                    new Review { Id = 2 }
                });
            _reviewRepository.Setup(r => r.CountBySpec(It.IsAny<ISpecification<Review>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(3);

            var service = new ReviewService(_reviewRepository.Object, _logger.Object, _eventStorage.Object);
            var result= await service.GetPageData(1, 2);

            Assert.NotEmpty(result.Items);
            Assert.Equal(2, result.Items.Count());
            Assert.Equal(3, result.TotalCount);
        }

        [Fact]
        public async void GetReviews_ReturnsEmpty()
        {
            _reviewRepository.Setup(r => r.GetBySpec(It.IsAny<ISpecification<Review>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Review>());
            _reviewRepository.Setup(r => r.CountBySpec(It.IsAny<ISpecification<Review>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(0);

            var service = new ReviewService(_reviewRepository.Object, _logger.Object, _eventStorage.Object);
            var result = await service.GetPageData(1, 20);

            Assert.Empty(result.Items);
            Assert.Equal(0, result.TotalCount);
        }

        [Fact]
        public async void GetReviews_OrderByName_ReturnsItems()
        {
            _reviewRepository.Setup(r => r.GetBySpec(It.IsAny<ISpecification<Review>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ISpecification<Review> spec, CancellationToken cancellationToken) =>
                {
                    var items = new List<Review> {
                        new Review { Id = 1, Rating = 2 },
                        new Review { Id = 2, Rating = 1 }
                    }.AsQueryable();
                    return items.OrderBy(spec.OrderBy).ToList();
                });

            var service = new ReviewService(_reviewRepository.Object, _logger.Object, _eventStorage.Object);
            var result = await service.GetPageData(orderBy: "rating");

            Assert.NotEmpty(result.Items);
            Assert.Equal(2, result.Items.Count());
            Assert.Equal(1, result.Items.First().Rating);
        }

        [Fact]
        public async void GetReviews_OrderByUpdated_ReturnsItems()
        {
            _reviewRepository.Setup(r => r.GetBySpec(It.IsAny<ISpecification<Review>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ISpecification<Review> spec, CancellationToken cancellationToken) =>
                {
                    var items = new List<Review> {
                        new Review { Id = 1, Rating = 2, Updated = new System.DateTime(2020, 4, 22) },
                        new Review { Id = 2, Rating = 1, Updated = new System.DateTime(2020, 4, 21) }
                    }.AsQueryable();
                    return items.OrderBy(spec.OrderBy).ToList();
                });

            var service = new ReviewService(_reviewRepository.Object, _logger.Object, _eventStorage.Object);
            var result = await service.GetPageData(orderBy: "updated");

            Assert.NotEmpty(result.Items);
            Assert.Equal(2, result.Items.Count());
            Assert.Equal(1, result.Items.First().Rating);
        }

        [Fact]
        public async void GetReviews_OrderById_ReturnsItems()
        {
            _reviewRepository.Setup(r => r.GetBySpec(It.IsAny<ISpecification<Review>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ISpecification<Review> spec, CancellationToken cancellationToken) =>
                {
                    var items = new List<Review> {
                        new Review { Id = 2, Rating = 2 },
                        new Review { Id = 1, Rating = 1 }
                    }.AsQueryable();
                    return items.OrderBy(spec.OrderBy).ToList();
                });

            var service = new ReviewService(_reviewRepository.Object, _logger.Object, _eventStorage.Object);
            var result = await service.GetPageData(orderBy: "id");

            Assert.NotEmpty(result.Items);
            Assert.Equal(2, result.Items.Count());
            Assert.Equal(1, result.Items.First().Rating);
        }

        [Fact]
        public async void GetReview_ReturnsItem()
        {
            _reviewRepository.Setup(r => r.GetById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((int id, CancellationToken cancellationToken) => new Review { Id = id });

            var service = new ReviewService(_reviewRepository.Object, _logger.Object, _eventStorage.Object);
            var item = await service.Get(1);

            Assert.NotNull(item);
            Assert.Equal(1, item.Id);
        }

        [Fact]
        public async void GetReview_ReturnsNull()
        {
            _reviewRepository.Setup(r => r.GetById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Review)null);

            var service = new ReviewService(_reviewRepository.Object, _logger.Object, _eventStorage.Object);
            var item = await service.Get(1);

            Assert.Null(item);
        }

        [Fact]
        public async void DeleteReview_Success()
        {
            _reviewRepository.Setup(r => r.Delete(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var service = new ReviewService(_reviewRepository.Object, _logger.Object, _eventStorage.Object);
            await service.Delete(1);

            _reviewRepository.Verify(r => r.Delete(It.IsAny<int>(), It.IsAny<CancellationToken>()));
            _eventStorage.Verify(r => r.EmitEvent(It.IsAny<DeleteEntityEvent<Review>>()), Times.Once);
        }
    }
}