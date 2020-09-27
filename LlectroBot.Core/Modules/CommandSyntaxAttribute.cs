using System;
using Discord.Commands;
using LlectroBot.Core.Configuration;

namespace LlectroBot.Core.Modules
{
    public class CommandSyntaxAttribute : Attribute
    {
        public string Syntax { get; set; }

        public CommandSyntaxAttribute(string syntax)
        {
            Syntax = syntax;
        }

        public string GetSyntax(CommandInfo cmd, IBotConfiguration botConfig, IGuildConfiguration guildConfiguration = null)
        {
            char prefix = botConfig.DirectMessageCommandPrefix;
            if (guildConfiguration != null)
            {
                prefix = guildConfiguration.CommandPrefix;
            }

            return Syntax
                .Replace("{prefix}", prefix.ToString(), StringComparison.OrdinalIgnoreCase)
                .Replace("{command}", cmd.Name, StringComparison.OrdinalIgnoreCase);

        }
    }
}
