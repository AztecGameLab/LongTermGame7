using ImGuiNET;
using System;
using System.Collections;
using UnityEngine;

enum DIRECTION
{
    NORTH=0,
    EAST=1,
    SOUTH=2,
    WEST=3
}

namespace Application.Gameplay.Combat
{
    [Serializable]
    public class RockPlatformStairsAction : BattleAction, IDebugImGui
    {
        [SerializeField] private string name = "Rock Platform/Stairs";
        [SerializeField] private string description = "Summon stairs and move up them";
        [SerializeField] private int apCost = 4;

        public override string Name => name;
        public override string Description => description;

        private bool _lockedIn;
        DIRECTION dir;
        private float _stairsDistance = 1;// distance away from monster for stairs to spawn
        private float _stairsHeight = 1;// hieght of stairs for monster to be placed on
        private Vector3 _targetPosition;//pos of stairs and new pos of player
        private Vector3[] _directionVector = new Vector3[4];//pos of stairs and new pos of player

        protected override IEnumerator Execute()
        {
            Debug.Log("Executing Rock Platform/Stairs Action...");

            //get stairs possition
            _targetPosition = _targetPosition + _directionVector[(int)dir];

            //summon stairs
            //  \_^-^_/

            //move up stairs
            _targetPosition = _targetPosition + new Vector3(0, _stairsHeight,0);
            User.transform.position = _targetPosition;

            //remove action points and exit
            if (User.TryGetComponent(out ActionPointTracker apTracker))
                apTracker.remainingActionPoints -= apCost;

            yield return new WaitForSeconds(1);
            Debug.Log("Done!");
        }

        

        public override void PrepEnter()
        {
            //=>_lockedIn = false;

            //get direction vectors
            _directionVector[(int)DIRECTION.NORTH].Set(0, 0, _stairsDistance);
            _directionVector[(int)DIRECTION.EAST].Set(_stairsDistance, 0, 0);
            _directionVector[(int)DIRECTION.SOUTH].Set(0, 0, -_stairsDistance);
            _directionVector[(int)DIRECTION.WEST].Set(-_stairsDistance, 0, 0);

            //check if each space occupied if is then remove from list
        }

        public override bool PrepTick() =>_lockedIn;

        public void RenderImGui()
        {
            ImGui.Text($"Choose Direction of the Stairs");
            ImGui.Text($"==============================");
            if (ImGui.Button("North"))
            {
                //set loc of stairs to north
                dir = DIRECTION.NORTH;
            }
            if (ImGui.Button("East"))
            {
                //set loc of stairs to east
                dir = DIRECTION.EAST;
            }
            if (ImGui.Button("South"))
            {
                //set loc of stairs to south
                dir = DIRECTION.SOUTH;
            }
            if (ImGui.Button("West"))
            {
                //set loc of stairs to west
                dir = DIRECTION.WEST;
            }
            ImGui.Text($"Action Point Cost: {apCost}");

            _lockedIn = true;   
        }
    }
}