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
        private readonly Subject<Unit> _onEnter = new Subject<Unit>();
        private readonly Subject<Unit> _onExit = new Subject<Unit>();

        private IDebugImGui _debugImGui;
        private IDisposable _debugImGuiDisposable;

        /// <summary>
        /// Gets or sets the controller in charge of the logic for this battle.
        /// </summary>
        /// <value>
        /// The controller in charge of the logic for this battle.
        /// </value>
        public BattleController Controller { get; set; }

        /// <summary>
        /// Gets an observable for each time this state is entered.
        /// </summary>
        /// <returns>An observable.</returns>
        public IObservable<Unit> ObserveEntered() => _onEnter;

        /// <summary>
        /// Gets an observable for each time this state is entered.
        /// </summary>
        /// <returns>An observable.</returns>
        public IObservable<Unit> ObserveExited() => _onExit;

        /// <inheritdoc/>
        public virtual void OnEnter()
        {
            if (_debugImGui != null)
            {
                _debugImGuiDisposable = ImGuiUtil.Register(_debugImGui);
            }

            _onEnter.OnNext(Unit.Default);
        }

        /// <inheritdoc/>
        public virtual void OnExit()
        {
            _onExit.OnNext(Unit.Default);
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
