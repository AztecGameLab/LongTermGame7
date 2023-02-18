using ImGuiNET;

namespace Application.StateMachine
{
    public class PlayActionAnimation : RoundState
    {
        public override void OnEnter()
        {
            base.OnEnter();
            // choose another action when this animation finished, assume the method below is called correctly
        }

        protected override void DrawGui()
        {
            ImGui.Begin("Playing Action");
            
            if (ImGui.Button("Finish Action"))
                OnActionEnd();
            
            ImGui.End();
        }

        private void OnActionEnd()
        {
            BattleRound.StateMachine.SetState(BattleRound.EnemyMoveMonsters);
        }
    }
}