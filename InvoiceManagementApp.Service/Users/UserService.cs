using InvoiceManagementApp.Models.Models;
using InvoiceManagementApp.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceManagementApp.Service.Users
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly PasswordHasher<User> _passwordHasher;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _passwordHasher = new PasswordHasher<User>();
        }

        public async Task RegisterAsync(User user)
        {
            // Hash the password
            user.Password = _passwordHasher.HashPassword(user, user.Password);
            await _userRepository.AddAsync(user);
        }
        public async Task<User> AuthenticateAsync(string username, string password)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user == null) return null;

            // Verify the password
            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, password);
            if (result == PasswordVerificationResult.Success)
            {
                return user;
            }

            return null;
        }
        public async Task<User> GetByVerificationTokenAsync(string token)
        {
            return await _userRepository.FirstOrDefaultAsync(u => u.VerificationToken == token);
        }
        public async Task UpdateAsync(User user)
        {
            await _userRepository.UpdateAsync(user);
        }
    }
}
