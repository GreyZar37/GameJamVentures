using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class DicePool : MonoBehaviour
{
    [SerializeField] private GameObject dicePrefab;
    [field: SerializeField] public int Sum {  get; private set; }
    private Dice[] dices;
    private readonly WaitForSeconds finishDelay = new WaitForSeconds(2);

    public event Action<int> OnRollDone;

    private void Start()
    {
        dices = GetComponentsInChildren<Dice>();
        foreach (Dice dice in dices)
        {
            Rigidbody rb = dice.GetComponent<Rigidbody>();
            rb.isKinematic = true;
            rb.interpolation = RigidbodyInterpolation.None;
            dice.OnDiceResult += UpdateSum;
            dice.transform.rotation = Random.rotation;
            rb.isKinematic = false;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
        }
    }

    private void UpdateSum(int result)
    {
        Sum = 0;
        foreach (Dice dice in dices) Sum += dice.Result;

        StopAllCoroutines();
        StartCoroutine(SetToFinishedAfterDelay());
    }

    private IEnumerator SetToFinishedAfterDelay()
    {
        yield return finishDelay;
        Debug.Log("Final Sum For Roll is: " +  Sum);
        OnRollDone?.Invoke(Sum);
    }
}