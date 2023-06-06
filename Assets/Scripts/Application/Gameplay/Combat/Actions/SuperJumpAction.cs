using Application.Core;
using Application.Core.Abstraction;
using Application.Gameplay.Combat;
using Application.Gameplay.Combat.Actions;
using Application.Gameplay.Combat.UI.Indicators;
using ImGuiNET;
using System;
using System.Collections;
using UnityEngine;

public class SuperJumpAction : BattleAction, IDebugImGui
{
    [SerializeField]
    private int apCost = 4;
    public override string Name => "Super Jump Anywhere";
    public override string Description => "Jumps across the stage with ease";
    public override int Cost => apCost;

    private Vector3 LandingSpot;
    private AimSystem _aimSystem = new AimSystem();
    private IPooledObject<ValidityIndicator> _indicator;

    protected override IEnumerator Execute()
    {
        Debug.Log("Executing debugging action...");
        ActionTracker.Spend(apCost);
        Debug.Log("Points were spent");
        Debug.Log(User.name);
        //Calculating the jump
        var launchVelocity = ProjectileMotion.GetLaunchVelocity(User.transform.position, LandingSpot);
        Debug.Log(launchVelocity);
        PhysicsComponent physics = User.GetComponent<PhysicsComponent>();
        // User.transform.position = LandingSpot;

        //Jumping
        physics.Velocity = launchVelocity;
        Debug.Log(launchVelocity);

        Debug.Log("Jumped to spot!");
        yield return null;
        Debug.Log("Done!");
    }

    public override void PrepEnter()
    {
        base.PrepTick();
        _aimSystem.Initialize();
        _indicator = Services.IndicatorFactory.Borrow<ValidityIndicator>();
    }

    public override void PrepTick()
    {
        LandingSpot = _aimSystem.Update().point;
        _indicator.Instance.transform.position = LandingSpot;
        IsPrepFinished |= Input.GetKeyDown(KeyCode.Mouse0);
    }

    public override void PrepExit()
    {
        base.PrepExit();
        _indicator.Dispose();
    }
    public void RenderImGui()
    {
        ImGui.Text($"Position: {LandingSpot}");
    }
}
