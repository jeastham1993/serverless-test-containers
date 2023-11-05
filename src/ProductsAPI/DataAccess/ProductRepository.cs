using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using ProductsAPI.Models;

namespace ProductsAPI.DataAccess;

public class ProductRepository
{
    private static readonly string PRODUCT_TABLE_NAME =
        Environment.GetEnvironmentVariable("TABLE_NAME") ?? "Products";

    private readonly AmazonDynamoDBClient _dynamoDbClient;

    public ProductRepository(AmazonDynamoDBClient client)
    {
        _dynamoDbClient = client;
    }

    public async Task<Product> GetProduct(string id)
    {
        var getItemResponse = await _dynamoDbClient.GetItemAsync(new GetItemRequest(PRODUCT_TABLE_NAME,
            new Dictionary<string, AttributeValue>(1)
            {
                { ProductMapper.PK, new AttributeValue(id) }
            }));

        return getItemResponse.IsItemSet ? ProductMapper.ProductFromDynamoDB(getItemResponse.Item) : null;
    }

    public async Task PutProduct(Product product)
    {
        await _dynamoDbClient.PutItemAsync(PRODUCT_TABLE_NAME, ProductMapper.ProductToDynamoDb(product));
    }

    public async Task DeleteProduct(string id)
    {
        await _dynamoDbClient.DeleteItemAsync(PRODUCT_TABLE_NAME, new Dictionary<string, AttributeValue>(1)
        {
            { ProductMapper.PK, new AttributeValue(id) }
        });
    }

    public async Task<ProductWrapper> GetAllProducts()
    {
        var data = await _dynamoDbClient.ScanAsync(new ScanRequest
        {
            TableName = PRODUCT_TABLE_NAME,
            Limit = 20
        });

        var products = new List<Product>();

        foreach (var item in data.Items) products.Add(ProductMapper.ProductFromDynamoDB(item));

        return new ProductWrapper(products);
    }
}