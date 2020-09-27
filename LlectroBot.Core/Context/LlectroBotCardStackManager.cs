using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Discord;
using Discord.Cards;
using Discord.WebSocket;
using LlectroBot.Core.Configuration;
using LlectroBot.Core.Services;

namespace LlectroBot.Core.Cards
{
    [RegisterService(typeof(ICardStackManager))]
    public class LlectroBotCardStackManager : CardStackManager<LlectroBotCardContext>
    {
        public static string TemplateFilename { get; set; } = "usertracker.template.json";
        public static string DataFilename { get; set; } = "usertracker.json";

        public IBotConfiguration BotConfiguration { get; set; }
        public LlectroBotCardStackManager(IDiscordClient discordClient, IBotConfiguration botConfiguration) : base(discordClient)
        {
            BotConfiguration = botConfiguration;
        }

        protected override ICardContext GetCardContext(IUserMessage message, SocketTextChannel tch)
        {
            var guildConfiguration = BotConfiguration.GetGuildConfiguration(tch.Guild);

            return new LlectroBotCardContext(DiscordClient, message.Author, message, BotConfiguration, guildConfiguration)
            {
                Guild = tch?.Guild,
                Channel = message.Channel,
            };
        }

        protected override async void OnSave()
        {
            using FileStream fs = File.Create(DataFilename);
            await JsonSerializer.SerializeAsync<List<ICardStack>>(fs, Stacks, Core.Configuration.BotConfiguration.JsonSerializerOptions);
        }

        protected override async Task<List<ICardStack>> OnRead()
        {
            string useFile = TemplateFilename;
            if (File.Exists(DataFilename))
                useFile = DataFilename;

            if (File.Exists(useFile))
            {
                using FileStream fs = File.OpenRead(useFile);
                var stacks = await JsonSerializer.DeserializeAsync<List<ICardStack>>(fs, Core.Configuration.BotConfiguration.JsonSerializerOptions);
                return stacks;
            }
            else
            {
                return new List<ICardStack>();
            }
        }
    }
}
