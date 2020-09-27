using System;
using System.Threading.Tasks;
using Discord;
using Discord.Cards;
using Discord.Commands;
using LlectroBot.Cards;
using LlectroBot.Core.Configuration;
using LlectroBot.Core.Modules;

namespace LlectroBot.Modules.Utility
{
    public class UtilityCommands : CommandModuleBase
    {
        private ICardStackManager CardStackManager { get; set; }
        public UtilityCommands(IDiscordClient discordClient, IDiscordBot discordBot, IServiceProvider serviceProvider, IBotConfiguration botConfiguration, ICardStackManager cardStackManager)
            : base(discordClient, discordBot, serviceProvider, botConfiguration)
        {
            CardStackManager = cardStackManager;
        }

        [Command("ping")]
        [CommandDescription("Requests a Pong response from the bot. Mostly useful to make sure the bot is receiving messages and is not ignoring you.")]
        [CommandSyntax("{prefix}{command}")]
        [CommandUsage(CommandUsage.Both)]
        public async Task Ping()
        {
            await ReplyAsync("Pong!");
        }

        [Command("count")]
        [Alias("countby")]
        [CommandDescription("Starts a dialog with the bot to count to a number")]
        [CommandSyntax("{prefix}{command}")]
        [CommandUsage(CommandUsage.Both)]
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task Count()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            CountToCard card = new CountToCard("What number should I count to between 1 and 100?", false, null, "That wasn't a number I understood. Please only use digits. What number should I count to?");
            CardStackManager.CreateCardStack(card, Context.Message);
        }



    }
}
