namespace Application.Gameplay
{
    using TMPro;
    using UnityEngine;

    /// <summary>
    /// A utility for writing 3D text notes into the scene.
    /// </summary>
    public class DeveloperNote : MonoBehaviour
    {
        [SerializeField]
        private Vector2 size = new Vector2(3.5f, 1.5f);

        [SerializeField]
        [TextArea(minLines: 10, maxLines: 20)]
        private string text = "Developer Note";

        [SerializeField]
        private HorizontalAlignmentOptions horizontalAlignment = HorizontalAlignmentOptions.Left;

        [SerializeField]
        private VerticalAlignmentOptions verticalAlignment = VerticalAlignmentOptions.Middle;

        private void Awake()
        {
            ApplyText();
        }

        private void OnValidate()
        {
            ApplyText();
        }

        private void ApplyText()
        {
            var tmpText = GetComponentInChildren<TMP_Text>();
            tmpText.text = text;
            tmpText.horizontalAlignment = horizontalAlignment;
            tmpText.verticalAlignment = verticalAlignment;

            var rectTransform = GetComponentInChildren<Canvas>().GetComponent<RectTransform>();
            rectTransform.sizeDelta = size;
        }
    }
}
