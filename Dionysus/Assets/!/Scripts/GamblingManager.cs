using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamblingManager : Singleton<GamblingManager>
{
    private static WaitForSecondsRealtime _waitForSeconds2 = new WaitForSecondsRealtime(2);
    [SerializeField] private Animator startBtnAnimator;
    
    [SerializeField] GameObject DicePoolPrefab;

    [SerializeField] Vector3[] dicePositions = new Vector3[2];

    GameState turn;


    [SerializeField] int PlayerPoints = 0;
    int PlayerDiffFromTarget;
    int EnemyDiffFromTarget;
    [SerializeField] int EnemyPoints = 0;

    [SerializeField] int TargetScore = 10;

    /// <summary>
    /// Boolean in the form of an int for turn purposes
    /// </summary>
    bool isPlayerFirstTurn = true;
    bool isEnemyFirstTurn = true;
    bool isPlayerLastTurn = false;
    bool isEnemyLastTurn = false;
    bool isRoundOver = false;
    bool isFirstTurn = true;
    private bool isGambling = false;

    public void SetGamblingSetup(bool isOn)
    {
        startBtnAnimator.SetBool("isAvailable", isOn);
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
        if (isRoundOver)
        {
            FinishRound();
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
                    DicePoolPrefab = Instantiate(DicePoolPrefab, dicePositions[0], Quaternion.identity);
                    DicePoolPrefab.transform.GetChild(0).rotation = UnityEngine.Random.rotation;
                    DicePoolPrefab.transform.GetChild(1).rotation = UnityEngine.Random.rotation;
                    yield return _waitForSeconds2;
                    List<char> PlayerRolls = DicePoolPrefab.GetComponent<Dice>().rolls;
                    if (isPlayerFirstTurn) 
                    { 
                        // isRoundOver = true; 
                        foreach (var roll in PlayerRolls)
                        {
                            PlayerPoints += roll - '0';
                            // Debug.Log($"Player Roll {roll}");
                        }
                    }
                    DicePoolPrefab.transform.position = dicePositions[0]; // Reroll
                    foreach (var roll in PlayerRolls)
                    {
                        if (roll == '1')
                        {
                            AddOrSubstract();
                        }
                        // Debug.Log($"Player Roll {roll}");
                    }
                    
                    PlayerDiffFromTarget = Math.Abs(TargetScore - PlayerPoints);
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
                    EnemyDiffFromTarget = Math.Abs(TargetScore - EnemyPoints);
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

    extern void AddOrSubstract(); // extern was just to avoid not giving this thing a body yet.

    void FinishRound()
    {
        if (PlayerDiffFromTarget < EnemyDiffFromTarget)
        {
            turn = GameState.WIN;
        }
        else
        {
            turn = GameState.LOSE;
        }
    }

    enum GameState
    {
        PLAYER_TURN,
        OPPONENT_TURN,
        WIN,
        LOSE
    }
}