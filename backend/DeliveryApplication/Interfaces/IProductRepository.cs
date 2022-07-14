using DeliveryApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryApplication.Interfaces
{
    public interface IProductRepository
    {
        Task<bool> ProductAlreadyExists(string name);
        void AddProduct(string name, string ingredients, double price);
        Task<List<Product>> GetAllProducts();
        Product GetProductByName(string productToOrder);
        Task<Product> GetProduct(int id);
    }
}
