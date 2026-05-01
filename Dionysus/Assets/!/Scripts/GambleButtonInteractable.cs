using UnityEngine;

public class GambleButtonInteractable : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        PlayerControllerManager.Instance.ChangePlayerController(PlayerControllerType.GamblingView);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}