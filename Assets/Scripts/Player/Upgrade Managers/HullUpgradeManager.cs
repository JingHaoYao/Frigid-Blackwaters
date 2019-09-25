using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HullUpgradeManager : MonoBehaviour {
    PlayerScript playerScript;
    int prevNumberUpgrades;
    bool dashEnabled = true;
    float dashCooldown = 2.5f;
    public bool spikesEnabled = false;
    public int spikeDamage = 2;
    public Sprite[] regularShipSprites;
    public Sprite[] shipHealthUpgrade1;
    public Sprite[] shipHealthUpgrade2;
    public Sprite[] shipSpeedHull1;
    public Sprite[] shipSpeedHull2;
    public Sprite[] shipSpeedHull3;
    public Sprite[] shipSpikeHull1;
    public Sprite[] shipSpikeHull2;
    public Sprite[] shipSpikeHull3;
    public Sprite[] shipDefenseHull1;
    public Sprite[] shipDefenseHull2;
    public Sprite[] shipDefenseHull3;
    public GameObject dashIcon;
    public GameObject spikeHitBox, waterFoamBurst;
    float dashMomentum = 8f;

    float dashCooldownPeriod = 0;

    void applySprites(Sprite[] sprites)
    {
        playerScript.downLeft = sprites[0];
        playerScript.left = sprites[1];
        playerScript.down = sprites[2];
        playerScript.up = sprites[3];
        playerScript.upLeft = sprites[4];
    }

    void applyUpgrades()
    {
        if (PlayerUpgrades.hullUpgrades.Count == 1)
        {
            playerScript.upgradeHealthBonus = 500;
            applySprites(regularShipSprites);
            playerScript.upgradeSpeedBonus = 0;
            playerScript.upgradeDefenseBonus = 0;
            spikesEnabled = false;
            dashMomentum = 10f;
            dashCooldown = 2.5f;
        }
        else if(PlayerUpgrades.hullUpgrades.Count == 2)
        {
            playerScript.upgradeHealthBonus = 1500;
            applySprites(shipHealthUpgrade1);
            playerScript.upgradeSpeedBonus = 0;
            playerScript.upgradeDefenseBonus = 0;
            spikesEnabled = false;
            dashMomentum = 10f;
            dashCooldown = 2.5f;
        }
        else if(PlayerUpgrades.hullUpgrades.Count == 3)
        {
            playerScript.upgradeHealthBonus = 3500;
            spikesEnabled = false;
            playerScript.upgradeSpeedBonus = 0;
            playerScript.upgradeDefenseBonus = 0;
            applySprites(shipHealthUpgrade2);
            dashCooldown = 2.5f;
            dashMomentum = 10f;
        }
        else if (PlayerUpgrades.hullUpgrades.Count > 3)
        {
            if (PlayerUpgrades.hullUpgrades[3] == "speed_hull_upgrade_1")
            {
                if (PlayerUpgrades.hullUpgrades.Count == 4)
                {
                    playerScript.upgradeHealthBonus = 4500;
                    playerScript.upgradeSpeedBonus = 3;
                    applySprites(shipSpeedHull1);
                    dashCooldown = 2.5f;
                    dashMomentum = 10f;
                }
                else if (PlayerUpgrades.hullUpgrades.Count == 5)
                {
                    playerScript.upgradeHealthBonus = 4500;
                    playerScript.upgradeSpeedBonus = 3;
                    applySprites(shipSpeedHull2);
                    dashCooldown = 2.5f;
                    dashMomentum = 13f;
                }
                else
                {
                    playerScript.upgradeSpeedBonus = 3;
                    playerScript.upgradeHealthBonus = 6500;
                    applySprites(shipSpeedHull3);
                    dashCooldown = 1.5f;
                    dashMomentum = 13f;
                }
            }
            else if(PlayerUpgrades.hullUpgrades[3] == "spikes_hull_upgrade_1")
            {
                if (PlayerUpgrades.hullUpgrades.Count == 4)
                {
                    spikesEnabled = true;
                    playerScript.upgradeHealthBonus = 4000;
                    spikeDamage = 2;
                    applySprites(shipSpikeHull1);
                    spikeHitBox.GetComponent<DamageAmount>().originDamage = spikeDamage;
                    spikeHitBox.GetComponent<DamageAmount>().updateDamage();
                    dashCooldown = 2.5f;
                    dashMomentum = 10f;
                }
                else if (PlayerUpgrades.hullUpgrades.Count == 5)
                {
                    spikesEnabled = true;
                    playerScript.upgradeHealthBonus = 6000;
                    spikeDamage = 4;
                    applySprites(shipSpikeHull2);
                    spikeHitBox.GetComponent<DamageAmount>().originDamage = spikeDamage;
                    spikeHitBox.GetComponent<DamageAmount>().updateDamage();
                    dashCooldown = 2.5f;
                    dashMomentum = 10f;
                }
                else
                {
                    spikesEnabled = true;
                    playerScript.upgradeHealthBonus = 6000;
                    spikeDamage = 8;
                    applySprites(shipSpikeHull3);
                    spikeHitBox.GetComponent<DamageAmount>().originDamage = spikeDamage;
                    dashCooldown = 2.5f;
                    dashMomentum = 10f;
                    spikeHitBox.GetComponent<DamageAmount>().updateDamage();
                }
            }
            else
            {
                if(PlayerUpgrades.hullUpgrades.Count == 4)
                {
                    playerScript.upgradeHealthBonus = 5000;
                    playerScript.upgradeDefenseBonus = 0f;
                    applySprites(shipDefenseHull1);
                    dashCooldown = 2.5f;
                    dashMomentum = 10f;
                }
                else if(PlayerUpgrades.hullUpgrades.Count == 5)
                {
                    playerScript.upgradeHealthBonus = 8000;
                    playerScript.upgradeDefenseBonus = 0.1f;
                    applySprites(shipDefenseHull2);
                    dashCooldown = 2.5f;
                    dashMomentum = 10f;
                }
                else
                {
                    playerScript.upgradeHealthBonus = 12000;
                    playerScript.upgradeDefenseBonus = 0.25f;
                    applySprites(shipDefenseHull3);
                    dashCooldown = 2.5f;
                    dashMomentum = 10f;
                }
            }
        }
        else
        {
            applySprites(regularShipSprites);
        }
    }

    void Start()
    {
        playerScript = GetComponent<PlayerScript>();
        prevNumberUpgrades = PlayerUpgrades.hullUpgrades.Count;
        applyUpgrades();
    }

    void Update()
    {
        if (prevNumberUpgrades != PlayerUpgrades.hullUpgrades.Count)
        {
            prevNumberUpgrades = PlayerUpgrades.hullUpgrades.Count;
            applyUpgrades();
        }

        if(dashEnabled == true)
        {
            dashIcon.SetActive(true);
            float threshold;
            if (playerScript.enemiesDefeated == true)
            {
                threshold = dashCooldown - 1;
            }
            else
            {
                threshold = 0;
            }

            if(dashCooldownPeriod <= threshold)
            {
                dashIcon.GetComponent<Image>().color = new Color(1, 1, 1, 1f);
                dashCooldownPeriod = 0;
                if (Input.GetKey((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.dash)) && playerScript.shipRooted == false && playerScript.playerDead == false)
                {
                    dashCooldownPeriod = dashCooldown;
                    Vector3 momentumVector = new Vector3(Mathf.Cos(playerScript.angleOrientation * Mathf.Deg2Rad), Mathf.Sin(playerScript.angleOrientation * Mathf.Deg2Rad), 0) * dashMomentum;
                    playerScript.momentumVector = momentumVector;
                    playerScript.momentumMagnitude = dashMomentum;
                    playerScript.momentumDuration = 1f;
                    float moveAngle = (360 + (Mathf.Atan2(momentumVector.y, momentumVector.x) * Mathf.Rad2Deg)) % 360;
                    Instantiate(waterFoamBurst, this.gameObject.transform.position, Quaternion.Euler(0, 0, moveAngle + 90));
                }
            }
            else
            {
                dashIcon.GetComponent<Image>().color = new Color(1, 1, 1, 0.63f);
                dashCooldownPeriod -= Time.deltaTime;
            }
        }
        else
        {
            dashIcon.SetActive(false);
        }
    }
}
