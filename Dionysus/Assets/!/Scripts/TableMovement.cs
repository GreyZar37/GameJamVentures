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
        transform.position = _generator.playerSpawnRoom.gridPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectNewRoom()
    {
        
        
    }
}
