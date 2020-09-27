using System.Collections.Generic;
using System.Threading.Tasks;

namespace Discord.Cards
{
    public class CardStack : ICardStack
    {
        public IGuild Guild { get; set; }
        public IChannel Channel { get; set; }
        public IUser User { get; set; }
        public Stack<ICard> Stack { get; set; } = new Stack<ICard>();
        public ICard ActiveCard => Stack.Peek();

        public ICardStackManager CardStackManager { get; private set; }

        public int CardCount => Stack.Count;

        public CardStack(ICardStackManager cardStackManager)
        {
            CardStackManager = cardStackManager;
        }

        public void Resume(IMessage message)
        {
            var activeCard = Stack.Peek();
            activeCard.MessageReceived(message);
        }

        public async Task<bool> ShowCard(ICard card, IMessage message, ICardContext cardContext)
        {
            // push the card onto the stack
            card.CardStack = this;
            card.Context = cardContext;
            Stack.Push(card);

            var result = await card.MessageReceived(message);
            if (result != CardResult.Continue)
                await CloseCard(card, result);
            return true;
        }
        private async Task CloseCard(ICard card, CardResult lastCardResult)
        {
            bool closeActiveCard = true;
            while (closeActiveCard)
            {
                // make sure we are closing cards in order
                if (Stack.Peek() != card)
                    break;

                //pull the card off the stack
                if (Stack.Count > 0)
                    // pull the active card off the stack
                    Stack.Pop();

                // if there are still cards left on the stack
                if (Stack.Count > 0)
                {


                    var lastCard = Stack.Peek();
                    // set the context before we activate the card
                    lastCard.Context = card.Context;
                    // let the parent dialog know the child closed
                    var result = await lastCard.MessageReceived(card.Context.UserMessage, lastCardResult);
                    card = lastCard;

                    // if the card indicated it wanted to close
                    if (result == CardResult.CloseAndContinue)
                        // loop around again to close the active card
                        closeActiveCard = true;
                }
                // the card was not the topmost card on the stack
                else
                {
                    // don't loop around again, bail
                    closeActiveCard = false;
                }
            }
        }

        public async Task RouteMessage(ICardContext context, IUserMessage message)
        {
            var card = ActiveCard;
            if (card != null)
            {
                card.Context = context;
                var cardResult = await card.MessageReceived(message);
                if (cardResult == CardResult.CloseAndContinue)
                    await CloseCard(card, cardResult);
            }
        }
    }
}
