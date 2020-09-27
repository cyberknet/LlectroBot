using System.Threading.Tasks;
using Discord;
using Discord.Cards;

namespace LlectroBot.Cards
{
    public class CountByCard : CardBase<int>
    {
        public bool Asked { get; set; } = false;
        public int Maximum { get; set; }
        public CountByCard(string message, bool isCancellable, int maximum, string[] cancelWords = null, string retryMessage = null)
            : base(message, isCancellable, cancelWords, retryMessage)
        {
            Maximum = maximum;
        }
        public override async Task<CardResult> MessageReceived(IMessage message, CardResult lastCardResult = CardResult.None)
        {
            CardResult result = CardResult.Continue;
            if (!Asked)
            {
                Asked = true;
                await ReplyAsync("What number should i increment by?");
            }
            else
            {
                if (!int.TryParse(message.Content, out int number))
                {
                    await ReplyAsync("That wasn't a number I understood. Please only use digits.");
                }
                else
                {
                    Result = number;
                    result = CardResult.CloseAndContinue;
                }
            }
            return result;

        }
    }
}
