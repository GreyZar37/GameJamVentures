using UnityEngine;

[RequireComponent(typeof(Collider))]
class Roll : MonoBehaviour
{
    Vector3 lastPosition;
    char rollNum;
    Collider finalRollCollider;
    void Awake()
    {
        rollNum = name[^1];
    }
    void OnTriggerStay(Collider other)
    {
        char currentRoll = name[^1];

        if (lastPosition == transform.position)
        {
            //rollNum = '0';
            Debug.Log($"You rolled a {rollNum}");
            rollNum = currentRoll;
            GetComponent<Collider>().enabled = false;
        }
        lastPosition = transform.position;
    }
}