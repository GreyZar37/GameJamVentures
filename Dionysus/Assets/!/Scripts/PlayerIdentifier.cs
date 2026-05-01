using UnityEngine;

public class PlayerIdentifier : MonoBehaviour
{
    [field: SerializeField] public PlayerControllerType playerControllerType {  get; private set; }
    public Transform PlayerCam {  get; private set; }

    private void Awake()
    {
        PlayerCam = GetComponentInChildren<Camera>().transform;
    }
}

public enum PlayerControllerType
{
    FreeRoam, Sitting, GamblingView
}