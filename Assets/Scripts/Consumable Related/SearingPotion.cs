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

	}

	void Update () {
		if(activated == false && consumableBonus.consumableActivated == true)
        {
            activated = true;
            StartCoroutine(cooldownReduce());
        }
	}
}
