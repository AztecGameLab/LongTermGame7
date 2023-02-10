namespace Application.Gameplay.Landmarks
{
    using System;
    using Core;
    using ImGuiNET;
    using UnityEngine;
    using Object = UnityEngine.Object;

    /// <summary>
    /// Displays a useful window for displaying and moving between landmarks.
    /// </summary>
    public class LandmarkViewer : IDisposable
    {
        private string _landmarkName = string.Empty;
        private IDisposable _disposable;
        private bool _isOpen;

        /// <summary>
        /// Set up the landmark viewing window.
        /// </summary>
        public void Init()
        {
            _disposable = ImGuiUtil.Register(DrawLandmarkView);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            _disposable.Dispose();
        }

        private static void TeleportTo(Component landmark)
        {
            var player = Object.FindObjectOfType<PlayerMovement>();
            player.transform.position = landmark.transform.position;
        }

        private static void TeleportTo(int index)
        {
            if (index < Landmark.Landmarks.Count && index >= 0)
            {
                TeleportTo(Landmark.Landmarks[index]);
            }
        }

        private void AddTemporary()
        {
            var player = Object.FindObjectOfType<PlayerMovement>();
            var newLandmark = new GameObject($"{_landmarkName} (TEMPORARY)").AddComponent<Landmark>();
            newLandmark.Id = _landmarkName;
            newLandmark.transform.position = player.transform.position;
            _landmarkName = string.Empty;

#if UNITY_EDITOR
            UnityEditor.Selection.activeObject = newLandmark;
#endif
        }

        private void DrawLandmarkView()
        {
            const int maxLandmarkNameSize = 32;

            ImGui.Begin("Landmarks", ref _isOpen);

            for (var index = 0; index < Landmark.Landmarks.Count; index++)
            {
                var landmark = Landmark.Landmarks[index];

                if (ImGui.Button($"Teleport to {landmark.Id}"))
                {
                    TeleportTo(landmark);
                }

                ImGui.SameLine();
                ImGui.Text($"(Press {index + 1})");

                ImGui.Spacing();
            }

            ImGui.Text("Add new landmark");

            if (ImGui.InputText(string.Empty, ref _landmarkName, maxLandmarkNameSize, ImGuiInputTextFlags.EnterReturnsTrue))
            {
                AddTemporary();
            }

            if (InputTools.TryGetNumberDown(out int number))
            {
                TeleportTo(number - 1);
            }

            ImGui.End();
        }
    }
}
