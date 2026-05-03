using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TableMovement : Singleton<TableMovement>
{
    [SerializeField] private HandLogic leftHand;
    [SerializeField] private HandLogic rightHand;
    [SerializeField] private HandLogic topHand;
    [SerializeField] private HandLogic bottomHand;
    
    [SerializeField] private Animator handsAnimator;
    
     public RoomGenerator.Room currentRoom;
     private RoomGenerator _generator;
     
     [SerializeField] private float smoothTime = 0.125f;
     
     private List<LightLogic> currentLights = new List<LightLogic>();
     
     [SerializeField] private GameObject Dionysos;
     
     private bool doorsOpen = false;
     public bool roomSelectionOnHalt = false;

     private GambleButtonInteractable _buttonInteractable;
   
     void Start()
    {
        _generator = FindAnyObjectByType<RoomGenerator>();
        currentRoom = _generator.playerSpawnRoom;
        print("current room: " + currentRoom);
        _buttonInteractable = FindAnyObjectByType<GambleButtonInteractable>(FindObjectsInactive.Include);
    }
    
    public void MoveGamblingTable(Vector3 targetPos)
    {
       
        StopAllCoroutines();
        StartCoroutine(MoveGamblingTableSmoothly(targetPos));
    }

    private void Update()
    {
        

        if (Singleton<PlayerControllerManager>.Instance.currentPlayerControllerType !=
            PlayerControllerType.GamblingView && doorsOpen)
        {
            
            CloseAllDoors();
        }
    }


    private void OpenAllDoors()
    {
        var neighbors = _generator.GetNeighbors(currentRoom.gridPosition.x, currentRoom.gridPosition.y);
        foreach (var neighbor in neighbors)
        {
            neighbor.physicalRoom.SetOpenState(true);
        }
        currentRoom.physicalRoom.SetOpenState(true);
        doorsOpen = true;
    }

    private void CloseAllDoors()
    {
        var neighbors = _generator.GetNeighbors(currentRoom.gridPosition.x, currentRoom.gridPosition.y);
        foreach (var neighbor in neighbors)
        {
            neighbor.physicalRoom.SetOpenState(false);
        }
        currentRoom.physicalRoom.SetOpenState(false);
        doorsOpen = false;
    }

    public void SelectNewRoom()
    {
        if (roomSelectionOnHalt) return;
        currentLights.Clear();
        currentLights.AddRange(FindObjectsByType<LightLogic>());


        EnableHands(currentRoom.physicalRoom.doors);
        ShowHands();
        roomSelectionOnHalt = true;
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

    public void ShowHands()
    {
        handsAnimator.SetBool("isHidden", false);
    }

    public void HideHands()
    {
        handsAnimator.SetBool("isHidden", true);
    }

    private void MoveToAnotherRoom(PhysicalRoom.DoorDirection direction)
    {
        PlayerControllerManager.Instance.SetPlayerStatus(PlayerStatus.Transitioning);
        OpenAllDoors();

        leftHand.ClearActions();
        rightHand.ClearActions();
        topHand.ClearActions();
        bottomHand.ClearActions();
        RoomGenerator.Room nextRoom;
        switch (direction)
        {
            case PhysicalRoom.DoorDirection.West:
                nextRoom = _generator.GetRoomFromGridPos(currentRoom.gridPosition.x - 1, currentRoom.gridPosition.y );
                MoveGamblingTable(nextRoom.worldPosition);
                currentRoom = nextRoom;
                break;
            case PhysicalRoom.DoorDirection.North:
                nextRoom = _generator.GetRoomFromGridPos(currentRoom.gridPosition.x , currentRoom.gridPosition.y+ 1);
                MoveGamblingTable(nextRoom.worldPosition);
                currentRoom = nextRoom;
                break;
            case PhysicalRoom.DoorDirection.East:
                nextRoom = _generator.GetRoomFromGridPos(currentRoom.gridPosition.x + 1, currentRoom.gridPosition.y);
                MoveGamblingTable(nextRoom.worldPosition);
                currentRoom = nextRoom;
                break;
            case PhysicalRoom.DoorDirection.South:
                nextRoom = _generator.GetRoomFromGridPos(currentRoom.gridPosition.x, currentRoom.gridPosition.y  - 1);
                MoveGamblingTable(nextRoom.worldPosition);
                currentRoom = nextRoom;
                break;
        }

        print(direction.ToString());
        HideHands();
    }
    
    private IEnumerator MoveGamblingTableSmoothly(Vector3 targetPos)
    {
        PlayerControllerManager.Instance.ChangePlayerController(PlayerControllerType.Sitting);

        Vector3 refPos = Vector3.zero;

        Vector3 flatTarget = new Vector3(targetPos.x, transform.position.y, targetPos.z);

        while (Vector3.Distance(transform.position, flatTarget) > 0.01f)
        {
            Vector3 newPos = Vector3.SmoothDamp(
                transform.position,
                flatTarget,
                ref refPos,
                smoothTime,
                Mathf.Infinity,
                Time.deltaTime
            );

            transform.position = new Vector3(newPos.x, transform.position.y, newPos.z);
            yield return null;
        }

        transform.position = flatTarget;
        StartCoroutine(ReachedNewRoom()) ;


    }
    
    private IEnumerator ReachedNewRoom()
    { 
        roomSelectionOnHalt = false;

        foreach (var lightOjb in currentLights)
        {
            lightOjb.TurnOff();
        }
        yield return new WaitForSeconds(0.5f);
       
        
        if (!currentRoom.isBattleRoom || currentRoom.visited)
        {
            SelectNewRoom();
        }
        else if( currentRoom.isBattleRoom)
        {
            CloseAllDoors();
            Dionysos.SetActive(true);
            _buttonInteractable.StartGambling();

        }
        
        yield return new WaitForSeconds(0.5f);

        foreach (var lightObj in currentLights)
        {
            if (lightObj != null)
            {
                lightObj.TurnOn();
            }
        }

        
        if(!currentRoom.visited)
          currentRoom.visited = true;
        
        if(PlayerControllerManager.Instance.currentPlayerStatus != PlayerStatus.Battling)
             PlayerControllerManager.Instance.SetPlayerStatus(PlayerStatus.Exploring);

        
        PlayerControllerManager.Instance.ChangePlayerController(PlayerControllerType.GamblingView);

    }
}
