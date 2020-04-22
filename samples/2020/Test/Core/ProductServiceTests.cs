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
    public class ProductServiceTests
    {
        private readonly Mock<IRepository<Product>> _productRepository;
        private readonly Mock<ILogger<ProductService>> _logger;

        public ProductServiceTests()
        {
            _productRepository = new Mock<IRepository<Product>>();
            _logger = new Mock<ILogger<ProductService>>();
        }

        [Fact]
        public async void SaveProduct_New_Success()
        {
            _productRepository.Setup(r => r.Create(It.IsAny<Product>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<CancellationToken>())).ReturnsAsync((Product product, string userEmail, string userDisplayName, bool fillUpdatedInfo, CancellationToken cancellationToken) =>
            {
                product.Id = 1;
                return product.Id;
            });
            _productRepository.Setup(r => r.GetById(1, It.IsAny<CancellationToken>())).ReturnsAsync(new Product
            {
                Id = 1
            });

            var service = new ProductService(_productRepository.Object, _logger.Object);
            var result = await service.Save(new Product(), true);

            Assert.Equal(1, result.Item.Id);
        }

        [Fact]
        public async void SaveProduct_UpdateExist_Success()
        {
            _productRepository.Setup(r => r.GetById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((int id, CancellationToken cancellationToken) => new Product { Id = id });
            _productRepository.Setup(r => r.Update(It.IsAny<Product>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var service = new ProductService(_productRepository.Object, _logger.Object);
            var result = await service.Save(new Product { Id = 1 });

            _productRepository.Verify(r => r.Update(It.IsAny<Product>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()));
            Assert.Equal(1, result.Item.Id);
        }

        [Fact]
        public async void SaveProduct_UpdateNotExist_CreateNew()
        {
            _productRepository.Setup(r => r.GetById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Product)null);
            _productRepository.Setup(r => r.Create(It.IsAny<Product>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<CancellationToken>())).ReturnsAsync(
                (Product product, string userEmail, string userDisplayName, bool fillUpdatedInfo, CancellationToken cancellationToken) =>
            {
                product.Id = 1;
                _productRepository.Setup(r => r.GetById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync((int id, CancellationToken cancellationToken) => new Product { Id = id });
                return product.Id;
            });

            var service = new ProductService(_productRepository.Object, _logger.Object);
            var result = await service.Save(new Product { Id = 1 }, true);

            _productRepository.Verify(r => r.Create(It.IsAny<Product>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()));
            Assert.Equal(1, result.Item.Id);
        }

        [Fact]
        public async void SaveProduct_UpdateNotExist_ThrowException()
        {
            _productRepository.Setup(r => r.GetById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Product)null);
            
            var service = new ProductService(_productRepository.Object, _logger.Object);
            var result = await service.Save(new Product { Id = 1 }, false);

            Assert.Equal("Product (Id = 1) doesn't exist", result.Errors.First());
        }

        [Fact]
        public async void GetProducts_ReturnsItems()
        {
            _productRepository.Setup(r => r.GetBySpec(It.IsAny<ISpecification<Product>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Product> {
                    new Product { Id = 1 },
                    new Product { Id = 2 }
                });
            _productRepository.Setup(r => r.CountBySpec(It.IsAny<ISpecification<Product>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(3);

            var service = new ProductService(_productRepository.Object, _logger.Object);
            var result= await service.GetPageData(1, 2);

            Assert.NotEmpty(result.Items);
            Assert.Equal(2, result.Items.Count());
            Assert.Equal(3, result.TotalCount);
        }

        [Fact]
        public async void GetProducts_ReturnsEmpty()
        {
            _productRepository.Setup(r => r.GetBySpec(It.IsAny<ISpecification<Product>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Product>());
            _productRepository.Setup(r => r.CountBySpec(It.IsAny<ISpecification<Product>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(0);

            var service = new ProductService(_productRepository.Object, _logger.Object);
            var result = await service.GetPageData(1, 20);

            Assert.Empty(result.Items);
            Assert.Equal(0, result.TotalCount);
        }

        [Fact]
        public async void GetProducts_FilterByName_ReturnsItems()
        {
            _productRepository.Setup(r => r.GetBySpec(It.IsAny<ISpecification<Product>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ISpecification<Product> spec, CancellationToken cancellationToken) =>
                {
                    var items = new List<Product> {
                        new Product { Id = 1, Name = "Test1" },
                        new Product { Id = 2, Name = "Test2" }
                    }.AsQueryable();
                    return items.Where(spec.Criteria).ToList();
                });

            var service = new ProductService(_productRepository.Object, _logger.Object);
            var result = await service.GetPageData(filter: "Name_Test1");

            Assert.NotEmpty(result.Items);
            Assert.Single(result.Items);
            Assert.Equal("Test1", result.Items.First().Name);
        }

        [Fact]
        public async void GetProducts_OrderByName_ReturnsItems()
        {
            _productRepository.Setup(r => r.GetBySpec(It.IsAny<ISpecification<Product>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ISpecification<Product> spec, CancellationToken cancellationToken) =>
                {
                    var items = new List<Product> {
                        new Product { Id = 1, Name = "zz" },
                        new Product { Id = 2, Name = "aa" }
                    }.AsQueryable();
                    return items.OrderBy(spec.OrderBy).ToList();
                });

            var service = new ProductService(_productRepository.Object, _logger.Object);
            var result = await service.GetPageData(orderBy: "name");

            Assert.NotEmpty(result.Items);
            Assert.Equal(2, result.Items.Count());
            Assert.Equal("aa", result.Items.First().Name);
        }

        [Fact]
        public async void GetProducts_OrderByUpdated_ReturnsItems()
        {
            _productRepository.Setup(r => r.GetBySpec(It.IsAny<ISpecification<Product>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ISpecification<Product> spec, CancellationToken cancellationToken) =>
                {
                    var items = new List<Product> {
                        new Product { Id = 1, Name = "zz", Updated = new System.DateTime(2020, 4, 22) },
                        new Product { Id = 2, Name = "aa", Updated = new System.DateTime(2020, 4, 21) }
                    }.AsQueryable();
                    return items.OrderBy(spec.OrderBy).ToList();
                });

            var service = new ProductService(_productRepository.Object, _logger.Object);
            var result = await service.GetPageData(orderBy: "updated");

            Assert.NotEmpty(result.Items);
            Assert.Equal(2, result.Items.Count());
            Assert.Equal("aa", result.Items.First().Name);
        }

        [Fact]
        public async void GetProducts_OrderById_ReturnsItems()
        {
            _productRepository.Setup(r => r.GetBySpec(It.IsAny<ISpecification<Product>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ISpecification<Product> spec, CancellationToken cancellationToken) =>
                {
                    var items = new List<Product> {
                        new Product { Id = 2, Name = "zz" },
                        new Product { Id = 1, Name = "aa" }
                    }.AsQueryable();
                    return items.OrderBy(spec.OrderBy).ToList();
                });

            var service = new ProductService(_productRepository.Object, _logger.Object);
            var result = await service.GetPageData(orderBy: "id");

            Assert.NotEmpty(result.Items);
            Assert.Equal(2, result.Items.Count());
            Assert.Equal("aa", result.Items.First().Name);
        }

        [Fact]
        public async void GetProducts_ByName_ReturnsItems()
        {
            _productRepository.Setup(r => r.GetBySpec(It.IsAny<ISpecification<Product>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Product>{
                    new Product { Name = "Product1" }
                });

            var service = new ProductService(_productRepository.Object, _logger.Object);
            var items = await service.GetProducts("product");

            Assert.NotEmpty(items);
        }

        [Fact]
        public async void GetProducts_ByName_ReturnsEmpty()
        {
            _productRepository.Setup(r => r.GetBySpec(It.IsAny<ISpecification<Product>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Product>());

            var service = new ProductService(_productRepository.Object, _logger.Object);
            var items = await service.GetProducts("product");

            Assert.Empty(items);
        }

        [Fact]
        public async void GetProduct_ReturnsItem()
        {
            _productRepository.Setup(r => r.GetById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((int id, CancellationToken cancellationToken) => new Product { Id = id });

            var service = new ProductService(_productRepository.Object, _logger.Object);
            var item = await service.Get(1);

            Assert.NotNull(item);
            Assert.Equal(1, item.Id);
        }

        [Fact]
        public async void GetProduct_ReturnsNull()
        {
            _productRepository.Setup(r => r.GetById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Product)null);

            var service = new ProductService(_productRepository.Object, _logger.Object);
            var item = await service.Get(1);

            Assert.Null(item);
        }

        [Fact]
        public async void DeleteProduct_Success()
        {
            _productRepository.Setup(r => r.Delete(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var service = new ProductService(_productRepository.Object, _logger.Object);
            await service.Delete(1);

            _productRepository.Verify(r => r.Delete(It.IsAny<int>(), It.IsAny<CancellationToken>()));
        }
    }
}