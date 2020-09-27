using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using LlectroBot.Core.Context;
using LlectroBot.Core.Preconditions;

namespace LlectroBot
{
    public partial class DiscordBot
    {
        private Task Client_MessageReceived(SocketMessage arg)
        {
            // don't process the command if it was a system message
            if (!(arg is SocketUserMessage message))
            {
                return Task.CompletedTask;
            }
            // don't process messages from other bots
            if (message.Author.IsBot)
            {
                return Task.CompletedTask;
            }
            // don't process messages from the current bot
            if (message.Author.Id == Client.CurrentUser.Id)
            {
                return Task.CompletedTask;
            }

            var t = Task.Run(() => ProcessMessage(message));
            return Task.CompletedTask;
        }

        private async Task ProcessMessage(SocketUserMessage message)
        {
            //var context = new SocketCommandContext(_client, message);

            try
            {
                // check if there is a card on the stack that can handle the 
                // message, and if so - send it there instead
                if (CardStackManager.RouteMessage(message))
                {
                    return;
                }
                else if (message.Channel is SocketTextChannel tch)
                {
                    var guildUser = message.Author as IGuildUser;
                    var guildConfig = BotConfiguration.GetGuildConfiguration(tch.Guild);
                    // if configuration for this guild was not found, fuggedaboudit
                    if (guildConfig == null)
                    {
                        return;
                    }

                    int argPos = 0;
                    // check if the message has the command prefix
                    var hasCommandPrefix = message.HasCharPrefix(guildConfig.CommandPrefix, ref argPos);
                    // check if the message starts with the mention prefix
                    var hasMentionPrefix = message.HasMentionPrefix(Client.CurrentUser, ref argPos);

                    // don't process messages that have the mention or command prefix
                    if (!hasCommandPrefix && !hasMentionPrefix)
                    {
                        return;
                    }

                    string messageText = message.Content
                        [argPos..] // .Substring(argPos, message.Content.Length - argPos)
                        .Trim();

                    var context = new DiscordCommandContext(Client, message, guildConfig, this, guildUser);

                    var result = await Commands.ExecuteAsync(
                        context,
                        messageText,
                        ServiceProvider,
                        MultiMatchHandling.Best);
                    // if the command did not succeed, and we can send messages to the channel
                    if (!result.IsSuccess && tch.HasPermission(ChannelPermission.SendMessages))
                    {
                        switch (result.Error.Value)
                        {
                            case CommandError.UnknownCommand:
                            case CommandError.ParseFailed:
                            case CommandError.BadArgCount:
                            case CommandError.ObjectNotFound:
                            case CommandError.MultipleMatches:
                                break;
                            case CommandError.UnmetPrecondition when result is AccessDeniedPreconditionResult access:
                                await tch.SendMessageAsync($"You do not have access to this command. You require the {access.RequiredRole} role.");
                                break;
                            case CommandError.UnmetPrecondition when result is PreconditionResult res:
                                await tch.SendMessageAsync(res.ErrorReason);
                                break;
                            case CommandError.Exception:
                                await tch.SendMessageAsync("An error has occured. The details will be reported for remediation.");
                                break;
                            case CommandError.Unsuccessful:
                                break;
                        }
                    }
                    await Logger.ChatMessage(message, result, tch.Guild);
                }

                else if (message.Channel is SocketDMChannel dm)
                {
                    int argPos = 0;
                    // check if the message has the command prefix
                    var hasCommandPrefix = message.HasCharPrefix(BotConfiguration.DirectMessageCommandPrefix, ref argPos);
                    // check if the message starts with the mention prefix
                    var hasMentionPrefix = message.HasMentionPrefix(Client.CurrentUser, ref argPos);

                    var context = new DiscordCommandContext(Client, message, null, this, null);

                    string messageText = message.Content
                        [argPos..] // .Substring(argPos, message.Content.Length - argPos)
                        .Trim();

                    var result = await Commands.ExecuteAsync(context, messageText, ServiceProvider, MultiMatchHandling.Best);
                    await Logger.ChatMessage(message, result);
                }

            }
            catch (Exception ex)
            {
                await Logger.Error(null, ex);
            }
        }
    }
}
