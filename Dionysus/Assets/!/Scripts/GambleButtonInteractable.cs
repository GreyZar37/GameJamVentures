using UnityEngine;

public class GambleButtonInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private SubtractionSelection selection;
    private SubtractionSelectionManager manager;

    
    [SerializeField] Animator diceAnimator;
    
    
    private void Awake()
    {
        manager = FindAnyObjectByType<SubtractionSelectionManager>();
    }
    
    public void Interact()
    {
        if (PlayerControllerManager.Instance.currentPlayerControllerType == PlayerControllerType.GamblingView 
            && Singleton<PlayerControllerManager>.Instance.currentPlayerStatus == PlayerStatus.Battling)
        {
            manager.SelectChoice(selection);
            return;
        }
        else
        {
            PlayerControllerManager.Instance.ChangePlayerController(PlayerControllerType.GamblingView);
        }

        if (Singleton<TableMovement>.Instance.currentRoom.isBattleRoom)
        {
            diceAnimator.SetBool("isHidden", false);
        }
        else
        {
             diceAnimator.SetBool("isHidden", true);
             Singleton<TableMovement>.Instance.SelectNewRoom();
        }
       

    }

    public void Highlight()
    {
        
    }

    public void Unhighlight()
    {
        
    }
    

   


}