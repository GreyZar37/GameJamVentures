using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class InteractorUI : MonoBehaviour
{
    private Image crosshair;
    
    [SerializeField] Color hitInteractableColor;
    [SerializeField] Color normalColor;
    
    [SerializeField] private float bonusScale = 1.25f;

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
        crosshair.color = hitInteractable ? hitInteractableColor : normalColor;

        if (hitInteractable)
        {
            crosshair.rectTransform.localScale = Vector3.one * bonusScale;
        }
        else
        {
             crosshair.rectTransform.localScale = Vector3.one;
        }
        
    }
}