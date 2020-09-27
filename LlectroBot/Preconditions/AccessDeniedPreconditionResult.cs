using Discord.Commands;
using LlectroBot.Roles;

namespace LlectroBot.Preconditions
{
    public class AccessDeniedPreconditionResult : PreconditionResult
    {
        public RoleLevel UserRole { get; set; }
        public RoleMatchType MatchType { get; set; }
        public RoleLevel RequiredRole { get; set; }

        public AccessDeniedPreconditionResult(RoleLevel userRole, RoleMatchType matchType, RoleLevel requiredRole)
            : base(CommandError.UnmetPrecondition, "Access Denied")
        {
            UserRole = userRole;
            MatchType = matchType;
            RequiredRole = requiredRole;
        }
    }
}
