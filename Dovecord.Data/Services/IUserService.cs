using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dovecord.Shared;

namespace Dovecord.Data.Services
{
    public interface IUserService
    {
        Task<List<User>> GetUsersAsync();
        Task<bool> CreateUserAsync(User user);
        Task<bool> CheckIfUserExistAsync(Guid userId);
        Task<bool> UserLoggedOnAsync(User user);
        Task<bool> UserLoggedOffAsync(User user);
        Task<User> GetUserByIdAsync(Guid id);
    }
}