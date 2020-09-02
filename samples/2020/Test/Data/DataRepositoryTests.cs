using Core.Entities;
using Data;
using Microsoft.EntityFrameworkCore;
using Polyrific.Project.Core;
using Polyrific.Project.Data;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Test.Data
{
    public class DataRepositoryTests
    {
        public DataRepositoryTests()
        {
            ContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase("sample-db")
                    .Options;

            Seed();
        }

        private DbContextOptions<ApplicationDbContext> ContextOptions { get; }

        private void Seed()
        {
            using var context = new ApplicationDbContext(ContextOptions);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var one = new Product()
            {
                Id = 1,
                Name = "Product 1"
            };

            var two = new Product()
            {
                Id = 2,
                Name = "Product 2"
            };

            context.AddRange(one, two);

            context.SaveChanges();
        }

        [Fact]
        public async Task GetBySpec_ReturnItemsWithoutName()
        {
            using var dbContext = new ApplicationDbContext(ContextOptions);
            var productRepository = new DataRepository<Product>(dbContext);
            var products = await productRepository.GetBySpec(new Specification<Product>(e => true, null, selector: e => new Product
            {
                Id = e.Id
            }));

            Assert.Equal(2, products.Count());
            Assert.True(!products.Any(e => e.Id == 0));
            Assert.True(!products.Any(e => e.Name != null));
        }

        [Fact]
        public async Task GetBySpecWithOrder_ReturnItemsWithoutName()
        {
            using var dbContext = new ApplicationDbContext(ContextOptions);
            var productRepository = new DataRepository<Product>(dbContext);
            var products = await productRepository.GetBySpec(new Specification<Product>(e => true, e => e.Name, true, e => new Product
            {
                Id = e.Id
            }));

            Assert.True(products.First().Id == 2);
            Assert.Equal(2, products.Count());
            Assert.True(!products.Any(e => e.Id == 0));
            Assert.True(!products.Any(e => e.Name != null));
        }

        [Fact]
        public async Task GetSingleBySpec_ReturnItemWithoutName()
        {
            using var dbContext = new ApplicationDbContext(ContextOptions);
            var productRepository = new DataRepository<Product>(dbContext);
            var product = await productRepository.GetSingleBySpec(new Specification<Product>(e => true, null, selector: e => new Product
            {
                Id = e.Id
            }));

            Assert.NotNull(product);
            Assert.Equal(1, product.Id);
            Assert.Null(product.Name);
        }
    }
}