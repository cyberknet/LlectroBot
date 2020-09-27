using System;
using Discord;
using LlectroBot.Core.Configuration;
using LlectroBot.Core.Modules;

namespace LlectroBot.Games.Modules
{
    public partial class GameCommands : CommandModuleBase
    {
        public GameCommands(IDiscordClient discordClient, IDiscordBot discordBot, IServiceProvider serviceProvider, IBotConfiguration botConfiguration)
            : base(discordClient, discordBot, serviceProvider, botConfiguration)
        {
        }


    }
}
