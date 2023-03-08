using Application.Core;
using Application.Core.Events;
using Application.Gameplay.Combat.Hooks;
using Application.Vfx;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Application.Gameplay.Combat
{
    [Serializable]
    public class ArenaBattleSetup : IDisposable
    {
        [SerializeField] 
        private SubArenaLookup subArenaLookup;
        
        private BattleController _controller;
        private IDisposable _disposable;
    
        public ArenaBattleSetup Init(BattleController controller)
        {
            _controller = controller;
            _disposable = Services.EventBus.AddListener<ArenaBattleStartData>(HandleArenaBattleStart, "Arena battle setup");
            return this;
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }

        private ArenaBattleStartData _data;
    
        private async void HandleArenaBattleStart(ArenaBattleStartData data)
        {
            _data = data;
            _originalSceneName = SceneManager.GetActiveScene().name;
            
            // todo: save original scene state better (actual serialization stuff)
            foreach (GameObject gameObject in data.PlayerTeamPrefabs)
            {
                Object.DontDestroyOnLoad(gameObject);
            }
            
            string subArenaSceneName = subArenaLookup.GetSceneName(Services.RegionTracker.CurrentRegion);
            SceneManager.LoadScene(subArenaSceneName);
            await Task.Delay(100);

            foreach (GameObject gameObject in data.PlayerTeamPrefabs)
            {
                SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
            }
            
            SubArenaPlayerSpawn[] playerSpawns = Object.FindObjectsOfType<SubArenaPlayerSpawn>();
            SubArenaEnemySpawn[] enemySpawns = Object.FindObjectsOfType<SubArenaEnemySpawn>();
            
            // todo: instantiate prefabs into spawn positions
            List<GameObject> enemyInstances = SpawnEntities(data.EnemyTeamPrefabs);
            DistributeSpawns(enemyInstances, enemySpawns);
            DistributeSpawns(data.PlayerTeamPrefabs, playerSpawns);
            
            // todo: pass a battle start to the controller
            var battleData = new BattleData()
            {
                PlayerTeamInstances = data.PlayerTeamPrefabs,
                EnemyTeamInstances = enemyInstances,
                Hooks = new List<Hook>(new[] { new DebuggingHook() }),
            };
            
            _controller.BeginBattle(battleData);
            _cleanupDisposable = _controller.OnBattleEnd.Subscribe(_ => RestoreOriginalScene());
        }
        
        private string _originalSceneName;
        private IDisposable _cleanupDisposable;

        private void RestoreOriginalScene()
        {
            _cleanupDisposable.Dispose();
            SceneManager.LoadScene(_originalSceneName);
        }

        private List<GameObject> SpawnEntities(List<GameObject> entities)
        {
            List<GameObject> result = new List<GameObject>();
            
            foreach (GameObject entity in entities)
            {
                var instance = Object.Instantiate(entity);
                result.Add(instance);
            }

            return result;
        }

        private void DistributeSpawns(List<GameObject> entities, MonoBehaviour[] spawns)
        {
            if (spawns.Length <= 0)
            {
                throw new Exception("No arena spawns defined! Please add some for the players and enemies");
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
