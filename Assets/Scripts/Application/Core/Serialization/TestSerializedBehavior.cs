namespace Application.Core.Serialization
{
    using System;
    using UnityEngine;

    /// <summary>
    /// A testing behaviour, to implement basic use of the serialization system.
    /// </summary>
    public class TestSerializedBehavior : SerializedBehavior
    {
        private int _counter;

        /// <inheritdoc/>
        public override string Id => "Counter Test";

        /// <inheritdoc/>
        public override void ReadData(object data)
        {
            var d = (Data)data;
            _counter = d.Counter;
        }

        /// <inheritdoc/>
        public override object WriteData()
        {
            return new Data { Counter = _counter };
        }

        private void OnGUI()
        {
            if (GUILayout.Button($"Counter: {_counter}"))
            {
                _counter++;
            }
        }

        [Serializable]
        private struct Data
        {
            public int Counter;
        }
    }
}
