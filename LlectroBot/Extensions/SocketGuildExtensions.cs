using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace Discord.WebSocket
{
    public static class SocketGuildExtensions
    {
        public static async Task<IMessageChannel> GetWelcomeMessageChannel(this SocketGuild guild)
        {
            var user = guild.CurrentUser;

            //First, test the default channel.
            if (guild.DefaultChannel != null && user.GetPermissions(guild.DefaultChannel).SendMessages)
                return guild.DefaultChannel;

            await System.Console.Out.WriteLineAsync($"Guild {guild.Name} - Unable to send to default channel.");

            //Next, Just loop through channels, until we find a writeable channel.
            var ch = guild.Channels.OfType<SocketTextChannel>().FirstOrDefault(o => user.GetPermissions(o).SendMessages == true);
            if (ch != null)
                return ch;

            await System.Console.Out.WriteLineAsync($"Guild {guild.Name} - Unable to send to ANY channel.");

            //Try to make a DM with the guild's owner.
            try
            {
                var dm = await guild.Owner.GetOrCreateDMChannelAsync();
                await dm.SendMessageAsync($"LLectroBot was unable to send this message to {guild.Name} because it did not have SEND_MESSAGES permission for any channels.\r" +
                    $"\nPlease grant LlectroBot the permissions to send messages in guild channels, and type '!setup' to walk through a guided setup process.");
                return dm;
            }
            catch
            {
                await System.Console.Out.WriteLineAsync($"Guild {guild.Name} - Unable to DM Owner");
                //unable to make a DM...
            }

            return null;
        }
    }
}
