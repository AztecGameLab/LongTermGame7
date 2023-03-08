namespace Application.Gameplay.Combat
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UniRx;
    using UnityEngine;

    /// <summary>
    /// Represents an action that a monster can take in a battle.
    /// Examples include moving, attacking, healing, and so on.
    /// </summary>
    [Serializable]
    public abstract class BattleAction
    {
        private List<IDisposable> _disposeOnExit = new List<IDisposable>();

        /// <summary>
        /// Gets the name of this move.
        /// </summary>
        /// <value>
        /// The name of this move.
        /// </value>
        public abstract string Name { get; }

        /// <summary>
        /// Gets the description of this move.
        /// </summary>
        /// <value>
        /// The description of this move.
        /// </value>
        public abstract string Description { get; }

        /// <summary>
        /// Gets or sets a value indicating whether gets whether the player has finished preparing this action.
        /// </summary>
        /// <value>
        /// A value indicating whether gets whether the player has finished preparing this action.
        /// </value>
        public bool IsPrepFinished { get; protected set; }

        /// <summary>
        /// Gets or sets the object currently using this action.
        /// <remarks>All actions depend on this being correctly set for their preparation and execution.
        /// It must be set prior to calling the "Prep" methods, or the "Execute" methods.</remarks>
        /// </summary>
        /// <value>
        /// The object currently using this action.
        /// </value>
        public GameObject User { get; set; }

        /// <summary>
        /// Allows this action to set itself up before running preparation logic.
        /// </summary>
        public virtual void PrepEnter()
        {
            IsPrepFinished = false;
        }

        /// <summary>
        /// Allows this action to clean up after its own preparation logic.
        /// For example, removing visual helpers that have been spawner.
        /// </summary>
        public virtual void PrepExit()
        {
            foreach (IDisposable disposable in _disposeOnExit)
            {
                disposable.Dispose();
            }

            _disposeOnExit.Clear();
        }

        /// <summary>
        /// Allows this action to run preparation logic. For example, letting the user
        /// aim the attack, or chose a position to move towards.
        /// </summary>
        public virtual void PrepTick()
        {
        }

        /// <summary>
        /// Run and observe the execution of this action.
        /// </summary>
        /// <returns>An observable that completes when the action finishes executing.</returns>
        public IObservable<Unit> Run() => Execute().ToObservable();

        /// <summary>
        /// Allows this action to perform whatever final logic sequence it needs.
        /// For example, launching a boulder and dealing damage, or walking to a new position.
        /// <remarks>The execution of an action will not be interrupted.</remarks>
        /// </summary>
        /// <returns>Unity coroutine information.</returns>
        protected abstract IEnumerator Execute();

        /// <summary>
        /// Registers a disposable to match the lifetime of the state.
        /// Hence, when this state exits, this disposable will be disposed.
        /// </summary>
        /// <param name="disposable">The disposable to track.</param>
        protected void DisposeOnExit(IDisposable disposable)
        {
            _disposeOnExit.Add(disposable);
        }
    }
}
