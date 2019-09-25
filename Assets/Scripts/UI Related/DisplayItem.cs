using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayItem : MonoBehaviour {
    public Sprite displayIcon;
    public int goldValue;
    public bool isArtifact;
    public bool isConsumable;
    //if the artifact has actives
    public bool hasActive;
    public bool isEquipped;
    public bool questItem;
    public int whichSlot;
    public bool soulBound;
}
