using System.Collections.Generic;
using System.Linq;
using Discord;
using LlectroBot.Roles;

namespace LlectroBot.Configuration
{
    public class GuildConfiguration : IGuildConfiguration
    {
        public ulong GuildId { get; set; }
        public string GuildName { get; set; }
        public char CommandPrefix { get; set; }

        public List<LlectroBotRole> Roles { get; set; } = new List<LlectroBotRole>();
        public bool AssignGuestRoleOnJoin { get; set; }

        public IRole GetGuildRole(RoleLevel level)
        {
            return Roles
                 .FirstOrDefault(o => o.Level == level)
                ?.Value;
        }
    }
}
