using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusketUpgradeManager : MonoBehaviour {
    ShipWeaponScript weaponScript;
    ShipWeaponTemplate weaponTemplate;
    int prevNumberUpgrades;
    bool precisionShot = false, scatterShot = false;
    public GameObject precisionBlast1, precisionBlast2, regularMusketSmoke, scatterBlast1, scatterBlast2;
    GameObject empoweredWeaponFlare;
    int numberShotsThreshold = 4;
    float origCoolDownTime;
    public Sprite regularMusketIcon, scatterShotIcon, precisionShotIcon;

    void applyUpgrades()
    {
        if(PlayerUpgrades.musketUpgrades.Count == 3)
        {
            weaponTemplate.coolDownTime = Mathf.Round(origCoolDownTime * 0.85f * 100f) / 100f;
            precisionShot = false;
            scatterShot = false;
        }
        else if(PlayerUpgrades.musketUpgrades.Count > 3)
        {
            weaponTemplate.coolDownTime = Mathf.Round(origCoolDownTime * 0.85f * 100f) / 100f;
            if (PlayerUpgrades.musketUpgrades[3] == "precision_shot_unlocked")
            {
                if(PlayerUpgrades.musketUpgrades.Count == 4)
                {
                    precisionShot = true;
                    empoweredWeaponFlare = precisionBlast1;
                    numberShotsThreshold = 4;
                }
                else if(PlayerUpgrades.musketUpgrades.Count == 5)
                {
                    precisionShot = true;
                    empoweredWeaponFlare = precisionBlast1;
                    numberShotsThreshold = 3;
                }
                else
                {
                    precisionShot = true;
                    empoweredWeaponFlare = precisionBlast2;
                    numberShotsThreshold = 3;
                }
            }
            else
            {
                if (PlayerUpgrades.musketUpgrades.Count == 4)
                {
                    scatterShot = true;
                    empoweredWeaponFlare = scatterBlast1;
                    numberShotsThreshold = 4;
                }
                else if (PlayerUpgrades.musketUpgrades.Count == 5)
                {
                    scatterShot = true;
                    empoweredWeaponFlare = scatterBlast1;
                    numberShotsThreshold = 3;
                }
                else
                {
                    scatterShot = true;
                    empoweredWeaponFlare = scatterBlast2;
                    numberShotsThreshold = 3;
                }
            }
        }
        else
        {
            weaponTemplate.coolDownTime = origCoolDownTime;
            precisionShot = false;
            scatterShot = false;
        }
    }

	void Start () {
        prevNumberUpgrades = PlayerUpgrades.musketUpgrades.Count;
        weaponScript = this.GetComponent<ShipWeaponTemplate>().shipWeaponEquipped.GetComponent<ShipWeaponScript>();
        weaponTemplate = GetComponent<ShipWeaponTemplate>();
        origCoolDownTime = weaponTemplate.coolDownTime;
        applyUpgrades();
        weaponScript.setTemplate();
    }

	void Update () {
		if(prevNumberUpgrades != PlayerUpgrades.musketUpgrades.Count)
        {
            prevNumberUpgrades = PlayerUpgrades.musketUpgrades.Count;
            applyUpgrades();
            weaponScript.setTemplate();
        }

        if (precisionShot || scatterShot)
        {
            if (weaponScript.numberShots == numberShotsThreshold - 1)
            {
                weaponScript.musketSmoke = empoweredWeaponFlare;
                if (scatterShot)
                {
                    weaponScript.weaponIcon.sprite = scatterShotIcon;
                }
                else
                {
                    weaponScript.weaponIcon.sprite = precisionShotIcon;
                }
            }

            if (weaponScript.numberShots >= numberShotsThreshold)
            {
                weaponScript.numberShots = 0;
                weaponScript.musketSmoke = regularMusketSmoke;
                weaponScript.weaponIcon.sprite = regularMusketIcon;
            }
        }
    }
}
