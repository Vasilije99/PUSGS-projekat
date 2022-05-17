using DeliveryApplication.Interfaces;
using DeliveryApplication.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryApplication.Data.Repo
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext dc;

        public UserRepository(DataContext dc)
        {
            this.dc = dc;
        }

        public async Task<User> Login(string email, string password)
        {
            var user = await dc.Users.FirstOrDefaultAsync(x => x.Email == email);

            if (user == null || user.PasswordKey == null)
                return null;
            if (!MatchPassword(password, user.Password, user.PasswordKey))
                return null;
            return user;
        }

        private bool MatchPassword(string password, byte[] passwordFromDb, byte[] passwordKey)
        {
            using(var hmac = new HMACSHA512(passwordKey))
            {
                var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for(int i = 0; i < passwordHash.Length; i++)
                {
                    if (passwordHash[i] != passwordFromDb[i])
                        return false;
                }

                return true;
            }
        }

        public void Register(string name, string username, string email, string surename, DateTime dateOfBirth, string address, string userType, string image, string password)
        {
            byte[] passwordHash, passwordKey;

            using(var hmac = new HMACSHA512())
            {
                passwordKey = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }

            User user = new User();
            user.Name = name;
            user.Username = username;
            user.Email = email;
            user.Surname = surename;
            user.DateOfBirth = dateOfBirth;
            user.Address = address;
            switch (userType)
            {
                case "Admin":
                    user.UserType = UserTypeEnum.Admin;
                    break;
                case "Deliverer":
                    user.UserType = UserTypeEnum.Deliverer;
                    break;
                default:
                    user.UserType = UserTypeEnum.Consumer;
                    break;
            }
            user.Image = "image1";
            user.IsVerified = false;
            user.Password = passwordHash;
            user.PasswordKey = passwordKey;

            dc.Users.Add(user);
        }

        public async Task<bool> UserAlreadyExists(string username)
        {
            return await dc.Users.AnyAsync(x => x.Username == username);
        }
    }
}
