using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    [SerializeField] private float rayLength = 2f;
    [SerializeField] private LayerMask rayMask = 1 << 0;

    private IInteractable currentInteractable;
    private Transform cam;
    private Ray ray;
    private RaycastHit hit;

    public static event Action<bool> OnInteractableHit;
    public static event Action<bool> OnPressInteract;

    private PlayerInput input;

    private void Start()
    {
        cam = GetComponentInChildren<Camera>().transform;
        input = InputManager.Instance.Input;
    }

    private void Update()
    {
        RaycastForInteractables();
        UpdateInteractInput();
    }

    private void UpdateInteractInput()
    {
        if(input.Player.Interact.WasPressedThisFrame())
        {
            currentInteractable?.Interact();
            OnPressInteract?.Invoke(currentInteractable != null);
        }
    }

    private void RaycastForInteractables()
    {
        ray = new Ray(cam.position, cam.forward);
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
}