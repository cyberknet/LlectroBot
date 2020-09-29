using System;
using System.Linq;
using System.Threading.Tasks;
using Discord.Cards.Extensions;

namespace Discord.Cards.Cards.PrimitiveCards
{
    public class InputBooleanCard : CardBase<bool>
    {
        public bool Asked { get; set; }
        public string[] TrueOptions { get; set; }
        public string[] FalseOptions { get; set; }
        public InputBooleanCard(string message, bool isCancellable, string[] cancelWords = null, string retryMessage = null, string[] trueOptions = null, string[] falseOptions = null)
             : base(message, isCancellable, cancelWords, retryMessage)
        {
            if (trueOptions == null || trueOptions.Count() == 0)
                trueOptions = new string[] { "true", "yes", "1" };
            if (falseOptions == null || falseOptions.Count() == 0)
                falseOptions = new string[] { "false", "no", "0" };
            TrueOptions = trueOptions;
            FalseOptions = falseOptions;
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
                if (TrueOptions.Contains(message.Content.ToLower().Trim(), StringComparer.OrdinalIgnoreCase))
                {
                    Result = true;
                    result = CardResult.CloseAndContinue;
                }
                else if (FalseOptions.Contains(message.Content.ToLower().Trim(), StringComparer.OrdinalIgnoreCase))
                {
                    Result = true;
                    result = CardResult.CloseAndContinue;
                }
                else
                {
                    var options = TrueOptions.Union(FalseOptions).ToArray();
                    string optionString = options.ToSentence();

                    await ReplyAsync($"I was expecting an answer of ${optionString}. ${RetryMessage}");
                }
            }
            return result;
        }


    }
}
