using UnityEngine;

public class TableMovement : MonoBehaviour
{
    [SerializeField] private GameObject leftHand;
    [SerializeField] private GameObject rightHand;
    [SerializeField] private GameObject topHand;
    [SerializeField] private GameObject bottomHand;
    
     private RoomGenerator.Room currentRoom;
     private RoomGenerator _generator;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _generator = FindAnyObjectByType<RoomGenerator>();
        currentRoom = _generator.playerSpawnRoom;
        SelectNewRoom();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SelectNewRoom()
    {
        EnableHands(currentRoom.physicalRoom.doors);
    }

    private void EnableHands(PhysicalRoom.DoorDirection doors)
    {
        leftHand.SetActive(doors.HasFlag(PhysicalRoom.DoorDirection.West));
        topHand.SetActive(doors.HasFlag(PhysicalRoom.DoorDirection.North));
        rightHand.SetActive(doors.HasFlag(PhysicalRoom.DoorDirection.East));
        bottomHand.SetActive(doors.HasFlag(PhysicalRoom.DoorDirection.South));
    }
}
