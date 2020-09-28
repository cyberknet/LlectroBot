using System;
using Discord;

namespace LlectroBot.Polling
{
    public interface IPollTracker
    {
        void CreatePoll(IUser author, IMessageChannel channel, TimeSpan duration, string question, string[] options);
        bool UserHasActivePoll(IUser author, IMessageChannel channel);
    }
}
