using UnityEngine;

class Roll : MonoBehaviour
{
    void OnCollisionStay(Collision collision)
    {
        Debug.Log($"You have rolled {name[^1]}");
    }

}