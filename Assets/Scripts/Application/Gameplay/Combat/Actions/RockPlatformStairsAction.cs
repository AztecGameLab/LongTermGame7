using System;    
using System.Collections;
using Core;
using Core.Utility;
using ImGuiNET;
using UI.Indicators;
using UnityEngine;

namespace Application.Gameplay.Combat.Actions
{
    [Serializable]
    public class RockPlatformStairsAction : BattleAction, IDebugImGui
    {
        [SerializeField] private string name = "Rock Platform/Stairs";
        [SerializeField] private string description = "Summon stairs and move up them";
        [SerializeField] private int apCost = 4;

        public override string Name => name;
        public override string Description => description;

        public GameObject StairsPrefab;//object for stairs
        public GameObject Stairs;
        private AimSystem _aimSystem = new AimSystem();
        private Vector3 _targetPosition;//pos of stairs and new pos of player w/o new hight
        public float maxRange = 5f; //max range the stairs can be placed away from the player
        public float stairsHeight = 1.5f;

        public override void PrepEnter()
        {
            _aimSystem.Initialize(Camera.main);
        }

        public override void PrepTick()
        {
            //get where mouse is
            _targetPosition = _aimSystem.Update().point;

            //check if the stair placement will be in range/the space is not occupied
            float distance = Vector3.Distance(_targetPosition, User.transform.position);

            if (distance <= maxRange && true)//replace true with check for avaiblity
            {
                IsPrepFinished |= Input.GetKeyDown(KeyCode.Mouse0);
            }
        }

        public void RenderImGui()
        {
            ImGui.Text($"Choose Place of the Stairs");
            ImGui.Text($"Position: {_targetPosition}");
            ImGui.Text($"Action Point Cost: {apCost}");
        }

        protected override IEnumerator Execute()
        {
            //summon stairs
            Stairs = Instantiate(StairsPrefab, _targetPosition) as GameObject;
            //place stairs
            //Stairs.transform.position(_targetPosition);

            //move up stairs
            _targetPosition = _targetPosition + new Vector3(0, stairsHeight, 0);
            User.transform.position = _targetPosition;

            //remove action points and exit
            if (User.TryGetComponent(out ActionPointTracker apTracker))
                apTracker.remainingActionPoints -= apCost;

            yield return new WaitForSeconds(1);
            Debug.Log("Done!");
        }
    }
}