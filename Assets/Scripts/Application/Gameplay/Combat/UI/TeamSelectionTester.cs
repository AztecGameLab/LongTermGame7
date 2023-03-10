using Application.Core;
using ImGuiNET;
using System.Collections.Generic;
using UnityEngine;

namespace Application.Gameplay.Combat.UI
{
    public class TeamSelectionTester : MonoBehaviour, IDebugImGui
    {
        [SerializeField] private TeamSelectionUI selectionUI;
        [SerializeField] private List<TeamMemberAuthoring> testingTeam;
        
        private void Start()
        {
            ImGuiUtil.Register(this);
            
            var teamData = new TeamData();

            foreach (TeamMemberAuthoring member in testingTeam)
            {
                teamData.unlockedMembers.Add(member.GenerateData());
            }
            
            selectionUI.BindTo(teamData);
            selectionUI.gameObject.SetActive(false);
        }

        private bool _selectorOpen;

        public void RenderImGui()
        {
            ImGui.Begin("Team Selection");
            
            if (ImGui.Checkbox("Open Team Selector", ref _selectorOpen))
            {
                selectionUI.gameObject.SetActive(_selectorOpen);
            }
            
            ImGui.End();
        }
    }
}