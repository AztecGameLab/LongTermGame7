namespace Application.Gameplay.Combat.States
{
    using System;
    using Core;
    using Core.Utility;

    /// <summary>
    /// A state of the current battle.
    /// </summary>
    public abstract class BattleState : IState
    {
        private IDebugImGui _debugImGui;
        private IDisposable _debugImGuiDisposable;

        /// <summary>
        /// Gets or sets the controller in charge of the logic for this battle.
        /// </summary>
        /// <value>
        /// The controller in charge of the logic for this battle.
        /// </value>
        public BattleController Controller { get; set; }

        /// <inheritdoc/>
        public virtual void OnEnter()
        {
            if (_debugImGui != null)
            {
                _debugImGuiDisposable = ImGuiUtil.Register(_debugImGui);
            }
        }

        /// <inheritdoc/>
        public virtual void OnExit()
        {
            _debugImGuiDisposable?.Dispose();
        }

        /// <inheritdoc/>
        public virtual void OnTick()
        {
        }

        protected void RegisterDebugImGui(IDebugImGui debugImGui)
        {
            _debugImGui = debugImGui;
        }
    }
}
