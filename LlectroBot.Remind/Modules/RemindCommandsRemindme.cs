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

namespace LlectroBot.Remind.Modules
{
    public partial class RemindCommands
    {
        [Command("remindme")]
        [Alias("remind", "reminder")]
        [CommandDescription("Sets a reminder for the bot to tell you something after a period of time elapses. Timeframe is passed in the form of 1y 3m 6d 1h 5m 2s")]
        [CommandSyntax("{prefix}{command} [timeframe] [remind text]")]
        [CommandUsage(CommandUsage.Both)]
        [RequireRole(Roles.RoleLevel.GlobalAdministrator, Roles.RoleMatchType.GreaterThanOrEqual)]
        public async Task RemindMe(TimeSpan timespan = default, params string[] args)
        {
            if (timespan != null)
            {
                await ReplyAsync($"No timespan provided, unable to send a reminder for {Context.User.Mention}");
            }
            else if (args == null || args.Length < 1 || string.IsNullOrWhiteSpace(args[0]))
            {
                await ReplyAsync($"No reminder text provided, unable to send a reminder for {Context.User.Mention}");
            }
            else
            {
                string reminderText = args.ToSentence();
                _reminderService.SetReminder(Context.User, Context.Guild, Context.Channel as ITextChannel, timespan, reminderText);
            }

        }
    }
}
