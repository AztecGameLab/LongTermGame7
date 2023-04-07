using Application.Core;
using Application.Core.Abstraction;
using Application.Gameplay.Combat;
using Application.Gameplay.Combat.Actions;
using ImGuiNET;
using System;
using System.Collections;
using UnityEngine;

 // Remember to use namespace
public class SuperJumpAction : BattleAction, IDebugImGui
{
    [SerializeField] 
    private int apCost = 4;
    public override string Name => "Super Jump Anywhere";
    public override string Description => "Jumps across the stage with ease";

    private Vector3 LandingSpot;
    private AimSystem _aimSystem = new AimSystem();

protected override IEnumerator Execute()
    {
        Debug.Log("Executing debugging action...");

        if (User.TryGetComponent(out ActionPointTracker apTracker))
            apTracker.TrySpend(apCost);
            Debug.Log("Points were spent");

            //Calculating the jump
            var launchVelocity = ProjectileMotion.GetLaunchVelocity(User.transform.position, LandingSpot);
            PhysicsComponent physics = User.GetComponent<PhysicsComponent>();
            
            //Jumping
            physics.Velocity = launchVelocity;

            Debug.Log("Jumped to spot!");
            yield return null;
            Debug.Log("Done!");
    }

        public override void PrepEnter()
        {
            base.PrepTick();
            _aimSystem.Initialize();
        }

        public override void PrepTick()
        {
            LandingSpot = _aimSystem.Update().point;
            IsPrepFinished |= Input.GetKeyDown(KeyCode.Mouse0);
        }

        public override void PrepExit()
        {
            base.PrepExit();
        }
        public void RenderImGui()
        {
            ImGui.Text($"Position: {LandingSpot}");
        }
    }
