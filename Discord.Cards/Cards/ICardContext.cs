namespace Discord.Cards
{
    public interface ICardContext
    {
        IDiscordClient DiscordClient { get; set; }
        IGuild Guild { get; set; }
        IMessageChannel Channel { get; set; }
        IUser User { get; set; }
        IUserMessage UserMessage { get; set; }
    }
}
