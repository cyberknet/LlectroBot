using LlectroBot.Core.Services;

namespace LlectroBot.Polling
{
    [RegisterService(typeof(IPollTracker))]
    public class PollTracker : IPollTracker
    {
    }
}
