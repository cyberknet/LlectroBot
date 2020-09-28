using System;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Discord.Commands;
using LlectroBot.Core.Modules;
using LlectroBot.Polling.Cards;

namespace LlectroBot.Polling.Modules
{
    public partial class PollingCommands
    {
        #region poll
        [Command("poll")]
        [CommandDescription("Obtains the information required to start a poll.")]
        [CommandSyntax("{prefix}{command} [duration] [question]")]
        [CommandUsage(CommandUsage.Channel)]
        public async Task Poll(params string[] options)
        {
            string duration = (options.Length > 0) ? (options[0] ?? string.Empty).ToLower().Trim() : string.Empty;
            string question = (options.Length > 1) ? string.Join(" ", options.Skip(1)) : string.Empty;
            bool hasPoll = _pollTracker.UserHasActivePoll(Context.User, Context.Channel);
            duration = duration?.ToLower().Trim(); 
            if (hasPoll)
            {
                await ReplyAsync("You already have a poll active on this channel, and cannot create another.");
            }
            else if (string.IsNullOrWhiteSpace(duration))
            {
                await ReplyAsync("You need to provide a duration and a question to start a poll. Duration may be either 5m, 10m, or 15m");
            }
            else if (duration!="1m" && duration != "5m" && duration != "10m" && duration != "15m")
            {
                await ReplyAsync("Duration may only be set to 5, 10, or 15 minutes by specifying 5m, 10m, or 15m respectively.");
            }
            else if (string.IsNullOrWhiteSpace(question))
            {
                await ReplyAsync("You need to provide a question to start a poll.");
            }
            else
            {
                TimeSpan durationTimeSpan = new TimeSpan(0, 1, 0);
                if (duration == "5m")
                    durationTimeSpan = new TimeSpan(0, 5, 0);
                if (duration == "10m")
                    durationTimeSpan = new TimeSpan(0, 10, 0);
                else if (duration == "15m")
                    durationTimeSpan = new TimeSpan(0, 15, 0);
                PollOptionsCard card = new PollOptionsCard(question, true, this._pollTracker, durationTimeSpan, new string[] { "--done--" }, new string[] { "cancel" });
                CardStackManager.CreateCardStack(card, Context.Message);
            }
        }
        #endregion
    }
}
