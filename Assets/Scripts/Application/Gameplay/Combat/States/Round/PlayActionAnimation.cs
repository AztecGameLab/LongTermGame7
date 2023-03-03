using Application.Core;
using ImGuiNET;
using System;
using UniRx;

namespace Application.Gameplay.Combat.States.Round
{
    public class PlayActionAnimation : RoundState
    {
        private IDisposable _disposable;
        
        public override void OnEnter()
        {
            base.OnEnter();
            _disposable = Round.SelectedAction.Run().Subscribe(_ => OnActionEnd());
            Services.EventBus.Invoke(new RoundStateEnterEvent<PlayActionAnimation>{State = this}, "Play Action Animation State");
        }

        public override void OnExit()
        {
            base.OnExit();
            _disposable?.Dispose();
            Services.EventBus.Invoke(new RoundStateExitEvent<PlayActionAnimation>{State = this}, "Play Action Animation State");
        }

        private void OnActionEnd()
        {
            Round.StateMachine.SetState(Round.PickActions);
        }
        
        protected override void DrawGui()
        {
            ImGui.Begin("Playing Action");

            if (Round.SelectedAction is IDebugImGui debugImGui)
                debugImGui.RenderImGui();
            
            if (ImGui.Button("Finish Action"))
                OnActionEnd();
            
            ImGui.End();
        }
    }
}