using System;
using Discord;

namespace LlectroBot.UserTracking
{
    public class UserState
    {
        public ulong UserId { get; set; }
        public string Username { get; set; }
        public string LastAction { get; set; }
        public DateTime LastActiveOn { get; set; }
    }
}
