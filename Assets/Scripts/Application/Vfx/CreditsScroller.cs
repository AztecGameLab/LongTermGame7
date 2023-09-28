#if UNITY_EDITOR
using UnityEditor;
#endif

using System;
using UnityEngine;

namespace Application.Vfx
{

    public class CreditsScroller : MonoBehaviour
    {

#if UNITY_EDITOR
        [CustomEditor(typeof(CreditsScroller))]
        private class EditorLogic : Editor
        {
            private void OnSceneGUI()
            {
                var t = target as CreditsScroller;
                t.startY = Handles.PositionHandle(t.transform.position + t.startY, t.transform.rotation) - t.transform.position;
                t.endY = Handles.PositionHandle(t.transform.position + t.endY, t.transform.rotation) - t.transform.position;
                Handles.Label(t.transform.position, $"{t._elapsedTime}");
            }
        }
#endif

        [SerializeField]
        private float duration = 120;

        [SerializeField]
        private Vector3 startY;

        [SerializeField]
        private Vector3 endY;

        private float _elapsedTime;

        private void Start()
        {
            Vector3 startPosition = transform.position;
            startY += startPosition;
            endY += startPosition;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(startY, 0.5f);
            Gizmos.DrawSphere(endY, 0.5f);
            Gizmos.DrawLine(transform.position + startY, transform.position + endY);
        }

        private void Update()
        {
            _elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(_elapsedTime / duration);
            transform.position = Vector3.Lerp(startY, endY, t);
        }
    }
}
