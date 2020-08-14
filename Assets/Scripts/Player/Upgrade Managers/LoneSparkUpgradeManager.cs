using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoneSparkUpgradeManager : WeaponFireTemplate
{
    ShipWeaponScript weaponScript;
    ShipWeaponTemplate weaponTemplate;
    int prevNumberUpgrades;

    [SerializeField] GameObject sparkInitial;

    float origCoolDownTime;

    void applyUpgrades()
    {
        if (PlayerUpgrades.loneSparkUpgrades.Count >= 3)
        {
            weaponTemplate.coolDownTime = Mathf.Round(origCoolDownTime * 0.85f * 100f) / 100f;
        }
        else if(PlayerUpgrades.loneSparkUpgrades.Count <= 2)
        {
            weaponTemplate.coolDownTime = origCoolDownTime;
        }
    }

    void Start()
    {
        prevNumberUpgrades = PlayerUpgrades.loneSparkUpgrades.Count;
        weaponScript = this.GetComponent<ShipWeaponTemplate>().shipWeaponEquipped.GetComponent<ShipWeaponScript>();
        weaponTemplate = GetComponent<ShipWeaponTemplate>();
        origCoolDownTime = weaponTemplate.coolDownTime;
        applyUpgrades();
        weaponScript.setTemplate();
    }

    void Update()
    {
        if (prevNumberUpgrades != PlayerUpgrades.loneSparkUpgrades.Count)
        {
            prevNumberUpgrades = PlayerUpgrades.loneSparkUpgrades.Count;
            applyUpgrades();
            weaponScript.setTemplate();
        }
    }

    public override GameObject fireWeapon(int whichSide, float angleOrientation, GameObject weaponPlume)
    {
        GameObject instant = Instantiate(sparkInitial, weaponScript.transform.position + Vector3.up * 0.5f, Quaternion.identity);
        return instant;
    }
}
