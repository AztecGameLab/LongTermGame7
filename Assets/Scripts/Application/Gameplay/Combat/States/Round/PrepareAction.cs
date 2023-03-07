﻿namespace Application.Gameplay.Combat.States.Round
{
    using System;
    using Core;
    using ImGuiNET;

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
            Services.EventBus.Invoke(new RoundStateEnterEvent<PrepareAction> { State = this }, "Prepare Action State");
        }

        /// <inheritdoc/>
        public override void OnTick()
        {
            base.OnTick();

            Round.PickActions.SelectedAction.PrepTick();

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
            Services.EventBus.Invoke(new RoundStateExitEvent<PrepareAction> { State = this }, "Prepare Action State");
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
