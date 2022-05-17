using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryApplication.Models
{
    public class Order
    {
        public int Id { get; set; }
        public Product ProductToOrder { get; set; }
        public int Count { get; set; }
        public string DeliveryAddress { get; set; }
        public string Comment { get; set; }
        public double Price { get; set; }
    }
}
