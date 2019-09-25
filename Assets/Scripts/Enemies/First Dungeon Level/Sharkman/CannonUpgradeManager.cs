using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonUpgradeManager : MonoBehaviour
{
    ShipWeaponScript weaponScript;
    ShipWeaponTemplate weaponTemplate;
    int prevNumberUpgrades;
    public GameObject regularCannonPlume, momentumBlast1, momentumBlast2, redHotPlume;
    bool momentumBlastUnlocked = false, synchroFiring = false;
    GameObject empoweredWeaponFlare;
    float origCoolDownTime;
    int prevNumShots = 0;
    float cycleReduction = 1;
    GameObject leftWeapon, rightWeapon;
    public Sprite momentumBlastIcon, redHotIcon, regularCannonIcon;

    void applyUpgrades()
    {
        if (PlayerUpgrades.cannonUpgrades.Count == 3)
        {
            weaponTemplate.coolDownTime = Mathf.Round(origCoolDownTime * 0.85f * 100f) / 100f;
            momentumBlastUnlocked = false;
            synchroFiring = false;
            weaponScript.weaponIcon.sprite = regularCannonIcon;
        }
        else if (PlayerUpgrades.cannonUpgrades.Count > 3)
        {
            weaponTemplate.coolDownTime = Mathf.Round(origCoolDownTime * 0.85f * 100f) / 100f;
            if (PlayerUpgrades.cannonUpgrades[3] == "momentum_blast_unlocked")
            {
                if (PlayerUpgrades.cannonUpgrades.Count == 4)
                {
                    empoweredWeaponFlare = momentumBlast1;
                    momentumBlastUnlocked = true;
                }
                else if (PlayerUpgrades.cannonUpgrades.Count == 5)
                {
                    empoweredWeaponFlare = momentumBlast1;
                    momentumBlastUnlocked = true;
                    weaponTemplate.coolDownTime = Mathf.Round(origCoolDownTime * 0.7f * 100f) / 100f;
                }
                else
                {
                    empoweredWeaponFlare = momentumBlast2;
                    momentumBlastUnlocked = true;
                    weaponTemplate.coolDownTime = Mathf.Round(origCoolDownTime * 0.7f * 100f) / 100f;
                }
            }
            else
            {
                if (PlayerUpgrades.cannonUpgrades.Count == 4)
                {
                    synchroFiring = true;
                    cycleReduction = 1f;
                    empoweredWeaponFlare = regularCannonPlume;
                    prevNumShots = weaponScript.numberShots;
                }
                else if (PlayerUpgrades.cannonUpgrades.Count == 5)
                {
                    synchroFiring = true;
                    cycleReduction = 2f;
                    empoweredWeaponFlare = regularCannonPlume;
                    prevNumShots = weaponScript.numberShots;
                }
                else
                {
                    synchroFiring = true;
                    empoweredWeaponFlare = redHotPlume;
                    cycleReduction = 2f;
                    prevNumShots = weaponScript.numberShots;
                }
            }
        }
        else
        {
            momentumBlastUnlocked = false;
            synchroFiring = false;
            weaponTemplate.coolDownTime = origCoolDownTime;
        }
    }

    void Start()
    {
        prevNumberUpgrades = PlayerUpgrades.cannonUpgrades.Count;
        weaponScript = this.GetComponent<ShipWeaponTemplate>().shipWeaponEquipped.GetComponent<ShipWeaponScript>();
        weaponTemplate = GetComponent<ShipWeaponTemplate>();
        leftWeapon = GameObject.Find("PlayerShip").GetComponent<ShipStats>().leftWeapon;
        rightWeapon = GameObject.Find("PlayerShip").GetComponent<ShipStats>().rightWeapon;
        origCoolDownTime = weaponTemplate.coolDownTime;
        applyUpgrades();
        weaponScript.setTemplate();
    }

    void Update()
    {
        if (prevNumberUpgrades != PlayerUpgrades.cannonUpgrades.Count)
        {
            prevNumberUpgrades = PlayerUpgrades.cannonUpgrades.Count;
            applyUpgrades();
            weaponScript.setTemplate();
        }

        if (momentumBlastUnlocked)
        {
            if (weaponScript.whichSide == 2)
            {
                weaponScript.musketSmoke = empoweredWeaponFlare;
                weaponScript.weaponIcon.sprite = momentumBlastIcon;
            }
            else if (weaponScript.whichSide == 3)
            {
                weaponScript.musketSmoke = empoweredWeaponFlare;
                weaponScript.weaponIcon.sprite = momentumBlastIcon;
            }
        }

        if (synchroFiring)
        {
            if (weaponScript.whichSide == 1)
            {
                if (weaponScript.mouseHovering && Input.GetMouseButtonDown(0))
                {
                    if (prevNumShots != weaponScript.numberShots)
                    {
                        weaponScript.numberShots = 0;
                        prevNumShots = 0;
                        if(leftWeapon.GetComponent<ShipWeaponScript>().onCooldown == false && leftWeapon.GetComponent<ShipWeaponScript>().shipWeaponTemplate.name ==
                            "Cannon Weapon Template")
                        {
                            leftWeapon.GetComponent<ShipWeaponScript>().forceFire();
                            leftWeapon.GetComponent<ShipWeaponScript>().coolDownPeriod -= cycleReduction;
                        }

                        if (rightWeapon.GetComponent<ShipWeaponScript>().onCooldown == false && rightWeapon.GetComponent<ShipWeaponScript>().shipWeaponTemplate.name ==
                            "Cannon Weapon Template")
                        {
                            rightWeapon.GetComponent<ShipWeaponScript>().forceFire();
                            rightWeapon.GetComponent<ShipWeaponScript>().coolDownPeriod -= cycleReduction;
                        }
                        weaponScript.coolDownPeriod -= cycleReduction;
                    }
                }
            }

            if(weaponScript.musketSmoke != empoweredWeaponFlare)
            {
                weaponScript.musketSmoke = empoweredWeaponFlare;
                weaponScript.weaponIcon.sprite = redHotIcon;
            }
        }
    }
}
