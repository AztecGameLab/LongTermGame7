using ImGuiNET;
using System;
using UnityEngine;

namespace Application.Core
{
    public class LevelDesignUtil
    {
        private IDisposable _disposable;
        private float _timeScale = 1;
        
        public void Init()
        {
            _disposable = ImGuiUtil.Register(Draw);
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }

        private void Draw()
        {
            ImGui.Begin("Designer Tools");
            
            if (ImGui.SmallButton("Slower") || Input.GetKeyDown(KeyCode.Q))
            {
                _timeScale /= 2;
                Time.timeScale = _timeScale;
            }
            
            ImGui.SameLine();
            ImGui.Text("(Q)");
            
            ImGui.SameLine();
            ImGui.SetNextItemWidth(50);
            if (ImGui.InputFloat(string.Empty, ref _timeScale))
            {
                Time.timeScale = _timeScale;
            }

            ImGui.SameLine();
            if (ImGui.SmallButton("Faster") || Input.GetKeyDown(KeyCode.E))
            {
                _timeScale *= 2;
                Time.timeScale = _timeScale;
            }
            
            ImGui.SameLine();
            ImGui.Text("(E)");

            ImGui.End();
        }
    }
}