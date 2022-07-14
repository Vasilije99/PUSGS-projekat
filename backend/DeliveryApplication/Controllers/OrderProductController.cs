using DeliveryApplication.Interfaces;
using DeliveryApplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderProductController : ControllerBase
    {
        private readonly IUnitOfWork uow;

        public OrderProductController(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        [HttpPost("newOrderProduct/{orderId}")]
        public async Task<IActionResult> CreateNewOrderProduct(Dictionary<string,int> products, int orderId)
        {
            foreach (var item in products)
            {
                uow.OrderProductsRepository.AddNewOrderProduct(item.Key, item.Value, orderId);
            }

            await uow.SaveAsync();
            return StatusCode(201);
        }

        [HttpGet("orderDetails/{orderId}")] 
        public async Task<IActionResult> GetOrderDetails(int orderId)
        {
            try
            {
                List<OrderProduct> orderProducts = await uow.OrderProductsRepository.OrderProductsByOrderId(orderId);
                Dictionary<string, int> products = new Dictionary<string, int>();

                for (int i = 0; i < orderProducts.Count; i++)
                {
                    Product product = await uow.ProductRepository.GetProduct(orderProducts[i].ProductId);
                    products.Add(product.Name, orderProducts[i].Count);
                }

                return Ok(products);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
