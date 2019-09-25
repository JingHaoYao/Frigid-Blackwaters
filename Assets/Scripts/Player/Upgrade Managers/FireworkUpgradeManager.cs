using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireworkUpgradeManager : MonoBehaviour {
    ShipWeaponScript weaponScript;
    ShipWeaponTemplate weaponTemplate;
    int prevNumberUpgrades;
    bool stockpileUnlocked = false, largerExplosionsUnlocked = false;
    public GameObject largerExplosionPlume1, largerExplosionPlume2, largerExplosionPlume3;
    int numberStockPile = 3, numberRocketsStockpiled = 0;
    GameObject empoweredWeaponFlare;
    float origCoolDownTime;
    public Sprite largerExplosionsUnlockedIcon;
    public Sprite[] stockPileIcons;
    int numberPrevShots;
    Sprite origFireworkIcon;
    GameObject origFireworkPlume;

    bool isOnlyFirework()
    {
        int whichSide = weaponScript.whichSide;

        if (whichSide == 1)
        {
            if (GameObject.Find("PlayerShip").GetComponent<ShipStats>().leftWeapon.GetComponent<ShipWeaponScript>().shipWeaponTemplate.gameObject.name == "Firework Weapon Template"
               || GameObject.Find("PlayerShip").GetComponent<ShipStats>().rightWeapon.GetComponent<ShipWeaponScript>().shipWeaponTemplate.gameObject.name == "Firework Weapon Template")
            {
                return false;
            }
        }
        else if(whichSide == 2)
        {
            if (GameObject.Find("PlayerShip").GetComponent<ShipStats>().frontWeapon.GetComponent<ShipWeaponScript>().shipWeaponTemplate.gameObject.name == "Firework Weapon Template"
               || GameObject.Find("PlayerShip").GetComponent<ShipStats>().rightWeapon.GetComponent<ShipWeaponScript>().shipWeaponTemplate.gameObject.name == "Firework Weapon Template")
            {
                return false;
            }
        }
        else
        {
            if (GameObject.Find("PlayerShip").GetComponent<ShipStats>().leftWeapon.GetComponent<ShipWeaponScript>().shipWeaponTemplate.gameObject.name == "Firework Weapon Template"
               || GameObject.Find("PlayerShip").GetComponent<ShipStats>().frontWeapon.GetComponent<ShipWeaponScript>().shipWeaponTemplate.gameObject.name == "Firework Weapon Template")
            {
                return false;
            }
        }
        return true;
    }

    void applyUpgrades()
    {
        if (PlayerUpgrades.fireworkUpgrades.Count == 3)
        {
            weaponTemplate.coolDownTime = Mathf.Round(origCoolDownTime * 0.85f * 100f) / 100f;
            largerExplosionsUnlocked = false;
            stockpileUnlocked = false;
            numberStockPile = 0;
            numberRocketsStockpiled = 0;
            weaponScript.noFireNormally = false;
        }
        else if (PlayerUpgrades.fireworkUpgrades.Count > 3)
        {
            weaponTemplate.coolDownTime = Mathf.Round(origCoolDownTime * 0.85f * 100f) / 100f;
            if (PlayerUpgrades.fireworkUpgrades[3] == "large_explosions_unlocked")
            {
                if (PlayerUpgrades.fireworkUpgrades.Count == 4)
                {
                    if (isOnlyFirework())
                    {
                        largerExplosionsUnlocked = true;
                    }
                    empoweredWeaponFlare = largerExplosionPlume1;
                }
                else if (PlayerUpgrades.fireworkUpgrades.Count == 5)
                {
                    empoweredWeaponFlare = largerExplosionPlume2;
                    if (isOnlyFirework())
                    {
                        largerExplosionsUnlocked = true;
                    }
                }
                else
                {
                    empoweredWeaponFlare = largerExplosionPlume3;
                    if (isOnlyFirework())
                    {
                        largerExplosionsUnlocked = true;
                    }
                }
            }
            else
            {
                if (PlayerUpgrades.fireworkUpgrades.Count == 4)
                {
                    if (isOnlyFirework())
                    {
                        stockpileUnlocked = true;
                    }
                    numberStockPile = 3;
                    numberPrevShots = weaponScript.numberShots;
                }
                else if (PlayerUpgrades.fireworkUpgrades.Count == 5)
                {
                    if (isOnlyFirework())
                    {
                        stockpileUnlocked = true;
                    }
                    numberStockPile = 5;
                    numberPrevShots = weaponScript.numberShots;
                }
                else
                {
                    if (isOnlyFirework())
                    {
                        stockpileUnlocked = true;
                    }
                    numberStockPile = 7;
                    numberPrevShots = weaponScript.numberShots;
                }
            }
        }
        else
        {
            weaponTemplate.coolDownTime = origCoolDownTime;
            largerExplosionsUnlocked = false;
            stockpileUnlocked = false;
            numberStockPile = 0;
            numberRocketsStockpiled = 0;
            weaponScript.noFireNormally = false;
        }
    }

    void Start()
    {
        prevNumberUpgrades = PlayerUpgrades.fireworkUpgrades.Count;
        weaponScript = this.GetComponent<ShipWeaponTemplate>().shipWeaponEquipped.GetComponent<ShipWeaponScript>();
        origFireworkIcon = this.GetComponent<ShipWeaponTemplate>().coolDownIcon;
        origFireworkPlume = this.GetComponent<ShipWeaponTemplate>().weaponFlare;
        weaponTemplate = GetComponent<ShipWeaponTemplate>();
        origCoolDownTime = weaponTemplate.coolDownTime;
        applyUpgrades();
        weaponScript.setTemplate();
    }

    void Update()
    {
        if (prevNumberUpgrades != PlayerUpgrades.fireworkUpgrades.Count)
        {
            prevNumberUpgrades = PlayerUpgrades.fireworkUpgrades.Count;
            applyUpgrades();
            weaponScript.setTemplate();
            if(stockpileUnlocked)
                weaponScript.weaponIcon.sprite = stockPileIcons[numberRocketsStockpiled];
        }

        if(isOnlyFirework() == false && (stockpileUnlocked == true || largerExplosionsUnlocked == true))
        {
            largerExplosionsUnlocked = false;
            stockpileUnlocked = false;
            weaponScript.musketSmoke = origFireworkPlume;
            weaponScript.weaponIcon.sprite = origFireworkIcon;
            numberStockPile = 0;
            numberRocketsStockpiled = 0;
            weaponScript.noFireNormally = false;
            applyUpgrades();
        }

        if (largerExplosionsUnlocked)
        {
            if (weaponScript.musketSmoke != empoweredWeaponFlare)
            {
                weaponScript.musketSmoke = empoweredWeaponFlare;
                weaponScript.weaponIcon.sprite = largerExplosionsUnlockedIcon;
            }
        }

        if (stockpileUnlocked)
        {
            weaponScript.noFireNormally = true;

            if (numberRocketsStockpiled < numberStockPile)
            {
                if (weaponScript.coolDownPeriod <= 0)
                {
                    weaponScript.coolDownPeriod = weaponScript.coolDownThreshold;
                    numberRocketsStockpiled++;
                    weaponScript.onCooldown = true;
                    weaponScript.weaponIcon.sprite = stockPileIcons[numberRocketsStockpiled];
                }
            }
            else
            {
                weaponScript.coolDownPeriod = weaponScript.coolDownThreshold;
                weaponScript.onCooldown = false;
            }

            if(weaponScript.mouseHovering == true && Input.GetMouseButtonDown(0))
            {
                if (numberRocketsStockpiled > 0)
                {
                    weaponScript.forceFire();
                    numberRocketsStockpiled--;
                    weaponScript.weaponIcon.sprite = stockPileIcons[numberRocketsStockpiled];
                }
            }

            if (numberPrevShots != weaponScript.numberShots)
            {
                numberPrevShots = weaponScript.numberShots;
            }
        }
    }
}
