using System;
using UnityEngine;


public class PhysicalRoom : MonoBehaviour
{
    [Flags]
    public enum DoorDirection
    {
        None  = 0,
        North = 1,
        East  = 2,
        South = 4,
        West  = 8
    }
    
    public DoorDirection doors;

    [Header("West Door/Wall")]
    [SerializeField] private GameObject westDoor;
    [SerializeField] private GameObject westWall;
    
    [Header("North Door/Wall")]
    [SerializeField] private GameObject northDoor;
    [SerializeField] private GameObject northWall;
 
    [Header("East Door/Wall")]
    [SerializeField] private GameObject eastDoor;
    [SerializeField] private GameObject eastWall;

    [Header("South Door/Wall")]
    [SerializeField] private GameObject southDoor;
    [SerializeField] private GameObject southWall;

    public void CreateDoors(DoorDirection assignDoors)
    {
        doors = assignDoors;
        northDoor.SetActive(assignDoors.HasFlag(DoorDirection.North));
        northWall.SetActive(!assignDoors.HasFlag(DoorDirection.North));
        
        eastDoor.SetActive(assignDoors.HasFlag(DoorDirection.East));
        eastWall.SetActive(!assignDoors.HasFlag(DoorDirection.East));
        
        southDoor.SetActive(assignDoors.HasFlag(DoorDirection.South));
        southWall.SetActive(!assignDoors.HasFlag(DoorDirection.South));
        
        westDoor.SetActive(assignDoors.HasFlag(DoorDirection.West));
        westWall.SetActive(!assignDoors.HasFlag(DoorDirection.West));
    }
 
}
