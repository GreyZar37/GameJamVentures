using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    // [SerializeField] public List<Roll> dice;
    [SerializeField] public List<char> rolls;  

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // dice = GetComponentsInChildren<Roll>();
    }


    void OnCollisionStay(Collision collision)
    {
        Collider[] diceColliders = GetComponentsInChildren<Collider>();
        foreach (var DiceCollider in diceColliders)
        {
            DiceCollider.enabled = false;
        }
    }
    // Update is called once per frame
    /*
    void Update()
    {
        Debug.Log("You rolled these numbers " + rolls.ToString());
    }*/

}
