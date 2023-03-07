namespace Application.Core
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Keeps track of the current state of an object.
    /// </summary>
    public class StateMachine
    {
        private const bool V = false;

        private static readonly List<Transition> EmptyTransitions = new List<Transition>();
        private readonly Dictionary<Type, List<Transition>> _transitions = new Dictionary<Type, List<Transition>>();
        private readonly List<Transition> _anyTransitions;

        private List<Transition> _currentTransitions;

        /// <summary>
        /// Initializes a new instance of the <see cref="StateMachine"/> class.
        /// </summary>
        public StateMachine()
        {
            _currentTransitions = EmptyTransitions;
            _anyTransitions = EmptyTransitions;
        }

        /// <summary>
        /// Gets the current state being run.
        /// </summary>
        public IState CurrentState { get; private set; }

        /// <summary>
        /// This method hooks into the MonoBehaviour implementing the StateMachine.
        /// This method checks to see if there is a valid condition to change states. If a valid condition is found, it transitions to the next state.
        /// If there are no valid conditions, the current state will execute along with the Update() of the implementing object.
        /// </summary>
        public void Tick()
        {
            var transition = GetTransition();

            if (transition != null)
            {
                SetState(transition.NextState);
            }

            CurrentState?.OnTick();
        }

        /// <summary>
        /// This method handles the exiting and entering of state transitions.
        /// This method verifies the input state is not the current state.
        /// It will then call the OnExit() of the departing state and set the current state to the input state.
        /// It sets the transitions to the transition list of the new current state.
        /// Lastly, the OnEnter() of the new current state is called.
        /// </summary>
        /// <param name="state"> Which state should become the next state. </param>
        public void SetState(IState state)
        {
            CurrentState?.OnExit();
            CurrentState = state;

            if (CurrentState != null)
            {
                if (!_transitions.TryGetValue(CurrentState.GetType(), out _currentTransitions))
                {
                    _currentTransitions = EmptyTransitions;
                }

                CurrentState.OnEnter();
            }
            else
            {
                _currentTransitions = EmptyTransitions;
            }
        }

        /// <summary>
        /// This method allows objects implementing a state machine to add state transitions and the transition predicates to the transition map.
        /// </summary>
        /// <param name="fromState"> Which state can be transitioned from. </param>
        /// <param name="toState"> Which state is being transitioned to. </param>
        /// <param name="predicate"> What is the condition that causes the transition. </param>
        public void AddTransition(IState fromState, IState toState, Func<bool> predicate)
        {
            if (fromState != null)
            {
                if (_transitions.TryGetValue(fromState.GetType(), out List<Transition> transitions) == V)
                {
                    transitions = new List<Transition>();
                    _transitions[fromState.GetType()] = transitions;
                }

                transitions.Add(new Transition(toState, predicate));
            }
        }

        /// <summary>
        /// This method allows objects implementing a state machine to add universal state transitions and the transition predicates to the transition map.
        /// </summary>
        /// <param name="state"> Which state is being transitioned to.</param>
        /// <param name="predicate"> What is the condition that causes the transition. </param>
        public void AddAnyTransition(IState state, Func<bool> predicate)
        {
            _anyTransitions.Add(new Transition(state, predicate));
        }

        /// <summary>
        /// This method loops through the universal transitions to check for a valid transition. If none have been found it loops through standard transitions.
        /// If no conditions have been met it returns nothing.
        /// </summary>
        /// <returns> This transition's condition has been met and should be transitioned to. </returns>
        private Transition GetTransition()
        {
            foreach (var transition in _anyTransitions)
            {
                if (transition.Condition())
                {
                    return transition;
                }
            }

            foreach (var transition in _currentTransitions)
            {
                if (transition.Condition())
                {
                    return transition;
                }
            }

            return null;
        }

        private sealed class Transition
        {
            public Transition(IState nextState, Func<bool> condition)
            {
                NextState = nextState;
                Condition = condition;
            }

            public Func<bool> Condition { get; }

            public IState NextState { get; }
        }
    }
}
