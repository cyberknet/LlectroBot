using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Discord;
using LlectroBot.Core.Configuration;
using LlectroBot.Roles;

namespace Discord
{
    public static class IGuildUserExtensions
    {
        public static int GetHierarchy(this IGuildUser user)
        {
            if (user.Guild.OwnerId == user.Id)
                return int.MaxValue;

            var roleIds = user.RoleIds.ToArray();
            int maxPos = 0;
            for (int i = 0; i < roleIds.Length; i++)
            {
                var role = user.Guild.GetRole(roleIds[i]);
                if (role != null && role.Position > maxPos)
                    maxPos = role.Position;
            }
            return maxPos;
        }

        public static bool HasPermission(this IGuildUser user, IGuildChannel channel, ChannelPermission permission)
        {
            if (user is null)
                return false;
            if (channel is null)
                return false;
            return user.GetPermissions(channel).ToList().Contains(permission);
        }

        public static RoleLevel GetRole(this IGuildUser user, IBotConfiguration botConfiguration, IGuildConfiguration guildConfiguration)
        {
            if (user is null)
                return RoleLevel.None;
            if (botConfiguration is null)
                return RoleLevel.None;
            if (guildConfiguration is null)
                return RoleLevel.None;

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
