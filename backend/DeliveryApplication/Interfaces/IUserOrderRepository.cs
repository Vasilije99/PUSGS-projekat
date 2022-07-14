using DeliveryApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryApplication.Interfaces
{
    public interface IUserOrderRepository
    {
        void AddNewUserOrder(int userId, int orderId);
        Task<List<UserOrder>> GetMyOrders(int userId);
    }
}
