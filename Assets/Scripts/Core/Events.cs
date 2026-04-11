namespace Shinrai.Core
{
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

    public enum GameEvent
    {
        CreatedPlayer,
        GameStarted,
    }
    
    public struct GameEventChanged
    {
        public GameEvent GameEvent;
    }
    
}