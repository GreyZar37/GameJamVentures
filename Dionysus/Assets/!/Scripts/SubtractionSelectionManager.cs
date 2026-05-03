using System;
using UnityEngine;

public class SubtractionSelectionManager : MonoBehaviour
{
    private Action<SubtractionSelection> gamblingManagerCallback;
    [SerializeField] private GameObject selectionsParent;

    private void Start()
    {
        selectionsParent.SetActive(false);
    }

    public void SetupSelection(Action<SubtractionSelection> callback)
    {
        gamblingManagerCallback = callback;
        selectionsParent.SetActive(true);
    }

    public void SelectChoice(SubtractionSelection selection)
    {
        if (gamblingManagerCallback == null) return;
        gamblingManagerCallback?.Invoke(selection);
        gamblingManagerCallback = null;
        selectionsParent.SetActive(false);
    }
}

public enum SubtractionSelection
{
    Add, Subtract, Stop
}