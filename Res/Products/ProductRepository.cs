using System.Collections.Generic;
using ResApp.Entities;

namespace ResApp.Products
{
    public class ProductRepository
    {
        private List<Product> productCache;

        public IEnumerable<Product> GetProducts(int count = 16)
        {
            if (productCache == null)
            {
                productCache = new List<Product>();
                for (int i = 0; i < count; i++)
                {
                    var product = new Product { Id = i, Name = "Product " + i, Category = (ProductCategory)(i % 4), Price = i * 4 + (i + 1) / 4 };
                    productCache.Add(product);
                }
            }
            return productCache;
        }
    }
}