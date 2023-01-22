namespace Application.Core
{
    using System;
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