using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace LlectroBot
{
    public partial class DiscordBot
    {
        private void RegisterEvents()
        {
            // general events
            Client.Log += Client_Log;
            Client.VoiceServerUpdated += Client_VoiceServerUpdated;
            Client.Connected += Client_Connected;



            // bot events
            Client.JoinedGuild += Client_JoinedGuild;
            Client.LatencyUpdated += Client_LatencyUpdated;
            Client.LeftGuild += Client_LeftGuild;
            Client.LoggedIn += Client_LoggedIn;
            Client.LoggedOut += Client_LoggedOut;
            Client.Ready += Client_Ready;
            Client.CurrentUserUpdated += Client_CurrentUserUpdated;

            // message events
            Client.MessageDeleted += Client_MessageDeleted;
            Client.MessageReceived += Client_MessageReceived;
            Client.MessagesBulkDeleted += Client_MessagesBulkDeleted;
            Client.MessageUpdated += Client_MessageUpdated;
            Client.ReactionAdded += Client_ReactionAdded;
            Client.ReactionRemoved += Client_ReactionRemoved;
            Client.ReactionsCleared += Client_ReactionsCleared;
            Client.RecipientAdded += Client_RecipientAdded;
            Client.RecipientRemoved += Client_RecipientRemoved;

            // guild events
            Client.ChannelCreated += Client_ChannelCreated;
            Client.ChannelDestroyed += Client_ChannelDestroyed;
            Client.ChannelUpdated += Client_ChannelUpdated;
            Client.GuildAvailable += Client_GuildAvailable;
            Client.GuildMembersDownloaded += Client_GuildMembersDownloaded;
            Client.GuildMemberUpdated += Client_GuildMemberUpdated;
            Client.GuildUnavailable += Client_GuildUnavailable;
            Client.GuildUpdated += Client_GuildUpdated;
            Client.RoleCreated += Client_RoleCreated;
            Client.RoleDeleted += Client_RoleDeleted;
            Client.RoleUpdated += Client_RoleUpdated;


            // user events
            Client.UserBanned += Client_UserBanned;
            Client.UserIsTyping += Client_UserIsTyping;
            Client.UserJoined += Client_UserJoined;
            Client.UserLeft += Client_UserLeft;
            Client.UserUnbanned += Client_UserUnbanned;
            Client.UserUpdated += Client_UserUpdated;
            Client.UserVoiceStateUpdated += Client_UserVoiceStateUpdated;

        }

        private Task Client_Log(LogMessage arg)
        {
            Console.WriteLine(arg);
            return Task.CompletedTask;
        }

        private Task Client_VoiceServerUpdated(SocketVoiceServer arg)
        {
            return Task.CompletedTask;
        }

        private Task Client_RoleUpdated(SocketRole arg1, SocketRole arg2)
        {
            return Task.CompletedTask;
        }

        private Task Client_RoleDeleted(SocketRole arg)
        {
            return Task.CompletedTask;
        }

        private Task Client_RoleCreated(SocketRole arg)
        {
            return Task.CompletedTask;
        }

        private Task Client_RecipientRemoved(SocketGroupUser arg)
        {
            return Task.CompletedTask;
        }

        private Task Client_RecipientAdded(SocketGroupUser arg)
        {
            return Task.CompletedTask;
        }

        private Task Client_Ready()
        {
            return Task.CompletedTask;
        }

        private Task Client_ReactionsCleared(Cacheable<IUserMessage, ulong> arg1, ISocketMessageChannel arg2)
        {
            return Task.CompletedTask;
        }

        private Task Client_ReactionRemoved(Cacheable<IUserMessage, ulong> arg1, ISocketMessageChannel arg2, SocketReaction arg3)
        {
            return Task.CompletedTask;
        }

        private Task Client_ReactionAdded(Cacheable<IUserMessage, ulong> arg1, ISocketMessageChannel arg2, SocketReaction arg3)
        {
            return Task.CompletedTask;
        }

        private Task Client_MessageUpdated(Cacheable<IMessage, ulong> arg1, SocketMessage arg2, ISocketMessageChannel arg3)
        {
            return Task.CompletedTask;
        }

        private Task Client_MessagesBulkDeleted(System.Collections.Generic.IReadOnlyCollection<Cacheable<IMessage, ulong>> arg1, ISocketMessageChannel arg2)
        {
            return Task.CompletedTask;
        }

        private Task Client_MessageDeleted(Cacheable<IMessage, ulong> arg1, ISocketMessageChannel arg2)
        {
            return Task.CompletedTask;
        }


        private Task Client_GuildUpdated(SocketGuild arg1, SocketGuild arg2)
        {
            return Task.CompletedTask;
        }

        private Task Client_GuildUnavailable(SocketGuild arg)
        {
            return Task.CompletedTask;
        }

        private Task Client_GuildMemberUpdated(SocketGuildUser arg1, SocketGuildUser arg2)
        {
            return Task.CompletedTask;
        }

        private Task Client_GuildMembersDownloaded(SocketGuild arg)
        {
            return Task.CompletedTask;
        }

        private Task Client_GuildAvailable(SocketGuild arg)
        {
            return Task.CompletedTask;
        }

        private Task Client_ChannelUpdated(SocketChannel arg1, SocketChannel arg2)
        {
            return Task.CompletedTask;
        }

        private Task Client_ChannelDestroyed(SocketChannel arg)
        {
            return Task.CompletedTask;
        }

        private Task Client_ChannelCreated(SocketChannel arg)
        {
            return Task.CompletedTask;
        }


    }
}
