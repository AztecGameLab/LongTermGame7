namespace Application.Vfx
{
    using UnityEngine;

    /// <summary>
    /// A visual effect that makes this GameObject bob up and down over time.
    /// </summary>
    public class HoverEffect : MonoBehaviour
    {
        [SerializeField]
        private float hoverSpeed = 2;

        [SerializeField]
        private float hoverAmount = 0.1f;

        private Vector3 _startingPosition;

        private void Awake()
        {
            _startingPosition = transform.localPosition;
        }

        private void Update()
        {
            Vector3 offset = new Vector3(0, Mathf.Sin(Time.time * hoverSpeed) * hoverAmount, 0);
            transform.localPosition = _startingPosition + offset;
        }
    }
}
