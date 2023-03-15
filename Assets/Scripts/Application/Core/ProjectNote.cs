namespace Application.Core
{
    using JetBrains.Annotations;
    using UnityEngine;

    /// <summary>
    /// An editor-only tool for documenting the unity project setup.
    /// </summary>
    [CreateAssetMenu]
    public class ProjectNote : ScriptableObject
    {
        [SerializeField]
        [TextArea(minLines: 30, maxLines: int.MaxValue)]
        [UsedImplicitly]
        private string message;
    }
}
