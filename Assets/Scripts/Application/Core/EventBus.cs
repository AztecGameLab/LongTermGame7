using System;
using System.Collections.Generic;
using UnityEngine;

namespace Application.Core
{
    public class EventBus
    {
        private struct DisposableListener<T> : IDisposable
        {
            public Action<T> Listener;
            public int Priority;

            public void Dispose()
            {
                Services.EventBus._listenerDictionary[typeof(T)][Priority].Remove(Listener);
            }
        }
        
        private Dictionary<Type, SortedList<int, List<object>>> _listenerDictionary = new Dictionary<Type, SortedList<int, List<object>>>();
        private IRichTextData _listenerFormat = Rtf.Composite(Rtf.Color(Color.red));
        private IRichTextData _eventFormat = Rtf.Composite(Rtf.Color(Color.cyan));
        
        public IDisposable AddListener<T>(Action<T> listener, string debugId, int priority = 0)
        {
            Debug.Log($"{debugId.Format(_listenerFormat)} started listening to " +
                      $"{nameof(T).Format(_eventFormat)} with priority {priority.ToString().Format(_eventFormat)}.");
            
            Type type = typeof(T);

            if (!_listenerDictionary.ContainsKey(type))
                _listenerDictionary.Add(type, new SortedList<int, List<object>>());
            
            if (!_listenerDictionary[type].ContainsKey(priority))
                _listenerDictionary[type].Add(priority, new List<object>());
            
            _listenerDictionary[type][priority].Add(listener);

            return new DisposableListener<T> { Listener = listener, Priority = priority };
        }

        private struct InvokeData
        {
            public Action Action;
            public Timing.ITiming Timing;
        }

        private List<InvokeData> _delayedInvokes = new List<InvokeData>();

        private struct DelayedDisposable : IDisposable
        {
            public InvokeData Data;
            
            public void Dispose()
            {
                Services.EventBus._delayedInvokes.Remove(Data);
            }
        }

        public void Update()
        {
            float currentTime = Time.time;

            foreach (var delayedInvoke in _delayedInvokes)
            {
                delayedInvoke.Timing.Update();
                
                if (delayedInvoke.Timing.IsReady)
                    delayedInvoke.Action.Invoke();
            }

            _delayedInvokes.RemoveAll(data => data.Timing.IsReady);
        }

        public static class Timing
        {
            public static ITiming Frames(int frames)
            {
                return new FrameTiming { RemainingFrames = frames };
            }

            public static ITiming Seconds(float seconds)
            {
                return new SecondsTiming { RemainingSeconds = seconds };
            }
            
            public interface ITiming
            {
                bool IsReady { get; }
                void Update();
            }

            private struct SecondsTiming : ITiming
            {
                public float RemainingSeconds;

                public bool IsReady => RemainingSeconds <= 0;
                
                public void Update()
                {
                    RemainingSeconds -= Time.deltaTime;
                }
            }
            
            private struct FrameTiming : ITiming
            {
                public int RemainingFrames;

                public bool IsReady => RemainingFrames <= 0;
                
                public void Update()
                {
                    RemainingFrames--;
                }
            }
        }
        
        public IDisposable InvokeAfter<T>(T data, Timing.ITiming timing, string debugId)
        {
            var invokeData = new InvokeData
            {
                Timing = timing,
                Action = () => { Invoke(data, debugId); }
            };
            
            _delayedInvokes.Add(invokeData);
            return new DelayedDisposable { Data = invokeData };
        }

        public void Invoke<T>(T data, string debugId)
        {
            Type type = typeof(T);
            
            if (!_listenerDictionary.ContainsKey(type))
                _listenerDictionary.Add(type, new SortedList<int, List<object>>());

            int listenerCount = 0;
            
            foreach (List<object> listeners in _listenerDictionary[type].Values)
                listenerCount += listeners.Count;
            
            Debug.Log($"{debugId.Format(_listenerFormat)} invoked " +
                      $"{typeof(T).Name.Format(_eventFormat)} ({listenerCount.ToString().Format(_listenerFormat)} listeners).");
                
            if (_listenerDictionary[type].Count <= 0)
                return;

            foreach (List<object> listeners in _listenerDictionary[type].Values)
            {
                foreach (Action<T> listener in listeners)
                    listener.Invoke(data);
            }
        }
    }
}