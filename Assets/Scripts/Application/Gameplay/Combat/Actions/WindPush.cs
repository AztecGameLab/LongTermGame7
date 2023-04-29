using Application.Core;
using Application.Core.Abstraction;
using Application.Gameplay.Combat;
using Application.Gameplay.Combat.Actions;
using Application.Gameplay.Combat.UI.Indicators;
using ImGuiNET;
using System;
using System.Collections;
using UnityEngine;
/// <summary>
/// An action that pushes monsters in a certain direction.
/// </summary>

public class WindPush : BattleAction
{

    private Vector3 direction;
    private AimSystem _aimSystem = new AimSystem();

    private IPooledObject<ValidityIndicator> _indicator;


    public override string Name => throw new NotImplementedException();
    public override string Description => throw new NotImplementedException();

    protected override IEnumerator Execute()
    {
        throw new NotImplementedException();
       PhysicsComponent[] Components = UnityEngine.Object.FindObjectsOfType<PhysicsComponent>();

        foreach (PhysicsComponent james in Components)
        {
            james.Velocity = (direction - User.transform.position).normalized;
        }
    }

    public override void PrepEnter()
        {
            base.PrepTick();
            _aimSystem.Initialize();
            _indicator = Services.IndicatorFactory.Borrow<ValidityIndicator>();
        }
    public override void PrepTick()
        {
            direction = _aimSystem.Update().point;
            _indicator.Instance.transform.position = direction;
            IsPrepFinished |= Input.GetKeyDown(KeyCode.Mouse0);
        }

        public override void PrepExit()
        {
            base.PrepExit();
            _indicator.Dispose();
        }
}

