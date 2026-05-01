using UnityEngine;

class Roll : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"You have rolled {name[^1]}");
    }
}