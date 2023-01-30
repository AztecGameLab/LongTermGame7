namespace Application.Core
{
    using ImGuiNET;
    using UnityEngine;

    /// <summary>
    /// Displays the ImGui demo window.
    /// </summary>
    public class ImGuiDemo : MonoBehaviour
    {
        private bool _isOpen;

        private void Awake()
        {
            ImGuiUtil.Register(() => ImGui.ShowDemoWindow(ref _isOpen)).AddTo(this);
        }
    }
}
