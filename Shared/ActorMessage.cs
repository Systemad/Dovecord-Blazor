namespace Dovecord.Shared
{
    public record ActorMessage(
        string Id, string Text, string User, bool IsGreeting = false, bool IsEdit = false) : Actor(User);
}
