using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/EnemyData", order = 1)]
public class EnemyData : ScriptableObject
{
     public int health;
     public int mustStopAtDiceValue;
     
     public float luck;
}
