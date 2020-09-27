namespace Discord.Cards
{
    public class CardContext : ICardContext
    {
        public IDiscordClient DiscordClient { get; set; }
        public IGuild Guild { get; set; }
        public IMessageChannel Channel { get; set; }
        public IUser User { get; set; }
        public IUserMessage UserMessage { get; set; }

        public CardContext(IDiscordClient discordClient, IGuild guild, IMessageChannel channel, IUser user, IUserMessage message) : this(discordClient, user, message)
        {
            Guild = guild;
            Channel = channel;
        }

        public CardContext(IDiscordClient discordClient, IUser user, IUserMessage message)
        {
            DiscordClient = discordClient;
            User = user;
            UserMessage = message;
        }
    }
}
