using System;

namespace Dovecord.Client.Shared.DTO.User;

public class UserDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public bool? IsOnline { get; set; }
}