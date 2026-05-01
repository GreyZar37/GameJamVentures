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

    [SerializeField] private GameObject westDoor;
    [SerializeField] private GameObject northDoor;
    [SerializeField] private GameObject eastDoor;
    [SerializeField] private GameObject southDoor;

    public void CreateDoors(DoorDirection assignDoors)
    {
        doors = assignDoors;
        northDoor.SetActive(!assignDoors.HasFlag(DoorDirection.North));
        eastDoor.SetActive(!assignDoors.HasFlag(DoorDirection.East));
        southDoor.SetActive(!assignDoors.HasFlag(DoorDirection.South));
        westDoor.SetActive(!assignDoors.HasFlag(DoorDirection.West));
    }
}
