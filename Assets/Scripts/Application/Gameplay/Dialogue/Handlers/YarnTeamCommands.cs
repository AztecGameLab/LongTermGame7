namespace Application.Gameplay.Dialogue.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Combat.UI;
    using Core;
    using UnityEngine;
    using Yarn.Unity;

    /// <summary>
    /// Yarn commands for interacting with the player team data.
    /// </summary>
    [Serializable]
    public class YarnTeamCommands : IYarnCommandHandler
    {
        private const string TargetSelected = "selected";
        private const string TargetUnlocked = "unlocked";

        [SerializeField]
        private DictionaryGenerator<string, TeamMemberAuthoring> teamMemberIds;

        private Dictionary<string, TeamMemberAuthoring> _teamMemberLookup;

        /// <inheritdoc/>
        public void RegisterCommands(DialogueRunner runner)
        {
            _teamMemberLookup = teamMemberIds.GenerateDictionary();

            runner.AddCommandHandler("team-open-selector", HandleOpenSelector);
            runner.AddCommandHandler<string, string>("team-add", HandleAdd);
            runner.AddCommandHandler<string, string>("team-remove", HandleRemove);
        }

        /// <inheritdoc/>
        public void UnregisterCommands(DialogueRunner runner)
        {
            runner.RemoveCommandHandler("team-open-selector");
            runner.RemoveCommandHandler("team-add");
            runner.RemoveCommandHandler("team-remove");
        }

        private static void HandleOpenSelector()
        {
            Services.EventBus.Invoke(new OpenTeamSelectorCommand(), "Yarn Team Commands");
        }

        private IList<TeamMemberData> GetList(string target)
        {
            TeamData data = Services.PlayerTeamData;

            return target switch
            {
                TargetSelected => data.SelectedMembers,
                TargetUnlocked => data.UnlockedMembers,
                _ => Array.Empty<TeamMemberData>(),
            };
        }

        private void HandleAdd(string arg, string target = "")
        {
            if (target == string.Empty)
            {
                target = TargetSelected;
            }

            IList<TeamMemberData> list = GetList(target);

            TeamMemberAuthoring member = _teamMemberLookup[arg];
            list.Add(member.GenerateData());
        }

        private void HandleRemove(string arg, string target = "")
        {
            if (target == string.Empty)
            {
                HandleRemove(arg, TargetUnlocked);
                HandleRemove(arg, TargetSelected);
            }
            else
            {
                IList<TeamMemberData> list = GetList(target);
                TeamMemberAuthoring member = _teamMemberLookup[arg];
                TeamMemberData memberToRemove = list.FirstOrDefault(data => data.Name == member.MemberName);
                list.Remove(memberToRemove);
            }
        }
    }
}
