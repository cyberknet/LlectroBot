using System;
using System.Threading.Tasks;
using Discord;
using Discord.Cards;
using Discord.Commands;
using LlectroBot.Core.Configuration;
using LlectroBot.Core.Modules;
using LlectroBot.Core.Preconditions;
using LlectroBot.Guild.Cards;

namespace LlectroBot.Guild.Modules
{
    public partial class GuildCommands : GuildCommandModuleBase
    {
        public ICardStackManager CardStackManager { get; set; }
#pragma warning disable IDE0052 // Remove unread private members
        private readonly IGuildService _guildService;
#pragma warning restore IDE0052 // Remove unread private members
        public GuildCommands(IDiscordClient discordClient, IDiscordBot discordBot, IServiceProvider serviceProvider, IBotConfiguration botConfiguration, ICardStackManager cardStackManager, IGuildService guildService)
            : base(discordClient, discordBot, serviceProvider, botConfiguration)
        {
            _guildService = guildService;
            CardStackManager = cardStackManager;
        }
    }
}
