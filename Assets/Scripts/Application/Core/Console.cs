using Application.Core;
using ImGuiNET;
using System;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeConsole : IDisposable
{
    private IDisposable _disposable;
    private bool _open;
    private bool _autoscroll = true;
    private List<string> _messages = new List<string>();
    private string _input = string.Empty;
    private bool _scrollToBottom;
    
    public void Init()
    {
        ImGuiUtil.Register(Draw);
    }

    public void Dispose()
    {
        _disposable.Dispose();
    }

    public void Clear()
    {
        _messages.Clear();
    }

    private void Draw()
    {
        ImGui.SetNextWindowSize(new Vector2(520, 600), ImGuiCond.FirstUseEver);
        if (!ImGui.Begin("Console", ref _open))
        {
            ImGui.End();
            return;
        }

        // As a specific feature guaranteed by the library, after calling Begin() the last Item represent the title bar.
        // So e.g. IsItemHovered() will return true when hovering the title bar.
        // Here we create a context menu only available from the title bar.
        if (ImGui.BeginPopupContextItem())
        {
            if (ImGui.MenuItem("Close Console"))
                _open = false;
            ImGui.EndPopup();
        }

        ImGui.TextWrapped("Enter 'HELP' for help.");

        if (ImGui.SmallButton("Clear"))           { Clear(); }
        ImGui.SameLine();
        bool copyToClipboard = ImGui.SmallButton("Copy");

        ImGui.Separator();

        // Options menu
        if (ImGui.BeginPopup("Options"))
        {
            ImGui.Checkbox("Auto-scroll", ref _autoscroll);
            ImGui.EndPopup();
        }

        // Options, Filter
        if (ImGui.Button("Options"))
            ImGui.OpenPopup("Options");
        ImGui.SameLine();
        ImGui.Separator();

        // Reserve enough left-over height for 1 separator + 1 input text
        float footerHeightToReserve = ImGui.GetStyle().ItemSpacing.y + ImGui.GetFrameHeightWithSpacing();
        if (ImGui.BeginChild("ScrollingRegion", new Vector2(0, -footerHeightToReserve), false, ImGuiWindowFlags.HorizontalScrollbar))
        {
            if (ImGui.BeginPopupContextWindow())
            {
                if (ImGui.Selectable("Clear")) Clear();
                ImGui.EndPopup();
            }

            ImGui.PushStyleVar(ImGuiStyleVar.ItemSpacing, new Vector2(4, 1)); // Tighten spacing
            if (copyToClipboard)
                ImGui.LogToClipboard();
            foreach (var item in _messages)
            {
                // Normally you would store more information in your item than just a string.
                // (e.g. make Items[] an array of structure, store color/type etc.)
                Vector4 color = Vector4.one;
                bool hasColor = false;
                if (item.Contains("[error]")) { color = new Vector4(1.0f, 0.4f, 0.4f, 1.0f); hasColor = true; }
                else if (item.Contains("# ")) { color = new Vector4(1.0f, 0.8f, 0.6f, 1.0f); hasColor = true; }
                if (hasColor)
                    ImGui.PushStyleColor(ImGuiCol.Text, color);
                ImGui.TextUnformatted(item);
                if (hasColor)
                    ImGui.PopStyleColor();
            }
            if (copyToClipboard)
                ImGui.LogFinish();

            // Keep up at the bottom of the scroll region if we were already at the bottom at the beginning of the frame.
            // Using a scrollbar or mouse-wheel will take away from the bottom edge.
            if (_scrollToBottom || (_autoscroll && ImGui.GetScrollY() >= ImGui.GetScrollMaxY()))
                ImGui.SetScrollHereY(1.0f);
            _scrollToBottom = false;

            ImGui.PopStyleVar();
        }
        ImGui.EndChild();
        ImGui.Separator();

        // Command-line
        bool reclaimFocus = false;
        ImGuiInputTextFlags inputTextFlags = ImGuiInputTextFlags.EnterReturnsTrue | ImGuiInputTextFlags.CallbackCompletion | ImGuiInputTextFlags.CallbackHistory;
        if (ImGui.InputText("Input", ref _input, 32, inputTextFlags))
        {
            _messages.Add(_input);
            _input = string.Empty;
            reclaimFocus = true;
        }

        // Auto-focus on window apparition
        ImGui.SetItemDefaultFocus();
        if (reclaimFocus)
            ImGui.SetKeyboardFocusHere(-1); // Auto focus previous widget

        ImGui.End();
    }
}