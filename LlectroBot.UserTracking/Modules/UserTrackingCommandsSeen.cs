using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using LlectroBot.Core.Help;
using LlectroBot.Core.Modules;

namespace LlectroBot.UserTracking.Modules
{
    public partial class UserTrackingCommands
    {
        #region seen
        [Command("seen")]
        [CommandDescription("Responds with the last time the bot saw a particular user active.")]
        [CommandSyntax("{prefix}{command} [User]")]
        [CommandUsage(CommandUsage.Channel)]
        [HelpModule(showHelpUnderModule: "UserCommands")]
        public async Task Seen(IUser user = null)
        {
            user ??= Context.User; //  = user ?? Context.User;
            if (Context.Guild == null) return;
            if (user == Context.User)
            {
                await ReplyAsync($"{Context.User.Username} was active just now.");
            }
            else
            {
                var userState = _userTracker.GetUserState(Context.Guild, user);
                if (userState == null)
                {
                    await ReplyAsync($"I've never seen {user.Username}");
                }
                else
                {
                    await ReplyAsync($"I last saw {user.Username} on {userState.LastActiveOn} {userState.LastAction}");
                }
            }
        }
        #endregion
    }
}
