using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryApplication.Models
{
    public enum OrderStateEnum { PENDING , ACTIVE , FINISHED  };
    public class Order
    {
        public int Id { get; set; }
        public string DeliveryAddress { get; set; }
        public string Comment { get; set; }
        public double Price { get; set; }
        public OrderStateEnum OrderState { get; set; }
        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
        public User Deliverer { get; set; }
        public virtual ICollection<UserOrder> UserOrders { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
    }
}
