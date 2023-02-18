using ImGuiNET;
using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Application.StateMachine
{
    public class PickMonster : RoundState
    {
        public override void OnEnter()
        {
            base.OnEnter();
            var partyView = Object.FindObjectOfType<PlayerPartyView>();
            
            // now we can enumerate partyView.PlayerPartyInstances, and go from there to
            // determine the monster being chosen.
            
            // we would probably want to load a UI and quit in response to a button / input, so assume OnSelectMonster is bound to that.
        }

        protected override void DrawGui()
        {
            ImGui.Begin("Pick Monster");
            
            if (ImGui.Button("Select Monster"))
                OnSelectMonster(null);
            
            ImGui.End();
        }

        private void OnSelectMonster(GameObject monster)
        {
            BattleRound.SelectedMonster = monster;
            BattleRound.StateMachine.SetState(BattleRound.PickActions);
        }
    }
}