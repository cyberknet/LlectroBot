using System;
using System.Threading.Tasks;
using Discord.Commands;
using LlectroBot.Core.Modules;

namespace LlectroBot.Games.Modules
{
    public partial class GameCommands
    {
        [Command("8ball")]
        [Alias("magic8ball")]
        [CommandDescription("Asks the magic 8 ball a question, and provides the answer.")]
        [CommandSyntax("{prefix}{command} [question]")]
        [CommandUsage(CommandUsage.Both)]
        public async Task Magic8Ball(params string[] question)
        {
            string[] answers = {"As I see it, yes.",    "Ask again later.",             "Better not tell you now.",
                                "Cannot predict now.",  "Concentrate and ask again.",   "Don’t count on it.",
                                "It is certain.",       "It is decidedly so.",          "Most likely.",
                                "My reply is no.",      "My sources say no.",           "Outlook not so good.",
                                "Outlook good.",        "Reply hazy, try again.",       "Signs point to yes.",
                                "Very doubtful.",       "Without a doubt.",             "Yes.",
                                "Yes – definitely.",    "You may rely on it."};
            var random = new Random();
            var index = random.Next(0, answers.Length);
            for (int i = 0; i < question.Length; i++)
            {
                if (question[i].Contains(' '))
                {
                    if (i > 0)
                    {
                        question[i - 1] = $"{question[i - 1]}\"";
                    }
                    else
                    {
                        question[i] = $"\"{question[i]}";
                    }

                    if (i < question.Length - 1)
                    {
                        question[i + 1] = $"\"{question[i + 1]}";
                    }
                    else
                    {
                        question[i] = $"{question[i]}\"";
                    }
                }
            }
            string questionText = string.Join(' ', question).Trim();
            if (!questionText.EndsWith("?"))
            {
                await ReplyAsync("The magic 8 ball only answers questions.");
            }
            else
            {

                await ReplyAsync($"{Context.User.Username} wants to know \"{questionText}\". The magic 8 ball says: {answers[index]}");
            }
        }
    }
}
