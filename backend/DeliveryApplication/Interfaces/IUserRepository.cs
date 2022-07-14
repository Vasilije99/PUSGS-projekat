using DeliveryApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryApplication.Interfaces
{
    public interface IUserRepository
    {
        void Register(string name, string username, string email, string surename, DateTime dateOfBirth, string address, string userType, string image, string password);
        Task<bool> UserAlreadyExists(string username);
        Task<User> Login(string email, string password);
        Task<User> FindUser(int id);
        Task<User> FindUserByUsername(string username);
        Task<List<User>> GetUnverifiedUsers();
        Task<List<User>> AllDeliverers();
    }
}
