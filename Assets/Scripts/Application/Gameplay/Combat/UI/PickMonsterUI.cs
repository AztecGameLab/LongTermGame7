namespace Application.Gameplay.Combat.UI
{
    using System;
    using System.Collections.Generic;
    using Core;
    using TMPro;
    using UniRx;
    using UnityEngine;

    /// <summary>
    /// A user-interface for displaying selected monster information,
    /// and allowing the user to select different monsters.
    /// </summary>
    public class PickMonsterUI : MonoBehaviour
    {
        private const int SelectionAngle = 90;

        private readonly Subject<GameObject> _monsterSubmitted = new Subject<GameObject>();

        [SerializeField]
        private TMP_Text selectedMonsterText;

        private CompositeDisposable _disposable;

        /// <summary>
        /// Gets the monster that is currently selected.
        /// </summary>
        public ReactiveProperty<GameObject> SelectedMonster { get; private set; }

        /// <summary>
        /// Gets an observable that changes each time the user selects a monster via the UI.
        /// </summary>
        /// <returns>An observable that changes each time the user selects a monster via the UI.</returns>
        public IObservable<GameObject> ObserveMonsterSubmitted() => _monsterSubmitted;

        /// <summary>
        /// Updates the UI based on the available monsters that can be selected.
        /// </summary>
        /// <param name="available">The monsters that can be selected.</param>
        public void Tick(IEnumerable<GameObject> available)
        {
            var direction = Vector3.zero;

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                direction = Vector3.forward;
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                direction = Vector3.back;
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                direction = Vector3.left;
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                direction = Vector3.right;
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                _monsterSubmitted.OnNext(SelectedMonster.Value);
            }

            if (direction != Vector3.zero && SelectedMonster.Value != null)
            {
                var closest = FindClosestInDirection(SelectedMonster.Value.transform.position, direction, available);

                if (closest != null)
                {
                    SelectedMonster.Value = closest;
                }
            }
        }

        private void Awake()
        {
            SelectedMonster = new ReactiveProperty<GameObject>();

            SelectedMonster
                .Subscribe(monster =>
                {
                    string monsterName = monster != null ? monster.name : "None";
                    selectedMonsterText.text = $"Selected {monsterName}";
                })
                .AddTo(this);
        }

        private GameObject FindClosestInDirection(Vector3 origin, Vector3 direction, IEnumerable<GameObject> objects)
        {
            GameObject result = null;
            float best = float.PositiveInfinity;

            foreach (GameObject obj in objects)
            {
                var position = obj.transform.position;
                var dirToTarget = position - origin;

                if (obj != SelectedMonster.Value && Vector3.Angle(direction, dirToTarget) <= SelectionAngle && dirToTarget.sqrMagnitude < best)
                {
                    best = dirToTarget.sqrMagnitude;
                    result = obj;
                }
            }

            return result;
        }
    }
}
