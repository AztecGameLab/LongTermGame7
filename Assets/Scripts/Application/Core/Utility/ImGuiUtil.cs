namespace Application.Core
{
    using System;
    using Gameplay.Combat;
    using ImGuiNET;

    /// <summary>
    /// Simplifies the process of registering ImGui handlers.
    /// </summary>
    public static class ImGuiUtil
    {
        /// <summary>
        /// Registers a new callback for drawing ImGui information.
        /// </summary>
        /// <param name="callback">The callback to run for drawing information.</param>
        /// <returns>A disposable for unregistering the callback.</returns>
        public static IDisposable Register(Action callback)
        {
            ImGuiUn.Layout += callback;
            return new RegistrationDisposable { Callback = callback };
        }

        /// <summary>
        /// Registers a new callback for drawing ImGui information.
        /// </summary>
        /// <param name="debugImGui">The class that will draw the information.</param>
        /// <returns>A disposable for unregistering the callback.</returns>
        public static IDisposable Register(IDebugImGui debugImGui)
        {
            if (debugImGui != null)
            {
                ImGuiUn.Layout += debugImGui.RenderImGui;
                return new RegistrationDisposable { Callback = debugImGui.RenderImGui };
            }

            return new RegistrationDisposable();
        }

        private struct RegistrationDisposable : IDisposable
        {
            public Action Callback;

            public void Dispose()
            {
                ImGuiUn.Layout -= Callback;
            }
        }
    }
}
