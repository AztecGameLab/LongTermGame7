namespace Application.Core
{
    using System.Collections;
    using UnityEngine;

    /// <summary>
    /// Console logger displays console messages on the screen to allow messages to be seen
    /// while not using the editor or while running the editor game window maximized.
    /// </summary>
    public class ConsoleLogger : MonoBehaviour
    {
        private readonly uint _numMessages = 10;
        private readonly Queue _messageQueue = new Queue();

        // Start is called before the first frame update
        private void Start()
        {
            Debug.Log("Starting console logging");
        }

        private void OnEnable()
        {
            Application.logMessageReceived += HandleLog;
        }

        private void OnDisable()
        {
            Application.logMessageReceived -= HandleLog;
        }

        private void HandleLog(string logString, string stackTrace, LogType type)
        {
            _messageQueue.Enqueue("[" + type + "] : " + logString);
            if (type == LogType.Exception)
            {
                _messageQueue.Enqueue(stackTrace);
            }
            while (_messageQueue.Count > _numMessages)
            {
                _messageQueue.Dequeue();
            }
        }

        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(Screen.width - 400, 0, 400, Screen.height));
            GUILayout.Label("\n" + string.Join("\n", _messageQueue.ToArray()));
            GUILayout.EndArea();
        }
    }
}
