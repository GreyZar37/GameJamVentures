using System;
using UnityEngine;

public class PlayDice : MonoBehaviour, IInteractable
{
    [SerializeField] private Animator startDicesAnimator;
    private Action onDiceClick;

    public void Interact()
    {
        if(onDiceClick != null)
        {
            onDiceClick?.Invoke();
            startDicesAnimator.SetBool("isHidden", true);
        }
    }

    public void ShowDice(Action callback)
    {
        startDicesAnimator.SetBool("isHidden", false);
        onDiceClick = callback;
    }

    public void Highlight()
    {

    }

    public void Unhighlight()
    {

    }
}