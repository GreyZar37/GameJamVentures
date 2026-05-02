using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamblingManager : Singleton<GamblingManager>
{
    private static WaitForSecondsRealtime _waitForSeconds2 = new WaitForSecondsRealtime(2);
    [SerializeField] private Animator startBtnAnimator;
    [SerializeField] private float smoothTime = 0.125f;
    [SerializeField] GameObject DicePoolPrefab;

    [SerializeField] Vector3[] dicePositions = new Vector3[2];

    GameState turn;


    [SerializeField] int PlayerPoints = 0;
    [SerializeField] int EnemyPoints = 0;

    /// <summary>
    /// Boolean in the form of an int for turn purposes
    /// </summary>
    bool isFirstTurn = true;
    bool isRoundOver = false;
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
        if (isFirstTurn)
        {
            int participantTurn = UnityEngine.Random.Range(0, 2);
            turn = (GameState) values.GetValue(participantTurn);
        }
        StartCoroutine(Gamble());
            /*
            if (!isGambling)
            {
                yield break;
            }
            */
    }
        IEnumerator Gamble()
        {
            
            switch (turn)
            {
                case GameState.PLAYER_TURN:
                    if (isRoundOver) { break; }
                    Debug.Log("Players turn");
                    if (!isFirstTurn) { isRoundOver = true; }
                    DicePoolPrefab = Instantiate(DicePoolPrefab, dicePositions[0], Quaternion.identity);
                    DicePoolPrefab.transform.GetChild(0).rotation = UnityEngine.Random.rotation;
                    DicePoolPrefab.transform.GetChild(1).rotation = UnityEngine.Random.rotation;
                    yield return _waitForSeconds2;
                    List<char> PlayerRolls = DicePoolPrefab.GetComponent<Dice>().rolls;
                    foreach (var roll in PlayerRolls)
                    {
                        PlayerPoints += roll - '0';
                        // Debug.Log($"Player Roll {roll}");
                    }
                    turn = GameState.OPPONENT_TURN;
                    isFirstTurn = false;
                    StartCoroutine(Gamble());
                    break;
                case GameState.OPPONENT_TURN:
                    if (isRoundOver) { break; }
                    Debug.Log("Enemies turn");
                    if (!isFirstTurn) { isRoundOver = true; }
                    DicePoolPrefab = Instantiate(DicePoolPrefab, dicePositions[1], Quaternion.identity);
                    DicePoolPrefab.transform.GetChild(0).rotation = UnityEngine.Random.rotation;
                    DicePoolPrefab.transform.GetChild(1).rotation = UnityEngine.Random.rotation;
                    yield return _waitForSeconds2;
                    List<char> EnemyRolls = DicePoolPrefab.GetComponent<Dice>().rolls;
                    foreach (var roll in EnemyRolls)
                    {
                        // EnemyPoints += int.Parse(roll);
                        EnemyPoints += roll - '0';
                        // Debug.Log($"Enemy Roll {roll}");
                    }
                    turn = GameState.PLAYER_TURN;
                    isFirstTurn = false;
                    StartCoroutine(Gamble());
                    break;
                default:
                    Debug.Log("Something is wrong here");
                    break;
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