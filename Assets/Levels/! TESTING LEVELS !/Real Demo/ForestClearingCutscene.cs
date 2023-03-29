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

        private IEnumerator Start()
        {
            yield return Services.DialogueSystem.RunDialogue(dialogue);
            var waitForEnd = new WaitForCombatToEnd();
            hooks.Add(waitForEnd);
            var message = new OverworldBattleStartData(friendlies, enemies, hooks, orderDecider);
            Services.EventBus.Invoke(message, "Forest Cutscene");
            yield return waitForEnd.Yield();
            yield return Services.DialogueSystem.RunDialogue(endingDialogue);
            print("finished!");
        }

        private class WaitForCombatToEnd : Hook
        {
            private bool _isCombatFinished;

            public override void OnBattleEnd()
            {
                base.OnBattleEnd();
                _isCombatFinished = true;
            }

            public IEnumerator Yield()
            {
                while (!_isCombatFinished)
                {
                    yield return null;
                }
            }
        }
    }
}
