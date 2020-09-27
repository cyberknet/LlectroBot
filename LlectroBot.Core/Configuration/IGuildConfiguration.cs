using System.Collections.Generic;
using Discord;
using LlectroBot.Roles;

namespace LlectroBot.Core.Configuration
{
    public interface IGuildConfiguration
    {
        ulong GuildId { get; set; }
        string GuildName { get; set; }
        char CommandPrefix { get; set; }
        IRole GetGuildRole(RoleLevel level);

        List<LlectroBotRole> Roles { get; set; }
        bool AssignGuestRoleOnJoin { get; set; }
    }
}
