using System.Runtime.CompilerServices;

namespace Discord.WebSocket
{
    public static class SocketTextChannelExtensions
    {
        public static bool HasPermission(this SocketTextChannel channel, SocketGuildUser user, ChannelPermission permission)
        {
            return user.GetPermissions(channel).ToList().Contains(permission);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasPermission(this SocketTextChannel context, ChannelPermission permission)
        {
            return context.Guild.CurrentUser.GetPermissions(context).ToList().Contains(permission);
        }
    }
}
