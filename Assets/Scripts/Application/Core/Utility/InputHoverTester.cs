namespace Application.Core.Utility
{
    using UniRx;
    using UnityEngine;

    public class InputHoverTester : MonoBehaviour
    {
        private string _currentObject;

        private void Start()
        {
            InputTools.ObjectMouseOver.Subscribe(value => _currentObject = value == null ? "None" : value.name);
        }

        private void OnGUI()
        {
            GUILayout.Label($"hovered object: {_currentObject}");
        }
    }
}
