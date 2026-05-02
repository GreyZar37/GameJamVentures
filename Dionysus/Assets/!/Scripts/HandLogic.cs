using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class HandLogic : MonoBehaviour, IInteractable
{
    private static readonly int Delay = Animator.StringToHash("delay");
    
    private float _randomDelayToAnimatorLoop;
    private Animator _animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _randomDelayToAnimatorLoop = Random.Range(0f, 0.4f);
        _animator = GetComponent<Animator>();
        print(_randomDelayToAnimatorLoop + " random num");
        print(_animator.GetFloat(Delay));
    }

    private void OnEnable()
    {
        _animator.SetFloat(Delay, _randomDelayToAnimatorLoop);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Interact()
    {
        throw new System.NotImplementedException();
    }
}
