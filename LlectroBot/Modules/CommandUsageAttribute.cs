using System;

namespace LlectroBot.Modules
{
    public class CommandUsageAttribute : Attribute
    {
        public CommandUsage Usage { get; private set; }
        public CommandUsageAttribute(CommandUsage usage)
        {
            Usage = usage;
        }
    }
}
