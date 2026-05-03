using UnityEngine;

public class DungeonMaster : MonoBehaviour
{
    public GameObject opponent;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GamblingManager2.Instance.OnGamblingEnd += OnBattleFinish;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnBattleFinish()
    {
        opponent.SetActive(false);
        TableMovement.Instance.SelectNewRoom();
        PlayerControllerManager.Instance.SetPlayerStatus(PlayerStatus.Exploring);
        
    }
}
