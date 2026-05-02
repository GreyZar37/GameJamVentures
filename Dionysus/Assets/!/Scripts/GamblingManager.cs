using System.Collections;
using UnityEngine;

public class GamblingManager : Singleton<GamblingManager>
{
    [SerializeField] private Animator startBtnAnimator;
    [SerializeField] private float smoothTime = 0.125f;
    [SerializeField] GameObject DicePoolPrefab;

    [SerializeField] Vector3[] dicePositions = new Vector3[2];

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
            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref refPos, smoothTime, Mathf.Infinity, Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;
    }

    public void StartGambling()
    {
        isGambling = true;
        isPlayersTurn = Random.Range(0,2);
        if (isPlayersTurn == 1)
        {
            Instantiate(DicePoolPrefab, dicePositions[0], Random.rotation);
            isPlayersTurn += 1;
            StartGambling();
        }
        else if (isPlayersTurn > 1)
        {
            Instantiate(DicePoolPrefab, dicePositions[1], Random.rotation);
            StopGambling();
        }
        else
        {
            Instantiate(DicePoolPrefab, dicePositions[1], Random.rotation);
            isPlayersTurn = 1;
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
}