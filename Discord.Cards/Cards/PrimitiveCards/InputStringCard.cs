using System.Threading.Tasks;

namespace Discord.Cards.PrimitiveCards
{
    public class InputStringCard : CardBase<string>
    {
        public bool Asked { get; set; }
        public int MaximumLength { get; set; }
        public InputStringCard(string message, bool isCancellable, int maximumLength = 0, string[] cancelWords = null, string retryMessage = null)
             : base(message, isCancellable, cancelWords, retryMessage)
        {
            MaximumLength = maximumLength;
        }
        public override async Task<CardResult> MessageReceived(IMessage message, CardResult lastCardResult = CardResult.None)
        {
            var result = CardResult.Continue;
            if (!Asked)
            {
                Asked = true;
                await ReplyAsync(Message);
            }
            else
            {
                if (message.Content.Trim().Length > MaximumLength && MaximumLength > 0)
                {
                    await ReplyAsync($"Sorry, that was too many characters. I need at most {MaximumLength} character{(MaximumLength > 1 ? "s" : string.Empty)}. {RetryMessage}");
                }
                else
                {
                    Result = message.Content.Trim();
                    result = CardResult.CloseAndContinue;
                }
            }
            return result;
        }
    }
}
