using System;
using System.Threading.Tasks;
using Discord;
using Discord.Cards;
using Discord.Commands;
using Discord.WebSocket;
using LlectroBot.Core.Configuration;
using LlectroBot.Core.Modules;
using LlectroBot.Core.Preconditions;

namespace LlectroBot.Admin.Modules
{
    public partial class AdminCommands : GuildCommandModuleBase
    {
        public ICardStackManager CardStackManager { get; set; }
        private readonly DiscordSocketClient _discordSocketClient;
        public AdminCommands(IDiscordClient discordClient, IDiscordBot discordBot, IServiceProvider serviceProvider, IBotConfiguration botConfiguration, ICardStackManager cardStackManager, DiscordSocketClient discordSocketClient)
            : base(discordClient, discordBot, serviceProvider, botConfiguration)
        {
            CardStackManager = cardStackManager;
            _discordSocketClient = discordSocketClient;
        }
    }
}
