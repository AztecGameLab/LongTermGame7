namespace Application.Gameplay.Combat.States
{
    using System;
    using Core;
    using Core.Utility;

    /// <summary>
    /// A state of the round in a battle.
    /// The round is the part where players and enemies take turns performing actions.
    /// </summary>
    [Serializable]
    public abstract class RoundState : IState
    {
        private IDisposable _debugImGuiDisposable;
        private IDebugImGui _debugImGui;

        /// <summary>
        /// Gets or sets the current round this state is taking place in.
        /// </summary>
        /// <value>
        /// The current round this state is taking place in.
        /// </value>
        public BattleRound Round { get; set; }

        /// <summary>
        /// Called the first frame this state is entered.
        /// An opportunity to prepare and cache information.
        /// </summary>
        public virtual void OnEnter()
        {
            if (_debugImGui != null)
            {
                _debugImGuiDisposable = ImGuiUtil.Register(_debugImGui);
            }
        }

        /// <summary>
        /// Called the first frame this state is exited.
        /// An opportunity to clean up after yourself.
        /// </summary>
        public virtual void OnExit()
        {
            _debugImGuiDisposable?.Dispose();
        }

        /// <summary>
        /// Called regularly while this state is active.
        /// An opportunity to perform frequent logic and updates.
        /// </summary>
        public virtual void OnTick()
        {
        }

        /// <summary>
        /// Called the first frame this round begins.
        /// A round begins directly after the previous one ends.
        /// </summary>
        public virtual void OnRoundBegin()
        {
        }

        /// <summary>
        /// Called the first frame this round finishes.
        /// A round finishes when all monsters from the player have been used.
        /// </summary>
        public virtual void OnRoundEnd()
        {
        }

        /// <summary>
        /// Utility method for quickly registering some ImGui debug information,
        /// and automatically disposing it when the state exits.
        /// </summary>
        /// <param name="debugImGui">The class used to draw ImGui information.</param>
        protected void RegisterImGuiDebug(IDebugImGui debugImGui)
        {
            _debugImGui = debugImGui;
        }
    }
}
