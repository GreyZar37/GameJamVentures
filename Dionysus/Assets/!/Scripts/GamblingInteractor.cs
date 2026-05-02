using UnityEngine;
using UnityEngine.InputSystem;

public class GamblingInteractor : Interactor
{
    [SerializeField] private Texture2D clickTexture;

    protected override void Start()
    {
        base.Start();
    }
    protected override void Update()
    {
        base.Update();
    }

    protected override void UpdateInteractInput()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            currentInteractable?.Interact();
            InvokeOnPressInteract();
        }
    }

    protected override void RaycastForInteractables()
    {
        ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue(), Camera.MonoOrStereoscopicEye.Mono);

        if (Physics.Raycast(ray, out hit, rayLength, rayMask, (QueryTriggerInteraction)1) &&
            hit.transform.TryGetComponent(out IInteractable newInteractable))
        {
            Cursor.SetCursor(clickTexture, Vector2.zero, CursorMode.ForceSoftware);
            if (currentInteractable == null)
            {
                currentInteractable = newInteractable;
                currentInteractable.Highlight();
            }
              
        }
        else if(currentInteractable != null)
        {
            currentInteractable.Unhighlight();
            currentInteractable = null;
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
    }

    private void OnEnable()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void OnDisable()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}