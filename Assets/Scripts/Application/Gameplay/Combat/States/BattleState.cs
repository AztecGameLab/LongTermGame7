namespace Application.Gameplay.Combat.States
{
    using System;
    using Core;
    using Core.Utility;
    using UniRx;

    /// <summary>
    /// A state of the current battle.
    /// </summary>
    public abstract class BattleState : IState
    {
        private readonly Subject<Unit> _onEnter = new Subject<Unit>();
        private readonly Subject<Unit> _onExit = new Subject<Unit>();

        private IDebugImGui _debugImGui;
        private IDisposable _debugImGuiDisposable;
        private CompositeDisposable _disposable = new CompositeDisposable();

        /// <summary>
        /// Gets or sets the controller in charge of the logic for this battle.
        /// </summary>
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
            _disposable?.Dispose();
            _disposable = new CompositeDisposable();
        }

        /// <inheritdoc/>
        public virtual void OnTick()
        {
        }

        /// <summary>
        /// Registers a disposable to match the lifetime of the state.
        /// Hence, when this state exits, this disposable will be disposed.
        /// </summary>
        /// <param name="disposable">The disposable to track.</param>
        protected void DisposeOnExit(IDisposable disposable)
        {
            _disposable.Add(disposable);
        }

        /// <summary>
        /// Registers an ImGui debugging window with the lifetime of this state.
        /// </summary>
        /// <param name="debugImGui">The instance for rendering ImGui info.</param>
        protected void RegisterDebugImGui(IDebugImGui debugImGui)
        {
            _debugImGui = debugImGui;
        }
    }
}
