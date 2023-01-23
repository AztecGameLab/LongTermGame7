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

        private static void TeleportTo(Landmark landmark)
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

            ImGui.SetNextItemWidth(100);

            if (ImGui.InputText("Add New Landmark", ref _landmarkName, 32, ImGuiInputTextFlags.EnterReturnsTrue))
            {
                AddTemporary();
            }
            
            if (Input.GetKeyDown(KeyCode.Alpha1)) TeleportTo(0);
            if (Input.GetKeyDown(KeyCode.Alpha2)) TeleportTo(1);
            if (Input.GetKeyDown(KeyCode.Alpha3)) TeleportTo(2);
            if (Input.GetKeyDown(KeyCode.Alpha4)) TeleportTo(3);
            if (Input.GetKeyDown(KeyCode.Alpha5)) TeleportTo(4);
            if (Input.GetKeyDown(KeyCode.Alpha6)) TeleportTo(5);
            if (Input.GetKeyDown(KeyCode.Alpha7)) TeleportTo(6);
            if (Input.GetKeyDown(KeyCode.Alpha8)) TeleportTo(7);
            if (Input.GetKeyDown(KeyCode.Alpha9)) TeleportTo(8);

            ImGui.End();
        }
    }
}
