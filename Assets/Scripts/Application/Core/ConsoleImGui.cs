using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImGuiNET;
using Application.Core;

public class ConsoleImGui : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {

        ImGuiUtil.Register(DrawImGui).AddTo(this);
    }

    private void DrawImGui()
    {
        ImGui.Begin("Console");
        ImGui.Text("");
        ImGui.End();
    }
}
