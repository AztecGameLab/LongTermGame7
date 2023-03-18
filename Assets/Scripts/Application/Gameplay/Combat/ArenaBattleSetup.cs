namespace Application.Gameplay.Combat
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core;
    using UniRx;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using Object = UnityEngine.Object;
    using Random = UnityEngine.Random;

    /// <summary>
    /// Listens for arena battle start events, and sets up the prerequisite information
    /// for the battle controller.
    /// </summary>
    [Serializable]
    public class ArenaBattleSetup : IDisposable
    {
        [SerializeField]
        private SubArenaLookup subArenaLookup;

        private BattleController _controller;
        private string _originalSceneName;
        private IDisposable _cleanupDisposable;
        private IDisposable _disposable;

        /// <summary>
        /// Prepares this class for execution.
        /// </summary>
        /// <param name="controller">The battle controller to be alerted when a battle is ready to be run.</param>
        /// <returns>This instance.</returns>
        public ArenaBattleSetup Init(BattleController controller)
        {
            _controller = controller;
            _disposable = Services.EventBus.AddListener<ArenaBattleStartData>(HandleArenaBattleStart, "Arena battle setup");
            return this;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            _disposable.Dispose();
        }

        private async void HandleArenaBattleStart(ArenaBattleStartData data)
        {
            _originalSceneName = SceneManager.GetActiveScene().name;

            string subArenaSceneName = subArenaLookup.GetSceneName(Services.RegionTracker.CurrentRegion);
            SceneManager.LoadScene(subArenaSceneName);
            await Task.Delay(100);

            List<GameObject> playerTeamInstances = new List<GameObject>();

            foreach (TeamMemberData memberData in data.PlayerTeamData)
            {
                playerTeamInstances.Add(memberData.CreateWorldView().gameObject);
            }

            Transform[] playerSpawns = Object.FindObjectsOfType<SubArenaPlayerSpawn>().Select(spawn => spawn.transform).ToArray();
            Transform[] enemySpawns = Object.FindObjectsOfType<SubArenaEnemySpawn>().Select(spawn => spawn.transform).ToArray();

            IReadOnlyCollection<GameObject> enemyTeamInstances = SpawnEntities(data.EnemyTeamPrefabs);
            DistributeSpawns(enemyTeamInstances, enemySpawns);
            DistributeSpawns(playerTeamInstances, playerSpawns);

            var battleData = new BattleData(playerTeamInstances, enemyTeamInstances, data.Hooks, data.EnemyOrderDecider);

            _controller.BeginBattle(battleData);
            _cleanupDisposable = _controller.OnBattleEnd.Subscribe(_ => RestoreOriginalScene());
        }

        private void RestoreOriginalScene()
        {
            _cleanupDisposable.Dispose();
            Services.EventBus.Invoke(new LevelChangeEvent { NextScene = _originalSceneName }, "Arena Battle Restoration");
        }

        private List<GameObject> SpawnEntities(IEnumerable<GameObject> entities)
        {
            List<GameObject> result = new List<GameObject>();

            foreach (GameObject entity in entities)
            {
                var instance = Object.Instantiate(entity);
                result.Add(instance);
            }

            return result;
        }

        private void DistributeSpawns(IEnumerable<GameObject> entities, Transform[] spawns)
        {
            if (spawns.Length <= 0)
            {
                Debug.LogError("No arena spawns defined! Please add some for the players and enemies");
            }

            int currentSpawn = Random.Range(0, spawns.Length);

            foreach (GameObject entity in entities)
            {
                entity.transform.position = spawns[currentSpawn].transform.position;
                currentSpawn = (currentSpawn + 1) % spawns.Length;
            }
        }
    }
}
