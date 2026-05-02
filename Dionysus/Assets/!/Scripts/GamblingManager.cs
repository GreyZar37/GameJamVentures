using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamblingManager : Singleton<GamblingManager>
{
    [SerializeField] private Animator startBtnAnimator;
    [SerializeField] private float smoothTime = 0.125f;
    [SerializeField] GameObject DicePoolPrefab;

    [SerializeField] Vector3[] dicePositions = new Vector3[2];

    GameState turn;


    int PlayerPoints = 0;
    int EnemyPoints = 0;

    /// <summary>
    /// Boolean in the form of an int for turn purposes
    /// </summary>
    int isPlayersTurn = 0;

    private bool isGambling = false;

    public void SetGamblingSetup(bool isOn)
    {
        startBtnAnimator.SetBool("isAvailable", isOn);
    }

    public void MoveGamblingTable(Vector3 targetPos)
    {
        StopAllCoroutines();
        StartCoroutine(MoveGamblingTableSmoothly(targetPos));
    }

    private IEnumerator MoveGamblingTableSmoothly(Vector3 targetPos)
    {
        Vector3 refPos = Vector3.zero;
        while(Vector3.Distance(transform.position, targetPos) > 0.01f)
        {
            var newPos = Vector3.SmoothDamp(transform.position, targetPos, ref refPos, smoothTime, Mathf.Infinity,
                Time.deltaTime);
            transform.position = new Vector3(newPos.x, transform.position.y, newPos.z);
            yield return null;
        }
        transform.position = targetPos;
    }

    public void StartGambling()
    {
        isGambling = true;
        var values = Enum.GetValues(typeof(GameState));
        int participantTurn = UnityEngine.Random.Range(0, 2);
        turn = (GameState) values.GetValue(participantTurn);
        switch (turn)
        {
            case GameState.PLAYER_TURN:
                Debug.Log("Players turn");
                DicePoolPrefab = Instantiate(DicePoolPrefab, dicePositions[0], UnityEngine.Random.rotation);
                turn = GameState.OPPONENT_TURN;
                List<char> PlayerRolls = DicePoolPrefab.GetComponent<Dice>().rolls;
                foreach (var roll in PlayerRolls)
                {
                    PlayerPoints += Convert.ToInt32(roll);
                    Debug.Log($"Player Roll {roll}");
                }
                break;
            case GameState.OPPONENT_TURN:
                Debug.Log("Enemies turn");
                DicePoolPrefab = Instantiate(DicePoolPrefab, dicePositions[1], UnityEngine.Random.rotation);
                List<char> EnemyRolls = DicePoolPrefab.GetComponent<Dice>().rolls;
                foreach (var roll in EnemyRolls)
                {
                    EnemyPoints += Convert.ToInt32(roll);
                    Debug.Log($"Enemy Roll {roll}");
                }
                turn = GameState.PLAYER_TURN;
                break;
            default:
                Debug.Log("Something is wrong here");
                break;
        }
        if (!isGambling)
        {
            return;
        }
    }
    public void StopGambling()
    {
        isGambling = false;
    }

    enum GameState
    {
        PLAYER_TURN,
        OPPONENT_TURN,
        WIN,
        LOSE
    }
}