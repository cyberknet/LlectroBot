using System;
using System.Collections.Generic;
using System.Text;

namespace LlectroBot.Core.Configuration
{
    public interface IConfigurationChannel
    {
        public string Name { get; set; }
        public ulong Id { get; set; }
    }
}
