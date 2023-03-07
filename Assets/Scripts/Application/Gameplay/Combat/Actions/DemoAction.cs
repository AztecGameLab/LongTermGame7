namespace Application.Gameplay.Combat.Actions
{
    using System;
    using System.Collections;
    using ImGuiNET;
    using UnityEngine;

    /// <summary>
    /// An example action implementation that can be used for testing.
    /// Simply showcases some common ways to define action behavior.
    /// </summary>
    [Serializable]
    public class DemoAction : BattleAction, IDebugImGui
    {
        [SerializeField]
        private string name = "Demo Action Name";

        [SerializeField]
        private string description = "Demo Action Description";

        [SerializeField]
        private int actionPointCost = 1;

        /// <inheritdoc/>
        public override string Name => name;

        /// <inheritdoc/>
        public override string Description => description;

        /// <inheritdoc/>
        public void RenderImGui()
        {
            IsPrepFinished |= ImGui.Button("Lock in demo action");
        }

        /// <inheritdoc/>
        protected override IEnumerator Execute()
        {
            Debug.Log("Executing debugging action...");

            if (User.TryGetComponent(out ActionPointTracker tracker))
            {
                tracker.TrySpend(actionPointCost);
            }

            yield return new WaitForSeconds(1);
            Debug.Log("Done!");
        }
    }
}
