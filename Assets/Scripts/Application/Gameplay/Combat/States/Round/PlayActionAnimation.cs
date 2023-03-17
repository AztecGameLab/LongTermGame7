namespace Application.Gameplay.Combat.States.Round
{
    using System;
    using Core;
    using ImGuiNET;
    using UniRx;

    /// <summary>
    /// The battle round state where an animation for an action is playing.
    /// </summary>
    [Serializable]
    public class PlayActionAnimation : RoundState, IDebugImGui
    {
        /// <summary>
        /// Sets up the play action animation state.
        /// </summary>
        public void Initialize()
        {
            RegisterImGuiDebug(this);
        }

        /// <inheritdoc/>
        public override void OnEnter()
        {
            base.OnEnter();
            Round.PickActions.SelectedAction.Run().Subscribe(_ => OnActionEnd());
        }

        /// <inheritdoc/>
        public void RenderImGui()
        {
            ImGui.Begin("Playing Action");

            if (Round.PickActions.SelectedAction is IDebugImGui debugImGui)
            {
                debugImGui.RenderImGui();
            }

            if (ImGui.Button("Finish Action"))
            {
                OnActionEnd();
            }

            ImGui.End();
        }

        private void OnActionEnd()
        {
            if (Round.Controller.IsBattling)
            {
                Round.TransitionTo(Round.PickActions);
            }
        }
    }
}
