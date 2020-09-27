using System;
using Discord;
using Discord.Cards;
using LlectroBot.Core.Configuration;
using LlectroBot.Core.Modules;

namespace LlectroBot.Polling.Modules
{
    public partial class PollingCommands : CommandModuleBase
    {
        private readonly IPollTracker _pollTracker = null;
        private readonly ICardStackManager CardStackManager = null;

        public void Test() { }

        public PollingCommands(IDiscordClient discordClient, IDiscordBot discordBot, IServiceProvider serviceProvider, IBotConfiguration botConfiguration, IPollTracker poller, ICardStackManager cardStackManager)
            : base(discordClient, discordBot, serviceProvider, botConfiguration)
        {
            _pollTracker = poller;
            CardStackManager = cardStackManager;
        }
    }
}
