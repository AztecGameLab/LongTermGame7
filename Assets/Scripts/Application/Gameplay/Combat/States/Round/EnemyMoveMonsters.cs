using Application.Core;
using Cinemachine;
using ImGuiNET;
using System;
using UnityEngine;

namespace Application.Gameplay.Combat.States.Round
{
    [Serializable]
    public class EnemyMoveMonsters : RoundState
    {
        public float enemyRadius = 3;
        
        public override void OnEnter()
        {
            base.OnEnter();
            Services.EventBus.Invoke(new RoundStateEnterEvent<EnemyMoveMonsters>{State = this}, "Enemy Move Monsters State");

            var group = Round.Controller.TargetGroup;
            Round.Controller.BattleCamera.Follow = group.transform;

            foreach (GameObject enemy in Round.Controller.EnemyTeam)
            {
                group.AddMember(enemy.transform, 1, enemyRadius);
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            Services.EventBus.Invoke(new RoundStateExitEvent<EnemyMoveMonsters>{State = this}, "Enemy Move Monsters State");

            foreach (GameObject enemy in Round.Controller.EnemyTeam)
            {
                Round.Controller.TargetGroup.RemoveMember(enemy.transform);
            }
        }

        private void OnDeciderFinish()
        {
            Round.StateMachine.SetState(Round.PickMonster);
        }
        
        protected override void DrawGui()
        {
            ImGui.Begin("Enemy Turn");
            
            if (ImGui.Button("Finish enemy turn"))
                OnDeciderFinish();
            
            ImGui.End();
        }
    }
}