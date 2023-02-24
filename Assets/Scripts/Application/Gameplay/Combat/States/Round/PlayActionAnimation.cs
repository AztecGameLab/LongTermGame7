using Application.Gameplay.Combat;
using ImGuiNET;
using System;
using UniRx;

namespace Application.StateMachine
{
    public class PlayActionAnimation : RoundState
    {
        private IDisposable _disposable;
        
        public override void OnEnter()
        {
            base.OnEnter();
            
            _disposable = Round.SelectedAction.Run(Round.SelectedMonster)
                .Subscribe(_ => OnActionEnd());
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

        private void OnActionEnd()
        {
            Round.StateMachine.SetState(Round.PickActions);
            _disposable?.Dispose();
        }
    }
}