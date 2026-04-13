using Shinrai.Entity;
using UnityEngine;

namespace Shinrai.Core
{
    /// <summary>
    /// God class that hold all the data that is required for in-game
    /// </summary>
    public class InGameDataManager : MonoBehaviour, IGameService
    {
        public Transform PlayerTransform;
        public PlayerController PlayerController;
        public bool IsGamePaused;
        public GameEventChanged GameEventChanged;
        
        private void Awake()
        {
            ServiceLocator.RegisterService(this);
            GameEventChanged = new GameEventChanged();
        }

        private void OnDestroy()
        {
            ServiceLocator.UnregisterService<InGameDataManager>();
        }

        public void AssignPlayer(Transform playerTransform)
        {
            PlayerTransform = playerTransform;
            PlayerController = playerTransform.GetComponent<PlayerController>();
        }
    }
}
