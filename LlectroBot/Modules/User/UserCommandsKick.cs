using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using LlectroBot.Core.Modules;
using LlectroBot.Core.Preconditions;
using LlectroBot.Roles;

namespace LlectroBot.Modules.User
{
    public partial class UserCommands
    {
        [Command("kick")]
        [RequireRole(RoleLevel.SuperMember, RoleMatchType.GraterThanOrEqual)]
        [CommandDescription("Kicks a user from the channel.")]
        [CommandSyntax("{prefix}{command} [Username] [reason]")]
        [CommandUsage(CommandUsage.Both)]
        [RequireRole(RoleLevel.SuperMember, RoleMatchType.GraterThanOrEqual)]
        public async Task Kick(IUser user, string reason)
        {
            string errorReason = string.Empty;
            if (user == null)
            {
                errorReason = "user";
            }
            if (reason != null)
            {
                if (string.IsNullOrEmpty(errorReason))
                {
                    errorReason = "reason";
                }
                else
                {
                    errorReason += " and reason";
                }
            }
            if (!string.IsNullOrEmpty(errorReason))
            {
                await ReplyAsync($"You must provide a valid {errorReason} to use this command.");
            }
            else
            {
                await ReplyAsync($"You can't kick {user.Username} for {reason}!");
            }
        }
    }
}
