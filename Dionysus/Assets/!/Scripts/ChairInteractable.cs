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
        GamblingManager2.Instance.SetupForGambling();
        chairCollider.enabled = false;
    }

    public void ReturnToDefault()
    {
        chairCollider.enabled = true;
    }

    public void Highlight()
    {
        
    }

    public void Unhighlight()
    {
        
    }
}