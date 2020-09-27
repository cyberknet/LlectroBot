using Discord.Commands;

namespace LlectroBot
{
    public interface IDiscordBot
    {
        CommandService Commands { get; }
    }
}
