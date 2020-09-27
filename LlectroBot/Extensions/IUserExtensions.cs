using System;
using System.Collections.Generic;
using System.Linq;
using LlectroBot.Configuration;
using LlectroBot.Roles;

namespace Discord.WebSocket
{
    public static class IUserExtensions
    {
        public static RoleLevel GetRole(this IGuildUser user, IBotConfiguration botConfiguration, IGuildConfiguration guildConfiguration)
        {
            if (user.Id == botConfiguration.GlobalAdminId)
            {
                return RoleLevel.GlobalAdministrator;
            }

            if (user.GuildPermissions.Administrator)
            {
                return RoleLevel.GuildAdministrator;
            }

            // get a list of guild roles
            var roles = guildConfiguration.Roles
                .OrderByDescending(r => (int)r.Level);
            foreach (var role in roles)
            {
                ulong guildRoleId = role.RoleId;
                if (user.RoleIds.Any(roleId => roleId == guildRoleId))
                {
                    return role.Level;
                }
            }
            return RoleLevel.None;
        }
    }
}
