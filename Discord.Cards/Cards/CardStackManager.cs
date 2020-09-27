using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord.WebSocket;

namespace Discord.Cards
{
    public abstract class CardStackManager<T> : ICardStackManager where T : CardContext
    {
        protected readonly IDiscordClient DiscordClient;

        public List<ICardStack> Stacks { get; set; }

        public CardStackManager(IDiscordClient discordClient)
        {
            Stacks = OnRead().GetAwaiter().GetResult();
            DiscordClient = discordClient;
        }

        public bool RouteMessage(IUserMessage message)
        {
            ICardStack stack;
            // check if this is a guild message
            var tch = message.Channel as SocketTextChannel;
            if (tch != null)
            {
                // it is a guild message, so get a guild stack for this user in the channel the message came from
                stack = GetStack(tch.Guild, message.Channel, message.Author);
            }
            // this is not a guild message
            else
            {
                // get a direct message stack
                stack = GetStack(message.Author);
            }

            // if an appropriate stack was not found
            if (stack == null || stack.CardCount == 0)
                // exit and let regular command processing take place
                return false;

            // an appropriate command stack was found
            try
            {
                var context = GetCardContext(message, tch);
                stack.RouteMessage(context, message);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        protected abstract ICardContext GetCardContext(IUserMessage message, SocketTextChannel tch);

        private ICardStack GetStack(IGuild guild, IChannel channel, IUser user)
        {
            return Stacks.FirstOrDefault(s =>
                s?.Guild.Id == guild.Id
                && s?.Channel.Id == channel.Id
                && s?.User.Id == user.Id);
        }
        private ICardStack GetStack(IUser user)
        {
            return Stacks.FirstOrDefault(s => s.Guild == null && s.Channel == null && s.User.Id == user.Id);
        }

        public bool CreateCardStack(ICard card, IUserMessage message)
        {
            ICardStack stack;
            var tch = message.Channel as SocketTextChannel;
            if (tch != null)
            {
                // it is a guild message, so get a guild stack for this user in the channel the message came from
                stack = GetStack(tch.Guild, message.Channel, message.Author);
            }
            // this is not a guild message
            else
            {
                // get a direct message stack
                stack = GetStack(message.Author);
            }
            if (stack != null && stack.CardCount > 0)
            {
                return false;
            }
            else
            {
                // stack did not exist, we have to create one
                if (stack == null)
                {
                    stack = new CardStack(this)
                    {
                        Channel = message.Channel,
                        Guild = tch?.Guild,
                        User = message.Author
                    };
                    Stacks.Add(stack);
                }
                stack.ShowCard(card, message, GetCardContext(message, tch));
            }
            return true;
        }

        public void Save()
        {
            OnSave();
        }

        protected abstract void OnSave();

        protected abstract Task<List<ICardStack>> OnRead();

    }
}
