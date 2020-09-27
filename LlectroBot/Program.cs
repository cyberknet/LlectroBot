using System.Threading.Tasks;
using LlectroBot.Core.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LlectroBot
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program().MainAsync().GetAwaiter().GetResult();
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task MainAsync()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            DiscordBot bot = new DiscordBot(services);
            bot.RunBotAsync().GetAwaiter().GetResult();
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IBotConfiguration>((sp) => BotConfiguration.FromConfig().GetAwaiter().GetResult());
        }


    }
}
