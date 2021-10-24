using System;
using System.Threading.Tasks;
using Dovecord.Shared;
using Refit;

namespace Dovecord.Client.Services;

public interface IUserApi
{
    [Post("/User/connect")]
    Task SendConnectedUser(User user);
}