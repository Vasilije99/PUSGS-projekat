using DeliveryApplication.Interfaces;
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
    public class UserOrderController : ControllerBase
    {
        private readonly IUnitOfWork uow;

        public UserOrderController(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        [HttpPost("newUserOrder/{userId}/{orderId}")]
        public async Task<IActionResult> CreateNewUserOrder(int userId, int orderId)
        {
            uow.UserOrderRepository.AddNewUserOrder(userId, orderId);

            await uow.SaveAsync();
            return StatusCode(201);
        }
    }
}
