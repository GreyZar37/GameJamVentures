using UnityEngine;

public class ChairInteractable : MonoBehaviour, IInteractable
{
    private Collider chairCollider;

    private void Awake()
    {
        chairCollider = GetComponent<Collider>();
    }

    public void Interact()
    {
        PlayerControllerManager.Instance.ChangePlayerController(PlayerControllerType.Sitting);
        GamblingManager.Instance.SetGamblingSetup(true);
        chairCollider.enabled = false;
    }

    public void Highlight()
    {
        
    }

    public void Unhighlight()
    {
        
    }
}