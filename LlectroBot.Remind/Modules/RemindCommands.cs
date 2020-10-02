using System;
using System.Threading.Tasks;
using Discord;
using Discord.Cards;
using Discord.Commands;
using Discord.WebSocket;
using LlectroBot.Core.Configuration;
using LlectroBot.Core.Modules;
using LlectroBot.Core.Preconditions;

namespace LlectroBot.Remind.Modules
{
    public partial class RemindCommands : CommandModuleBase
    {
        public ICardStackManager CardStackManager { get; set; }
        private readonly IReminderService _reminderService;
        public RemindCommands(IDiscordClient discordClient, IDiscordBot discordBot, IServiceProvider serviceProvider, IBotConfiguration botConfiguration, ICardStackManager cardStackManager, IReminderService reminderService)
            : base(discordClient, discordBot, serviceProvider, botConfiguration)
        {
            CardStackManager = cardStackManager;
            _reminderService = reminderService;
        }
    }
}
