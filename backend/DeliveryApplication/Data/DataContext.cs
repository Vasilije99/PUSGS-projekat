using DeliveryApplication.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryApplication.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }
        public DbSet<UserOrder> UserOrders { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderProduct>()
                .HasKey(op => new { op.OrderId, op.ProductId });
            modelBuilder.Entity<OrderProduct>()
                .HasOne(op => op.Order)
                .WithMany(o => o.OrderProducts)
                .HasForeignKey(op => op.OrderId);
            modelBuilder.Entity<OrderProduct>()
                .HasOne(op => op.Product)
                .WithMany(p => p.OrderProducts)
                .HasForeignKey(op => op.ProductId);

            modelBuilder.Entity<UserOrder>()
                .HasKey(uo => new { uo.UserId, uo.OrderId });
            modelBuilder.Entity<UserOrder>()
                .HasOne(uo => uo.User)
                .WithMany(u => u.UserOrders)
                .HasForeignKey(uo => uo.UserId);
            modelBuilder.Entity<UserOrder>()
                .HasOne(uo => uo.Order)
                .WithMany(o => o.UserOrders)
                .HasForeignKey(uo => uo.OrderId);

            //modelBuilder.Entity<Order>().Property(x => x.Deliverer).IsRequired(false);
            //base.OnModelCreating(modelBuilder);
        }
    }
}
