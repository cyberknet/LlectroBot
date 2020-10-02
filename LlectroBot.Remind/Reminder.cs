using System;
using System.Collections.Generic;
using System.Text;
using Discord;

namespace LlectroBot.Remind
{
    public class Reminder : IReminder
    {
        public IGuild Guild { get; set; }
        public IUser User { get; set; }
        public ITextChannel Channel { get; set; }
        public DateTime RemindOn { get; set; }
        public string Message { get; set; }
    }
}
