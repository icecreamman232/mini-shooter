using System.Collections;
using System.Collections.Generic;
using Shinrai.Entity;
using Shinrai.Levels;
using UnityEngine;

namespace Shinrai.Core
{
    public class LevelManager : MonoBehaviour, IBootStrap
    {
        [SerializeField] private PlayerController _playerPrefab;
        [SerializeField] private Transform _roomParent;
        [SerializeField] private Room[] _roomPrefabs;
        [SerializeField] private EnemyController[] _enemyPrefabs;

        private PlayerController _player;
        private HashSet<EnemyController> _spawnedEnemies;
        private Room _currentRoom;
        private int _maxItemNumber = 3;
        
        public PlayerController Player => _player;
        
        public void Install()
        {
            EventBus.Subscribe<GameEventChanged>(OnGameEventChanged);
            InitializeFirstLevel();
        }

        public void Uninstall()
        {
            EventBus.Unsubscribe<GameEventChanged>(OnGameEventChanged);
        }
        
        private void InitializeFirstLevel()
        {
            CreateRoom();
            ServiceLocator.GetService<CameraController>().SetPosition(_currentRoom.PlayerSpawnPoint.position);
            CreatePlayer(_currentRoom.PlayerSpawnPoint.position);
            SpawnEnemies(_currentRoom.EnemySpawnPoints);
        }

        private void InitializeNextLevel()
        {
            StartCoroutine(OnLoadNextLevel());
        }

        private IEnumerator OnLoadNextLevel()
        {
            EventBus.Emit(new LoadingScreenEvent
            {
                LoadingDuration = 0.5f,
                IsFadeOut = false
            });
            yield return new WaitForSeconds(0.5f);
            
            CreateRoom();
            ServiceLocator.GetService<CameraController>().SetPosition(_currentRoom.PlayerSpawnPoint.position);
            CreatePlayer(_currentRoom.PlayerSpawnPoint.position);
            SpawnEnemies(_currentRoom.EnemySpawnPoints);
            
            EventBus.Emit(new LoadingScreenEvent
            {
                LoadingDuration = 0.5f,
                IsFadeOut = true
            });
        }

        private void CreatePlayer(Vector3 spawnPosition)
        {
            if (_player != null)
            {
                _player.transform.position = spawnPosition;
                return;
            }
            _player = Instantiate(_playerPrefab, spawnPosition, Quaternion.identity);
        }

        private void CreateRoom()
        {
            if (_currentRoom != null)
            {
                Destroy(_currentRoom.gameObject);
            }
            var chosenPrefab = _roomPrefabs.PickRandom();
            _currentRoom = Instantiate(chosenPrefab, Vector3.zero, Quaternion.identity, _roomParent);
        }
        
        private void SpawnEnemies(Transform[] spawnPoints)
        {
            if (_spawnedEnemies == null)
            {
                _spawnedEnemies = new HashSet<EnemyController>();
            }
            else
            {
                _spawnedEnemies.Clear();
            }
            foreach (var spawnPoint in spawnPoints)
            {
                var enemyPrefab = _enemyPrefabs.PickRandom();
                var enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity, _currentRoom.transform);
                enemy.Health.OnEnemyDeath += OnEnemyDead;
                _spawnedEnemies.Add(enemy);
            }
        }

        private void OnEnemyDead(EnemyController controller)
        {
            _spawnedEnemies.Remove(controller);
            controller.Health.OnEnemyDeath -= OnEnemyDead;
            //All enemies area dead,spawn rewards
            if (_spawnedEnemies.Count == 0)
            {
                _currentRoom.SpawnItems();
            }
        }
        
        private void OnGameEventChanged(GameEventChanged eventArgs)
        {
            if (eventArgs.GameEvent == GameEvent.LoadNextRoom)
            {
                this.DelayCall(2f, InitializeNextLevel);
            }
        }
    }
}
