using System;
using System.Collections.Generic;
using System.Text;
using Discord;

namespace LlectroBot.Polling
{
    public class Poll : IPoll
    {
        public IUser Author { get; set; }
        public IMessageChannel Channel { get; set; }
        public string Question { get; set; }
        public string[] Options { get; set; }
        public IUserMessage Message { get; set; }
        public List<IEmote> AllowedEmoji { get; set; }
        public DateTime ExpiresOn { get; set; }
    }
}
