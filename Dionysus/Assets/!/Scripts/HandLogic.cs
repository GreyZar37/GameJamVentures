using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class HandLogic : MonoBehaviour, IInteractable
{
    private static readonly int Delay = Animator.StringToHash("delay");
    private static readonly int IsPointing = Animator.StringToHash("isPointing");

    private float _randomDelayToAnimatorLoop;
    [SerializeField] private Animator animator;
    private Collider  _collider;

    private Action OnPressed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _collider  = GetComponent<Collider>();
        _randomDelayToAnimatorLoop = Random.Range(0f, 0.4f);
        animator = GetComponent<Animator>();
        animator.SetFloat(Delay, _randomDelayToAnimatorLoop);

    }

    private void Update()
    {
        _collider.enabled = Singleton<PlayerControllerManager>.Instance.currentPlayerControllerType ==
                            PlayerControllerType.GamblingView;
    }


    public void AssignAction(Action action)
    {
         OnPressed += action;
    }

    public void ClearActions()
    {
        if( OnPressed != null)
         OnPressed -= OnPressed;
    }
    

    public void Interact()
    {
        OnPressed?.Invoke();
    }

    public void Highlight()
    {
        animator.SetBool(IsPointing, true);   
    }

    public void Unhighlight()
    {
         animator.SetBool(IsPointing, false);
    }
}
