using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using LlectroBot.Configuration;

namespace LlectroBot.UserTracking
{
    public class UserTracker : IUserTracker
    {
        private readonly List<GuildTracker> _guilds = new List<GuildTracker>();
        private const string templateFilename = "usertracking.template.json";
        private const string dataFilename = "usertracking.json";

        private DiscordSocketClient DiscordClient { get; set; }

        public UserTracker(DiscordSocketClient discordSocketClient)
        {
            DiscordClient = discordSocketClient;

            _guilds = ReadJson().GetAwaiter().GetResult();

            RegisterForEvents();
        }

        

        private void UpdateUserState(IGuild guild, IUser user, string action)
        {
            var guildTracker = GetGuildTracker(guild);
            var userState = guildTracker.Users.FirstOrDefault(u => u.UserId == user.Id);
            if (userState == null)
            {
                userState = new UserState
                {
                    UserId = user.Id,
                    Username = user.Username
                };
                guildTracker.Users.Add(userState);
            }
            userState.LastActiveOn = DateTime.Now;
            userState.LastAction = action;
            SaveJson();
        }
        private void UpdateUser(IGuild guild, IUser oldUser, IUser newUser)
        {
            if (newUser.Username.ToLower().Trim() != oldUser.Username.ToLower().Trim())
            {
                GuildTracker guildTracker = GetGuildTracker(guild);
                // get the user state for the old user
                var oldUserState = guildTracker.Users.FirstOrDefault(u => u.UserId == oldUser.Id);
                // get the user state for the new user
                var newUserState = guildTracker.Users.FirstOrDefault(u => u.UserId == newUser.Id);

                // if the new user state is null, and the old user state exists
                if (newUserState == null && oldUserState != null)
                {
                    // update the old user state to have the new user information
                    oldUserState.UserId = newUser.Id;
                    oldUserState.Username = newUser.Username;
                    
                    // check if there is a transition where a name changed from this old username already
                    var ut = guildTracker.Transitions.FirstOrDefault(t => t.FromUserName.ToLower().Trim() == oldUser.Username.ToLower().Trim());
                    if (ut != null)
                    {
                        // if it does, the chain is no longer valid - so update it to be for the current transition
                        ut.UserId = newUser.Id;
                        ut.ToUserName = newUser.Username;
                    }
                    else
                    {
                        // if it does not, create a new user transition
                        ut = new UserTransition
                        {
                            UserId = newUser.Id,
                            FromUserName = oldUser.Username,
                            ToUserName = newUser.Username
                        };
                        guildTracker.Transitions.Add(ut);
                    }
                }
                SaveJson();
            }
        }

        private GuildTracker GetGuildTracker(IGuild guild)
        {
            var guildTracker = _guilds.FirstOrDefault(g => g.GuildId == guild.Id);

            if (guildTracker == null)
            {
                guildTracker = new GuildTracker
                {
                    GuildId = guild.Id,
                    GuildName = guild.Name
                };
                _guilds.Add(guildTracker);
                SaveJson();
            }

            return guildTracker;
        }

        #region Persistence
        private async void SaveJson()
        {
            using FileStream fs = File.Create(dataFilename);
            await JsonSerializer.SerializeAsync<List<GuildTracker>>(fs, _guilds, BotConfiguration.JsonSerializerOptions);
        }

        private async Task<List<GuildTracker>> ReadJson()
        {
            string useFile = templateFilename;
            if (File.Exists(dataFilename))
                useFile = dataFilename;

            if (File.Exists(useFile))
            {
                using FileStream fs = File.OpenRead(useFile);
                var guildTrackers = await JsonSerializer.DeserializeAsync<List<GuildTracker>>(fs, BotConfiguration.JsonSerializerOptions);
                return guildTrackers;
            }
            else
            {
                return new List<GuildTracker>();
            }
        }
        #endregion

        public UserState GetUserState(IGuild guild, IUser user)
        {
            var guildTracker = _guilds.FirstOrDefault(g => g.GuildId == guild.Id);
            var userState = guildTracker.Users.FirstOrDefault(u => u.UserId == user.Id);
            return userState;
        }
        #region DiscordSocketClient events
        private void RegisterForEvents()
        {
            DiscordClient.UserBanned += Client_UserBanned;
            DiscordClient.UserIsTyping += Client_UserIsTyping;
            DiscordClient.UserJoined += Client_UserJoined;
            DiscordClient.UserLeft += Client_UserLeft;
            DiscordClient.UserUpdated += Client_UserUpdated;
            DiscordClient.UserVoiceStateUpdated += Client_UserVoiceStateUpdated;

            DiscordClient.GuildMemberUpdated += Client_GuildMemberUpdated;
        }

        private Task Client_GuildMemberUpdated(SocketGuildUser arg1, SocketGuildUser arg2)
        {
            UpdateUser(arg1.Guild, arg1, arg2);
            return Task.CompletedTask;
        }

        private Task Client_UserBanned(SocketUser arg1, SocketGuild arg2)
        {
            UpdateUserState(arg2, arg1, "getting banned");
            return Task.CompletedTask;
        }

        private Task Client_UserIsTyping(SocketUser arg1, ISocketMessageChannel arg2)
        {
            foreach (var mutualGuild in arg1.MutualGuilds)
                foreach (var guildChannel in mutualGuild.Channels)
                    if (guildChannel.Id == arg2.Id)
                        UpdateUserState(mutualGuild, arg1, $"typing in {arg2.Name}");
            return Task.CompletedTask;
        }

        private Task Client_UserJoined(SocketGuildUser arg)
        {
            UpdateUserState(arg.Guild, arg, $"Joining guild {arg.Guild.Name}");
            return Task.CompletedTask;
        }

        private Task Client_UserLeft(SocketGuildUser arg)
        {
            UpdateUserState(arg.Guild, arg, "leaving the guild");
            return Task.CompletedTask;
        }

        private Task Client_UserUpdated(SocketUser arg1, SocketUser arg2)
        {
            foreach (var guild in arg1.MutualGuilds)
                UpdateUser(guild, arg1, arg2);
            return Task.CompletedTask;
        }

        private Task Client_UserVoiceStateUpdated(SocketUser arg1, SocketVoiceState arg2, SocketVoiceState arg3)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
