using DeliveryApplication.Dtos;
using DeliveryApplication.Interfaces;
using DeliveryApplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork uow;
        private readonly IConfiguration configuration;

        public UserController(IUnitOfWork uow, IConfiguration configuration)
        {
            this.uow = uow;
            this.configuration = configuration;
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

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginRequest)
        {
            var user = await uow.UserRepository.Login(loginRequest.Email, loginRequest.Password);
            if (user == null)
                return BadRequest("User with this datas doesn't exist");

            var loginResponse = new LoginResponseDto();
            loginResponse.Id = user.Id;
            loginResponse.Email = user.Email;
            loginResponse.Token = CreateJWT(user);

            return Ok(loginResponse);
        }

        private string CreateJWT(User user)
        {
            var secretKey = configuration.GetSection("AppSettings:Key").Value;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(10),
                SigningCredentials = signingCredentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
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
