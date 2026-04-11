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
        
        private HashSet<EnemyController> _spawnedEnemies;
        private Room _currentRoom;
        
        public void Install()
        {
            InitializeFirstLevel();
        }

        public void Uninstall()
        {
           
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
            
        }

        private void CreatePlayer(Vector3 spawnPosition)
        {
            var player = Instantiate(_playerPrefab, spawnPosition, Quaternion.identity);
            
        }

        private void CreateRoom()
        {
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
            if (_spawnedEnemies.Count == 0)
            {
                InitializeNextLevel();
            }
        }
    }
}
