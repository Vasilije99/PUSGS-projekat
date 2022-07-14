using DeliveryApplication.Interfaces;
using DeliveryApplication.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryApplication.Data.Repo
{
    public class UserOrderRepository : IUserOrderRepository
    {
        private readonly DataContext dc;

        public UserOrderRepository(DataContext dc)
        {
            this.dc = dc;
        }

        public void AddNewUserOrder(int userId, int orderId)
        {
            UserOrder uo = new UserOrder();
            uo.UserId = userId;
            uo.OrderId = orderId;

            dc.UserOrders.Add(uo);
        }

        public async Task<List<UserOrder>> GetMyOrders(int userId)
        {
            return await dc.UserOrders.Where(x => x.UserId == userId).ToListAsync();
        }
    }
}
