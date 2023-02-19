namespace Application.StateMachine
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// BattleStateController will manage the conditions to trigger transitions between battle states.
    /// </summary>
    public class BattleStateController : MonoBehaviour
    {
        private StateMachine _stateMachine;
        private bool hasLostBattle = false;
        private bool hasWonBattle = false;
        private bool isReadyToPass = false;
        private bool isInitialized = false;

        private void Awake()
        {
            _stateMachine = new StateMachine();

            // Initialize state transitions
            var introState = new BattleIntroState();
            var lossState = new BattleLossState();
            var victoryState = new BattleVictoryState();
            var enemyActivityState = new EnemyActivityState();
            var playerActivityState = new PlayerActivityState();


            // Add discrete transitions
            _stateMachine.AddTransition(introState, playerActivityState, BattleIsInitialized());
            _stateMachine.AddTransition(playerActivityState, enemyActivityState, PlayerReadyToPassTurn());


            // Add ANY transitions
            _stateMachine.AddAnyTransition(lossState, PlayerHasLostBattle());
            _stateMachine.AddAnyTransition(victoryState, PlayerHasWonBattle());

            // Enumerate transition conditions

            Func<bool> PlayerHasLostBattle() => () => hasLostBattle = true;
            Func<bool> PlayerHasWonBattle() => () => hasWonBattle = true;
            Func<bool> BattleIsInitialized() => () => isInitialized = true;
            Func<bool> PlayerReadyToPassTurn() => () => isReadyToPass = true;
        }

        // Update is called once per frame
        private void Update()
        {
            _stateMachine.Tick();
        }
    }
}
