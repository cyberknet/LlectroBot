using System;
using System.Collections.Generic;
using System.Text;

namespace LlectroBot.Cards.LelectroBotCards
{
    public enum SetupStep
    {
        Welcome = 0,
        GuildCommandPrefix = 10,
        BotGuildAdministratorRole = 20,
        BotGuildSuperMemberRole = 30,
        BotGuildMemberRole = 40,
        BotGuildGuestRole = 50,
        AssignGuestRoleOnJoin = 60
    }
}
