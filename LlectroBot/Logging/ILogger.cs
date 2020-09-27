using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace LlectroBot.Logging
{
    public interface ILogger
    {
        Task ChatMessage(IMessage message, IResult result, IGuild guild = null);
        Task Error(IGuild guild, Exception ex, [CallerMemberName] string Method = "");
    }
}
