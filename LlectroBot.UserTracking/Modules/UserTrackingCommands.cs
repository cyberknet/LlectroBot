using System;
using Discord;
using LlectroBot.Core.Configuration;
using LlectroBot.Core.Modules;

namespace LlectroBot.UserTracking.Modules
{
    public partial class UserTrackingCommands : CommandModuleBase
    {
        private readonly IUserTrackerService _userTracker = null;

        public void Test() { }

        public UserTrackingCommands(IDiscordClient discordClient, IDiscordBot discordBot, IServiceProvider serviceProvider, IBotConfiguration botConfiguration, IUserTrackerService userTracker)
            : base(discordClient, discordBot, serviceProvider, botConfiguration)
        {
            _userTracker = userTracker;

        }
    }
}
