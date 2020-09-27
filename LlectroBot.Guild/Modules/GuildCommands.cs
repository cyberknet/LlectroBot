using System;
using System.Threading.Tasks;
using Discord;
using Discord.Cards;
using Discord.Commands;
using LlectroBot.Core.Configuration;
using LlectroBot.Core.Modules;
using LlectroBot.Core.Preconditions;
using LlectroBot.Guild.Cards;

namespace LlectroBot.Modules.Help
{
    public partial class GuildCommands : GuildCommandModuleBase
    {
        public ICardStackManager CardStackManager { get; set; }
        public GuildCommands(IDiscordClient discordClient, IDiscordBot discordBot, IServiceProvider serviceProvider, IBotConfiguration botConfiguration, ICardStackManager cardStackManager)
            : base(discordClient, discordBot, serviceProvider, botConfiguration)
        {
            CardStackManager = cardStackManager;
        }

        [Command("setup")]
        [CommandDescription("Creates or updates the bot configuration for a guild.")]
        [CommandSyntax("{prefix}{command}")]
        [CommandUsage(CommandUsage.Channel)]
        [RequireRole(Roles.RoleLevel.GuildAdministrator, Roles.RoleMatchType.GraterThanOrEqual)]
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task Setup()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            var card = new SetupCard(null, true, new string[] { "cancel" }, null);
            CardStackManager.CreateCardStack(card, Context.Message);
        }
    }
}
