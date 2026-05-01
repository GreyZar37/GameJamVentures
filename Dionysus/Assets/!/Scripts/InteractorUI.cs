using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class InteractorUI : MonoBehaviour
{
    private Image crosshair;

    private void Awake()
    {
        crosshair = GetComponent<Image>();
    }

    private void Start()
    {
        Interactor.OnInteractableHit += OnInteractableHit;
    }

    private void OnDestroy()
    {
        Interactor.OnInteractableHit -= OnInteractableHit;
    }

    private void OnInteractableHit(bool hitInteractable)
    {
        crosshair.color = hitInteractable ? Color.green : Color.white;
    }
}