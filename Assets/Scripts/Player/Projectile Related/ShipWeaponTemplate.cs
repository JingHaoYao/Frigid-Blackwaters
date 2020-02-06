using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipWeaponTemplate : MonoBehaviour
{
    public Sprite up, upleft, left, downleft, down;
    public GameObject weaponFlare;
    public Sprite coolDownIcon;
    public float coolDownTime;
    public GameObject shipWeaponEquipped;
    public int whichLevelUnlock = 1;
}
