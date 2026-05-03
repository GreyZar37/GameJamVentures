using System.Linq;
using UnityEngine;

public class PlayerControllerManager : Singleton<PlayerControllerManager>
{
    [field: SerializeField] public PlayerControllerType StartController { get; private set; }
    private PlayerIdentifier[] players;
    
    [SerializeField] private TableMovement tableMovement;

    public PlayerControllerType currentPlayerControllerType;

  
    [field: SerializeField]  public PlayerStatus currentPlayerStatus {  get; private set; } = PlayerStatus.Exploring;

    public void SetPlayerStatus(PlayerStatus newPlayerStatus)
    {
        currentPlayerStatus = newPlayerStatus;
    }
    
    private void Awake()
    {
        players = FindObjectsByType<PlayerIdentifier>(FindObjectsInactive.Include);
    }

    private void Start()
    {
        ChangePlayerControllerInstantly(StartController);
    }

    public void ChangePlayerController(PlayerControllerType type) //Deactivates all players, then activates the specific one
    {
        PlayerIdentifier oldPlayer = players.First(x => x.gameObject.activeSelf);
        PlayerIdentifier newPlayer = players.First(x => x.playerControllerType == type);

        DisableAllPlayerControllers();

        TransitionCamera.Instance.StartTransition(oldPlayer.PlayerCam, newPlayer.PlayerCam, () =>
        {
            newPlayer.gameObject.SetActive(true);
        });
        
        currentPlayerControllerType  = type;
    }
    public void ChangePlayerControllerInstantly(PlayerControllerType type)
    {
        PlayerIdentifier newPlayer = players.First(x => x.playerControllerType == type);
        DisableAllPlayerControllers();
        newPlayer.gameObject.SetActive(true);
    }

    public void DisableAllPlayerControllers()
    {
        foreach (PlayerIdentifier player in players) player.gameObject.SetActive(false);
    }
}