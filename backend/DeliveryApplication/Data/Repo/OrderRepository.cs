using DeliveryApplication.Interfaces;
using DeliveryApplication.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryApplication.Data.Repo
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DataContext dc;

        public OrderRepository(DataContext dc)
        {
            this.dc = dc;
        }

        public Order CreateNewOrder(Order newOrder)
        {
            newOrder.OrderState = OrderStateEnum.PENDING;

            dc.Orders.Add(newOrder);

            return newOrder;
        }

        public async Task<Order> DoesOrderPending(int orderId)
        {
            return await dc.Orders.Where(x => (x.OrderState == OrderStateEnum.PENDING || x.OrderState == OrderStateEnum.ACTIVE) && x.OrderState != OrderStateEnum.FINISHED && x.Id == orderId).FirstAsync();
        }

        public async Task<List<Order>> GetAllOrders()
        {
            return await dc.Orders.ToListAsync();
        }

        public async Task<List<Order>> GetFinishedOrders()
        {
            return await dc.Orders.Where(x => x.OrderState == OrderStateEnum.FINISHED).ToListAsync();
        }

        public async Task<Order> GetLastAddedOrder()
        {
            return await dc.Orders.LastAsync();
        }

        public async Task<Order> GetMyActiveOrder(int delivererId)
        {
            return await dc.Orders.FirstOrDefaultAsync(x => x.Deliverer.Id == delivererId && x.OrderState == OrderStateEnum.ACTIVE);
        }

        public async Task<List<Order>> GetMyFinishedOrders(User deliverer)
        {
            return await dc.Orders.Where(x => x.OrderState == OrderStateEnum.FINISHED && x.Deliverer == deliverer).ToListAsync();
        }

        public async Task<Order> GetOrderById(int id)
        {
            return await dc.Orders.FindAsync(id);
        }

        public async Task<List<Order>> GetPendingOrders()
        {
            return await dc.Orders.Where(x => x.OrderState == OrderStateEnum.PENDING).ToListAsync();
        }

        public async Task<List<Order>> GetStartedOrders()
        {
            return await dc.Orders.Where(x => x.OrderState == OrderStateEnum.FINISHED || x.OrderState == OrderStateEnum.ACTIVE).ToListAsync();
        }

        public async Task<Order> GetUserFinishedOrder(int orderId)
        {
            return await dc.Orders.FirstOrDefaultAsync(x => x.OrderState == OrderStateEnum.FINISHED && x.Id == orderId);
        }
    }
}
