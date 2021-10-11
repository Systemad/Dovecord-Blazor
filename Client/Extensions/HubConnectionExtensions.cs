using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dovecord.Shared;
using Microsoft.AspNetCore.SignalR.Client;

namespace Dovecord.Client.Extensions
{
    public static class HubConnectionExtensions
    {
        public static IDisposable OnMessageReceived(
            this HubConnection connection, Func<ChannelMessage, Task> handler) =>
            connection.On("MessageReceived", handler);
        
        public static IDisposable OnDeleteMessageReceived(
            this HubConnection connection, Func<Guid, Task> handler) =>
            connection.On("DeleteMessageReceived", handler);

        public static IDisposable OnUserListReceived(
            this HubConnection connection, Func<List<User>, Task> handler) =>
            connection.On("SendUserList", handler);

        public static IDisposable OnUserTyping(
            this HubConnection connection, Func<ActorAction, Task> handler) =>
            connection.On("UserTyping", handler);
    }
}
