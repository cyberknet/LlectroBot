using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace LlectroBot
{
    public partial class DiscordBot
    {
        private Task Client_LoggedOut()
        {
            return Task.CompletedTask;
        }

        private Task Client_LoggedIn()
        {
            return Task.CompletedTask;
        }

        private Task Client_LeftGuild(SocketGuild arg)
        {
            BotConfiguration.RemoveGuild(arg);
            return Task.CompletedTask;
        }

        private Task Client_LatencyUpdated(int arg1, int arg2)
        {
            return Task.CompletedTask;
        }

        private async Task Client_JoinedGuild(SocketGuild arg)
        {
            BotConfiguration.AddGuild(arg);
            IMessageChannel channel = await arg.GetWelcomeMessageChannel();
            if (channel != null)
            {
                var eb = new EmbedBuilder()
                    .WithTitle("LlectroBot has arrived!")
                    .WithColor(Color.Green)
                    .WithDescription("Here I am to save the day. You're almost ready - but there is one more important step. I'm a fully functional bot, but I need to know a bit about your guild first. Have a server admin type !setup, and I'll guide them through the easiest bot setup they've ever done, guaranteed!")
                    .AddField("Help", $"I have a complete help catalog built in. To access it, type !help in channel or via DM.")
                    .AddField("No Really, HELP!", $"Contact <@{BotConfiguration.GlobalAdminId}> if you get stuck or find something seriously broken.")
                    .WithUrl("http://github.com/cyberknet/LlectroBot");
                await channel.SendMessageAsync(embed: eb.Build());
            }
            else
            {
                // TODO: log that the embed was unable to be sent.
            }
        }

        private Task Client_CurrentUserUpdated(SocketSelfUser arg1, SocketSelfUser arg2)
        {
            return Task.CompletedTask;
        }

        private Task Client_Connected()
        {
            BotConfiguration.LoadGuildInformation(Client);
            return Task.CompletedTask;
        }
    }
}
