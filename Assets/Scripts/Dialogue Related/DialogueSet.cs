using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSet : MonoBehaviour
{
    public int whatDialogueType;
    public bool checkPointDialogue = false;
    // 1 - tavern dialogues
    // 2 - dungeon entrance dialogues
    // 3 - boss dialogues (No longer included)
    // 4 - examine dialogues
    // 5 - shop dialogues
    // 6 - returning from hub dialogues
    // 7 - story dialogues
    // 8 - unique room dialogues
    public string[] dialogues;
    public string[] dialogueNames;
    public Sprite background;
    public Sprite[] character1Sprites;
    public Sprite[] character2Sprites;
    public Sprite[] character3Sprites;
    public Sprite[] character4Sprites;
    public Sprite[] panelSprites;
    public string substituteMusic;
    public string originalMusic;
    public GameObject[] addedItems;
    public Color[] textColors;
}
