namespace Dovecord.Shared
{
    public record ActorAction(string User, bool IsTyping) : Actor(User);
}
