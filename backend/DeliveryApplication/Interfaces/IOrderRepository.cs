using DeliveryApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryApplication.Interfaces
{
    public interface IOrderRepository
    {
        Order CreateNewOrder(Order newOrder);
        Task<List<Order>> GetPendingOrders();
        Task<List<Order>> GetFinishedOrders();
        Task<List<Order>> GetAllOrders();
        Task<Order> GetOrderById(int id);
        Task<List<Order>> GetMyFinishedOrders(User deliverer);
        Task<List<Order>> GetStartedOrders();
        Task<Order> GetLastAddedOrder();
        Task<Order> DoesOrderPending(int orderId);
        Task<Order> GetUserFinishedOrder(int orderId);
        Task<Order> GetMyActiveOrder(int delivererId);
    }
}
