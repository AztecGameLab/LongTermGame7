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
                };
            hooks.Add(customHook);
            var message = new OverworldBattleStartData(friendlies, enemies, hooks, orderDecider);
            Services.EventBus.Invoke(message, "Forest Cutscene");
            yield return customHook.YieldUntilBattleEnd();
            yield return Services.DialogueSystem.RunDialogue(endingDialogue);
        }

        private sealed class CustomHook : Hook
        {
            public Func<IEnumerator> SelectionHint;
            public Func<IEnumerator> AbilityHint;
            public Func<IEnumerator> BattleObjectiveHint;

            private bool _isCombatFinished;

            public override IEnumerator OnBattleStart()
            {
                yield return base.OnBattleStart();

                Controller.Round.PickMonster.ObserveEntered()
                    .Take(1)
                    .Subscribe(_ => Controller.Interrupts.Enqueue(SelectionHint));

                Controller.Round.PickActions.ObserveEntered()
                    .Take(1)
                    .Subscribe(_ => Controller.Interrupts.Enqueue(AbilityHint));

                Controller.Round.RoundNumber
                    .Where(round => round == 2)
                    .Subscribe(_ => Controller.Interrupts.Enqueue(BattleObjectiveHint));
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
