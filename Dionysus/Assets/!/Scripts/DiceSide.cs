using UnityEngine;

public class DiceSide : MonoBehaviour
{
    [SerializeField, Range(1, 6)] private int value = 1;

    private Dice dice;

    private void Awake()
    {
        dice = GetComponentInParent<Dice>();
    }

    private void OnTriggerEnter(Collider other)
    {
        dice.Result = value;
    }
}