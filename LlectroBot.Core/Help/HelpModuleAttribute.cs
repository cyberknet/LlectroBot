using System;
using System.Collections.Generic;
using System.Text;

namespace LlectroBot.Core.Help
{
    [AttributeUsage(AttributeTargets.Method)]
    public class HelpModuleAttribute : Attribute
    {
        public string ShowHelpUnderModule { get; set; }
        public HelpModuleAttribute(string showHelpUnderModule)
        {
            ShowHelpUnderModule = showHelpUnderModule;
        }
    }
}
