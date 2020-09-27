using System;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using LlectroBot.Context;
using LlectroBot.Roles;

namespace LlectroBot.Preconditions
{
    public class RequireRoleAttribute : PreconditionAttribute
    {
        public RoleLevel Level { get; private set; }
        public RoleMatchType MatchType { get; private set; }
        public RequireRoleAttribute(RoleLevel level, RoleMatchType matchType)
        {
            Level = level;
            MatchType = matchType;
        }

        private bool Matches(RoleLevel role)
        {
            return MatchType switch
            {
                RoleMatchType.LessThanOrequal => role <= Level,
                RoleMatchType.LessThan => role < Level,
                RoleMatchType.Equal => role == Level,
                RoleMatchType.GreaterThan => role > Level,
                RoleMatchType.GraterThanOrEqual => role >= Level,
                RoleMatchType.NotEqualTo => role != Level,
                _ => false,
            };
        }

        public override async Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            if (context is DiscordCommandContext discordContext)
            {
                RoleLevel role = discordContext.GuildUser.GetRole(discordContext.GuildConfiguration);
                if (Matches(role))
                {
                    return PreconditionResult.FromSuccess();
                }

                return new AccessDeniedPreconditionResult(role, MatchType, Level);
            }
            else
            {
                await Task.FromResult(true);
                throw new InvalidOperationException($"{nameof(RequireRoleAttribute)} is only valid on type {nameof(DiscordCommandContext)}.");
            }
        }
    }
}
