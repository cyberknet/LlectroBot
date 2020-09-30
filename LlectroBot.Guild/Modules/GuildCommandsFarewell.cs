using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using Discord;
using Discord.Commands;
using LlectroBot.Core.Modules;
using LlectroBot.Core.Preconditions;

namespace LlectroBot.Guild.Modules
{
    public partial class GuildCommands
    {
        [Command("farewell")]
        [Alias("partmessage")]
        [CommandDescription("Enables or disables the farewell for a guild. In the farewell message, {mention} will be replaced with a mention of the user.")]
        [CommandSyntax("{prefix}{command} [on|off] [channel] [farewell message]")]
        [CommandUsage(CommandUsage.Channel)]
        [RequireRole(Roles.RoleLevel.GuildAdministrator, Roles.RoleMatchType.GreaterThanOrEqual)]
        public async Task Farewell(params string[] args)
        {
            string farewell = string.Empty;
            if (args.Length == 0)
            {
                await ReplyAsync($"Insufficient parameters provided for farewell command.");
            }
            else
            {
                bool enabled = false;
                string enabledString = args[0];
                switch (enabledString.ToLower().Trim())
                {
                    case "on":
                    case "true":
                        enabled = true;
                        break;
                    case "off":
                    case "false":
                        break;
                    default:
                        await ReplyAsync("Unable to determine if farewell should be enabled or disabled.");
                        return;
                }

                IChannel channel = null;
                if (args.Length > 1)
                {
                    string channelName = args[1];
                    var channels = await this.Context.Guild.GetChannelsAsync();
                    foreach(IChannel guildChan in channels)
                    {
                        if (channelName == MentionUtils.MentionChannel(guildChan.Id))
                        {
                            channel = guildChan;
                            break;
                        }
                    }
                }
                
                
                if (args.Length > 2)
                    farewell = args.Skip(2).ToSentence();
                if (enabled && channel is null)
                {
                    await ReplyAsync("Channel was not provided and is required.");
                }
                else if (enabled && string.IsNullOrWhiteSpace(farewell))
                {
                    await ReplyAsync("Farewell was not provided and is required.");
                }
                else
                {
                    Context.GuildConfiguration.FarewellOnPart = enabled;
                    Context.GuildConfiguration.SetFarewellChannel(channel);
                    Context.GuildConfiguration.Farewell = farewell;
                    BotConfiguration.Save();
                }
            }
        }
    }
}
