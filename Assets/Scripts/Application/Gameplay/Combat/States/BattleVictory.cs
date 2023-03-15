namespace Application.Gameplay.Combat.States
{
    using System;
    using System.Collections;
    using ElRaccoone.Tweens;
    using UI;
    using UniRx;
    using UnityEngine;

    /// <summary>
    /// The battle state for when the player successfully completes an encounter.
    /// </summary>
    [Serializable]
    public class BattleVictory : BattleState
    {
        [SerializeField]
        private VictoryUI victoryUI;

        /// <summary>
        /// Sets up the battle victory state.
        /// </summary>
        public void Initialize()
        {
            // No initialization needed right now.
        }

        /// <inheritdoc/>
        public override void OnEnter()
        {
            base.OnEnter();
            victoryUI.gameObject.SetActive(true);
            DisposeOnExit(VictoryCoroutine().ToObservable().Subscribe(_ => Controller.EndBattle()));
        }

        /// <inheritdoc/>
        public override void OnExit()
        {
            base.OnExit();
            victoryUI.gameObject.SetActive(false);
        }

        private IEnumerator VictoryCoroutine()
        {
            yield return victoryUI.victoryText.TweenCanvasGroupAlpha(1, 1);
            yield return new WaitUntil(() => Input.anyKeyDown);
            yield return victoryUI.victoryText.TweenCanvasGroupAlpha(0, 1);
        }
    }
}
