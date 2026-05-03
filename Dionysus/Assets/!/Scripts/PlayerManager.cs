using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    [field: SerializeField] public int PlayerHealth { get; private set; } = 3;

    public void SetPlayerHealth(int health)
    {
        PlayerHealth = health;
    }
}