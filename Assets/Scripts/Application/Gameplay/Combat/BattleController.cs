using Application.Gameplay.Combat.Effects;
using ElRaccoone.Tweens;

namespace Application.Gameplay.Combat
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Cinemachine;
    using Core;
    using Core.Utility;
    using Deciders;
    using Hooks;
    using ImGuiNET;
    using States;
    using UI;
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
        private readonly List<GameObject> _spawnedUIElements = new List<GameObject>();

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
        private CinemachineVirtualCamera[] cameras;

        [SerializeField]
        private CinemachineTargetGroup battleTargetGroup;

        [SerializeField]
        private float battleTargetRadius;

        [Header("UI")]

        [SerializeField]
        private CanvasGroup battleBars;

        [SerializeField]
        private CanvasGroup battleUi;

        private Transform _trueFollow;

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

        public EffectApplier EffectApplier { get; private set; }

        public Queue<Func<IEnumerator>> Interrupts { get; } = new Queue<Func<IEnumerator>>();

        /// <summary>
        /// Gets a value indicating whether a battle is currently running.
        /// </summary>
        public bool IsBattling { get; private set; }

        /// <summary>
        /// Gets the current state that this battle controller is in.
        /// </summary>
        public IState CurrentState => BattleStateMachine.CurrentState;

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

        public IDisposable TemporaryFollow(Transform target)
        {
            return new FollowDisposable { value = AddFollowStack(target), parent = this };
        }

        private LinkedListNode<Transform> AddFollowStack(Transform target)
        {
            LinkedListNode<Transform> node = _followStack.AddFirst(target);
            _trueFollow.position = target.position;
            _trueFollow.SetParent(target);
            return node;
        }

        private void RemoveFollowStack(LinkedListNode<Transform> value)
        {
            _followStack.Remove(value);

            if (_followStack.Count > 0)
            {
                _trueFollow.position = _followStack.First.Value.position;
                _trueFollow.SetParent(_followStack.First.Value);
            }
        }

        private LinkedList<Transform> _followStack = new LinkedList<Transform>();

        private struct FollowDisposable : IDisposable
        {
            public BattleController parent;
            public LinkedListNode<Transform> value;
            private bool _disposed;

            public void Dispose()
            {
                if (!_disposed)
                {
                    _disposed = true;
                    parent.RemoveFollowStack(value);
                }
            }
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

            gameObject.SetActive(true);
            StartCoroutine(BattleCoroutine(data));
        }

        /// <summary>
        /// Stops running logic for a battle, cleaning up all of the related state.
        /// </summary>
        public void EndBattle()
        {
            IsBattling = false;
        }

        private void Awake()
        {
            ImGuiUtil.Register(DrawImGuiWindow).AddTo(this);
            _trueFollow = new GameObject("Camera Follow").transform;
            _trueFollow.SetParent(transform);
            battleCamera.Follow = _trueFollow;
            AddFollowStack(battleTargetGroup.Transform);

            BattleStateMachine = new StateMachine();
            EffectApplier = new EffectApplier(this);

            battleLoss.Controller = this;
            battleVictory.Controller = this;
            battleRound.Controller = this;
            battleIntro.Controller = this;
        }

        private IEnumerator BattleCoroutine(BattleData data)
        {
            // todo: move this into a hook for the overworldbattle, or something
            var worldLoader = FindObjectOfType<PlayerSpawn>();

            if (worldLoader)
            {
                worldLoader.MonsterFollowPlayer.Enabled = false;
            }

            battleBars.alpha = 1;
            BattleCamera.Priority = BattleCameraActivePriority;
            battleTargetGroup.RemoveAllMembers();
            Interrupts.Clear();

            battleIntro.Initialize();
            battleRound.Initialize();
            battleVictory.Initialize();
            battleLoss.Initialize();

            IsBattling = true;

            EnemyOrderDecider = data.Decider;

            PlayerTeam.Clear();

            foreach (GameObject playerTeamInstance in data.PlayerTeamInstances)
            {
                PlayerTeam.Add(playerTeamInstance);
                playerTeamInstance.GetComponentInChildren<PlayerTeamMemberBattleUI>(true).gameObject.SetActive(true);
                battleTargetGroup.AddMember(playerTeamInstance.transform, 1, battleTargetRadius);
            }

            EnemyTeam.Clear();

            foreach (GameObject enemyTeamInstance in data.EnemyTeamInstances)
            {
                EnemyTeam.Add(enemyTeamInstance);
                enemyTeamInstance.GetComponentInChildren<EnemyTeamMemberBattleUI>(true).gameObject.SetActive(true);
                battleTargetGroup.AddMember(enemyTeamInstance.transform, 1, battleTargetRadius);
            }

            _hooks.Clear();

            foreach (Hook hook in data.Hooks)
            {
                _hooks.Add(hook);
                hook.Controller = this;
                yield return hook.OnBattleStart();
            }

            BattleStateMachine.SetState(battleIntro);

            while (IsBattling)
            {
                foreach (Hook hook in _hooks)
                {
                    hook.OnBattleUpdate();
                }

                if (Interrupts.Count > 0)
                {
                    battleUi.interactable = false;
                    battleUi.blocksRaycasts = false;
                    yield return battleUi.TweenCanvasGroupAlpha(0, 0.25f).Yield();

                    while (Interrupts.Count > 0)
                    {
                        yield return Interrupts.Dequeue().Invoke();
                    }

                    yield return battleUi.TweenCanvasGroupAlpha(1, 0.25f).Yield();
                    battleUi.interactable = true;
                    battleUi.blocksRaycasts = true;
                }


                BattleStateMachine.Tick();

                yield return null;
            }

            BattleCamera.Priority = 0;
            battleBars.alpha = 0;

            // todo: move this into a hook for the overworldbattle, or something
            if (worldLoader)
            {
                worldLoader.MonsterFollowPlayer.Enabled = true;
            }

            BattleStateMachine.SetState(null);

            foreach (var hook in _hooks)
            {
                yield return hook.OnBattleEnd();
            }

            _hooks.Clear();
            PlayerTeam.Clear();
            EnemyTeam.Clear();

            foreach (GameObject spawnedUIElement in _spawnedUIElements)
            {
                Destroy(spawnedUIElement);
            }

            _battleEndSubject.OnNext(Unit.Default);
            gameObject.SetActive(false);
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
