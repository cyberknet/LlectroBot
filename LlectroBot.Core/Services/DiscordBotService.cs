using System;
using System.Collections.Generic;
using System.Text;
using Discord.WebSocket;
using LlectroBot.Core.Configuration;

namespace LlectroBot.Core.Services
{
    public abstract class DiscordBotService
    {
        protected DiscordSocketClient _discordSocketClient;
        protected IBotConfiguration _botConfiguration;
        public DiscordBotService(DiscordSocketClient discordSocketClient, IBotConfiguration botConfiguration)
        {
            _discordSocketClient = discordSocketClient;
            _botConfiguration = botConfiguration;
            RegisterForEvents();
        }

        protected abstract void RegisterForEvents();
    }
}
