namespace Application.Gameplay
{
    using System.Collections.Generic;
    using Combat.UI;
    using Core;
    using Core.Utility;
    using ImGuiNET;
    using UnityEngine;

    /// <summary>
    /// A testing script for evaluating the team selection system.
    /// Creates or loads team data from a file, and binds the selection UI to it.
    /// In a real scenario, we'd probably want to do this in the main menu before
    /// starting up gameplay.
    /// </summary>
    public class TeamDataLoader : MonoBehaviour, IDebugImGui
    {
        private const string TeamDataID = "player_team";

        [SerializeField]
        private TeamSelectionUI selectionUI;

        [SerializeField]
        private List<TeamMemberAuthoring> testingTeam;

        [SerializeField]
        private TeamMemberAuthoring testingPlayer;

        /// <inheritdoc/>
        public void RenderImGui()
        {
            ImGui.Begin("Team Selection");

            if (ImGui.Button("Open Team Selector"))
            {
                Services.EventBus.Invoke(new OpenTeamSelectorCommand(), "ImGui Team Selection Button");
            }

            if (ImGui.Button("Save"))
            {
                Services.Serializer.Store(TeamDataID, Services.PlayerTeamData);
            }

            ImGui.End();
        }

        private void Awake()
        {
            ImGuiUtil.Register(this);

            if (!Services.Serializer.Exists(TeamDataID))
            {
                Services.PlayerTeamData = new TeamData { Player = testingPlayer.GenerateData() };

                foreach (TeamMemberAuthoring member in testingTeam)
                {
                    Services.PlayerTeamData.UnlockedMembers.Add(member.GenerateData());
                }
            }
            else
            {
                Services.Serializer.Lookup(TeamDataID, out TeamData data);
                Services.PlayerTeamData = data;
            }

            selectionUI.BindTo(Services.PlayerTeamData);
            selectionUI.gameObject.SetActive(false);

            Services.EventBus.AddListener<OpenTeamSelectorCommand>(_ => OpenTeamSelector(), "Team Data Loader").AddTo(this);
        }

        private void OpenTeamSelector()
        {
            selectionUI.gameObject.SetActive(true);
        }
    }
}
