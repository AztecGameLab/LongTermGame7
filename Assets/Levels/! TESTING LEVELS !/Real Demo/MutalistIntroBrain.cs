using Application.Core.Utility;
using System;
using System.Collections.Generic;
using UniRx;

namespace Levels.__TESTING_LEVELS__.Real_Demo
{
    using System.Collections;
    using Application.Gameplay.Combat;
    using Application.Gameplay.Combat.Brains;
    using UnityEngine;

    public abstract class AttackEmission : MonoBehaviour
    {
        public abstract IObservable<Unit> ObserveFinished();

        public List<GameObject> IgnoreList { get; } = new List<GameObject>();
    }

    /// <summary>
    /// The brain for the biomutalist in the intro cutscene.
    /// </summary>
    public class MutalistIntroBrain : MonsterBrain
    {
        [SerializeField] private AttackEmission lightAttackProjectile;
        [SerializeField] private AttackEmission multiAttackProjectile;
        [SerializeField] private AttackEmission beamAttack;

        public enum Stage
        {
            LightAttack,
            MultiAttack,
            FinisherAttack,
        }

        public Stage CurrentStage = Stage.LightAttack;
        private BattleController _controller;

        /// <inheritdoc/>
        protected override IEnumerator MakeDecision(BattleController controller)
        {
            _controller = controller;

            switch (CurrentStage)
            {
                case Stage.LightAttack:
                    yield return LightAttack().ToObservable().ToYieldInstruction();
                    break;
                case Stage.MultiAttack:
                    yield return MultiAttack().ToObservable().ToYieldInstruction();
                    break;
                case Stage.FinisherAttack:
                    yield return FinisherAttack().ToObservable().ToYieldInstruction();
                    break;
            }
        }

            // By default, shoots a light purple energy ball
        private IEnumerator LightAttack()
        {
            var target = _controller.PlayerTeam[Random.Range(0, _controller.PlayerTeam.Count)];
            var vectorToTarget = target.transform.position - transform.position;
            var instance = Instantiate(lightAttackProjectile, transform.position + Vector3.up, Quaternion.LookRotation(vectorToTarget));
            instance.IgnoreList.Add(gameObject);
            yield return instance.ObserveFinished().ToYieldInstruction();
        }

            // slightly angrier, begins to shoot multiple balls, that track and hit each.
        private IEnumerator MultiAttack()
        {
            yield return null;
        }

            // final stage, light laser beam that one shots each in path
        private IEnumerator FinisherAttack()
        {
            yield return null;
        }
    }
}