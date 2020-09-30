using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Cards;
using Discord.Commands;
using LlectroBot.Core.Modules;
using LlectroBot.Core.Preconditions;
using LlectroBot.Guild.Cards;

namespace LlectroBot.Guild.Modules
{
    public partial class GuildCommands
    {
        [Command("nickname")]
        [CommandDescription("Changes the username for the bot or a user.")]
        [CommandSyntax("{prefix}{command} [newnickname]",
                       "{prefix}{command} [user] [newnickname]")]
        [CommandUsage(CommandUsage.Channel)]
        [RequireRole(Roles.RoleLevel.SuperMember, Roles.RoleMatchType.GreaterThanOrEqual)]
        public async Task Nickname(params string[] args)
        {
            // check if any args were provided
            if (args.Length == 0)
            {
                await ReplyAsync("Insufficient parameters provided.");
            }
            else
            {
                var guildBotUser = await Context.Guild.GetCurrentUserAsync();
                // check if the first parameter provided is a user mention
                if (MentionUtils.TryParseUser(args[0], out ulong userId))
                {
                    // if so, check and see if a second parameter was provided
                    if (args.Length < 2)
                    {
                        // if not, gripe
                        await ReplyAsync($"No new username provided, not changing the username for {args[0]}");
                    }
                    else
                    {
                        var newNickname = args[1];
                        // check if the second parameter is a valid nickname
                        if (string.IsNullOrWhiteSpace(newNickname) || newNickname.Trim().Length < 3)
                        {
                            await ReplyAsync($"New username provided is not valid.");
                        }
                        else
                        {
                            IGuildUser user;
                            if (userId == guildBotUser.Id)
                                user = await Context.Guild.GetCurrentUserAsync();
                            else
                                user = await this.Context.Guild.GetUserAsync(userId);
                            await UpdateUser(guildBotUser, user, newNickname);
                        }
                    }
                }
                // first argument provided, but not a user mention
                else
                {
                    var newNickName = args[0];
                    // check if the first parameter is a valid nickname
                    if (args.Length > 1)
                    {
                        await ReplyAsync($"More than one parameter was provided. Aborting in case '{newNickName}' is supposed to be a user mention.");
                    }
                    if (string.IsNullOrWhiteSpace(newNickName) || newNickName.Trim().Length < 3)
                    {
                        await ReplyAsync($"New username provided is not valid.");
                    }
                    else
                    {
                        await UpdateUser(guildBotUser, guildBotUser, newNickName);
                    }
                }
            }
        }

        private async Task UpdateUser(IGuildUser bot, IGuildUser user, string newNickname)
        {
            string oldNickname = user.Username;
            try
            {

                if (user.Id == DiscordClient.CurrentUser.Id)
                    await ReplyAsync($"I changed my name from {oldNickname} to {newNickname}");
                else if (bot.GetHierarchy() <= user.GetHierarchy())
                {
                    await ReplyAsync($"{user.Mention} has a higher role than me. I can't change the nickname of someone whose role is higher than me.");
                }
                else
                {
                    await user.ModifyAsync(f =>
                    {
                        f.Nickname = newNickname.Trim();
                    });
                    await ReplyAsync($"I updated {user.Mention}'s nickname from {oldNickname} to {newNickname}");
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
        }
    }
}
