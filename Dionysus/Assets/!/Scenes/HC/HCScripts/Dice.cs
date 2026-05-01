using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    [SerializeField] Vector3[] diceSidesWithRotation;
    [SerializeField] MeshCollider[] diceSidesWithComponents;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        diceSidesWithComponents = GetComponentsInChildren<MeshCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionStay(Collision collision)
    {
        /*
        for (int conPoint = 0; conPoint < diceSides.Length; conPoint++)
        {
            diceSides[conPoint] = collision.contacts[conPoint].point;
        }*/
        // diceSides[0] = transform.rotation.eulerAngles;
        
        // Debug.Log($"You rolled a {}");
    }

}
