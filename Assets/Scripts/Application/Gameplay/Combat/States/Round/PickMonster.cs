using Application.Gameplay.Combat.UI;
using UniRx;

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

        [SerializeField]
        private PickMonsterUI pickMonsterUI;

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

        private CompositeDisposable _disposable;

        /// <inheritdoc/>
        public override void OnEnter()
        {
            base.OnEnter();
            
            if (_availableMonsters.Count <= 0)
            {
                Debug.LogError("Cannot select next monster - there are none available ones left!");
                Round.NextRound();
                return;
            }
            
            SelectedMonster = _availableMonsters.Count > 0 ? _availableMonsters[_selectedMonsterIndex] : null;
            pickMonsterCamera.Priority = PickMonsterCameraActivePriority;
            pickMonsterUI.gameObject.SetActive(true);
            pickMonsterUI.SelectedMonster.Value = SelectedMonster;
            _disposable = new CompositeDisposable();
            _disposable.Add(pickMonsterUI.ObserveMonsterSubmitted().Subscribe(OnSelectMonster));
            _disposable.Add(pickMonsterUI.SelectedMonster.Subscribe(SelectNextMonster));

            pickMonsterCamera.Follow = SelectedMonster != null
                ? SelectedMonster.transform
                : Round.Controller.PlayerTeam[0].transform;
        }

        /// <inheritdoc/>
        public override void OnExit()
        {
            base.OnExit();
            pickMonsterCamera.Priority = 0;
            pickMonsterUI.gameObject.SetActive(false);
            _disposable?.Dispose();
        }

        public override void OnTick()
        {
            base.OnTick();
            pickMonsterUI.Tick(pickMonsterCamera.transform.position, _availableMonsters);
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
                // SelectNextMonster();
            }

            ImGui.End();
        }

        private void SelectNextMonster(GameObject monster)
        {
            if (_availableMonsters.Count <= 0 || monster == null)
            {
                Debug.LogError("Cannot select next monster - there are none available ones left!");
                return;
            }

            _selectedMonsterIndex = (_selectedMonsterIndex + 1) % _availableMonsters.Count;
            SelectedMonster = monster;
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
