using AutoMapper;
using DeliveryApplication.Dtos;
using DeliveryApplication.Interfaces;
using DeliveryApplication.Models;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryApplication.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("AllowAllHeaders")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork uow;
        private readonly IConfiguration configuration;
        private readonly IMapper mapper;

        public UserController(IUnitOfWork uow, IConfiguration configuration, IMapper mapper)
        {
            this.uow = uow;
            this.configuration = configuration;
            this.mapper = mapper;
        }

        [HttpGet("getUser/{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            try
            {
                var user = await uow.UserRepository.FindUser(id);
                var userDto = mapper.Map<UserDto>(user);
                return Ok(userDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #region Register region
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto newUser)
        {
            if (await uow.UserRepository.UserAlreadyExists(newUser.Username))
                return BadRequest("User already exists, please try something else.");
            if (RegisterUserFieldsValidation(newUser, out string message))
                return BadRequest(message);

            string[] temp = newUser.Image.Split((char)92);

            uow.UserRepository.Register(newUser.Name, newUser.Username, newUser.Email, newUser.Surname, newUser.DateOfBirth, newUser.Address, newUser.UserType, temp[temp.Length - 1], newUser.Password1);
            await uow.SaveAsync();
            return StatusCode(201);
        }

        #endregion

        #region Login region
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginRequest)
        {
            try
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
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

        [HttpPut("verifyUser/{username}")]
        public async Task<IActionResult> VerifyUser(string username)
        {
            try
            {
                var user = await uow.UserRepository.FindUserByUsername(username);
                user.IsVerified = VerificationStep.Verified;

                SendMail();

                await uow.SaveAsync();
                var userDto = mapper.Map<UserDto>(user);
                return Ok(userDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private void SendMail()
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Verification Confirmation", "vasilije240799@gmail.com"));
            message.To.Add(new MailboxAddress("vasilije", "vunkovic08@gmail.com"));
            message.Subject = "verification";
            message.Body = new TextPart("plain")
            {
                Text = "Your account is verified, and now you is a verified deliverer. Congretulations!"
            };

            using(var client = new SmtpClient())
            {
                try
                {
                    client.Connect("smtp.gmail.com", 465, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate("vasilije240799@gmail.com", "hijipitqgequumcb");
                    client.Send(message);
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }

        [HttpPut("declineVerification/{username}")]
        public async Task<IActionResult> DeclineVerification(string username)
        {
            try
            {
                var user = await uow.UserRepository.FindUserByUsername(username);
                user.IsVerified = VerificationStep.NotVerified;
                await uow.SaveAsync();
                var userDto = mapper.Map<UserDto>(user);
                return Ok(userDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("updateUser/{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserDto updatedUserDto)
        {
            try
            {
                var user = await uow.UserRepository.FindUser(id);
                mapper.Map(updatedUserDto, user);
                await uow.SaveAsync();
                return Ok(updatedUserDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("passwordChange/{id}")]
        public async Task<IActionResult> ChangePassword(int id, PaswordChangeDto pcDto)
        {
            try
            {
                var user = await uow.UserRepository.FindUser(id);
                if(ComparePassword(pcDto.CurrentPassword, user.Password, user.PasswordKey))
                {
                    byte[] passwordHash, passwordKey;
                    using (var hmac = new HMACSHA512())
                    {
                        passwordKey = hmac.Key;
                        passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(pcDto.NewPassword));
                    }

                    user.Password = passwordHash;
                    user.PasswordKey = passwordKey;

                    await uow.SaveAsync();
                    return Ok();
                }

                return BadRequest("Error");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        bool ComparePassword(string password, byte[] passwordFromDb, byte[] passwordKey)
        {
            using (var hmac = new HMACSHA512(passwordKey))
            {
                var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < passwordHash.Length; i++)
                {
                    if (passwordHash[i] != passwordFromDb[i])
                        return false;
                }

                return true;
            }
        }

        [HttpPut("verificationRequest/{id}")]
        public async Task<IActionResult> VerificationRequest(int id)
        {
            try
            {
                var user = await uow.UserRepository.FindUser(id);
                user.IsVerified = VerificationStep.PendingVerification;
                await uow.SaveAsync();
                var userDto = mapper.Map<UserDto>(user);
                return Ok(userDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("pendingVerificationUsers")]
        public async Task<IActionResult> GetNotVerificationUsers()
        {
            try
            {
                var users = await uow.UserRepository.GetUnverifiedUsers();
                var userDto = mapper.Map<List<UserDto>>(users);
                return Ok(userDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("deliverers")]
        public async Task<IActionResult> GetDeliverers()
        {
            try
            {
                var deliverers = await uow.UserRepository.AllDeliverers();
                var userDto = mapper.Map<List<UserDto>>(deliverers);
                return Ok(userDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private bool RegisterUserFieldsValidation(RegisterDto newUser, out string message)
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

            if (newUser.Password1.Length < 3 || newUser.Password2.Length < 3)
            {
                message = "Minimum length of password is 3";
                return true;
            }

            message = "";
            return false;
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
    }
}
