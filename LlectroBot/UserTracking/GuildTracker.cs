using System;
using System.Collections.Generic;
using System.Text;

namespace LlectroBot.UserTracking
{
    public class GuildTracker
    {
        public ulong GuildId { get; set; }
        public string GuildName { get; set; }
        public List<UserState> Users { get; set; } = new List<UserState>();
        public List<UserTransition> Transitions { get; set; } = new List<UserTransition>();
    }
}
