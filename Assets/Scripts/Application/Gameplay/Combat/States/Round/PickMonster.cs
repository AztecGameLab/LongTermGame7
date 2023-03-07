namespace Application.Gameplay.Combat.States.Round
{
    using System;
    using System.Collections.Generic;
    using Cinemachine;
    using Core;
    using ImGuiNET;
    using UnityEngine;

    /// <summary>
    /// The battle round state where you select which monster you want to use in combat.
    /// </summary>
    [Serializable]
    public class PickMonster : RoundState, IDebugImGui
    {
        private const int PickMonsterCameraActivePriority = 50;

        private List<GameObject> _availableMonsters;

        [SerializeField]
        private CinemachineVirtualCamera pickMonsterCamera;

        private int _selectedMonsterIndex;

        /// <summary>
        /// Gets the currently selected monster.
        /// </summary>
        /// <value>
        /// The currently selected monster.
        /// </value>
        public GameObject SelectedMonster { get; private set; }

        /// <summary>
        /// Sets up the pick monster state.
        /// </summary>
        public void Initialize()
        {
            RegisterImGuiDebug(this);
            _availableMonsters = new List<GameObject>();
        }

        /// <inheritdoc/>
        public override void OnRoundBegin()
        {
            _availableMonsters.AddRange(Round.Controller.PlayerTeam);
            _selectedMonsterIndex = 0;
        }

        /// <inheritdoc/>
        public override void OnRoundEnd()
        {
            _availableMonsters.Clear();
        }

        /// <inheritdoc/>
        public override void OnEnter()
        {
            base.OnEnter();
            Services.EventBus.Invoke(new RoundStateEnterEvent<PickMonster> { State = this }, "Pick Monster State");
            pickMonsterCamera.Priority = PickMonsterCameraActivePriority;

            SelectedMonster = _availableMonsters.Count > 0 ? _availableMonsters[_selectedMonsterIndex] : null;

            pickMonsterCamera.Follow = SelectedMonster != null
                ? SelectedMonster.transform
                : Round.Controller.PlayerTeam[0].transform;
        }

        /// <inheritdoc/>
        public override void OnExit()
        {
            base.OnExit();
            Services.EventBus.Invoke(new RoundStateExitEvent<PickMonster> { State = this }, "Pick Monster State");
            pickMonsterCamera.Priority = 0;
        }

        /// <inheritdoc/>
        public void RenderImGui()
        {
            ImGui.Begin("Pick Monster");

            if (_availableMonsters.Count <= 0)
            {
                ImGui.Text("No monsters left to move! You must end the round.");

                if (ImGui.Button("Next Round"))
                {
                    Round.NextRound();
                }
            }

            ImGui.Text("Available Monsters:");

            foreach (GameObject availableMonster in _availableMonsters)
            {
                ImGui.Text($"\t{availableMonster.name}");
            }

            ImGui.Text($"Currently selected: {(SelectedMonster == null ? "None" : SelectedMonster.name)}");

            if (SelectedMonster != null && ImGui.Button($"Select {SelectedMonster.name}"))
            {
                OnSelectMonster(SelectedMonster);
            }

            if (ImGui.Button("Next Monster") && _availableMonsters.Count > 0)
            {
                SelectNextMonster();
            }

            ImGui.End();
        }

        private void SelectNextMonster()
        {
            if (_availableMonsters.Count <= 0)
            {
                Debug.LogError("Cannot select next monster - there are none available ones left!");
                return;
            }

            _selectedMonsterIndex = (_selectedMonsterIndex + 1) % _availableMonsters.Count;
            SelectedMonster = _availableMonsters[_selectedMonsterIndex];
            pickMonsterCamera.Follow = SelectedMonster.transform;
        }

        private void OnSelectMonster(GameObject monster)
        {
            _availableMonsters.Remove(monster);
            _selectedMonsterIndex = 0;

            Round.TransitionTo(Round.PickActions);
            pickMonsterCamera.Follow = monster.transform;
        }
    }
}
