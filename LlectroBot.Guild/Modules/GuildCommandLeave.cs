using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord.Cards;
using Discord.Commands;
using LlectroBot.Core.Modules;
using LlectroBot.Core.Preconditions;
using LlectroBot.Guild.Cards;

namespace LlectroBot.Guild.Modules
{
    public partial class GuildCommands
    {
        [Command("leave")]
        [CommandDescription("Asks the bot to leave the current guild.")]
        [CommandSyntax("{prefix}{command}")]
        [CommandUsage(CommandUsage.Channel)]
        [RequireRole(Roles.RoleLevel.GuildAdministrator, Roles.RoleMatchType.GraterThanOrEqual)]
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task Leave()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            var card = new SetupCard(null, true, new string[] { "cancel" }, null);
            CardStackManager.CreateCardStack(card, Context.Message);
        }
    }
}
