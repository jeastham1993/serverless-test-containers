using System.Collections.Generic;
using System.Globalization;
using Amazon.DynamoDBv2.Model;
using ProductsAPI.Models;

namespace ProductsAPI.DataAccess;

public class ProductMapper
{
    public static string PK = "PK";
    public static string NAME = "name";
    public static string PRICE = "price";

    public static Product ProductFromDynamoDB(Dictionary<string, AttributeValue> items)
    {
        var product = new Product(items[PK].S, items[NAME].S, decimal.Parse(items[PRICE].N));

        return product;
    }

    public static Dictionary<string, AttributeValue> ProductToDynamoDb(Product product)
    {
        var item = new Dictionary<string, AttributeValue>(3);
        item.Add(PK, new AttributeValue(product.Id));
        item.Add(NAME, new AttributeValue(product.Name));
        item.Add(PRICE, new AttributeValue
        {
            N = product.Price.ToString(CultureInfo.InvariantCulture)
        });

        return item;
    }
}