using ImGuiNET;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;

namespace Application.Gameplay.Combat
{
    using UnityEngine;

    public class ActionSet : MonoBehaviour
    {
        [SerializeReference]
        public List<BattleAction> actions;
    }

    [Serializable]
    public class DemoAction : BattleAction, IDebugImGui
    {
        [SerializeField] private string name = "Demo Action Name";
        [SerializeField] private string description = "Demo Action Description";
        [SerializeField] private int apCost = 1;
        
        public override string Name => name;
        public override string Description => description;
        
        protected override IEnumerator Execute(GameObject user)
        {
            Debug.Log("Executing debugging action...");
            
            if (user.TryGetComponent(out ActionPointTracker apTracker))
                apTracker.remainingActionPoints -= apCost;
            
            yield return new WaitForSeconds(1);
            Debug.Log("Done!");
        }

        private bool _lockedIn;

        public override void PrepEnter() => _lockedIn = false;

        public override bool PrepTick() => _lockedIn;

        public void RenderImGui()
        {
            if (ImGui.Button("Lock in demo action"))
                _lockedIn = true;
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

    // public interface IAction
    // {
    //     string Name { get; }
    //     string Description { get; }
    //
    //     IObservable<Unit> Execute(GameObject user);
    // }

    [Serializable]
    public abstract class BattleAction
    {
        public abstract string Name { get; }
        public abstract string Description { get; }

        public virtual void PrepEnter() {}
        public virtual void PrepExit() {}
        public virtual bool PrepTick() { return false; }

        protected abstract IEnumerator Execute(GameObject user);
        public IObservable<Unit> Run(GameObject user) => Execute(user).ToObservable();
    }
}