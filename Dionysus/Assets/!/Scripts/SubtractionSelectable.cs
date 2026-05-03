using UnityEngine;

public class SubtractionSelectable : MonoBehaviour, IInteractable
{
    [SerializeField] private SubtractionSelection selection;
    private SubtractionSelectionManager manager;

    private void Awake()
    {
        manager = GetComponentInParent<SubtractionSelectionManager>();
    }

    public void Interact()
    {
        manager.SelectChoice(selection);
    }

    public void Highlight()
    {
       
    }

    public void Unhighlight()
    {
        
    }
}