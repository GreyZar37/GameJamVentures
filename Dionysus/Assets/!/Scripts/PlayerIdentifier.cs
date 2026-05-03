using UnityEngine;

public class PlayerIdentifier : MonoBehaviour
{
    [field: SerializeField] public PlayerControllerType playerControllerType {  get; private set; }
    [field: SerializeField] public Transform PlayerCam { get; private set; }
  
}

public enum PlayerControllerType
{
    FreeRoam, Sitting, GamblingView, Dying
}

public enum PlayerStatus
{
    Transitioning, Battling, Exploring
}