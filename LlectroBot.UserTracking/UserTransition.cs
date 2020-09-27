namespace LlectroBot.UserTracking
{
    public class UserTransition
    {
        public ulong UserId { get; set; }
        public string FromUserName { get; set; }
        public string ToUserName { get; set; }
    }
}
