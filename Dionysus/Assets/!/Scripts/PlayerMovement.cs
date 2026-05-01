using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float moveSmoothTime = 0.075f;

    private CharacterController controller;
    private Transform cam;

    private float deltaTime;
    private Vector2 smoothMove, refMove;
    private Vector3 move;
    private Vector3 fallVelocity;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        cam = GetComponentInChildren<Camera>().transform;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        deltaTime = Time.deltaTime;
        UpdateMovement();
    }


    private void UpdateMovement()
    {
        Vector2 moveInput = InputManager.Instance.Input.Player.Movement.ReadValue<Vector2>();
        smoothMove = Vector2.SmoothDamp(smoothMove, moveInput, ref refMove, moveSmoothTime, Mathf.Infinity, deltaTime);

        if (controller.isGrounded && fallVelocity.y < 0) fallVelocity.y = -2f;

        Vector3 forward = new Vector3(cam.forward.x, 0f, cam.forward.z).normalized;

        move = forward * smoothMove.y + cam.right * smoothMove.x;
        controller.Move((move * moveSpeed + fallVelocity) * deltaTime);
    }
}