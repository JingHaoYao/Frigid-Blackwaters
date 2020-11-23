using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponFireTemplate : MonoBehaviour
{
    public virtual GameObject fireWeapon(int whichSide, float angleOrientation, GameObject weaponPlume)
    {
        return null;
    }

    public virtual void InitializeTextIcon(Text text) {
        text.enabled = false;
    }

    public virtual void TookDamage(int damage, Enemy enemy)
    {

    }

    public virtual void KilledEnemy(Enemy enemy)
    {

    }
}
