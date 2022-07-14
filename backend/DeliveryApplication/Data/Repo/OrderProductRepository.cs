using DeliveryApplication.Interfaces;
using DeliveryApplication.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryApplication.Data.Repo
{
    public class OrderProductRepository : IOrderProductsRepository
    {
        private readonly DataContext dc;
        public OrderProductRepository(DataContext dc)
        {
            this.dc = dc;
        }

        public void AddNewOrderProduct(string productName, int count, int orderId)
        {
            int productId = dc.Products.FirstOrDefault(x => x.Name == productName).Id;

            OrderProduct op = new OrderProduct();
            op.OrderId = orderId;
            op.Count = count;
            op.ProductId = productId;

            dc.OrderProducts.Add(op);
        }

        public async Task<List<OrderProduct>> OrderProductsByOrderId(int orderId)
        {
            return await dc.OrderProducts.Where(x => x.OrderId == orderId).ToListAsync();
        }
    }
}
