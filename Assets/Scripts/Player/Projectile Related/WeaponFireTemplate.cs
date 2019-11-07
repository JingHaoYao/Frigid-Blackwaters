using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponFireTemplate : MonoBehaviour
{
    public abstract GameObject fireWeapon(int whichSide, float angleOrientation, GameObject weaponPlume);
}
