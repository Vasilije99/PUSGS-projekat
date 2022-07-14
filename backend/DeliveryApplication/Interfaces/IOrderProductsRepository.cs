using DeliveryApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryApplication.Interfaces
{
    public interface IOrderProductsRepository
    {
        void AddNewOrderProduct(string productName, int count, int orderId);

        Task<List<OrderProduct>> OrderProductsByOrderId(int orderId);
    }
}
