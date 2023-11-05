using System.Collections.Generic;

namespace ProductsAPI.Models;

public class ProductWrapper
{
    public ProductWrapper()
    {
    }

    public ProductWrapper(List<Product> products)
    {
        Products = products;
    }

    public List<Product> Products { get; set; }
}