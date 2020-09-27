using Discord;
using Discord.Cards;
using LlectroBot.Core.Configuration;

namespace LlectroBot.Core.Cards
{
    public class LlectroBotCardContext : CardContext
    {
        public IBotConfiguration BotConfiguration { get; set; }
        public IGuildConfiguration GuildConfiguration { get; set; }

        public LlectroBotCardContext(IDiscordClient discordClient, IUser user, IUserMessage message, IBotConfiguration botConfiguration, IGuildConfiguration guildConfiguration) : base(discordClient, user, message)
        {
            BotConfiguration = botConfiguration;
            GuildConfiguration = guildConfiguration;
        }
        public LlectroBotCardContext(IDiscordClient discordClient, IGuild guild, IMessageChannel channel, IUser user, IUserMessage message, IBotConfiguration botConfiguration, IGuildConfiguration guildConfiguration) : base(discordClient, guild, channel, user, message)
        {
            BotConfiguration = botConfiguration;
            GuildConfiguration = guildConfiguration;
        }




    }
}
