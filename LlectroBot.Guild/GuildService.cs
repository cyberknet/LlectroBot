using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using LlectroBot.Core.Configuration;
using LlectroBot.Core.Services;

namespace LlectroBot.Guild
{
    [RegisterServiceInterface(typeof(IGuildService))]
    public class GuildService : IGuildService
    {
        private readonly DiscordSocketClient _discordClient;
        private readonly IBotConfiguration _botConfiguration;
        public GuildService(DiscordSocketClient discordSocketClient, IBotConfiguration botConfiguration)
        {
            _discordClient = discordSocketClient;
            _botConfiguration = botConfiguration;
            RegisterForEvents();
        }

        private void RegisterForEvents()
        {
            _discordClient.UserJoined += Client_UserJoined;
            _discordClient.UserLeft += Client_UserLeft;
        }

        private async Task Client_UserLeft(SocketGuildUser user)
        {
            var guild = user.Guild;
            var guildConfig = _botConfiguration.GetGuildConfiguration(guild);

            // greet the user if configured
            if (guildConfig.FarewellOnPart && guildConfig.FarewellChannel != null)
            {
                string message = guildConfig.Farewell.Replace("{user}", user.Mention);
                if (_discordClient.GetChannel(guildConfig.FarewellChannel.Id) is IMessageChannel channel)
                {
                    await channel.SendMessageAsync(message);
                }
            }
        }

        private async Task Client_UserJoined(SocketGuildUser user)
        {
            var guild = user.Guild;
            var guildConfig = _botConfiguration.GetGuildConfiguration(guild);
            if (guildConfig != null)
            {
                // assign the guest role to the user if configured
                if (guildConfig.AssignGuestRoleOnJoin)
                {
                    var guestRole = guildConfig.GetGuildRole(Roles.RoleLevel.Guest);
                    if (guestRole != null)
                    {
                        await user.AddRoleAsync(guestRole);
                    }
                }

                // greet the user if configured
                if (guildConfig.GreetingOnJoin && guildConfig.GreetingChannel != null)
                {
                    string message = guildConfig.Greeting.Replace("{user}", user.Mention);
                    if (_discordClient.GetChannel(guildConfig.GreetingChannel.Id) is IMessageChannel channel)
                    {
                        await channel.SendMessageAsync(message);
                    }
                }
            }
            
        }
    }
}
