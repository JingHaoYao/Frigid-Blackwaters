using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadWeaponChoices : MonoBehaviour {
    // Names of all weapon templates to load in
    string[] weaponTemplateNames = new string[17] {
        "Musket Weapon Template",
        "Cannon Weapon Template",
        "Shotgun Weapon Template",
        "Firework Weapon Template",
        "Dragon Breath Weapon Template",
        "Sniper Weapon Template",
        "Chemical Sprayer Template",
        "Glaive Launcher Weapon Template",
        "Plant Mortar Weapon Template",
        "Pod Flyers Weapon Template",
        "Pollux Shrine Weapon Template",
        "Lone Spark Weapon Template",
        "Gadget Shot Weapon Template",
        "Fin Blade Weapon Template",
        "Revolving Cannon Weapon Template",
        "Smelting Laser Weapon Template",
        "Tremor Maker Weapon Template"
    };
    public GameObject leftWeapon, rightWeapon, frontWeapon;

    public void loadWeapon(bool destroy = true)
    {
        leftWeapon.GetComponent<ShipWeaponScript>().swapTemplate(Resources.Load<ShipWeaponTemplate>("Player/Weapon Templates/" + weaponTemplateNames[PlayerUpgrades.whichLeftWeaponEquipped]).GetComponent<ShipWeaponTemplate>(), destroy);
        rightWeapon.GetComponent<ShipWeaponScript>().swapTemplate(Resources.Load<ShipWeaponTemplate>("Player/Weapon Templates/" + weaponTemplateNames[PlayerUpgrades.whichRightWeaponEquipped]).GetComponent<ShipWeaponTemplate>(), destroy);
        frontWeapon.GetComponent<ShipWeaponScript>().swapTemplate(Resources.Load<ShipWeaponTemplate>("Player/Weapon Templates/" + weaponTemplateNames[PlayerUpgrades.whichFrontWeaponEquipped]).GetComponent<ShipWeaponTemplate>(), destroy);
    }

	void Start () {
        loadWeapon(false);
	}
}
