using System.Linq;
using System.Threading.Tasks;

namespace Discord.Cards.Cards.PrimitiveCards
{
    public class InputRoleCard : CardBase<IRole>
    {
        public bool Asked { get; set; }
        public InputRoleCard(string message, bool isCancellable, string[] cancelWords = null, string retryMessage = null)
            : base(message, isCancellable, cancelWords, retryMessage)
        {
        }
        public override async Task<CardResult> MessageReceived(IMessage message, CardResult lastCardResult = CardResult.None)
        {
            var result = CardResult.Continue;
            if (!Asked)
            {
                Asked = true;
                await ReplyAsync(Message);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(message.Content))
                {
                    await ReplyAsync($"Sorry, I didn't see a response. {RetryMessage}");
                }
                else
                {
                    if (message.MentionedRoleIds.Count == 0)
                    {
                        await ReplyAsync($"You need to mention the role, not just type its name. {RetryMessage}");
                    }
                    else if (message.MentionedRoleIds.Count > 1)
                    {
                        await ReplyAsync($"Please only mention one role. {RetryMessage}");
                    }
                    else
                    {
                        var roleId = message.MentionedRoleIds.First();
                        Result = Context.Guild.GetRole(roleId);
                        result = CardResult.CloseAndContinue;
                    }

                }
            }
            return result;
        }
    }
}
