using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryApplication.Models
{
    public enum UserTypeEnum { Consumer, Deliverer, Admin }
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public byte[] Password { get; set; }
        public byte[] PasswordKey { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public UserTypeEnum UserType { get; set; }
        public string Image { get; set; }
        public bool IsVerified { get; set; }
    }
}
