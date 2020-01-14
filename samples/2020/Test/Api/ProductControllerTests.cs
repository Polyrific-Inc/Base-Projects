using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Api;
using Api.Controllers;
using Api.Dto;
using Core.Entities;
using Core.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace Test.Api
{
    public class ProductControllerTests : IClassFixture<AppTestFixture>, IDisposable
    {
        private readonly Mock<IProductService> _productService;
        private readonly AppTestFixture _fixture;
        private readonly HttpClient _client;

        public ProductControllerTests(AppTestFixture fixture, ITestOutputHelper output)
        {
            _productService = new Mock<IProductService>();

            _fixture = fixture;
            _fixture.Output = output;

            _client = _fixture.WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                    {
                        services.AddTransient<IProductService>(_ => _productService.Object);
                    });
                })
                .CreateClient();
            
        }

        [Fact]
        public async void GetAll_WithPagingValues_ReturnsObjects()
        {
            var dummyItems = new List<Product>
            {
                new Product{ Id = 1 },
                new Product{ Id = 2 },
                new Product{ Id = 3 }
            };
            _productService.Setup(s => s.GetProducts(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync((int page, int size) => (
                    dummyItems.OrderBy(i => i.Id)
                        .Skip((page - 1) * size)
                        .Take(size), 
                    dummyItems.Count
                ));

            var response = await _client.GetAsync("/product?page=1&size=2");
            
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var pageResult = JsonConvert.DeserializeObject<PageResult<ProductDto>>(responseString);

            Assert.NotEmpty(pageResult.Items);
            Assert.Equal(2, pageResult.Items.Count());
            Assert.Equal(3, pageResult.TotalCount);
            Assert.Equal(1, pageResult.Page);
            Assert.Equal(2, pageResult.PageSize);
        }

        [Fact]
        public async void GetAll_WithoutPagingValues_ReturnsObjects()
        {
            var dummyItems = new List<Product>();
            for (int i = 0; i < 21; i++)
            {
                dummyItems.Add(new Product { Id = i + 1 });
            }

            _productService.Setup(s => s.GetProducts(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync((int page, int size) => (
                    dummyItems.OrderBy(i => i.Id)
                        .Skip((page - 1) * size)
                        .Take(size),
                    dummyItems.Count
                ));

            var response = await _client.GetAsync("/product");

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var pageResult = JsonConvert.DeserializeObject<PageResult<ProductDto>>(responseString);

            Assert.NotEmpty(pageResult.Items);
            Assert.Equal(20, pageResult.Items.Count());
            Assert.Equal(21, pageResult.TotalCount);
            Assert.Equal(1, pageResult.Page);
            Assert.Equal(20, pageResult.PageSize);
        }

        [Fact]
        public async void GetByName_ReturnsObject()
        {
            _productService.Setup(s => s.GetProducts(It.IsAny<string>()))
                .ReturnsAsync((string name) => new List<Product> { new Product { Name = name } });

            var response = await _client.GetAsync("/product/product1");

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var items = JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(responseString);
            
            Assert.NotEmpty(items);
            Assert.All(items, item => Assert.Contains("product1", item.Name, StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public async void GetByName_ReturnsEmpty()
        {
            _productService.Setup(s => s.GetProducts(It.IsAny<string>()))
                .ReturnsAsync(new List<Product>());

            var response = await _client.GetAsync("/product/product1");

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var items = JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(responseString);
            
            Assert.Empty(items);
        }

        [Fact]
        public async void Post_Success()
        {
            _productService.Setup(s => s.AddProduct(It.IsAny<Product>())).ReturnsAsync(1);

            var content = new StringContent(JsonConvert.SerializeObject(new NewProductDto()), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/product", content);

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            int.TryParse(responseString, out int newId);

            Assert.Equal(1, newId);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                _fixture.Output = null;
        }
    }
}