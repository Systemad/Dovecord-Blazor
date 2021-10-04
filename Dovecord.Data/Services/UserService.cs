using System;
using System.Linq;
using System.Threading.Tasks;
using Dovecord.Shared;
using Microsoft.EntityFrameworkCore;

namespace Dovecord.Data.Services
{
    public class UserService : IUserService
    {
        private ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<bool> CreateUser(Guid userId, string username)
        {
            var newuser = new User
            {
                Id = userId,
                Username = username,
            };
            await _context.Users.AddAsync(newuser);
            var created = await _context.SaveChangesAsync();
            return created > 0;
        }

        public async Task<bool> CheckIfUserExist(Guid userId)
        {
            var userexist = await _context.Users.AnyAsync(u => u.Id == userId);
            return userexist;
        }
    }
}