using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Discord;
using Discord.WebSocket;
using LlectroBot.Core.Configuration;
using LlectroBot.Core.Services;

namespace LlectroBot.Remind
{
    [RegisterServiceInterface(typeof(IReminderService))]
    public class ReminderService : TimerService, IReminderService
    {
        public List<IReminder> Reminders { get; set; } = new List<IReminder>();
        public ReminderService(DiscordSocketClient discordSocketClient, IBotConfiguration botConfiguration)
                : base(discordSocketClient, botConfiguration)
        {
            double interval = 1000 * 15;
            InitializeTimer(interval, true);
        }

        protected override void OnTimerElapsed(DateTime signalTime)
        {
            var reminders = Reminders.Where(p => p.RemindOn <= signalTime).ToArray();
            for (int i = 0; i < reminders.Length; i++)
            {
                SendReminder(reminders[i]);
            }
        }

        private void SendReminder(IReminder reminder)
        {
            if (Reminders.Contains(reminder))
            {
                Reminders.Remove(reminder);
                reminder.Channel.SendMessageAsync($"{reminder.User.Mention}, here is your reminder for '{reminder.Message}'");
            }
        }

        protected override void RegisterForEvents()
        {
            // this service does not need to listen for any events
        }

        public void SetReminder(IUser user, IGuild guild, ITextChannel textChannel, TimeSpan timespan, string reminderText)
        {
            var reminder = new Reminder 
            { 
                User = user, 
                Channel = textChannel, 
                Guild = guild, 
                RemindOn = DateTime.Now + timespan, 
                Message = reminderText 
            };
            Reminders.Add(reminder);
        }

        public void SetReminder(IUser user, ITextChannel textChannel, TimeSpan timespan, string reminderText)
        {
            SetReminder(user, null, textChannel, timespan, reminderText);
        }
    }
}
