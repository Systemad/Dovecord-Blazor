namespace Dovecord.Client.Shared.DTO.Actor
{
    public record ActorAction(string User, bool IsTyping) : DTO.Actor.Actor(User);
}
