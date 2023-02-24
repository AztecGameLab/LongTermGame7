using ImGuiNET;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Application.Gameplay.Combat
{
    using UnityEngine;

    public class ActionSet : MonoBehaviour
    {
        [SerializeReference]
        public List<IAction> actions;
    }

    [Serializable]
    public class DemoAction : IAction, IDebugImGui
    {
        [SerializeField] private string name = "Demo Action Name";
        [SerializeField] private string description = "Demo Action Description";
        
        public string Name => name;
        public string Description => description;

        private bool _waitingToConfirm;
        
        public IEnumerator Execute(GameObject user)
        {
            _waitingToConfirm = true;

            while (_waitingToConfirm)
            {
                yield return null;
            }
        }

        public void RenderImGui()
        {
            if (ImGui.Button("Confirm"))
                _waitingToConfirm = false;
        }
    }

    public interface IDebugImGui
    {
        void RenderImGui();
    }

    public interface IDebugGizmos
    {
        void RenderGizmos();
    }

    public interface IAction
    {
        string Name { get; }
        string Description { get; }

        IEnumerator Execute(GameObject user);
    }
}