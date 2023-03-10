namespace Application.Gameplay.Combat.UI
{
    using System.Collections.Generic;
    using Core;
    using Core.Serialization;
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
        private const string SaveName = "player_team.json";

        [SerializeField]
        private TeamSelectionUI selectionUI;

        [SerializeField]
        private List<TeamMemberAuthoring> testingTeam;

        [SerializeField]
        private TeamMemberAuthoring testingPlayer;

        private TeamData _teamData;

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
                Serializer.Save(SaveName, _teamData);
            }

            ImGui.End();
        }

        private void Start()
        {
            ImGuiUtil.Register(this);

            if (!Serializer.TryLoad(SaveName, out _teamData))
            {
                _teamData = new TeamData { Player = testingPlayer.GenerateData() };

                foreach (TeamMemberAuthoring member in testingTeam)
                {
                    _teamData.UnlockedMembers.Add(member.GenerateData());
                }
            }

            selectionUI.BindTo(_teamData);
            selectionUI.gameObject.SetActive(false);

            Services.PlayerTeamData = _teamData;
            Services.EventBus.AddListener<OpenTeamSelectorCommand>(_ => OpenTeamSelector(), "Team Data Loader").AddTo(this);
        }

        private void OpenTeamSelector()
        {
            selectionUI.gameObject.SetActive(true);
        }
    }
}
