using System;
using System.Collections.Generic;
using System.Linq;
using ResApp.Entities;
using ResApp.Products;

namespace ResApp.Orders
{
    public class OrderRepository
    {
        private static List<Order> orderCache;

        private static void CheckCache(int count)
        {
            if (orderCache != null) return;
            orderCache = new List<Order>();
            for (int i = 0; i < count; i++)
            {
                var order = new Order
                {
                    Id = i,
                    Name = "Order " + i,
                    Status = (OrderStatus)(i % 3),
                    Products = GetProductsByOrderId(i)
                };
                orderCache.Add(order);
            }
        }

        private static IEnumerable<Product> GetProductsByOrderId(int orderId)
        {
            return new ProductRepository()
                .GetProducts()
                .Where(p => p.Id % (orderId + 1) == 0);
        }

        public IEnumerable<Order> GetOrders(int count = 4)
        {
            CheckCache(count);
            return orderCache;
        }

        public void UpdateOrder(Order order)
        {
            if (orderCache == null) return;
            for (int i = 0; i < orderCache.Count; i++)
            {
                if (orderCache[i].Id == order.Id)
                {
                    orderCache[i] = order;
                    return;
                }
            }
        }
    }
}
