
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoomGenerator : MonoBehaviour
{
    
    private Room[,] _grid;
    private int _roomAmount;

    private Room _playerSpawnRoom, _bossSpawnRoom;
    private List<Room> _uncheckedRooms = new List<Room>();
    
    [Header("Generation Settings")]
    [SerializeField] private int gridSize = 3;
    
    [SerializeField] private int width;
    [SerializeField] private int height;
    
    [SerializeField] private int maxRooms;

    [Header("Prefabs")]
    [SerializeField] private PhysicalRoom[] roomPrefabs;

    [SerializeField] private GameObject westEastCorridorPrefab;
    [SerializeField] private GameObject northSouthCorridorPrefab;
    
    [SerializeField] private GameObject criticalPath;

 
 
 
 
 

    void Start()
    {
        _roomAmount = width * height;
        
        GenerateGrid();

        if (maxRooms > _grid.Length)
        {
            maxRooms = _grid.Length;
        }

        CreateGuaranteedPath(_playerSpawnRoom, _bossSpawnRoom);
        RemoveRandomRooms();
        GenerateDoors();
    }
    

    private void GenerateGrid()
    {
      _grid = new Room[width, height];
      

      for (int i = 0; i < width; i++)
      {
          for (int j = 0; j < height; j++)
          {
              var room = new Room();
              room.gridPosition = new Vector3Int(i * gridSize, 0, j * gridSize);
              var spawnedRoom = Instantiate(roomPrefabs[Random.Range(0, roomPrefabs.Length)], room.gridPosition, Quaternion.identity);
              room.roomPrefab = spawnedRoom.gameObject;
              _uncheckedRooms.Add(room);

              _grid[i,j] = room;
          }
      }
      _playerSpawnRoom = _grid[Random.Range(0, _grid.GetLength(0)), 0];
      _bossSpawnRoom =  _grid[Random.Range(0, _grid.GetLength(0) - 1), height - 1];
      
 
    }

    private void GenerateDoors()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Room room = _grid[x, y];

                if (room.roomPrefab == null)
                    continue;

                PhysicalRoom.DoorDirection doors = PhysicalRoom.DoorDirection.None;

                if (HasRoom(x, y + 1))
                    doors |= PhysicalRoom.DoorDirection.North;

                if (HasRoom(x + 1, y))
                    doors |= PhysicalRoom.DoorDirection.East;

                if (HasRoom(x, y - 1))
                    doors |= PhysicalRoom.DoorDirection.South;

                if (HasRoom(x - 1, y))
                    doors |= PhysicalRoom.DoorDirection.West;

                room.roomPrefab.GetComponent<PhysicalRoom>().CreateDoors(doors);
                SpawnCorridor(x,y, doors);
            }
        }
    }

    private void SpawnCorridor(int x, int y, PhysicalRoom.DoorDirection direction)
    {
        Vector3Int roomPos = GetWorldGridPos(x, y);

        if (direction.HasFlag(PhysicalRoom.DoorDirection.East))
        {
            Vector3 corridorPos = roomPos + new Vector3(gridSize / 2f, 0, 0);
            Instantiate(westEastCorridorPrefab, corridorPos, Quaternion.identity);
        }

        if (direction.HasFlag(PhysicalRoom.DoorDirection.North))
        {
            Vector3 corridorPos = roomPos + new Vector3(0, 0, gridSize / 2f);
            Instantiate(northSouthCorridorPrefab, corridorPos, Quaternion.identity);
        }
    }

    private bool HasRoom(int x, int y)
    {
        if (x < 0 || x >= width || y < 0 || y >= height)
            return false;

        return _grid[x, y].roomPrefab != null;
    }
    private void CreateGuaranteedPath(Room start, Room end)
    {
        Room current = start;
        current.markedProtected = true;

        while (current != end)
        {
            var xDir = Math.Sign( end.gridPosition.x - current.gridPosition.x);
            if (xDir != 0)
            {
                current = GetRoomFromWorldPos(current.gridPosition.x + (gridSize * xDir), current.gridPosition.z);
                current.markedProtected = true;
                Instantiate(criticalPath, current.gridPosition, Quaternion.identity);
                _uncheckedRooms.Remove(current);
            }

            var zDir = Math.Sign( end.gridPosition.z - current.gridPosition.z);
            if (zDir != 0)
            {
                current = GetRoomFromWorldPos(current.gridPosition.x, current.gridPosition.z + (gridSize * zDir));
                current.markedProtected = true;
                Instantiate(criticalPath, current.gridPosition, Quaternion.identity);
                _uncheckedRooms.Remove(current);
            }
        }
    }

    private Room GetRoomFromWorldPos(int x, int y)
    {
        foreach (var room in _grid)
        {
            if (room.gridPosition.x == x && room.gridPosition.z == y)
            {
                return room;
            }
        }
        return null;
    }

    private Vector3Int GetWorldGridPos(int x, int y)
    {
        return _grid[x,y].gridPosition;
    }
    
    private void RemoveRandomRooms()
    {
        while (_roomAmount > maxRooms && _uncheckedRooms.Count > 0)
        {
            var randomRoom = _uncheckedRooms[Random.Range(0, _uncheckedRooms.Count)];

            if (randomRoom.roomPrefab != null && !randomRoom.markedProtected)
            {
                
                Destroy(randomRoom.roomPrefab );
                randomRoom.roomPrefab = null;
                _roomAmount -= 1;
                _uncheckedRooms.Remove(randomRoom);

            }
        }
        
    }

    private void OnDrawGizmosSelected()
    {
         Gizmos.color = Color.green;
         Gizmos.DrawWireCube(_playerSpawnRoom.gridPosition, Vector3.one  * gridSize);
         Gizmos.color = Color.darkRed;
         Gizmos.DrawWireCube(_bossSpawnRoom.gridPosition, Vector3.one * gridSize);

    }


    private class Room
    {
        public  Vector3Int gridPosition;
        public GameObject roomPrefab;
        
        public Room[] neighbors;

        public bool markedProtected;
    }
}
