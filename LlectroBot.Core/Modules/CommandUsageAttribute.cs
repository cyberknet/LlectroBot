using System;

namespace LlectroBot.Core.Modules
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
