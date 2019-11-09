using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadWeaponChoices : MonoBehaviour {
    // Names of all weapon templates to load in
    string[] weaponTemplateNames = new string[6] { "Musket Weapon Template", "Cannon Weapon Template", "Shotgun Weapon Template", "Firework Weapon Template", "Dragon Breath Weapon Template", "Sniper Weapon Template"};
    public GameObject leftWeapon, rightWeapon, frontWeapon;

    void loadWeapon()
    {
        leftWeapon.GetComponent<ShipWeaponScript>().swapTemplate(Resources.Load<ShipWeaponTemplate>("Player/Weapon Templates/" + weaponTemplateNames[PlayerUpgrades.whichLeftWeaponEquipped]).GetComponent<ShipWeaponTemplate>());
        rightWeapon.GetComponent<ShipWeaponScript>().swapTemplate(Resources.Load<ShipWeaponTemplate>("Player/Weapon Templates/" + weaponTemplateNames[PlayerUpgrades.whichRightWeaponEquipped]).GetComponent<ShipWeaponTemplate>());
        frontWeapon.GetComponent<ShipWeaponScript>().swapTemplate(Resources.Load<ShipWeaponTemplate>("Player/Weapon Templates/" + weaponTemplateNames[PlayerUpgrades.whichFrontWeaponEquipped]).GetComponent<ShipWeaponTemplate>());
    }

	void Start () {
        loadWeapon();
	}
}
