using System.Collections;
using UnityEngine;

public class GamblingManager : Singleton<GamblingManager>
{
    [SerializeField] private Animator startBtnAnimator;
    [SerializeField] private float smoothTime = 0.125f;

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
    }
    public void StopGambling()
    {
        isGambling = false;
    }
}