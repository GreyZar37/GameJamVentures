using UnityEngine;

public class GambleButtonInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject DicePool;
    public void Interact()
    {
        PlayerControllerManager.Instance.ChangePlayerController(PlayerControllerType.GamblingView);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        GamblingManager.Instance.StartGambling();
        // DicePool.SetActive(true);
    }
}