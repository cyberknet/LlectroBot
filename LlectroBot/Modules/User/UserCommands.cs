using System;
using Discord;
using LlectroBot.Core.Configuration;
using LlectroBot.Core.Modules;
using LlectroBot.UserTracking;

namespace LlectroBot.Modules.User
{
    public partial class UserCommands : CommandModuleBase
    {
        private readonly IUserTracker _userTracker = null;

        public UserCommands(IDiscordClient discordClient, IDiscordBot discordBot, IServiceProvider serviceProvider, IBotConfiguration botConfiguration, IUserTracker userTracker)
            : base(discordClient, discordBot, serviceProvider, botConfiguration)
        {
            _userTracker = userTracker;

        }
    }
}
