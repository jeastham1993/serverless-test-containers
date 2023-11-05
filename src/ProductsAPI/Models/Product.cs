using System;

namespace ProductsAPI.Models;

public class Product
{
    public Product()
    {
    }

    public Product(string id, string name, decimal price)
    {
        Id = id;
        Name = name;
        Price = price;
    }

    public string Id { get; set; }

    public string Name { get; set; }

    public decimal Price { get; private set; }

    public void SetPrice(decimal newPrice)
    {
        Price = Math.Round(newPrice, 2);
    }

    public override string ToString()
    {
        return "Product{" +
               "id='" + Id + '\'' +
               ", name='" + Name + '\'' +
               ", price=" + Price +
               '}';
    }
}