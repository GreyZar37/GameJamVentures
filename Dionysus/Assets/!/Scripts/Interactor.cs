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

    private bool interactSubscribed = false;
    public static event Action<bool> OnInteractableHit;

    private void Start()
    {
        cam = GetComponentInChildren<Camera>().transform;
        //Interact subscribe logic in bottom of script
    }

    private void Update()
    {
        ray = new Ray(cam.position, cam.forward);
        if(Physics.Raycast(ray, out hit, rayLength, rayMask, QueryTriggerInteraction.Ignore))
        {
            if(currentInteractable == null && hit.transform.TryGetComponent(out IInteractable hitInteractable))
            {
                currentInteractable = hitInteractable;
                OnInteractableHit?.Invoke(true);
            }
            else if(currentInteractable != null && !hit.transform.TryGetComponent(out hitInteractable))
            {
                currentInteractable = null;
                OnInteractableHit?.Invoke(false);
            }
        }
        else if(currentInteractable != null)
        {
            currentInteractable = null;
            OnInteractableHit?.Invoke(false);
        }
    }

    private void OnInteract(InputAction.CallbackContext ctx)
    {
        currentInteractable?.Interact();
    }

    private void OnEnable()
    {
        if (!interactSubscribed)
        {
            InputManager.Instance.Input.Player.Interact.started += OnInteract;
            interactSubscribed = true;
        }
    }

    private void OnDisable()
    {
        if (interactSubscribed)
        {
            //InputManager.Instance.Input.Player.Interact.started -= OnInteract;
            interactSubscribed = false;
        }
        OnInteractableHit?.Invoke(false);
    }
}