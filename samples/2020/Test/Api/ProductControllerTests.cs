using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
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
using Newtonsoft.Json.Linq;
using Polyrific.Project.Core;
using Polyrific.Project.Core.Exceptions;
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
            _productService.Setup(s => s.GetPageData(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync((int page, int size) => (
                    new Paging<Product>
                    {
                        Items = dummyItems.OrderBy(i => i.Id)
                        .Skip((page - 1) * size)
                        .Take(size),
                        Page = page,
                        PageSize = size,
                        TotalCount = dummyItems.Count
                    }                    
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

            _productService.Setup(s => s.GetPageData(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync((int page, int size) => (
                    new Paging<Product>
                    {
                        Items = dummyItems.OrderBy(i => i.Id)
                        .Skip((page - 1) * size)
                        .Take(size),
                        Page = page,
                        PageSize = size,
                        TotalCount = dummyItems.Count
                    }
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
        public async void GetAll_ReturnsEmpty()
        {
            _productService.Setup(s => s.GetPageData(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new Paging<Product>
                {
                    Items = new List<Product>(),
                    TotalCount = 0
                });

            var response = await _client.GetAsync("/product");

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var pageResult = JsonConvert.DeserializeObject<PageResult<ProductDto>>(responseString);

            Assert.Empty(pageResult.Items);
        }

        [Fact]
        public async void GetById_ReturnsObject()
        {
            _productService.Setup(s => s.Get(It.IsAny<int>()))
                .ReturnsAsync((int id) => new Product { Id = id });

            var response = await _client.GetAsync("/product/1");

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var item = JsonConvert.DeserializeObject<ProductDto>(responseString);

            Assert.NotNull(item);
            Assert.Equal(1, item.Id);
        }

        [Fact]
        public async void GetById_ReturnsNull()
        {
            _productService.Setup(s => s.Get(It.IsAny<int>()))
                .ReturnsAsync((int id) => null);

            var response = await _client.GetAsync("/product/1");

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var item = JsonConvert.DeserializeObject<ProductDto>(responseString);

            Assert.Null(item);
        }

        [Fact]
        public async void GetByName_ReturnsObject()
        {
            _productService.Setup(s => s.GetProducts(It.IsAny<string>()))
                .ReturnsAsync((string name) => new List<Product> { new Product { Name = name } });

            var response = await _client.GetAsync("/product/name/product1");

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

            var response = await _client.GetAsync("/product/name/product1");

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var items = JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(responseString);
            
            Assert.Empty(items);
        }

        [Fact]
        public async void Post_Success()
        {
            _productService.Setup(s => s.Save(It.IsAny<Product>(), It.IsAny<bool>())).ReturnsAsync(new Result<Product>
            {
                Success = true,
                Item = new Product
                {
                    Id = 1
                }
            });


            var content = new StringContent(JsonConvert.SerializeObject(new NewProductDto()), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/product", content);

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            int.TryParse(responseString, out int newId);

            Assert.Equal(1, newId);
        }

        [Fact]
        public async void Put_Success()
        {
            _productService.Setup(s => s.Save(It.IsAny<Product>(), It.IsAny<bool>())).ReturnsAsync(new Result<Product>
            {
                Success = true,
                Item = new Product
                {
                    Id = 1
                }
            });

            var content = new StringContent(JsonConvert.SerializeObject(new UpdatedProductDto { Id = 1 }), Encoding.UTF8, "application/json");
            var response = await _client.PutAsync("/product/1", content);

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            int.TryParse(responseString, out int newId);

            _productService.Verify(s => s.Save(It.IsAny<Product>(), It.IsAny<bool>()));
        }

        [Fact]
        public async void Put_IdsNotMatch_Failed()
        {
            var content = new StringContent(JsonConvert.SerializeObject(new UpdatedProductDto { Id = 2 }), Encoding.UTF8, "application/json");
            var response = await _client.PutAsync("/product/1", content);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async void Put_NotExists_Failed()
        {
            _productService.Setup(s => s.Save(It.IsAny<Product>(), It.IsAny<bool>())).Throws(new NotExistEntityException(1));

            var content = new StringContent(JsonConvert.SerializeObject(new UpdatedProductDto { Id = 1 }), Encoding.UTF8, "application/json");
            var response = await _client.PutAsync("/product/1", content);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async void Delete_Success()
        {
            _productService.Setup(s => s.Delete(It.IsAny<int>())).ReturnsAsync(new Result
            {
                Success = true
            });

            var response = await _client.DeleteAsync("/product/1");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
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