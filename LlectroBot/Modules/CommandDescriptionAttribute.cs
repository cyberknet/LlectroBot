using System;

namespace LlectroBot.Modules
{
    public class CommandDescriptionAttribute : Attribute
    {
        public string Description { get; private set; }
        public CommandDescriptionAttribute(string description)
        {
            Description = description;
        }
    }
}
