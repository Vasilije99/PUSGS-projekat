using DeliveryApplication.Interfaces;
using DeliveryApplication.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryApplication.Data.Repo
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataContext dc;

        public ProductRepository(DataContext dc)
        {
            this.dc = dc;
        }

        public void AddProduct(string name, string ingredients, double price)
        {
            Product newProduct = new Product();
            newProduct.Name = name;
            newProduct.Ingredients = ingredients;
            newProduct.Price = price;

            dc.Products.Add(newProduct);
        }

        public async Task<List<Product>> GetAllProducts()
        {
            return await dc.Products.ToListAsync();
        }

        public async Task<Product> GetProduct(int id)
        {
            return await dc.Products.FindAsync(id);
        }

        public Product GetProductByName(string productToOrder)
        {
            return dc.Products.FirstOrDefault(x => x.Name == productToOrder);
        }

        public async Task<bool> ProductAlreadyExists(string name)
        {
            return await dc.Products.AnyAsync(x => x.Name == name);
        }

    }
}
