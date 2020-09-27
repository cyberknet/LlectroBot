using System.Threading.Tasks;

namespace Discord.Cards
{
    public interface ICard
    {
        ICardStack CardStack { get; set; }
        ICardContext Context { get; set; }
        //Task<CardResult> MessageReceived(IMessage message);
        Task<CardResult> MessageReceived(IMessage message, CardResult lastCardResult = CardResult.None);
        bool IsCancellable { get; }
        string[] CancelWords { get; }
        string Message { get; set; }
        string RetryMessage { get; set; }
    }
}