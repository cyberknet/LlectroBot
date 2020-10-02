using System;
using System.Collections.Generic;
using System.Text;
using Discord;

namespace LlectroBot.Remind
{
    public interface IReminder
    {
        IGuild Guild { get; set; }
        IUser User { get; set; }
        ITextChannel Channel { get; set; }
        DateTime RemindOn { get; set; }
        string Message { get; set; }
    }
}
