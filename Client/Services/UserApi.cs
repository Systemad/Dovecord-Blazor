using System.Threading.Tasks;
using Dovecord.Client.Shared.DTO.User;
using Refit;

namespace Dovecord.Client.Services;

public interface IUserApi
{
    [Post("/User/connect")]
    Task SendConnectedUser(UserDto user);
}