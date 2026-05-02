using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Roll : MonoBehaviour
{
    Vector3 lastPosition;

    [SerializeField] Dice DicePool;
    char rollNum;
    Collider finalRollCollider;
    void Awake()
    {
        rollNum = name[^1];
        DicePool.dice.Add(GetComponent<Roll>());
    }
    void OnTriggerStay(Collider other)
    {
        char currentRoll = name[^1];

        if (lastPosition == transform.position)
        {
            //rollNum = '0';
            Debug.Log($"You rolled a {rollNum}");
            DicePool.rolls.Add(rollNum);
            rollNum = currentRoll;
            GetComponent<Collider>().enabled = false;
        }
        lastPosition = transform.position;
    }
}