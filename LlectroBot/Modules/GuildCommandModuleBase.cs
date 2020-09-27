using System;
using Discord;
using Discord.Commands;
using LlectroBot.Configuration;
using LlectroBot.Context;

namespace LlectroBot.Modules
{
    [RequireContext(ContextType.Guild)]
    public abstract class GuildCommandModuleBase : ModuleBase<DiscordCommandContext>
    {
        protected readonly IDiscordBot _discordBot;
        protected readonly IBotConfiguration _botConfiguration;
        protected readonly IServiceProvider _serviceProvider;
        protected readonly IDiscordClient _discordClient;

        public GuildCommandModuleBase(IDiscordClient discordClient, IDiscordBot discordBot, IServiceProvider serviceProvider, IBotConfiguration botConfiguration)
        {
            _discordClient = discordClient;
            _discordBot = discordBot;
            _botConfiguration = botConfiguration;
            _serviceProvider = serviceProvider;
        }
    }
}
