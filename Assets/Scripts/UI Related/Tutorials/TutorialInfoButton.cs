using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialInfoButton : MonoBehaviour
{
    [SerializeField] List<TutorialEntry> allTutorialEntries;

    public void ShowTutorial()
    {
        PlayerProperties.tutorialWidgetMenu.Initialize(allTutorialEntries);
    }
}
