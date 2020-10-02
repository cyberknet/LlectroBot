using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Cards;

namespace LlectroBot.Polling.Cards
{
    public class PollOptionsCard : CardBase<List<string>>
    {
        private bool Asked { get; set; }
        public string[] CompleteWords { get; set; }
        private readonly TimeSpan _duration;
        
        private readonly IPollService _pollTracker;
        public PollOptionsCard(string message, bool isCancellable, IPollService pollTracker, TimeSpan duration, string[] completeWords, string[] cancelWords = null, string retryMessage = null)
            : base(message, isCancellable, cancelWords, retryMessage)
        {
            CompleteWords = completeWords;
            _pollTracker = pollTracker;
            _duration = duration;
        }
        public async override Task<CardResult> MessageReceived(IMessage message, CardResult lastCardResult = CardResult.None)
        {
            string stopOptions = CompleteWords.ToOxfordCommaList(", ", "or");
            string cancelOptions = CancelWords.ToOxfordCommaList(", ", "or");
            string doneOrCancelText = $"When you are done, type {stopOptions}. To cancel, type {cancelOptions}.";
            if (!Asked)
            {
                await ReplyAsync($"Initiating a poll for the question '{Message}'. Enter up to five options, one per line. {doneOrCancelText}");
                Asked = true;
                return CardResult.Continue;
            }
            else
            {
                if (CancelWords.Contains(message.Content, StringComparer.OrdinalIgnoreCase))
                {
                    Result.Clear();
                    await ReplyAsync("OK, cancelling the poll.");
                    return CardResult.Cancel;
                }
                if (CompleteWords.Contains(message.Content, StringComparer.OrdinalIgnoreCase))
                {
                    if (Result.Count == 0)
                    {
                        await ReplyAsync("No options were captured, cancelling the poll.");
                        return CardResult.Cancel;
                    }
                    else
                    {
                        _pollTracker.CreatePoll(Context.UserMessage.Author, Context.Channel, _duration, Message, Result.ToArray());
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

                    if (Result.Count == 5)
                    {
                        _pollTracker.CreatePoll(Context.UserMessage.Author, Context.Channel, _duration, Message, Result.ToArray());
                        return CardResult.CloseAndContinue;
                    }
                    else
                    {
                        return CardResult.Continue;
                    }
                    
                }    
            }
            return CardResult.Continue;
        }
    }
}
