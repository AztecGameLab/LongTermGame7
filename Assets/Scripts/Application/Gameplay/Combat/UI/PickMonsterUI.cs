namespace Application.Gameplay.Combat.UI
{
    using System;
    using TMPro;
    using UniRx;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// A user-interface for displaying selected monster information,
    /// and allowing the user to select different monsters.
    /// </summary>
    public class PickMonsterUI : MonoBehaviour
    {
        private readonly Subject<Unit> _monsterSubmitted = new Subject<Unit>();

        [SerializeField]
        private TMP_Text selectedMonsterText;

        [SerializeField]
        private Button nextMonsterButton;

        [SerializeField]
        private Button submitMonsterButton;

        private CompositeDisposable _disposable;

        /// <summary>
        /// Gets an observable that changes each time the user selects a monster via the UI.
        /// </summary>
        /// <returns>An observable.</returns>
        public IObservable<Unit> ObserveMonsterSubmitted() => _monsterSubmitted;

        /// <summary>
        /// Gets an observable that changes each time the user cycles to a new monster via the UI.
        /// </summary>
        /// <returns>An observable.</returns>
        public IObservable<Unit> ObserveSelectNextMonster() => nextMonsterButton.OnClickAsObservable();

        /// <summary>
        /// Sets up the UI to display monster selection information.
        /// </summary>
        /// <param name="selectedMonster">The currently selected monster.</param>
        public void Initialize(IReadOnlyReactiveProperty<GameObject> selectedMonster)
        {
            _disposable?.Dispose();
            _disposable = new CompositeDisposable();

            submitMonsterButton.OnClickAsObservable()
                .Merge(Observable.EveryGameObjectUpdate().Where(_ => Input.GetKeyDown(KeyCode.Return)).Select(_ => Unit.Default))
                .Subscribe(_ => _monsterSubmitted.OnNext(Unit.Default))
                .AddTo(_disposable);

            selectedMonster.Subscribe(UpdateTextDisplay).AddTo(_disposable);
        }

        private void UpdateTextDisplay(GameObject monster)
        {
            string monsterName = monster != null ? monster.name : "None";
            selectedMonsterText.text = $"Selected {monsterName}";
        }
    }
}
