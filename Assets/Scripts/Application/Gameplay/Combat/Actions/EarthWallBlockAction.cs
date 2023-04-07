using ImGuiNET;
using System;
using System.Collections;
using UnityEngine;

namespace Application.Gameplay.Combat
{
    [Serializable]
    public class EarthWallBlockAction : BattleAction, IDebugImGui
    {
        [SerializeField] 
        private int apCost = 2;
        public override string Name => "Earth Wall Block";
        public override string Description => "Place a wall of earth down that can be sent towards enemies if hit";

        [SerializeField] 

        //Getting the position in front of the monster. In PrepTick, we'll use it.
        private Vector3 EarthWallTarget;
        //_PrefabSpawnPosition = user.transform.position;    //Need to find out how to get position of monster
        protected override IEnumerator Execute()
        {
            Debug.Log("Executing debugging action...");

            if (User.TryGetComponent(out ActionPointTracker apTracker))
                apTracker.remainingActionPoints -= apCost;
            
            yield return null;
            Debug.Log("Done!");
        }

        public override void PrepEnter()
        {
            //Getting the position forward to the monster to place Earth Wall there.
            _PrefabSpawnPosition += Vector3.forward * Time.deltaTime;

            //Need to adjust later in to match EarthWall placement so that it's always forward of wherever the monster is facing
        }

        public override bool PrepTick(){
            //Spawn prefab at _PrefabSpawnPosition
            return true;
        }

        public void RenderImGui()
        {
            // if (ImGui.Button("Lock in demo action"));
        }
    }
}