using UnityEngine;

public class GambleButtonInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject DicePool;
    public void Interact()
    {
        PlayerControllerManager.Instance.ChangePlayerController(PlayerControllerType.GamblingView);
        GamblingManager2.Instance.StartGambling();
    }

    public void Highlight()
    {
        
    }

    public void Unhighlight()
    {
        
    }
}