using ImGuiNET;
using System;
using System.Collections;
using UnityEngine;

namespace Application.Gameplay.Combat
{
    [Serializable]
    public class DemoAction : BattleAction, IDebugImGui
    {
        [SerializeField] private string name = "Demo Action Name";
        [SerializeField] private string description = "Demo Action Description";
        [SerializeField] private int apCost = 1;
        
        public override string Name => name;
        public override string Description => description;
        
        protected override IEnumerator Execute()
        {
            Debug.Log("Executing debugging action...");
            
            if (User.TryGetComponent(out ActionPointTracker apTracker))
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
}