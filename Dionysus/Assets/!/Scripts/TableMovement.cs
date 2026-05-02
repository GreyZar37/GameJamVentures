using UnityEngine;

public class TableMovement : MonoBehaviour
{
    [SerializeField] private HandLogic leftHand;
    [SerializeField] private HandLogic rightHand;
    [SerializeField] private HandLogic topHand;
    [SerializeField] private HandLogic bottomHand;
    
     public RoomGenerator.Room currentRoom;
     private RoomGenerator _generator;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _generator = FindAnyObjectByType<RoomGenerator>();
        currentRoom = _generator.playerSpawnRoom;
        SelectNewRoom();
    }


    public void OpenAllDoors()
    {
        var neighbors = _generator.GetNeighbors(currentRoom.gridPosition.x, currentRoom.gridPosition.y);
        foreach (var neighbor in neighbors)
        {
            neighbor.physicalRoom.SetOpenState(true);
        }
        currentRoom.physicalRoom.SetOpenState(true);
    }

    public void CloseAllDoors()
    {
        var neighbors = _generator.GetNeighbors(currentRoom.gridPosition.x, currentRoom.gridPosition.y);
        foreach (var neighbor in neighbors)
        {
            neighbor.physicalRoom.SetOpenState(false);
        }
        currentRoom.physicalRoom.SetOpenState(false);
    }

    private void SelectNewRoom()
    {
        EnableHands(currentRoom.physicalRoom.doors);
    }

    private void EnableHands(PhysicalRoom.DoorDirection doors)
    {
        leftHand.gameObject.SetActive(doors.HasFlag(PhysicalRoom.DoorDirection.West));
        leftHand.AssignAction(() =>
        {
            MoveToAnotherRoom(PhysicalRoom.DoorDirection.West);
        });
        
        topHand.gameObject.SetActive(doors.HasFlag(PhysicalRoom.DoorDirection.North));
        topHand.AssignAction(() =>
        {
            MoveToAnotherRoom(PhysicalRoom.DoorDirection.North);
        });
        
        
        rightHand.gameObject.SetActive(doors.HasFlag(PhysicalRoom.DoorDirection.East));
        rightHand.AssignAction(() =>
        {
            MoveToAnotherRoom(PhysicalRoom.DoorDirection.East);
        });
        
        bottomHand.gameObject.SetActive(doors.HasFlag(PhysicalRoom.DoorDirection.South));
        bottomHand.AssignAction(() =>
        {
            MoveToAnotherRoom(PhysicalRoom.DoorDirection.South);
        });
    }

    private void MoveToAnotherRoom(PhysicalRoom.DoorDirection direction)
    {
        leftHand.ClearActions();
        rightHand.ClearActions();
        topHand.ClearActions();
        bottomHand.ClearActions();
        
        print(direction.ToString());
        
    }
}
