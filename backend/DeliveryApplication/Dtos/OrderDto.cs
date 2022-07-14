using DeliveryApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryApplication.Dtos
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string DeliveryAddress { get; set; }
        public string Comment { get; set; }
        public double Price { get; set; }
        public OrderStateEnum OrderState { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
    }
}
