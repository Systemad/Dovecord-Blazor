using System;
using System.Threading.Tasks;
using Dovecord.Shared;

namespace Dovecord.Data.Services
{
    public interface IUserService
    {
        Task<bool> CreateUser(User user);
        Task<bool> CheckIfUserExist(Guid userId);
    }
}