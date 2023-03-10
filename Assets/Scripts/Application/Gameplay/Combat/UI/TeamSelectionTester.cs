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
    public class TeamSelectionTester : MonoBehaviour, IDebugImGui
    {
        private const string SaveName = "player_team.json";

        [SerializeField]
        private TeamSelectionUI selectionUI;

        [SerializeField]
        private List<TeamMemberAuthoring> testingTeam;

        private bool _selectorOpen;
        private TeamData _teamData;

        /// <inheritdoc/>
        public void RenderImGui()
        {
            ImGui.Begin("Team Selection");

            if (ImGui.Checkbox("Open Team Selector", ref _selectorOpen))
            {
                selectionUI.gameObject.SetActive(_selectorOpen);
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
                _teamData = new TeamData();

                foreach (TeamMemberAuthoring member in testingTeam)
                {
                    _teamData.UnlockedMembers.Add(member.GenerateData());
                }
            }

            selectionUI.BindTo(_teamData);
            selectionUI.gameObject.SetActive(false);
        }
    }
}
