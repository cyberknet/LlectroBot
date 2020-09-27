using System.Collections.Generic;
using Discord;

namespace LlectroBot.Core.Configuration
{
    public interface IBotConfiguration
    {
        ulong DebugChannelId { get; set; }
        List<GuildConfiguration> Guilds { get; set; }
        string AuthenticationToken { get; set; }
        char DirectMessageCommandPrefix { get; set; }
        ulong GlobalAdminId { get; set; }

        public void Save();
        void LoadGuildInformation(IDiscordClient client);
        void AddGuild(IGuild arg);
        void RemoveGuild(IGuild guild);
        public IGuildConfiguration GetGuildConfiguration(IGuild guild);
    }
}
