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
    }
    
    public struct GameEventChanged
    {
        public GameEvent GameEvent;
    }
    
    //===================================================
    
    //============== LOADING SCREEN ====================

    public struct LoadingScreenEvent
    {
        public float LoadingDuration;
        public bool IsFadeOut;
    }
    
    public struct TreasureChestPromptEvent
    {
        public bool IsPromptVisible;
    }
}