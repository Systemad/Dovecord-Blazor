using System;
using System.Threading.Tasks;

namespace Dovecord.Data.Services
{
    public interface IUserService
    {
        Task<bool> CreateUser(Guid userId, string username);
        Task<bool> CheckIfUserExist(Guid userId);
    }
}