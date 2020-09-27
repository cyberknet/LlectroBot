using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using LlectroBot.Core.Modules;

namespace LlectroBot.Games.Modules
{
    public partial class GameCommands
    {
        [Command("flip")]
        [Alias("coin", "cointoss", "toss")]
        [CommandDescription("Tosses a coin. If a username is provided, the user is challenged to the coin toss. If no prediction is provided, the challenger is presumed to have chosen heads.")]
        [CommandSyntax("{prefix}{command} [optional user] [optional prediction: heads|tails]")]
        [CommandUsage(CommandUsage.Both)]
        //[RequireBotPermission(ChannelPermission.SendMessages)]
        public async Task Flip(params string[] values)
        {
            bool hadPrediction = false;
            bool predictedHeads = true;
            IUser otherUser = null;
            IUser user = Context.User;

            if (values.Length > 2)
            {
                return;
            }

            if (values.Length == 0)
            {
                hadPrediction = false;
            }
            else if (values.Length == 1)
            {
                // if the value parses as a coin face
                if (TryParseCoinFace(values[0], ref predictedHeads))
                {
                    // remember that we had a prediction
                    hadPrediction = true;
                }
                // if the value does not parse as a coin face, it must be a user
                else
                {
                    // if we are unable to parse a user out, then bail.
                    if (Context.Guild != null && !TryParseUser(values[0], ref otherUser))
                    {
                        return;
                    }
                }
            }
            else if (values.Length == 2)
            {
                // when two parameters are present, the first must be a user
                if (Context.Guild == null || !TryParseUser(values[0], ref otherUser))
                {
                    return;
                }
                // when two parameters are present, the second must be a coin face
                if (!TryParseCoinFace(values[1], ref predictedHeads))
                {
                    return;
                }

                hadPrediction = true;
            }

            if (user.Id == otherUser?.Id)
            {
                otherUser = null;
            }
            //var user = Context.User;
            var random = new Random();
            var value = random.Next(0, 2) == 0;
            string result = value ? "heads" : "tails";
            string who = otherUser == null ? user.Username : $"{user.Username} and {otherUser.Username}";
            string winnerText = ", {0} is the winner!";

            if (otherUser == null && hadPrediction)
            {
                winnerText = predictedHeads == value ? ", you guessed correctly!" : " - you didn't guess correctly.";
            }
            else if (otherUser == null)
            {
                winnerText = string.Empty;
            }
            else
            {
                var winner = (value == predictedHeads ? user : otherUser);
                winnerText = string.Format(winnerText, winner.Username);
            }

            await ReplyAsync($"I flipped a coin for {who} and the result was {result}{winnerText}");
        }

        private bool TryParseUser(string username, ref IUser user)
        {
            if (Context.Guild != null)
            {
                if (MentionUtils.TryParseUser(username, out ulong userId))
                {
                    user = DiscordClient.GetUserAsync(userId).GetAwaiter().GetResult();
                    return true;
                }
                else
                {
                    var users = Context.Guild.GetUsersAsync().GetAwaiter().GetResult();
                    var foundUser = users.FirstOrDefault(u => u.Username.ToLower().Trim() == username.ToLower().Trim());
                    if (foundUser != null)
                    {
                        user = foundUser;
                        return true;
                    }
                }
            }
            return false;
        }
        private bool TryParseCoinFace(string text, ref bool isHeads)
        {
            switch (text.ToLower().Trim())
            {
                case "heads":
                case "h":
                case "0":
                case "true":
                    isHeads = true;
                    break;

                case "tails":
                case "t":
                case "1":
                case "false":
                    isHeads = false;
                    break;
                default:
                    return false;
            }
            return true;
        }
    }
}
