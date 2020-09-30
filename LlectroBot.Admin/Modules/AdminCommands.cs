using System;
using System.Threading.Tasks;
using Discord;
using Discord.Cards;
using Discord.Commands;
using LlectroBot.Core.Configuration;
using LlectroBot.Core.Modules;
using LlectroBot.Core.Preconditions;

namespace LlectroBot.Admin.Modules
{
    public partial class AdminCommands : GuildCommandModuleBase
    {
        public ICardStackManager CardStackManager { get; set; }
        public AdminCommands(IDiscordClient discordClient, IDiscordBot discordBot, IServiceProvider serviceProvider, IBotConfiguration botConfiguration, ICardStackManager cardStackManager)
            : base(discordClient, discordBot, serviceProvider, botConfiguration)
        {
            CardStackManager = cardStackManager;
        }
    }
}
