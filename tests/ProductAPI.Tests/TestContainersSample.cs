using FluentAssertions;
using ProductsAPI.DataAccess;
using ProductsAPI.Models;

namespace ProductAPI.Tests;

public class TestContainersSample : IClassFixture<StartupFixture>
{
    private readonly StartupFixture _fixture;
    private readonly ProductRepository _productRepository;
    
    public TestContainersSample(StartupFixture fixture)
    {
        this._fixture = fixture;
        _productRepository = new ProductRepository(fixture.DynamoDbClient);
    }
    [Fact]
    public async Task CanCreateAndRetrieveProducts()
    {
        await _productRepository.PutProduct(new Product("1234", "James", 10));

        var product = await _productRepository.GetProduct("1234");

        product.Name.Should().Be("James");
    }
}