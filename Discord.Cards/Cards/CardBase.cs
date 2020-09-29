using System.Threading.Tasks;

namespace Discord.Cards
{
    public abstract class CardBase<T> : ICard
    {
        public ICardStack CardStack { get; set; }
        public ICardContext Context { get; set; }
        public virtual bool IsCancellable { get; protected set; }
        public virtual string[] CancelWords { get; protected set; }
        public string Message { get; set; }
        public string RetryMessage { get; set; }
        public T Result { get; set; }

        public CardBase(string message, bool isCancellable, string[] cancelWords = null, string retryMessage = null)
        {
            Message = message;
            RetryMessage = retryMessage ?? message;
            IsCancellable = isCancellable;
            if (cancelWords == null)
                cancelWords = new string[] { };
            CancelWords = cancelWords;

            InitializeResult();
        }

        private void InitializeResult()
        {
            var constructorInfo = typeof(T).GetConstructor(System.Type.EmptyTypes);
            if (constructorInfo != null)
            {
                Result = (T)constructorInfo.Invoke(null);
            }
            else
                Result = default;
        }

        public abstract Task<CardResult> MessageReceived(IMessage message, CardResult lastCardResult = CardResult.None);

        /// <summary>
        ///     Sends a message to the source channel.
        /// </summary>
        /// <param name="message">
        /// Contents of the message; optional only if <paramref name="embed" /> is specified.
        /// </param>
        /// <param name="isTTS">Specifies if Discord should read this <paramref name="message"/> aloud using text-to-speech.</param>
        /// <param name="embed">An embed to be displayed alongside the <paramref name="message"/>.</param>
        /// <param name="allowedMentions">
        ///     Specifies if notifications are sent for mentioned users and roles in the <paramref name="message"/>.
        ///     If <c>null</c>, all mentioned roles and users will be notified.
        /// </param>
        protected virtual async Task<IUserMessage> ReplyAsync(string message = null, bool isTTS = false, Embed embed = null, RequestOptions options = null)
        {
            return await Context.Channel.SendMessageAsync(message, isTTS, embed, options).ConfigureAwait(false);
        }

    }
}
