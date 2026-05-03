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
        }
        else
        {
            PlayerControllerManager.Instance.ChangePlayerController(PlayerControllerType.GamblingView);
        }

        if (Singleton<TableMovement>.Instance.currentRoom.isBattleRoom)
        {
            diceAnimator.SetBool("isHidden", false);
            Singleton<TableMovement>.Instance.SelectNewRoom();
        }
        else
        {
             diceAnimator.SetBool("isHidden", true);
        }
       

    }

    public void Highlight()
    {
        
    }

    public void Unhighlight()
    {
        
    }
    

   


}