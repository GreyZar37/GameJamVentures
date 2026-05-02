using System;
using System.Collections;
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
                break;
            case GameState.OPPONENT_TURN:
                break;
            case GameState.WIN:
                break;
            default:
                break;
        }
        if (isPlayersTurn == 1)
        {
            Debug.Log("Players turn");
            Instantiate(DicePoolPrefab, dicePositions[0], UnityEngine.Random.rotation);
            isPlayersTurn += 1;
            StartGambling();
        }
        else if (isPlayersTurn > 1)
        {
            Debug.Log("Enemies turn");
            Instantiate(DicePoolPrefab, dicePositions[1], UnityEngine.Random.rotation);
            return;
            //StopGambling();
        }
        else
        {
            Debug.Log("Enemy is starting");
            Instantiate(DicePoolPrefab, dicePositions[1], UnityEngine.Random.rotation);
            isPlayersTurn = 1;
            isGambling = false;
            StartGambling();
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