using System.Collections.Generic;
using UnityEngine;

namespace Application.Core
{
    /// <summary>
    /// A serialized representation of an Event.
    /// </summary>
    /// <typeparam name="TData">The data that should be passed with the event.</typeparam>
    public class Event<TData> : ScriptableObject
    {
        public delegate void Listener(TData data);

        // All listeners to this event. Sorted by priority as an int.
        private SortedList<int, List<Listener>> _listeners = new SortedList<int, List<Listener>>();
        
        // todo: these are literally only used for debugging: maybe strip out for final builds (preprocessor stuff)?
        private IRichTextData _listenerFormat = Rtf.Composite(Rtf.Color(Color.red));
        private IRichTextData _eventFormat = Rtf.Composite(Rtf.Color(Color.cyan));
        private int _listenerCount;

        /// <summary>
        /// Register an function that will be called in response to <see cref="Invoke"/>.
        /// </summary>
        /// <seealso cref="Listener"/>
        /// <seealso cref="IDebugDisposable"/>
        /// <param name="action">The function that will be called.</param>
        /// <param name="debugId">A human-friendly ID for whatever is subscribing to this event.</param>
        /// <param name="priority">Determines the order in which listeners are called:
        /// higher priority listeners are called before lower priority listeners.</param>
        /// <remarks>IMPORTANT: Remember to call <see cref="RemoveListener"/> once you are done!
        /// If you don't, there may be errors flooding the console when it tries to call a function on 
        /// an object that no longer exists.</remarks>
        public void AddListener(Listener action, string debugId, int priority = 0)
        {
            if (_listeners.ContainsKey(priority) == false)
            {
                _listeners.Add(priority, new List<Listener>());
            }
            
            _listeners[priority].Add(action);
            _listenerCount++;
            
            Debug.Log($"{debugId.Format(_listenerFormat)} started listening to " +
                      $"{name.Format(_eventFormat)} with priority {priority.ToString().Format(_eventFormat)}.");
        }

        /// <summary>
        /// Removes a function from being called in response to <see cref="Invoke"/>
        /// </summary>
        /// <param name="action">The function that will be removed.</param>
        /// <param name="debugId">A human-friendly ID for whatever is removing this listener.</param>
        /// <param name="debugReason">A human-friendly reason for why this listener is being removed.</param>
        public void RemoveListener(Listener action, string debugId, string debugReason = "None")
        {
            Debug.Log($"{debugId.Format(_listenerFormat)} stopped listening to " +
                      $"{name.Format(_eventFormat)} for reason: \"{debugReason.Italic()}\".");

            foreach (List<Listener> listeners in _listeners.Values)
            {
                listeners.Remove(action);
                _listenerCount--;
            }
        }

        /// <summary>
        /// Alert all listeners that this event has been invoked, passing the desired information.
        /// </summary>
        /// <param name="data">The information that should be passed to listeners.</param>
        /// <param name="debugId">A human-friendly ID for whatever is invoking this event.</param>
        public void Invoke(TData data, string debugId)
        {
            Debug.Log($"{debugId.Format(_listenerFormat)} invoked " +
                      $"{name.Format(_eventFormat)} ({_listenerCount.ToString().Format(_listenerFormat)} listeners).");

            if (_listenerCount <= 0)
            {
                return;
            }
            
            foreach (List<Listener> listeners in _listeners.Values)
            {
                foreach (Listener listener in listeners)
                {
                    listener.Invoke(data);
                }
            }
        }
    }
}