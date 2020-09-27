using System;
using Discord;
using LlectroBot.Core.Configuration;
using LlectroBot.Core.Modules;

namespace LlectroBot.UserTracking.Modules
{
    public partial class UserTrackingCommands : CommandModuleBase
    {
        private readonly IUserTracker _userTracker = null;

        public void Test() { }

        public UserTrackingCommands(IDiscordClient discordClient, IDiscordBot discordBot, IServiceProvider serviceProvider, IBotConfiguration botConfiguration, IUserTracker userTracker)
            : base(discordClient, discordBot, serviceProvider, botConfiguration)
        {
            _userTracker = userTracker;

        }
    }
}
