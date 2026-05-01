using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] private float lookSmoothTime = 0.01f;
    [SerializeField] private float mouseSensitivity = 1f;
    [SerializeField] private float xRotClamp = 80f;
    [SerializeField] private float yRotClamp = 80f;

    private Transform cam;

    private Vector2 smoothLook, refLook;
    private float xRotation, yRotation;
    private float deltaTime;

    private void Awake()
    {
        cam = GetComponentInChildren<Camera>().transform;
    }

    private void Update()
    {
        deltaTime = Time.deltaTime;
        UpdateLook();
    }

    private void UpdateLook()
    {
        Vector2 lookInput = InputManager.Instance.Input.Player.Look.ReadValue<Vector2>();
        smoothLook = Vector2.SmoothDamp(smoothLook, lookInput, ref refLook, lookSmoothTime, Mathf.Infinity, deltaTime);

        xRotation -= smoothLook.y * mouseSensitivity;
        if(xRotClamp != 0) xRotation = Mathf.Clamp(xRotation, -xRotClamp, xRotClamp);

        yRotation += smoothLook.x * mouseSensitivity;
        if(yRotClamp != 0) yRotation = Mathf.Clamp(yRotation, -yRotClamp, yRotClamp);

        cam.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }
}
