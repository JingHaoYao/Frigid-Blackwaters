using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadWeaponChoices : MonoBehaviour {
    public GameObject[] weaponTemplates;
    public GameObject leftWeapon, rightWeapon, frontWeapon;

    void loadWeapon()
    {
        leftWeapon.GetComponent<ShipWeaponScript>().swapTemplate(weaponTemplates[PlayerUpgrades.whichLeftWeaponEquipped].GetComponent<ShipWeaponTemplate>());
        rightWeapon.GetComponent<ShipWeaponScript>().swapTemplate(weaponTemplates[PlayerUpgrades.whichRightWeaponEquipped].GetComponent<ShipWeaponTemplate>());
        frontWeapon.GetComponent<ShipWeaponScript>().swapTemplate(weaponTemplates[PlayerUpgrades.whichFrontWeaponEquipped].GetComponent<ShipWeaponTemplate>());
    }

	void Start () {
        loadWeapon();
	}
}
