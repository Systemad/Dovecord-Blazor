namespace Dovecord.Shared
{
    public class ApiRoutes
    {
        public const string Base = "/api";
        public const string Health = Base + "/health";

        // new api route marker - do not delete

        public static class Users
        {
            public const string UserId = "{userId}";
            public const string GetList = Base + "/users";
            public const string GetRecord = Base + "/users/" + UserId;
            public const string Create = Base + "/users";
            public const string Delete = Base + "/users/" + UserId;
            public const string Put = Base + "/users/" + UserId;
            public const string Patch = Base + "/users/" + UserId;
        }

        public static class Channels
        {
            public const string ChannelId = "{channelId}";
            public const string GetList = Base + "/channels";
            public const string GetRecord = Base + "/channels/" + ChannelId;
            public const string Create = Base + "/channels";
            public const string Delete = Base + "/channels/" + ChannelId;
            public const string Put = Base + "/channels/" + ChannelId;
            public const string Patch = Base + "/channels/" + ChannelId;
        }
        public static class Messages
        {
            public const string MessageId = "{messageId}";
            public const string GetList = Base + "/messages";
            public const string GetRecord = Base + "/messages/" + MessageId;
            public const string Create = Base + "/messages";
            public const string Delete = Base + "/messages/" + MessageId;
            public const string Put = Base + "/messages/" + MessageId;
            public const string Patch = Base + "/messages/" + MessageId;
        }
    }
}
