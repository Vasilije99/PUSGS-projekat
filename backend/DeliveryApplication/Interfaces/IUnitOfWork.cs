using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryApplication.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        IProductRepository ProductRepository { get; }
        IOrderRepository OrderRepository { get; }
        IOrderProductsRepository OrderProductsRepository { get; }
        IUserOrderRepository UserOrderRepository { get; }
        Task<bool> SaveAsync();
    }
}
