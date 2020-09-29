using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Discord;
using LlectroBot.Roles;

namespace LlectroBot.Core.Configuration
{
    public class GuildConfiguration : IGuildConfiguration
    {
        public ulong GuildId { get; set; }
        public string GuildName { get; set; }
        public char CommandPrefix { get; set; }

        public List<LlectroBotRole> Roles { get; private set; } = new List<LlectroBotRole>();
        public bool AssignGuestRoleOnJoin { get; set; }
        public bool GreetingOnJoin { get; set; }
        public string Greeting { get; set; }
        public bool FarewellOnPart { get; set; }
        public string Farewell { get; set; }
        public IConfigurationChannel GreetingChannel { get; private set; }
        public IConfigurationChannel FarewellChannel { get; private set; }

        public IRole GetGuildRole(RoleLevel level)
        {
            return Roles
                 .FirstOrDefault(o => o.Level == level)
                ?.Value;
        }

        public void SetGreetingChannel(IChannel channel)
        {
            if (channel != null)
                GreetingChannel = new ConfigurationChannel { Id = channel.Id, Name = channel.Name };
            else
                GreetingChannel = null;
        }
        public void SetFarewellChannel(IChannel channel)
        {
            if (channel != null)
                FarewellChannel = new ConfigurationChannel { Id = channel.Id, Name = channel.Name };
            else
                FarewellChannel = null;
        }
    }
}
