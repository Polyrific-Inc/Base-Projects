using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Entities;
using Core.Services;
using Moq;
using Polyrific.Project.Core;
using Polyrific.Project.Core.Exceptions;
using Xunit;

namespace Test.Core
{
    public class ProductServiceTests
    {
        private readonly Mock<IRepository<Product>> _productRepository;

        public ProductServiceTests()
        {
            _productRepository = new Mock<IRepository<Product>>();
        }

        [Fact]
        public async void SaveProduct_New_Success()
        {
            _productRepository.Setup(r => r.Create(It.IsAny<Product>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var service = new ProductService(_productRepository.Object);
            var newId = await service.SaveProduct(new Product());

            Assert.Equal(1, newId);
        }

        [Fact]
        public async void SaveProduct_UpdateExist_Success()
        {
            _productRepository.Setup(r => r.GetById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((int id, CancellationToken cancellationToken) => new Product { Id = id });
            _productRepository.Setup(r => r.Update(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var service = new ProductService(_productRepository.Object);
            var id = await service.SaveProduct(new Product { Id = 1 });

            _productRepository.Verify(r => r.Update(It.IsAny<Product>(), It.IsAny<CancellationToken>()));
            Assert.Equal(1, id);
        }

        [Fact]
        public async void SaveProduct_UpdateNotExist_CreateNew()
        {
            _productRepository.Setup(r => r.GetById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Product)null);
            _productRepository.Setup(r => r.Create(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var service = new ProductService(_productRepository.Object);
            var newId = await service.SaveProduct(new Product { Id = 1 });

            _productRepository.Verify(r => r.Create(It.IsAny<Product>(), It.IsAny<CancellationToken>()));
            Assert.Equal(1, newId);
        }

        [Fact]
        public async void SaveProduct_UpdateNotExist_ThrowException()
        {
            _productRepository.Setup(r => r.GetById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Product)null);
            
            var service = new ProductService(_productRepository.Object);
            
            await Assert.ThrowsAsync<NotExistEntityException>(async () => await service.SaveProduct(new Product { Id = 1 }, false));
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

            var service = new ProductService(_productRepository.Object);
            var (entities, total) = await service.GetProducts(1, 2);

            Assert.NotEmpty(entities);
            Assert.Equal(2, entities.Count());
            Assert.Equal(3, total);
        }

        [Fact]
        public async void GetProducts_ReturnsEmpty()
        {
            _productRepository.Setup(r => r.GetBySpec(It.IsAny<ISpecification<Product>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Product>());
            _productRepository.Setup(r => r.CountBySpec(It.IsAny<ISpecification<Product>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(0);

            var service = new ProductService(_productRepository.Object);
            var (entities, total) = await service.GetProducts(1, 20);

            Assert.Empty(entities);
            Assert.Equal(0, total);
        }

        [Fact]
        public async void GetProducts_ByName_ReturnsItems()
        {
            _productRepository.Setup(r => r.GetBySpec(It.IsAny<ISpecification<Product>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Product>{
                    new Product { Name = "Product1" }
                });

            var service = new ProductService(_productRepository.Object);
            var items = await service.GetProducts("product");

            Assert.NotEmpty(items);
        }

        [Fact]
        public async void GetProducts_ByName_ReturnsEmpty()
        {
            _productRepository.Setup(r => r.GetBySpec(It.IsAny<ISpecification<Product>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Product>());

            var service = new ProductService(_productRepository.Object);
            var items = await service.GetProducts("product");

            Assert.Empty(items);
        }

        [Fact]
        public async void GetProduct_ReturnsItem()
        {
            _productRepository.Setup(r => r.GetById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((int id, CancellationToken cancellationToken) => new Product { Id = id });

            var service = new ProductService(_productRepository.Object);
            var item = await service.GetProduct(1);

            Assert.NotNull(item);
            Assert.Equal(1, item.Id);
        }

        [Fact]
        public async void GetProduct_ReturnsNull()
        {
            _productRepository.Setup(r => r.GetById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Product)null);

            var service = new ProductService(_productRepository.Object);
            var item = await service.GetProduct(1);

            Assert.Null(item);
        }

        [Fact]
        public async void DeleteProduct_Success()
        {
            _productRepository.Setup(r => r.Delete(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var service = new ProductService(_productRepository.Object);
            await service.DeleteProduct(1);

            _productRepository.Verify(r => r.Delete(It.IsAny<int>(), It.IsAny<CancellationToken>()));
        }
    }
}