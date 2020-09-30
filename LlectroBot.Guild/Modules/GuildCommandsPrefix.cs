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
        [Command("prefix")]
        [CommandDescription("changes the command prefix used by the bot in this guild.")]
        [CommandSyntax("{prefix}{command}")]
        [CommandUsage(CommandUsage.Channel)]
        [RequireRole(Roles.RoleLevel.GuildAdministrator, Roles.RoleMatchType.GreaterThanOrEqual)]
        public async Task Prefix(params string[] args)
        {
            if (args == null || args.Length < 1)
                await ReplyAsync("No new prefix was provided.");
            else if (string.IsNullOrWhiteSpace(args[0]) || args[0].Length > 1)
                await ReplyAsync("Provided prefix is not valid.");
            else
            {
                var newPrefix = args[0][0];
                var previousPrefix = Context.GuildConfiguration.CommandPrefix;
                Context.GuildConfiguration.CommandPrefix = newPrefix;
                await ReplyAsync("Prefix changed from {previousPrefix} to {newPrefix}.");
            }
        }
    }
}
