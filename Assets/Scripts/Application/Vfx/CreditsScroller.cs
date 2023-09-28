#if UNITY_EDITOR
using UnityEditor;
#endif

using Application.Audio;
using Application.Core;
using Application.Gameplay;
using FMODUnity;
using System;
using TriInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            }
        }
#endif

        [SerializeField]
        private float duration = 120;

        [SerializeField]
        private Vector3 startY;

        [SerializeField]
        private Vector3 endY;

        [SerializeField]
        private string postCreditsScene;

        [SerializeField]
        private EventReference creditsMusic;

        private float _elapsedTime;
        private MusicPlayer.ActiveMusic _creditsMusic;

        private void Start()
        {
            Vector3 startPosition = transform.position;
            startY += startPosition;
            endY += startPosition;
            _creditsMusic = Services.MusicPlayer.AddMusic(creditsMusic);
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(startY, 0.5f);
            Gizmos.DrawSphere(endY, 0.5f);
            Gizmos.DrawLine(transform.position + startY, transform.position + endY);
        }

        private void OnGUI()
        {
            GUILayout.Label($"Remaining: {duration - _elapsedTime}");
        }

        private void Update()
        {
            _elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(_elapsedTime / duration);
            transform.position = Vector3.Lerp(startY, endY, t);

            if (t >= 1)
            {
                Services.EventBus.Invoke(new LevelChangeEvent{NextScene = postCreditsScene}, "Change to post-credits");
                _creditsMusic.Dispose();
                Destroy(this);
            }
        }
    }
}
