using System;
using Discord;
using Discord.Commands;
using LlectroBot.Core.Configuration;
using LlectroBot.Core.Context;

namespace LlectroBot.Core.Modules
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
