using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using LlectroBot.Core.Modules;

namespace LlectroBot.Games.Modules
{
    public partial class GameCommands
    {
        [Command("card")]
        [Alias("highcard")]
        [CommandDescription("Draws a playing card for you and another username, or the bot if not provided. Highest card wins.")]
        [CommandSyntax("{prefix}{command} [optional: Username]")]
        [CommandUsage(CommandUsage.Both)]
        public async Task Card(IUser user = null)
        {
            var random = new Random();
            string[] suitNames = { "Spades", "Clubs", "Diamonds", "Hearts" };
            string[] cardNames = { "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Jack", "Queen", "King", "Ace" };
            int theirCard = random.Next(0, cardNames.Length);
            int theirSuit = random.Next(0, suitNames.Length);
            int myCard = theirCard;
            int mySuit = theirSuit;

            while (myCard == theirCard && mySuit == theirSuit)
            {
                myCard = random.Next(0, cardNames.Length);
                mySuit = random.Next(0, suitNames.Length);
            }

            bool challengerWins = false;

            if (theirCard > myCard)
            {
                challengerWins = true;
            }
            else if ((theirCard == myCard) && (theirSuit > mySuit))
            {
                challengerWins = true;
            }

            string challengedName = "I";
            string win = "win";
            if (user != null)
            {
                challengedName = user.Username;
                win = "wins";

            }
            string theirCardName = $"{cardNames[theirCard]} of {suitNames[theirSuit]}";
            string myCardName = $"{cardNames[myCard]} of {suitNames[mySuit]}";
            string winnerName = challengedName;
            if (challengerWins)
            {
                winnerName = Context.User.Username;
            }

            await ReplyAsync($"{challengedName} drew the {myCardName} and {Context.User.Username} drew the {theirCardName}. {winnerName} {win}!");
        }
    }
}
