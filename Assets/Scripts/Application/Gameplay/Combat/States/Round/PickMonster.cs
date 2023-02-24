using ImGuiNET;
using System.Collections.Generic;
using UnityEngine;

namespace Application.Gameplay.Combat.States.Round
{
    public class PickMonster : RoundState
    {
        private readonly List<GameObject> _availableMonsters = new List<GameObject>();
        
        private GameObject SelectedMonster => _availableMonsters.Count > _selectedMonsterIndex 
            ? _availableMonsters[_selectedMonsterIndex] 
            : null;
        
        private int _selectedMonsterIndex;
        
        public override void OnRoundBegin()
        {
            _availableMonsters.AddRange(Round.Controller.PlayerTeam);
            _selectedMonsterIndex = 0;
        }

        public override void OnRoundEnd()
        {
            _availableMonsters.Clear();
        }

        protected override void DrawGui()
        {
            ImGui.Begin("Pick Monster");

            if (_availableMonsters.Count <= 0)
            {
                ImGui.Text("No monsters left to move! You must end the round.");
                
                if (ImGui.Button("Next Round"))
                    Round.Controller.BattleStateMachine.SetState(Round.Controller.BattleRound);
            }

            ImGui.Text("Available Monsters:");
            
            foreach (GameObject availableMonster in _availableMonsters)
            {
                ImGui.Text($"\t{availableMonster.name}");
            }
            
            ImGui.Text($"Currently selected: {(SelectedMonster == null ? "None" : SelectedMonster.name)}");

            if (SelectedMonster != null && ImGui.Button($"Select {SelectedMonster.name}"))
                OnSelectMonster(SelectedMonster);

            if (ImGui.Button("Next Monster"))
            {
                if (_availableMonsters.Count > 0)
                    _selectedMonsterIndex = (_selectedMonsterIndex + 1) % _availableMonsters.Count;
            }
            
            ImGui.End();
        }

        private void OnSelectMonster(GameObject monster)
        {
            _availableMonsters.Remove(monster);
            _selectedMonsterIndex = 0;
            
            Round.SelectedMonster = monster;
            Round.StateMachine.SetState(Round.PickActions);
        }
    }
}