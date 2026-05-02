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
        Debug.Log($"Rollnum variable on Awake {rollNum}");
    }
    void OnTriggerStay(Collider other)
    {
        char currentRoll = name[^1];

        if (lastPosition == transform.position)
        {
            //rollNum = '0';
            Debug.Log($"Rollnum variable in if statement {rollNum}");
            rollNum = currentRoll;
            GetComponent<Collider>().enabled = false;
        }
        lastPosition = transform.position;
        Debug.Log($"Number Roll {name[^1]}");
        Debug.Log($"Current Roll {currentRoll}");
    }
}