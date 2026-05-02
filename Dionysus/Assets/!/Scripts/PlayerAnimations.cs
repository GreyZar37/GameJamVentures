using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] private Animator camAnimator, armsAnimator;
    private const string _VELOCITY = "Velocity";
    private CharacterController controller;
    private float velocity;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }
    private void Start()
    {
        Interactor.OnPressInteract += OnInteract;
    }
    private void OnDestroy()
    {
        Interactor.OnPressInteract -= OnInteract;
    }

    private void OnInteract(bool success)
    {
        if (isActiveAndEnabled) armsAnimator.SetTrigger("Gather");
    }

    private void LateUpdate()
    {
        if(controller == null) return;
        velocity = controller.velocity.magnitude;
        camAnimator.SetFloat(_VELOCITY, velocity);
        armsAnimator.SetFloat(_VELOCITY, velocity);
    }
}
