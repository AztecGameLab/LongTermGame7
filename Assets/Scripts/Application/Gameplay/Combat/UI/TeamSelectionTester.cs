using Application.Core;
using ImGuiNET;
using UnityEngine;

namespace Application.Gameplay.Combat.UI
{
    public class TeamSelectionTester : MonoBehaviour, IDebugImGui
    {
        [SerializeField] private TeamSelectionUI selectionUI;
        [SerializeField] private TeamData teamData;
        
        private void Start()
        {
            ImGuiUtil.Register(this);
            selectionUI.BindTo(teamData);
            selectionUI.gameObject.SetActive(false);
        }

        public void RenderImGui()
        {
            ImGui.Begin("Team Selection");
            
            if (ImGui.Button("Open Team Selector"))
            {
                selectionUI.gameObject.SetActive(true);
            }
            
            ImGui.End();
        }
    }
}