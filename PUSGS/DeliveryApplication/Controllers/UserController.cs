using DeliveryApplication.Dtos;
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
        public async Task<IActionResult> Register(RegisterDto newUser)
        {
            if (await uow.UserRepository.UserAlreadyExists(newUser.Username))
                return BadRequest("User already exists, please try something else.");
            if (FieldsValidation(newUser, out string message))
                return BadRequest(message);

            uow.UserRepository.Register(newUser.Name, newUser.Username, newUser.Email, newUser.Surname, newUser.DateOfBirth, newUser.Address, newUser.UserType, newUser.Image, newUser.Password1);
            await uow.SaveAsync();
            return StatusCode(201);
        }

        private bool FieldsValidation(RegisterDto newUser, out string message)
        {
            if (newUser.Username.Length == 0 ||
                   newUser.Email.Length == 0 ||
                   newUser.Name.Length == 0 ||
                   newUser.Surname.Length == 0 ||
                   newUser.Address.Length == 0 ||
                   newUser.UserType.Length == 0 ||
                   newUser.DateOfBirth.ToString().Length == 0)
            {
                message = "All fields must be filled!";
                return true;
            }

            if (newUser.Password1 != newUser.Password2)
            {
                message = "Pasword are not matching";
                return true;
            }

            if(newUser.Password1.Length < 3 || newUser.Password2.Length < 3)
            {
                message = "Minimum length of password is 3";
                return true;
            }

            message = "";
            return false;
        }
    }
}
