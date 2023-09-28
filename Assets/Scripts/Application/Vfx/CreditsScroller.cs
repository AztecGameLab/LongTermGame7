namespace Application.Vfx
{
#if UNITY_EDITOR
    using UnityEditor;
#endif

    using Audio;
    using Core;
    using FMODUnity;
    using Gameplay;
    using UnityEngine;

    /// <summary>
    /// Handles logic for playing the credits scene.
    /// </summary>
    public class CreditsScroller : MonoBehaviour
    {
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
                Services.EventBus.Invoke(new LevelChangeEvent { NextScene = postCreditsScene }, "Change to post-credits");
                _creditsMusic.Dispose();
                Destroy(this);
            }
        }

#if UNITY_EDITOR
        [CustomEditor(typeof(CreditsScroller))]
        private sealed class EditorLogic : Editor
        {
            private void OnSceneGUI()
            {
                var t = target as CreditsScroller;

                if (t != null)
                {
                    Transform transform = t.transform;
                    Vector3 pos = transform.position;
                    Quaternion rot = transform.rotation;
                    t.startY = Handles.PositionHandle(pos + t.startY, rot) - pos;
                    t.endY = Handles.PositionHandle(pos + t.endY, rot) - pos;
                }
            }
        }
#endif
    }
}
