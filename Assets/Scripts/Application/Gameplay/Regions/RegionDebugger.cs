using Application.Core;
using ImGuiNET;

namespace Application.Gameplay.Regions
{
    /// <summary>
    /// Displays debugging information about regions.
    /// </summary>
    public static class RegionDebugger
    {
        /// <summary>
        /// Sets up the region debugger.
        /// </summary>
        public static void Init()
        {
            ImGuiUtil.Register(DrawRegionDebuggingWindow);
        }

        private static void DrawRegionDebuggingWindow()
        {
            ImGui.Begin("Region");
            ImGui.Text(Services.RegionTracker.CurrentRegion.ToString());
            ImGui.End();
        }
    }
}
