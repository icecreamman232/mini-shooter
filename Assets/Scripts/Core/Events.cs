using Shinrai.Modifiers;

namespace Shinrai.Core
{
    //============== PLAYER HEALTH CHANGE================
    public struct PlayerHealthChangedEvent
    {
        public float CurrentHealth;
        public float MaxHealth;

        public PlayerHealthChangedEvent(float currentHealth, float maxHealth)
        {
            CurrentHealth = currentHealth;
            MaxHealth = maxHealth;
        }
    }
    //===================================================
    
    //============== GAME EVENTS ========================
    public enum GameEvent
    {
        CreatedPlayer,
        GameStarted,
        LoadNextRoom,
    }
    
    public struct GameEventChanged
    {
        public GameEvent GameEvent;
        
        public GameEventChanged(GameEvent gameEvent)
        {
            GameEvent = gameEvent;
        }
    }
    
    //===================================================
    
    //============== LOADING SCREEN ====================

    public struct LoadingScreenEvent
    {
        public float LoadingDuration;
        public bool IsFadeOut;
    }
    
    public struct PickupItemPromptEvent
    {
        public bool IsPromptVisible;
    }
    
    public struct ExternalStateChangeEvent
    {
        public StatTarget StatTarget;
        public float Value;

        public ExternalStateChangeEvent(StatTarget statTarget, float value)
        {
            StatTarget = statTarget;
            Value = value;
        }
    }
}