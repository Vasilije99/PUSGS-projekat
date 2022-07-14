using DeliveryApplication.Data.Repo;
using DeliveryApplication.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryApplication.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext dc;
        public UnitOfWork(DataContext dc)
        {
            this.dc = dc;
        }

        public IUserRepository UserRepository =>
            new UserRepository(dc);

        public IProductRepository ProductRepository =>
            new ProductRepository(dc);

        public IOrderRepository OrderRepository =>
            new OrderRepository(dc);

        public IOrderProductsRepository OrderProductsRepository =>
            new OrderProductRepository(dc);

        public IUserOrderRepository UserOrderRepository =>
            new UserOrderRepository(dc);
        public async Task<bool> SaveAsync()
        {
            return await dc.SaveChangesAsync() > 0;
        }
    }
}
