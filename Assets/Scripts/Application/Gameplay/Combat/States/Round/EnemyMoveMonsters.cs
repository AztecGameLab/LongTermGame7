namespace Application.Gameplay.Combat.States.Round
{
    using System;
    using Cinemachine;
    using Core;
    using ImGuiNET;
    using UniRx;
    using UnityEngine;

    /// <summary>
    /// The battle round state where the enemy gets a chance to make their monsters move around.
    /// </summary>
    [Serializable]
    public class EnemyMoveMonsters : RoundState, IDebugImGui
    {
        private const int EnemyCameraActivePriority = 100;

        [SerializeField]
        private float enemyRadius = 3;

        [SerializeField]
        private CinemachineTargetGroup enemyTargetGroup;

        [SerializeField]
        private CinemachineVirtualCamera enemyVirtualCamera;

        private IDisposable _disposable;

        /// <summary>
        /// Sets up the enemy move monster state.
        /// </summary>
        public void Initialize()
        {
            // No need to do anything here, for now.
        }

        /// <inheritdoc/>
        public override void OnEnter()
        {
            base.OnEnter();
            // enemyVirtualCamera.Priority = EnemyCameraActivePriority;

            _disposable = Round.Controller.EnemyOrderDecider.Run(Round.Controller).Subscribe(_ => OnDeciderFinish());
            enemyTargetGroup.RemoveAllMembers();
            enemyTargetGroup.AddMemberRange(Round.Controller.EnemyTeam, 1, enemyRadius);
        }

        /// <inheritdoc/>
        public override void OnExit()
        {
            base.OnExit();
            enemyVirtualCamera.Priority = 0;

            _disposable?.Dispose();
        }

        /// <inheritdoc/>
        public void RenderImGui()
        {
            ImGui.Begin("Enemy Turn");

            if (ImGui.Button("Finish enemy turn"))
            {
                OnDeciderFinish();
            }

            ImGui.End();
        }

        private void OnDeciderFinish()
        {
            Round.TransitionTo(Round.PickMonster);
        }
    }
}
