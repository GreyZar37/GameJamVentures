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
        
        if (TableMovement.Instance.currentRoom.isBattleRoom)
        {
            StartGambling();
        }
        else
        {
             TableMovement.Instance.SelectNewRoom();
        }
       

    }

    public void Highlight()
    {
        
    }

    public void Unhighlight()
    {
        
    }

    public void StartGambling()
    {
        GamblingManager2.Instance.StartGambling();
        PlayerControllerManager.Instance.SetPlayerStatus(PlayerStatus.Battling);

    }
    

   


}