using System;
using System.Collections.Generic;
using System.Text;

namespace LlectroBot.Core.Configuration
{
    public class ConfigurationChannel : IConfigurationChannel
    {
        public string Name { get; set; }
        public ulong Id { get; set; }
    }
}
