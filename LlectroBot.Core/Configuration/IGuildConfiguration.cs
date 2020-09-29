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
        bool AssignGuestRoleOnJoin { get; set; }
        List<LlectroBotRole> Roles { get; }
        bool GreetingOnJoin { get; set; }
        string Greeting { get; set; }
        bool FarewellOnPart { get; set; }
        string Farewell { get; set; }
        IConfigurationChannel GreetingChannel { get; }
        IConfigurationChannel FarewellChannel { get; }


        IRole GetGuildRole(RoleLevel level);
        void SetGreetingChannel(IChannel channel);
        void SetFarewellChannel(IChannel channel);
    }
}
