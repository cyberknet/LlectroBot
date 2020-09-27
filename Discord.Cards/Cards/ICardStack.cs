using System.Collections.Generic;
using System.Threading.Tasks;

namespace Discord.Cards
{
    public interface ICardStack
    {
        ICardStackManager CardStackManager { get; }
        IChannel Channel { get; set; }
        IGuild Guild { get; set; }
        Stack<ICard> Stack { get; set; }
        IUser User { get; set; }
        ICard ActiveCard { get; }
        int CardCount { get; }

        void Resume(IMessage message);
        Task<bool> ShowCard(ICard card, IMessage message, ICardContext cardContext);
        Task RouteMessage(ICardContext context, IUserMessage message);
    }
}