using UnityEngine;

namespace Shinrai.Levels
{
    public class Room : MonoBehaviour
    {
        [SerializeField] private Transform _playerSpawnPoint;
        [SerializeField] private Transform[] _enemySpawnPoint;
        
        public Transform PlayerSpawnPoint => _playerSpawnPoint;
        public Transform[] EnemySpawnPoints => _enemySpawnPoint;
    }
}
