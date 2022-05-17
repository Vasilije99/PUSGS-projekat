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
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork uow;

        public UserController(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(string name)
        {
            uow.UserRepository.Register(name);
            await uow.SaveAsync();
            return StatusCode(201);
        }
    }
}
