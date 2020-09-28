using System;
using System.Collections.Generic;
using System.Text;
using Discord;

namespace LlectroBot.Polling
{
    public interface IPoll
    {
        IUser Author { get; set; }
        IMessageChannel Channel { get; set; }
        string Question { get; set; }
        string[] Options { get; set; }
        IUserMessage Message { get; set; }
        List<IEmote> AllowedEmoji { get; set; }
        DateTime ExpiresOn { get; set; }
    }
}
