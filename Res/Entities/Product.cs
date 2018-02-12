using System;
using System.Linq;

namespace ResApp.Entities
{
    public enum ProductCategory
    {
        Food,
        Grocery,
        Household,
        Sport
    }
    
    public class Product : EntityBase
    {
        public ProductCategory Category { get; set; }
        public double Price { get; set; }
    }
}
