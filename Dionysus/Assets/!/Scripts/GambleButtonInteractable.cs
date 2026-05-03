using UnityEngine;

public class GambleButtonInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private SubtractionSelection selection;
    private SubtractionSelectionManager manager;

    
    [SerializeField] Animator diceAnimator;
    [SerializeField] private AudioClip bossMusic;
    [SerializeField] private AudioClip laughingSound;
    
    private void Awake()
    {
        manager = FindAnyObjectByType<SubtractionSelectionManager>();
    }
    
    public void Interact()
    {
        if(PlayerControllerManager.Instance.currentPlayerStatus == PlayerStatus.Transitioning) return;
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
        
        if (TableMovement.Instance.currentRoom.isBattleRoom && !TableMovement.Instance.currentRoom.enemyBeaten)
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

    private void StartGambling()
    {
        if (TableMovement.Instance.currentRoom == RoomGenerator.Instance.bossSpawnRoom)
        {
            SoundManager.Instance.SwitchMusic(bossMusic);
        }
        GamblingManager2.Instance.StartGambling(TableMovement.Instance.currentRoom.enemyData);
        PlayerControllerManager.Instance.SetPlayerStatus(PlayerStatus.Battling);
        AudioSource.PlayClipAtPoint(laughingSound,transform.position, 2);

    }
    

   


}