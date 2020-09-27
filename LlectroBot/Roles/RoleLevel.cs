using System.ComponentModel;

namespace LlectroBot.Roles
{
    public enum RoleLevel
    {
        [Description("Default Discord Role")]
        None = 0,
        [Description("Guest")]
        Guest = 20,
        [Description("Member")]
        Member = 40,
        [Description("Super Member")]
        SuperMember = 60,
        [Description("Guild Administrator")]
        GuildAdministrator = 100,
        [Description("Global Administrator")]
        GlobalAdministrator = 1000
    }
}
