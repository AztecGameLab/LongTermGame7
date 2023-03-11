namespace Application.Gameplay.Combat.Actions
{
    using System;
    using System.Collections;
    using Core;
    using ImGuiNET;
    using Newtonsoft.Json;
    using UnityEngine;

    /// <summary>
    /// An example action implementation that can be used for testing.
    /// Simply showcases some common ways to define action behavior.
    /// </summary>
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public class DemoAction : BattleAction, IDebugImGui
    {
        [SerializeField]
        [JsonProperty]
        private string name = "Demo Action Name";

        [SerializeField]
        [JsonProperty]
        private string description = "Demo Action Description";

        [SerializeField]
        [JsonProperty]
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
