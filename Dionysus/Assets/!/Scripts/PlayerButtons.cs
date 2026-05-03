using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerButtons : MonoBehaviour
{
    [SerializeField] private ChairInteractable chair;
    void Update()
    {
         if(InputManager.Instance.Input.Player.Back.WasPressedThisFrame() && PlayerControllerManager.Instance.currentPlayerStatus == PlayerStatus.Exploring)
         {
             Singleton<PlayerControllerManager>.Instance.ChangePlayerController(PlayerControllerType.FreeRoam);
             chair.ReturnToDefault();
             
         };
    }
}
