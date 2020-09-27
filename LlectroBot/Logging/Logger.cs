using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using LlectroBot.Core.Configuration;
using LlectroBot.Core.Preconditions;

namespace LlectroBot.Logging
{
    public class Logger : ILogger
    {
        private DiscordSocketClient Client { get; set; }
        private IBotConfiguration BotConfiguration { get; set; }
        private ITextChannel _loggingChannel;
        private ITextChannel LoggingChannel
        {
            get { if (_loggingChannel == null) { _loggingChannel = Client.GetChannel(BotConfiguration.DebugChannelId) as SocketTextChannel; } return _loggingChannel; }
            set => _loggingChannel = value;
        }
        public Logger(IBotConfiguration configuration, IDiscordClient client)
        {
            BotConfiguration = configuration;
            Client = client as DiscordSocketClient;
        }

        public async Task ChatMessage(IMessage message, IResult result, IGuild guild = null)
        {
            var title = $"Direct Message from {message.Author.Username}";
            if (guild != null)
            {
                title = $"Channel {message.Channel.Name} in guild {guild.Name}";
            }

            EmbedBuilder builder = new EmbedBuilder()
                .WithTitle(title)
                .AddField("From", message.Author.Username, true);

            if (message.Channel is ITextChannel)
            {
                builder.AddField("Channel", message.Channel.Name, true);
            }

            if (message.Channel is IDMChannel)
            {
                builder.AddField("Channel", "Direct Message", true);
            }

            if (guild != null)
            {
                builder.AddField("Guid", guild.Name, true);
            }

            builder.AddField("Success", result.IsSuccess, true);

            // if access denied, document
            if (result.Error == CommandError.UnmetPrecondition && result is AccessDeniedPreconditionResult accessDenied)
            {
                builder.AddField("Error", "Access Denied")
                    .AddField("User Role", accessDenied.UserRole.ToString(), true)
                    .AddField("Match Type", accessDenied.MatchType.ToString(), true)
                    .AddField("Required Role", accessDenied.RequiredRole.ToString(), true);
            }

            // if parse error, document
            else if (result.Error == CommandError.ParseFailed && result is ParseResult tr)
            {
                builder.AddField("Error", "Parse Failed");
                if (!string.IsNullOrEmpty(tr.ErrorReason))
                {
                    builder.AddField("Error Message", tr.ErrorReason, true);
                }

                if (tr.ParamValues != null)
                {
                    foreach (var pv in tr.ParamValues.Where(o => !o.IsSuccess))
                    {
                        foreach (var val in pv.Values)
                        {
                            builder.AddField("Value", val.Value, true)
                                .AddField("Score", val.Score, true);
                        }
                    }
                }
            }

            // all other failures, make a best effort
            else if (!result.IsSuccess)
            {
                builder.AddField("Failure Reason", result.ErrorReason);
            }

            if (message.Embeds.Count == 0)
            {
                builder.AddField("Message", message.Content);
            }
            else
            {
                builder.AddField("Message", $"Embedded: {message.Embeds.First().Title}");
            }

            await SendToChannel(LoggingChannel, builder.Build());
        }

        public async Task Error(IGuild guild, Exception ex, [CallerMemberName] string Method = "")
        {
            string Message = $"{guild?.Name} - {Method} - {ex.Message}";
            await WriteToConsole(Message);
        }

        public async Task SendToChannel(ITextChannel ch, Embed embed, string Message = "")
        {
            if (ch != null)
            {
                await ch.SendMessageAsync(Message, embed: embed);
            }
        }

        public async Task WriteToConsole(string Message)
        {
            if (System.Environment.UserInteractive)
            {
                await Console.Out.WriteLineAsync(Message);
            }
        }
    }
}
