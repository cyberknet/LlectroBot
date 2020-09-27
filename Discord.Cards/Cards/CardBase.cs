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

        protected string ArrayToSentence(string[] options, string qualifier = "")
        {
            string optionString;


            if (options == null || options.Length == 0)
            {
                return string.Empty;
            }
            else if (options.Length == 1)
            {
                optionString = options[0];
            }
            // when there are just two options in the list
            else if (options.Length == 2)
            {
                optionString = string.Join($"{qualifier} and {qualifier}", options);
            }
            else
            {
                string join = $"{qualifier}, and {qualifier}";
                optionString = string.Join($"{qualifier}, {qualifier}", options[..^3]); // join all but the last two indices with a comma
                                                                                        // when there are three options in the list
                if (optionString.Length == 3)
                {
                    // append the last index manually
                    optionString += $"{join}{options[^1]}"; // add the last index
                }
                // when there are four or more options
                else
                {
                    // append the remaining indices
                    optionString += string.Join(join, options[^2..]);
                }
            }

            return $"{qualifier}{optionString}{qualifier}";
        }

    }
}
