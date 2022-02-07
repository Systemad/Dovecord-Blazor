namespace Dovecord.Client.Shared.DTO.Actor
{
    public record ActorCommand(
        string User,
        string OriginalText) : DTO.Actor.Actor(User);
}
