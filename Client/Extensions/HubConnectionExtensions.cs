using System;
using System.Threading.Tasks;
using Dovecord.Shared;
using Microsoft.AspNetCore.SignalR.Client;

namespace Dovecord.Client.Extensions
{
    public static class HubConnectionExtensions
    {
        public static IDisposable OnMessageReceived(
            this HubConnection connection, Func<ActorMessage, Task> handler) =>
            connection.On("MessageReceived", handler);
        
        public static IDisposable OnDeleteMessageReceived(
            this HubConnection connection, Func<string, Task> handler) =>
            connection.On("DeleteMessageReceived", handler);

        public static IDisposable OnUserLoggedOn(
            this HubConnection connection, Func<Actor, Task> handler) =>
            connection.On("UserLoggedOn", handler);

        public static IDisposable OnUserLoggedOff(
            this HubConnection connection, Func<Actor, Task> handler) =>
            connection.On("UserLoggedOff", handler);

        public static IDisposable OnUserTyping(
            this HubConnection connection, Func<ActorAction, Task> handler) =>
            connection.On("UserTyping", handler);
    }
}
