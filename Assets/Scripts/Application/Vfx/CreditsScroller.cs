namespace Application.Vfx
{
    using Core;
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

        private float _elapsedTime;

        private void Start()
        {
            startY = transform.localPosition;
        }

        private void OnGUI()
        {
            GUILayout.Label($"Remaining: {duration - _elapsedTime}");
        }

        private void Update()
        {
            _elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(_elapsedTime / duration);
            transform.localPosition = Vector3.Lerp(startY, endY, t);

            if (t >= 1 || Input.GetKeyDown(KeyCode.Escape))
            {
                Services.EventBus.Invoke(new LevelChangeEvent { NextScene = postCreditsScene }, "Change to post-credits");
                Destroy(this);
            }
        }
    }
}
