using System.Text.Json.Serialization;
using Discord;
using LlectroBot.Core.Configuration;

namespace LlectroBot.Roles
{
    public class LlectroBotRole
    {
        public string Name { get; set; }
        [JsonIgnore]
        public IRole Value { get; set; }
        public ulong RoleId { get; set; }
        public RoleLevel Level { get; set; }

        [JsonIgnore]
        public virtual IGuildConfiguration GuildConfiguration { get; set; }

        public LlectroBotRole() { }
        public LlectroBotRole(RoleLevel level, IRole role)
        {
            Level = level;
            RoleId = role.Id;
            Name = role.Name;
            Value = role;
        }
    }
}
