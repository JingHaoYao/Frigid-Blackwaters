using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearingPotion : MonoBehaviour {
    ShipWeaponScript[] shipWeaponScripts;
    ConsumableBonus consumableBonus;
    bool activated;

    IEnumerator cooldownReduce()
    {
        foreach(ShipWeaponScript element in shipWeaponScripts)
        {
            element.coolDownThreshold /= 1.25f;
        }
        yield return new WaitForSeconds(60f);
        foreach(ShipWeaponScript element in shipWeaponScripts)
        {
            element.coolDownThreshold *= 1.25f;
        }
    }

	void Start () {
        consumableBonus = GetComponent<ConsumableBonus>();
        shipWeaponScripts = GameObject.Find("PlayerShip").GetComponentsInChildren<ShipWeaponScript>();
        consumableBonus.SetAction(coolDownReduceAction);
	}

    void coolDownReduceAction()
    {
        if (activated == false)
        {
            activated = true;
            StartCoroutine(cooldownReduce());
        }
    }
}
