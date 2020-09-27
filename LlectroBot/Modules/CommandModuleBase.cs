using System;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using LlectroBot.Configuration;
using LlectroBot.Context;

namespace LlectroBot.Modules
{
    [RequireContext(ContextType.DM | ContextType.Group | ContextType.Guild)]
    public abstract class CommandModuleBase : ModuleBase<DiscordCommandContext>
    {
        protected readonly IDiscordBot DiscordBot;
        protected readonly IBotConfiguration BotConfiguration;
        protected readonly IServiceProvider ServiceProivder;
        protected readonly IDiscordClient DiscordClient;

        public CommandModuleBase(IDiscordClient discordClient, IDiscordBot discordBot, IServiceProvider serviceProvider, IBotConfiguration botConfiguration)
        {
            DiscordClient = discordClient;
            DiscordBot = discordBot;
            BotConfiguration = botConfiguration;
            ServiceProivder = serviceProvider;
        }
    }
}
