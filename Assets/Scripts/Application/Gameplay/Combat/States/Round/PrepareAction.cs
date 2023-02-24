using ImGuiNET;

namespace Application.Gameplay.Combat.States.Round
{
    public class PrepareAction : RoundState
    {
        public override void OnEnter()
        {
            base.OnEnter();
            Round.SelectedAction.PrepEnter();
        }

        public override void OnTick()
        {
            base.OnTick();
            
            if (Round.SelectedAction.PrepTick())
                Round.StateMachine.SetState(Round.PlayAnimation);
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

        public override void OnExit()
        {
            base.OnExit();
            Round.SelectedAction.PrepExit();
        }
    }
}