using System.Collections.Generic;
using System.Threading;
using Core.Entities;
using Core.Services;
using Moq;
using Polyrific.Project.Core;
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
        public async void GetProducts_ReturnsItems()
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
        public async void GetProducts_ReturnsEmpty()
        {
            _productRepository.Setup(r => r.GetBySpec(It.IsAny<ISpecification<Product>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Product>());

            var service = new ProductService(_productRepository.Object);
            var items = await service.GetProducts("product");

            Assert.Empty(items);
        }
    }
}