using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using LlectroBot.Core.Configuration;
using LlectroBot.Core.Help;
using LlectroBot.Core.Modules;

namespace LlectroBot.Help.Modules
{
    public class HelpCommands : CommandModuleBase
    {
        public HelpCommands(IDiscordClient discordClient, IDiscordBot discordBot, IServiceProvider serviceProvider, IBotConfiguration botConfiguration)
            : base(discordClient, discordBot, serviceProvider, botConfiguration)
        {
        }

        #region Help
        [Command("help")]
        public async Task Help(string command = null)
        {
            EmbedBuilder builder = new EmbedBuilder();
            var commands = DiscordBot.Commands;
            var commandInfos = commands.Commands;

            char prefix = BotConfiguration.DirectMessageCommandPrefix;
            var guildConfig = Context.GuildConfiguration;
            if (guildConfig != null)
            {
                prefix = guildConfig.CommandPrefix;
            }

            if (string.IsNullOrWhiteSpace(command))
            {
                await ShowGenericHelp(builder, commandInfos, prefix);
            }
            else
            {
                if (command.StartsWith(prefix))
                {
                    command = command[1..]; // .Substring(1)
                }

                command = command.ToLower().Trim();
                var commandInfo = commandInfos.FirstOrDefault(ci =>
                    ci.Name.ToLower().Trim() == command || // command name == command
                    ci.Aliases.Select(a => a.ToLower().Trim()).Contains(command)); // one of command aliases == command
                // if the command could be found
                if (commandInfo != null)
                {
                    // show the command help
                    await ShowCommandHelp(commandInfo, builder, prefix);
                }
                // if the command could not be found
                else
                {
                    // show the generic help
                    await ShowGenericHelp(builder, commandInfos, prefix);
                }
            }
        }

        private async Task ShowCommandHelp(CommandInfo commandInfo, EmbedBuilder builder, char prefix)
        {

            var commandName = char.ToUpper(commandInfo.Name[0]) + commandInfo.Name[1..]; // .Substring(1)
            var syntaxAttribute = commandInfo.Attributes.Where(a => a is CommandSyntaxAttribute).FirstOrDefault() as CommandSyntaxAttribute;
            var descriptionAttribute = commandInfo.Attributes.Where(a => a is CommandDescriptionAttribute).FirstOrDefault() as CommandDescriptionAttribute;
            var usageAttribute = commandInfo.Attributes.Where(a => a is CommandUsageAttribute).FirstOrDefault() as CommandUsageAttribute;
            var description = descriptionAttribute?.Description;
            var syntax = syntaxAttribute?.GetSyntax(commandInfo, BotConfiguration, Context.GuildConfiguration);
            var aliases = string.Join(' ', commandInfo.Aliases.Select(a => $"{prefix}{a}"));
            var usage = usageAttribute?.Usage switch
            {
                CommandUsage.DM => "DM",
                CommandUsage.Channel => "Channel",
                CommandUsage.Both => "Channel and DM",
                _ => "Unknown"
            };


            builder.WithTitle($"{commandName} Command");
            if (!string.IsNullOrWhiteSpace(description))
            {
                builder.WithDescription(description);
            }

            if (syntax != null)
            {
                if (syntax.Length == 1)
                {
                    builder.AddField("Syntax", syntax[0]);
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < syntax.Length; i++)
                        sb.AppendLine(syntax[i]);
                    builder.AddField("Syntax", sb.ToString());
                }
            }

            if (!string.IsNullOrEmpty(aliases))
            {
                builder.AddField("Aliases", aliases, true);
            }

            if (!string.IsNullOrEmpty(aliases))
            {
                builder.AddField("Works In", usage, true);
            }

            await Context.User.SendMessageAsync(embed: builder.Build());
        }

        private async Task ShowGenericHelp(EmbedBuilder builder, IEnumerable<CommandInfo> commandInfos, char prefix)
        {
            builder.WithTitle("LlectroBot Help");
            builder.Description = "I support the following commands.";
            StringBuilder fieldValue = new StringBuilder();
            void addField(CommandInfo lastCommand, string moduleName, StringBuilder fieldValue)
            {
                var value = fieldValue.ToString().Trim();
                string module = moduleName
                    .Replace("Commands", "", StringComparison.OrdinalIgnoreCase)
                    .Replace("Command", string.Empty, StringComparison.OrdinalIgnoreCase);
                // if the module name is a PascalCaseWord, add spaces before capital letters (except the first)
                module = Regex.Replace(module, "([a-z])([A-Z])", "$1 $2");
                builder.AddField($"{module} Commands", value);
            }

            var ciTemp = commandInfos.Select(ci =>
            {
                var helpModule = ci.Attributes.Where(a => a is HelpModuleAttribute).FirstOrDefault() as HelpModuleAttribute;
                return new
                {
                    Syntax = ci.Attributes.Where(a => a is CommandSyntaxAttribute).FirstOrDefault() as CommandSyntaxAttribute,
                    ModuleName = helpModule?.ShowHelpUnderModule ?? ci.Module.Name,
                    Command = ci
                };
            });

            var sortedCommandInfos = ciTemp
                // only show commands that have a syntax attribute (skip some hidden commands)
                .Where(ci => ci.Syntax != null)
                // order by 
                .OrderBy(ci =>ci.ModuleName);


            var firstCommandInfo = sortedCommandInfos.FirstOrDefault();
            var lastCommand = firstCommandInfo; lastCommand = null;
            // loop over all the commands found
            foreach (var commandInfo in sortedCommandInfos)
            {
                
                var syntax = commandInfo.Syntax;
                // if the module has changed, we need to add the last module to the Embed
                if (lastCommand != null && lastCommand.ModuleName != commandInfo.ModuleName)
                {
                    addField(lastCommand.Command, lastCommand.ModuleName, fieldValue);
                    fieldValue.Length = 0;
                }

                // add the command to the filedValue stringbuilder for display later
                fieldValue.Append($" {prefix}{commandInfo.Command.Name}");
                lastCommand = commandInfo;
            }

            // if the fieldValue stringbuilder has information present
            if (fieldValue.Length > 0)
            {
                // add the last collected module of commands to the Embed
                addField(lastCommand.Command, lastCommand.ModuleName, fieldValue);
            }
            builder.Footer = new EmbedFooterBuilder
            {
                Text = $"For more information on any command including description and parameters, type {prefix}help [command]"
            };
            await Context.User.SendMessageAsync(embed: builder.Build());
        }
        #endregion
    }
}
