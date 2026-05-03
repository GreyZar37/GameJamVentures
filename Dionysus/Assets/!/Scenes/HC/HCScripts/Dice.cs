using System;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    // [SerializeField] public List<Roll> dice;
    [SerializeField] public List<char> rolls;

    private int result;
    public int Result
    {
        get 
        { 
            return result; 
        }
        set
        {
            result = value;
            OnDiceResult?.Invoke(result);
        }
    }
    public event Action<int> OnDiceResult;
    // [SerializeField] public int rollSum = 0;  

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // dice = GetComponentsInChildren<Roll>();
    }


    void OnCollisionStay(Collision collision)
    {
        /*Collider[] diceColliders = GetComponentsInChildren<Collider>();
        foreach (var DiceCollider in diceColliders)
        {
            DiceCollider.enabled = false;
        }*/
    }
    // Update is called once per frame
    /*
    void Update()
    {
        Debug.Log("You rolled these numbers " + rolls.ToString());
    }*/

}
