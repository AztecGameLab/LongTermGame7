namespace Application.Core
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using JetBrains.Annotations;
    using Rtf;
    using UnityEngine;

    /// <summary>
    /// Routes events throughout the codebase.
    /// Allows separate systems to communicate with each other.
    /// </summary>
    public class EventBus
    {
        private readonly Dictionary<Type, SortedDataStore> _listenerDictionary = new Dictionary<Type, SortedDataStore>();

        // Color formatting information for debug messages.
        private readonly IRichTextData _listenerFormat = Rtf.Rtf.Composite(Rtf.Rtf.Color(Color.red));
        private readonly IRichTextData _eventFormat = Rtf.Rtf.Composite(Rtf.Rtf.Color(Color.cyan));

        /// <summary>
        /// Gets or sets a value indicating whether debugging messages should be printed to the console when
        /// events are fired.
        /// </summary>
        /// <value>
        /// A value indicating the logging state of this EventBus.
        /// </value>
        [PublicAPI]
        public bool VerboseLogging { get; set; } = true;

        /// <summary>
        /// Registers a new listener for an event of type T.
        /// </summary>
        /// <param name="listener">The action to take when the event occurs.</param>
        /// <param name="debugId">A human-readable name for the object calling this function.</param>
        /// <param name="priority">Order of execution for listeners: lower is early, higher is late.</param>
        /// <typeparam name="T">The type of event you are listening for.</typeparam>
        /// <returns>A handle for your listener: call dispose on it to stop listening for events.</returns>
        [PublicAPI]
        public IDisposable AddListener<T>(Action<T> listener, string debugId, int priority)
        {
            Log($"{debugId.Format(_listenerFormat)} started listening to " +
                      $"{typeof(T).Name.Format(_eventFormat)} with priority {priority.ToString(CultureInfo.InvariantCulture).Format(_eventFormat)}.");

            Type type = typeof(T);

            if (!_listenerDictionary.ContainsKey(type))
            {
                _listenerDictionary.Add(type, new SortedDataStore());
            }

            return _listenerDictionary[type].Add(listener, priority, debugId);
        }

        /// <summary>
        /// Registers a new listener for an event of type T.
        /// </summary>
        /// <param name="listener">The action to take when the event occurs.</param>
        /// <param name="debugId">A human-readable name for the object calling this function.</param>
        /// <typeparam name="T">The type of event you are listening for.</typeparam>
        /// <returns>A handle for your listener: call dispose on it to stop listening for events.</returns>
        [PublicAPI]
        public IDisposable AddListener<T>(Action<T> listener, string debugId)
        {
            return AddListener(listener, debugId, 0);
        }

        /// <summary>
        /// Calls an event of type T.
        /// </summary>
        /// <param name="data">The data to pass along with the event.</param>
        /// <param name="debugId">A human-readable name for the object calling this function.</param>
        /// <typeparam name="T">The type of data to send with the event.</typeparam>
        [PublicAPI]
        public void Invoke<T>(T data, string debugId)
        {
            Type type = typeof(T);

            if (!_listenerDictionary.ContainsKey(type))
            {
                _listenerDictionary.Add(type, new SortedDataStore());
            }

            var listenerData = _listenerDictionary[type];
            int listenerCount = listenerData.Count;

            Log($"{debugId.Format(_listenerFormat)} invoked " +
                      $"{typeof(T).Name.Format(_eventFormat)} ({listenerCount} {"Listener".Plural(listenerCount)}: {listenerData}).");

            if (listenerCount <= 0)
            {
                return;
            }

            foreach (List<object> listeners in listenerData.Data)
            {
                foreach (Action<T> listener in listeners)
                {
                    listener.Invoke(data);
                }
            }
        }

        private void Log(string message)
        {
            if (VerboseLogging)
            {
                Debug.Log(message);
            }
        }

        // A collection of data, sorted by priority and counted.
        private sealed class SortedDataStore
        {
            private readonly SortedList<int, List<object>> _data = new SortedList<int, List<object>>();
            private readonly List<string> _dataIds = new List<string>();
            private string _debugString;

            /// <summary>
            /// Gets how many listeners are registered in this group.
            /// </summary>
            public int Count { get; private set; }

            /// <summary>
            /// Gets all of the Listeners in this group, sorted by priority.
            /// </summary>
            public IEnumerable Data => _data.Values;

            /// <summary>
            /// Adds new data to this collection.
            /// </summary>
            /// <param name="data">The data to store.</param>
            /// <param name="priority">Order of storage for data: lower is early, higher is late.</param>
            /// <param name="debugId">A human-readable name for the object calling this function.</param>
            /// <returns>A handle for this data: dispose it to remove from the collection.</returns>
            public IDisposable Add(object data, int priority, string debugId)
            {
                if (!_data.ContainsKey(priority))
                {
                    _data.Add(priority, new List<object>());
                }

                _dataIds.Add(debugId);
                _data[priority].Add(data);
                Count++;

                UpdateDebugString();
                return new UnregisterDisposable(this, data, priority, debugId);
            }

            public override string ToString()
            {
                return _debugString;
            }

            private void UpdateDebugString()
            {
                _debugString = FormatTools.PrettyList(_dataIds);
            }

            // Unregisters itself when disposed. Struct to avoid allocations.
            private struct UnregisterDisposable : IDisposable, IEquatable<UnregisterDisposable>
            {
                private readonly SortedDataStore _parent;
                private readonly object _data;
                private readonly int _priority;
                private readonly string _listenerId;

                private bool _valid; // ensure that we can only be disposed once

                public UnregisterDisposable(SortedDataStore parent, object data, int priority, string listenerId)
                {
                    _parent = parent;
                    _data = data;
                    _priority = priority;
                    _listenerId = listenerId;

                    _valid = true;
                }

                public void Dispose()
                {
                    if (!_valid)
                    {
                        return;
                    }

                    _valid = false;

                    _parent._data[_priority].Remove(_data);
                    _parent._dataIds.Remove(_listenerId);
                    _parent.Count--;
                    _parent.UpdateDebugString();
                }

                public bool Equals(UnregisterDisposable other)
                {
                    if (!Equals(_parent, other._parent))
                    {
                        return false;
                    }

                    return Equals(_data, other._data)
                           && _priority == other._priority
                           && _listenerId == other._listenerId;
                }

                public override bool Equals(object obj)
                {
                    return obj is UnregisterDisposable other && Equals(other);
                }

                public override int GetHashCode()
                {
                    unchecked
                    {
                        var hashCode = _parent != null ? _parent.GetHashCode() : 0;
                        hashCode = (hashCode * 397) ^ (_data != null ? _data.GetHashCode() : 0);
                        hashCode = (hashCode * 397) ^ _priority;
                        hashCode = (hashCode * 397) ^ (_listenerId != null ? _listenerId.GetHashCode() : 0);
                        return hashCode;
                    }
                }
            }
        }
    }
}
