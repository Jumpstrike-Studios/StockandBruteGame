using UnityEngine;

/// <summary>
/// For actors that can be hurt and die
/// </summary>
public interface IAliveActor
{
   public struct ActorVitals
    {
        [ReadOnly]
        [Tooltip("The actor's maximum health.")]
        public int MaxHealth;

        [Tooltip("The actor's starting and current health.")]
        public int Health;
        [ReadOnly]
        [Tooltip("Set to true if the actor's max health is -1")]
        public bool Invincible;
    
    
    /// <summary>
    /// When the actor's health reaches 0, the actor will be removed from the scene
    /// </summary>
    [Tooltip("The actor is removed from the world upon reaching 0 health")]
    public bool RemoveOnDeath;
    public ActorVitals(int BaseHealth)
    {
        MaxHealth = BaseHealth;
        Health = MaxHealth;
        RemoveOnDeath=true;
        Invincible = MaxHealth<0;
    }
    }

}