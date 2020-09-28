using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using LlectroBot.Core.Services;

namespace LlectroBot.Polling
{
    [RegisterService(typeof(IPollTracker))]
    public class PollTracker : IPollTracker
    {
        List<IPoll> _activePolls = new List<IPoll>();

        DiscordSocketClient _discordSocketClient;

        System.Timers.Timer _pollTimer;

        public PollTracker(DiscordSocketClient discordSocketClient)
        {
            _discordSocketClient = discordSocketClient;
            RegisterForEvents();
            _pollTimer = new System.Timers.Timer(1000 * 15); // 1000ms * 15 = 15 seconds
            _pollTimer.Elapsed += _pollTimer_Elapsed;
            _pollTimer.Start();
        }

        private void _pollTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            DateTime Now = DateTime.Now;
            var expiringPolls = _activePolls.Where(p => p.ExpiresOn <= Now).ToArray();
            for(int i = 0; i < expiringPolls.Length; i++)
            {
                ExpirePoll(expiringPolls[i]);
            }
        }

        private void RegisterForEvents()
        {
            _discordSocketClient.ReactionAdded += Client_ReactionAdded;
        }

        public async void ExpirePoll(IPoll poll)
        {
            if (_activePolls.Contains(poll))
            {
                _activePolls.Remove(poll);
                var channel = poll.Channel;
                var message = await channel.GetMessageAsync(poll.Message.Id);
                EmbedBuilder builder = new EmbedBuilder();
                builder.WithTitle("Poll Results Are In!");
                builder.WithDescription($"The results are in for {poll.Author.Mention}'s question \"{poll.Question}\":");
                for (int i = 0; i < poll.Options.Length; i++)
                {
                    var optionEmote = poll.AllowedEmoji[i];
                    var votes = 1;
                    var key = message.Reactions.Keys.FirstOrDefault(k => k.Name == optionEmote.Name);
                    if (key != null)
                        votes = message.Reactions[key].ReactionCount;
                    votes -= 1;
                    builder.AddField(poll.Options[i], votes);
                }
                builder.WithFooter("* Initial bot votes not included in vote totals.");
                await poll.Channel.SendMessageAsync(embed: builder.Build());
                await poll.Channel.DeleteMessageAsync(poll.Message);
            }
        }

        public void ExpirePoll(IUser author, IMessageChannel channel)
        {
            var poll = _activePolls.FirstOrDefault(p => p.Author.Id == author.Id && p.Channel.Id == channel.Id);
            if (poll != null)
            {
                ExpirePoll(poll);
            }
        }

        private async Task Client_ReactionAdded(Cacheable<IUserMessage, ulong> message, ISocketMessageChannel channel, SocketReaction reaction)
        {
            if (reaction.User.IsSpecified)
            {
                var user = reaction.User.Value;
                var poll = _activePolls.FirstOrDefault(p => p.Message.Id == message.Id);
                if (poll != null)
                {
                    var userMessage = await message.GetOrDownloadAsync();
                    // if the user used an emote that wasn't allowed
                    if (!poll.AllowedEmoji.Exists(emote => emote.Name == reaction.Emote.Name))
                    {
                        // remove the emote
                        await userMessage.RemoveReactionAsync(reaction.Emote, reaction.User.Value);
                    }
                    else
                    {
                        // don't remove multiple votes from the bot
                        if (user.Id != _discordSocketClient.CurrentUser.Id)
                        {
                            // loop through all the emotes used
                            foreach (var emote in userMessage.Reactions.Keys.Where(r => r != reaction.Emote))
                            {
                                
                                if (emote.Name != reaction.Emote.Name)
                                {
                                    await userMessage.RemoveReactionAsync(emote, user);
                                }
                            }
                        }
                    }
                }
            }
        }

        public async void CreatePoll(IUser author, IMessageChannel channel, TimeSpan duration, string question, string[] options)
        {
            IPoll poll = _activePolls.FirstOrDefault(p => p.Author.Id == author.Id && p.Channel.Id == channel.Id);
            if (poll != null)
            {
                // user already has a poll running. We limit polls to one per user per channel
            }
            else
            {
                poll = new Poll
                {
                    Author = author,
                    Channel = channel,
                    Question = question,
                    Options = options,
                    ExpiresOn = DateTime.Now + duration
                };

                var optionEmoji = new string[]
                {
                "\u0031\uFE0F\u20E3", // 1
                "\u0032\uFE0F\u20E3", // 2
                "\u0033\uFE0F\u20E3", // 3
                "\u0034\uFE0F\u20E3", // 4
                "\u0035\uFE0F\u20E3", // 5
                "\u0036\uFE0F\u20E3", // 6
                "\u0037\uFE0F\u20E3", // 7
                "\u0038\uFE0F\u20E3", // 8
                "\u0039\uFE0F\u20E3", // 9
                };
                StringBuilder pollText = new StringBuilder();
                EmbedBuilder builder = new EmbedBuilder();
                builder.WithTitle($"A new poll has started");
                pollText.AppendLine($"{author.Mention} started a {duration.Minutes} minute poll for: \"{ question}\"");
                List<IEmote> addEmoji = new List<IEmote>();
                for (int i = 0; i < options.Length; i++)
                {
                    pollText.AppendLine($"\t{optionEmoji[i]} {options[i]}");
                    addEmoji.Add(new Emoji(optionEmoji[i]));

                }
                pollText.Append("To vote for an option, click on the respective emote below.");
                builder.WithDescription(pollText.ToString());
                poll.AllowedEmoji = addEmoji;
                _activePolls.Add(poll);

                var message = await channel.SendMessageAsync(embed: builder.Build());
                poll.Message = message;

                // add the emote options for this poll
                foreach(var emoji in addEmoji)
                {
                    await message.AddReactionAsync(emoji);
                }
            }
        }

        public bool UserHasActivePoll(IUser author, IMessageChannel channel)
        {
            IPoll poll = _activePolls.FirstOrDefault(p => p.Author.Id == author.Id && p.Channel.Id == channel.Id);
            return poll != null;
        }
    }
}
