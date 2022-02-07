using System;
using System.Threading.Tasks;
using Dovecord.Client.Shared.DTO.Actor;
using Dovecord.Client.Shared.DTO.Message;
using Microsoft.AspNetCore.SignalR.Client;

namespace Dovecord.Client.Extensions;

public static class HubConnectionExtensions
{
    public static IDisposable OnMessageReceived(
        this HubConnection connection, Func<ChannelMessageDto, Task> handler) =>
        connection.On("MessageReceived", handler);
        
    public static IDisposable OnDeleteMessageReceived(
        this HubConnection connection, Func<string, Task> handler) =>
        connection.On("DeleteMessageReceived", handler);
    
    public static IDisposable DataRefresh(
        this HubConnection connection, Func<Task> handler) =>
        connection.On("UpdateData", handler);

    public static IDisposable OnUserTyping(
        this HubConnection connection, Func<ActorAction, Task> handler) =>
        connection.On("UserTyping", handler);
}