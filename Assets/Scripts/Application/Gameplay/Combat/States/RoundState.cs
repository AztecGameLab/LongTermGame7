namespace Application.Gameplay.Combat.States
{
    using System;
    using Core;
    using Core.Utility;
    using UniRx;

    /// <summary>
    /// A state of the round in a battle.
    /// The round is the part where players and enemies take turns performing actions.
    /// </summary>
    [Serializable]
    public abstract class RoundState : IState
    {
        private readonly Subject<Unit> _entered = new Subject<Unit>();
        private readonly Subject<Unit> _exited = new Subject<Unit>();

        private IDebugImGui _debugImGui;
        private CompositeDisposable _disposables = new CompositeDisposable();

        /// <summary>
        /// Gets or sets the current round this state is taking place in.
        /// </summary>
        public BattleRound Round { get; set; }

        /// <summary>
        /// Gets an observable for each time this state is entered.
        /// </summary>
        /// <returns>An observable.</returns>
        public IObservable<Unit> ObserveEntered() => _entered;

        /// <summary>
        /// Gets an observable for each time this state is exited.
        /// </summary>
        /// <returns>An observable.</returns>
        public IObservable<Unit> ObserveExited() => _exited;

        /// <summary>
        /// Called the first frame this state is entered.
        /// An opportunity to prepare and cache information.
        /// </summary>
        public virtual void OnEnter()
        {
            if (_debugImGui != null)
            {
                DisposeOnExit(ImGuiUtil.Register(_debugImGui));
            }

            _entered.OnNext(Unit.Default);
        }

        /// <summary>
        /// Called the first frame this state is exited.
        /// An opportunity to clean up after yourself.
        /// </summary>
        public virtual void OnExit()
        {
            _exited.OnNext(Unit.Default);

            _disposables?.Dispose();
            _disposables = new CompositeDisposable();
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
        /// Register a disposable to be cleaned up when this state exits.
        /// </summary>
        /// <param name="disposable">The disposable to remove on state exit.</param>
        protected void DisposeOnExit(IDisposable disposable)
        {
            _disposables.Add(disposable);
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
