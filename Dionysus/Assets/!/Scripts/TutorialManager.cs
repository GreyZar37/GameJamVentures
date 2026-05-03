using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

class TutorialManager : MonoBehaviour
{
    [SerializeField] TMP_Text tutorialDialogue;
    [TextArea(3, 5)] public List<string> dialogueDescription;
    [SerializeField] Button continueButton;

    int dialogueIndex = 0;

    void Awake()
    {
        tutorialDialogue.text = "Welcome to The Trial of Dionysus";
    }

    public void ClickedContinue()
    {
        if (dialogueIndex >= dialogueDescription.Count) { SceneManager.LoadScene("RoomTemplate"); }
        else
        {
            tutorialDialogue.text = dialogueDescription[dialogueIndex];
            dialogueIndex++;
        }
    }
}