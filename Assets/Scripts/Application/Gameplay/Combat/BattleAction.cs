using System;
using System.Collections;
using UniRx;
using UnityEngine;

namespace Application.Gameplay.Combat
{
    [Serializable]
    public abstract class BattleAction
    {
        public abstract string Name { get; }
        public abstract string Description { get; }
        
        public GameObject User { get; set; }

        public virtual void PrepEnter() {}
        public virtual void PrepExit() {}
        public virtual bool PrepTick() { return false; }

        protected abstract IEnumerator Execute();
        public IObservable<Unit> Run() => Execute().ToObservable();
    }
}