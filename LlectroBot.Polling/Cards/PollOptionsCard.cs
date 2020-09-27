using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Cards;

namespace LlectroBot.Polling.Cards
{
    public class PollOptionsCard : CardBase<List<string>>
    {
        private bool Asked { get; set; }
        public string[] CompleteWords { get; set; }
        public PollOptionsCard(string message, bool isCancellable, string[] completeWords, string[] cancelWords = null, string retryMessage = null)
            : base(message, isCancellable, cancelWords, retryMessage)
        {
            CompleteWords = completeWords;
        }
        public async override Task<CardResult> MessageReceived(IMessage message, CardResult lastCardResult = CardResult.None)
        {
            string stopOptions = ArrayToSentence(CompleteWords, "'");
            string cancelOptions = ArrayToSentence(CancelWords, "'");
            string doneOrCancelText = $"When you are done, type {stopOptions}. To cancel, type {cancelOptions}.";
            if (!Asked)
            {
                await ReplyAsync($"Initiating a poll for the question '{Message}'. Enter up to five options, one per line. {doneOrCancelText}");
                return CardResult.Continue;
            }
            else
            {
                if (CancelWords.Contains(message.Content, StringComparer.OrdinalIgnoreCase))
                {
                    Result.Clear();
                    return CardResult.Cancel;
                }
                if (CompleteWords.Contains(message.Content, StringComparer.OrdinalIgnoreCase))
                {
                    if (Result.Count == 0)
                    {
                        return CardResult.Cancel;
                    }
                    else
                    {
                        StartPoll();
                        return CardResult.CloseAndContinue;
                    }
                }
                else if (String.IsNullOrWhiteSpace(message.Content))
                {
                    await ReplyAsync($"I didn't understand that option. You can add {5 - Result.Count} more options. {doneOrCancelText}");
                }
                else if(Result.Contains(message.Content.Trim(), StringComparer.OrdinalIgnoreCase))
                {
                    await ReplyAsync($"You already added that option. You can add {5 - Result.Count} more options. {doneOrCancelText}");
                }
                else
                {
                    Result.Add(message.Content.Trim());
                    StartPoll();
                    return Result.Count == 5 ? CardResult.CloseAndContinue : CardResult.Continue;
                }    
            }
            return CardResult.Continue;
        }

        private void StartPoll()
        {

        }
    }
}
