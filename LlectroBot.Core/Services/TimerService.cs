using System;
using System.Collections.Generic;
using System.Text;
using Discord.WebSocket;
using LlectroBot.Core.Configuration;

namespace LlectroBot.Core.Services
{
    public abstract class TimerService : DiscordBotService
    {
        protected System.Timers.Timer Timer { get; set; }
        protected TimerService(DiscordSocketClient discordSocketClient, IBotConfiguration botConfiguration)
            : base(discordSocketClient, botConfiguration)
        {
            Timer = new System.Timers.Timer();
            Timer.Elapsed += Timer_Elapsed;
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            OnTimerElapsed(e.SignalTime);
        }
        protected abstract void OnTimerElapsed(DateTime signalTime);

        protected void InitializeTimer(double interval, bool autoReset)
        {
            StopTimer();
            Timer.Interval = interval;
            Timer.AutoReset = autoReset;
            StartTimer();
        }

        protected void StopTimer() => Timer.Stop();
        protected void StartTimer() => Timer.Start();
        
    }
}
