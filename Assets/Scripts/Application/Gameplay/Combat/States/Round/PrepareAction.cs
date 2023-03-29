namespace Application.Gameplay.Combat.States.Round
{
    using System;
    using Core;
    using ImGuiNET;
    using UnityEngine;

    /// <summary>
    /// The battle round state where you give the final additional data to the action before it is performed.
    /// </summary>
    [Serializable]
    public class PrepareAction : RoundState, IDebugImGui
    {
        /// <summary>
        /// Sets up the prepare action state.
        /// </summary>
        public void Initialize()
        {
            RegisterImGuiDebug(this);
        }

        /// <inheritdoc/>
        public override void OnEnter()
        {
            base.OnEnter();
            Round.PickActions.SelectedAction.PrepEnter();
        }

        /// <inheritdoc/>
        public override void OnTick()
        {
            base.OnTick();

            Round.PickActions.SelectedAction.PrepTick();

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Round.TransitionTo(Round.PickActions);
            }

            if (Round.PickActions.SelectedAction.IsPrepFinished)
            {
                Round.TransitionTo(Round.PlayActionAnimation);
            }
        }

        /// <inheritdoc/>
        public override void OnExit()
        {
            base.OnExit();
            Round.PickActions.SelectedAction.PrepExit();
        }

        /// <inheritdoc/>
        public void RenderImGui()
        {
            ImGui.Begin("Preparing Action");

            if (Round.PickActions.SelectedAction is IDebugImGui debugImGui)
            {
                debugImGui.RenderImGui();
            }

            if (ImGui.Button("Cancel Prep"))
            {
                Round.TransitionTo(Round.PickActions);
            }

            ImGui.End();
        }
    }
}
