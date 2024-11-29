using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using Project_Coffe.Entities;
using System;
using Project_Coffe.Data;
using Project_Coffe.Models.ModelInterface;

namespace CoffeeShopAPI.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _dbContext;

        public UserService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> RegisterUser(User user)
        {
            user.PasswordHash = HashPassword(user.PasswordHash);
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }

        public async Task<User> LoginUser(string email, string password)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null || user.PasswordHash != HashPassword(password))
            {
                return null;
            }
            return user;
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
