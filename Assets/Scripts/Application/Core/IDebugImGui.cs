namespace Application.Gameplay.Combat
{
    /// <summary>
    /// An object that can draw some debugging ImGui information.
    /// </summary>
    public interface IDebugImGui
    {
        /// <summary>
        /// Draw some debugging ImGuis.
        /// </summary>
        void RenderImGui();
    }
}
