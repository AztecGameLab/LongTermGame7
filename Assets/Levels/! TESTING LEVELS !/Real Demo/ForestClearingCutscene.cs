using System;
using UniRx;
using UniRx.Diagnostics;

namespace Levels.__TESTING_LEVELS__.Real_Demo
{
    using System.Collections;
    using System.Collections.Generic;
    using Application.Core;
    using Application.Gameplay.Combat;
    using Application.Gameplay.Combat.Deciders;
    using Application.Gameplay.Combat.Hooks;
    using UnityEngine;
    using Yarn.Unity;

    public abstract class Popup : MonoBehaviour
    {
        public abstract IEnumerator Show();
    }

    /// <summary>
    /// A cutscene that plays out in the forest clearing level.
    /// </summary>
    public class ForestClearingCutscene : MonoBehaviour
    {
        [SerializeReference]
        private List<Hook> hooks;

        [SerializeField]
        private List<GameObject> friendlies;

        [SerializeField]
        private List<GameObject> enemies;

        [SerializeField]
        private EnemyOrderDecider orderDecider;

        [SerializeField]
        private DialogueReference dialogue;

        [SerializeField]
        private DialogueReference endingDialogue;

        [SerializeField] private MutalistIntroBrain brain;

        [SerializeField] private DialogueReference mutalistMidStage;
        [SerializeField] private DialogueReference mutalistFinalStage;
        [SerializeField] private DialogueReference mutalistVictory;

        [SerializeField] private Popup selectionHint;
        [SerializeField] private Popup abilityHint;
        [SerializeField] private Popup battleObjectiveHint;

        private IEnumerator Start()
        {
            yield return Services.DialogueSystem.RunDialogue(dialogue);
            var customHook = new CustomHook
                {
                    AbilityHint = abilityHint.Show,
                    SelectionHint = selectionHint.Show,
                    BattleObjectiveHint = battleObjectiveHint.Show,
                    MutalistMidStage = MidStage,
                    MutalistFinalStage = FinalStage,
                    MutalistVictory = () => Services.DialogueSystem.RunDialogue(mutalistVictory),
                };
            hooks.Add(customHook);
            var message = new OverworldBattleStartData(friendlies, enemies, hooks, orderDecider);
            Services.EventBus.Invoke(message, "Forest Cutscene");
            yield return customHook.YieldUntilBattleEnd();
            yield return Services.DialogueSystem.RunDialogue(endingDialogue);
        }

        private IEnumerator MidStage()
        {
            brain.CurrentStage = MutalistIntroBrain.Stage.MultiAttack;
            yield return Services.DialogueSystem.RunDialogue(mutalistMidStage);
        }

        private IEnumerator FinalStage()
        {
            brain.CurrentStage = MutalistIntroBrain.Stage.FinisherAttack;
            yield return Services.DialogueSystem.RunDialogue(mutalistFinalStage);
        }

        private sealed class CustomHook : Hook
        {
            public Func<IEnumerator> SelectionHint;
            public Func<IEnumerator> AbilityHint;
            public Func<IEnumerator> BattleObjectiveHint;
            public Func<IEnumerator> MutalistMidStage;
            public Func<IEnumerator> MutalistFinalStage;
            public Func<IEnumerator> MutalistVictory;

            private bool _isCombatFinished;

            public override IEnumerator OnBattleStart()
            {
                yield return base.OnBattleStart();

                Controller.Round.PickMonster.ObserveEntered()
                    .Take(1)
                    .Subscribe(_ => Controller.StartCoroutine(SelectionHint.Invoke()));

                Controller.Round.PickActions.ObserveEntered()
                    .Take(1)
                    .Subscribe(_ => Controller.StartCoroutine(AbilityHint.Invoke()));

                Controller.Round.RoundNumber
                    .Where(round => round == 2)
                    .Subscribe(_ => Controller.StartCoroutine(BattleObjectiveHint.Invoke()));

                Controller.Round.EnemyMoveMonsters.ObserveEntered()
                    .Where(_ => Controller.Round.RoundNumber.Value == 2)
                    .Take(1)
                    .Subscribe(_ => Controller.Interrupts.Enqueue(MutalistMidStage));

                Controller.Round.EnemyMoveMonsters.ObserveEntered()
                    .Where(_ => Controller.Round.RoundNumber.Value == 3)
                    .Take(1)
                    .Subscribe(_ => Controller.Interrupts.Enqueue(MutalistFinalStage));

                Controller.Round.ObserveExited()
                    .Take(1)
                    .Subscribe(_ => Controller.Interrupts.Enqueue(MutalistVictory));
            }

            public override IEnumerator OnBattleEnd()
            {
                yield return base.OnBattleEnd();
                _isCombatFinished = true;
            }

            public IEnumerator YieldUntilBattleEnd()
            {
                while (!_isCombatFinished)
                {
                    yield return null;
                }
            }
        }
    }
}
