using System;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    [SerializeField] protected float rayLength = 2f;
    [SerializeField] protected LayerMask rayMask = 1 << 0;

    protected IInteractable currentInteractable;
    protected Camera cam;
    protected Ray ray;
    protected RaycastHit hit;

    public static event Action<bool> OnInteractableHit;
    public static event Action<bool> OnPressInteract;

    protected PlayerInput input;

    protected virtual void Start()
    {
        cam = GetComponentInChildren<Camera>();
        input = InputManager.Instance.Input;
    }

    protected virtual void Update()
    {
        RaycastForInteractables();
        UpdateInteractInput();
    }

    protected virtual void UpdateInteractInput()
    {
        if(input.Player.Interact.WasPressedThisFrame())
        {
            currentInteractable?.Interact();
            InvokeOnPressInteract();
        }
    }

    protected virtual void RaycastForInteractables()
    {
        ray = new Ray(cam.transform.position, cam.transform.forward);
        if (Physics.Raycast(ray, out hit, rayLength, rayMask, QueryTriggerInteraction.Ignore))
        {
            if (currentInteractable == null && hit.transform.TryGetComponent(out IInteractable hitInteractable))
            {
                currentInteractable = hitInteractable;
                OnInteractableHit?.Invoke(true);
            }
            else if (currentInteractable != null && !hit.transform.TryGetComponent(out hitInteractable))
            {
                currentInteractable = null;
                OnInteractableHit?.Invoke(false);
            }
        }
        else if (currentInteractable != null)
        {
            currentInteractable = null;
            OnInteractableHit?.Invoke(false);
        }
    }

    protected void InvokeOnPressInteract()
    {
        OnPressInteract?.Invoke(currentInteractable != null);
    }
}