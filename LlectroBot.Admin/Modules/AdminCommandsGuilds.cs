using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Cards;
using Discord.Commands;
using Discord.WebSocket;
using LlectroBot.Core.Modules;
using LlectroBot.Core.Preconditions;

namespace LlectroBot.Admin.Modules
{
    public partial class AdminCommands
    {
        [Command("guilds")]
        [Alias("guildlist")]
        [CommandDescription("Shows the guilds the bot is a member of, and how many members those guilds have.")]
        [CommandSyntax("{prefix}{command}")]
        [CommandUsage(CommandUsage.Both)]
        [RequireRole(Roles.RoleLevel.GlobalAdministrator, Roles.RoleMatchType.GreaterThanOrEqual)]
        public async Task Guilds()
        {
            EmbedBuilder builder = new EmbedBuilder()
                .WithTitle("Guilds using LlectroBot")
                .WithDescription("The following guilds have LlectroBot in their guild.");
            var iguilds = new List<IGuild>(await Context.Client.GetGuildsAsync());
            var socketGuilds = iguilds.Select(g => g as SocketGuild).OrderByDescending(g => g.MemberCount);
            foreach (var guild in socketGuilds)
            {
                builder.AddField(guild.Name, $"Members: {guild.MemberCount}, Owner: {guild.Owner.Mention}", false);
            }

        }
    }
}
