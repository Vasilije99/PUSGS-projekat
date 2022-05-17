using DeliveryApplication.Interfaces;
using DeliveryApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public void Register(string name)
        {
            User user = new User();
            user.Name = name;
            user.Username = "username1";
            user.Email = "email1";
            user.Surname = "surname1";
            user.DateOfBirth = DateTime.Now;
            user.Address = "adresa1";
            user.UserType = UserTypeEnum.Admin;
            user.Image = "image1";
            user.IsVerified = false;
            user.Password = new byte[] { 1 };
            user.PasswordKey = new byte[] { 1 };

            dc.Users.Add(user);
        }
    }
}
