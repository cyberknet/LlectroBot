using System.Collections.Generic;
using System.IO.Enumeration;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using System.Linq;

namespace LlectroBot.Configuration
{
    public class BotConfiguration : IBotConfiguration
    {
        public List<GuildConfiguration> Guilds { get; set; } = new List<GuildConfiguration>();
        public char DirectMessageCommandPrefix { get; set; }
        public ulong DebugChannelId { get; set; }
        public string AuthenticationToken { get; set; }
        public ulong GlobalAdminId { get; set; }

        private static readonly string templateFilename = "appsettings.template.json";
        private static readonly string dataFilename = "appsettings.json";
        public static JsonSerializerOptions JsonSerializerOptions { get; private set; } = new JsonSerializerOptions { WriteIndented = true, IgnoreReadOnlyProperties = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        public async void LoadGuildInformation(IDiscordClient client)
        {
            var guilds = await client.GetGuildsAsync();
            foreach (var guildConfig in Guilds)
            {
                var guild = guilds.FirstOrDefault(g => g.Id == guildConfig.GuildId);
                if (guild != null)
                {
                    foreach (var role in guildConfig.Roles)
                    {
                        role.GuildConfiguration = guildConfig;
                        role.Value = guild.GetRole(role.RoleId);
                    }
                }
            }
        }

        public IGuildConfiguration GetGuildConfiguration(IGuild guild)
        {
            if (guild == null) return null;
            return GetGuildConfigurationInternal(guild);
        }
        private GuildConfiguration GetGuildConfigurationInternal(IGuild guild)
        {
            return Guilds.FirstOrDefault(gc => gc.GuildId == guild?.Id);
        }

        public static async Task<BotConfiguration> FromConfig()
        {
            return await ReadJson();
        }

        private async void SaveJson()
        {
            using (FileStream fs = File.Create(dataFilename))
            {
                //var json = JsonSerializer.Serialize(this, JsonSerializerOptions);
                //byte[] info = System.Text.Encoding.UTF8.GetBytes(json);
                //fs.Write(info);
                await JsonSerializer.SerializeAsync<BotConfiguration>(fs, this, JsonSerializerOptions);
            }
        }

        private static async Task<BotConfiguration> ReadJson()
        {
            BotConfiguration config = null;
            string useFile = templateFilename;
            if (File.Exists(dataFilename))
                useFile = dataFilename;

            if (File.Exists(useFile))
            {
                using (FileStream fs = File.OpenRead(useFile))
                {
                    try
                    {
                        config = await JsonSerializer.DeserializeAsync<BotConfiguration>(fs, JsonSerializerOptions);
                    }
                    catch (System.Exception ex)
                    {
                        config = new BotConfiguration();
                    }
                }
            }
            else
            {
                config = new BotConfiguration();
            }
            return config;
        }

        public void Save()
        {
            SaveJson();
        }

        public void AddGuild(IGuild guild)
        {
            Guilds.Add(new GuildConfiguration()
            {
                CommandPrefix = this.DirectMessageCommandPrefix,
                GuildId = guild.Id,
                GuildName = guild.Name
            });
            Save();
        }

        public void RemoveGuild(IGuild guild)
        {
            var guildConfig = GetGuildConfigurationInternal(guild);
            Guilds.Remove(guildConfig);
            Save();
        }
    }
}
