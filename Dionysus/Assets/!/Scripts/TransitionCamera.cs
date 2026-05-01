using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Camera), typeof(AudioListener))]
public class TransitionCamera : Singleton<TransitionCamera>
{
    private Camera cam;
    private AudioListener listener;
    [SerializeField] private float transitionTime = 0.5f;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        listener = GetComponent<AudioListener>();

        SetCameraAndListener(false);
    }

    public void StartTransition(Transform start, Transform end, Action finishCallback = null)
    {
        StopAllCoroutines();
        StartCoroutine(RunTransition(start, end, finishCallback));
    }
    
    private IEnumerator RunTransition(Transform start, Transform end, Action finishCallback = null)
    {
        SetCameraAndListener(true);

        transform.SetPositionAndRotation(start.position, start.rotation);

        float progress = 0f;
        while(Vector3.Distance(transform.position, end.position) > 0.01f)
        {
            transform.position = Vector3.Lerp(start.position, end.position, progress);
            transform.rotation = Quaternion.Lerp(start.rotation, end.rotation, progress);
            progress += Time.deltaTime / transitionTime;
            yield return null;
        }
        transform.SetPositionAndRotation(end.position, end.rotation);

        SetCameraAndListener(false);

        finishCallback?.Invoke();
    }

    private void SetCameraAndListener(bool isOn)
    {
        cam.enabled = isOn;
        listener.enabled = isOn;
    }
}