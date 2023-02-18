using ImGuiNET;

namespace Application.StateMachine
{
    public class EnemyMoveMonsters : RoundState
    {
        public override void OnEnter()
        {
            base.OnEnter();
            var controller = BattleRound.Controller;
            // controller.StartCoroutine(controller.Decider.ExecuteTurn(controller));
            
            // subscribe to when the decider finishes its stuff, and call below method
        }

        protected override void DrawGui()
        {
            ImGui.Begin("Enemy Turn");
            
            if (ImGui.Button("Finish enemy turn"))
                OnDeciderFinish();
            
            ImGui.End();
        }

        private void OnDeciderFinish()
        {
            BattleRound.StateMachine.SetState(BattleRound.PickMonster);
        }
    }
}