using System.Threading.Tasks;
using Discord.WebSocket;

namespace LlectroBot
{
    public partial class DiscordBot
    {
        private Task Client_UserVoiceStateUpdated(SocketUser arg1, SocketVoiceState arg2, SocketVoiceState arg3)
        {
            return Task.CompletedTask;
        }

        private Task Client_UserUpdated(SocketUser arg1, SocketUser arg2)
        {
            return Task.CompletedTask;
        }

        private Task Client_UserUnbanned(SocketUser arg1, SocketGuild arg2)
        {
            return Task.CompletedTask;
        }

        private Task Client_UserLeft(SocketGuildUser arg)
        {
            return Task.CompletedTask;
        }

        private async Task Client_UserJoined(SocketGuildUser arg)
        {
            var guildConfiguration = BotConfiguration.GetGuildConfiguration(arg.Guild);
            if (guildConfiguration.AssignGuestRoleOnJoin)
            {
                var role = guildConfiguration.GetGuildRole(Roles.RoleLevel.Member);
                if (role != null)
                    await arg.AddRoleAsync(role);
            }
        }

        private Task Client_UserIsTyping(SocketUser arg1, ISocketMessageChannel arg2)
        {
            return Task.CompletedTask;
        }

        private Task Client_UserBanned(SocketUser arg1, SocketGuild arg2)
        {
            return Task.CompletedTask;
        }
    }
}
