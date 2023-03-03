using Application.Core;
using ImGuiNET;

namespace Application.Gameplay.Combat.States.Round
{
    public class EnemyMoveMonsters : RoundState
    {
        public override void OnEnter()
        {
            base.OnEnter();
            var controller = Round.Controller;
            
            Services.EventBus.Invoke(new RoundStateEnterEvent<EnemyMoveMonsters>{State = this}, "Enemy Move Monsters State");
            // controller.StartCoroutine(controller.Decider.ExecuteTurn(controller));
            
            // subscribe to when the decider finishes its stuff, and call below method
        }

        public override void OnExit()
        {
            base.OnExit();
            Services.EventBus.Invoke(new RoundStateExitEvent<EnemyMoveMonsters>{State = this}, "Enemy Move Monsters State");
        }

        private void OnDeciderFinish()
        {
            Round.StateMachine.SetState(Round.PickMonster);
        }
        
        protected override void DrawGui()
        {
            ImGui.Begin("Enemy Turn");
            
            if (ImGui.Button("Finish enemy turn"))
                OnDeciderFinish();
            
            ImGui.End();
        }
    }
}