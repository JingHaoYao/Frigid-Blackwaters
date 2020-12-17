using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HullUpgradeManager : MonoBehaviour {
    PlayerScript playerScript;
    int prevNumberUpgrades;
    bool dashEnabled = true;
    float dashCooldown = 1;
    public bool spikesEnabled = false;
    public int spikeDamage = 2;
    public GameObject dashIcon;
    public GameObject spikeHitBox, waterFoamBurst;
    float dashMomentum = 8f;
    [SerializeField] GameObject dashEffectClone;
    [SerializeField] GameObject smallDashEffect;
    List<DashCloneEffect> dashClones = new List<DashCloneEffect>();
    [SerializeField] GameObject dashParticles;
    

    private float dashCooldownPeriod = 0;

     // Ship Types and what they mean
     // 0 - base
     // 1  - health upgrade 1
     // 2 - health upgrade 2
     // 3 - defense 1
     // 4 - defense 2
     // 5 - defense 3
     // 6 - speed 1
     // 7 - speed 2
     // 8 - speed 3
     // 9 - spikes 1
     // 10 - spikes 2
     // 11 - spikes 3

    void StartDashEffect()
    {
        foreach(DashCloneEffect effect in dashClones)
        {
            if(effect.gameObject.activeSelf == false)
            {
                effect.Initialize((360 + PlayerProperties.playerScript.whatAngleTraveled) % 360, PlayerProperties.spriteRenderer.sortingOrder);
                return;
            }
        }

        GameObject newInstant = Instantiate(dashEffectClone, PlayerProperties.playerShipPosition, Quaternion.identity);
        DashCloneEffect newEffect = newInstant.GetComponent<DashCloneEffect>();
        dashClones.Add(newEffect);
        newEffect.Initialize((360 + PlayerProperties.playerScript.whatAngleTraveled) % 360, PlayerProperties.spriteRenderer.sortingOrder);
    }

    IEnumerator DashEffect()
    {
        for(int i = 0; i < 4; i++)
        {
            StartDashEffect();
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator scaleBounce(float duration, float mag)
    {
        float increment = mag / duration;
        float timer = duration;
        while(transform.rotation.x > 0)
        {
            timer -= Time.deltaTime;
            transform.rotation = Quaternion.Euler(increment * timer, 0, 0);
            yield return null;
        }

        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    void ShortScaleEffect()
    {
        this.transform.rotation = Quaternion.Euler(50, 0, 0);
        StartCoroutine(scaleBounce(0.4f, 50));
    }

    void applyUpgrades()
    {
        if (PlayerUpgrades.hullUpgrades.Count == 1)
        {
            playerScript.upgradeHealthBonus = 500;
            playerScript.SetAnimationShipType(0);
            playerScript.upgradeSpeedBonus = 0;
            playerScript.upgradeDefenseBonus = 0;
            spikesEnabled = false;
            dashMomentum = 10f;
        }
        else if(PlayerUpgrades.hullUpgrades.Count == 2)
        {
            playerScript.upgradeHealthBonus = 1500;
            playerScript.SetAnimationShipType(1);
            playerScript.upgradeSpeedBonus = 0;
            playerScript.upgradeDefenseBonus = 0;
            spikesEnabled = false;
            dashMomentum = 10f;
        }
        else if(PlayerUpgrades.hullUpgrades.Count == 3)
        {
            playerScript.upgradeHealthBonus = 3500;
            spikesEnabled = false;
            playerScript.upgradeSpeedBonus = 0;
            playerScript.upgradeDefenseBonus = 0;
            playerScript.SetAnimationShipType(2);
            dashMomentum = 10f;
        }
        else if (PlayerUpgrades.hullUpgrades.Count > 3)
        {
            if (PlayerUpgrades.hullUpgrades[3] == "speed_hull_upgrade_1")
            {
                if (PlayerUpgrades.hullUpgrades.Count == 4)
                {
                    playerScript.upgradeHealthBonus = 4500;
                    playerScript.upgradeSpeedBonus = 2;
                    playerScript.SetAnimationShipType(6);
                    dashMomentum = 10f;
                }
                else if (PlayerUpgrades.hullUpgrades.Count == 5)
                {
                    playerScript.upgradeHealthBonus = 4500;
                    playerScript.upgradeSpeedBonus = 2;
                    playerScript.SetAnimationShipType(7);
                    dashMomentum = 13f;
                }
                else
                {
                    playerScript.upgradeSpeedBonus = 4;
                    playerScript.upgradeHealthBonus = 6500;
                    playerScript.SetAnimationShipType(8);
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
                    playerScript.SetAnimationShipType(9);
                    spikeHitBox.GetComponent<DamageAmount>().originDamage = spikeDamage;
                    spikeHitBox.GetComponent<DamageAmount>().updateDamage();
                    dashMomentum = 10f;
                }
                else if (PlayerUpgrades.hullUpgrades.Count == 5)
                {
                    spikesEnabled = true;
                    playerScript.upgradeHealthBonus = 6000;
                    spikeDamage = 4;
                    playerScript.SetAnimationShipType(10);
                    spikeHitBox.GetComponent<DamageAmount>().originDamage = spikeDamage;
                    spikeHitBox.GetComponent<DamageAmount>().updateDamage();
                    dashMomentum = 10f;
                }
                else
                {
                    spikesEnabled = true;
                    playerScript.upgradeHealthBonus = 6000;
                    spikeDamage = 8;
                    playerScript.SetAnimationShipType(11);
                    spikeHitBox.GetComponent<DamageAmount>().originDamage = spikeDamage;
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
                    playerScript.SetAnimationShipType(3);
                    dashMomentum = 10f;
                }
                else if(PlayerUpgrades.hullUpgrades.Count == 5)
                {
                    playerScript.upgradeHealthBonus = 8000;
                    playerScript.upgradeDefenseBonus = 0.1f;
                    playerScript.SetAnimationShipType(4);
                    dashMomentum = 10f;
                }
                else
                {
                    playerScript.upgradeHealthBonus = 12000;
                    playerScript.upgradeDefenseBonus = 0.25f;
                    playerScript.SetAnimationShipType(5);
                    dashMomentum = 10f;
                }
            }
        }
        else
        {
            playerScript.upgradeHealthBonus = 0;
            playerScript.SetAnimationShipType(0);
            playerScript.upgradeSpeedBonus = 0;
            playerScript.upgradeDefenseBonus = 0;
            spikesEnabled = false;
            dashMomentum = 10f;
        }

        playerScript.CheckAndUpdateHealth();
    }

    void Start()
    {
        playerScript = GetComponent<PlayerScript>();
        prevNumberUpgrades = PlayerUpgrades.hullUpgrades.Count;
        applyUpgrades();
    }

    public void SetDashOffCooldown()
    {
        dashCooldownPeriod = 0;
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

            if(dashCooldownPeriod <= 0)
            {
                dashIcon.GetComponent<Image>().color = new Color(1, 1, 1, 1f);
                dashCooldownPeriod = 0;
                if (Input.GetKey((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.dash)) && !playerScript.isShipRooted() && playerScript.playerDead == false && playerScript.windowAlreadyOpen == false)
                {
                    dashCooldownPeriod = dashCooldown;
                    foreach (ArtifactSlot slot in FindObjectOfType<Artifacts>().artifactSlots)
                    {
                        if (slot.displayInfo != null && slot.displayInfo.GetComponent<ArtifactEffect>())
                            slot.displayInfo.GetComponent<ArtifactEffect>().playerDashed();
                    }
                    Vector3 momentumVector = new Vector3(Mathf.Cos(playerScript.angleOrientation * Mathf.Deg2Rad), Mathf.Sin(playerScript.angleOrientation * Mathf.Deg2Rad), 0) * dashMomentum;
                    playerScript.setPlayerMomentum(momentumVector, 1f);
                    float moveAngle = (360 + (Mathf.Atan2(momentumVector.y, momentumVector.x) * Mathf.Rad2Deg)) % 360;
                    Instantiate(waterFoamBurst, this.gameObject.transform.position, Quaternion.Euler(0, 0, moveAngle + 90));
                    StartCoroutine(DashEffect());
                    ShortScaleEffect();
                    Instantiate(smallDashEffect, transform.position, Quaternion.Euler(0, 0, PlayerProperties.playerScript.whatAngleTraveled + 180));
                    Instantiate(dashParticles, transform.position, Quaternion.Euler(0, 0, PlayerProperties.playerScript.whatAngleTraveled - 90));
                    PlayerProperties.playerScript.FlashWhitePickup();
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
