using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class TutorialEntry
{
    [SerializeField] public Characters whichCharacter;
    [SerializeField] public string dialogueText;
    [SerializeField] public Vector3 localPosition;
    [SerializeField] public Vector2 highlightBoxDimensions;
    [SerializeField] public Vector3 highlightBoxPosition;
    [SerializeField] public bool hideHighlightBox;
    [SerializeField] public UnityEvent stepAction;

    public enum Characters
    {
        Elora,
        Rowan,
        Tabitha,
        Malfar,
        Geordi,
        Merrow
    }
}
