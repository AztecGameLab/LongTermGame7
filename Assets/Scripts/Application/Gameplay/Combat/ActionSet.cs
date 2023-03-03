using System.Collections.Generic;

namespace Application.Gameplay.Combat
{
    using UnityEngine;

    public class ActionSet : MonoBehaviour
    {
        [SerializeReference]
        public List<BattleAction> actions;
    }

    public interface IDebugImGui
    {
        void RenderImGui();
    }

    public interface IDebugGizmos
    {
        void RenderGizmos();
    }
}