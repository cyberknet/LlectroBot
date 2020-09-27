using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Cards;

namespace LlectroBot.Cards
{
    public class CountToCard : CardBase<int>
    {
        public bool Asked { get; private set; }
        public CountByCard CountByCard { get; set; }
        public CountToCard(string message, bool isCancellable, string[] cancelWords, string retryMessage)
            : base(message, isCancellable, cancelWords, retryMessage)
        {

        }
        public override async Task<CardResult> MessageReceived(IMessage message, CardResult lastCardResult = CardResult.None)
        {
            CardResult result = CardResult.Continue;
            if (!Asked)
            {
                Asked = true;
                await ReplyAsync(Message);
            }
            else if (CountByCard != null)
            {
                StringBuilder builder = new StringBuilder();

                builder.Append("0, ");
                int i = 0;
                while (i < Result)
                {
                    i += CountByCard.Result;
                    builder.Append($"{i}, ");
                }
                await ReplyAsync($"OK, counting to {Result} by {CountByCard.Result}: {builder.ToString()[0..^2]}"); // builder.ToString().Substring(0, builder.ToString().Length - 2)

                return CardResult.CloseAndContinue;

            }
            else
            {
                if (!int.TryParse(message.Content, out int number))
                {
                    await ReplyAsync(RetryMessage);
                }
                else
                {
                    Result = number;
                    CountByCard = new CountByCard("What number should I count by?", false, number);
                    await CardStack.ShowCard(CountByCard, message, Context);
                    result = CardResult.Continue;
                }
            }
            return result;
        }
    }
}
