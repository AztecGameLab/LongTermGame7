using System;
using UnityEngine;

namespace Application.Core
{
    public class TestSerializedBehavior : SerializedBehavior
    {
        private int _counter;
        
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
            public int counter;
        }

        public override void ReadData(object data)
        {
            var d = (Data)data;
            _counter = d.counter;
        }

        public override object WriteData()
        {
            return new Data { counter = _counter };
        }

        public override string GetID()
        {
            return "Counter Test";
        }
    }
}