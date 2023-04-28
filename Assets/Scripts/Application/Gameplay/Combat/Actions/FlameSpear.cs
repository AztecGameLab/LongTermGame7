using Application.Core;
using Application.Core.Utility;
using Application.Gameplay.Combat.Effects;
using Application.Gameplay.Combat.UI.Indicators;
using System.Collections;
using UnityEngine;

namespace Application.Gameplay.Combat.Actions
{
    public class FlameSpear : BattleAction
    {
        [SerializeField] private int apCost = 3;
        [SerializeField] private float spread = 30;
        [SerializeField] private float distance = 5;
        [SerializeField] private BurningSettings burnSettings;
        [SerializeField] private float initialDamage = 1;
        [SerializeField] private LayerMask damageMask;

        public override string Name => "Flame Spear";
        public override string Description => "A jet of flame expels from this menace, hurting enemies and catching them on fire";

        private IPooledObject<SliceIndicator> _sliceIndicator;
        private AimSystem _aimSystem = new AimSystem();
        private Vector3 _targetPoint;
        private Collider[] _resultBuffer = new Collider[100];

        public override void PrepEnter()
        {
            base.PrepEnter();
            _sliceIndicator = Services.IndicatorFactory.Borrow<SliceIndicator>();
            _aimSystem.Initialize(groundSnap: false);
            DisposeOnExit(_sliceIndicator);
        }

        public override void PrepTick()
        {
            base.PrepTick();
            var hitInfo = _aimSystem.Update();
            var position = User.transform.position;
            _sliceIndicator.Instance.UpdateView(position, hitInfo.point - position, distance, spread);
            _targetPoint = hitInfo.point;

            IsPrepFinished |= Input.GetKeyDown(KeyCode.Mouse0) && ActionTracker.CanAfford(apCost);
        }

        protected override IEnumerator Execute()
        {
            ActionTracker.Spend(apCost);
            var position = User.transform.position;
            int hits = Scanner.GetAllInSlice(position, _targetPoint - position, spread, distance, damageMask, _resultBuffer);

            for (int i = 0; i < hits; i++)
            {
                var cur = _resultBuffer[i].GetRoot();

                if (cur.TryGetComponent(out LivingEntity entity))
                {
                    entity.Damage(initialDamage);
                    Controller.EffectApplier.ApplyBurning(cur, burnSettings);
                }
            }

            yield return null;
        }
    }
}
