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
        [Command("greeting")]
        [Alias("greet")]
        [CommandDescription("Enables or disables the greeting for a guild. In the greeting, {mention} will be replaced with a mention of the user.")]
        [CommandSyntax("{prefix}{command} [on|off] [channel] [greeting]")]
        [CommandUsage(CommandUsage.Channel)]
        [RequireRole(Roles.RoleLevel.GuildAdministrator, Roles.RoleMatchType.GreaterThanOrEqual)]
        public async Task Greeting(params string[] args)
        {
            string greeting = string.Empty;
            if (args.Length == 0)
            {
                await ReplyAsync($"Insufficient parameters provided for greeting command.");
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
                        await ReplyAsync("Unable to determine if greeting should be enabled or disabled.");
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
                    greeting = args.Skip(2).ToSentence();
                if (enabled && channel is null)
                {
                    await ReplyAsync("Channel was not provided and is required.");
                }
                else if (enabled && string.IsNullOrWhiteSpace(greeting))
                {
                    await ReplyAsync("Greeting was not provided and is required.");
                }
                else
                {
                    Context.GuildConfiguration.GreetingOnJoin = enabled;
                    Context.GuildConfiguration.SetGreetingChannel(channel);
                    Context.GuildConfiguration.Greeting = greeting;
                    BotConfiguration.Save();
                }
            }
        }
    }
}
