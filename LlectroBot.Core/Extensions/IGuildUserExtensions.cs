using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Discord;

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
    }
}
