using System.Collections.Generic;

namespace Discord.Cards
{
    public interface ICardStackManager
    {
        List<ICardStack> Stacks { get; set; }

        bool RouteMessage(IUserMessage message);
        bool CreateCardStack(ICard card, IUserMessage message);

        void Save();
    }
}