using System;

namespace Dovecord.Client.Shared.DTO.Message;

public class MessageManipulationDto
{
    public string? Content { get; set; }
    public Guid ChannelId { get; set; }
}