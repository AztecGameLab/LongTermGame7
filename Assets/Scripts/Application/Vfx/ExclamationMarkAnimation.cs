namespace Application.Vfx
{
    using UnityEngine;

    /// <summary>
    /// Plays an hovering animation on this GameObject.
    /// </summary>
    public class ExclamationMarkAnimation : MonoBehaviour
    {
        [SerializeField]
        private float amplatude;

        [SerializeField]
        private float dialation;

        private Vector3 _defaultPos;

        private void Start()
        {
            _defaultPos = transform.position;
        }

        private void Update()
        {
            transform.position = _defaultPos + new Vector3(0, amplatude * Mathf.Sin(Time.time / dialation), 0);
        }
    }
}
