using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Cards;
using Discord.Cards.Cards.PrimitiveCards;
using Discord.Cards.PrimitiveCards;
using LlectroBot.Core.Cards;
using LlectroBot.Roles;

namespace LlectroBot.Guild.Cards
{
    public class SetupCard : CardBase<bool>
    {
        private LlectroBotCardContext LlectroBotContext => Context as LlectroBotCardContext;

        public SetupStep ActiveSetupStep = SetupStep.Welcome;
        public InputStringCard InputStringCard { get; set; }
        public InputRoleCard InputRoleCard { get; set; }
        public InputBooleanCard InputBooleanCard { get; set; }

        public SetupCard(string message, bool isCancellable, string[] cancelWords = null, string retryMessage = null)
            : base(message, isCancellable, cancelWords, retryMessage)
        {

        }
        public override async Task<CardResult> MessageReceived(IMessage message, CardResult lastCardResult = CardResult.None)
        {
            switch (ActiveSetupStep)
            {
                case SetupStep.Welcome:
                    await SendWelcomeMessage();
                    return await RequestGuildCommandPrefix(message);
                case SetupStep.GuildCommandPrefix:
                    SaveGuildCommandPrefix();
                    return await RequestGuildRole(
                        SetupStep.BotGuildAdministratorRole,
                        "What role would you like to use for the Server Administrator?",
                        "What role would you like to use?",
                        message);
                case SetupStep.BotGuildAdministratorRole:
                    SaveGuildRole(RoleLevel.GuildAdministrator);
                    return await RequestGuildRole(
                        SetupStep.BotGuildSuperMemberRole,
                        "What role would you like to use for Super Members?",
                        "What role would you like to use?",
                        message);
                case SetupStep.BotGuildSuperMemberRole:
                    SaveGuildRole(RoleLevel.SuperMember);
                    return await RequestGuildRole(
                        SetupStep.BotGuildMemberRole,
                        "What role would you like to use for Members?",
                        "What role would you like to use?",
                        message);
                case SetupStep.BotGuildMemberRole:
                    SaveGuildRole(RoleLevel.Member);
                    return await RequestGuildRole(
                        SetupStep.BotGuildGuestRole,
                        "What role would you like to use for Guests?",
                        "What role would you like to use?",
                        message);
                case SetupStep.BotGuildGuestRole:
                    SaveGuildRole(RoleLevel.Guest);
                    return await RequestAssignGuestRoleOnJoin(message);
                case SetupStep.AssignGuestRoleOnJoin:
                    SaveAssignGuestRoleOnJoin();
                    return await SetupComplete();
            }
            return CardResult.Continue;
        }

        private async Task SendWelcomeMessage()
        {
            await ReplyAsync($"Welcome to LlectroBot Setup. I have a few quick questions for you before I am ready for use.");
        }
        private async Task<CardResult> RequestGuildCommandPrefix(IMessage message)
        {
            ActiveSetupStep = SetupStep.GuildCommandPrefix;
            InputStringCard = new InputStringCard("What character would you like to use for your command prefix?", true, 1, new string[] { "cancel" }, "What character would you like to use?");
            await CardStack.ShowCard(InputStringCard, message, Context);
            return CardResult.Continue;
        }
        private void SaveGuildCommandPrefix()
        {
            LlectroBotContext.GuildConfiguration.CommandPrefix = InputStringCard.Result[0];
            LlectroBotContext.BotConfiguration.Save();
            InputStringCard = null;
        }

        private async Task<CardResult> RequestGuildRole(SetupStep nextSetupStep, string question, string retryQuestion, IMessage message)
        {
            ActiveSetupStep = nextSetupStep;
            InputRoleCard = new InputRoleCard(question, true, new string[] { "cancel" }, retryQuestion);
            await CardStack.ShowCard(InputRoleCard, message, Context);
            return CardResult.Continue;
        }
        private void SaveGuildRole(RoleLevel roleLevel)
        {
            var existingRole = LlectroBotContext.GuildConfiguration.Roles.FirstOrDefault(
                r => r.Level == roleLevel);
            if (existingRole != null)
            {
                existingRole.Value = InputRoleCard.Result;
            }
            else
            {
                var role = new LlectroBotRole(roleLevel, InputRoleCard.Result);
                LlectroBotContext.GuildConfiguration.Roles.Add(role);
                LlectroBotContext.BotConfiguration.Save();
            }

            InputRoleCard = null;
        }

        private async Task<CardResult> RequestAssignGuestRoleOnJoin(IMessage message)
        {
            ActiveSetupStep = SetupStep.AssignGuestRoleOnJoin;
            InputBooleanCard = new InputBooleanCard("Should I assign a Guest role to users when they join? (yes|no)", true, new string[] { "cancel" }, "Should I auto-assign a guest role?", new string[] { "yes" }, new string[] { "no" });
            await CardStack.ShowCard(InputBooleanCard, message, Context);
            return CardResult.Continue;
        }

        private void SaveAssignGuestRoleOnJoin()
        {
            LlectroBotContext.GuildConfiguration.AssignGuestRoleOnJoin = InputBooleanCard.Result;
            InputBooleanCard = null;
        }

        private async Task<CardResult> SetupComplete()
        {
            await ReplyAsync("Setup is complete!");
            return CardResult.CloseAndContinue;
        }

    }
}
