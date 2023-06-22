namespace Application.Gameplay.Combat.States.Round
{
    using System;
    using System.Collections.Generic;
    using Cinemachine;
    using Core;
    using Core.Utility;
    using UI;
    using UniRx;
    using UnityEngine;

    /// <summary>
    /// The battle round state where you select which monster you want to use in combat.
    /// </summary>
    [Serializable]
    public class PickMonster : RoundState
    {
        private const int PickMonsterCameraActivePriority = 20;

        private HashSet<GameObject> _usedMonsters;

        [SerializeField]
        private PickMonsterUI pickMonsterUI;

        [SerializeField]
        private GameObject selectedMonsterUI;

        private GameObject _selectionUIInstance;
        private int _selectedMonsterIndex;
        private CompositeDisposable _disposable;

        /// <summary>
        /// Gets the currently selected monster.
        /// </summary>
        public ReactiveProperty<GameObject> SelectedMonster { get; private set; } = new ReactiveProperty<GameObject>();

        private IList<GameObject> PlayerTeam => Round.Controller.PlayerTeam;

        /// <summary>
        /// Sets up the pick monster state.
        /// </summary>
        public void Initialize()
        {
            _usedMonsters = new HashSet<GameObject>();
            SelectedMonster = new ReactiveProperty<GameObject>();
            SelectedMonster.Subscribe(HandleSelectedMonsterChange);
        }

        /// <inheritdoc/>
        public override void OnRoundBegin()
        {
            _usedMonsters.Clear();
            _selectedMonsterIndex = 0;
        }

        private IDisposable _cameraDisposable;

        /// <inheritdoc/>
        public override void OnEnter()
        {
            base.OnEnter();

            if (PlayerTeam.Count <= 0)
            {
                return;
            }

            if (_usedMonsters.Count >= PlayerTeam.Count)
            {
                Debug.Log("Cannot select next monster - there are none available ones left!");
                Round.NextRound();
                return;
            }

            // Show selection UI on the default selection
            _selectionUIInstance = UnityEngine.Object.Instantiate(selectedMonsterUI);
            UpdateSelectedMonsterUI();

            SelectedMonster.Value = PlayerTeam[_selectedMonsterIndex];
            // pickMonsterCamera.Priority = PickMonsterCameraActivePriority;
            pickMonsterUI.gameObject.SetActive(true);
            pickMonsterUI.Initialize(SelectedMonster);
            _disposable = new CompositeDisposable();
            pickMonsterUI.ObserveMonsterSubmitted().Subscribe(_ => SubmitCurrentMonster()).AddTo(_disposable);
            pickMonsterUI.ObserveSelectNextMonster().Subscribe(_ => SelectNextMonster()).AddTo(_disposable);

            // pickMonsterCamera.Follow = SelectedMonster.Value != null
            //     ? SelectedMonster.Value.transform
            //     : Round.Controller.PlayerTeam[0].transform;
            // pickMonsterCamera.transform.position = Round.Controller.PlayerTeam[0].transform.position;
            // pickMonsterCamera.PreviousStateIsValid = false;
            _cameraDisposable?.Dispose();
            _cameraDisposable = Round.Controller.TemporaryFollow(SelectedMonster.Value.transform);
        }

        /// <inheritdoc/>
        public override void OnExit()
        {
            base.OnExit();
            // pickMonsterCamera.Priority = 0;
            pickMonsterUI.gameObject.SetActive(false);
            UnityEngine.Object.Destroy(_selectionUIInstance);
            _disposable?.Dispose();
            _cameraDisposable?.Dispose();
            _cameraDisposable = null;
        }

        /// <inheritdoc/>
        public override void OnTick()
        {
            base.OnTick();

            if (PlayerTeam.Count <= 0)
            {
                return;
            }

            if (InputTools.TryGetInputDirectionDown(out Vector2 direction) && SelectedMonster.Value != null)
            {
                var ray = new Ray(SelectedMonster.Value.transform.position, new Vector3(direction.x, 0, direction.y));
                var closest = PlayerTeam.GetClosestInDirection(ray, obj => obj != SelectedMonster.Value && !_usedMonsters.Contains(obj));

                if (closest != null)
                {
                    SelectedMonster.Value = closest;
                    _selectedMonsterIndex = PlayerTeam.IndexOf(closest);
                }
            }
        }

        private void HandleSelectedMonsterChange(GameObject target)
        {
            if (target != null)
            {
                // pickMonsterCamera.Follow = target.transform;
                _cameraDisposable?.Dispose();
                _cameraDisposable = Round.Controller.TemporaryFollow(target.transform);
                UpdateSelectedMonsterUI();
            }
        }

        private void SelectNextMonster()
        {
            IncrementCurrentIndex();
            SelectedMonster.Value = PlayerTeam[_selectedMonsterIndex];
        }

        private void IncrementCurrentIndex()
        {
            for (int i = 1; i < PlayerTeam.Count; i++)
            {
                int wrappedIndex = (_selectedMonsterIndex + i) % PlayerTeam.Count;

                if (!_usedMonsters.Contains(PlayerTeam[wrappedIndex]))
                {
                    _selectedMonsterIndex = wrappedIndex;
                    return;
                }
            }
        }

        private void SubmitCurrentMonster()
        {
            _usedMonsters.Add(SelectedMonster.Value);
            IncrementCurrentIndex();

            // Cleanup the selection UI
            Round.TransitionTo(Round.PickActions);
        }

        private void UpdateSelectedMonsterUI()
        {
            // Either a monster or the player is selected
            GameObject selection = SelectedMonster.Value != null
                ? SelectedMonster.Value
                : Round.Controller.PlayerTeam[0];

            // Rotation is weird to grab dynamically but constant so I hardcoded it
            _selectionUIInstance.transform.rotation = Quaternion.Euler(15, 0, 0);
            _selectionUIInstance.transform.SetParent(selection.transform);

            // Center the UI vertically and move it just behind the monster
            _selectionUIInstance.transform.position = selection.transform.position + new Vector3(0, 1, 0.1f);
        }
    }
}
