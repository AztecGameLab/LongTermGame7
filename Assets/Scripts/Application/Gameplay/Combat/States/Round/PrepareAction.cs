using Application.Core;
using ImGuiNET;

namespace Application.Gameplay.Combat.States.Round
{
    public class PrepareAction : RoundState
    {
        public override void OnEnter()
        {
            base.OnEnter();
            Round.SelectedAction.PrepEnter();
            Services.EventBus.Invoke(new RoundStateEnterEvent<PrepareAction>{State = this}, "Prepare Action State");
        }

        public override void OnTick()
        {
            base.OnTick();
            
            if (Round.SelectedAction.PrepTick())
                Round.StateMachine.SetState(Round.PlayAnimation);
        }

        public override void OnExit()
        {
            base.OnExit();
            Round.SelectedAction.PrepExit();
            Services.EventBus.Invoke(new RoundStateExitEvent<PrepareAction>{State = this}, "Prepare Action State");
        }
        
        protected override void DrawGui()
        {
            ImGui.Begin("Preparing Action");

            if (Round.SelectedAction is IDebugImGui debugImGui)
                debugImGui.RenderImGui();

            if (ImGui.Button("Cancel Prep"))
                Round.StateMachine.SetState(Round.PickActions);
            
            ImGui.End();
        }
    }
}