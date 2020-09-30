using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Cards;
using Discord.Commands;
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
            var guilds = new List<IGuild>(await Context.Client.GetGuildsAsync());
            foreach (var guild in guilds)
            {
                var guildMembers = await guild.GetUsersAsync();
                var guildOwner = await guild.GetOwnerAsync();
                builder.AddField(guild.Name, $"Members: {guildMembers.Count()}, Owner: {guildOwner.Mention}", false);
            }

        }
    }
}
