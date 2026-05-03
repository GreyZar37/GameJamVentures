using UnityEngine;

public class GambleButtonInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private SubtractionSelection selection;
    private SubtractionSelectionManager manager;

    
    [SerializeField] GameObject DicePool;
    
    private void Awake()
    {
        manager = FindAnyObjectByType<SubtractionSelectionManager>();
    }
    
    public void Interact()
    {
        if (PlayerControllerManager.Instance.currentPlayerControllerType == PlayerControllerType.GamblingView)
        {
            manager.SelectChoice(selection);
        }
        else
        {
            PlayerControllerManager.Instance.ChangePlayerController(PlayerControllerType.GamblingView);
            GamblingManager2.Instance.StartGambling();
        }
       

    }

    public void Highlight()
    {
        
    }

    public void Unhighlight()
    {
        
    }
    

   


}