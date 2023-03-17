namespace Application.Gameplay.Combat.States
{
    using System;
    using System.Collections;
    using ElRaccoone.Tweens;
    using UI;
    using UniRx;
    using UnityEngine;

    /// <summary>
    /// The battle round state where the player loses the battle.
    /// </summary>
    [Serializable]
    public class BattleLoss : BattleState
    {
        [SerializeField]
        private DefeatUI defeatUI;

        /// <summary>
        /// Sets up the battle loss state.
        /// </summary>
        public void Initialize()
        {
            // No initialization needed right now.
        }

        /// <inheritdoc/>
        public override void OnEnter()
        {
            base.OnEnter();
            defeatUI.gameObject.SetActive(true);
            DisposeOnExit(VictoryCoroutine().ToObservable().Subscribe(_ => Controller.EndBattle()));
        }

        /// <inheritdoc/>
        public override void OnExit()
        {
            base.OnExit();
            defeatUI.gameObject.SetActive(false);
        }

        private IEnumerator VictoryCoroutine()
        {
            yield return defeatUI.defeatText.TweenCanvasGroupAlpha(1, 1).Yield();
            yield return new WaitUntil(() => Input.anyKey);
            yield return defeatUI.defeatText.TweenCanvasGroupAlpha(0, 1).Yield();
        }
    }
}
