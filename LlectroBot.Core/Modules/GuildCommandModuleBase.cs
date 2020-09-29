using System;
using Discord;
using Discord.Commands;
using LlectroBot.Core.Configuration;
using LlectroBot.Core.Context;

namespace LlectroBot.Core.Modules
{
    [RequireContext(ContextType.Guild)]
    public abstract class GuildCommandModuleBase : CommandModuleBase
    {
        public GuildCommandModuleBase(IDiscordClient discordClient, IDiscordBot discordBot, IServiceProvider serviceProvider, IBotConfiguration botConfiguration)
            : base(discordClient, discordBot, serviceProvider, botConfiguration)
        {
        }
    }
}
