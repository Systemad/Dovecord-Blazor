namespace Dovecord.Shared
{
    public record ActorCommand(
        string User,
        string OriginalText) : Actor(User);
}
