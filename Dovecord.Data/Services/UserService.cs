using System;
using System.Collections.Generic;
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

        public async Task<List<User>> GetUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<bool> CreateUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            var created = await _context.SaveChangesAsync();
            return created > 0;
        }

        public async Task<bool> CheckIfUserExistAsync(Guid userId)
        {
            var userexist = await _context.Users.AnyAsync(u => u.Id == userId);
            return userexist;
        }

        public async Task<bool> UserLoggedOnAsync(User user)
        {
            _context.Attach(user);
            user.Online = true;
            //_context.Update(user);
            var saved = await _context.SaveChangesAsync();
            return saved > 0;
        }

        public async Task<bool> UserLoggedOffAsync(User user)
        {
            _context.Attach(user);
            user.Online = false;
            //_context.Update(user);
            var saved = await _context.SaveChangesAsync();
            return saved > 0;
        }

        public async Task<User> GetUserByIdAsync(Guid id)
        {
            return await _context.Users.SingleOrDefaultAsync(x => x.Id == id);
        }
    }
}