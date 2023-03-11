using Application.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Application.Gameplay.Combat.UI
{
    public class PickMonsterUI : MonoBehaviour
    {
        private readonly Subject<GameObject> _monsterSubmitted = new Subject<GameObject>();

        private CompositeDisposable _disposable;
        
        public IObservable<GameObject> ObserveMonsterSubmitted() => _monsterSubmitted;

        public ReactiveProperty<GameObject> SelectedMonster { get; } = new ReactiveProperty<GameObject>();

        public void Tick(Vector3 sourcePosition, IEnumerable<GameObject> available)
        {
            var direction = Vector3.zero;

            if (Input.GetKeyDown(KeyCode.UpArrow)) direction = Vector3.forward;
            if (Input.GetKeyDown(KeyCode.DownArrow)) direction = Vector3.back;
            if (Input.GetKeyDown(KeyCode.LeftArrow)) direction = Vector3.left;
            if (Input.GetKeyDown(KeyCode.RightArrow)) direction = Vector3.right;
            
            if (Input.GetKeyDown(KeyCode.Return))
                _monsterSubmitted.OnNext(SelectedMonster.Value);

            if (direction != Vector3.zero)
            {
                 var closest = FindClosestInDirection(sourcePosition, direction, available);

                if (closest != null)
                {
                    SelectedMonster.Value = closest;
                }
            }
        }

        private GameObject FindClosestInDirection(Vector3 origin, Vector3 direction, IEnumerable<GameObject> objects)
        {
            GameObject result = null;
            float best = float.PositiveInfinity;

            foreach (GameObject obj in objects)
            {
                var position = obj.transform.position;
                var dirToTarget = position- origin;

                if (obj != SelectedMonster.Value && Vector3.Angle(direction, dirToTarget) <= 90 && dirToTarget.sqrMagnitude < best)
                {
                    best = dirToTarget.sqrMagnitude;
                    result = obj;
                }
            }

            return result;
        }
    }
}