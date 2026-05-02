
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

public class RoomGenerator : MonoBehaviour
{
    
    
    private Room[,] _grid;
    private int _roomAmount;

    public Room playerSpawnRoom, bossSpawnRoom;
    private List<Room> _uncheckedRooms = new List<Room>();


    public Action OnPlayerSpawn;
    
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

    [SerializeField] private GameObject playerTable;
    void Start()
    {
        _roomAmount = width * height;
        
        GenerateGrid();

        if (maxRooms > _grid.Length)
        {
            maxRooms = _grid.Length;
        }

        CreateGuaranteedPath(playerSpawnRoom, bossSpawnRoom);
        RemoveRandomRooms();
        GenerateDoors();
        var spawnPos = new  Vector3(playerSpawnRoom.worldPosition.x, 1.25f, playerSpawnRoom.worldPosition.z);
        Instantiate(playerTable, spawnPos, Quaternion.identity);
        OnPlayerSpawn?.Invoke();
        
    }
    

    private void GenerateGrid()
    {
      _grid = new Room[width, height];
      

      for (int i = 0; i < width; i++)
      {
          for (int j = 0; j < height; j++)
          {
              var room = new Room();
              room.worldPosition = new Vector3Int(i * gridSize, 0, j * gridSize);
              var spawnedRoom = Instantiate(roomPrefabs[Random.Range(0, roomPrefabs.Length)], room.worldPosition, Quaternion.identity);
              room.roomPrefab = spawnedRoom.gameObject;
              room.physicalRoom = spawnedRoom.GetComponent<PhysicalRoom>();
              room.gridPosition = new Vector2Int(i, j);
              _uncheckedRooms.Add(room);

              _grid[i,j] = room;
          }
      }
      playerSpawnRoom = _grid[Random.Range(0, _grid.GetLength(0)), 0];
      bossSpawnRoom =  _grid[Random.Range(0, _grid.GetLength(0) - 1), height - 1];
      
 
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
        _uncheckedRooms.Remove(current);


        while (current != end)
        {
            var xDir = Math.Sign( end.worldPosition.x - current.worldPosition.x);
            if (xDir != 0)
            {
                current = GetRoomFromWorldPos(current.worldPosition.x + (gridSize * xDir), current.worldPosition.z);
                current.markedProtected = true;
                Instantiate(criticalPath, current.worldPosition, Quaternion.identity);
                _uncheckedRooms.Remove(current);
            }

            var zDir = Math.Sign( end.worldPosition.z - current.worldPosition.z);
            if (zDir != 0)
            {
                current = GetRoomFromWorldPos(current.worldPosition.x, current.worldPosition.z + (gridSize * zDir));
                current.markedProtected = true;
                Instantiate(criticalPath, current.worldPosition, Quaternion.identity);
                _uncheckedRooms.Remove(current);
            }
        }
    }

    private Room GetRoomFromWorldPos(int x, int y)
    {
        foreach (var room in _grid)
        {
            if (room.worldPosition.x == x && room.worldPosition.z == y)
            {
                return room;
            }
        }
        return null;
    }
    
   

    public Vector3Int GetWorldGridPos(int x, int y)
    {
        return _grid[x,y].worldPosition;
    }

    public Room GetRoomFromGridPos(int x, int y)
    {
        return _grid[x, y];
    }
    
    private void RemoveRandomRooms()
    {
        while (_roomAmount > maxRooms && _uncheckedRooms.Count > 0)
        {
            var randomRoom = _uncheckedRooms[Random.Range(0, _uncheckedRooms.Count)];

            if (randomRoom.roomPrefab != null && !randomRoom.markedProtected)
            {
                GameObject savedPrefab = randomRoom.roomPrefab;

                randomRoom.roomPrefab = null;

                if (RoomsAreStillConnected())
                {
                    Destroy(savedPrefab);
                    _roomAmount -= 1;
                }
                else
                {
                    randomRoom.roomPrefab = savedPrefab;
                }

                _uncheckedRooms.Remove(randomRoom);
            }
            else
            {
                _uncheckedRooms.Remove(randomRoom);
            }
        }
    }
    
    private bool RoomsAreStillConnected()
    {
        Room startRoom = null;
        int totalRooms = 0;

        foreach (Room room in _grid)
        {
            if (room.roomPrefab != null)
            {
                totalRooms++;

                if (startRoom == null)
                    startRoom = room;
            }
        }

        if (startRoom == null)
            return true;

        HashSet<Room> visited = new HashSet<Room>();
        Queue<Room> queue = new Queue<Room>();

        visited.Add(startRoom);
        queue.Enqueue(startRoom);

        while (queue.Count > 0)
        {
            Room current = queue.Dequeue();

            int x = current.worldPosition.x / gridSize;
            int y = current.worldPosition.z / gridSize;

            CheckNeighbor(x + 1, y, visited, queue);
            CheckNeighbor(x - 1, y, visited, queue);
            CheckNeighbor(x, y + 1, visited, queue);
            CheckNeighbor(x, y - 1, visited, queue);
        }

        return visited.Count == totalRooms;
    }
    
    private void CheckNeighbor(int x, int y, HashSet<Room> visited, Queue<Room> queue)
    {
        if (x < 0 || x >= width || y < 0 || y >= height)
            return;

        Room neighbor = _grid[x, y];

        if (neighbor.roomPrefab == null)
            return;

        if (!visited.Add(neighbor))
            return;

        queue.Enqueue(neighbor);
    }


    public List<Room> GetNeighbors(int x, int y)
    {
        List<Room> neighbors = new List<Room>();

        AddNeighbor(x + 1, y, neighbors); 
        AddNeighbor(x - 1, y, neighbors); 
        AddNeighbor(x, y + 1, neighbors);
        AddNeighbor(x, y - 1, neighbors); 

        return neighbors;
    }

    private void AddNeighbor(int x, int y, List<Room> neighbors)
    {
        if (x < 0 || x >= width || y < 0 || y >= height)
            return;

        Room neighbor = _grid[x, y];

        if (neighbor == null)
            return;

        if (neighbor.roomPrefab == null)
            return;

        neighbors.Add(neighbor);
    }

    public class Room
    {
        public Vector2Int gridPosition;
        public Vector3Int worldPosition;
        public GameObject roomPrefab;
        public PhysicalRoom physicalRoom;
        
        public Room[] neighbors;

        public bool markedProtected;
    }
}
