using Discord;
using Discord.Commands;
using LlectroBot.Configuration;

namespace LlectroBot.Context
{
    public class DiscordCommandContext : CommandContext
    {
        public IGuildConfiguration GuildConfiguration { get; private set; }
        public IGuildUser GuildUser { get; set; }
        public IChannel GuildChannel { get; set; }

        public IDiscordBot DiscordBot { get; set; }

        public DiscordCommandContext(IDiscordClient client, IUserMessage message, IGuildConfiguration guildConfiguration, IDiscordBot discordBot, IGuildUser guildUser)
            : base(client, message)
        {
            GuildConfiguration = guildConfiguration;
            DiscordBot = discordBot;
            GuildUser = guildUser;
        }

    }
}
