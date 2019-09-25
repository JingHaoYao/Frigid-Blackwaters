using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonsBreathUpgradeManager : MonoBehaviour {
    ShipWeaponScript weaponScript;
    ShipWeaponTemplate weaponTemplate;
    int prevNumberUpgrades;
    bool blueFire = false, longerFlames = false;
    public GameObject longerFlames1, longerFlames2, longerFlames3, blueFlames1, blueFlames2, blueFlames3;
    GameObject empoweredWeaponFlare;
    float origCoolDownTime;
    public Sprite blueFireIcon, longerFlamesIcon;
    int numberPrevShots;

    void applyUpgrades()
    {
        if (PlayerUpgrades.dragonBreathUpgrades.Count == 3)
        {
            weaponTemplate.coolDownTime = Mathf.Round(origCoolDownTime * 0.85f * 100f) / 100f;
            longerFlames = false;
            blueFire = false;
        }
        else if (PlayerUpgrades.dragonBreathUpgrades.Count > 3)
        {
            weaponTemplate.coolDownTime = Mathf.Round(origCoolDownTime * 0.85f * 100f) / 100f;
            if (PlayerUpgrades.dragonBreathUpgrades[3] == "blue_flames_unlocked")
            {
                if (PlayerUpgrades.dragonBreathUpgrades.Count == 4)
                {
                    longerFlames = true;
                    empoweredWeaponFlare = blueFlames1;
                }
                else if (PlayerUpgrades.dragonBreathUpgrades.Count == 5)
                {
                    empoweredWeaponFlare = blueFlames2;
                    longerFlames = true;
                }
                else
                {
                    empoweredWeaponFlare = blueFlames3;
                    longerFlames = true;
                }
            }
            else
            {
                if (PlayerUpgrades.dragonBreathUpgrades.Count == 4)
                {
                    blueFire = true;
                    empoweredWeaponFlare = longerFlames1;
                }
                else if (PlayerUpgrades.dragonBreathUpgrades.Count == 5)
                {
                    empoweredWeaponFlare = longerFlames2;
                    blueFire = true;
                }
                else
                {
                    empoweredWeaponFlare = longerFlames3;
                    blueFire = true;
                }
            }
        }
        else
        {
            weaponTemplate.coolDownTime = origCoolDownTime;
            longerFlames = false;
            blueFire = false;
        }
    }

    void Start()
    {
        prevNumberUpgrades = PlayerUpgrades.dragonBreathUpgrades.Count;
        weaponScript = this.GetComponent<ShipWeaponTemplate>().shipWeaponEquipped.GetComponent<ShipWeaponScript>();
        weaponTemplate = GetComponent<ShipWeaponTemplate>();
        origCoolDownTime = weaponTemplate.coolDownTime;
        applyUpgrades();
        weaponScript.setTemplate();
    }

    void Update()
    {
        if (prevNumberUpgrades != PlayerUpgrades.dragonBreathUpgrades.Count)
        {
            prevNumberUpgrades = PlayerUpgrades.dragonBreathUpgrades.Count;
            applyUpgrades();
            weaponScript.setTemplate();
        }

        if (blueFire || longerFlames)
        {
            if (weaponScript.musketSmoke != empoweredWeaponFlare)
            {
                weaponScript.musketSmoke = empoweredWeaponFlare;
                if (blueFire)
                {
                    weaponScript.weaponIcon.sprite = longerFlamesIcon;
                }
                else
                {
                    weaponScript.weaponIcon.sprite = blueFireIcon;
                }
            }
        }
    }
}
