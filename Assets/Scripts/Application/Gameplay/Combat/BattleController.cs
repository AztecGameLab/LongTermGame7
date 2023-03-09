namespace Application.Gameplay.Combat
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Application.Gameplay.Combat.UI;
    using Cinemachine;
    using Core;
    using Deciders;
    using Hooks;
    using ImGuiNET;
    using States;
    using UniRx;
    using UnityEngine;

    /// <summary>
    /// The generic controller for all battles.
    /// Handles the common turn sequencing and logic, win and lose conditions.
    /// </summary>
    public class BattleController : MonoBehaviour
    {
        private const int BattleCameraActivePriority = 10;

        private readonly List<Hook> _hooks = new List<Hook>();
        private readonly Subject<Unit> _battleEndSubject = new Subject<Unit>();

        [Header("States")]

        [SerializeField]
        private BattleIntro battleIntro;

        [SerializeField]
        private BattleRound battleRound;

        [SerializeField]
        private BattleLoss battleLoss;

        [SerializeField]
        private BattleVictory battleVictory;

        [Header("Camera References")]

        [SerializeField]
        private CinemachineVirtualCamera battleCamera;

        [SerializeField]
        private CinemachineTargetGroup battleTargetGroup;

        [SerializeField]
        private float battleTargetRadius;

        [Header("UI")]

        [SerializeField]
        private CanvasGroup battleBars;

        [SerializeField]
        private PlayerTeamMemberBattleUI playerBattleUI;

        [SerializeField]
        private EnemyTeamMemberBattleUI enemyBattleUI;

        private List<GameObject> _spawnedUIElements = new List<GameObject>();

        /// <summary>
        /// Gets the battle intro logic.
        /// </summary>
        public BattleIntro Intro => battleIntro;

        /// <summary>
        /// Gets the battle round logic.
        /// </summary>
        public BattleRound Round => battleRound;

        /// <summary>
        /// Gets the battle victory logic.
        /// </summary>
        public BattleVictory Victory => battleVictory;

        /// <summary>
        /// Gets the battle loss logic.
        /// </summary>
        public BattleLoss Loss => battleLoss;

        /// <summary>
        /// Gets the instantiated GameObjects of every member on the player team.
        /// </summary>
        public Collection<GameObject> PlayerTeam { get; } = new Collection<GameObject>();

        /// <summary>
        /// Gets the instantiated GameObjects of every member on the enemy team.
        /// </summary>
        public Collection<GameObject> EnemyTeam { get; } = new Collection<GameObject>();

        /// <summary>
        /// Gets the script in charge of managing the enemy's turn.
        /// </summary>
        public EnemyOrderDecider EnemyOrderDecider { get; private set; }

        /// <summary>
        /// Gets a value indicating whether a battle is currently running.
        /// </summary>
        public bool IsBattling { get; private set; }

        /// <summary>
        /// Gets an observable that watches the end of the battle.
        /// </summary>
        public IObservable<Unit> OnBattleEnd => _battleEndSubject;

        private StateMachine BattleStateMachine { get; set; }

        private CinemachineVirtualCamera BattleCamera => battleCamera;

        /// <summary>
        /// Moves to a new BattleState.
        /// </summary>
        /// <param name="state">The new state to begin running.</param>
        public void TransitionTo(BattleState state)
        {
            BattleStateMachine.SetState(state);
        }

        /// <summary>
        /// Starts running logic for a battle, specified by the passed in data.
        /// </summary>
        /// <param name="data">The initial state needed to start a battle.</param>
        public void BeginBattle(BattleData data)
        {
            if (IsBattling)
            {
                return;
            }

            battleBars.alpha = 1;
            BattleCamera.Priority = BattleCameraActivePriority;
            battleTargetGroup.RemoveAllMembers();
            battleTargetGroup.AddMemberRange(PlayerTeam, 1, battleTargetRadius);
            battleTargetGroup.AddMemberRange(EnemyTeam, 1, battleTargetRadius);

            Debug.Log("Starting battle!");

            battleIntro.Initialize();
            battleRound.Initialize();
            battleVictory.Initialize();
            battleLoss.Initialize();

            _hooks.Clear();
            IsBattling = true;

            foreach (Hook hook in data.Hooks)
            {
                _hooks.Add(hook);
                hook.Controller = this;
                hook.OnBattleStart();
            }

            EnemyOrderDecider = data.Decider;

            PlayerTeam.Clear();

            foreach (GameObject playerTeamInstance in data.PlayerTeamInstances)
            {
                PlayerTeam.Add(playerTeamInstance);
                PlayerTeamMemberBattleUI instance = Instantiate(playerBattleUI, playerTeamInstance.transform);
                instance.BindTo(playerTeamInstance);
                _spawnedUIElements.Add(instance.gameObject);
            }

            EnemyTeam.Clear();

            foreach (GameObject enemyTeamInstance in data.EnemyTeamInstances)
            {
                EnemyTeam.Add(enemyTeamInstance);
                EnemyTeamMemberBattleUI instance = Instantiate(enemyBattleUI, enemyTeamInstance.transform);
                instance.BindTo(enemyTeamInstance);
                _spawnedUIElements.Add(instance.gameObject);
            }

            BattleStateMachine.SetState(battleIntro);
        }

        /// <summary>
        /// Stops running logic for a battle, cleaning up all of the related state.
        /// </summary>
        public void EndBattle()
        {
            IsBattling = false;
            BattleCamera.Priority = 0;
            battleBars.alpha = 0;

            // todo: we may have to pass more information on the ending of battle, e.g. win vs. loss and whatnot
            Debug.Log("Ending battle!");

            foreach (var hook in _hooks)
            {
                hook.OnBattleEnd();
            }

            BattleStateMachine.SetState(null);

            _hooks.Clear();
            PlayerTeam.Clear();
            EnemyTeam.Clear();

            foreach (GameObject spawnedUIElement in _spawnedUIElements)
            {
                Destroy(spawnedUIElement);
            }

            _battleEndSubject.OnNext(Unit.Default);
        }

        private static void AddRange<T>(Collection<T> destination, IEnumerable<T> source)
        {
            foreach (T data in source)
            {
                destination.Add(data);
            }
        }

        private void Awake()
        {
            ImGuiUtil.Register(DrawImGuiWindow).AddTo(this);

            BattleStateMachine = new StateMachine();

            battleLoss.Controller = this;
            battleVictory.Controller = this;
            battleRound.Controller = this;
            battleIntro.Controller = this;
        }

        private void Update()
        {
            if (IsBattling)
            {
                BattleStateMachine.Tick();

                foreach (Hook hook in _hooks)
                {
                    hook.OnBattleUpdate();
                }
            }
        }

        private void DrawImGuiWindow()
        {
            ImGui.Begin("Battle Controller");

            ImGui.Text($"Current State: {BattleStateMachine.CurrentState?.GetType().Name}");

            if (ImGui.Button("End Battle"))
            {
                EndBattle();
            }

            ImGui.Text("Enemy team:");

            foreach (GameObject enemyTeamInstance in EnemyTeam)
            {
                ImGui.Text($"\t{enemyTeamInstance.name}");
            }

            ImGui.Text("Player team:");

            foreach (GameObject playerTeamInstance in PlayerTeam)
            {
                ImGui.Text($"\t{playerTeamInstance.name}");
            }

            ImGui.Text("Hooks:");

            foreach (Hook hook in _hooks)
            {
                ImGui.Text($"\t{hook.GetType().Name}");
            }

            ImGui.End();
        }
    }
}
