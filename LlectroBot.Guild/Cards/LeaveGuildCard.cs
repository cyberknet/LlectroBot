using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Cards;
using Discord.Cards.Cards.PrimitiveCards;
using Discord.Cards.PrimitiveCards;
using LlectroBot.Core.Cards;
using LlectroBot.Roles;
namespace LlectroBot.Guild.Cards
{
    public class LeaveGuildCard : CardBase<bool>
    {
        private LlectroBotCardContext LlectroBotContext => Context as LlectroBotCardContext;
        private InputBooleanCard ConfirmLeave;

        public LeaveGuildCard(string message, bool isCancellable, string[] cancelWords = null, string retryMessage = null)
            : base(message, isCancellable, cancelWords, retryMessage)
        {

        }

        public override async Task<CardResult> MessageReceived(IMessage message, CardResult lastCardResult = CardResult.None)
        {
            if (ConfirmLeave == null)
            {
                ConfirmLeave = new InputBooleanCard("Are you sure you want the bot to leave the guild? All guild configuration will be lost.",
                    true,
                    new string[] { "cancel" },
                    "Are you sure?",
                    new string[] { "yes" },
                    new string[] { "no" });
                await this.CardStack.ShowCard(ConfirmLeave, message, this.LlectroBotContext);
                return CardResult.Continue;
            }
            else
            {
                // if they didn't cancel out of the card, and they answered true (yes)
                if (lastCardResult != CardResult.Cancel && ConfirmLeave.Result)
                {
                    // adios, buck-o. Time to boogie!
                    var builder = new EmbedBuilder()
                        .WithTitle("So long, and thanks for all the fish! 👋")
                        .WithDescription(
                        "They asked me here and gave me trust.\n" +
                        "Then go they said, so go I must.\n" +
                        "But if by chance you didn't think,\n" +
                        "Just go ahead, and click this link!\n"
                        )
                        .WithUrl($"https://discordapp.com/oauth2/authorize?client_id={Context.DiscordClient.CurrentUser.Id}&scope=bot&permissions=2146958847");
                    var botUser = await Context.Guild.GetCurrentUserAsync();
                    if (botUser.HasPermission(Context.Channel as IGuildChannel, ChannelPermission.SendMessages))
                    {
                        await ReplyAsync(embed: builder.Build());
                    }
                    else
                    {
                        ulong guildOwnerId = Context.Guild.OwnerId;
                        var channel = Context.DiscordClient.GetDMChannelAsync(guildOwnerId) as ITextChannel;
                        await channel.SendMessageAsync(embed: builder.Build());
                    }
                }
                return CardResult.CloseAndContinue;
            }
        }
    }
}
