using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantMortarUpgradeManager : WeaponFireTemplate
{
    ShipWeaponScript weaponScript;
    ShipWeaponTemplate weaponTemplate;
    GameObject empoweredWeaponFlare;
    public GameObject regularAirBlast1, regularAirBlast2, ailaBlast1, ailaBlast2, ailaBlast3, spreadBlast1, spreadblast2, spreadBlast3;

    int prevNumberUpgrades;
    float origCoolDownTime;
    bool ailaPlantUpgrade = false, spreadBlastUpgrade = false;
    int numberOfAilaPlantsShot;
    int numberShotsThreshold = 0;

    void applyUpgrades()
    {
        weaponTemplate.weaponFlare = regularAirBlast1;
        if (PlayerUpgrades.plantMortarUpgrades.Count == 3)
        {
            weaponTemplate.coolDownTime = Mathf.Round(origCoolDownTime * 0.85f * 100f) / 100f;
            ailaPlantUpgrade = false;
            spreadBlastUpgrade = false;
        }
        else if(PlayerUpgrades.plantMortarUpgrades.Count == 2)
        {
            weaponTemplate.coolDownTime = origCoolDownTime;
            ailaPlantUpgrade = false;
            spreadBlastUpgrade = false;
            weaponTemplate.weaponFlare = regularAirBlast2;
        }
        else if (PlayerUpgrades.plantMortarUpgrades.Count > 3)
        {
            weaponTemplate.coolDownTime = Mathf.Round(origCoolDownTime * 0.85f * 100f) / 100f;
            if (PlayerUpgrades.plantMortarUpgrades[3] == "unlock_aila_upgrade")
            {
                if (PlayerUpgrades.plantMortarUpgrades.Count == 4)
                {
                    ailaPlantUpgrade = true;
                    empoweredWeaponFlare = ailaBlast1;
                    numberShotsThreshold = 3;
                }
                else if (PlayerUpgrades.plantMortarUpgrades.Count == 5)
                {
                    ailaPlantUpgrade = true;
                    empoweredWeaponFlare = ailaBlast2;
                    numberShotsThreshold = 3;
                }
                else
                {
                    ailaPlantUpgrade = true;
                    empoweredWeaponFlare = ailaBlast3;
                    numberShotsThreshold = 2;
                }
            }
            else
            {
                if (PlayerUpgrades.plantMortarUpgrades.Count == 4)
                {
                    spreadBlastUpgrade = true;
                    empoweredWeaponFlare = spreadBlast1;
                }
                else if (PlayerUpgrades.plantMortarUpgrades.Count == 5)
                {
                    spreadBlastUpgrade = true;
                    empoweredWeaponFlare = spreadblast2;
                }
                else
                {
                    spreadBlastUpgrade = true;
                    empoweredWeaponFlare = spreadBlast3;
                }
            }
        }
        else
        {
            weaponTemplate.coolDownTime = origCoolDownTime;
            ailaPlantUpgrade = false;
            spreadBlastUpgrade = false;
        }
    }


    void Start()
    {
        prevNumberUpgrades = PlayerUpgrades.plantMortarUpgrades.Count;
        weaponScript = this.GetComponent<ShipWeaponTemplate>().shipWeaponEquipped.GetComponent<ShipWeaponScript>();
        weaponTemplate = GetComponent<ShipWeaponTemplate>();
        origCoolDownTime = weaponTemplate.coolDownTime;
        applyUpgrades();
        weaponScript.setTemplate();
    }

    void Update()
    {
        if (prevNumberUpgrades != PlayerUpgrades.plantMortarUpgrades.Count)
        {
            prevNumberUpgrades = PlayerUpgrades.plantMortarUpgrades.Count;
            applyUpgrades();
            weaponScript.setTemplate();
        }

        if (ailaPlantUpgrade)
        {

            if (PlayerProperties.playerScript.enemiesDefeated == false)
            {
                if (weaponScript.numberShots == numberShotsThreshold - 1)
                {
                    weaponScript.weaponPlume = empoweredWeaponFlare;
                }

                if (weaponScript.numberShots >= numberShotsThreshold)
                {
                    weaponScript.numberShots = 0;
                    weaponScript.weaponPlume = regularAirBlast2;
                }
            }
            else
            {
                if (weaponScript.weaponPlume != regularAirBlast2)
                {
                    weaponScript.weaponPlume = regularAirBlast2;
                }
            }
        }

        if (spreadBlastUpgrade)
        {
            if (weaponScript.weaponPlume != empoweredWeaponFlare)
            {
                weaponScript.weaponPlume = empoweredWeaponFlare;
            }
        }
    }

    public override GameObject fireWeapon(int whichSide, float angleOrientation, GameObject weaponPlume)
    {
        GameObject instant = Instantiate(weaponPlume, weaponScript.transform.position + Vector3.up * 0.5f, Quaternion.identity);
        return instant;
    }
}
