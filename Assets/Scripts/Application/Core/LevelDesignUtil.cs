namespace Application.Core
{
    using System;
    using ImGuiNET;
    using UnityEngine;

    /// <summary>
    /// Useful ImGUI utilities for level designers.
    /// </summary>
    public class LevelDesignUtil : IDisposable
    {
        private const int TimeScaleMultiplier = 2;
        private const int TimeScaleInputFieldWidth = 50;

        private IDisposable _disposable;
        private float _timeScale = 1;

        /// <summary>
        /// Initialize this window.
        /// </summary>
        public void Init()
        {
            _disposable = ImGuiUtil.Register(Draw);
        }

        /// <summary>
        /// Shuts down this window.
        /// </summary>
        public void Dispose()
        {
            _disposable.Dispose();
        }

        private void Draw()
        {
            ImGui.Begin("Designer Tools");

            // Slower Button
            if (ImGui.SmallButton("Slower") || Input.GetKeyDown(KeyCode.Q))
            {
                _timeScale /= TimeScaleMultiplier;
                Time.timeScale = _timeScale;
            }

            ImGui.SameLine();
            ImGui.Text("(Q)");

            // Time Scale Input Field
            ImGui.SameLine();
            ImGui.SetNextItemWidth(TimeScaleInputFieldWidth);
            if (ImGui.InputFloat(string.Empty, ref _timeScale))
            {
                Time.timeScale = _timeScale;
            }

            // Faster Button
            ImGui.SameLine();
            if (ImGui.SmallButton("Faster") || Input.GetKeyDown(KeyCode.E))
            {
                _timeScale *= TimeScaleMultiplier;
                Time.timeScale = _timeScale;
            }

            ImGui.SameLine();
            ImGui.Text("(E)");

            ImGui.End();
        }
    }
}
