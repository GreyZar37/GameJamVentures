using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Prop : MonoBehaviour
{
    [Range(0, 100)]
    [SerializeField] private float survivability;

    private void Awake()
    {
        var random = Random.Range(0, 100);
        if (random <= survivability)
        {
            gameObject.SetActive(false);
        }
    }
}
