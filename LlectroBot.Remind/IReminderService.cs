using System;
using Discord;

namespace LlectroBot.Remind
{
    public interface IReminderService
    {
        void SetReminder(IUser user, IGuild guild, ITextChannel textChannel, TimeSpan timespan, string reminderText);
    }
}
